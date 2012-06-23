Imports System.Runtime.CompilerServices
Module ISynchronizeInvokeExtensions

    <System.Runtime.CompilerServices.Extension()>
    Public Sub InvokeEx(ByVal c As System.ComponentModel.ISynchronizeInvoke, ByVal s As Action)
        If c.InvokeRequired Then
            c.Invoke(s, Nothing)
        Else
            s()
        End If
    End Sub
End Module