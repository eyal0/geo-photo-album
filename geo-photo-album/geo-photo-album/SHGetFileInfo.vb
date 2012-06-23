Imports System.Runtime.InteropServices
Module SHGetFileInfo
    Public Declare Auto Function SHGetFileInfo Lib "shell32.dll" (ByVal pszPath As String,
                                                                  ByVal dwFileAttributes As Integer,
                                                                  ByRef psfi As SHFILEINFO,
                                                                  ByVal cbFileInfo As Integer,
                                                                  ByVal uFlags As Integer) As IntPtr

    Public Const SHGFI_ICON As Integer = &H100
    Public Const SHGFI_SMALLICON As Integer = &H1
    Public Const SHGFI_LARGEICON As Integer = &H0
    Public Const SHGFI_OPENICON As Integer = &H2

    Structure SHFILEINFO
        Public hIcon As IntPtr
        Public iIcon As Integer
        Public dwAttributes As Integer
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=260)> _
        Public szDisplayName As String
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=80)> _
        Public szTypeName As String
    End Structure

    ' GetShellIconAsImage
    Function GetShellIconAsImage(ByVal argPath As String) As Image
        Dim mShellFileInfo As New SHFILEINFO
        mShellFileInfo.szDisplayName = New String(Chr(0), 260)
        mShellFileInfo.szTypeName = New String(Chr(0), 80)
        SHGetFileInfo(argPath, 0, mShellFileInfo, System.Runtime.InteropServices.Marshal.SizeOf(mShellFileInfo), SHGFI_ICON Or SHGFI_SMALLICON)
        ' attempt to create a System.Drawing.Icon from the icon handle that was returned in the SHFILEINFO structure
        Dim mIcon As System.Drawing.Icon
        Dim mImage As System.Drawing.Image
        Try
            mIcon = System.Drawing.Icon.FromHandle(mShellFileInfo.hIcon)
            mImage = mIcon.ToBitmap
        Catch ex As Exception
            ' for some reason the icon could not be converted so create a blank System.Drawing.Image to return instead
            mImage = New System.Drawing.Bitmap(16, 16)
        End Try
        ' return the final System.Drawing.Image
        Return mImage
    End Function
End Module
