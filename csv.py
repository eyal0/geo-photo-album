import sys, os, gps, datetime

class Csv:
    """For reading, writing, and merging csv files"""
    def __init__(self, *filenames):
        self.fields = []
        self.rows = []
        if(filenames):
            for f in filenames:
                self.read_file(f)
    
    @classmethod  
    def read_file_rows(cls, filename):
        """A generator that gives the rows in a csv file, discarding them as they return"""
        f = open(filename, 'r')
        new_line = f.readline().rstrip()
        new_fields = new_line.split(",")
        assert(len(new_fields) == len(set(new_fields))) #we can't handle duplicates
        #got the fields sorted out, now add rows
        for new_line in f:
            values = new_line.rstrip().split(",")
            assert(len(new_fields) == len(values))
            yield(dict([(new_fields[x], values[x]) for x in range(len(new_fields))]))
        f.close()
    
    def read_file(self, filename):
        """Read filename into the class"""
        f = open(filename, 'r')
        new_line = f.readline().rstrip()
        new_fields = new_line.split(",")
        assert(len(new_fields) == len(set(new_fields))) #we can't handle duplicates
        if set(self.fields).issuperset(set(new_fields)):
            pass
        elif set(new_fields).issuperset(set(self.fields)):
            self.fields = new_fields
        else:
            self.fields.append(set(self.fields).difference(set(new_fields)))
        #got the fields sorted out, now add rows
        for new_line in f:
            values = new_line.rstrip().split(",")
            assert(len(new_fields) == len(values))
            self.rows.append(dict([(new_fields[x], values[x]) for x in range(len(new_fields))]))
        f.close()

    def _write_file(self, file):
        """Write the csv"""
        print(",".join(self.fields),file=file)
        for row in self.rows:
            print(",".join([row[field] for field in self.fields]), file=file)

    def display(self): 
        """print csv to the screen"""
        self._write_file(sys.stdout)

    def write_file(self, filename):
        """"write csv to file"""
        f = open(filename, mode='w')
        self._write_file(f)
        f.close()

if(__name__=='__main__'):
    start_time = datetime.datetime.now()
    csv_source = r'C:\Users\Eyal\Documents\coding\geo-photo-album-bak\csv'
    csv_destination = r'C:\Users\Eyal\Documents\coding\geo-photo-album-bak\sorted_csv'
    for filename in os.listdir(csv_source):
        print(filename)
        csv = Csv(os.path.join(csv_source, filename))
        csv.rows.sort(key=lambda x: gps.GpsSample.from_dict(x))
        csv.write_file(os.path.join(csv_destination, filename))
    print(datetime.datetime.now() - start_time)