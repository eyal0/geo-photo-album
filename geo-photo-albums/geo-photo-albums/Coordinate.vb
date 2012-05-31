Imports System.Math

Structure Coordinate
    Implements IEquatable(Of Coordinate)

    ''' <summary>
    ''' New coordinate from Latitude and Longitude in Radians
    ''' </summary>
    ''' <param name="LatitudeInRadians">Latitude in Radians</param>
    ''' <param name="LongitudeInRadians">Longitude in Radians</param>
    ''' <remarks>Hidden so that radians and degrees won't get confused</remarks>
    Private Sub New(latitudeInRadians As Double, longitudeInRadians As Double)
        Me.LatitudeInRadians = latitudeInRadians
        Me.LongitudeInRadians = longitudeInRadians
    End Sub

    ''' <summary>
    ''' Coordinate from Radians
    ''' </summary>
    ''' <param name="latitudeInRadians">Latitude in Radians</param>
    ''' <param name="longitudeInRadians">Longitude in Radians</param>
    ''' <returns>New Coordinate</returns>
    ''' <remarks></remarks>
    Public Shared Function FromRadians(latitudeInRadians As Double, longitudeInRadians As Double) As Coordinate
        Return New Coordinate(latitudeInRadians, longitudeInRadians)
    End Function

    ''' <summary>
    ''' Coordinate from Degrees
    ''' </summary>
    ''' <param name="latitudeInDegrees">Latitude in Degrees</param>
    ''' <param name="longitudeInDegrees">Longitude in Degrees</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FromDegrees(latitudeInDegrees As Double, longitudeInDegrees As Double) As Coordinate
        Return New Coordinate(DegreesToRadians(latitudeInDegrees), DegreesToRadians(longitudeInDegrees))
    End Function
    ''' <summary>
    ''' Convert degrees to radians
    ''' </summary>
    ''' <param name="degrees">input in degrees</param>
    ''' <returns>output in radians</returns>
    ''' <remarks></remarks>
    Private Shared Function DegreesToRadians(degrees As Double) As Double
        Return degrees * Math.PI / 180
    End Function

    ''' <summary>
    ''' Convert radians to degrees
    ''' </summary>
    ''' <param name="radians">input in radians</param> 
    ''' <returns>output in degrees</returns>
    ''' <remarks></remarks>
    Private Shared Function RadiansToDegrees(radians As Double) As Double
        Return radians * 180 / Math.PI
    End Function

    ''' <summary>
    ''' Latitude in Radians
    ''' </summary>
    ''' <value>Latitude in Radians</value>
    ''' <returns>Latitude in Radians</returns>
    ''' <remarks></remarks>
    Public Property LatitudeInRadians As Double

    ''' <summary>
    ''' Longitude in Radians
    ''' </summary>
    ''' <value>Longitude in Radians</value>
    ''' <returns>Longitude in Radians</returns>
    ''' <remarks></remarks>
    Public Property LongitudeInRadians As Double

    ''' <summary>
    ''' Latitude in Degrees
    ''' </summary>
    ''' <value>Latitude in Degrees</value>
    ''' <returns>Latitude in Degrees</returns>
    ''' <remarks></remarks>
    Public Property LatitudeInDegrees() As Double
        Get
            Return RadiansToDegrees(LatitudeInRadians)
        End Get
        Set(ByVal value As Double)
            LatitudeInRadians = DegreesToRadians(value)
        End Set
    End Property

    ''' <summary>
    ''' Longitude in Degrees
    ''' </summary>
    ''' <value>Longitude in Degrees</value>
    ''' <returns>Longitude in Degrees</returns>
    ''' <remarks></remarks>
    Public Property LongitudeInDegrees() As Double
        Get
            Return RadiansToDegrees(LongitudeInRadians)
        End Get
        Set(ByVal value As Double)
            LongitudeInRadians = DegreesToRadians(value)
        End Set
    End Property

    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is Coordinate AndAlso Equals1(DirectCast(obj, Coordinate))
    End Function

    Public Function Equals1(other As Coordinate) As Boolean Implements System.IEquatable(Of Coordinate).Equals
        Return (LatitudeInRadians = other.LatitudeInRadians AndAlso
                LongitudeInRadians = other.LongitudeInRadians)
    End Function

    Public Shared Operator =(first As Coordinate, other As Coordinate) As Boolean
        Return first.Equals1(other)
    End Operator

    Public Shared Operator <>(first As Coordinate, other As Coordinate) As Boolean
        Return Not first.Equals1(other)
    End Operator

    Public Overrides Function GetHashCode() As Integer
        Return LatitudeInRadians.GetHashCode Xor LongitudeInRadians.GetHashCode
    End Function
    ''' <summary>
    ''' Compute the distance along a great circle between two coordinates on Earth.
    ''' </summary>
    ''' <param name="other">Other coordinate</param>
    ''' <returns>Distance in meters</returns>
    ''' <remarks></remarks>
    Public Function Distance(other As Coordinate) As Double
        If Me.Equals(other) Then
            Return 0
        End If
        Dim a As Double = 6378137
        Dim b As Double = 6356752.3142
        Dim f As Double = 1 / 298.257223563
        Dim l As Double = other.LongitudeInRadians - Me.LongitudeInRadians
        Dim u1 As Double = Atan((1 - f) * Tan(Me.LatitudeInRadians))
        Dim u2 As Double = Atan((1 - f) * Tan(other.LatitudeInRadians))
        Dim sin_u1 As Double = Sin(u1)
        Dim cos_u1 As Double = Cos(u1)
        Dim sin_u2 As Double = Sin(u2)
        Dim cos_u2 As Double = Cos(u2)
        Dim lambda As Double = 1
        Dim lambda_pi As Double = 2 * PI
        Dim iter_limit As UInteger = 20

        Dim cos_sq_alpha As Double = 0
        Dim sin_sigma As Double = 0
        Dim cos2sigma_m As Double = 0
        Dim cos_sigma As Double = 0
        Dim sigma As Double = 0

        Do While Abs(lambda - lambda_pi) > 0.000000000001 AndAlso --iter_limit > 0
            Dim sin_lambda As Double = Sin(lambda)
            Dim cos_lambda As Double = Cos(lambda)
            sin_sigma = Sqrt((cos_u2 * sin_lambda) * (cos_u2 * sin_lambda) +
                             (cos_u1 * sin_u2 - sin_u1 * cos_u2 * cos_lambda) *
                             (cos_u1 * sin_u2 - sin_u1 * cos_u2 * cos_lambda))
            cos_sigma = sin_u1 * sin_u2 + cos_u1 * cos_u2 * cos_lambda
            sigma = Atan2(sin_sigma, cos_sigma)
            Dim alpha As Double = Asin(cos_u1 * cos_u2 * sin_lambda / sin_sigma)
            cos_sq_alpha = Cos(alpha) * Cos(alpha)
            cos2sigma_m = cos_sigma - 2 * sin_u1 * sin_u2 / cos_sq_alpha
            Dim cc As Double = f / 16 * cos_sq_alpha * (4 + f * (4 - 3 * cos_sq_alpha))
            lambda_pi = lambda
            lambda = (l + (1 - cc) * f * Sin(alpha) *
                      (sigma + cc * sin_sigma * (cos2sigma_m + cc * cos_sigma * (-1 + 2 * cos2sigma_m * cos2sigma_m))))
        Loop
        Dim usq As Double = cos_sq_alpha * (a * a - b * b) / (b * b)
        Dim aa As Double = 1 + usq / 16384 * (4096 + usq * (-768 + usq * (320 - 175 * usq)))
        Dim bb As Double = usq / 1024 * (256 + usq * (-128 + usq * (74 - 47 * usq)))
        Dim delta_sigma As Double = bb * sin_sigma * (cos2sigma_m + bb / 4 * (cos_sigma * (-1 + 2 * cos2sigma_m * cos2sigma_m) -
                                                      bb / 6 * cos2sigma_m * (-3 + 4 * sin_sigma * sin_sigma) * (-3 + 4 * cos2sigma_m * cos2sigma_m)))
        Dim c As Double = b * aa * (sigma - delta_sigma)
        Return c
    End Function

    ''' <summary>
    ''' Convert coordinate and zoom level to x,y on Google Map
    ''' </summary>
    ''' <param name="zoom">Google Maps zoom level, 0 is farthest out</param>
    ''' <returns>coordinate on Google Map</returns>
    ''' <remarks>
    ''' from view-source:https://google-developers.appspot.com/maps/documentation/javascript/examples/map-coordinates
    ''' </remarks>
    Function ZoomToPoint(zoom As UInteger) As PointD
        Dim x As Double = 128 + LongitudeInDegrees * 256 / 360
        Dim siny As Double = Sin(LatitudeInRadians)
        If siny < -0.9999 Then siny = -0.9999
        If siny > 0.9999 Then siny = 0.9999
        Dim y As Double = 128 + 0.5 * Log((1 + siny) / (1 - siny)) * -(256 / (2 * PI))
        Return New PointD(x * (1 << CInt(zoom)), y * (1 << CInt(zoom)))
    End Function

    ''' <summary>
    ''' Return the coordinate that is fraction 'fraction' of the distance between Me and other
    ''' </summary>
    ''' <param name="other">coordinate</param>
    ''' <param name="fraction">fraction between 0 and 1, inclusive</param>
    ''' <returns>Coordinate that if fraction f of the distance to other</returns>
    ''' <remarks>from http://williams.best.vwh.net/avform.htm#Intermediate</remarks>
    Function IntermediatePoint(other As Coordinate, fraction As Double) As Coordinate
        Dim d As Double = Me.Distance(other)
        Dim A As Double = Sin((1 - fraction) * d) / Sin(d)
        Dim B As Double = Sin(fraction * d) / Sin(d)
        Dim x As Double = (A * Cos(Me.LatitudeInRadians) * Cos(Me.LongitudeInRadians) +
                           B * Cos(other.LatitudeInRadians) * Cos(other.LongitudeInRadians))
        Dim y As Double = (A * Cos(Me.LatitudeInRadians) * Sin(Me.LongitudeInRadians) +
                           B * Cos(other.LatitudeInRadians) * Sin(other.LongitudeInRadians))
        Dim z As Double = A * Sin(Me.LatitudeInRadians) + B * Sin(other.LatitudeInRadians)
        Dim lat As Double = Atan2(z, Sqrt(x * x + y * y))
        Dim lon As Double = Atan2(y, x)
        Dim ret As Coordinate = Coordinate.FromRadians(lat, lon)
        Return ret
    End Function

    Public Overrides Function ToString() As String
        Return LatitudeInDegrees.ToString + "," + LongitudeInDegrees.ToString
    End Function
End Structure