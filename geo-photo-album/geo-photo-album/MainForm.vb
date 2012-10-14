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

    Private Function GetSortedGpsSamples(ByVal sources As List(Of String)) As IEnumerable(Of GpsSample)
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
        Return gps_samples
    End Function

    Const LogMinTileSize As Integer = 8

    Private Function GetTileSizes(gps_samples As IEnumerable(Of GpsSample), MaxZoomDepth As Integer) As SortedDictionary(Of Integer, SortedDictionary(Of Integer, Dictionary(Of Point, Integer)))
        Dim output_files As New SortedDictionary(Of Integer, SortedDictionary(Of Integer, Dictionary(Of Point, Integer)))
        Dim prev(0 To MaxZoomDepth - 1) As Tuple(Of Integer, GpsSample)
        Dim sample_index As Integer = 0
        For Each g As GpsSample In gps_samples
            Dim z As Integer = 0
            Dim latest_prev As Tuple(Of Integer, GpsSample) = Nothing
            Do While z < prev.Length
                If prev(z) IsNot Nothing AndAlso (latest_prev Is Nothing OrElse prev(z).Item1 > latest_prev.Item1) Then
                    latest_prev = prev(z)
                End If
                If latest_prev Is Nothing OrElse latest_prev.Item2.Coordinate.ZoomToPoint(z).Floor <> g.Coordinate.ZoomToPoint(z).Floor Then
                    Exit Do
                End If
                z = z + 1
            Loop
            If z < prev.Length Then 'otherwise it's not significant enough
                prev(z) = New Tuple(Of Integer, GpsSample)(sample_index, g) 'save the most recent output from this zoom level
                'need to output to this zoom level
                Dim tile_size As Integer = LogMinTileSize
                If Not output_files.ContainsKey(z) Then
                    output_files.Add(z, New SortedDictionary(Of Integer, Dictionary(Of Point, Integer)))
                End If
                If Not output_files(z).ContainsKey(LogMinTileSize) Then
                    output_files(z).Add(LogMinTileSize, New Dictionary(Of Point, Integer))
                End If
                Dim new_point As New Point(g.Coordinate.ZoomToPoint(z).Floor.X >> LogMinTileSize,
                                           g.Coordinate.ZoomToPoint(z).Floor.Y >> LogMinTileSize)
                If Not output_files(z)(LogMinTileSize).ContainsKey(new_point) Then
                    output_files(z)(LogMinTileSize).Add(new_point, 1)
                Else
                    output_files(z)(LogMinTileSize)(new_point) += 1
                End If
            End If
            sample_index += 1
        Next
        'Now combine tiles
        For Each zoom_level As Integer In output_files.Keys
            Const MinSamplesPerFile As Integer = 1024 'min length of json tile file
            Dim LogTestTileSize As Integer = LogMinTileSize + 1
            Do While LogTestTileSize <= LogMinTileSize + zoom_level
                If output_files(zoom_level).ContainsKey(LogTestTileSize - 1) Then
                    For Each kvp As KeyValuePair(Of Point, Integer) In output_files(zoom_level)(LogTestTileSize - 1)
                        If Not output_files(zoom_level).ContainsKey(LogTestTileSize) Then
                            output_files(zoom_level).Add(LogTestTileSize, New Dictionary(Of Point, Integer))
                        End If
                        Dim higher_point As New Point(kvp.Key.X \ 2, kvp.Key.Y \ 2)
                        If Not output_files(zoom_level)(LogTestTileSize).ContainsKey(higher_point) Then
                            output_files(zoom_level)(LogTestTileSize).Add(higher_point, kvp.Value)
                        Else
                            output_files(zoom_level)(LogTestTileSize)(higher_point) += kvp.Value
                        End If
                    Next
                    'now remove lower points if they are too small
                    For Each higher_point As Point In output_files(zoom_level)(LogTestTileSize).Keys.ToList
                        Dim use_bigger_tile As Boolean = False
                        If output_files(zoom_level)(LogTestTileSize)(higher_point) < MinSamplesPerFile * 8 Then
                            For x As Integer = 0 To 1
                                For y As Integer = 0 To 1
                                    Dim p As New Point(higher_point.X * 2 + x, higher_point.Y * 2 + y)
                                    If (output_files(zoom_level)(LogTestTileSize - 1).ContainsKey(p) AndAlso
                                        output_files(zoom_level)(LogTestTileSize - 1)(p) < MinSamplesPerFile) Then
                                        use_bigger_tile = True
                                        GoTo FoundSmallFile
                                    End If
                                Next
                            Next
                        End If
FoundSmallFile:         If use_bigger_tile Then 'there are small tiles and the bigger one isn't too much bigger
                            For x As Integer = 0 To 1
                                For y As Integer = 0 To 1
                                    Dim p As New Point(higher_point.X * 2 + x, higher_point.Y * 2 + y)
                                    output_files(zoom_level)(LogTestTileSize - 1).Remove(p)
                                Next
                            Next
                        Else 'remove the big file
                            output_files(zoom_level)(LogTestTileSize).Remove(higher_point)
                        End If
                    Next
                    If output_files(zoom_level)(LogTestTileSize - 1).Count = 0 Then
                        output_files(zoom_level).Remove(LogTestTileSize - 1)
                    End If
                    If output_files(zoom_level)(LogTestTileSize).Count = 0 Then
                        output_files(zoom_level).Remove(LogTestTileSize)
                    End If
                End If
                LogTestTileSize += 1
            Loop
        Next
        Return output_files
    End Function


    Sub WriteGpsSamples(dest_dir As String, gps_samples As IEnumerable(Of GpsSample), TileSizes As SortedDictionary(Of Integer, SortedDictionary(Of Integer, Dictionary(Of Point, Integer))))
        Dim sample_index As Integer = 0
        Dim output_files As New SortedDictionary(Of Integer, SortedDictionary(Of Integer, Dictionary(Of Point, IO.StreamWriter)))
        Dim prev(0 To TileSizes.Count - 1) As Tuple(Of Integer, GpsSample)
        Dim start_datetime As DateTimeOffset? = Nothing
        Dim end_datetime As DateTimeOffset? = Nothing
        For Each g As GpsSample In gps_samples
            If start_datetime Is Nothing Then
                start_datetime = g.Datetime
            End If
            end_datetime = g.Datetime
            Dim z As Integer = 0
            Dim latest_prev As Tuple(Of Integer, GpsSample) = Nothing
            Do While z < prev.Length
                If prev(z) IsNot Nothing AndAlso (latest_prev Is Nothing OrElse prev(z).Item1 > latest_prev.Item1) Then
                    latest_prev = prev(z)
                End If
                If latest_prev Is Nothing OrElse latest_prev.Item2.Coordinate.ZoomToPoint(z).Floor <> g.Coordinate.ZoomToPoint(z).Floor Then
                    Exit Do
                End If
                z = z + 1
            Loop
            If z < prev.Length Then 'otherwise it's not significant enough
                prev(z) = New Tuple(Of Integer, GpsSample)(sample_index, g) 'save the most recent output from this zoom level
                'need to output to this zoom level
                Dim LogTileSizeGuess As Integer = LogMinTileSize
                Dim PointGuess As Point = New Point(g.Coordinate.ZoomToPoint(z).Floor.X >> LogTileSizeGuess,
                                                    g.Coordinate.ZoomToPoint(z).Floor.Y >> LogTileSizeGuess)
                Do Until (TileSizes.ContainsKey(z) AndAlso
                          TileSizes(z).ContainsKey(LogTileSizeGuess) AndAlso
                          TileSizes(z)(LogTileSizeGuess).ContainsKey(PointGuess))
                    LogTileSizeGuess += 1
                    PointGuess = New Point(g.Coordinate.ZoomToPoint(z).Floor.X >> LogTileSizeGuess,
                                           g.Coordinate.ZoomToPoint(z).Floor.Y >> LogTileSizeGuess)
                Loop
                Dim current_filename As String = IO.Path.Combine(dest_dir,
                                                                 TupletoFilename(z, LogTileSizeGuess, PointGuess))
                If Not output_files.ContainsKey(z) Then
                    output_files.Add(z, New SortedDictionary(Of Integer, Dictionary(Of Point, IO.StreamWriter)))
                End If
                If Not output_files(z).ContainsKey(LogTileSizeGuess) Then
                    output_files(z).Add(LogTileSizeGuess, New Dictionary(Of Point, IO.StreamWriter))
                End If
                If Not output_files(z)(LogTileSizeGuess).ContainsKey(PointGuess) Then
                    If Not IO.Directory.Exists(IO.Path.GetDirectoryName(current_filename)) Then
                        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(current_filename))
                    End If
                    output_files(z)(LogTileSizeGuess).Add(PointGuess, IO.File.CreateText(current_filename))
                    output_files(z)(LogTileSizeGuess)(PointGuess).Write("{" & vbCrLf &
                                                                        "  ""path"" : [" & vbCrLf &
                                                                        "    " & GpsSampleToJsonString(sample_index, g))
                Else
                    output_files(z)(LogTileSizeGuess)(PointGuess).Write("," & vbCrLf &
                                                                        "    " & GpsSampleToJsonString(sample_index, g))
                End If
            End If
            sample_index += 1
        Next
        For Each zoom_level As Integer In output_files.Keys
            For Each LogTileSize As Integer In output_files(zoom_level).Keys
                For Each current_file As IO.StreamWriter In output_files(zoom_level)(LogTileSize).Values
                    current_file.Write(vbCrLf &
                                       "  ]" & vbCrLf &
                                       "}")
                    current_file.Close()
                Next
            Next
        Next
        Dim tile_info As IO.StreamWriter = IO.File.CreateText(IO.Path.Combine(dest_dir, "tile_info.json"))
        tile_info.Write("{" & vbCrLf &
                        "  ""tiles"" : {")
        For Each s1 As String In output_files.Keys.Select(Iterator Function(zoom_level As Integer)
                                                              Yield vbCrLf
                                                              Yield "    """ & zoom_level & """ : {"
                                                              For Each s2 As String In output_files(zoom_level).Keys.Select(Iterator Function(LogTileSize As Integer)
                                                                                                                                Yield vbCrLf & _
                                                                                                                                      "      """ & LogTileSize & """ : ["
                                                                                                                                For Each s3 As String In output_files(zoom_level)(LogTileSize).Keys.Select(Iterator Function(p As Point)
                                                                                                                                                                                                               Yield vbCrLf
                                                                                                                                                                                                               Yield "        """ & TupletoFilename(zoom_level, LogTileSize, p).Replace(IO.Path.DirectorySeparatorChar, "/") & """"
                                                                                                                                                                                                           End Function).Interleave(YieldOne(",")).Flatten
                                                                                                                                    Yield s3
                                                                                                                                Next
                                                                                                                                Yield vbCrLf & _
                                                                                                                                      "      ]"
                                                                                                                            End Function).Interleave(YieldOne(",")).Flatten
                                                                  Yield s2
                                                              Next

                                                              Yield vbCrLf & _
                                                                    "    }"
                                                          End Function).Interleave(YieldOne(",")).Flatten()
            tile_info.Write(s1)
        Next
        tile_info.Write(vbCrLf &
                        "  }," & vbCrLf &
                        "  ""start_datetime"" : """ & start_datetime.Value.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffzzz", Globalization.CultureInfo.InvariantCulture) & """," & vbCrLf &
                        "  ""end_datetime"" : """ & end_datetime.Value.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffzzz", Globalization.CultureInfo.InvariantCulture) & """," & vbCrLf &
                        "  ""generation_datetime"" : " & Date.UtcNow().Ticks & vbCrLf &
                        "}")
        tile_info.Close()
    End Sub

    Private Sub btnFilter_Click(sender As System.Object, e As System.EventArgs) Handles btnFilter.Click
        Dim sources As New List(Of String)
        For Each filename As String In txtFilterSrc.Text.Split(";"c)
            sources.Add(filename.Trim(""""c))
        Next
        Dim destination As String = txtFilterDest.Text.Trim(""""c)
        If System.IO.Directory.Exists(destination) Then
            Const MaxDepth As Integer = 24
            Dim TileSizes As SortedDictionary(Of Integer, SortedDictionary(Of Integer, Dictionary(Of Point, Integer))) = GetTileSizes(GetSortedGpsSamples(sources), MaxDepth)
            WriteGpsSamples(destination, GetSortedGpsSamples(sources), TileSizes)
        Else 'it's a file destination
            Dim c As New Csv
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

    Function TupletoFilename(zoom As Integer, LogTileSize As Integer, Tile As Point) As String
        Return IO.Path.Combine("tiles", zoom.ToString, "tile_" & zoom & "_" & LogTileSize & "_" & Tile.X & "_" & Tile.Y & ".json")
    End Function

    Function GpsSampleToJsonString(sample_index As Integer, g As GpsSample) As String
        Return ("[" &
                sample_index & "," &
                """" & g.Datetime.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffzzz", Globalization.CultureInfo.InvariantCulture) & """," &
                g.Coordinate.LatitudeInDegrees & "," &
                g.Coordinate.LongitudeInDegrees &
                "]")
    End Function
#End Region

#Region "Tag Files"
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
                lvFileTags.my_json.MergeFile(txtTagFile.Text)
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
        System.IO.File.WriteAllLines(txtTagFile.Text, lvFileTags.my_json.ToJson)
    End Sub

    Private Sub btnSaveTags_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveTags.Click
        If Not lvFileTags.my_json.ContainsKey("Tags") Then
            lvFileTags.my_json.Add("Tags", New Json)
        End If
        Dim json_all_tags As Json
        json_all_tags = lvFileTags.my_json("Tags")
        Dim for_addition As Json = Json.FromString(txtTags.Text)
        Dim various As New List(Of Boolean)
        For mru_index As Integer = 0 To MRU_keys.Count - 1
            Dim mru_index1 As Integer = mru_index
            various.Add(MRU_rows(MRU_keys(mru_index)).Count = 0 OrElse _
                        Not MRU_rows(MRU_keys(mru_index)).Values.All(Function(j As Json)
                                                                         Return MRU_rows(MRU_keys(mru_index1)).Values(0) = j
                                                                     End Function))
        Next
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
                ElseIf lstTagsMRU.GetItemCheckState(mru_index) = CheckState.Checked AndAlso Not various(mru_index) Then
                    If Not json_all_tags(current_hash).ContainsKey(MRU_keys(mru_index)) Then
                        json_all_tags(current_hash).Add(MRU_keys(mru_index), MRU_rows(MRU_keys(mru_index)).Values(0))
                    Else
                        json_all_tags(current_hash)(MRU_keys(mru_index)) = MRU_rows(MRU_keys(mru_index)).Values(0)
                    End If
                    AddAutoTag(New KeyValuePair(Of String, Json)(MRU_keys(mru_index), MRU_rows(MRU_keys(mru_index)).Values(0)))
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
        If Not lvFileTags.my_json.ContainsKey("TagsMRU") Then
            lvFileTags.my_json.Add("TagsMRU", New Json)
        End If
        If Not lvFileTags.my_json("TagsMRU").IsArray Then
            lvFileTags.my_json("TagsMRU") = New Json
        End If
        Dim tag As New Json
        tag.Add(kvp.Key, kvp.Value)
        If kvp.Key <> "DateTime" AndAlso kvp.Key <> "Filename" AndAlso Not kvp.Key.StartsWith("Gps") Then
            If lvFileTags.my_json("TagsMRU").IsArray AndAlso lvFileTags.my_json("TagsMRU").Count > 0 AndAlso Not lvFileTags.my_json("TagsMRU")(0) = tag Then
                lvFileTags.my_json("TagsMRU").Remove(tag)
                lvFileTags.my_json("TagsMRU").Insert(0, tag)
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

        If lvFileTags.my_json.ContainsKey("TagsMRU") Then
            If lvFileTags.my_json("TagsMRU").IsArray Then
                For Each tag As Json In lvFileTags.my_json("TagsMRU")
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
#End Region

#Region "Output"
    Dim photo_info As New Json
    Class PhotoInfo
        Implements IComparable(Of PhotoInfo)

        Property Filename As String
        Property DTO As DateTimeOffset
        Property Rank As Integer = -1
        Property LatLng As Coordinate? = Nothing
        Property Orientation As Integer = 1
        Property Passwords As List(Of String)
        Property ImageKey As Byte()
        Property EncryptedImageKeys As List(Of Tuple(Of Byte(), Byte()))

        Public Function CompareTo(other As PhotoInfo) As Integer Implements IComparable(Of PhotoInfo).CompareTo
            Return DTO.CompareTo(other.DTO)
        End Function

        Public Function ToJson(relative_path As String) As Json
            Dim ret As New Json
            If (relative_path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) OrElse
                        relative_path.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase) OrElse
                        relative_path.EndsWith(".webm", StringComparison.CurrentCultureIgnoreCase)) Then
                'do nothing
            ElseIf (relative_path.EndsWith(".mov", StringComparison.CurrentCultureIgnoreCase) OrElse
                    relative_path.EndsWith(".avi", StringComparison.CurrentCultureIgnoreCase) OrElse
                    relative_path.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase)) Then
                relative_path = relative_path.Substring(0, relative_path.LastIndexOf(".")) + ".webm"
            Else
                Throw New ApplicationException("Unrecognized file extension" & relative_path)
            End If
            ret.Add(New Json(relative_path))
            ret.Add(New Json(Rank))
            ret.Add(New Json(DTO.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffzzz", Globalization.CultureInfo.InvariantCulture)))
            ret.Add(New Json(LatLng.Value.LatitudeInDegrees))
            ret.Add(New Json(LatLng.Value.LongitudeInDegrees))
            If EncryptedImageKeys IsNot Nothing Then
                ret.Add(New Json(EncryptedImageKeys.Select(Function(t As Tuple(Of Byte(), Byte()))
                                                               Return New Json(New Json(CryptoHelpers.ByteArrayToString(t.Item1)), New Json(CryptoHelpers.ByteArrayToString(t.Item2)))
                                                           End Function)))
            End If
            Return ret
        End Function
    End Class

    Class PasswordInfo
        Sub New()
            'do nothing
        End Sub

        Sub New(Password As String)
            Me.Password = Password
            Const KEY_BYTE_COUNT As Integer = 16
            Const SALT_BYTE_COUNT As Integer = 8
            Dim pbkdf2 As New System.Security.Cryptography.Rfc2898DeriveBytes(Me.Password, SALT_BYTE_COUNT)
            Key = pbkdf2.GetBytes(KEY_BYTE_COUNT)
            Salt = pbkdf2.Salt
            Iterations = pbkdf2.IterationCount
        End Sub

        Sub New(password As String, salt As Byte())
            Me.Password = password
            Const KEY_BYTE_COUNT As Integer = 16
            Dim pbkdf2 As New System.Security.Cryptography.Rfc2898DeriveBytes(Me.Password, salt)
            Key = pbkdf2.GetBytes(KEY_BYTE_COUNT)
            Me.Salt = pbkdf2.Salt
            Iterations = pbkdf2.IterationCount
        End Sub

        Shared Function FromJsons(password As String, password_jsons As Json) As PasswordInfo
            For Each password_json As Json In password_jsons
                Dim salt As Byte() = CryptoHelpers.StringToByteArray(password_json(0).ToString)
                Dim current_pinfo As New PasswordInfo(password, salt)
                current_pinfo.RandomBytesEncrypted = CryptoHelpers.StringToByteArray(password_json(1).ToString)
                current_pinfo.RandomBytesPlus1Encrypted = CryptoHelpers.StringToByteArray(password_json(2).ToString)
                Dim RandomBytes As Byte() = AES_ECB_Decrypt(current_pinfo.Key, current_pinfo.RandomBytesEncrypted)
                Dim RandomBytesPlus1 As Byte() = AES_ECB_Decrypt(current_pinfo.Key, current_pinfo.RandomBytesPlus1Encrypted)
                RandomBytesPlus1(0) = CType((RandomBytesPlus1(0) + 255) Mod 256, Byte)
                If CryptoHelpers.ByteArrayToString(RandomBytes) = CryptoHelpers.ByteArrayToString(RandomBytesPlus1) Then
                    Return current_pinfo
                End If
            Next
            Return Nothing
        End Function

        Function ToJson() As Json
            If RandomBytesEncrypted Is Nothing OrElse RandomBytesPlus1Encrypted Is Nothing Then
                Dim RandomBytes As Byte() = New Byte(0 To 15) {}
                Security.Cryptography.RandomNumberGenerator.Create.GetBytes(RandomBytes)
                RandomBytesEncrypted = AES_ECB_Encrypt(Key, RandomBytes)
                RandomBytes(0) = CType((RandomBytes(0) + 1) Mod 256, Byte)
                RandomBytesPlus1Encrypted = AES_ECB_Encrypt(Key, RandomBytes)
                RandomBytes(0) = CType((RandomBytes(0) + 255) Mod 256, Byte)
            End If
            Dim ret As New Json
            ret.Add(New Json(CryptoHelpers.ByteArrayToString(Salt)), New Json(CryptoHelpers.ByteArrayToString(RandomBytesEncrypted)), New Json(CryptoHelpers.ByteArrayToString(RandomBytesPlus1Encrypted)))
            Return ret
        End Function
        Property Password As String
        Property Iterations As Integer
        Property Salt As Byte()
        Property Key As Byte()
        Property RandomBytesEncrypted As Byte()
        Property RandomBytesPlus1Encrypted As Byte()
    End Class

    Private ForceOverwrite As Boolean = True

    Private Sub btnOutput_Click(sender As Object, e As EventArgs) Handles btnOutput.Click
        Dim sources As New List(Of String)
        For Each filename As String In txtFilterSrc.Text.Split(";"c)
            sources.Add(filename.Trim(""""c))
        Next
        Dim destination As String = txtOutputDest.Text.Trim(""""c)
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

        If IO.File.Exists(txtOutputSrc.Text) Then
            Dim gpa_info As Json = Json.FromFile(txtOutputSrc.Text)
            Dim tags As Json = gpa_info("Tags")
            Dim all_photos As New List(Of PhotoInfo)
            Dim rootdir As String = Nothing
            For Each kvp As KeyValuePair(Of String, Json) In tags
                Dim new_photo As New PhotoInfo
                new_photo.Filename = kvp.Value("Filename").ToString
                Dim current_datetime As DateTime = DateTime.Parse(kvp.Value("DateTime").ToString)
                If kvp.Value.ContainsKey("TimeCorrection") Then
                    current_datetime = current_datetime + New TimeSpan(CInt(kvp.Value("TimeCorrection").ToDouble), 0, 0)
                End If
                new_photo.DTO = New DateTimeOffset(current_datetime, New TimeSpan(CInt(kvp.Value("TimeZone").ToDouble), 0, 0))
                new_photo.Orientation = CInt(kvp.Value("Orientation").ToDouble)
                If rootdir Is Nothing Then
                    rootdir = IO.Path.GetDirectoryName(new_photo.Filename)
                Else
                    Do Until IO.Path.GetFullPath(new_photo.Filename).StartsWith(rootdir)
                        rootdir = IO.Path.GetDirectoryName(rootdir)
                    Loop
                End If
                If kvp.Value.ContainsKey("GpsLatitude") AndAlso kvp.Value.ContainsKey("GpsLongitude") Then
                    new_photo.LatLng = Coordinate.FromDegrees(kvp.Value("GpsLatitude").ToDouble, kvp.Value("GpsLongitude").ToDouble)
                End If
                If kvp.Value.ContainsKey("Password") Then
                    new_photo.Passwords = New List(Of String)
                    If kvp.Value("Password").IsString Then
                        new_photo.Passwords.Add(kvp.Value("Password").ToString)
                    ElseIf kvp.Value("Password").IsArray Then
                        For Each p As String In kvp.Value("Password")
                            new_photo.Passwords.Add(p)
                        Next
                    End If

                End If
                all_photos.Add(new_photo)
            Next
            all_photos.Sort() 'sorted by datetimeoffset only!
            Dim sample_iterator As Iterator(Of GpsSample) = gps_samples.GetIterator()
            sample_iterator.MoveNext()
            Dim prev_sample As GpsSample = sample_iterator.Next
            For Each current_photo As PhotoInfo In all_photos
                'gps tag the photo if needed
                If current_photo.LatLng Is Nothing Then
                    Do Until sample_iterator.Current.Datetime >= current_photo.DTO AndAlso prev_sample.Datetime <= current_photo.DTO
                        prev_sample = sample_iterator.Next
                    Loop
                    current_photo.LatLng = prev_sample.Coordinate.IntermediatePoint(sample_iterator.Current.Coordinate,
                                                                                    (current_photo.DTO - prev_sample.Datetime).TotalSeconds /
                                                                                    (sample_iterator.Current.Datetime - prev_sample.Datetime).TotalSeconds)
                End If
            Next
            'TODO: later here we will sort by rank from the shootout
            Dim current_rank As Integer = 0
            Dim photo_info As New Json
            If IO.File.Exists(IO.Path.Combine(destination, "photo_info.json")) Then
                photo_info = Json.FromFile(IO.Path.Combine(destination, "photo_info.json"))
            End If
            'Convert photos to dictionary for speed
            Dim photo_dict As New Dictionary(Of String, Json)
            If photo_info.ContainsKey("photos") Then
                For Each j As Json In photo_info("photos")
                    photo_dict.Add(j(0).ToString, j)
                Next
            End If
            Dim passwords As New Dictionary(Of String, PasswordInfo)
            Dim AuxPasswords As New List(Of String)
            If IO.Directory.Exists(txtOutputDest.Text) Then
                For Each current_photo As PhotoInfo In all_photos
                    current_photo.Rank = current_rank
                    current_rank += 1
                    'write photo
                    Dim relative_path As String = current_photo.Filename.Substring(rootdir.Length + IO.Path.PathSeparator.ToString.Length)
                    Dim thumbnail_dest_file As String
                    If (relative_path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) OrElse
                        relative_path.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)) Then
                        thumbnail_dest_file = IO.Path.Combine(destination, "thumbnails", relative_path)
                    Else
                        thumbnail_dest_file = IO.Path.Combine(destination, "thumbnails", relative_path.Substring(0, relative_path.LastIndexOf(".")) + ".jpg")
                    End If
                    If Not IO.Directory.Exists(IO.Path.GetDirectoryName(thumbnail_dest_file)) Then
                        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(thumbnail_dest_file))
                    End If
                    Dim dest_file As String = IO.Path.Combine(destination, "photos", relative_path)
                    If (relative_path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) OrElse
                        relative_path.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase) OrElse
                        relative_path.EndsWith(".webm", StringComparison.CurrentCultureIgnoreCase)) Then
                        dest_file = IO.Path.Combine(destination, "photos", relative_path)
                    ElseIf (relative_path.EndsWith(".mov", StringComparison.CurrentCultureIgnoreCase) OrElse
                            relative_path.EndsWith(".avi", StringComparison.CurrentCultureIgnoreCase) OrElse
                            relative_path.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase)) Then
                        dest_file = IO.Path.Combine(destination, "photos", relative_path.Substring(0, relative_path.LastIndexOf(".")) + ".webm")
                    Else
                        Throw New ApplicationException("Unrecognized file extension" & relative_path)
                    End If
                    relative_path = relative_path.Replace(IO.Path.DirectorySeparatorChar, "/")
                    If Not IO.Directory.Exists(IO.Path.GetDirectoryName(dest_file)) Then
                        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(dest_file))
                    End If
                    Dim write_dest As Boolean
                    Dim write_thumbnail As Boolean
                    If current_photo.Passwords Is Nothing Then
                        write_dest = (ForceOverwrite OrElse
                                      Not IO.File.Exists(dest_file) OrElse
                                      IO.File.GetLastWriteTime(dest_file) < IO.File.GetLastWriteTime(current_photo.Filename))
                        write_thumbnail = (ForceOverwrite OrElse
                                           Not IO.File.Exists(thumbnail_dest_file) OrElse
                                           IO.File.GetLastWriteTime(thumbnail_dest_file) < IO.File.GetLastWriteTime(current_photo.Filename))
                    Else
                        For Each current_password As String In current_photo.Passwords
                            If Not passwords.ContainsKey(current_password) Then
                                Dim possible_pinfo As PasswordInfo = Nothing
                                If photo_info.ContainsKey("passwords") Then
                                    possible_pinfo = PasswordInfo.FromJsons(current_password, photo_info("passwords"))
                                End If
                                If possible_pinfo IsNot Nothing Then
                                    passwords.Add(current_password, possible_pinfo)
                                Else
                                    passwords.Add(current_password, New PasswordInfo(current_password))
                                End If
                            End If
                        Next
                        'can we figure out the current ImageKey?
                        Dim salts As New List(Of Byte())
                        If photo_info.ContainsKey("passwords") Then
                            For Each json_password As Json In photo_info("passwords")
                                salts.Add(StringToByteArray(json_password(0).ToString))
                            Next
                        End If
                        current_photo.ImageKey = GetImageKey(photo_dict(relative_path), passwords, AuxPasswords, salts)
                        write_dest = (ForceOverwrite OrElse
                                      Not IO.File.Exists(dest_file + ".aes") OrElse
                                      IO.File.GetLastWriteTime(dest_file + ".aes") < IO.File.GetLastWriteTime(current_photo.Filename) OrElse
                                      current_photo.ImageKey Is Nothing)
                        write_thumbnail = (ForceOverwrite OrElse
                                           Not IO.File.Exists(thumbnail_dest_file + ".aes") OrElse
                                           IO.File.GetLastWriteTime(thumbnail_dest_file + ".aes") < IO.File.GetLastWriteTime(current_photo.Filename) OrElse
                                           current_photo.ImageKey Is Nothing)
                        If write_dest Or write_thumbnail Then 'if we must update encrypted files, we much update both
                            write_dest = True
                            write_thumbnail = True
                        End If
                    End If
                    If write_dest Then
                        If (relative_path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) OrElse
                            relative_path.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)) Then
                            ResizeJpegMax(current_photo.Filename, dest_file, 1600, 1600, current_photo.Orientation)
                        ElseIf (relative_path.EndsWith(".avi", StringComparison.CurrentCultureIgnoreCase) OrElse
                                relative_path.EndsWith(".mov", StringComparison.CurrentCultureIgnoreCase)) Then
                            ' Start the child process.
                            Dim p As New Process()
                            ' Redirect the output stream of the child process.
                            p.StartInfo.UseShellExecute = False
                            p.StartInfo.RedirectStandardOutput = True
                            'p.StartInfo.RedirectStandardError = True
                            p.StartInfo.CreateNoWindow = True
                            p.StartInfo.FileName = IO.Path.Combine(IO.Directory.GetCurrentDirectory, "ffmpeg.exe")
                            p.StartInfo.Arguments = "-y -i """ & current_photo.Filename & """ """ & dest_file & """"
                            p.Start()
                            '' Do not wait for the child process to exit before
                            '' reading to the end of its redirected stream.
                            ''p.WaitForExit()
                            '' Read the output stream first and then wait.
                            Dim output As String = p.StandardOutput.ReadToEnd()
                            p.WaitForExit()
                            'Debug.WriteLine(output)
                            'output = p.StandardError.ReadToEnd()
                            'Debug.WriteLine(output)

                        Else
                            Throw New ApplicationException("Unknown file type " & relative_path)
                        End If
                    End If
                    If write_thumbnail Then
                        If (relative_path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) OrElse
                            relative_path.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)) Then
                            ResizeJpegMin(current_photo.Filename, thumbnail_dest_file, 48, 48, current_photo.Orientation)
                        ElseIf (relative_path.EndsWith(".avi", StringComparison.CurrentCultureIgnoreCase) OrElse
                                relative_path.EndsWith(".mov", StringComparison.CurrentCultureIgnoreCase)) Then
                            Dim b As Bitmap = GetFrameFromVideo(current_photo.Filename, 0.3)
                            ResizeJpegMin(b, thumbnail_dest_file, 48, 48, current_photo.Orientation)
                        Else
                            Throw New ApplicationException("Unknown file type " & relative_path)
                        End If
                    End If
                    'encrypt if needed
                    If current_photo.Passwords IsNot Nothing Then
                        If write_dest Then 'in this case, both write_dest and write_thumbnail will be the same
                            'encrypt the file with a random key

                            current_photo.ImageKey = AES_Encrypt_File(dest_file, dest_file + ".aes")
                            IO.File.Delete(dest_file) 'delete unencrypted dest
                            AES_Encrypt_File(thumbnail_dest_file, thumbnail_dest_file + ".aes", current_photo.ImageKey)
                            IO.File.Delete(thumbnail_dest_file) 'delete unencrypted thumbnail
                        End If
                        'encrypt the ImageKey with each password
                        current_photo.EncryptedImageKeys = New List(Of Tuple(Of Byte(), Byte()))
                        For Each current_password As String In current_photo.Passwords
                            Dim ImageKeyEncrypted As Byte() = AES_ECB_Encrypt(passwords(current_password).Key, current_photo.ImageKey)
                            current_photo.ImageKey(0) = CType((current_photo.ImageKey(0) + 1) Mod 256, Byte)
                            Dim ImageKeyPlus1Encrypted As Byte() = AES_ECB_Encrypt(passwords(current_password).Key, current_photo.ImageKey)
                            current_photo.ImageKey(0) = CType((current_photo.ImageKey(0) + 255) Mod 256, Byte) 'put it back
                            current_photo.EncryptedImageKeys.Add(New Tuple(Of Byte(), Byte())(ImageKeyEncrypted, ImageKeyPlus1Encrypted))
                        Next
                    End If
                    If photo_info Is Nothing Then
                        photo_info = New Json
                    End If
                    If Not photo_info.ContainsKey("photos") Then
                        photo_info.Add("photos", New Json)
                    End If
                    Dim new_photo_info As Json = current_photo.ToJson(relative_path)
                    If Not photo_dict.ContainsKey(new_photo_info(0).ToString) Then
                        photo_dict.Add(new_photo_info(0).ToString, new_photo_info)
                    Else
                        'completely replace the old one with the new one, including passwords
                        photo_dict(new_photo_info(0).ToString) = new_photo_info
                    End If
                Next
                'convert dict back to photo_info
                photo_info("photos") = New Json(photo_dict.Values.ToArray)
                photo_info.Remove("passwords")
                If passwords.Count > 0 Then
                    photo_info.Add("passwords", New Json)
                    For Each pinfo As PasswordInfo In passwords.Values
                        photo_info("passwords").Add(pinfo.ToJson())
                    Next
                End If
            End If
            IO.File.WriteAllLines(IO.Path.Combine(destination, "photo_info.json"), photo_info.ToJson("", "  "))
        End If
    End Sub

    Function GetImageKey(photo_info As Json, passwords As Dictionary(Of String, PasswordInfo), AuxPasswords As List(Of String), salts As List(Of Byte())) As Byte()
        If photo_info.Count < 6 Then
            Return Nothing
        End If
        For Each current_pinfo As PasswordInfo In passwords.Values
            For Each ImageKeyPair As Json In photo_info(5)
                Dim ImageKeyEncrypted As Byte() = CryptoHelpers.StringToByteArray(ImageKeyPair(0).ToString)
                Dim ImageKeyPlus1Encrypted As Byte() = CryptoHelpers.StringToByteArray(ImageKeyPair(1).ToString)
                Dim ImageKey As Byte() = AES_ECB_Decrypt(current_pinfo.Key, ImageKeyEncrypted)
                Dim ImageKeyPlus1 As Byte() = AES_ECB_Decrypt(current_pinfo.Key, ImageKeyPlus1Encrypted)
                ImageKeyPlus1(0) = CType((ImageKeyPlus1(0) + 255) Mod 256, Byte)
                If CryptoHelpers.ByteArrayToString(ImageKey) = CryptoHelpers.ByteArrayToString(ImageKeyPlus1) Then
                    Return ImageKey
                End If
            Next
        Next
        For Each PossiblePassword As String In AuxPasswords
            For Each salt As Byte() In salts
                Dim current_pinfo As PasswordInfo = New PasswordInfo(PossiblePassword, salt)
                For Each ImageKeyPair As Json In photo_info(5)
                    Dim ImageKeyEncrypted As Byte() = CryptoHelpers.StringToByteArray(ImageKeyPair(0).ToString)
                    Dim ImageKeyPlus1Encrypted As Byte() = CryptoHelpers.StringToByteArray(ImageKeyPair(1).ToString)
                    Dim ImageKey As Byte() = AES_ECB_Decrypt(current_pinfo.Key, ImageKeyEncrypted)
                    Dim ImageKeyPlus1 As Byte() = AES_ECB_Decrypt(current_pinfo.Key, ImageKeyPlus1Encrypted)
                    ImageKeyPlus1(0) = CType((ImageKeyPlus1(0) + 255) Mod 256, Byte)
                    If CryptoHelpers.ByteArrayToString(ImageKey) = CryptoHelpers.ByteArrayToString(ImageKeyPlus1) Then
                        Return ImageKey
                    End If
                Next
            Next
        Next
        Do
            Dim PossiblePassword As String = Interaction.InputBox("Input possible password")
            If PossiblePassword <> "" Then
                For Each salt As Byte() In salts
                    Dim current_pinfo As PasswordInfo = New PasswordInfo(PossiblePassword, salt)
                    If Not AuxPasswords.Contains(PossiblePassword) Then
                        AuxPasswords.Add(PossiblePassword)
                    End If
                    For Each ImageKeyPair As Json In photo_info(5)
                        Dim ImageKeyEncrypted As Byte() = CryptoHelpers.StringToByteArray(ImageKeyPair(0).ToString)
                        Dim ImageKeyPlus1Encrypted As Byte() = CryptoHelpers.StringToByteArray(ImageKeyPair(1).ToString)
                        Dim ImageKey As Byte() = AES_ECB_Decrypt(current_pinfo.Key, ImageKeyEncrypted)
                        Dim ImageKeyPlus1 As Byte() = AES_ECB_Decrypt(current_pinfo.Key, ImageKeyPlus1Encrypted)
                        ImageKeyPlus1(0) = CType((ImageKeyPlus1(0) + 255) Mod 256, Byte)
                        If CryptoHelpers.ByteArrayToString(ImageKey) = CryptoHelpers.ByteArrayToString(ImageKeyPlus1) Then
                            Return ImageKey
                        End If
                    Next
                Next
            Else
                Exit Do
            End If
        Loop
        Return Nothing
    End Function

    Function PasswordsMatch(password_jsons As Json, photo_info As Json, passwords As List(Of String)) As Boolean
        If photo_info.Count < 6 OrElse passwords.Count <> photo_info(5).Count Then
            Return False
        End If
        Dim success_count(0 To passwords.Count - 1) As Byte
        For i As Integer = 0 To success_count.Length - 1
            success_count(i) = 0
        Next
        For password_index As Integer = 0 To passwords.Count - 1
            Dim password As String = passwords(password_index)
            For Each password_json As Json In password_jsons
                Dim salt As Byte() = CryptoHelpers.StringToByteArray(password_json(0).ToString)
                Dim RandomBytesEncrypted As Byte() = CryptoHelpers.StringToByteArray(password_json(1).ToString)
                Dim RandomBytesPlus1Encrypted As Byte() = CryptoHelpers.StringToByteArray(password_json(2).ToString)
                Dim current_pinfo As New PasswordInfo(password, salt)
                Dim RandomBytes As Byte() = AES_ECB_Decrypt(current_pinfo.Key, RandomBytesEncrypted)
                Dim RandomBytesPlus1 As Byte() = AES_ECB_Decrypt(current_pinfo.Key, RandomBytesPlus1Encrypted)
                RandomBytes(0) = CType((RandomBytes(0) + 1) Mod 256, Byte)
                If CryptoHelpers.ByteArrayToString(RandomBytes) = CryptoHelpers.ByteArrayToString(RandomBytesPlus1) Then
                    'This password is valid for something!
                    Dim ImageKeyPair As Json = photo_info(5)(password_index)
                    Dim ImageKeyEncrypted As Byte() = CryptoHelpers.StringToByteArray(ImageKeyPair(0).ToString)
                    Dim ImageKeyPlus1Encrypted As Byte() = CryptoHelpers.StringToByteArray(ImageKeyPair(1).ToString)
                    Dim ImageKey As Byte() = AES_ECB_Decrypt(current_pinfo.Key, ImageKeyEncrypted)
                    Dim ImageKeyPlus1 As Byte() = AES_ECB_Decrypt(current_pinfo.Key, ImageKeyPlus1Encrypted)
                    ImageKey(0) = CType((ImageKey(0) + 1) Mod 256, Byte)
                    If CryptoHelpers.ByteArrayToString(ImageKey) <> CryptoHelpers.ByteArrayToString(ImageKeyPlus1) Then
                        'this password fails for this key
                        Return False
                    End If
                End If
            Next
        Next
        Return True
    End Function

    Private Sub btnOutputSrcFile_Click(sender As Object, e As EventArgs) Handles btnOutputSrcFile.Click
        SourceFile_Click(sender, e, txtOutputSrc)
    End Sub
#End Region
#Region "Shootout"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim s As New Shootout(Of Integer)

        s.SetLesser(0, 1)
        s.SetLesser(1, 2)
        s.SetLesser(3, 4)
        s.SetLesser(4, 5)
        s.SetLesser(0, 3)
        s.SetLesser(1, 4)
        s.SetLesser(2, 5)
        s.SetLesser(2, 3)

        's.SetLessThanOrEqual(5, 0)
        s.Trim()
        MsgBox(s.Compare(0, 2))
    End Sub
#End Region
End Class
