Imports System.Runtime.CompilerServices

Class MainForm

    Private Sub btnSourceFile_Click(sender As System.Object, e As System.EventArgs) Handles btnSourceFile.Click
        Dim ofd As New OpenFileDialog
        ofd.DefaultExt = "csv"
        If System.IO.File.Exists(txtSource.Text) Then
            ofd.FileName = txtSource.Text
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(txtSource.Text)
        ElseIf System.IO.Directory.Exists(txtSource.Text) Then
            ofd.InitialDirectory = txtSource.Text
        End If
        ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        ofd.Multiselect = True
        ofd.Title = "Find CSV File(s)..."
        If ofd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If ofd.FileNames.Count > 1 Then
                txtSource.Text = String.Join(",", ofd.FileNames.Select(Function(s As String)
                                                                           Return """" + s + """"
                                                                       End Function))
            Else
                txtSource.Text = ofd.FileName
            End If
        End If
    End Sub
End Class
