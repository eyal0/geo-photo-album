'''
Created on May 20, 2012

@author: Eyal
'''

import subprocess, re, gps, itertools, os, csv, heapq, datetime, geomath

#exif.communicate(input="\n".join([r"C:\Users\Eyal\Pictures\Thailand 2011\DSCF0008 (2).JPG",
#                                  r"C:\Users\Eyal\Pictures\Thailand 2011\DSCF0009 (2).JPG"]))

#for x in exif.communicate()[0].splitlines():
#    print(x.decode("utf-8"))
#exit()

#http://williams.best.vwh.net/avform.htm#Intermediate

class TaggedFile(object):
    """File with gps info"""
    def __init__(self, filename, gps):
        self._gps = gps
        self._filename = filename
    
    @classmethod
    def from_strings(cls, file, date, latlon, tz):
            year, month, day, hour, minute, second = tuple(date.split(":"))
            second, microsecond = divmod(float(second)*1000000,1000000)
            second = int(second)
            microsecond = int(microsecond)
            year, month, day, hour, minute = tuple(map(int,[year,month,day,hour,minute]))
            latitude = longitude = None
            if(latlon):
                latitude, longitude = tuple(latlon.split(", "))
                latitude = float(latitude)
                longitude = float(longitude)
            return(TaggedFile(file,
                              gps.GpsSample(year, month, day,
                                            hour, minute, second, microsecond,
                                            tz, latitude, longitude)))
    
    @property
    def gps(self, x=None):
        if(x != None):
            self._gps = x
        return(self._gps)
    
    def __repr__(self):
        return("exif.%s(%s, %s)" % (self.__class__.__name__, repr(self._filename), repr(self._gps)))

def iter_by_n(it, n):
    l = []
    while(True):
        try:
            l.append(next(it))
        except StopIteration:
            if(l):
                yield(l)
                return
        if(len(l) >= n):
            yield(l)
            l = []

def load_file_tags(filenames, tz):
    """input an iterator of files, yield many TaggedFile"""
    ExifTool_path = ".\\exiftool.exe"
    for some_filenames in iter_by_n(filenames, 100):
        exif = subprocess.Popen(itertools.chain([ExifTool_path, r"-FileModifyDate", r"-CreateDate", r"-FileName",
                                                 r'-d', r'%Y:%m:%d:%H:%M:%S', r"-s",
                                                 r'-GPSPosition', r'-c', r'%+.10f'], some_filenames),
                                stdout=subprocess.PIPE)
        current_file = current_datetime = current_gps = None
        for x in exif.communicate()[0].splitlines():
            x = x.decode("utf-8")
            m = re.match("FileName\s*: (.*)", x)
            if(m):
                current_file = m.group(1)
                yield(TaggedFile.from_strings(current_file, current_datetime, current_gps, tz))
                current_file = current_datetime = current_gps = None      
            m = re.match("FileModifyDate\s*: ([0-9:.]+)", x)
            if(m):
                current_datetime = m.group(1)
            m = re.match("CreateDate\s*: ([0-9:.]+)", x)
            if(m):
                current_datetime = m.group(1)
            m = re.match("GPSPosition\s*: ([0-9+.]+, [0-9+.]+)", x)
            if(m):
                current_gps = m.group(1)
        if(current_file):
            yield(TaggedFile.from_strings(current_file, current_datetime, current_gps, tz))

def recurse_all_files(name):
    if(os.path.isdir(name)):
        for name2 in map(lambda x: os.path.join(name, x), os.listdir(name)):
            for y in recurse_all_files(name2):
                yield(y)
    else:
        yield(name)

if(__name__ == '__main__'):
    Bangkok_time = datetime.timezone(datetime.timedelta(hours=+7), "Bangkok time")
    dirname = r"c:\Users\Eyal\Pictures\Thailand 2011\Bangkok"
    sorted_csv_dir = r'C:\Users\Eyal\Documents\coding\geo-photo-album\sorted_csv'
    file_tags = load_file_tags(recurse_all_files(dirname), Bangkok_time)
    all_csv = []
    for filename in os.listdir(sorted_csv_dir):
        all_csv.append(csv.Csv.read_file_rows(os.path.join(sorted_csv_dir,filename)))
    all_gpsPoints = heapq.merge(*map(lambda x: map(gps.GpsSample.from_dict, x),all_csv))
    
    tagged_files = load_file_tags(iter([r'c:\users\eyal\Pictures\Thailand 2011\Bangkok\DSCF9015.JPG']), Bangkok_time)
    tagged_files = list(tagged_files)
    for x in tagged_files:
        x.before = x.after = None
    for g in all_gpsPoints:
        for t in tagged_files:
            if(g <= t.gps and (t.before == None or g > t.before)):
                t.before = g
            if(g >= t.gps and (t.after == None or g < t.after)):
                t.after = g
                fraction = (t.gps - t.before) / (t.after - t.before)
                print("fraction is %f" % fraction)
                print("%.10f,%.10f" % geomath.intermediate_point(t.before.latitude, t.before.longitude, t.after.latitude, t.after.longitude, fraction))
    for x in tagged_files:
        print(x)