Imports System.Runtime.CompilerServices

Module EnumerableExtensions
    <Extension()>
    Public Iterator Function Repeat(Of T)(i As T) As IEnumerable(Of T)
        Do While True
            Yield i
        Loop
    End Function

    ''' <summary>
    ''' Merge multiple, sorted enumerables into a single, sorted enumerables
    ''' </summary>
    ''' <typeparam name="Tvalue">Type of elements in the output enumerable</typeparam>
    ''' <param name="e">Enumerable of sorted enumerables</param>
    ''' <returns>Enumerable of sorted outputs</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Iterator Function MergeSorted(Of Tvalue)(e As IEnumerable(Of IEnumerable(Of Tvalue))) As IEnumerable(Of Tvalue)
        Dim inputs As New List(Of Tuple(Of Tvalue, IEnumerator(Of Tvalue)))
        For Each i As IEnumerable(Of Tvalue) In e
            Dim enumerator1 As IEnumerator(Of Tvalue) = i.GetEnumerator()
            If enumerator1.MoveNext Then
                Dim value As Tvalue = enumerator1.Current
                inputs.Add(New Tuple(Of Tvalue, IEnumerator(Of Tvalue))(value,
                                                                    enumerator1))
            End If
        Next
        Do While inputs.Count > 0
            inputs.Sort()
            Yield inputs(0).Item1
            If inputs(0).Item2.MoveNext() Then
                inputs(0) = New Tuple(Of Tvalue, IEnumerator(Of Tvalue))(inputs(0).Item2.Current,
                                                                         inputs(0).Item2)
            Else
                inputs.RemoveAt(0)
            End If
        Loop
    End Function
End Module
