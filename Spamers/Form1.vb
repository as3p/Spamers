Imports System.Web
Imports System.IO
Imports System.Net.Mail

Public Class Form1

    'Kirim Email
    Sub kirim_email()
        Dim mail As New MailMessage
        Dim smtpSrv As New SmtpClient

        Dim pengirim As String = GabungTextbox32.Text
        Dim NamaPengirim As String = GabungTextbox31.Text
        Dim password As String = GabungTextbox33.Text
        Dim penerima As String = GabungTextbox35.Text
        Dim Subjek As String = GabungTextbox36.Text
        Dim Pesan As String = GabungRichTextBox1.Text

        smtpSrv.Credentials = New Net.NetworkCredential(pengirim, password)
        smtpSrv.Port = Convert.ToInt32(GabungTextbox34.Text)
        smtpSrv.Host = GabungComboBox21.Text
        smtpSrv.EnableSsl = True
        mail.From = New MailAddress(pengirim, NamaPengirim)
        mail.To.Add(penerima)
        mail.Subject = Subjek

        'Logic jika RadioButton HTML di pilih 
        If GabungRadioButton12.Checked Then
            mail.IsBodyHtml = True
        End If
        mail.Body = Pesan

        'Logic jika kirim tanpa lampiran
        If GabungTextbox37.Text = "" Then
        Else
            'Logic jika kirim dengan lampiran
            Dim lampiran As New System.Net.Mail.Attachment(GabungTextbox37.Text) 'untuk lampiran
            mail.Attachments.Add(lampiran) 'browse kirim lampiran
        End If

        smtpSrv.Send(mail)
    End Sub

    'Combobox SMTP
    Sub pilihan_ComboBox()
        GabungComboBox21.Items.Clear()
        GabungComboBox21.Items.Add("Server Gmail")
        GabungComboBox21.Items.Add("Server Yahoo")
        GabungComboBox21.Items.Add("Server Aol")
        GabungComboBox21.Items.Add("Server Outlook")
        GabungComboBox21.Items.Add("Server Yandek")
        GabungComboBox21.Items.Add("Any")
    End Sub

    Sub isi_ComboBox()
        If GabungComboBox21.SelectedItem = "Server Gmail" Then
            GabungTextbox38.Text = "smtp.gmail.com"
            GabungTextbox34.Text = "587"
        ElseIf GabungComboBox21.SelectedItem = "Server Yahoo" Then
            GabungTextbox38.Text = "smtp.mail.yahoo.com"
            GabungTextbox34.Text = "587"
        ElseIf GabungComboBox21.SelectedItem = "Server Aol" Then
            GabungTextbox38.Text = "smtp.aol.com"
            GabungTextbox34.Text = "587"
        ElseIf GabungComboBox21.SelectedItem = "Server Outlook" Then
            GabungTextbox38.Text = "smtp.live.com"
            GabungTextbox34.Text = "587"
        ElseIf GabungComboBox21.SelectedItem = "Server Yandek" Then
            GabungTextbox38.Text = "smtp.yandex.com"
            GabungTextbox34.Text = "587"
        ElseIf GabungComboBox21.SelectedItem = "Any" Then
            GabungTextbox38.Text = " "
            GabungTextbox34.Text = " "
        End If
    End Sub



    Private Sub GabungFlatButton11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungFlatButton11.Click
        Application.Exit()
    End Sub


    'untuk mouse
    Dim Point As New System.Drawing.Point()
    Dim X, Y As Integer
    Private Sub Panel1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove
        If e.Button = MouseButtons.Left Then
            Point = Control.MousePosition
            Point.X = Point.X - (X)
            Point.Y = Point.Y - (Y)
            Me.Location = Point
        End If
    End Sub
    Private Sub Panel1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseDown
        X = Control.MousePosition.X - Me.Location.X
        Y = Control.MousePosition.Y - Me.Location.Y
    End Sub


    Private Sub GabungFlatButton33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungFlatButton33.Click
        kirim_email()
    End Sub

    'Lampiran
    Private Sub GabungFlatButton32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungFlatButton32.Click
        OpenFileDialog1.Title = "Please Select a File"
        OpenFileDialog1.FileName = " "
        OpenFileDialog1.InitialDirectory = "D:\"
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        'Browse file
        Dim strm As System.IO.Stream
        strm = OpenFileDialog1.OpenFile()
        GabungTextbox37.Text = OpenFileDialog1.FileName.ToString()
        If Not (strm Is Nothing) Then
            strm.Close()
        End If
    End Sub
   

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Menampilkan Combobox
        pilihan_ComboBox()
        'Menghide Button Message
        GabungFlatButton34.Visible = False
    End Sub
    Private Sub GabungComboBox21_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungComboBox21.SelectedIndexChanged
        isi_ComboBox()
    End Sub


    'Hide Password
    Private Sub GabungCheckBox11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungCheckBox11.Click
        If GabungCheckBox11.Checked = False Then
            GabungTextbox33.UseSystemPasswordChar = True
            GabungCheckBox11.Text = "Show Password"
        Else
            GabungTextbox33.UseSystemPasswordChar = False
            GabungCheckBox11.Text = "Hide Password"
        End If
    End Sub

    'Memunculkan message
    Private Sub GabungRadioButton12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungRadioButton12.Click
        If GabungRadioButton12.Checked Then
            GabungFlatButton34.Visible = True
        Else
            GabungFlatButton34.Visible = False
        End If
    End Sub

    Private Sub GabungRadioButton11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungRadioButton11.Click
        If GabungRadioButton11.Checked Then
            GabungFlatButton34.Visible = False
        Else
            GabungFlatButton34.Visible = True
        End If
    End Sub

    Dim hitung As Integer = 11
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If hitung > 0 Then
            hitung = hitung - 1
            Label2.Text = hitung.ToString()
        ElseIf hitung = 0 Then
            GabungTextbox35.Text = "-"
            Timer1.Enabled = False
            hitung = 11
        End If
    End Sub





    Sub warna_grid_listview()
        For Each i As ListViewItem In ListView1.Items
            If i.Index Mod 2 = 0 Then
                i.BackColor = Color.FromArgb(41, 133, 211)
            Else
                i.BackColor = Color.FromArgb(255, 255, 255)
            End If
        Next
    End Sub

 
    Private Sub GabungFlatButton35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungFlatButton35.Click
        Dim lvl As New ListViewItem
        'menghitung jumlah
        lvl.Text = ListView1.Items.Count + 1
        lvl.SubItems.Add(GabungTextbox39.Text)
        ListView1.Items.Add(lvl)
        lvl = Nothing
        Call warna_grid_listview()
    End Sub

    Private Sub GabungSwitch11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungSwitch11.Click
        If GabungSwitch11.Switched = True Then
            Label14.Text = "OKE"
        Else
            GabungSwitch11.Switched = False
            Label14.Text = "NG"
        End If

    End Sub

    'Upload Email
    Private Sub GabungFlatButton36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungFlatButton36.Click
        'File akan di filter hanya file .csv
        OpenFileDialog1.Filter = ("ONLY Text Files (*.txt) | *.txt")
        OpenFileDialog1.FileName = ""

        'Jika button memilih oke dan jika tidak maka tidak akan menjalankan apa apa
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            'program akan mengeksekusi
            Dim filePath As String = OpenFileDialog1.FileName
            Dim streamReader As New IO.StreamReader(OpenFileDialog1.FileName)
            While (streamReader.Peek() > -1)
                Dim lvl As New ListViewItem
                lvl.Text = ListView1.Items.Count + 1
                lvl.SubItems.Add(streamReader.ReadLine)
                ListView1.Items.Add(lvl)
                lvl = Nothing
                Call warna_grid_listview()
            End While
        End If
    End Sub

    'Save Email
    Private Sub GabungFlatButton37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GabungFlatButton37.Click
        SaveFileDialog1.Filter = ("ONLY Text Files (*.txt) | *.txt")
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim Tulis As New IO.StreamWriter(SaveFileDialog1.FileName)
            Dim HeaderKolom As ListView.ColumnHeaderCollection = ListView1.Columns
            For Each lvl As ListViewItem In ListView1.Items
                Dim Simpan As String = ""
                For i = 0 To lvl.SubItems.Count - 1
                    Simpan += HeaderKolom(i).Text + " :" + lvl.SubItems(i).Text + Space(3)
                Next
                Tulis.WriteLine(Simpan)
            Next
            Tulis.Close() 'Or  Write.Flush()
            MessageBox.Show("File Telah di buat")
        End If
    End Sub
End Class
