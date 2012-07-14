Imports System.Runtime.CompilerServices
Imports System.Web.Script.Serialization

Class MainForm
#Region "Common CSV"
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
#End Region
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
    Dim MRU_rows As New Dictionary(Of String, Json)
    Dim MRU_keys As New List(Of String)

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
                lvFileTags.my_json = myJson
            End If
        End If
        ofd.Dispose()
    End Sub

    Private Sub txtTagFilter_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagFilter.KeyPress
        If e.KeyChar = vbCr Then
            btnFilterTags.PerformClick()
        End If
    End Sub

    Private Sub btnFilterTags_Click(sender As System.Object, e As System.EventArgs) Handles btnFilterTags.Click
        lvFileTags.Filter = txtTagFilter.Text
    End Sub

    Private Sub btnSaveJson_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveJson.Click
        System.IO.File.WriteAllLines(txtTagFile.Text, myJson.ToJson)
    End Sub
#End Region

    Private Sub btnSaveTags_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveTags.Click
        If Not myJson.ContainsKey("Tags") Then
            myJson.Add("Tags", New Json)
        End If
        Dim json_all_tags As Json
        json_all_tags = myJson("Tags")
        Dim for_addition As Json = Json.FromString(txtTags.Text)
        For Each selected_index As Integer In lvFileTags.SelectedIndices
            Dim current_hash As String = lvFileTags.Hash(selected_index)
            If Not json_all_tags.ContainsKey(current_hash) Then
                json_all_tags.Add(current_hash, New Json) 'add it in no matter what, maybe remove later
            End If
            For mru_index As Integer = 0 To MRU_keys.Count - 1
                If lstTagsMRU.GetItemCheckState(mru_index) = CheckState.Checked AndAlso MRU_rows(MRU_keys(mru_index)).ContainsKey(current_hash) Then
                    If Not json_all_tags(current_hash).ContainsKey(MRU_keys(mru_index)) Then
                        json_all_tags(current_hash).Add(MRU_keys(mru_index), MRU_rows(MRU_keys(mru_index))(current_hash))
                    Else
                        json_all_tags(current_hash)(MRU_keys(mru_index)) = MRU_rows(MRU_keys(mru_index))(current_hash)
                    End If
                    AddAutoTag(New KeyValuePair(Of String, Json)(MRU_keys(mru_index), MRU_rows(MRU_keys(mru_index))(current_hash)))
                ElseIf lstTagsMRU.GetItemCheckState(mru_index) = CheckState.Unchecked Then
                    If (json_all_tags(current_hash).ContainsKey(MRU_keys(mru_index)) AndAlso
                        json_all_tags(current_hash)(MRU_keys(mru_index)) = MRU_rows(MRU_keys(mru_index))(current_hash)) Then
                        json_all_tags(current_hash).Remove(MRU_keys(mru_index))
                    End If
                End If
            Next
            For Each add_tag As KeyValuePair(Of String, Json) In for_addition 'may override checkboxes
                If Not json_all_tags(current_hash).ContainsKey(add_tag.Key) Then
                    json_all_tags(current_hash).Add(add_tag)
                ElseIf json_all_tags(current_hash)(add_tag.Key) <> add_tag.Value Then
                    json_all_tags(current_hash)(add_tag.Key) = add_tag.Value
                End If
                AddAutoTag(New KeyValuePair(Of String, Json)(add_tag.Key, add_tag.Value))
            Next
            If json_all_tags(current_hash).Count = 0 Then 'clean up
                json_all_tags.Remove(current_hash)
                If lvFileTags.Items(selected_index).SubItems.Count > 1 Then
                    lvFileTags.Items(selected_index).SubItems.RemoveAt(1)
                End If
            Else
                If lvFileTags.Items(selected_index).SubItems.Count = 1 Then
                    lvFileTags.Items(selected_index).SubItems.Add(json_all_tags(current_hash).ToString)
                Else
                    lvFileTags.Items(selected_index).SubItems(1).Text = json_all_tags(current_hash).ToString
                End If
            End If
        Next
    End Sub

    Private Sub AddAutoTag(kvp As KeyValuePair(Of String, Json))
        If Not myJson.ContainsKey("TagsMRU") Then
            myJson.Add("TagsMRU", New Json)
        End If
        If Not myJson("TagsMRU").IsArray Then
            myJson("TagsMRU") = New Json
        End If
        Dim tag As New Json
        tag.Add(kvp.Key, kvp.Value)
        If kvp.Key <> "DateTime" AndAlso kvp.Key <> "Filename" AndAlso Not kvp.Key.StartsWith("Gps") Then
            If Not myJson("TagsMRU")(0) = tag Then
                myJson("TagsMRU").Remove(tag)
                myJson("TagsMRU").Insert(0, tag)
            End If
        End If
    End Sub

    Private Function GetAutoTags(filename As String) As List(Of KeyValuePair(Of String, Json))
        Dim ret As New List(Of KeyValuePair(Of String, Json))
        Dim er As ExifLib.ExifReader
        Try
            er = New ExifLib.ExifReader(filename)
        Catch
            er = Nothing
        End Try
        Dim dt As DateTime
        If er IsNot Nothing AndAlso er.GetTagValue(ExifLib.ExifTags.DateTimeOriginal, dt) Then
            ret.Add(New KeyValuePair(Of String, Json)("DateTime",
                                                      New Json(dt.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffff", Globalization.CultureInfo.InvariantCulture))))
        ElseIf er IsNot Nothing AndAlso er.GetTagValue(ExifLib.ExifTags.DateTimeDigitized, dt) Then
            ret.Add(New KeyValuePair(Of String, Json)("DateTime",
                                                      New Json(dt.ToString("""yyyy-MM-ddTHH\:mm\:ss.fffffff", Globalization.CultureInfo.InvariantCulture))))
        ElseIf er IsNot Nothing AndAlso er.GetTagValue(ExifLib.ExifTags.DateTime, dt) Then
            ret.Add(New KeyValuePair(Of String, Json)("DateTime",
                                                      New Json(dt.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffff", Globalization.CultureInfo.InvariantCulture))))
        Else
            dt = System.IO.File.GetLastWriteTime(filename)
            ret.Add(New KeyValuePair(Of String, Json)("DateTime",
                                                      New Json(dt.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffff", Globalization.CultureInfo.InvariantCulture))))
        End If
        Dim orient As UInt16
        If er IsNot Nothing AndAlso er.GetTagValue(ExifLib.ExifTags.Orientation, orient) Then
            ret.Add(New KeyValuePair(Of String, Json)("Orientation",
                                                      New Json(orient)))
        End If
        Dim GpsLatitude() As Double = Nothing
        If er IsNot Nothing AndAlso er.GetTagValue(ExifLib.ExifTags.GPSLatitude, GpsLatitude) Then
            ret.Add(New KeyValuePair(Of String, Json)("GpsLatitude", New Json(CDbl(GpsLatitude(0) + GpsLatitude(1) / 60 + GpsLatitude(2) / 3600))))
        End If
        Dim GpsLongitude() As Double = Nothing
        If er IsNot Nothing AndAlso er.GetTagValue(ExifLib.ExifTags.GPSLongitude, GpsLongitude) Then
            ret.Add(New KeyValuePair(Of String, Json)("GpsLongitude", New Json(CDbl(GpsLongitude(0) + GpsLongitude(1) / 60 + GpsLongitude(2) / 3600))))
        End If
        ret.Add(New KeyValuePair(Of String, Json)("Filename", New Json(filename)))

        If myJson.ContainsKey("TagsMRU") Then
            If myJson("TagsMRU").IsArray Then
                For Each tag As Json In myJson("TagsMRU")
                    ret.Add(New KeyValuePair(Of String, Json)(tag.Keys(0), tag(tag.Keys(0))))
                Next
            End If
        End If
        Return ret
    End Function

    Private Sub lvFileTags_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvFileTags.SelectedIndexChanged
        Dim valid_selection As Boolean = lvFileTags.SelectedItems.Count > 0
        Dim jsons As New Dictionary(Of String, Json)
        For Each i As Integer In lvFileTags.SelectedIndices
            If lvFileTags.IsDirectory(i) Then
                valid_selection = False
                Exit For
            ElseIf lvFileTags.Json(i) IsNot Nothing Then
                jsons.Add(lvFileTags.Hash(i), lvFileTags.Json(i))
            End If
        Next
        If valid_selection Then
            'convert the jsons to MRU rows
            lstTagsMRU.Items.Clear()
            MRU_rows = New Dictionary(Of String, Json)
            For Each index As Integer In lvFileTags.SelectedIndices
                Dim hash As String = lvFileTags.Hash(index)
                Dim json As Json = lvFileTags.Json(index)
                If json IsNot Nothing Then
                    For Each Tag As KeyValuePair(Of String, Json) In json
                        If Not MRU_rows.ContainsKey(Tag.Key) Then
                            MRU_rows.Add(Tag.Key, New Json)
                        End If
                        MRU_rows(Tag.Key).Add(hash, Tag.Value)
                    Next
                End If
                For Each autotag As KeyValuePair(Of String, Json) In GetAutoTags(lvFileTags.Fullname(index))
                    If Not MRU_rows.ContainsKey(autotag.Key) Then
                        MRU_rows.Add(autotag.Key, New Json)
                    End If
                    If Not MRU_rows(autotag.Key).ContainsKey(hash) Then
                        MRU_rows(autotag.Key).Add(hash, autotag.Value)
                    End If
                Next
            Next
            MRU_keys = New List(Of String)(MRU_rows.Keys)
            Dim json_for_text As New Json
            For Each TagName As String In MRU_keys
                Dim various As Boolean = False
                Dim match_count As Integer = 0
                Dim prev_tag As Json = Nothing
                For Each index As Integer In lvFileTags.SelectedIndices
                    Dim hash As String = lvFileTags.Hash(index)
                    'We want to write the tag:  TagName, MRU_rows(TagName)(hash) for file with hash "hash"
                    'Currently we have the tag: TagName, lvFileTags.Json(index)(Tagname) for file with hash lvFileTags.Hash(index)
                    If (lvFileTags.Json(index) IsNot Nothing AndAlso lvFileTags.Json(index).ContainsKey(TagName) AndAlso
                        MRU_rows(TagName).ContainsKey(hash)) Then
                        If MRU_rows(TagName)(hash) = lvFileTags.Json(index)(TagName) Then
                            match_count += 1
                        End If
                    End If
                    If MRU_rows(TagName).ContainsKey(hash) Then
                        If prev_tag Is Nothing Then
                            prev_tag = MRU_rows(TagName)(hash)
                        ElseIf prev_tag <> MRU_rows(TagName)(hash) Then
                            various = True
                        End If
                    End If
                Next
                Dim checkstate As CheckState
                If match_count = lvFileTags.SelectedIndices.Count Then
                    checkstate = Windows.Forms.CheckState.Checked
                ElseIf match_count = 0 Then
                    checkstate = Windows.Forms.CheckState.Unchecked
                Else
                    checkstate = Windows.Forms.CheckState.Indeterminate
                End If
                If various Then
                    lstTagsMRU.Items.Add(TagName + ": Various", checkstate)
                Else
                    If prev_tag Is Nothing Then prev_tag = New Json
                    lstTagsMRU.Items.Add(New Json(New KeyValuePair(Of String, Json)(TagName, prev_tag)), checkstate)
                    If checkstate = Windows.Forms.CheckState.Checked Then
                        'checked and no various, so it's a constant for all of them
                        json_for_text.Add(TagName, prev_tag)
                    End If
                End If
            Next
            txtTags.Text = json_for_text.ToString
            CheckJson()
            If lvFileTags.SelectedItems.Count = 1 Then
                Try
                    picPreview.Image = System.Drawing.Image.FromFile(lvFileTags.Fullname(lvFileTags.SelectedIndices(0)))
                    picPreview.Visible = True
                Catch ex As Exception
                    picPreview.Visible = False
                End Try
                If picPreview.Visible = False Then
                    Try
                        wmpPreview.URL = lvFileTags.Fullname(lvFileTags.SelectedIndices(0))
                        wmpPreview.Visible = True
                    Catch ex As Exception
                        wmpPreview.Visible = False
                    End Try
                Else
                    wmpPreview.URL = ""
                End If
            End If
        Else
            txtTags.Text = ""
            CheckJson()
        End If
    End Sub

    'Private Sub UpdateTagsMRU()
    '    If PriorityTagsMRU Then Return
    '    Dim j As Json = Json.FromString(txtTags.Text)
    '    For item_index As Integer = 0 To lstTagsMRU.Items.Count - 1
    '        Dim j2 As Json = DirectCast(lstTagsMRU.Items(item_index), Json)
    '        If j.ContainsKey(j2.Keys(0)) AndAlso j(j2.Keys(0)) = j2(j2.Keys(0)) Then
    '            lstTagsMRU.SetItemCheckState(item_index, CheckState.Checked)
    '        Else
    '            'do nothing, this isn't perfect....
    '        End If
    '    Next
    'End Sub

    Private Function CheckJson() As Boolean
        txtTags.ForeColor = Color.Black
        Try
            If txtTags.Text <> "" Then
                Json.FromString(txtTags.Text)
            End If
            txtTags.ForeColor = Color.Green
            btnSaveTags.Enabled = (lvFileTags.SelectedItems.Count > 0)
            Return True
        Catch ex As Exception
            txtTags.ForeColor = Color.Red
            btnSaveTags.Enabled = False
            Return False
        End Try
    End Function

    Private Sub txtTags_TextChanged(sender As Object, e As EventArgs) Handles txtTags.TextChanged
        CheckJson()
    End Sub

    Private Sub btnDetailView_Click(sender As Object, e As EventArgs) Handles btnDetailView.Click
        lvFileTags.View = View.Details
    End Sub

    Private Sub btnIconView_Click(sender As Object, e As EventArgs) Handles btnIconView.Click
        lvFileTags.View = View.LargeIcon
    End Sub
End Class
