Public Class Shootout(Of TItem As IEquatable(Of TItem))
    Private ItemGraph As New Dictionary(Of TItem, HashSet(Of TItem))
    Public Sub Add(item As TItem)
        ItemGraph.Add(item, New HashSet(Of TItem))
    End Sub

    Public Sub Add(ParamArray items() As TItem)
        For Each item As TItem In items
            ItemGraph.Add(item, New HashSet(Of TItem))
        Next
    End Sub

    Public Sub SetLessThanOrEqual(item1 As TItem, item2 As TItem)
        If Not ItemGraph.ContainsKey(item1) OrElse Not ItemGraph.ContainsKey(item2) Then
            Throw New ApplicationException("Can't set rank of unknown item")
        End If
        If item1.Equals(item2) Then
            Throw New ApplicationException("Can't set item less than self")
        End If
        ItemGraph(item1).Add(item2)
    End Sub

    Public Sub SetEqual(item1 As TItem, item2 As TItem)
        If Not ItemGraph.ContainsKey(item1) OrElse Not ItemGraph.ContainsKey(item2) Then
            Throw New ApplicationException("Can't set rank of unknown item")
        End If
        If Not item1.Equals(item2) Then
            ItemGraph(item1).Add(item2)
            ItemGraph(item2).Add(item1)
        End If
    End Sub

    Public Function LessThanOrEqual(item1 As TItem, item2 As TItem) As Boolean
        If item1.Equals(item2) Then
            Return True
        End If
        Dim Visited As New List(Of TItem)({item1})
        Dim i As Integer = 0
        Do While i < Visited.Count
            If ItemGraph(Visited(i)).Contains(item2) Then
                Return True
            End If
            Visited.AddRange(ItemGraph(Visited(i)).Except(Visited))
            i += 1
        Loop
        Return False
    End Function

    Public Function GreaterThanOrEqual(item1 As TItem, item2 As TItem) As Boolean
        Return LessThanOrEqual(item2, item1)
    End Function

    Public Function Compare(item1 As TItem, item2 As TItem) As Integer
        Dim lte As Boolean = LessThanOrEqual(item1, item2)
        Dim gte As Boolean = GreaterThanOrEqual(item1, item2)
        If lte And gte Then
            Return 0
        ElseIf lte Then
            Return -1
        ElseIf gte Then
            Return 1
        Else
            Throw New ApplicationException("Comparison Unknown")
        End If
    End Function

    Public Sub Trim()
        For Each item As TItem In ItemGraph.Keys
            For Each neighbor As TItem In ItemGraph(item).Except({item})
                'can we reach neighbor from item without the direct connection?
                Dim Visited As New List(Of TItem)({item})
                Dim i As Integer = 0
                Do While i < Visited.Count
                    For Each CurrentNeighbor As TItem In ItemGraph(Visited(i))
                        If Not Visited.Contains(CurrentNeighbor) AndAlso Not (Visited(i).Equals(item) AndAlso CurrentNeighbor.Equals(neighbor)) Then
                            Visited.Add(CurrentNeighbor)
                        End If
                    Next
                    If Visited.Contains(neighbor) Then
                        'can trim
                        ItemGraph(item).Remove(neighbor)
                        Exit For
                    End If
                    i += 1
                Loop
            Next
        Next
    End Sub
End Class
