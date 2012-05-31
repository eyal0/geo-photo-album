Structure GpsSample
    Implements IComparable(Of GpsSample)

    Public Coordinate As Coordinate
    Public Datetime As DateTimeOffset

    Sub New(dt As DateTimeOffset, c As Coordinate)
        Datetime = dt
        Coordinate = c
    End Sub

    Shared Function FromDict(d As IDictionary(Of String, String)) As GpsSample
        Dim datetime_ As DateTimeOffset
        Dim coordinate_ As Coordinate
        datetime_ = DateTimeOffset.ParseExact(d("DATE") + " " + d("TIME") + "+0", "yyyy/MM/dd HH:mm:ss.fffz", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        coordinate_ = Coordinate.FromDegrees(Double.Parse(d("LATITUDE")), Double.Parse(d("LONGITUDE")))
        Return New GpsSample(datetime_, coordinate_)
    End Function

    Public Function CompareTo(other As GpsSample) As Integer Implements System.IComparable(Of GpsSample).CompareTo
        Return Me.Datetime.CompareTo(other.Datetime)
    End Function
End Structure
