'''
Created on May 19, 2012

@author: Eyal
'''

import datetime, geomath, os, itertools, collections
import csv
import heapq

class GpsSample(datetime.datetime):
    '''
    A single GPS reading
    '''
    
    def __new__(cls, year, month=None, day=None, hour=0, minute=0, second=0,
                microsecond=0, tzinfo=None, latitude=None, longitude=None):
        self=super().__new__(cls, year, month, day, hour, minute, second, microsecond, tzinfo)
        self._latitude = latitude
        self._longitude = longitude
        return self
    
    @property
    def latitude(self, x=None):
        """latitude (-90 - 90)"""
        if(x != None):
            self._latitude = x
        return self._latitude
    
    @property
    def longitude(self, x=None):
        """longitude (-180 - 180)"""
        if(x != None):
            self._longitude = x
        return self._longitude
            
    @classmethod
    def from_dict(cls, d):
        year, month, day = d["DATE"].split("/")
        hour, minute, seconds = d["TIME"].split(":")
        year, month, day, hour, minute = map(int, [year, month, day, hour, minute])
        second, microsecond = map(int, list(divmod(float(seconds)*1000000,1000000)))
        return cls.__new__(cls, year, month, day, hour, minute, second, microsecond, datetime.timezone.utc, float(d["LATITUDE"]), float(d["LONGITUDE"]))
    
    def __repr__(self):
        """Convert to formal string, for repr()."""
        L = [self.year, self.month, self.day, # These are never zero
             self.hour, self.minute, self.second, self.microsecond]
        if L[-1] == 0:
            del L[-1]
        if L[-1] == 0:
            del L[-1]
        s = ", ".join(map(str, L))
        s = "%s(%s)" % ('gps.' + self.__class__.__name__, s)
        if self.tzinfo is not None:
            assert s[-1:] == ")"
            s = s[:-1] + ", tzinfo=%r" % self.tzinfo + ")"
        if self._latitude is not None:
            assert s[-1:] == ")"
            s = s[:-1] + ", latitude=%r" % self._latitude + ")"
        if self._longitude is not None:
            assert s[-1:] == ")"
            s = s[:-1] + ", longitude=%r" % self._longitude + ")"
        return s
    
    def __str__(self):
        return(super().__str__() + " " + str(self._latitude) + ", " + str(self._longitude))
        
def _last(it):
    current = None
    for x in it:
        current = x
    return(current)

def _map_last(middle, last, it):
    current = None
    for x in it:
        if(current != None):
            yield(middle(current))
        if isinstance(x, collections.Iterable):
            current = list(x)
        else:
            current = x
    yield(last(current))

def consume(iterator, n):
    "Advance the iterator n-steps ahead. If n is none, consume entirely."
    # Use functions that consume iterators at C speed.
    if n is None:
        # feed the entire iterator into a zero-length deque
        collections.deque(iterator, maxlen=0)
    else:
        # advance to the empty slice starting at position n
        next(itertools.islice(iterator, n, n), None)

def gpsPoints_to_json(gpsPoints):
    yield(                      '{')
    yield(                      '  "path" : [')
    for x in _map_last(lambda a: '    [%f,%f],' % (a.latitude, a.longitude),
                       lambda a: '    [%f,%f]'  % (a.latitude, a.longitude),
                       iter(gpsPoints)):
        yield(x)
    yield(                      '  ]')
    yield(                      '}')

if(__name__ == '__main__'):
    all_csv = []
    for filename in os.listdir('C:\\Users\\Eyal\\Documents\\geo-photo-album\\gps tracks\\sorted_csv'):
        print(filename)
        all_csv.append(csv.Csv.read_file_rows(os.path.join('C:\\Users\\Eyal\\Documents\\geo-photo-album\\gps tracks\\sorted_csv',filename)))
    all_gpsPoints = heapq.merge(*map(lambda x: map(GpsSample.from_dict,x),all_csv))
    
    zoom_level = 0
    
    grouped_gps_points = map(lambda x: x[1], 
                             itertools.groupby(all_gpsPoints,
                                               lambda x: geomath.lat_lon_zoom_to_point(x.latitude,x.longitude,zoom_level)))
    
    filtered_gpsPoints = itertools.chain.from_iterable(_map_last(lambda x: [x[0]],
                                                                 lambda x: [x[0],x[-1]],
                                                                 grouped_gps_points))
    
    zoom_file = open("C:\\Users\\Eyal\\Documents\\geo-photo-album\\gps tracks\\zoom%s.json" % str(zoom_level), mode="w")
    for x in gpsPoints_to_json(filtered_gpsPoints):
        print(x, file=zoom_file)
    zoom_file.close()
    
    
    #csv.write_file('C:\\Users\\Eyal\\Documents\\geo-photo-album\\gps tracks\\sorted_csv\\BT747log_20111018_199_FUJI_2.csv')