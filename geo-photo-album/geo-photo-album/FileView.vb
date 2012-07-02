Public Class FileView
    Inherits ListView

    Dim bw As Threading.Thread
    Property my_json As Json

    Function CacheShellIcon(ByVal argPath As String) As String
        Dim mKey As String = argPath
        ' check if an icon for this key has already been added to the collection
        If SmallImageList.Images.ContainsKey(mKey) = False Then
            Me.InvokeEx(Sub() Me.SmallImageList.Images.Add(mKey, GetShellIconAsImage(argPath)))
        End If
        If LargeImageList.Images.ContainsKey(mKey) = False Then
            Try
                Me.InvokeEx(Sub() Me.LargeImageList.Images.Add(mKey, New System.Drawing.Bitmap(argPath)))
            Catch ex As Exception
                Me.InvokeEx(Sub() Me.LargeImageList.Images.Add(mKey, GetShellIconAsImage(argPath)))
            End Try
        End If
        Return mKey
    End Function

    Structure LviInfo
        Dim fullname As String
        Dim text As String
        Dim hash As String
        Dim json As Json
        Dim json_string As String
        Dim IsDirectory As Boolean
    End Structure

    ReadOnly Property IsDirectory(index As Integer) As Boolean
        Get
            Return CType(Me.Items(index).Tag, LviInfo).IsDirectory
        End Get
    End Property

    ReadOnly Property Json(index As Integer) As Json
        Get
            Return CType(Me.Items(index).Tag, LviInfo).json
        End Get
    End Property

    ReadOnly Property Fullname(index As Integer) As String
        Get
            Return CType(Me.Items(index).Tag, LviInfo).fullname
        End Get
    End Property

    ReadOnly Property Hash(index As Integer) As String
        Get
            Return CType(Me.Items(index).Tag, LviInfo).hash
        End Get
    End Property

    Private Sub AddLvi(l As LviInfo)
        Dim lvi As New ListViewItem(l.text)
        lvi.ImageKey = CacheShellIcon(l.fullname)
        lvi.Tag = l
        If l.json IsNot Nothing Then
            lvi.SubItems.Add(l.json_string)
        End If
        Me.InvokeEx(Sub() Me.Items.Add(lvi))
    End Sub

    Private Sub UpdateFileTags(filter_text As String)
        Me.InvokeEx(Sub()
                        Me.Items.Clear()
                    End Sub)
        If (System.IO.Directory.Exists(filter_text) AndAlso
            (New System.IO.DirectoryInfo(filter_text).Attributes And IO.FileAttributes.ReparsePoint) = 0) Then
            If System.IO.Directory.GetParent(filter_text) IsNot Nothing Then
                Dim l As New LviInfo
                l.fullname = System.IO.Directory.GetParent(filter_text).FullName
                l.text = ".."
                l.IsDirectory = True
                AddLvi(l)
            End If
            For Each d As String In System.IO.Directory.EnumerateDirectories(filter_text)
                If bw.ThreadState = Threading.ThreadState.AbortRequested Then Exit Sub
                Dim d_info As New System.IO.DirectoryInfo(d)
                If (d_info.Attributes And IO.FileAttributes.ReparsePoint) = 0 Then
                    Dim l As New LviInfo
                    l.text = System.IO.Path.GetFileName(d)
                    l.fullname = d_info.FullName
                    l.IsDirectory = True
                    AddLvi(l)
                End If
            Next
            Dim json_files As Json = Nothing
            If my_json.ContainsKey("Tags") Then
                json_files = my_json("Tags")
            End If
            For Each f As String In System.IO.Directory.EnumerateFiles(filter_text)
                If bw.ThreadState = Threading.ThreadState.AbortRequested Then
                    Exit Sub
                End If
                Dim l As New LviInfo
                l.text = System.IO.Path.GetFileName(f)
                l.fullname = f
                l.IsDirectory = False
                l.hash = SHA1CalcFile(f)
                If json_files IsNot Nothing Then
                    If json_files.ContainsKey(l.hash) Then
                        l.json = json_files(l.hash)
                        l.json_string = l.json.ToString
                    End If
                End If
                AddLvi(l)
            Next
        End If
    End Sub

    WriteOnly Property Filter As String
        Set(value As String)
            If bw IsNot Nothing Then
                bw.Abort()
                Do While bw.IsAlive
                    Application.DoEvents()
                Loop
            End If
            bw = New Threading.Thread(New Threading.ParameterizedThreadStart(Sub(o As Object)
                                                                                 UpdateFileTags(DirectCast(o, String))
                                                                             End Sub))
            bw.Start(value)
        End Set
    End Property

    Private Sub Me_MouseDoubleClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick
        Dim s As String = CType(Me.FocusedItem.Tag, LviInfo).fullname
        If System.IO.Directory.Exists(s) Then
            Do While bw.IsAlive
                bw.Abort()
                Application.DoEvents()
            Loop
            Me.Filter = s
        End If
    End Sub
End Class
