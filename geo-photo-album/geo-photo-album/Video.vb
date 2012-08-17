Imports DexterLib

Module Video
    Public Function GetFrameFromVideo(videoFile As String, percentagePosition As Double) As Bitmap
        If percentagePosition > 1 OrElse percentagePosition < 0 Then
            Throw New ArgumentOutOfRangeException("percentagePosition", percentagePosition, "Valid range is 0.0 .. 1.0")
        End If

        Try
            Dim mediaDet As MediaDet = Nothing
            Dim mediaType As _AMMediaType = Nothing
            If openVideoStream(videoFile, mediaDet, mediaType) Then
                Dim streamLength As Double = mediaDet.StreamLength

                'calculates the REAL target size of our frame
                Dim target As Size = GetVideoSize(mediaType)

                Dim s As Size = GetVideoSize(videoFile)
                'equal to sizeof(CommonClasses.BITMAPINFOHEADER);
                Dim bmpinfoheaderSize As Integer = 40

                'get size for buffer
                Dim bufferSize As Integer = (((s.Width * s.Height) * 24) \ 8) + bmpinfoheaderSize
                'equal to mediaDet.GetBitmapBits
                '    (0d, ref bufferSize, ref *buffer, target.Width, target.Height);    

                'allocates enough memory to store the frame
                Dim framebuffer(bufferSize) As Byte

                Dim pinnedArray As System.Runtime.InteropServices.GCHandle = System.Runtime.InteropServices.GCHandle.Alloc(framebuffer, Runtime.InteropServices.GCHandleType.Pinned)
                Dim pointer As IntPtr = pinnedArray.AddrOfPinnedObject()

                'gets bitmap, save in frameBuffer2
                mediaDet.GetBitmapBits(streamLength * percentagePosition, bufferSize, framebuffer(0), target.Width, target.Height)

                'now in buffer2 we have a BITMAPINFOHEADER structure 
                'followed by the DIB bits
                Dim bmp As New Bitmap(target.Width, target.Height, target.Width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, pinnedArray.AddrOfPinnedObject + bmpinfoheaderSize)

                pinnedArray.Free()

                bmp.RotateFlip(RotateFlipType.Rotate180FlipX)
                Return bmp
            End If
        Catch ex As System.Runtime.InteropServices.COMException
            Throw New InvalidVideoFileException(getErrorMsg(CUInt(ex.ErrorCode)), ex)
        End Try

        Throw New InvalidVideoFileException("No video stream was found")
    End Function

    ''' <summary>
    ''' Returns a description for errorCode passed. Only errors of type VFW (defined in vfwmsgs.h)
    ''' </summary>
    ''' <param name="errorCode"></param>
    ''' <returns></returns>
    Private Function getErrorMsg(errorCode As UInteger) As String
        Dim errorMsg As String = Nothing
        Select Case errorCode
            Case &H80040200UI
                'VFW_E_INVALIDMEDIATYPE
                errorMsg = "An invalid media type was specified"
                Exit Select
            Case &H80040201UI
                'VFW_E_INVALIDSUBTYPE
                errorMsg = "An invalid media subtype was specified"
                Exit Select
            Case &H80040202UI
                'VFW_E_NEED_OWNER
                errorMsg = "This object can only be created as an aggregated object"
                Exit Select
            Case &H80040203UI
                'VFW_E_ENUM_OUT_OF_SYNC
                errorMsg = "The enumerator has become invalid"
                Exit Select
            Case &H80040204UI
                'VFW_E_ALREADY_CONNECTED
                errorMsg = "At least one of the pins involved in the operation is already connected"
                Exit Select
            Case &H80040205UI
                'VFW_E_FILTER_ACTIVE
                errorMsg = "This operation cannot be performed because the filter is active"
                Exit Select
            Case &H80040206UI
                'VFW_E_NO_TYPES
                errorMsg = "One of the specified pins supports no media types"
                Exit Select
            Case &H80040207UI
                'VFW_E_NO_ACCEPTABLE_TYPES
                errorMsg = "There is no common media type between these pins"
                Exit Select
            Case &H80040208UI
                'VFW_E_INVALID_DIRECTION
                errorMsg = "Two pins of the same direction cannot be connected together"
                Exit Select
            Case &H80040209UI
                'VFW_E_NOT_CONNECTED
                errorMsg = "The operation cannot be performed because the pins are not connected"
                Exit Select
            Case &H80040210UI
                'VFW_E_NO_ALLOCATOR
                errorMsg = "No sample buffer allocator is available"
                Exit Select
            Case &H80040211UI
                'VFW_E_NOT_COMMITTED
                errorMsg = "Cannot allocate a sample when the allocator is not active"
                Exit Select
            Case &H80040212UI
                'VFW_E_SIZENOTSET
                errorMsg = "Cannot allocate memory because no size has been set"
                Exit Select
            Case &H80040213UI
                'VFW_E_NO_CLOCK
                errorMsg = "Cannot lock for synchronization because no clock has been defined"
                Exit Select
            Case &H80040214UI
                'VFW_E_NO_SINK
                errorMsg = "Quality messages could not be sent because no quality sink has been defined"
                Exit Select
            Case &H80040215UI
                'VFW_E_NO_INTERFACE
                errorMsg = "A required interface has not been implemented"
                Exit Select
            Case &H80040216UI
                'VFW_E_NOT_FOUND
                errorMsg = "An object or name was not found"
                Exit Select
            Case &H80040217UI
                'VFW_E_CANNOT_CONNECT
                errorMsg = "No combination of intermediate filters could be found to make the connection"
                Exit Select
            Case &H80040218UI
                'VFW_E_CANNOT_RENDER
                errorMsg = "No combination of filters could be found to render the stream"
                Exit Select
            Case &H80040219UI
                'VFW_E_CHANGING_FORMAT
                errorMsg = "Could not change formats dynamically"
                Exit Select
            Case &H80040220UI
                'VFW_E_NO_COLOR_KEY_SET
                errorMsg = "No color key has been set"
                Exit Select
            Case &H80040221UI
                'VFW_E_NO_DISPLAY_PALETTE
                errorMsg = "Display does not use a palette"
                Exit Select
            Case &H80040222UI
                'VFW_E_TOO_MANY_COLORS
                errorMsg = "Too many colors for the current display settings"
                Exit Select
            Case &H80040223UI
                'VFW_E_STATE_CHANGED
                errorMsg = "The state changed while waiting to process the sample"
                Exit Select
            Case &H80040224UI
                'VFW_E_NOT_STOPPED
                errorMsg = "The operation could not be performed because the filter is not stopped"
                Exit Select
            Case &H80040225UI
                'VFW_E_NOT_PAUSED
                errorMsg = "The operation could not be performed because the filter is not paused"
                Exit Select
            Case &H80040226UI
                'VFW_E_NOT_RUNNING
                errorMsg = "The operation could not be performed because the filter is not running"
                Exit Select
            Case &H80040227UI
                'VFW_E_WRONG_STATE
                errorMsg = "The operation could not be performed because the filter is in the wrong state"
                Exit Select
            Case &H80040228UI
                'VFW_E_START_TIME_AFTER_END
                errorMsg = "The sample start time is after the sample end time"
                Exit Select
            Case &H80040229UI
                'VFW_E_INVALID_RECT
                errorMsg = "The supplied rectangle is invalid"
                Exit Select
            Case &H80040230UI
                'VFW_E_TYPE_NOT_ACCEPTED
                errorMsg = "This pin cannot use the supplied media type"
                Exit Select
            Case &H80040231UI
                'VFW_E_CIRCULAR_GRAPH
                errorMsg = "The filter graph is circular"
                Exit Select
            Case &H80040232UI
                'VFW_E_NOT_ALLOWED_TO_SAVE
                errorMsg = "Updates are not allowed in this state"
                Exit Select
            Case &H80040233UI
                'VFW_E_TIME_ALREADY_PASSED
                errorMsg = "An attempt was made to queue a command for a time in the past"
                Exit Select
            Case &H80040234UI
                'VFW_E_ALREADY_CANCELLED
                errorMsg = "The queued command has already been canceled"
                Exit Select
            Case &H80040235UI
                'VFW_E_CORRUPT_GRAPH_FILE
                errorMsg = "Cannot render the file because it is corrupt"
                Exit Select
            Case &H80040236UI
                'VFW_E_ADVISE_ALREADY_SET
                errorMsg = "An overlay advise link already exists"
                Exit Select
            Case &H40237
                'VFW_S_STATE_INTERMEDIATE
                errorMsg = "The state transition has not completed"
                Exit Select
            Case &H80040239UI
                'VFW_E_NO_MODEX_AVAILABLE
                errorMsg = "This Advise cannot be canceled because it was not successfully set"
                Exit Select
            Case &H80040240UI
                'VFW_E_NO_FULLSCREEN
                errorMsg = "The media type of this file is not recognized"
                Exit Select
            Case &H80040241UI
                'VFW_E_CANNOT_LOAD_SOURCE_FILTER
                errorMsg = "The source filter for this file could not be loaded"
                Exit Select
            Case &H40242
                'VFW_S_PARTIAL_RENDER
                errorMsg = "Some of the streams in this movie are in an unsupported format"
                Exit Select
            Case &H80040243UI
                'VFW_E_FILE_TOO_SHORT
                errorMsg = "A file appeared to be incomplete"
                Exit Select
            Case &H80040244UI
                'VFW_E_INVALID_FILE_VERSION
                errorMsg = "The version number of the file is invalid"
                Exit Select
            Case &H40245
                'VFW_S_SOME_DATA_IGNORED
                errorMsg = "The file contained some property settings that were not used"
                Exit Select
            Case &H40246
                'VFW_S_CONNECTIONS_DEFERRED
                errorMsg = "Some connections have failed and have been deferred"
                Exit Select
            Case &H40103
                'VFW_E_INVALID_CLSID
                errorMsg = "A registry entry is corrupt"
                Exit Select
            Case &H80040249UI
                'VFW_E_SAMPLE_TIME_NOT_SET
                errorMsg = "No time stamp has been set for this sample"
                Exit Select
            Case &H40250
                'VFW_S_RESOURCE_NOT_NEEDED
                errorMsg = "The resource specified is no longer needed"
                Exit Select
            Case &H80040251UI
                'VFW_E_MEDIA_TIME_NOT_SET
                errorMsg = "No media time stamp has been set for this sample"
                Exit Select
            Case &H80040252UI
                'VFW_E_NO_TIME_FORMAT_SET
                errorMsg = "No media time format has been selected"
                Exit Select
            Case &H80040253UI
                'VFW_E_MONO_AUDIO_HW
                errorMsg = "Cannot change balance because audio device is mono only"
                Exit Select
            Case &H40260
                'VFW_S_MEDIA_TYPE_IGNORED
                errorMsg = "ActiveMovie cannot play MPEG movies on this processor"
                Exit Select
            Case &H80040261UI
                'VFW_E_NO_TIME_FORMAT
                errorMsg = "Cannot get or set time related information on an object that is using a time format of TIME_FORMAT_NONE"
                Exit Select
            Case &H80040262UI
                'VFW_E_READ_ONLY
                errorMsg = "The connection cannot be made because the stream is read only and the filter alters the data"
                Exit Select
            Case &H40263
                'VFW_S_RESERVED
                errorMsg = "This success code is reserved for internal purposes within ActiveMovie"
                Exit Select
            Case &H80040264UI
                'VFW_E_BUFFER_UNDERFLOW
                errorMsg = "The buffer is not full enough"
                Exit Select
            Case &H80040266UI
                'VFW_E_UNSUPPORTED_STREAM
                errorMsg = "Pins cannot connect due to not supporting the same transport"
                Exit Select
            Case &H40267
                'VFW_S_STREAM_OFF
                errorMsg = "The stream has been turned off"
                Exit Select
            Case &H40270
                'VFW_S_CANT_CUE
                errorMsg = "The stop time for the sample was not set"
                Exit Select
            Case &H80040272UI
                'VFW_E_OUT_OF_VIDEO_MEMORY
                errorMsg = "The VideoPort connection negotiation process has failed"
                Exit Select
            Case &H80040276UI
                'VFW_E_DDRAW_CAPS_NOT_SUITABLE
                errorMsg = "This User Operation is inhibited by DVD Content at this time"
                Exit Select
            Case &H80040277UI
                'VFW_E_DVD_INVALIDDOMAIN
                errorMsg = "This Operation is not permitted in the current domain"
                Exit Select
            Case &H40280
                'VFW_E_DVD_NO_BUTTON
                errorMsg = "This object cannot be used anymore as its time has expired"
                Exit Select
            Case &H80040281UI
                'VFW_E_DVD_WRONG_SPEED
                errorMsg = "The operation cannot be performed at the current playback speed"
                Exit Select
            Case &H80040283UI
                'VFW_E_DVD_MENU_DOES_NOT_EXIST
                errorMsg = "The specified command was either cancelled or no longer exists"
                Exit Select
            Case &H80040284UI
                'VFW_E_DVD_STATE_WRONG_VERSION
                errorMsg = "The data did not contain a recognized version"
                Exit Select
            Case &H80040285UI
                'VFW_E_DVD_STATE_CORRUPT
                errorMsg = "The state data was corrupt"
                Exit Select
            Case &H80040286UI
                'VFW_E_DVD_STATE_WRONG_DISC
                errorMsg = "The state data is from a different disc"
                Exit Select
            Case &H80040287UI
                'VFW_E_DVD_INCOMPATIBLE_REGION
                errorMsg = "The region was not compatible with the current drive"
                Exit Select
            Case &H80040288UI
                'VFW_E_DVD_NO_ATTRIBUTES
                errorMsg = "The requested DVD stream attribute does not exist"
                Exit Select
            Case &H80040290UI
                'VFW_E_DVD_NO_GOUP_PGC
                errorMsg = "The current parental level was too low"
                Exit Select
            Case &H80040291UI
                'VFW_E_DVD_INVALID_DISC
                errorMsg = "The specified path does not point to a valid DVD disc"
                Exit Select
            Case &H80040292UI
                'VFW_E_DVD_NO_RESUME_INFORMATION
                errorMsg = "There is currently no resume information"
                Exit Select
            Case &H80040295UI
                'VFW_E_PIN_ALREADY_BLOCKED_ON_THIS_THREAD
                errorMsg = "An operation failed due to a certification failure"
                Exit Select
            Case &H80040296UI
                'VFW_E_VMR_NOT_IN_MIXER_MODE
                errorMsg = "The VMR could not find any ProcAmp hardware on the current display device"
                Exit Select
            Case &H80040297UI
                'VFW_E_VMR_NO_AP_SUPPLIED
                errorMsg = "The application has not yet provided the VMR filter with a valid allocator-presenter object"
                Exit Select
            Case &H80040298UI
                'VFW_E_VMR_NO_DEINTERLACE_HW
                errorMsg = "The VMR could not find any ProcAmp hardware on the current display device"
                Exit Select
            Case &H80040299UI
                'VFW_E_VMR_NO_PROCAMP_HW
                errorMsg = "VMR9 does not work with VPE-based hardware decoders"
                Exit Select
            Case &H8004029AUI
                'VFW_E_DVD_VMR9_INCOMPATIBLEDEC
                errorMsg = "VMR9 does not work with VPE-based hardware decoders"
                Exit Select
            Case &H8004029BUI
                'VFW_E_NO_COPP_HW
                errorMsg = "The current display device does not support Content Output Protection Protocol (COPP) H/W"
                Exit Select
        End Select
        Return errorMsg
    End Function

    Private Class InvalidVideoFileException
        Inherits System.Exception
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(info As System.Runtime.Serialization.SerializationInfo, context As System.Runtime.Serialization.StreamingContext)
            MyBase.New(info, context)
        End Sub
        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub
    End Class


    ''' <summary>
    ''' Returns the Size of video frames
    ''' </summary>
    ''' <param name="videoFile">Path to the video file</param>
    ''' <returns>Size of video. Size.Empty for failures (no video stream present, file not supported..)</returns>
    Private Function GetVideoSize(videoFile As String) As Size
        If videoFile Is Nothing Then
            Throw New ArgumentNullException("videoFile")
        End If

        Dim mediaDet As MediaDet = Nothing
        Dim mediaType As _AMMediaType = Nothing
        Try
            If openVideoStream(videoFile, mediaDet, mediaType) Then
                Return GetVideoSize(mediaType)
            End If
        Finally
            If mediaDet IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mediaDet)
            End If
        End Try
        Return Size.Empty
    End Function

    Private Function getVideoSize(mediaType As _AMMediaType) As Size
        Dim videoInfo As WinStructs.VIDEOINFOHEADER = DirectCast(System.Runtime.InteropServices.Marshal.PtrToStructure(mediaType.pbFormat, GetType(WinStructs.VIDEOINFOHEADER)), WinStructs.VIDEOINFOHEADER)

        Return New Size(videoInfo.bmiHeader.biWidth, videoInfo.bmiHeader.biHeight)
    End Function

    Private Function openVideoStream(videoFile As String, ByRef mediaDet As MediaDet, ByRef aMMediaType As _AMMediaType) As Boolean
        mediaDet = New MediaDet

        'loads file
        mediaDet.Filename = videoFile

        'gets # of streams
        Dim streamsNumber As Integer = mediaDet.OutputStreams

        'finds a video stream
        For i As Integer = 0 To streamsNumber - 1
            mediaDet.CurrentStream = i
            Dim mediaType As _AMMediaType = mediaDet.StreamMediaType

            If mediaType.majortype = MayorTypes.MEDIATYPE_Video Then
                'video stream found
                aMMediaType = mediaType
                Return True
            End If
        Next

        'no video stream found
        System.Runtime.InteropServices.Marshal.ReleaseComObject(mediaDet)
        mediaDet = Nothing
        aMMediaType = New _AMMediaType()
        Return False
    End Function

    ' --- major and sub types --- from uuids.h
    ' extracted with regex .+?\((MEDIASUBTYPE_\w+),(.+?)\)//

    Private Class MayorTypes
        Public Shared MEDIATYPE_Video As New Guid(&H73646976, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIATYPE_Audio As New Guid(&H73647561, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIATYPE_Text As New Guid(&H73747874, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIATYPE_Midi As New Guid(&H7364696D, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIATYPE_Stream As New Guid(&HE436EB83UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIATYPE_Interleaved As New Guid(&H73766169, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIATYPE_File As New Guid(&H656C6966, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIATYPE_ScriptCommand As New Guid(&H73636D64, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIATYPE_AUXLine21Data As New Guid(&H670AEA80, &H3A82, &H11D0, &HB7, &H9B, &H0, _
            &HAA, &H0, &H37, &H67, &HA7)

        Public Shared MEDIATYPE_VBI As New Guid(&HF72A76E1UI, &HEB0A, &H11D0, &HAC, &HE4, &H0, _
            &H0, &HC0, &HCC, &H16, &HBA)

        Public Shared MEDIATYPE_Timecode As New Guid(&H482DEE3, &H7817, &H11CF, &H8A, &H3, &H0, _
            &HAA, &H0, &H6E, &HCB, &H65)

        Public Shared MEDIATYPE_LMRT As New Guid(&H74726C6D, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIATYPE_URL_STREAM As New Guid(&H736C7275, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

    End Class

    Private Class SubTypes
        Public Shared MEDIASUBTYPE_CLPL As New Guid(&H4C504C43, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_YUYV As New Guid(&H56595559, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IYUV As New Guid(&H56555949, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_YVU9 As New Guid(&H39555659, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_Y411 As New Guid(&H31313459, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_Y41P As New Guid(&H50313459, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_YUY2 As New Guid(&H32595559, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_YVYU As New Guid(&H55595659, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_UYVY As New Guid(&H59565955, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_Y211 As New Guid(&H31313259, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_CLJR As New Guid(&H524A4C43, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IF09 As New Guid(&H39304649, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_CPLA As New Guid(&H414C5043, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_MJPG As New Guid(&H47504A4D, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_TVMJ As New Guid(&H4A4D5654, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_WAKE As New Guid(&H454B4157, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_CFCC As New Guid(&H43434643, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IJPG As New Guid(&H47504A49, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_Plum As New Guid(&H6D756C50, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_DVCS As New Guid(&H53435644, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_DVSD As New Guid(&H44535644, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_MDVF As New Guid(&H4656444D, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_RGB1 As New Guid(&HE436EB78UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_RGB4 As New Guid(&HE436EB79UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_RGB8 As New Guid(&HE436EB7AUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_RGB565 As New Guid(&HE436EB7BUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_RGB555 As New Guid(&HE436EB7CUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_RGB24 As New Guid(&HE436EB7DUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)



        '	******************

        Public Shared MEDIASUBTYPE_ARGB1555 As New Guid(&H297C55AF, &HE209, &H4CB3, &HB7, &H57, &HC7, _
            &H6D, &H6B, &H9C, &H88, &HA8)

        Public Shared MEDIASUBTYPE_ARGB4444 As New Guid(&H6E6415E6, &H5C24, &H425F, &H93, &HCD, &H80, _
            &H10, &H2B, &H3D, &H1C, &HCA)

        Public Shared MEDIASUBTYPE_ARGB32 As New Guid(&H773C9AC0, &H3274, &H11D0, &HB7, &H24, &H0, _
            &HAA, &H0, &H6C, &H1A, &H1)

        Public Shared MEDIASUBTYPE_A2R10G10B10 As New Guid(&H2F8BB76D, &HB644, &H4550, &HAC, &HF3, &HD3, _
            &HC, &HAA, &H65, &HD5, &HC5)

        Public Shared MEDIASUBTYPE_A2B10G10R10 As New Guid(&H576F7893, &HBDF6, &H48C4, &H87, &H5F, &HAE, _
            &H7B, &H81, &H83, &H45, &H67)

        Public Shared MEDIASUBTYPE_AYUV As New Guid(&H56555941, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_AI44 As New Guid(&H34344941, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IA44 As New Guid(&H34344149, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_RGB32_D3D_DX7_RT As New Guid(&H32335237, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_RGB16_D3D_DX7_RT As New Guid(&H36315237, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_ARGB32_D3D_DX7_RT As New Guid(&H38384137, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_ARGB4444_D3D_DX7_RT As New Guid(&H34344137, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_ARGB1555_D3D_DX7_RT As New Guid(&H35314137, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_RGB32_D3D_DX9_RT As New Guid(&H32335239, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_RGB16_D3D_DX9_RT As New Guid(&H36315239, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_ARGB32_D3D_DX9_RT As New Guid(&H38384139, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_ARGB4444_D3D_DX9_RT As New Guid(&H34344139, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_ARGB1555_D3D_DX9_RT As New Guid(&H35314139, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        '	***************************************

        Public Shared MEDIASUBTYPE_YV12 As New Guid(&H32315659, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_NV12 As New Guid(&H3231564E, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IMC1 As New Guid(&H31434D49, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IMC2 As New Guid(&H32434D49, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IMC3 As New Guid(&H33434D49, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IMC4 As New Guid(&H34434D49, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_S340 As New Guid(&H30343353, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_S342 As New Guid(&H32343353, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_Overlay As New Guid(&HE436EB7FUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_MPEG1Packet As New Guid(&HE436EB80UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_MPEG1Payload As New Guid(&HE436EB81UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_MPEG1AudioPayload As New Guid(&H50, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_MPEG1System As New Guid(&HE436EB84UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_MPEG1VideoCD As New Guid(&HE436EB85UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_MPEG1Video As New Guid(&HE436EB86UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_MPEG1Audio As New Guid(&HE436EB87UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_Avi As New Guid(&HE436EB88UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_Asf As New Guid(&H3DB80F90, &H9412, &H11D1, &HAD, &HED, &H0, _
            &H0, &HF8, &H75, &H4B, &H99)

        Public Shared MEDIASUBTYPE_QTMovie As New Guid(&HE436EB89UI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_QTRpza As New Guid(&H617A7072, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_QTSmc As New Guid(&H20636D73, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_QTRle As New Guid(&H20656C72, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_QTJpeg As New Guid(&H6765706A, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_PCMAudio_Obsolete As New Guid(&HE436EB8AUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_PCM As New Guid(&H1, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_WAVE As New Guid(&HE436EB8BUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_AU As New Guid(&HE436EB8CUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_AIFF As New Guid(&HE436EB8DUI, &H524F, &H11CE, &H9F, &H53, &H0, _
            &H20, &HAF, &HB, &HA7, &H70)

        Public Shared MEDIASUBTYPE_dvsd_ As New Guid(&H64737664, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_dvhd As New Guid(&H64687664, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_dvsl As New Guid(&H6C737664, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_dv25 As New Guid(&H35327664, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_dv50 As New Guid(&H30357664, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_dvh1 As New Guid(&H31687664, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_Line21_BytePair As New Guid(&H6E8D4A22, &H310C, &H11D0, &HB7, &H9A, &H0, _
            &HAA, &H0, &H37, &H67, &HA7)

        Public Shared MEDIASUBTYPE_Line21_GOPPacket As New Guid(&H6E8D4A23, &H310C, &H11D0, &HB7, &H9A, &H0, _
            &HAA, &H0, &H37, &H67, &HA7)

        Public Shared MEDIASUBTYPE_Line21_VBIRawData As New Guid(&H6E8D4A24, &H310C, &H11D0, &HB7, &H9A, &H0, _
            &HAA, &H0, &H37, &H67, &HA7)

        Public Shared MEDIASUBTYPE_TELETEXT As New Guid(&HF72A76E3UI, &HEB0A, &H11D0, &HAC, &HE4, &H0, _
            &H0, &HC0, &HCC, &H16, &HBA)

        Public Shared MEDIASUBTYPE_WSS As New Guid(&H2791D576, &H8E7A, &H466F, &H9E, &H90, &H5D, _
            &H3F, &H30, &H83, &H73, &H8B)

        Public Shared MEDIASUBTYPE_VPS As New Guid(&HA1B3F620UI, &H9792, &H4D8D, &H81, &HA4, &H86, _
            &HAF, &H25, &H77, &H20, &H90)

        Public Shared MEDIASUBTYPE_DRM_Audio As New Guid(&H9, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_IEEE_FLOAT As New Guid(&H3, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_DOLBY_AC3_SPDIF As New Guid(&H92, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_RAW_SPORT As New Guid(&H240, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_SPDIF_TAG_241h As New Guid(&H241, &H0, &H10, &H80, &H0, &H0, _
            &HAA, &H0, &H38, &H9B, &H71)

        Public Shared MEDIASUBTYPE_DssVideo As New Guid(&HA0AF4F81UI, &HE163, &H11D0, &HBA, &HD9, &H0, _
            &H60, &H97, &H44, &H11, &H1A)

        Public Shared MEDIASUBTYPE_DssAudio As New Guid(&HA0AF4F82UI, &HE163, &H11D0, &HBA, &HD9, &H0, _
            &H60, &H97, &H44, &H11, &H1A)

        Public Shared MEDIASUBTYPE_VPVideo As New Guid(&H5A9B6A40, &H1A22, &H11D1, &HBA, &HD9, &H0, _
            &H60, &H97, &H44, &H11, &H1A)

    End Class
End Module
