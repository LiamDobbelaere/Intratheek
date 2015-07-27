Imports System.Data.SqlClient

Public Class boek_verwijderen
    Inherits System.Web.UI.Page

    Dim strBoekNummer As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        If Session("beheerder") = False Then
            pnlWrapper.Visible = False
            lblBevoegd.Visible = True
        End If

        strBoekNummer = Request.QueryString("isbn")

        imgCover.ImageUrl = "img/boeken/" & strBoekNummer & ".jpg"

        'Zoek de naam van het boek en steek het in de literal
        'Initialiseer een databaseverbinding
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        'Check eerst nog even of dit ISBN wel bestaat
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT Titel FROM tblBoeken " _
                            & "WHERE ISBN = '" & strBoekNummer & "'"
        myReader = myCmd.ExecuteReader()

        litBoekNaam.Text = "<b>"
        While myReader.Read()
            litBoekNaam.Text &= myReader.GetString(0)
        End While
        myReader.Close()
        litBoekNaam.Text &= "</b></font> (ISBN " & strBoekNummer & ")"
    End Sub

    Protected Sub btnNeen_Click(sender As Object, e As EventArgs) Handles btnNeen.Click
        Response.Redirect("boeken.aspx")
    End Sub

    Protected Sub btnJa_Click(sender As Object, e As EventArgs) Handles btnJa.Click
        'Oké, we gaan het boek verwijderen!
        'Dit valt nog mee SQL gewijs
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        '[Stap 1] Verwijder alle auteurs in tblBoekAuteurs (=/= de echte auteur records!)
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "DELETE FROM tblBoekAuteurs " _
                            & "WHERE ISBN = '" & strBoekNummer & "'"
        myCmd.ExecuteNonQuery()
        
        '[Stap 2] Verwijder alle tags in tblBoekGenres (=/= de werkelijke tags!)
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "DELETE FROM tblBoekGenres " _
                            & "WHERE ISBN = '" & strBoekNummer & "'"
        myCmd.ExecuteNonQuery()

        '[Stap 3] Verwijder alle uitleningen, deze zullen niet meer functioneren
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "DELETE tblUitleningenBoeken FROM tblUitleningenBoeken INNER JOIN tblBibliotheek ON tblBibliotheek.BoekNummer = tblUitleningenBoeken.BoekNummer " _
                            & "WHERE ISBN = '" & strBoekNummer & "'"
        myCmd.ExecuteNonQuery()

        '[Stap 4] Verwijder alle boekexemplaren
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "DELETE FROM tblBibliotheek " _
                            & "WHERE ISBN = '" & strBoekNummer & "'"
        myCmd.ExecuteNonQuery()

        '[Stap 5] Verwijder het boek zelf
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "DELETE FROM tblBoeken " _
                            & "WHERE ISBN = '" & strBoekNummer & "'"
        myCmd.ExecuteNonQuery()

        myConn.Close()
        Response.Redirect("boeken.aspx")
    End Sub
End Class