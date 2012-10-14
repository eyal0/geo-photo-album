Module ResizeJpeg
    Public Sub ResizeJpegMin(ByVal imgPhoto As System.Drawing.Image, filePathDestination As String, ByVal minWidth As Integer, ByVal minHeight As Integer, orientation As Integer)
        Select Case orientation
            Case 1
                imgPhoto.RotateFlip(RotateFlipType.RotateNoneFlipNone)
            Case 2
                imgPhoto.RotateFlip(RotateFlipType.RotateNoneFlipX)
            Case 3
                imgPhoto.RotateFlip(RotateFlipType.Rotate180FlipNone)
            Case 4
                imgPhoto.RotateFlip(RotateFlipType.Rotate180FlipX)
            Case 5
                imgPhoto.RotateFlip(RotateFlipType.Rotate90FlipX)
            Case 6
                imgPhoto.RotateFlip(RotateFlipType.Rotate90FlipNone)
            Case 7
                imgPhoto.RotateFlip(RotateFlipType.Rotate270FlipX)
            Case 8
                imgPhoto.RotateFlip(RotateFlipType.Rotate270FlipNone)
        End Select
        Dim sourceWidth As Integer = imgPhoto.Width
        Dim sourceHeight As Integer = imgPhoto.Height
        Dim nPercentW As Double = (CType(minWidth, Double) / CType(sourceWidth, Double))
        Dim nPercentH As Double = (CType(minHeight, Double) / CType(sourceHeight, Double))
        'if we have to pad the height pad both the top and the bottom
        'with the difference between the scaled height and the desired height
        Dim nPercent As Double = Math.Max(nPercentH, nPercentW)
        Dim destWidth As Integer = CType((sourceWidth * nPercent), Integer)
        Dim destHeight As Integer = CType((sourceHeight * nPercent), Integer)
        ResizeJpeg(imgPhoto, filePathDestination, destWidth, destHeight, orientation)
    End Sub

    Public Sub ResizeJpegMin(ByVal filePathSource As String, filePathDestination As String, ByVal minWidth As Integer, ByVal minHeight As Integer, orientation As Integer)
        Dim imgPhoto As System.Drawing.Image = System.Drawing.Image.FromFile(filePathSource)
        ResizeJpegMin(imgPhoto, filePathDestination, minWidth, minHeight, orientation)
    End Sub

    Public Sub ResizeJpegMax(ByVal filePathSource As String, filePathDestination As String, ByVal maxWidth As Integer, ByVal maxHeight As Integer, orientation As Integer)
        Dim imgPhoto As System.Drawing.Image = System.Drawing.Image.FromFile(filePathSource)
        Select Case orientation
            Case 1
                imgPhoto.RotateFlip(RotateFlipType.RotateNoneFlipNone)
            Case 2
                imgPhoto.RotateFlip(RotateFlipType.RotateNoneFlipX)
            Case 3
                imgPhoto.RotateFlip(RotateFlipType.Rotate180FlipNone)
            Case 4
                imgPhoto.RotateFlip(RotateFlipType.Rotate180FlipX)
            Case 5
                imgPhoto.RotateFlip(RotateFlipType.Rotate90FlipX)
            Case 6
                imgPhoto.RotateFlip(RotateFlipType.Rotate90FlipNone)
            Case 7
                imgPhoto.RotateFlip(RotateFlipType.Rotate270FlipX)
            Case 8
                imgPhoto.RotateFlip(RotateFlipType.Rotate270FlipNone)
        End Select
        Dim sourceWidth As Integer = imgPhoto.Width
        Dim sourceHeight As Integer = imgPhoto.Height
        Dim nPercentW As Double = (CType(maxWidth, Double) / CType(sourceWidth, Double))
        Dim nPercentH As Double = (CType(maxHeight, Double) / CType(sourceHeight, Double))
        'if we have to pad the height pad both the top and the bottom
        'with the difference between the scaled height and the desired height
        Dim nPercent As Double = Math.Min(nPercentH, nPercentW)
        Dim destWidth As Integer = CType((sourceWidth * nPercent), Integer)
        Dim destHeight As Integer = CType((sourceHeight * nPercent), Integer)
        ResizeJpeg(imgPhoto, filePathDestination, destWidth, destHeight, orientation)
    End Sub

    Private Sub ResizeJpeg(ByVal imgPhoto As System.Drawing.Image, filePathDestination As String, ByVal destWidth As Integer, ByVal destHeight As Integer, orientation As Integer)
        Dim destLeft As Integer = 0
        Dim destTop As Integer = 0

        Dim bmPhoto As Bitmap = New Bitmap(imgPhoto, destWidth, destHeight)
        bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution)
        Dim grPhoto As Graphics = Graphics.FromImage(bmPhoto)
        grPhoto.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        grPhoto.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        grPhoto.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        Dim destRect As Rectangle = New Rectangle(destLeft, destTop, destWidth, destHeight)
        grPhoto.DrawImage(imgPhoto, destRect, New Rectangle(0, 0, imgPhoto.Width, imgPhoto.Height), GraphicsUnit.Pixel)
        grPhoto.Dispose()
        Try
            For Each propertyItem As Imaging.PropertyItem In imgPhoto.PropertyItems
                If propertyItem.Id = ExifLib.ExifTags.Orientation AndAlso propertyItem.Value(0) <> 1 Then
                    propertyItem.Value(0) = 1
                End If
                Try
                    bmPhoto.SetPropertyItem(propertyItem)
                Catch
                    'nothing
                End Try
            Next
        Catch
            'nothing
        End Try
        Dim enc As System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.Quality
        Dim encParms As System.Drawing.Imaging.EncoderParameters = New Imaging.EncoderParameters(1)
        Dim encParm As System.Drawing.Imaging.EncoderParameter = New Imaging.EncoderParameter(enc, 100L)
        encParms.Param(0) = encParm
        Dim codecInfo() As Imaging.ImageCodecInfo = Imaging.ImageCodecInfo.GetImageEncoders
        Dim codecInfoJpeg As Imaging.ImageCodecInfo = codecInfo(1)
        bmPhoto.Save(filePathDestination, codecInfoJpeg, encParms)
        bmPhoto.Dispose()
        imgPhoto.Dispose()
    End Sub

End Module
