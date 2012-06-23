Imports System.Runtime.CompilerServices
Imports System.Web.Script.Serialization

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
    End Sub
#End Region

#Region "Filter CSV"
    Private Sub btnFilterSrcFile_Click(sender As System.Object, e As System.EventArgs) Handles btnFilterSrcFile.Click
        SourceFile_Click(sender, e, txtFilterSrc)
    End Sub

    Private Sub btnFilterSrcDir_Click(sender As System.Object, e As System.EventArgs) Handles btnFilterSrcDir.Click
        SourceDir_Click(sender, e, txtFilterSrc)
    End Sub

    Private Sub btnFilterDestFile_Click(sender As System.Object, e As System.EventArgs) Handles btnFilterDestFile.Click
        DestFile_Click(sender, e, txtFilterDest)
    End Sub

    Private Sub btnFilterDestDir_Click(sender As System.Object, e As System.EventArgs) Handles btnFilterDestDir.Click
        DestDir_Click(sender, e, txtFilterDest)
    End Sub

    Private Sub btnFilter_Click(sender As System.Object, e As System.EventArgs) Handles btnFilter.Click
        Dim sources As New List(Of String)
        For Each filename As String In txtFilterSrc.Text.Split(";"c)
            sources.Add(filename.Trim(""""c))
        Next
        Dim destination As String = txtFilterDest.Text.Trim(""""c)
        If System.IO.Directory.Exists(destination) Then
            Dim source_enumerables As New List(Of IEnumerable(Of String))
            For Each source As String In sources
                If System.IO.Directory.Exists(source) Then
                    Dim csv_filenames As IEnumerable(Of String) = System.IO.Directory.EnumerateFiles(source, "*.csv", IO.SearchOption.AllDirectories)
                    Dim csv_file_enumerables As IEnumerable(Of IEnumerable(Of String))
                    csv_file_enumerables = csv_filenames.Select(Function(filename As String)
                                                                    Return System.IO.File.ReadLines(filename)
                                                                End Function)
                    source_enumerables.AddRange(csv_file_enumerables)
                ElseIf System.IO.File.Exists(source) Then
                    source_enumerables.Add(System.IO.File.ReadLines(source))
                Else
                    Throw New ApplicationException(source + " not found")
                End If
            Next
            Dim csvs_rows As IEnumerable(Of IEnumerable(Of Dictionary(Of String, String)))
            csvs_rows = source_enumerables.Select(Function(csv_file_lines As IEnumerable(Of String))
                                                      Return Csv.EnumerateLines(csv_file_lines)
                                                  End Function)
            Dim gps_samples_enumerable As IEnumerable(Of IEnumerable(Of GpsSample))
            gps_samples_enumerable = csvs_rows.Select(Function(csv_rows As IEnumerable(Of IDictionary(Of String, String)))
                                                          Return csv_rows.Select(Function(dict As IDictionary(Of String, String))
                                                                                     Return GpsSample.FromDict(dict)
                                                                                 End Function)
                                                      End Function)
            Dim gps_samples As IEnumerable(Of GpsSample)
            gps_samples = gps_samples_enumerable.MergeSorted()
            Dim filtered_gps_samples As IEnumerable(Of GpsSample)
            filtered_gps_samples = gps_samples.FilterByPrevious(Function(x As GpsSample, y As GpsSample)
                                                                    Return x.Coordinate.ZoomToPoint(0).Floor <> y.Coordinate.ZoomToPoint(0).Floor
                                                                End Function)
            For Each g As GpsSample In filtered_gps_samples
                Debug.WriteLine(g)
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
    End Sub
#End Region

#Region "Tag Files"
    Dim myJson As New Json

    Private Sub btnTagDestFile_Click(sender As System.Object, e As System.EventArgs) Handles btnLoadJson.Click
        Dim ofd As New OpenFileDialog
        ofd.DefaultExt = "json"
        If System.IO.File.Exists(txtTagFile.Text) Then
            ofd.FileName = System.IO.Path.GetFileName(txtTagFile.Text)
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(txtTagFile.Text)
        ElseIf System.IO.Directory.Exists(txtTagFile.Text) Then
            ofd.InitialDirectory = txtTagFile.Text
        End If
        ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
        ofd.Title = "Select JSON file..."
        If ofd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            txtTagFile.Text = ofd.FileName
            'Now load in the tags and everything else
            If System.IO.File.Exists(txtTagFile.Text) Then
                myJson.MergeFile(txtTagFile.Text)
            End If
        End If
        ofd.Dispose()
    End Sub

    Private Sub txtTagFilter_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagFilter.KeyPress
        If e.KeyChar = vbCr Then
            btnFilterTags.PerformClick()
        End If
    End Sub
#End Region
    ' utility function to convert a byte array into a hex string
    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)
        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next
        Return sb.ToString().ToLower
    End Function

    Public Function SHA1CalcFile(ByVal filepath As String) As String
        ' open file (as read-only)
        Using reader As New System.IO.FileStream(filepath, IO.FileMode.Open, IO.FileAccess.Read)
            Using sha1 As New System.Security.Cryptography.SHA1CryptoServiceProvider
                ' hash contents of this stream
                Dim hash() As Byte = sha1.ComputeHash(reader)
                ' return formatted hash
                Return ByteArrayToString(hash)
            End Using
        End Using
    End Function

    Private Sub UpdateFileTags(filter_text As String)
        txtTagFilter.InvokeEx(Sub()
                                  txtTagFilter.Text = filter_text
                              End Sub)
        lvFileTags.InvokeEx(Sub()
                                lvFileTags.Items.Clear()
                            End Sub)
        If (System.IO.Directory.Exists(filter_text) AndAlso
            (New System.IO.DirectoryInfo(filter_text).Attributes And IO.FileAttributes.ReparsePoint) = 0) Then
            If System.IO.Directory.GetParent(filter_text) IsNot Nothing Then
                Dim lvi_subdir As New ListViewItem("..")
                lvi_subdir.Tag = System.IO.Directory.GetParent(filter_text).FullName
                lvi_subdir.ImageKey = CacheShellIcon(System.IO.Directory.GetParent(filter_text).FullName)
                lvFileTags.InvokeEx(Sub()
                                        lvFileTags.Items.Add(lvi_subdir)
                                    End Sub)
            End If
            For Each d As String In System.IO.Directory.EnumerateDirectories(filter_text)
                If lvFileTagsWorker.CancellationPending Then
                    Exit Sub
                End If
                Dim d_info As New System.IO.DirectoryInfo(d)
                If (d_info.Attributes And IO.FileAttributes.ReparsePoint) = 0 Then
                    Dim lvi As New ListViewItem(System.IO.Path.GetFileName(d))
                    lvi.Tag = d
                    lvi.ImageKey = CacheShellIcon(d)
                    lvFileTags.InvokeEx(Sub()
                                            lvFileTags.Items.Add(lvi)
                                        End Sub)
                End If
            Next
            Dim json_files As Dictionary(Of String, Json) = Nothing
            If myJson.ContainsKey("Tags") Then
                json_files = myJson("Tags")
            End If
            For Each f As String In System.IO.Directory.EnumerateFiles(filter_text)
                If lvFileTagsWorker.CancellationPending Then
                    Exit Sub
                End If
                Dim lvi As New ListViewItem(System.IO.Path.GetFileName(f))
                If json_files IsNot Nothing Then
                    Dim sha1_hash As String = SHA1CalcFile(f)
                    If json_files.ContainsKey(sha1_hash) Then
                        lvi.SubItems.Add(String.Join("", json_files(sha1_hash).ToString("", "").Skip(1).Select2(Function(s As String)
                                                                                                                    Return s
                                                                                                                End Function,
                                                                                                                Function(s As String)
                                                                                                                    Return ""
                                                                                                                End Function)))
                    End If
                End If
                lvi.Tag = f
                lvi.ImageKey = CacheShellIcon(f)
                lvFileTags.InvokeEx(Sub()
                                        lvFileTags.Items.Add(lvi)
                                    End Sub)
            Next
        End If
    End Sub

    Private Sub btnFilterTags_Click(sender As System.Object, e As System.EventArgs) Handles btnFilterTags.Click
        Do While lvFileTagsWorker.IsBusy
            lvFileTagsWorker.CancelAsync()
            Application.DoEvents()
        Loop
        lvFileTagsWorker.RunWorkerAsync(txtTagFilter.Text)
    End Sub

    Private Sub lvFileTags_MouseDoubleClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles lvFileTags.MouseDoubleClick
        Dim s As String = CType(lvFileTags.SelectedItems(0).Tag, String)
        If System.IO.Directory.Exists(s) Then
            Do While lvFileTagsWorker.IsBusy
                lvFileTagsWorker.CancelAsync()
                Application.DoEvents()
            Loop
            lvFileTagsWorker.RunWorkerAsync(s)
        End If
    End Sub

    Function CacheShellIcon(ByVal argPath As String) As String
        Dim mKey As String = argPath
        ' check if an icon for this key has already been added to the collection
        If ImageList1.Images.ContainsKey(mKey) = False Then
            ImageList1.Images.Add(mKey, GetShellIconAsImage(argPath))
        End If
        Return mKey
    End Function

    Private Sub lvFileTagsWorker_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles lvFileTagsWorker.DoWork
        UpdateFileTags(CType(e.Argument, String))
    End Sub

    Private Sub btnSaveJson_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveJson.Click
        Dim s As New System.Web.Script.Serialization.JavaScriptSerializer
        Dim sb As New System.Text.StringBuilder
        s.Serialize(myJson, sb)
        System.IO.File.WriteAllText(txtTagFile.Text, sb.ToString)
    End Sub

    Function StringToDict(s As String) As Dictionary(Of String, Object)
        Dim ret As New Dictionary(Of String, Object)
        For Each tag As String In s.Split(","c, ";"c)
            tag = tag.Trim
            If tag.Contains(":") Then
                ret.Add(tag.Substring(0, tag.IndexOf(":")).Trim,
                                 tag.Substring(tag.IndexOf(":") + 1).Trim)
            Else
                ret.Add(tag, "")
            End If
        Next
        Return ret
    End Function

    Function DictToString(d As Dictionary(Of String, Object)) As String
        Return String.Join(", ", d.Select(Function(kvp As KeyValuePair(Of String, Object))
                                              If kvp.Value.ToString <> "" Then
                                                  Return kvp.Key + ": " + kvp.Value.ToString
                                              Else
                                                  Return kvp.Key
                                              End If
                                          End Function))
    End Function

    Private Sub btnSaveTags_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveTags.Click
        If Not myJson.ContainsKey("Tags") Then
            myJson.Add("Tags")
        End If
        Dim json_all_tags As Json
        json_all_tags = myJson("Tags")
        Dim hash As String = SHA1CalcFile(DirectCast(lvFileTags.SelectedItems(0).Tag, String))
        If Not json_all_tags.ContainsKey(hash) Then
            json_all_tags.Add(hash, Json.FromString(txtTags.Text))
        Else
            json_all_tags(hash) = Json.FromString(txtTags.Text)
        End If
        If json_all_tags(hash).Count = 0 Then
            json_all_tags.Remove(hash)
        End If
    End Sub

    Private Sub lvFileTags_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvFileTags.SelectedIndexChanged
        If lvFileTags.SelectedItems.Count > 0 AndAlso lvFileTags.SelectedItems(0).SubItems.Count > 1 Then
            txtTags.Text = lvFileTags.SelectedItems(0).SubItems(1).Text
        Else
            txtTags.Text = ""
        End If
    End Sub
End Class
