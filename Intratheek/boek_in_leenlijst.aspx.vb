Public Class boek_in_leenlijst
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)
    End Sub

    Protected Sub btnBoekenToevoegen_Click(sender As Object, e As EventArgs) Handles btnBoekenToevoegen.Click
        Response.Redirect("boeken.aspx")
    End Sub

    Protected Sub btnDoorgaanLeenlijst_Click(sender As Object, e As EventArgs) Handles btnDoorgaanLeenlijst.Click
        Response.Redirect("leenlijst.aspx")
    End Sub
End Class