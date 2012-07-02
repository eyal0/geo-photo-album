Public Module SHA1
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
End Module
