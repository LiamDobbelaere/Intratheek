Public Class lening_klaar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)
    End Sub

    Protected Sub btnOpenPDF_Click(sender As Object, e As EventArgs) Handles btnOpenPDF.Click
        Response.Redirect(Session("pdflocatie"))
    End Sub

    Protected Sub btnBibliotheekkaart_Click(sender As Object, e As EventArgs) Handles btnBibliotheekkaart.Click
        Response.Redirect("bibliotheekkaart.aspx")
    End Sub
End Class