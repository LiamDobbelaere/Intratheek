Imports System.Data.SqlClient
Imports System.IO

Public Class boek_nieuw
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PageLoadOperations(litAangemeld, Session)

        If Session("beheerder") = False Then
            pnlWrapper.Visible = False
            lblBevoegd.Visible = True
        End If
    End Sub

    Protected Sub btnAanmaken_Click(sender As Object, e As EventArgs) Handles btnAanmaken.Click
        'Initialiseer een databaseverbinding
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        lblBestaat.Visible = False
        lblCoverVal.Visible = False
        lblCoverSize.Visible = False
        lblCoverForm.Visible = False

        If Not fuCover.HasFile Then
            'litAangemeld.Text &= fuCover.HasFile
            lblCoverVal.Visible = True
        End If

        Dim nieuwBoek As BoekInfo
        'Als alle validatiechecks in orde zijn
        If Page.IsValid Then
            '+ extra validatie voor coverafbeelding is in orde
            If fuCover.HasFile Then
                'We gaan het boek toevoegen, maak een boekobject aan om de info bij te houden
                nieuwBoek.ISBN = txtISBN.Text
                nieuwBoek.Titel = txtTitel.Text.Trim().PrepSQL()
                nieuwBoek.AuteurA = txtAuteurA.Text.Trim().PrepSQL()
                nieuwBoek.AuteurB = txtAuteurB.Text.Trim().PrepSQL()
                nieuwBoek.Jaar = txtJaar.Text
                nieuwBoek.Uitgeverij = txtUitgeverij.Text.Trim().PrepSQL()
                nieuwBoek.Bladzijden = txtBladzijden.Text.Trim()
                nieuwBoek.Flaptekst = txtFlaptekst.Text.Trim().PrepSQL()

                '[Stap 1] Kijk of het boek met dat ISBN nog niet bestaat
                myCmd = myConn.CreateCommand
                myCmd.CommandText = "SELECT COUNT(*) FROM tblBoeken " _
                                    & "WHERE ISBN = '" & nieuwBoek.ISBN & "'"

                Dim intBestaat As Integer = myCmd.ExecuteScalar()

                If intBestaat > 0 Then
                    lblBestaat.Visible = True
                Else
                    '[Stap 2] Kijk of de geüploadde cover geldig is
                    If fuCover.PostedFile.ContentType = "image/jpeg" Then
                        If fuCover.PostedFile.ContentLength <= 1000000 Then
                            'Sla de cover op in de map img/boeken met als bestandsnaam het ISBN
                            fuCover.SaveAs(Server.MapPath("~/img/boeken/") & nieuwBoek.ISBN & ".jpg")

                            '^ [Stap 3] Kijk of de Uitgeverij al bestaat of nog niet
                            myCmd = myConn.CreateCommand
                            myCmd.CommandText = "SELECT * FROM tblUitgeverijen"
                            myReader = myCmd.ExecuteReader()

                            '| Zorg dat er geen meerdere zelfde uitgeverijen komen met verschillende hoofdletters
                            Dim blnBestondAl As Boolean = False
                            While myReader.Read()
                                If myReader.GetString(0).ToUpper() = nieuwBoek.Uitgeverij.ToUpper() Then
                                    nieuwBoek.Uitgeverij = myReader.GetString(0)
                                    blnBestondAl = True
                                End If
                            End While
                            myReader.Close()

                            'v Als hij nog niet bestaat, voeg het dan eerst toe aan de database
                            If blnBestondAl = False Then
                                myCmd = myConn.CreateCommand
                                myCmd.CommandText = "INSERT INTO tblUitgeverijen (Naam) VALUES('" & nieuwBoek.Uitgeverij & "')"
                                myCmd.ExecuteNonQuery()
                            End If

                            '^ [Stap 4] Mooi, nu we zover zijn, kunnen we eerst en vooral het boek toevoegen
                            myCmd = myConn.CreateCommand
                            myCmd.CommandText = "INSERT INTO tblBoeken (ISBN, Titel, Jaar, Uitgeverij, Bladzijden, Flaptekst) " _
                                                & "VALUES('" & nieuwBoek.ISBN & "', '" & nieuwBoek.Titel & "', '" & nieuwBoek.Jaar & "', '" _
                                                & nieuwBoek.Uitgeverij & "', '" & nieuwBoek.Bladzijden & "', '" & nieuwBoek.Flaptekst & "');"
                            myCmd.ExecuteNonQuery()

                            '[Stap 5] Verwerk de auteurs op een efficiënte manier, Deforche zou trots zijn
                            VerwerkAuteur(nieuwBoek.ISBN, nieuwBoek.AuteurA)
                            VerwerkAuteur(nieuwBoek.ISBN, nieuwBoek.AuteurB)

                            '^ [Stap 6] Geweldig, nu voegen we nog de tags toe aan het boek die gebruikt worden in de zoekfunctie
                            If txtTags.Text.Trim <> String.Empty Then
                                Dim lstTags As List(Of String) = New List(Of String)
                                myCmd = myConn.CreateCommand
                                myCmd.CommandText = "SELECT * FROM tblGenres"
                                myReader = myCmd.ExecuteReader()

                                While myReader.Read()
                                    lstTags.Add(myReader.GetString(0).ToUpper())
                                End While
                                myReader.Close()

                                Dim strBoekTags As List(Of String) = txtTags.Text.Split(" ").ToList()

                                '| Voeg niet bestaande tags eerst toe aan tblGenres en link dan de tag ook aan het boek
                                For Each nieuweTag As String In strBoekTags
                                    'Start de tag met een hoofdletter, gevolgd door kleine letters
                                    Dim nieuweTag_verwerkt As String = nieuweTag.ToLower()
                                    nieuweTag_verwerkt = Char.ToUpper(nieuweTag.Substring(0, 1)) & nieuweTag.Substring(1)

                                    If Not lstTags.Contains(nieuweTag.ToUpper()) Then
                                        myCmd = myConn.CreateCommand
                                        myCmd.CommandText = "INSERT INTO tblGenres (GenreNaam) VALUES('" & nieuweTag_verwerkt & "')"
                                        myCmd.ExecuteNonQuery()
                                    End If

                                    myCmd = myConn.CreateCommand
                                    myCmd.CommandText = "INSERT INTO tblBoekGenres (ISBN, Genre) VALUES('" & nieuwBoek.ISBN & "', '" & nieuweTag_verwerkt & "')"
                                    myCmd.ExecuteNonQuery()
                                Next
                            End If
                            Response.Redirect("boekdetails.aspx?isbn=" & nieuwBoek.ISBN)
                        Else
                            lblCoverSize.Visible = True
                        End If
                    Else
                        lblCoverForm.Visible = True
                    End If
                End If
            End If
        End If

        myConn.Close()
    End Sub
End Class