Public Class Form2

    Public _numSem As String
    Public _numAnnee As String
    Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click

        If textBox1.Text = "" Or Int16.Parse(textBox1.Text) < 1 Or Int16.Parse(textBox1.Text) > 52 Then
            MsgBox("Veuillez renseigner un N° de semaine valide!")
        Else
            _numSem = textBox1.Text
            _numAnnee = comboBox1.Text
            Close()
        End If

    End Sub
End Class