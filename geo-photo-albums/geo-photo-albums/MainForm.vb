Imports System.Runtime.CompilerServices

Class MainForm

    Private Sub SourceFile_Click(sender As System.Object, e As System.EventArgs, txt As TextBox)
        Dim ofd As New OpenFileDialog
        ofd.DefaultExt = "csv"
        If System.IO.File.Exists(txt.Text) Then
            ofd.FileName = txt.Text
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(txt.Text)
        ElseIf System.IO.Directory.Exists(txt.Text) Then
            ofd.InitialDirectory = txt.Text
        End If
        ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        ofd.Multiselect = True
        ofd.Title = "Find CSV File(s)..."
        If ofd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If ofd.FileNames.Count > 1 Then
                txt.Text = String.Join(",", ofd.FileNames.Select(Function(s As String)
                                                                     Return """" + s + """"
                                                                 End Function))
            Else
                txt.Text = ofd.FileNames(0)
            End If
        End If
        ofd.Dispose()
    End Sub
    Private Sub SourceDir_Click(sender As System.Object, e As System.EventArgs, txt As TextBox)
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "Find CSV Folder..."
        If System.IO.File.Exists(txt.Text) Then
            fbd.SelectedPath = System.IO.Path.GetDirectoryName(txt.Text)
        ElseIf System.IO.Directory.Exists(txt.Text) Then
            fbd.SelectedPath = txt.Text
        End If
        If fbd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txt.Text = fbd.SelectedPath
        End If
        fbd.Dispose()
    End Sub
    Private Sub DestFile_Click(sender As System.Object, e As System.EventArgs, txt As TextBox)
        Dim sfd As New SaveFileDialog
        sfd.DefaultExt = "csv"
        If System.IO.File.Exists(txt.Text) Then
            sfd.FileName = txt.Text
            sfd.InitialDirectory = System.IO.Path.GetDirectoryName(txt.Text)
        ElseIf System.IO.Directory.Exists(txt.Text) Then
            sfd.InitialDirectory = txt.Text
        End If
        sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        sfd.Title = "Select output file..."
        If sfd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txt.Text = sfd.FileName
        End If
        sfd.Dispose()
    End Sub
    Private Sub DestDir_Click(sender As System.Object, e As System.EventArgs, txt As TextBox)
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "Select output Folder..."
        If System.IO.File.Exists(txt.Text) Then
            fbd.SelectedPath = System.IO.Path.GetDirectoryName(txt.Text)
        ElseIf System.IO.Directory.Exists(txt.Text) Then
            fbd.SelectedPath = txt.Text
        End If
        If fbd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txt.Text = fbd.SelectedPath
        End If
        fbd.Dispose()
    End Sub
#Region "Sort CSV"
    Private Sub btnSourceFile_Click(sender As System.Object, e As System.EventArgs) Handles btnSortSrcFile.Click
        SourceFile_Click(sender, e, txtSortSrc)
    End Sub

    Private Sub btnSourceDir_Click(sender As System.Object, e As System.EventArgs) Handles btnSortSrcDir.Click
        SourceDir_Click(sender, e, txtSortSrc)
    End Sub

    Private Sub btnDestFile_Click(sender As System.Object, e As System.EventArgs) Handles btnSortDestFile.Click
        DestFile_Click(sender, e, txtSortDest)
    End Sub

    Private Sub btnDestDir_Click(sender As System.Object, e As System.EventArgs) Handles btnSortDestDir.Click
        DestDir_Click(sender, e, txtSortDest)
    End Sub

    Private Sub sort_dir_recursive(source_dir As String, dest_dir As String)
        For Each f As String In System.IO.Directory.EnumerateFiles(source_dir, "*.csv")
            Dim c As New Csv(f)
            c.Sort(Function(x As IDictionary(Of String, String))
                       Return GpsSample.FromDict(x)
                   End Function)
            c.WriteFile(System.IO.Path.Combine(dest_dir, System.IO.Path.GetFileName(f)))
        Next
        For Each d As String In System.IO.Directory.EnumerateDirectories(source_dir)
            sort_dir_recursive(d,
                               System.IO.Path.Combine(dest_dir, System.IO.Path.GetFileName(d)))
        Next
    End Sub

    Private Sub btnSort_Click(sender As System.Object, e As System.EventArgs) Handles btnSort.Click
        Dim start_time As DateTimeOffset = DateTimeOffset.Now
        Dim sources As New List(Of String)
        For Each filename As String In txtSortSrc.Text.Split(";"c)
            sources.Add(filename.Trim(""""c))
        Next
        Dim destination As String = txtSortDest.Text.Trim(""""c)
        If System.IO.Directory.Exists(destination) Then
            For Each source As String In sources
                If System.IO.Directory.Exists(source) Then
                    sort_dir_recursive(source, destination)
                ElseIf System.IO.File.Exists(source) Then
                    Dim c As New Csv(source)
                    c.Sort(Function(x As IDictionary(Of String, String))
                               Return GpsSample.FromDict(x)
                           End Function)
                    c.WriteFile(System.IO.Path.Combine(destination,
                                                       System.IO.Path.GetFileName(source)))
                Else
                    Throw New ApplicationException(source + " not found")
                End If
            Next
        Else 'it's a file destination
            Dim c As New Csv()
            For Each source As String In sources
                If System.IO.Directory.Exists(source) Then
                    For Each csv_file As String In System.IO.Directory.GetFiles(source, "*.csv", IO.SearchOption.AllDirectories)
                        c.Read(csv_file)
                    Next
                ElseIf System.IO.File.Exists(source) Then
                    c.Read(source)
                Else
                    Throw New ApplicationException(source + " not found")
                End If
            Next
            c.Sort(Function(x As IDictionary(Of String, String))
                       Return GpsSample.FromDict(x)
                   End Function)
            c.WriteFile(destination)
        End If
        MsgBox((DateTimeOffset.Now - start_time).TotalSeconds)
    End Sub
#End Region

#Region "Filter CSV"

#End Region
End Class
