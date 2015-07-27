Imports System.Security.Principal
Imports System.Data.SqlClient

Public Class bibliotheekkaart
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        dtsBoekenLijst.SelectCommand &= " AND ADGuid = CONVERT(binary(16),'" & WindowsIdentity.GetCurrent().GetGUID() & "', 2)"

        If grdBoeken.Rows.Count = 0 Then
            lblLeeg.Text = "Er zijn geen uitgeleende boeken op de bibliotheekkaart."
        End If

    End Sub

    Protected Sub imbCover_Command(sender As Object, e As CommandEventArgs)
        Response.Redirect("boekdetails.aspx?isbn=" & e.CommandArgument.ToString)
    End Sub

    Protected Sub grdBoeken_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdBoeken.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblUitleendatum As Label = Convert.ChangeType(e.Row.FindControl("lblUitleendatum"), GetType(Label))
            Dim lblTerugbrengen As Label = Convert.ChangeType(e.Row.FindControl("lblTerugbrengen"), GetType(Label))
            Dim btnVerlengen As Button = Convert.ChangeType(e.Row.FindControl("btnVerlengen"), GetType(Button))

            Dim vndDatum As Date = Date.Now
            Dim terugDatum As Date = CDate(lblTerugbrengen.Text)
            Dim dblDagen As Double = Math.Ceiling((terugDatum - vndDatum).TotalDays)

            lblUitleendatum.Text &= "<br/>&nbsp;"

            If dblDagen >= 0 Then
                lblTerugbrengen.Text &= "<br/>(" & dblDagen.ToString() & " dagen)"
            Else
                lblUitleendatum.Text &= "<br/>&nbsp;"
                lblTerugbrengen.ForeColor = Drawing.Color.FromArgb(153, 0, 0)
                lblTerugbrengen.Font.Bold = True
                lblTerugbrengen.Text &= "<br/>(" & Math.Abs(dblDagen).ToString() & " dagen te laat)"
            End If

            If Math.Ceiling((terugDatum - vndDatum).TotalDays) <= 7 Then
                btnVerlengen.Visible = True
            Else
                btnVerlengen.Visible = False
            End If

        End If
    End Sub

    Protected Sub btnVerlengen_Click(sender As Object, e As EventArgs)
        Dim self As Button = Convert.ChangeType(sender, GetType(Button))
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        'SQL date format: YYYY-MM-DDT00:00:00

        Dim strISBN As String = self.CommandArgument

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT [Terugbrengen tegen] FROM tblUitleningenBoeken " _
        & "INNER JOIN tblBibliotheek ON tblUitleningenBoeken.BoekNummer = tblBibliotheek.BoekNummer " _
        & "INNER JOIN tblUitleningen ON tblUitleningenBoeken.UitleningID = tblUitleningen.UitleningID " _
        & "WHERE ISBN = " & strISBN & " AND Teruggebracht = 0 AND ADGuid = CONVERT(binary(16),'" & WindowsIdentity.GetCurrent().GetGUID() & "', 2)"

        Dim dteTerugTegen As Date = myCmd.ExecuteScalar
        dteTerugTegen = dteTerugTegen.AddDays(21)

        Dim sCurdate As String = String.Empty
        sCurdate = dteTerugTegen.Year.ToString() & "-" & dteTerugTegen.Month.ToString("00") & "-" & dteTerugTegen.Day.ToString("00") & "T" & dteTerugTegen.Hour.ToString("00") & ":" & _
            dteTerugTegen.Minute.ToString("00") & ":" & dteTerugTegen.Second.ToString("00")

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "UPDATE tblUitleningenBoeken " _
        & "SET [Terugbrengen tegen] = '" & sCurdate & "' " _
        & "FROM tblUitleningenBoeken " _
        & "INNER JOIN tblBibliotheek ON tblUitleningenBoeken.BoekNummer = tblBibliotheek.BoekNummer " _
        & "INNER JOIN tblUitleningen ON tblUitleningenBoeken.UitleningID = tblUitleningen.UitleningID " _
        & "WHERE ISBN = " & strISBN & " AND Teruggebracht = 0 AND ADGuid = CONVERT(binary(16),'" & WindowsIdentity.GetCurrent().GetGUID() & "', 2)"

        myCmd.ExecuteNonQuery()

        myConn.Close()

        Response.Redirect(Request.RawUrl, False)
    End Sub

    Protected Sub btnTerugbrengen_Click(sender As Object, e As EventArgs)
        Dim self As Button = Convert.ChangeType(sender, GetType(Button))

        'litAangemeld.Text &= "Terugbrengen " & self.CommandArgument.ToString()
        'Exit Sub

        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        Dim strISBN As String = self.CommandArgument

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "UPDATE tblUitleningenBoeken " _
        & "SET Teruggebracht = 1 " _
        & "FROM tblUitleningenBoeken " _
        & "INNER JOIN tblBibliotheek ON tblUitleningenBoeken.BoekNummer = tblBibliotheek.BoekNummer " _
        & "INNER JOIN tblUitleningen ON tblUitleningenBoeken.UitleningID = tblUitleningen.UitleningID " _
        & "WHERE ISBN = " & strISBN & " AND Teruggebracht = 0 AND ADGuid = CONVERT(binary(16),'" & WindowsIdentity.GetCurrent().GetGUID() & "', 2)"

        myCmd.ExecuteNonQuery()

        myConn.Close()

        Response.Redirect(Request.RawUrl, False)
    End Sub
End Class