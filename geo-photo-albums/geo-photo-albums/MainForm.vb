Imports System.Runtime.CompilerServices

Class MainForm

#Region "Sort CSV"
    Private Sub btnSourceFile_Click(sender As System.Object, e As System.EventArgs) Handles btnSourceFile.Click
        Dim ofd As New OpenFileDialog
        ofd.DefaultExt = "csv"
        If System.IO.File.Exists(txtSource.Text) Then
            ofd.FileName = txtSource.Text
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(txtSource.Text)
        ElseIf System.IO.Directory.Exists(txtSource.Text) Then
            ofd.InitialDirectory = txtSource.Text
        End If
        ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        ofd.Multiselect = True
        ofd.Title = "Find CSV File(s)..."
        If ofd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If ofd.FileNames.Count > 1 Then
                txtSource.Text = String.Join(",", ofd.FileNames.Select(Function(s As String)
                                                                           Return """" + s + """"
                                                                       End Function))
            Else
                txtSource.Text = ofd.FileNames(0)
            End If
        End If
        ofd.Dispose()
    End Sub

    Private Sub btnSourceDir_Click(sender As System.Object, e As System.EventArgs) Handles btnSourceDir.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "Find CSV Folder..."
        If System.IO.File.Exists(txtSource.Text) Then
            fbd.SelectedPath = System.IO.Path.GetDirectoryName(txtSource.Text)
        ElseIf System.IO.Directory.Exists(txtSource.Text) Then
            fbd.SelectedPath = txtSource.Text
        End If
        If fbd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtSource.Text = fbd.SelectedPath
        End If
        fbd.Dispose()
    End Sub

    Private Sub btnDestFile_Click(sender As System.Object, e As System.EventArgs) Handles btnDestFile.Click
        Dim sfd As New SaveFileDialog
        sfd.DefaultExt = "csv"
        If System.IO.File.Exists(txtDestination.Text) Then
            sfd.FileName = txtDestination.Text
            sfd.InitialDirectory = System.IO.Path.GetDirectoryName(txtDestination.Text)
        ElseIf System.IO.Directory.Exists(txtDestination.Text) Then
            sfd.InitialDirectory = txtDestination.Text
        End If
        sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        sfd.Title = "Select output file..."
        If sfd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtDestination.Text = sfd.FileName
        End If
        sfd.Dispose()
    End Sub

    Private Sub btnDestDir_Click(sender As System.Object, e As System.EventArgs) Handles btnDestDir.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "Select output Folder..."
        If System.IO.File.Exists(txtDestination.Text) Then
            fbd.SelectedPath = System.IO.Path.GetDirectoryName(txtDestination.Text)
        ElseIf System.IO.Directory.Exists(txtDestination.Text) Then
            fbd.SelectedPath = txtDestination.Text
        End If
        If fbd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtDestination.Text = fbd.SelectedPath
        End If
        fbd.Dispose()
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
        For Each filename As String In txtSource.Text.Split(";"c)
            sources.Add(filename.Trim(""""c))
        Next
        Dim destination As String = txtDestination.Text.Trim(""""c)
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

End Class
