Structure PointD
    Implements IEquatable(Of PointD)

    Sub New(x As Double, y As Double)
        Me.X = x
        Me.Y = y
    End Sub

    Public Property X As Double
    Public Property Y As Double

    Public Function Equals1(other As PointD) As Boolean Implements System.IEquatable(Of PointD).Equals
        Return Me.X = other.X And Me.Y = other.Y
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is PointD AndAlso Equals1(DirectCast(obj, PointD))
    End Function

    Shared Operator =(p1 As PointD, p2 As PointD) As Boolean
        Return p1.Equals1(p2)
    End Operator

    Shared Operator <>(p1 As PointD, p2 As PointD) As Boolean
        Return Not p1.Equals1(p2)
    End Operator

    Public Overrides Function GetHashCode() As Integer
        Return X.GetHashCode Xor Y.GetHashCode
    End Function
End Structure
