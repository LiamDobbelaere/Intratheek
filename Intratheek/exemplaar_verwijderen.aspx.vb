Imports System.Data.SqlClient

Public Class exemplaar_verwijderen
    Inherits System.Web.UI.Page

    Dim strISBN, strBoekNummer As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        If Session("beheerder") = False Then
            pnlWrapper.Visible = False
            lblBevoegd.Visible = True
        End If

        strISBN = Request.QueryString("isbn")
        strBoekNummer = Request.QueryString("boeknummer")

        imgCover.ImageUrl = "img/boeken/" & strISBN & ".jpg"

        litBoekNaam.Text = "exemplaar <b>" & strBoekNummer & "</b> (ISBN " & strISBN & ") "
    End Sub

    Protected Sub btnNeen_Click(sender As Object, e As EventArgs) Handles btnNeen.Click
        Response.Redirect("exemplarenbeheer.aspx?isbn=" & strISBN)
    End Sub

    Protected Sub btnJa_Click(sender As Object, e As EventArgs) Handles btnJa.Click
        'We gaan het exemplaar verwijderen
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        '[Stap 1] Verwijder alle uitleningen met dit exemplaar
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "DELETE tblUitleningenBoeken FROM tblUitleningenBoeken " _
                            & "WHERE BoekNummer = '" & strBoekNummer & "'"
        myCmd.ExecuteNonQuery()

        '[Stap 2] Verwijder het exemplaar van de bibliotheek
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "DELETE FROM tblBibliotheek " _
                            & "WHERE BoekNummer = '" & strBoekNummer & "'"
        myCmd.ExecuteNonQuery()

        'Klaar! Easy.

        myConn.Close()
        Response.Redirect("exemplarenbeheer.aspx?isbn=" & strISBN)
    End Sub
End Class