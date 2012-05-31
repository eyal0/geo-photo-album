Class Csv
    Private Fields_ As New List(Of String)
    Private Rows_ As New List(Of IDictionary(Of String, String))

    Sub Sort(Of T As IComparable(Of T))(keySelector As Func(Of IDictionary(Of String, String), T))
        Rows_ = Rows_.OrderBy(Of T)(keySelector).ToList
    End Sub

        ''' <summary>
        ''' Yield a stream of CSV lines
        ''' </summary>
        ''' <returns>IEnumerable of CSV lines</returns>
        ''' <remarks></remarks>
    Iterator Function EnumerateLines() As IEnumerable(Of String)
        Yield String.Join(",", Fields_)
        For Each row As IDictionary(Of String, String) In Rows_
            Dim row_copy As IDictionary(Of String, String) = row
            Yield String.Join(",", Fields_.Select(Function(f As String)
                                                      Return row_copy(f)
                                                  End Function))
        Next
    End Function

    ''' <summary>
    ''' Write Csv class' contents to a file
    ''' </summary>
    ''' <param name="filename">Name of file to write</param>
    ''' <remarks></remarks>
    Sub WriteFile(filename As String)
        System.IO.File.WriteAllLines(filename, EnumerateLines)
    End Sub

    ''' <summary>
    ''' Load a file into this Csv class
    ''' </summary>
    ''' <param name="lines">A stream of lines in a csv</param>
    ''' <remarks></remarks>
    Sub ReadLines(lines As IEnumerable(Of String))
        Dim lines1 As IEnumerator(Of String) = lines.GetEnumerator
        lines1.MoveNext()
        Dim line As String = lines1.Current
        Dim new_fields As New List(Of String)(line.Split(","c))
        Dim new_fields_set As New HashSet(Of String)(new_fields)
        Debug.Assert(new_fields_set.Count = new_fields.Count)
        Dim fields_set As New HashSet(Of String)(Fields_)
        If fields_set.IsSupersetOf(new_fields_set) Then
            'do nothing
        ElseIf new_fields_set.IsSupersetOf(fields_set) Then
            Fields_ = new_fields
        Else
            Fields_ = Fields_.Concat(new_fields_set.Except(fields_set)).ToList
        End If
        Do While lines1.MoveNext
            line = lines1.Current
            Dim values As New List(Of String)(line.Split(","c))
            Debug.Assert(values.Count = Fields_.Count)
            Dim new_row As New Dictionary(Of String, String)
            For i As Integer = 0 To Fields_.Count - 1
                new_row.Add(new_fields(i), values(i))
            Next
            Rows_.Add(new_row)
        Loop
    End Sub

    ''' <summary>
    ''' Read file into Csv
    ''' </summary>
    ''' <param name="filename">Csv filename</param>
    ''' <remarks></remarks>
    Sub Read(filename As String)
        Me.ReadLines(System.IO.File.ReadLines(filename))
    End Sub

    ''' <summary>
    ''' Read files into Csv
    ''' </summary>
    ''' <param name="filenames">Csv filenames</param>
    ''' <remarks></remarks>
    Sub Read(filenames As Generic.IEnumerable(Of String))
        For Each filename As String In filenames
            Read(filename)
        Next
    End Sub

    ''' <summary>
    ''' New Csv loaded with named files
    ''' </summary>
    ''' <param name="filenames">Csv files</param>
    ''' <remarks></remarks>
    Sub New(filenames As Generic.IEnumerable(Of String))
        Read(filenames)
    End Sub

    ''' <summary>
    ''' New Csv loaded from named file
    ''' </summary>
    ''' <param name="filename">Csv file</param>
    ''' <remarks></remarks>
    Sub New(filename As String)
        Read(filename)
    End Sub

    Sub New()
        'do nothing
    End Sub
End Class
