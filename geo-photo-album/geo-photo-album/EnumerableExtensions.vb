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
    Iterator Function MergeSorted(Of Tvalue As IComparable(Of Tvalue))(e As IEnumerable(Of IEnumerable(Of Tvalue))) As IEnumerable(Of Tvalue)
        Dim inputs As List(Of Tuple(Of Tvalue, IEnumerator(Of Tvalue)))
        'we use SelectMany so that we can yield nothing if the csv is empty
        inputs = e.SelectMany(Iterator Function(x As IEnumerable(Of Tvalue))
                                  Dim x1 As IEnumerator(Of Tvalue) = x.GetEnumerator
                                  If x1.MoveNext Then
                                      Yield New Tuple(Of Tvalue, IEnumerator(Of Tvalue))(x1.Current, x1)
                                  End If
                              End Function).ToList
        Do While inputs.Count > 0
            inputs = inputs.OrderBy(Function(x As Tuple(Of Tvalue, IEnumerator(Of Tvalue)))
                                        Return x.Item1
                                    End Function).ToList
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
    Iterator Function FilterByPrevious(Of T As {Structure, IComparable(Of T)})(input As IEnumerable(Of T),
                                                  pairFilter As Func(Of T, T, Boolean)) As IEnumerable(Of T)
        Dim e As IEnumerator(Of T) = input.GetEnumerator
        Dim previous_input As T? = Nothing
        Dim previous_output As T? = Nothing
        Do While e.MoveNext
            If previous_output Is Nothing OrElse pairFilter(CType(previous_output, T), e.Current) Then
                Yield e.Current
                previous_output = e.Current
                previous_input = Nothing
            Else
                previous_input = e.Current
            End If
        Loop
        If previous_input IsNot Nothing Then 'we need to output the last one
            Yield CType(previous_input, T)
        End If
    End Function

    ''' <summary>
    ''' Like Select but special case for the last element
    ''' </summary>
    ''' <typeparam name="Tin"></typeparam>
    ''' <typeparam name="Tout"></typeparam>
    ''' <param name="e">enumerable input</param>
    ''' <param name="butLast">function to apply to all but the last member</param>
    ''' <param name="last"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Iterator Function Select2(Of Tin, Tout)(e As IEnumerable(Of Tin),
                                            butLast As Func(Of Tin, Tout),
                                            last As Func(Of Tin, Tout)) As IEnumerable(Of Tout)
        Dim e1 As IEnumerator(Of Tin) = e.GetEnumerator
        If e1.MoveNext Then
            Dim previous As Tin = e1.Current

            Do While e1.MoveNext()
                If previous IsNot Nothing Then
                    Yield butLast(previous)
                End If
                previous = e1.Current
            Loop
            Yield last(previous)
        End If
    End Function

    Interface IIterator(Of T)
        Inherits IEnumerator(Of T)
        ''' <summary>
        ''' True if current is valid
        ''' </summary>
        ''' <value>True if current is valid</value>
        ''' <returns>True or False</returns>
        ''' <remarks></remarks>
        ReadOnly Property HasCurrent As Boolean

        ''' <summary>
        ''' Returns current element and moves to next
        ''' </summary>
        ''' <returns>current element</returns>
        ''' <remarks></remarks>
        Function [Next]() As T
    End Interface

    Class Iterator(Of T)
        Implements IIterator(Of T)

        Private e As IEnumerator(Of T)
        Private hasCurrent_ As Boolean = False

        Sub New(e As IEnumerator(Of T))
            Me.e = e
        End Sub

        Public ReadOnly Property HasCurrent As Boolean Implements IIterator(Of T).HasCurrent
            Get
                Return hasCurrent_
            End Get
        End Property

        Public ReadOnly Property Current As T Implements IEnumerator(Of T).Current
            Get
                Return e.Current
            End Get
        End Property

        Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
            Get
                Return e.Current
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            hasCurrent_ = e.MoveNext
            Return hasCurrent_
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            e.Reset()
        End Sub

        Public Function [Next]() As T Implements IIterator(Of T).Next
            If Not HasCurrent Then
                Throw New ArgumentException("Past end of list in Next")
            End If
            Dim ret As T = e.Current
            MoveNext()
            Return ret
        End Function
#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    e.Dispose()
                End If
            End If
            Me.disposedValue = True
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    <Extension>
    Function GetIterator(Of T)(e As IEnumerable(Of T)) As Iterator(Of T)
        Return New Iterator(Of T)(e.GetEnumerator)
    End Function
End Module
