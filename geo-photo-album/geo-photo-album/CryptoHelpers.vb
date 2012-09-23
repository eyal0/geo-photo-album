Public Module CryptoHelpers
    ' utility function to convert a byte array into a hex string
    Public Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)
        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next
        Return sb.ToString().ToLower
    End Function

    Public Function StringToByteArray(ByVal s As String) As Byte()
        If s.Length Mod 2 <> 0 Then
            Throw New ArgumentException("string must have even length")
        End If
        Dim ret(0 To CType(s.Length / 2, Integer) - 1) As Byte
        For i As Integer = 0 To ret.Count - 1 Step 1
            ret(i) = Convert.ToByte(s.Substring(i * 2, 2), 16)
        Next
        Return ret
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

    'returns the key that was used for encryption
    Function AES_Encrypt_File(SourceFilename As String, DestinationFilename As String, Optional ImageKey As Byte() = Nothing) As Byte()
        Dim AES As New System.Security.Cryptography.AesManaged
        AES.Mode = Security.Cryptography.CipherMode.CBC
        If ImageKey IsNot Nothing Then
            AES.Key = ImageKey
        Else
            AES.KeySize = 128 'will generate a random key
        End If
        Dim Encryptor As Security.Cryptography.ICryptoTransform = AES.CreateEncryptor
        Dim plain_bytes As Byte() = System.IO.File.ReadAllBytes(SourceFilename)
        Dim cipher_bytes As Byte() = Encryptor.TransformFinalBlock(plain_bytes, 0, plain_bytes.Length)
        Dim output_bytes(0 To cipher_bytes.Length + AES.IV.Length - 1) As Byte
        Buffer.BlockCopy(AES.IV, 0, output_bytes, 0, AES.IV.Length)
        Buffer.BlockCopy(cipher_bytes, 0, output_bytes, AES.IV.Length, cipher_bytes.Length)
        IO.File.WriteAllBytes(DestinationFilename, output_bytes)
        Return AES.Key
    End Function

    Function AES_ECB_Encrypt(Key As Byte(), Input As Byte(), Optional iv As Byte() = Nothing, Optional start As Integer = 0, Optional length As Integer = -1) As Byte()
        If length = -1 Then
            length = Input.Length - start
        End If
        Dim AES As New System.Security.Cryptography.AesManaged
        AES.Key = Key
        AES.Mode = Security.Cryptography.CipherMode.ECB
        AES.Padding = Security.Cryptography.PaddingMode.None
        If iv IsNot Nothing Then
            AES.IV = iv
        End If
        Dim Encryptor As Security.Cryptography.ICryptoTransform = AES.CreateEncryptor()
        Dim Output As Byte() = Encryptor.TransformFinalBlock(Input, start, length)
        Dim IVandOutput(0 To Output.Length + AES.IV.Length - 1) As Byte
        Buffer.BlockCopy(AES.IV, 0, IVandOutput, 0, AES.IV.Length)
        Buffer.BlockCopy(Output, 0, IVandOutput, AES.IV.Length, Output.Length)
        Return IVandOutput
    End Function


    'Function AES_ECB_Encrypt(Key As Byte(), Input As Byte(), Optional start As Integer = 0, Optional length As Integer = -1) As Byte()
    '    If length = -1 Then
    '        length = Input.Length - start
    '    End If
    '    Dim AES As New System.Security.Cryptography.AesManaged
    '    AES.Key = Key
    '    AES.Mode = Security.Cryptography.CipherMode.ECB
    '    AES.Padding = Security.Cryptography.PaddingMode.None
    '    Dim Encryptor As Security.Cryptography.ICryptoTransform = AES.CreateEncryptor()
    '    Dim Output As Byte() = Encryptor.TransformFinalBlock(Input, start, length)
    '    Dim IVandOutput(0 To Output.Length + AES.IV.Length - 1) As Byte
    '    Buffer.BlockCopy(AES.IV, 0, IVandOutput, 0, AES.IV.Length)
    '    Buffer.BlockCopy(Output, 0, IVandOutput, AES.IV.Length, Output.Length)
    '    Return IVandOutput
    'End Function

    Function AES_ECB_Decrypt(Key As Byte(), Input As Byte(), Optional start As Integer = 0, Optional length As Integer = -1) As Byte()
        If length = -1 Then
            length = Input.Length - start
        End If
        Dim AES As New System.Security.Cryptography.AesManaged
        AES.Key = Key
        AES.Mode = Security.Cryptography.CipherMode.ECB
        AES.Padding = Security.Cryptography.PaddingMode.None
        Buffer.BlockCopy(Input, start, AES.IV, 0, AES.IV.Length)
        start += AES.IV.Length
        length -= AES.IV.Length
        Dim Decryptor As Security.Cryptography.ICryptoTransform = AES.CreateDecryptor()
        Dim Output As Byte() = Decryptor.TransformFinalBlock(Input, start, length)
        Return Output
    End Function
End Module
