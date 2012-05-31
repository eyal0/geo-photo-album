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
    Iterator Function MergeSorted(Of Tvalue)(e As IEnumerable(Of IEnumerable(Of Tvalue))) As IEnumerable(Of Tvalue)
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

    ''' <summary>
    ''' Filter input enumerable by looking at the current and most recently output value.
    ''' First value is always output if input.Count > 0.
    ''' Last value is always output if input.Count > 1.
    ''' </summary>
    ''' <typeparam name="T">Type of the enumerated values</typeparam>
    ''' <param name="input">Enumerable of values</param>
    ''' <param name="pairFilter">Function that returns True if second paramater should be output
    ''' given that the first parameter was most recently output.</param>
    ''' <returns>Enumerable of filtered values</returns>
    ''' <remarks></remarks>
    <Extension()>
    Iterator Function FilterByPrevious(Of T)(input As IEnumerable(Of T),
                                                  pairFilter As Func(Of T, T, Boolean)) As IEnumerable(Of T)
        Dim e As IEnumerator(Of T) = input.GetEnumerator
        Dim previous_input As T = Nothing
        Dim previous_output As T = Nothing
        Do While e.MoveNext
            If previous_input Is Nothing OrElse pairFilter(previous_input, e.Current) Then
                Yield e.Current
                previous_output = e.Current
                previous_input = Nothing
            Else
                previous_input = e.Current
            End If
        Loop
        If previous_input IsNot Nothing Then 'we need to output the last one
            Yield previous_input
        End If
    End Function
End Module
