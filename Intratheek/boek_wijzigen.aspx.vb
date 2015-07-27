Imports System.Data.SqlClient
Imports System.IO

Public Class boek_wijzigen
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

        If Not Page.IsPostBack Then
            'Aan de hand van het ISBN gaan we alle tekstvakken proberen in te vullen
            'met de gegevens uit de database
            Dim dbBoekInfo As BoekInfo
            dbBoekInfo.ISBN = strBoekNummer

            'Initialiseer een databaseverbinding
            Dim myConn As SqlConnection
            Dim myCmd As SqlCommand
            Dim myReader As SqlDataReader

            myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
            myConn.Open()

            'Check eerst nog even of dit ISBN wel bestaat
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "SELECT COUNT(*) FROM tblBoeken " _
                                & "WHERE ISBN = '" & dbBoekInfo.ISBN & "'"
            Dim intBestaat As Integer = myCmd.ExecuteScalar()

            If intBestaat = 0 Then
                lblBoekBestaat.Visible = True
                pnlWrapper.Visible = False
                Exit Sub
            End If

            myCmd = myConn.CreateCommand
            myCmd.CommandText = "SELECT Titel, Jaar, Uitgeverij, Flaptekst, Bladzijden FROM tblBoeken " _
                                & "WHERE ISBN = '" & dbBoekInfo.ISBN & "'"
            myReader = myCmd.ExecuteReader()

            While myReader.Read()
                dbBoekInfo.Titel = myReader.GetString(0)
                dbBoekInfo.Jaar = myReader.GetString(1)
                dbBoekInfo.Uitgeverij = myReader.GetString(2)
                dbBoekInfo.Flaptekst = myReader.GetString(3)
                dbBoekInfo.Bladzijden = myReader.GetValue(4).ToString()
            End While
            myReader.Close()

            myCmd = myConn.CreateCommand
            myCmd.CommandText = "SELECT Voornaam, Achternaam FROM tblBoekAuteurs INNER JOIN tblAuteurs ON tblAuteurs.AuteurID = tblBoekAuteurs.AuteurID " _
                                & "WHERE ISBN = '" & dbBoekInfo.ISBN & "'"
            myReader = myCmd.ExecuteReader()

            While myReader.Read()
                If dbBoekInfo.AuteurA = String.Empty Then
                    dbBoekInfo.AuteurA = myReader.GetString(0) & " " & myReader.GetString(1)
                Else
                    dbBoekInfo.AuteurB = myReader.GetString(0) & " " & myReader.GetString(1)
                End If
            End While
            myReader.Close()

            Dim strTags As String = String.Empty
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "SELECT Genre FROM tblBoekGenres " _
                                & "WHERE ISBN = '" & dbBoekInfo.ISBN & "'"
            myReader = myCmd.ExecuteReader()

            While myReader.Read()
                strTags &= myReader.GetString(0) & " "
            End While
            myReader.Close()

            If strTags <> String.Empty Then
                strTags = strTags.Remove(strTags.Length - 1, 1)
            End If
            dbBoekInfo.Tags = strTags

            myConn.Close()

            txtISBN.Text = dbBoekInfo.ISBN
            txtTitel.Text = dbBoekInfo.Titel
            txtJaar.Text = dbBoekInfo.Jaar
            txtUitgeverij.Text = dbBoekInfo.Uitgeverij
            txtFlaptekst.Text = dbBoekInfo.Flaptekst
            txtBladzijden.Text = dbBoekInfo.Bladzijden
            txtAuteurA.Text = dbBoekInfo.AuteurA
            txtAuteurB.Text = dbBoekInfo.AuteurB
            txtTags.Text = dbBoekInfo.Tags

            Session("oudboek") = dbBoekInfo
        End If
    End Sub

    Protected Sub btnBijwerken_Click(sender As Object, e As EventArgs) Handles btnBijwerken.Click
        Dim oudBoek As BoekInfo = Convert.ChangeType(Session("oudboek"), GetType(BoekInfo))

        'Initialiseer een databaseverbinding
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        lblBestaat.Visible = False
        lblCoverSize.Visible = False
        lblCoverForm.Visible = False

        Dim nieuwBoek As BoekInfo
        'Als alle validatiechecks in orde zijn
        If Page.IsValid Then
            'Oké, de persoon kan een ander ISBN toekennen
            'Dit ISBN mag niet gelijk zijn aan een ander bestaand boek

            nieuwBoek.ISBN = txtISBN.Text
            nieuwBoek.AuteurA = txtAuteurA.Text.PrepSQL()
            nieuwBoek.AuteurB = txtAuteurB.Text.PrepSQL()
            nieuwBoek.Flaptekst = txtFlaptekst.Text.PrepSQL()
            nieuwBoek.Bladzijden = txtBladzijden.Text
            nieuwBoek.Jaar = txtJaar.Text
            nieuwBoek.Uitgeverij = txtUitgeverij.Text.PrepSQL()
            nieuwBoek.Titel = txtTitel.Text.PrepSQL()
            nieuwBoek.Tags = txtTags.Text.PrepSQL()

            If nieuwBoek.ISBN <> oudBoek.ISBN Then
                'litAangemeld.Text &= nieuwBoek.ISBN & " " & oudBoek.ISBN

                '[Stap 1] Kijk of een boek met dat ISBN nog niet bestaat
                myCmd = myConn.CreateCommand
                myCmd.CommandText = "SELECT COUNT(*) FROM tblBoeken " _
                                    & "WHERE ISBN = '" & nieuwBoek.ISBN & "'"
                Dim intBestaat As Integer = myCmd.ExecuteScalar()

                If intBestaat > 0 Then
                    lblBestaat.Visible = True
                    myConn.Close()
                    Exit Sub
                End If
            End If

            '[Stap 2] Nu kijken we eerst nog of de coverafbeelding geldig is
            If fuCover.HasFile Then
                If fuCover.PostedFile.ContentType = "image/jpeg" Then
                    If fuCover.PostedFile.ContentLength <= 1000000 Then
                        'Sla de cover op in de map img/boeken met als bestandsnaam het ISBN
                        fuCover.SaveAs(Server.MapPath("~/img/boeken/") & nieuwBoek.ISBN & ".jpg")
                    Else
                        lblCoverSize.Visible = True
                        myConn.Close()
                        Exit Sub
                    End If
                Else
                    lblCoverForm.Visible = True
                    myConn.Close()
                    Exit Sub
                End If
            End If

            '^ [Stap 3] Als de uitgeverij anders is, moeten we eerst zorgen
            'dat deze uitgeverij ook al in de database zit
            If nieuwBoek.Uitgeverij <> oudBoek.Uitgeverij Then
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
            End If

            'Verwijder eventuele constraints als het ISBN wijzigt!
            If (nieuwBoek.ISBN <> oudBoek.ISBN) Or (nieuwBoek.AuteurA <> oudBoek.AuteurA) Or (nieuwBoek.AuteurB <> oudBoek.AuteurB) Then
                'Verwijder alle oude auteurs
                myCmd = myConn.CreateCommand
                myCmd.CommandText = "DELETE FROM tblBoekAuteurs " _
                                    & "WHERE ISBN = '" & oudBoek.ISBN & "'"
                myCmd.ExecuteNonQuery()
            End If

            If (nieuwBoek.ISBN <> oudBoek.ISBN) Or (nieuwBoek.Tags <> oudBoek.Tags) Then
                'Verwijder alle oude tags
                myCmd = myConn.CreateCommand
                myCmd.CommandText = "DELETE FROM tblBoekGenres " _
                                    & "WHERE ISBN = '" & oudBoek.ISBN & "'"
                myCmd.ExecuteNonQuery()
            End If

            'Unlink de tabel tblBibliotheek
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "ALTER TABLE tblBibliotheek NOCHECK CONSTRAINT tblBibliotheek$tblBoekentblBibliotheek"
            myCmd.ExecuteNonQuery()


            'Pas ook eerst alle records in tblBibliotheek aan 
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "UPDATE tblBibliotheek " _
                                & "SET ISBN=" & nieuwBoek.ISBN _
                                & " WHERE ISBN = '" & oudBoek.ISBN & "'"
            myCmd.ExecuteNonQuery()

            'De ISBN, cover en uitgeverij zijn nu in orde, nu kunnen we het boek beginnen wegschrijven
            '[Stap 4] Schrijf de hoofdgegevens weg naar tblBoeken
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "UPDATE tblBoeken " _
                                & "SET ISBN='" & nieuwBoek.ISBN & "',Titel='" & nieuwBoek.Titel & "',Jaar='" & nieuwBoek.Jaar & "',Uitgeverij='" & nieuwBoek.Uitgeverij _
                                & "',Bladzijden='" & nieuwBoek.Bladzijden & "',Flaptekst='" & nieuwBoek.Flaptekst _
                                & "' WHERE ISBN = '" & oudBoek.ISBN & "'"
            myCmd.ExecuteNonQuery()

            'Relink de tabel tblBibliotheek
            myCmd = myConn.CreateCommand
            myCmd.CommandText = "ALTER TABLE tblBibliotheek WITH CHECK CHECK CONSTRAINT tblBibliotheek$tblBoekentblBibliotheek"
            myCmd.ExecuteNonQuery()

            '[Stap 5] Om de auteurs weg te schrijven (als ze anders zijn, tenminste)
            'verwijderen we eerst alle auteurs en voegen dan de nieuwe toe
            'Dit is het simpelste in het geval dat er een auteur minder zou zijn dan voorheen bijvoorbeeld
            If (nieuwBoek.ISBN <> oudBoek.ISBN) Or (nieuwBoek.AuteurA <> oudBoek.AuteurA) Or (nieuwBoek.AuteurB <> oudBoek.AuteurB) Then
                VerwerkAuteur(nieuwBoek.ISBN, nieuwBoek.AuteurA)
                VerwerkAuteur(nieuwBoek.ISBN, nieuwBoek.AuteurB)
            End If

            '[Stap 6] Als laatste verwijderen we alle geassocieerde tags als die aangepast zijn,
            'en voegen ze eventueel opnieuw toe
            If (nieuwBoek.ISBN <> oudBoek.ISBN) Or (nieuwBoek.Tags <> oudBoek.Tags) Then
                'Verwijder alle oude tags
                myCmd = myConn.CreateCommand
                myCmd.CommandText = "DELETE FROM tblBoekGenres " _
                                    & "WHERE ISBN = '" & oudBoek.ISBN & "'"
                myCmd.ExecuteNonQuery()

                If nieuwBoek.Tags.Trim <> String.Empty Then
                    Dim lstTags As List(Of String) = New List(Of String)
                    myCmd = myConn.CreateCommand
                    myCmd.CommandText = "SELECT * FROM tblGenres"
                    myReader = myCmd.ExecuteReader()

                    While myReader.Read()
                        lstTags.Add(myReader.GetString(0).ToUpper())
                    End While
                    myReader.Close()

                    Dim strBoekTags As List(Of String) = nieuwBoek.Tags.Split(" ").ToList()

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
            End If

            '[Stap 7] Als het ISBN wijzigt, werk de cover ook bij
            If (nieuwBoek.ISBN <> oudBoek.ISBN) Then
                If File.Exists(Server.MapPath("img/boeken/" & oudBoek.ISBN & ".jpg")) And Not fuCover.HasFile Then
                    File.Copy(Server.MapPath("img/boeken/" & oudBoek.ISBN & ".jpg"), Server.MapPath("img/boeken/" & nieuwBoek.ISBN & ".jpg"))
                    File.Delete(Server.MapPath("img/boeken/" & oudBoek.ISBN & ".jpg"))
                End If

                If fuCover.HasFile Then
                    File.Delete(Server.MapPath("img/boeken/" & oudBoek.ISBN & ".jpg"))
                End If
            End If
        End If

        myConn.Close()

        Response.Redirect("boekdetails.aspx?isbn=" & nieuwBoek.ISBN)
    End Sub
End Class