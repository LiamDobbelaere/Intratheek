Public Class uitleningenbeheer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        lblFilter.Text = "Filter: "

        If Session("beheerder") = False Then
            pnlWrapper.Visible = False
            lblBevoegd.Visible = True
        End If

        If Session("zoekterm-boek") IsNot Nothing Then
            Dim strZoekTerm As String = Session("zoekterm-boek")

            If strZoekTerm <> String.Empty Then
                lblFilter.Visible = True
                lblFilter.Text &= "(Boek: " & strZoekTerm & ") "
                'Oud systeem, zou de zoekterm moeten splitten en manueel toevoegen
                'dtsBoekenLijst.FilterExpression = "Titel LIKE '%" & strZoekTerm & "%' OR Auteurs LIKE '%" & strZoekTerm & "%' OR Tags LIKE'%" & strZoekTerm & "%'"

                'Nieuw systeem
                'Splits de zoekterm op
                Dim strZoekwaarden() As String = strZoekTerm.Split(" ")
                Dim strOR As String = String.Empty
                dtsUitleningen.FilterExpression &= "("
                For Each zoekterm As String In strZoekwaarden
                    If zoekterm.Length > 2 Then
                        dtsUitleningen.FilterExpression &= strOR & "Titel LIKE '%" & zoekterm & "%' OR Auteurs LIKE '%" & zoekterm & "%' OR Tags LIKE'%" & zoekterm & "%'" & _
                                                                   " OR ISBN LIKE '" & zoekterm & "'"
                        If strOR = String.Empty Then
                            strOR = "OR "
                        End If
                    End If
                Next
                dtsUitleningen.FilterExpression &= ")"
            End If
        End If

        If Not chkToonAlle.Checked Then
            lblFilter.Visible = True
            lblFilter.Text &= "(Enkel niet teruggebracht)"

            Dim strAND As String = String.Empty

            If dtsUitleningen.FilterExpression <> String.Empty Then
                strAND = " AND "
            End If

            dtsUitleningen.FilterExpression &= strAND & "Teruggebracht = 0"
        Else
            lblFilter.Visible = True
            lblFilter.Text &= "(Alle uitleningen)"
        End If

        If Session("zoekterm-gebruiker") IsNot Nothing Then
            Dim strZoekTerm As String = Session("zoekterm-gebruiker")

            If strZoekTerm <> String.Empty Then
                lblFilter.Visible = True
                lblFilter.Text &= "(Gebruiker: " & Session("zoekterm-gebruiker") & ") "
            End If
        End If

        If Not Page.IsPostBack Then
            fvBoekdetails.Visible = False
        End If

        If grdUitleningen.Rows.Count = 0 Or grdUitleningen.NoVisibleRows() Then
            grdUitleningen.Visible = False
            lblGeenRes.Visible = True
        Else
            lblGeenRes.Visible = False
        End If

        grdUitleningen.DataBind()
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUitleningen.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblUsername As Label = Convert.ChangeType(e.Row.FindControl("lblUsername"), GetType(Label))
            Dim lblUitleendatum As Label = Convert.ChangeType(e.Row.FindControl("lblUitleendatum"), GetType(Label))
            Dim lblTerugbrengen As Label = Convert.ChangeType(e.Row.FindControl("lblTerugbrengen"), GetType(Label))

            Dim strGuid As String = lblUsername.Text

            lblUsername.Text = UsernameFromGUID(strGuid)

            Dim vndDatum As Date = Date.Now
            Dim terugDatum As Date = CDate(lblTerugbrengen.Text)
            Dim dblDagen As Double = Math.Ceiling((terugDatum - vndDatum).TotalDays)

            lblUitleendatum.Text &= "<br/>&nbsp;"

            If dblDagen >= 0 Then
                lblTerugbrengen.Text &= "<br/>(" & dblDagen.ToString() & " dagen)"
            Else
                lblUitleendatum.Text &= "<br/>&nbsp;"
                e.Row.BorderColor = Drawing.Color.FromArgb(153, 0, 0)
                e.Row.BorderStyle = BorderStyle.Solid
                e.Row.BorderWidth = 2
                e.Row.ForeColor = Drawing.Color.FromArgb(153, 0, 0)
                e.Row.Font.Bold = True
                lblTerugbrengen.Text &= "<br/>(" & Math.Abs(dblDagen).ToString() & " dagen te laat)"
            End If

            If Session("zoekterm-gebruiker") IsNot Nothing Then
                Dim strZoekTerm As String = Session("zoekterm-gebruiker")

                If strZoekTerm <> String.Empty Then
                    Dim blnVisible As Boolean = False

                    Dim strZoekwaarden() As String = strZoekTerm.Split(" ")
                    For Each zoekterm As String In strZoekwaarden
                        If lblUsername.Text.ToLower().Contains(zoekterm.ToLower()) Then
                            blnVisible = True
                        End If
                    Next

                    If Not blnVisible Then
                        e.Row.Visible = blnVisible
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub chkToonAlle_CheckedChanged(sender As Object, e As EventArgs) Handles chkToonAlle.CheckedChanged
        If Not chkToonAlle.Checked Then
            Dim strAND As String = String.Empty

            If dtsUitleningen.FilterExpression <> String.Empty Then
                strAND = " AND "
            End If

            dtsUitleningen.FilterExpression &= strAND & "Teruggebracht = 0"
        End If

        grdUitleningen.DataBind()
    End Sub

    Protected Sub btnZoek_Click(sender As Object, e As EventArgs) Handles btnZoekGebr.Click
        If txtZoekenGebr.Text.Trim() <> String.Empty Then
            Session("zoekterm-gebruiker") = txtZoekenGebr.Text.Trim().Replace("%", String.Empty).Replace("'", String.Empty)
            Session("zoekterm-boek") = String.Empty
            Response.Redirect(Request.RawUrl, False)
        Else
            Session("zoekterm-boek") = txtZoekenBoek.Text.Trim().Replace("%", String.Empty).Replace("'", String.Empty)
            Session("zoekterm-gebruiker") = String.Empty
            Response.Redirect(Request.RawUrl, False)
        End If
    End Sub

    Protected Sub btnLeeg_Click(sender As Object, e As EventArgs) Handles btnLeeg.Click
        Session("zoekterm-gebruiker") = String.Empty
        Session("zoekterm-boek") = String.Empty
        fvBoekdetails.Visible = False
        Response.Redirect(Request.RawUrl, False)
    End Sub

    Protected Sub btnZoekBoek_Click(sender As Object, e As EventArgs) Handles btnZoekBoek.Click
        If txtZoekenGebr.Text.Trim() <> String.Empty Then
            Session("zoekterm-gebruiker") = txtZoekenGebr.Text.Trim().Replace("%", String.Empty).Replace("'", String.Empty)
            Session("zoekterm-boek") = String.Empty
            Response.Redirect(Request.RawUrl, False)
        Else
            Session("zoekterm-boek") = txtZoekenBoek.Text.Trim().Replace("%", String.Empty).Replace("'", String.Empty)
            Session("zoekterm-gebruiker") = String.Empty
            Response.Redirect(Request.RawUrl, False)
        End If
    End Sub

    Protected Sub grdUitleningen_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdUitleningen.SelectedIndexChanged
        Dim lblISBN As Label = Convert.ChangeType(grdUitleningen.SelectedRow.FindControl("lblISBN"), GetType(Label))
        Dim strISBN = lblISBN.Text

        fvBoekdetails.Visible = True
        dtsBoekDetails.FilterExpression = "ISBN=" & strISBN
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
End Class