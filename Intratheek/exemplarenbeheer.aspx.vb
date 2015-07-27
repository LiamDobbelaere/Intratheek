Imports System.Data.SqlClient

Public Class exemplarenbeheer
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

        litBoekInfo.Text = "<b>"
        While myReader.Read()
            litBoekInfo.Text &= myReader.GetString(0)
        End While
        myReader.Close()
        litBoekInfo.Text &= "</b><br/> (ISBN " & strBoekNummer & ")"

        dtsBoekExemplaren.FilterExpression = "ISBN=" & strBoekNummer

        If Not Page.IsPostBack Then
            If grdExemplaren.Rows.Count > 0 Then
                Dim lblNummer As Label = Convert.ChangeType(grdExemplaren.Rows(0).Cells(0).FindControl("lblNummer"), GetType(Label))

                ddlTalen.SelectedValue = lblNummer.Text.Substring(0, 1)
            End If
        End If
    End Sub

    Protected Sub btnNieuwEx_Click(sender As Object, e As EventArgs) Handles btnNieuwEx.Click
        If Not Page.IsValid Then
            Exit Sub
        End If

        Dim strBoeknr As String = txtExNummer.Text

        If strBoeknr.Length = 2 Then
            strBoeknr = "0" & strBoeknr
        ElseIf strBoeknr.Length = 1 Then
            strBoeknr = "00" & strBoeknr
        End If

        strBoeknr = ddlTalen.SelectedValue.ToString() & strBoeknr

        'Oké, simpel, we voegen een nieuw record toe aan tblBibliotheek met het ISBN
        lblBestaat.Visible = False

        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        'Check eerst nog even of dit boeknummer al niet bestaat
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT COUNT(*) FROM tblBibliotheek " _
                            & "WHERE BoekNummer=" & strBoeknr
        Dim intBestaat As Integer = myCmd.ExecuteScalar()

        If intBestaat > 0 Then
            lblBestaat.Visible = True
            myConn.Close()
            Exit Sub
        End If

        'Voeg het exemplaar toe
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "INSERT INTO tblBibliotheek(BoekNummer, ISBN) " _
                            & "VALUES('" & strBoeknr & "', '" & strBoekNummer & "')"
        myCmd.ExecuteNonQuery()

        myConn.Close()

        Response.Redirect(Request.RawUrl, False)
    End Sub

    Protected Sub btnAutomEx_Click(sender As Object, e As EventArgs) Handles btnAutomEx.Click
        Dim shtBoeknummers As List(Of Short) = New List(Of Short)
        Dim shtBoekNummer As Short = 0

        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        'Check eerst nog even of dit ISBN wel bestaat
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT BoekNummer FROM tblBibliotheek " _
                            & "WHERE BoekNummer LIKE '" & ddlTalen.SelectedValue.ToString() & "%'"
        myReader = myCmd.ExecuteReader()

        While myReader.Read()
            shtBoeknummers.Add(CShort(myReader.GetString(0)))
        End While
        myReader.Close()

        For i As Integer = CShort(ddlTalen.SelectedValue) * 1000 To (CShort(ddlTalen.SelectedValue) * 1000) + 999
            If Not shtBoeknummers.Contains(i) Then
                shtBoekNummer = i
                Exit For
            End If
        Next

        'Voeg het exemplaar toe
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "INSERT INTO tblBibliotheek(BoekNummer, ISBN) " _
                            & "VALUES('" & shtBoekNummer.ToString() & "', '" & strBoekNummer & "')"
        myCmd.ExecuteNonQuery()

        myConn.Close()

        Response.Redirect(Request.RawUrl, False)
    End Sub
End Class