Imports System.Security.Principal
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices

Module CommonMethods
    Public Sub PageLoadOperations(litAangemeld As Literal, Session As HttpSessionState)
        Dim wiGebruiker As WindowsIdentity = WindowsIdentity.GetCurrent()

        litAangemeld.Text = String.Empty

        If Session("leenlijst") IsNot Nothing Then
            Dim boekenlijst As List(Of String) = Session("leenlijst")

            If boekenlijst.Count = 0 Then
                litAangemeld.Text &= "<div id=leenlijst_uit><img src=""img/book_white.png"" /></div>"
            Else
                litAangemeld.Text &= "<a href=leenlijst.aspx><div id=leenlijst><div id=leennummer>" & boekenlijst.Count & "</div><img src=""img/book_white.png"" /></div></a>"
            End If
        Else
            litAangemeld.Text &= "<div id=leenlijst_uit><img src=""img/book_white.png"" /></div>"
        End If


        litAangemeld.Text &= "<b>" & wiGebruiker.GetUsername() & "</b>"

        If wiGebruiker.IsInGroup("IntratheekBeheer") Then
            litAangemeld.Text &= "<br/>" & "<a href=beheer.aspx>" & My.Settings.sBeheerder & "</a>"
            Session("beheerder") = True
        Else
            litAangemeld.Text &= "<br/>" & My.Settings.sGebruiker
            Session("beheerder") = False
        End If
    End Sub

    Structure BoekInfo
        Dim ISBN, Titel, AuteurA, AuteurB, Jaar, Uitgeverij, Bladzijden, Flaptekst, Tags As String
    End Structure

    Structure Auteur
        Dim ID As Integer
        Dim Voornaam, Achternaam As String
    End Structure

    Public Sub VerwerkAuteur(isbn As String, auteur As String)
        If auteur = String.Empty Then
            Exit Sub
        End If

        'Initialiseer een databaseverbinding
        Dim myConn As SqlConnection
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim blnBestondAl As Boolean

        myConn = New SqlConnection(ConfigurationManager.ConnectionStrings("IntratheekConnectionString").ConnectionString)
        myConn.Open()

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT * FROM tblAuteurs"
        myReader = myCmd.ExecuteReader()

        '| Zorg dat er geen meerdere zelfde auteurs zijn met verschillende hoofdletters
        blnBestondAl = False
        Dim dbAuteur As Auteur
        Dim intAuteurID As Integer
        While myReader.Read()
            dbAuteur.ID = myReader.GetInt32(0)
            dbAuteur.Voornaam = myReader.GetString(1).ToUpper()
            dbAuteur.Achternaam = myReader.GetString(2).ToUpper()

            'Voor het geval voornaam en achternaam verwisseld zijn
            Dim strNaam As String() = auteur.ToUpper().Split(" ")
            If dbAuteur.Voornaam = strNaam(0) Then
                'Doe enkel de check als de array groot genoeg is, voor auteurs zonder achternaam
                If strNaam.GetUpperBound(0) = 1 AndAlso dbAuteur.Achternaam = strNaam(1) Then
                    blnBestondAl = True
                    intAuteurID = dbAuteur.ID
                ElseIf strNaam.GetUpperBound(0) = 0 Then
                    blnBestondAl = True
                    intAuteurID = dbAuteur.ID
                End If
            ElseIf strNaam.GetUpperBound(0) = 1 AndAlso dbAuteur.Voornaam = strNaam(1) Then
                If dbAuteur.Achternaam = strNaam(0) Then
                    blnBestondAl = True
                    intAuteurID = dbAuteur.ID
                End If
            End If
        End While
        myReader.Close()

        'v Als de auteur nog niet bestond voegen we hem eerst toe, anders gebruiken we intAuteurID later
        If blnBestondAl = False Then
            Dim strAuteurNaam As String() = auteur.Split(" ")
            Dim strVoornaam, strAchternaam As String
            strVoornaam = strAuteurNaam(0)
            If strAuteurNaam.GetUpperBound(0) = 1 Then
                strAchternaam = strAuteurNaam(1)
            Else
                strAchternaam = String.Empty
            End If

            myCmd = myConn.CreateCommand
            myCmd.CommandText = "INSERT INTO tblAuteurs (Voornaam, Achternaam) OUTPUT Inserted.AuteurID VALUES('" & strVoornaam & "', '" & strAchternaam & "')"
            intAuteurID = myCmd.ExecuteScalar()
        End If

        'Voeg uiteindelijk de auteur al toe
        myCmd = myConn.CreateCommand
        myCmd.CommandText = "INSERT INTO tblBoekAuteurs (ISBN, AuteurID) " _
                            & "VALUES('" & isbn & "', '" & intAuteurID & "')"
        myCmd.ExecuteNonQuery()

        myConn.Close()
    End Sub

    <Extension()>
    Function NoVisibleRows(grdview As GridView)
        Dim blnNoVisibleRows As Boolean = True

        For Each row As GridViewRow In grdview.Rows
            If row.Visible = True Then
                blnNoVisibleRows = False
            End If
        Next

        Return blnNoVisibleRows
    End Function

    <Extension()>
    Function PrepSQL(text As String) As String
        Return text.Replace("'", "''")
    End Function

End Module
