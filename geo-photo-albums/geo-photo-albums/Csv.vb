Class Csv
    Private Fields_ As New List(Of String)
    Private Rows_ As New List(Of IDictionary(Of String, String))

    Sub Sort(comparer As IComparer(Of IDictionary(Of String, String)))
        Rows_.Sort(comparer)
    End Sub

    ''' <summary>
    ''' Write Csv class' contents to a file
    ''' </summary>
    ''' <param name="file">Stream for writing</param>
    ''' <remarks></remarks>
    Sub WriteFile(file As System.IO.TextWriter)
        file.WriteLine(String.Join(",", Fields_))
        For Each row As IDictionary(Of String, String) In Rows_
            Dim row_copy As IDictionary(Of String, String) = row
            file.WriteLine(String.Join(",", Fields_.Select(Function(f As String)
                                                               Return row_copy(f)
                                                           End Function)))
        Next
    End Sub

    ''' <summary>
    ''' Write Csv class' contents to a file
    ''' </summary>
    ''' <param name="filename">Name of file to write</param>
    ''' <remarks></remarks>
    Sub WriteFile(filename As String)
        Dim file As System.IO.TextWriter = System.IO.File.CreateText(filename)
        WriteFile(file)
        file.Close()
    End Sub

    ''' <summary>
    ''' Load a file into this Csv class
    ''' </summary>
    ''' <param name="file">A stream of lines in a csv</param>
    ''' <remarks></remarks>
    Sub Read(file As System.IO.TextReader)
        Dim line As String = file.ReadLine()
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
        Do
            line = file.ReadLine
            If line IsNot Nothing Then
                Dim values As New List(Of String)(line.Split(","c))
                Debug.Assert(values.Count = Fields_.Count)
                Dim new_row As New Dictionary(Of String, String)
                For i As Integer = 0 To Fields_.Count - 1
                    new_row.Add(new_fields(0), values(0))
                Next
                Rows_.Add(new_row)
            End If
        Loop While line IsNot Nothing
    End Sub

    ''' <summary>
    ''' Read streams into Csv
    ''' </summary>
    ''' <param name="files">Csv streams</param>
    ''' <remarks></remarks>
    Sub Read(files As Generic.IEnumerable(Of System.IO.TextReader))
        For Each f As System.IO.TextReader In files
            Read(f)
        Next
    End Sub

    ''' <summary>
    ''' Read file into Csv
    ''' </summary>
    ''' <param name="filename">Csv filename</param>
    ''' <remarks></remarks>
    Sub Read(filename As String)
        Dim file As System.IO.TextReader = System.IO.File.OpenText(filename)
        Read(file)
        file.Close()
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
End Class
