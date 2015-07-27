Imports System.Security.Principal
Imports System.Drawing
Imports System.Data.SqlClient

Public Class boekdetails
    Inherits System.Web.UI.Page


    Dim strBoekNummer As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        strBoekNummer = Request.QueryString("isbn")
        dtsBoekDetails.FilterExpression = "ISBN = " & strBoekNummer
    End Sub

    Protected Sub litGenres_DataBinding(sender As Object, e As EventArgs)
        Dim s As Literal = Convert.ChangeType(sender, GetType(Literal))
        Dim strGenres As String() = s.Text.Replace(" ", String.Empty).Split(",")
        s.Text = "<div id=genrelijst>"

        For Each genre As String In strGenres
            s.Text &= "<a href=# class=genre>" & genre & "</a>" & " | "
        Next

        s.Text = s.Text.Remove(s.Text.Length - 3, 3)

        s.Text &= "</div>"

    End Sub

    Protected Sub btnLenen_Click(sender As Object, e As EventArgs)
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        Dim lb As Label = Convert.ChangeType(fvBoekdetails.Row.FindControl("lblISBN"), GetType(Label))
        Dim strISBN As String = lb.Text
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
            myConn.Close()
            Response.Redirect("boek_in_leenlijst.aspx", False)
        Else
            boekenlijst = Session("leenlijst")
            If Not boekenlijst.Contains(strBoekNummer) Then
                boekenlijst.Add(strBoekNummer)
                Session("leenlijst") = boekenlijst
                myConn.Close()
                Response.Redirect("boek_in_leenlijst.aspx", False)
            Else
                boekenlijst.Remove(strBoekNummer)
                Session("leenlijst") = boekenlijst
                myConn.Close()
                Response.Redirect(Request.RawUrl, False)
            End If
        End If


    End Sub

    Protected Sub btnLenen_PreRender(sender As Object, e As EventArgs)
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        Dim lb As Label = Convert.ChangeType(fvBoekdetails.Row.FindControl("lblISBN"), GetType(Label))
        Dim strISBN As String = lb.Text
        Dim strBoekenBeschikbaar As List(Of String) = New List(Of String)

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT COUNT(*) FROM tblUitleningenBoeken " _
                            & "INNER JOIN tblBibliotheek ON tblBibliotheek.BoekNummer = tblUitleningenBoeken.BoekNummer " _
                            & "INNER JOIN tblUitleningen ON tblUitleningen.UitleningID = tblUitleningenBoeken.UitleningID " _
                            & "WHERE Teruggebracht = 0 AND tblBibliotheek.ISBN = '" & strISBN & "' AND ADGuid = CONVERT(binary(16), '" & WindowsIdentity.GetCurrent().GetGUID & "', 2)"

        Dim intHasLent As Integer = 0
        intHasLent = myCmd.ExecuteScalar()

        If intHasLent Then
            Dim s As Button = Convert.ChangeType(sender, GetType(Button))
            s.Text = "Geleend"
            s.BackColor = Color.Green
            s.Enabled = False
            Exit Sub
        End If




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

        myConn.Close()

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
End Class