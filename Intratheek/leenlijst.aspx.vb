Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Security.Principal

Public Class leenlijst
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        Dim strFilter As String = String.Empty

        If Session("leenlijst") IsNot Nothing Then
            Dim lstBoeken As List(Of String) = Session("leenlijst")

            If lstBoeken.Count > 0 Then
                For Each boek In lstBoeken
                    strFilter &= "BoekNummer=" & boek & " OR "
                Next

                strFilter = strFilter.Remove(strFilter.Length - 4, 4)
                dtsBoekenLijst.FilterExpression = strFilter
            Else
                grdBoeken.Visible = False
            End If
        Else
            grdBoeken.Visible = False
        End If


    End Sub

    Protected Sub imbCover_Command(sender As Object, e As CommandEventArgs)
        Response.Redirect("boekdetails.aspx?isbn=" & e.CommandArgument.ToString)
    End Sub

    Protected Sub btnLenen_Click(sender As Object, e As EventArgs)
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        Dim self As Button = Convert.ChangeType(sender, GetType(Button))
        Dim strISBN As String = self.CommandArgument
        Dim strBoekenBeschikbaar As List(Of String) = New List(Of String)

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT tblBibliotheek.BoekNummer FROM tblBoeken " & _
                            "INNER JOIN tblBibliotheek ON tblBibliotheek.ISBN = tblBoeken.ISBN " & _
                            "WHERE tblBibliotheek.ISBN = '" & strISBN & "' AND tblBibliotheek.BoekNummer NOT IN " & _
                            "(SELECT tblUitleningenBoeken.BoekNummer FROM tblUitleningenBoeken " & _
                            "INNER JOIN tblBibliotheek ON tblBibliotheek.BoekNummer = tblUitleningenBoeken.BoekNummer " & _
                            "WHERE tblBibliotheek.ISBN = '" & strISBN & "' AND Teruggebracht = 0)"

        myReader = myCmd.ExecuteReader()

        Do While myReader.Read()
            strBoekenBeschikbaar.Add(myReader.GetString(0))
        Loop

        Dim strBoekNummer As String = strBoekenBeschikbaar(0)
        Dim boekenlijst As List(Of String)

        If Session("leenlijst") Is Nothing Then
            Session("leenlijst") = New List(Of String)
            boekenlijst = Session("leenlijst")
            boekenlijst.Add(strBoekNummer)
            Session("leenlijst") = boekenlijst
        Else
            boekenlijst = Session("leenlijst")
            If Not boekenlijst.Contains(strBoekNummer) Then
                boekenlijst.Add(strBoekNummer)
                Session("leenlijst") = boekenlijst
            Else
                boekenlijst.Remove(strBoekNummer)
                Session("leenlijst") = boekenlijst
            End If
        End If

        myConn.Close()

        Response.Redirect(Request.RawUrl, False)
    End Sub

    Protected Sub btnLenen_PreRender(sender As Object, e As EventArgs)
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        Dim self As Button = Convert.ChangeType(sender, GetType(Button))
        Dim strISBN As String = self.CommandArgument
        Dim strBoekenBeschikbaar As List(Of String) = New List(Of String)

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT tblBibliotheek.BoekNummer FROM tblBoeken " & _
                            "INNER JOIN tblBibliotheek ON tblBibliotheek.ISBN = tblBoeken.ISBN " & _
                            "WHERE tblBibliotheek.ISBN = '" & strISBN & "' AND tblBibliotheek.BoekNummer NOT IN " & _
                            "(SELECT tblUitleningenBoeken.BoekNummer FROM tblUitleningenBoeken " & _
                            "INNER JOIN tblBibliotheek ON tblBibliotheek.BoekNummer = tblUitleningenBoeken.BoekNummer " & _
                            "WHERE tblBibliotheek.ISBN = '" & strISBN & "' AND Teruggebracht = 0)"

        myReader = myCmd.ExecuteReader()

        Do While myReader.Read()
            strBoekenBeschikbaar.Add(myReader.GetString(0))
        Loop

        If strBoekenBeschikbaar.Count = 0 Then
            Dim s As Button = Convert.ChangeType(sender, GetType(Button))
            s.Text = "Niet beschikbaar"
            s.BackColor = Color.Gray
            s.Enabled = False
            Exit Sub
        End If

        Dim blnHeeftBoek As Boolean = False
        If Session("leenlijst") IsNot Nothing Then
            Dim boekenlijst As List(Of String) = Session("leenlijst")

            For Each boek In strBoekenBeschikbaar
                If boekenlijst.Contains(boek) Then
                    blnHeeftBoek = True
                End If
            Next

            If blnHeeftBoek Then
                Dim s As Button = Convert.ChangeType(sender, GetType(Button))
                s.Text = "Verwijder uit leenlijst"
                s.BackColor = Color.Black
                Exit Sub
            End If
        End If


    End Sub

    Protected Sub grdBoeken_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdBoeken.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btn As Button = Convert.ChangeType(e.Row.FindControl("btnLenen"), GetType(Button))
            Dim lbl As Label = Convert.ChangeType(e.Row.FindControl("lblISBN"), GetType(Label))

            btn.CommandArgument = lbl.Text
        End If
    End Sub

    Protected Sub btnUitlenen_Click(sender As Object, e As EventArgs) Handles btnUitlenen.Click
        Dim boekenlijst As List(Of String) = Session("leenlijst")

        If boekenlijst.Count = 0 Then
            Exit Sub
        End If

        'INSERT INTO [Intratheek].[dbo].[tblUitleningen]
        '   ([ADGuid]
        '   ,[Uitleendatum])
        'OUTPUT Inserted.UitleningID
        'VALUES()
        '   (CONVERT(binary(16), 'ac9e07471cf5e042b3b389beed3d0562', 2), '20150326')
        'GO()
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim sDate As String = String.Empty
        Dim d As Date = DateTime.Now

        sDate = d.Year.ToString() & "-" & d.Month.ToString("00") & "-" & d.Day.ToString("00") & "T" & d.Hour.ToString("00") & ":" & d.Minute.ToString("00") & ":" & d.Second.ToString("00")

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "INSERT INTO [Intratheek].[dbo].[tblUitleningen] " _
            & "([ADGuid],[Uitleendatum]) " _
            & "OUTPUT Inserted.UitleningID " _
            & "VALUES" _
            & "(CONVERT(binary(16), '" & WindowsIdentity.GetCurrent().GetGUID() & "', 2), '" & sDate & "')"

        'litAangemeld.Text &= myCmd.CommandText

        Dim intPrimKey As Integer = 0
        intPrimKey = myCmd.ExecuteScalar()

        Dim db As Date = d.AddDays(21)
        Dim sDateD As String = String.Empty

        sDateD = db.Year.ToString() & "-" & db.Month.ToString("00") & "-" & db.Day.ToString("00") & "T" & db.Hour.ToString("00") & ":" & db.Minute.ToString("00") & ":" & db.Second.ToString("00")

        For Each boek In boekenlijst
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "INSERT INTO [Intratheek].[dbo].[tblUitleningenBoeken] " _
                & "([UitleningID],[BoekNummer],[Terugbrengen tegen]) " _
                & "VALUES(" _
                & "'" & intPrimKey.ToString & "','" & boek.ToString() & "','" & sDateD & "'" _
                & ")"
            myCmd.ExecuteNonQuery()
        Next

        Dim pi As Printout.PrintOutInfo
        pi.Gebruikersnaam = WindowsIdentity.GetCurrent().GetUsername()

        Dim po As Printout.PrintOutOptions
        po.tableBorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY
        po.normalFontSize = 12
        po.bigFontSize = 18
        po.backgroundColour = New iTextSharp.text.BaseColor(153, 0, 0)
        po.coloredTextColour = New iTextSharp.text.BaseColor(153, 0, 0)

        Dim pitems As List(Of Printout.BookEntry) = New List(Of Printout.BookEntry)
        For Each boek In boekenlijst
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "SELECT Titel, dbo.GetBookAuthors(tblBoeken.ISBN) AS Auteurs FROM tblBoeken " & _
                                "INNER JOIN tblBibliotheek ON tblBibliotheek.ISBN = tblBoeken.ISBN" & _
                                " WHERE tblBibliotheek.BoekNummer = " & boek.ToString()
            myReader = myCmd.ExecuteReader()

            Dim strTitel, strAuteurs As String
            While myReader.Read()
                strTitel = myReader.GetString(0)
                strAuteurs = myReader.GetString(1)
            End While
            myReader.Close()

            pitems.Add(CreateBookEntry(boek.ToString(), strTitel, strAuteurs, d.ToShortDateString(), d.AddDays(21).ToShortDateString()))
        Next

        Dim strFilename As String = DateTime.Now.Day & DateTime.Now.Month & DateTime.Now.Year & DateTime.Now.Hour & DateTime.Now.Minute & DateTime.Now.Second

        Printout.Create(Server.MapPath("tmp/" & strFilename & ".pdf"), po, pi, pitems, Server.MapPath("img/logo.png"))

        Session("leenlijst") = Nothing
        Session("pdflocatie") = "tmp/" & strFilename & ".pdf"

        Response.Redirect("lening_klaar.aspx")
    End Sub
End Class