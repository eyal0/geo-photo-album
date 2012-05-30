Structure GpsSample
    Private coordinate_ As Coordinate
    Private datetime_ As DateTimeOffset

    Sub New(dt As DateTimeOffset, c As Coordinate)
        Dim Bangkok As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Bangkok")
        datetime_ = dt
        coordinate_ = c
    End Sub

    Shared Function FromDict(d As IDictionary(Of String, String)) As GpsSample
        Dim datetime_ As DateTimeOffset
        Dim coordinate_ As Coordinate
        datetime_ = DateTimeOffset.ParseExact(d("DATE") + " " + d("TIME") + "+0", "yyyy/MM/dd HH:mm:ss.ffffffz", System.Globalization.DateTimeFormatInfo.InvariantInfo)
        coordinate_ = Coordinate.FromDegrees(Double.Parse(d("LATIITUDE")), Double.Parse(d("LONGITUDE")))
        Return New GpsSample(datetime_, coordinate_)
    End Function
End Structure
