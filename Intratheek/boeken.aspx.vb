Imports System.Security.Principal

Public Class boeken
    Inherits System.Web.UI.Page

    Dim strAction As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        strAction = Request.QueryString("action")

        lblFilter.Text = "Filter: "

        If Session("zoekterm") IsNot Nothing Then
            Dim strZoekTerm As String = Session("zoekterm")

            If strZoekTerm <> String.Empty Then
                lblFilter.Visible = True
                lblFilter.Text &= "(Zoekterm: " & strZoekTerm & ") "
                'Oud systeem, zou de zoekterm moeten splitten en manueel toevoegen
                'dtsBoekenLijst.FilterExpression = "Titel LIKE '%" & strZoekTerm & "%' OR Auteurs LIKE '%" & strZoekTerm & "%' OR Tags LIKE'%" & strZoekTerm & "%'"

                'Nieuw systeem
                'Splits de zoekterm op
                Dim strZoekwaarden() As String = strZoekTerm.Split(" ")
                Dim strOR As String = String.Empty
                dtsBoekenLijst.FilterExpression &= "("
                For Each zoekterm As String In strZoekwaarden
                    dtsBoekenLijst.FilterExpression &= strOR & "Titel LIKE '%" & zoekterm & "%' OR Auteurs LIKE '%" & zoekterm & "%' OR Tags LIKE'%" & zoekterm & _
                                                               "%' OR ISBN LIKE '" & zoekterm & "'"
                    If strOR = String.Empty Then
                        strOR = "OR "
                    End If
                Next
                dtsBoekenLijst.FilterExpression &= ")"
            End If
        End If

        If Session("taal") IsNot Nothing Then
            Dim shtTaal As Short = Session("taal")
            Dim strAND As String = String.Empty

            If dtsBoekenLijst.FilterExpression <> String.Empty Then
                strAND = " AND "
            End If

            If shtTaal <> 0 Then
                lblFilter.Visible = True
                lblFilter.Text &= "(Categorie: " & ddlTalen.SelectedItem.Text & ") "
                dtsBoekenLijst.FilterExpression &= strAND & "BoekNummer LIKE '" & shtTaal.ToString() & "%'"
            End If
        End If

        If Session("beheerder") = True Then
            chkToonAlle.Visible = True
        End If

        If chkToonAlle.Checked Then
            lblFilter.Visible = True
            lblFilter.Text &= "(Niet toegekende boeken)"

            Dim strAND As String = String.Empty

            If dtsBoekenLijst.FilterExpression <> String.Empty Then
                strAND = " AND "
            End If

            dtsBoekenLijst.FilterExpression &= strAND & "BoekNummer IS NULL"
        Else
            lblFilter.Visible = True
            lblFilter.Text &= "(Toegekende boeken)"
            Dim strAND As String = String.Empty

            If dtsBoekenLijst.FilterExpression <> String.Empty Then
                strAND = " AND "
            End If

            dtsBoekenLijst.FilterExpression &= strAND & "BoekNummer IS NOT NULL"
        End If
        grdBoeken.DataBind()

        If grdBoeken.Rows.Count = 0 Then
            lblGeenRes.Visible = True
        Else
            lblGeenRes.Visible = False
        End If

        'litAangemeld.Text &= dtsBoekenLijst.FilterExpression
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdBoeken.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblDesc As Label = Convert.ChangeType(e.Row.FindControl("lblDesc"), GetType(Label))
            Dim hplMeer As HyperLink = Convert.ChangeType(e.Row.FindControl("hplMeer"), GetType(HyperLink))
            Dim hplWijzigen As HyperLink = Convert.ChangeType(e.Row.FindControl("hplWijzigen"), GetType(HyperLink))
            Dim hplVerwijderen As HyperLink = Convert.ChangeType(e.Row.FindControl("hplVerwijderen"), GetType(HyperLink))
            Dim hplExemplaren As HyperLink = Convert.ChangeType(e.Row.FindControl("hplExemplaren"), GetType(HyperLink))
            Dim litStreepje As Literal = Convert.ChangeType(e.Row.FindControl("litStreepje"), GetType(Literal))

            Dim strWords As String() = lblDesc.Text.ToString.Split(" "c)
            Dim intCharacters As Integer = 0
            Dim strDesc As String = String.Empty

            For Each word As String In strWords
                If intCharacters <= 250 Then
                    intCharacters += word.Length
                    strDesc &= word & " "
                Else
                    strDesc &= "... "
                    hplMeer.Visible = True
                    Exit For
                End If
            Next

            lblDesc.Text = strDesc

            If Session("beheerder") = True Then
                If strAction = "modify" Then
                    hplWijzigen.Visible = True
                End If
                'litStreepje.Text = " | "
                If strAction = "delete" Then
                    hplVerwijderen.Visible = True
                End If
                If strAction = "bibman" Then
                    hplExemplaren.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub imbCover_Command(sender As Object, e As CommandEventArgs)
        Response.Redirect("boekdetails.aspx?isbn=" & e.CommandArgument.ToString)
    End Sub

    Protected Sub btnZoek_Click(sender As Object, e As EventArgs) Handles btnZoek.Click
        Session("zoekterm") = txtZoeken.Text.Trim().Replace("%", String.Empty).Replace("'", String.Empty)
        Response.Redirect(Request.RawUrl, False)
    End Sub

    Protected Sub ddlTalen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTalen.SelectedIndexChanged
        Session("taal") = ddlTalen.SelectedValue
        Response.Redirect(Request.RawUrl, False)
    End Sub

    Protected Sub ddlTalen_Init(sender As Object, e As EventArgs) Handles ddlTalen.Init
        ddlTalen.SelectedValue = Session("taal")
    End Sub

    Protected Sub btnLeeg_Click(sender As Object, e As EventArgs) Handles btnLeeg.Click
        Session("taal") = 0
        Session("zoekterm") = String.Empty
        Response.Redirect(Request.RawUrl, False)
    End Sub

    Protected Sub chkToonAlle_CheckedChanged(sender As Object, e As EventArgs) Handles chkToonAlle.CheckedChanged
        If chkToonAlle.Checked Then
            Dim strAND As String = String.Empty

            If dtsBoekenLijst.FilterExpression <> String.Empty Then
                strAND = " AND "
            End If

            dtsBoekenLijst.FilterExpression &= strAND & "BoekNummer IS NULL"
        Else
            Dim strAND As String = String.Empty

            If dtsBoekenLijst.FilterExpression <> String.Empty Then
                strAND = " AND "
            End If

            dtsBoekenLijst.FilterExpression &= strAND & "BoekNummer IS NOT NULL"
        End If

        grdBoeken.DataBind()
    End Sub
End Class