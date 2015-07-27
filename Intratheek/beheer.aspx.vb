Public Class beheer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        If Session("beheerder") = False Then
            pnlWrapper.Visible = False
            lblBevoegd.Visible = True
        End If
    End Sub

    Protected Sub btnToevoegen_Click(sender As Object, e As EventArgs) Handles btnToevoegen.Click
        Response.Redirect("boek_nieuw.aspx")
    End Sub

    Protected Sub btnWijzigen_Click(sender As Object, e As EventArgs) Handles btnWijzigen.Click
        Response.Redirect("boeken.aspx?action=modify")
    End Sub

    Protected Sub btnVerwijderen_Click(sender As Object, e As EventArgs) Handles btnVerwijderen.Click
        Response.Redirect("boeken.aspx?action=delete")
    End Sub

    Protected Sub btnExemplaren_Click(sender As Object, e As EventArgs) Handles btnExemplaren.Click
        Response.Redirect("boeken.aspx?action=bibman")
    End Sub

    Protected Sub btnUitleningenBeheer_Click(sender As Object, e As EventArgs) Handles btnUitleningenBeheer.Click
        Response.Redirect("uitleningenbeheer.aspx")
    End Sub
End Class