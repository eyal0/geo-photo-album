Module WinStructs
    ''' <summary>
    ''' The VIDEOINFOHEADER structure describes the bitmap and color information for a video image
    ''' </summary>
    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)> _
    Public Structure VIDEOINFOHEADER
        ''' <summary>RECT structure that specifies the source video window. This structure can be a clipping rectangle, to select a portion of the source video stream.</summary>
        Public rcSource As RECT
        ''' <summary>RECT structure that specifies the destination video window.</summary>
        Public rcTarget As RECT
        ''' <summary>Approximate data rate of the video stream, in bits per second</summary>
        Public dwBitRate As UInteger
        ''' <summary>Data error rate, in bit errors per second</summary>
        Public dwBitErrorRate As UInteger
        ''' <summary>The desired average display time of the video frames, in 100-nanosecond units. The actual time per frame may be longer. See Remarks.</summary>
        Public AvgTimePerFrame As Long
        ''' <summary>BITMAPINFOHEADER structure that contains color and dimension information for the video image bitmap. If the format block contains a color table or color masks, they immediately follow the bmiHeader member. You can get the first color entry by casting the address of member to a BITMAPINFO pointer</summary>
        Public bmiHeader As BITMAPINFOHEADER
    End Structure

    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)> _
    Public Structure RECT
        Private left As Integer
        Private top As Integer
        Private right As Integer
        Private bottom As Integer
    End Structure

    ''' <summary>
    ''' The BITMAPINFOHEADER structure contains information about the dimensions and color format of a device-independent bitmap (DIB). 
    ''' SEE MSDN
    ''' </summary>
    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)> _
    Public Structure BITMAPINFOHEADER
        ''' <summary>Specifies the number of bytes required by the structure. This value does not include the size of the color table or the size of the color masks, if they are appended to the end of structure. See Remarks.</summary>
        Public biSize As UInteger
        ''' <summary>Specifies the width of the bitmap, in pixels. For information about calculating the stride of the bitmap, see Remarks.</summary>
        Public biWidth As Integer
        ''' <summary>Specifies the height of the bitmap, in pixels. SEE MSDN</summary>
        Public biHeight As Integer
        ''' <summary>Specifies the number of planes for the target device. This value must be set to 1</summary>
        Public biPlanes As UShort
        ''' <summary>Specifies the number of bits per pixel (bpp).  For uncompressed formats, this value gives to the average number of bits per pixel. For compressed formats, this value gives the implied bit depth of the uncompressed image, after the image has been decoded.</summary>
        Public biBitCount As UShort
        ''' <summary>For compressed video and YUV formats, this member is a FOURCC code, specified as a DWORD in little-endian order. For example, YUYV video has the FOURCC 'VYUY' or 0x56595559. SEE MSDN</summary>
        Public biCompression As UInteger
        ''' <summary>Specifies the size, in bytes, of the image. This can be set to 0 for uncompressed RGB bitmaps</summary>
        Public biSizeImage As UInteger
        ''' <summary>Specifies the horizontal resolution, in pixels per meter, of the target device for the bitmap</summary>
        Public biXPelsPerMeter As Integer
        ''' <summary>Specifies the vertical resolution, in pixels per meter, of the target device for the bitmap</summary>
        Public biYPelsPerMeter As Integer
        ''' <summary>Specifies the number of color indices in the color table that are actually used by the bitmap. See Remarks for more information.</summary>
        Public biClrUsed As UInteger
        ''' <summary>Specifies the number of color indices that are considered important for displaying the bitmap. If this value is zero, all colors are important</summary>
        Public biClrImportant As UInteger
    End Structure
End Module
