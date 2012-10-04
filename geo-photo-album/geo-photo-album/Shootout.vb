Public Class Shootout(Of TItem As IEquatable(Of TItem))
    Private GreaterItems As New Dictionary(Of Node, HashSet(Of TItem))
    Private EqualGroups As New Dictionary(Of TItem, Node)

    Class UnknownCompareException
        Inherits Exception

        Sub New()
            MyBase.New("Unknown comparison result")
        End Sub
    End Class

    Class AlreadySetException
        Inherits Exception

        Sub New()
            MyBase.New("Can't re-define relation between items")
        End Sub
    End Class

    Private Class Node
        Inherits HashSet(Of TItem)

        Sub New(i As TItem)
            MyBase.New({i})
        End Sub
    End Class

    Public Function IsKnown(a As TItem) As Boolean
        Return EqualGroups.ContainsKey(a)
    End Function

    Public Function IsLesser(a As TItem, b As TItem) As Boolean
        Dim ToVisit As New List(Of Node)
        If GreaterItems.ContainsKey(EqualGroups(a)) Then
            For Each i As TItem In GreaterItems(EqualGroups(a))
                If Not ToVisit.Contains(EqualGroups(i)) Then
                    ToVisit.Add(EqualGroups(i))
                End If
            Next
        End If
        Dim current As Integer = 0
        Do While current < ToVisit.Count
            If ToVisit(current).Contains(b) Then
                Return True
            End If
            If GreaterItems.ContainsKey(ToVisit(current)) Then
                For Each i As TItem In GreaterItems(ToVisit(current))
                    ToVisit.Add(EqualGroups(i))
                Next
            End If
            current += 1
        Loop
        Return False
    End Function

    Public Function IsEqual(a As TItem, b As TItem) As Boolean
        Return EqualGroups(a).Contains(b)
    End Function

    Public Function IsLesserorEqual(a As TItem, b As TItem) As Boolean
        Return IsEqual(a, b) OrElse IsLesser(a, b)
    End Function

    Public Function IsGreater(a As TItem, b As TItem) As Boolean
        Return IsLesser(b, a)
    End Function

    Public Function IsGreaterOrEqual(a As TItem, b As TItem) As Boolean
        Return Not IsGreater(a, b) Or IsEqual(a, b)
    End Function

    Public Function Compare(a As TItem, b As TItem) As Integer
        If IsLesser(a, b) Then
            Return -1
        ElseIf IsGreater(a, b) Then
            Return 1
        ElseIf IsEqual(a, b) Then
            Return 0
        Else
            Throw New UnknownCompareException
        End If
    End Function

    Public Sub SetLesser(lesser As TItem, greater As TItem)
        If IsKnown(lesser) And IsKnown(greater) Then
            If IsLesser(lesser, greater) Then
                Return 'do nothing
            ElseIf IsEqual(lesser, greater) OrElse IsGreater(lesser, greater) Then
                Throw New AlreadySetException
            End If
        End If
        If Not EqualGroups.ContainsKey(greater) Then
            EqualGroups.Add(greater, New Node(greater))
        End If
        If Not EqualGroups.ContainsKey(lesser) Then
            EqualGroups.Add(lesser, New Node(lesser))
        End If
        If Not GreaterItems.ContainsKey(EqualGroups(lesser)) Then
            GreaterItems.Add(EqualGroups(lesser), New HashSet(Of TItem))
        End If
        GreaterItems(EqualGroups(lesser)).Add(greater)
    End Sub

    Public Sub SetEqual(a As TItem, b As TItem)
        If IsKnown(a) And IsKnown(b) Then
            If IsEqual(a, b) Then
                Return 'do nothing
            ElseIf IsLesser(a, b) OrElse IsGreater(a, b) Then
                Throw New AlreadySetException
            End If
        End If
        If Not EqualGroups.ContainsKey(a) Then
            EqualGroups.Add(a, New Node(a))
        End If
        If Not EqualGroups.ContainsKey(b) Then
            EqualGroups.Add(b, New Node(b))
        End If
        EqualGroups(a).UnionWith(EqualGroups(b))
        EqualGroups(b) = EqualGroups(a)
    End Sub

    Public Sub SetGreater(greater As TItem, lesser As TItem)
        SetLesser(lesser, greater)
    End Sub

    Public Sub Trim()
        'For Each node As Node In EqualGroups.Values
        '    Dim greaternodes As New HashSet(Of HashSet(Of TItem))
        '    For Each greateritem As TItem In GreaterItems(node).ToList
        '        If greaternodes.Contains(EqualGroups(greateritem)) Then
        '            GreaterItems(node).Remove(greateritem)
        '        Else
        '            greaternodes.Add(EqualGroups(greateritem))
        '        End If
        '    Next
        'Next
        'can we reach neighbor from item without the direct connection?
        For Each source As Node In GreaterItems.Keys
            For Each destination As TItem In GreaterItems(source)
                Dim destinationNode As Node = EqualGroups(destination)
                'can we remove this destination?
                'only if we can show that source points to destination's node in another way
                Dim ToVisit As New List(Of Node)
                If GreaterItems.ContainsKey(source) Then
                    For Each i As TItem In GreaterItems(source).Except({destination})
                        If Not ToVisit.Contains(EqualGroups(i)) Then
                            ToVisit.Add(EqualGroups(i))
                        End If
                    Next
                End If
                Dim current As Integer = 0
                Do While current < ToVisit.Count
                    If ToVisit(current).Contains(destination) Then
                        'can remove
                        GreaterItems(source).Remove(destination)
                        Exit For
                    End If
                    If GreaterItems.ContainsKey(ToVisit(current)) Then
                        For Each i As TItem In GreaterItems(ToVisit(current))
                            ToVisit.Add(EqualGroups(i))
                        Next
                    End If
                    current += 1
                Loop
            Next
        Next
    End Sub
End Class