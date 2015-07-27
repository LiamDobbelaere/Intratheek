Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Module Printout
    Public Sub Create(filename As String, options As PrintOutOptions, info As PrintOutInfo, items As List(Of BookEntry), logolocation As String, Optional showlogo As Boolean = True)
        Dim fs As FileStream = New FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)
        Dim doc As Document = New Document(New Rectangle(PageSize.A4), 5, 5, 54, 54)
        Dim writer As PdfWriter = PdfWriter.GetInstance(doc, fs)
        doc.Open()

        Dim table As PdfPTable = New PdfPTable(2)
        table.DefaultCell.BorderColor = options.tableBorderColor
        table.DefaultCell.Padding = 10
        table.DefaultCell.PaddingTop = 0

        FontFactory.RegisterDirectories()
        Dim normalFont As iTextSharp.text.Font = FontFactory.GetFont(options.fontName, options.normalFontSize)
        Dim normalFontAC As iTextSharp.text.Font = FontFactory.GetFont(options.fontName, options.normalFontSize, options.coloredTextColour)
        Dim normalFontACBOLD As iTextSharp.text.Font = FontFactory.GetFont(options.fontName, options.normalFontSize, 1, options.coloredTextColour)
        Dim normalFontWT As iTextSharp.text.Font = FontFactory.GetFont(options.fontName, options.normalFontSize, BaseColor.WHITE)
        Dim boldFont As iTextSharp.text.Font = FontFactory.GetFont(options.fontName, options.normalFontSize, 1)
        Dim bigFont As iTextSharp.text.Font = FontFactory.GetFont(options.fontName, options.bigFontSize, BaseColor.WHITE)

        'First row, cell two
        Dim img As iTextSharp.text.Image = Image.GetInstance(logolocation)
        img.ScalePercent(50)
        Dim cell As PdfPCell = New PdfPCell(img)
        cell.Border = 0

        cell.HorizontalAlignment = 0
        cell.VerticalAlignment = 1
        cell.PaddingTop = 5
        cell.PaddingBottom = 10
        cell.BorderColorBottom = options.backgroundColour
        cell.BorderWidthBottom = 2
        table.AddCell(cell)

        'First row, cell one
        cell = New PdfPCell(table.DefaultCell)
        cell.Border = 0
        Dim phrase As New Paragraph()
        phrase.Add(info.Gebruikersnaam & Environment.NewLine)
        phrase.Alignment = 2
        cell.PaddingTop = 20
        cell.AddElement(phrase)
        cell.PaddingBottom = 10
        cell.BorderColorBottom = options.backgroundColour
        cell.BorderWidthBottom = 2
        table.AddCell(cell)


        'Fourth row, spacing
        cell = New PdfPCell(table.DefaultCell)
        cell.Colspan = 2
        cell.FixedHeight = 30
        cell.Border = 0
        table.AddCell(cell)

        doc.Add(table)

        table = New PdfPTable(4)
        table.DefaultCell.BorderColor = options.tableBorderColor
        table.DefaultCell.Padding = 10
        table.DefaultCell.PaddingTop = 0
        Dim widths As Integer() = {20, 30, 25, 25}
        table.SetWidths(widths)

        'First row, heading one
        cell = New PdfPCell(table.DefaultCell)
        cell.AddElement(New Chunk("Boeknummer", normalFontWT))
        cell.BackgroundColor = options.backgroundColour
        cell.Border = 0
        table.AddCell(cell)

        'First row, heading two
        cell = New PdfPCell(table.DefaultCell)
        cell.AddElement(New Chunk("Boek", normalFontWT))
        cell.BackgroundColor = options.backgroundColour
        cell.Border = 0
        table.AddCell(cell)

        'First row, heading three
        cell = New PdfPCell(table.DefaultCell)
        cell.AddElement(New Chunk("Uitgeleend op", normalFontWT))
        cell.BackgroundColor = options.backgroundColour
        cell.Border = 0
        table.AddCell(cell)

        'First row, heading four
        cell = New PdfPCell(table.DefaultCell)
        cell.AddElement(New Chunk("Uitgeleend tot", normalFontWT))
        cell.BackgroundColor = options.backgroundColour
        cell.Border = 0
        table.AddCell(cell)

        Dim c As Paragraph

        For Each item As BookEntry In items
            c = New Paragraph(item.BoekNummer, normalFont)
            cell = New PdfPCell(table.DefaultCell)
            cell.AddElement(c)
            table.AddCell(cell)

            c = New Paragraph(item.BoekTitel, boldFont)
            Dim c2 As Paragraph = New Paragraph(item.BoekAuteur, normalFont)
            cell = New PdfPCell(table.DefaultCell)
            cell.AddElement(c)
            cell.AddElement(c2)
            table.AddCell(cell)

            c = New Paragraph(item.UitgeleendOp, normalFont)
            cell = New PdfPCell(table.DefaultCell)
            cell.AddElement(c)
            table.AddCell(cell)

            c = New Paragraph(item.UitgeleendTot, normalFont)
            cell = New PdfPCell(table.DefaultCell)
            cell.AddElement(c)
            table.AddCell(cell)
        Next

        doc.Add(table)

        doc.Close()
    End Sub

    Public Function CreateBookEntry(BoekNummer As String, BoekTitel As String, BoekAuteur As String, UitgeleendOp As String, UitgeleendTot As String) As BookEntry
        Dim pi As BookEntry
        pi.BoekNummer = BoekNummer
        pi.BoekTitel = BoekTitel
        pi.BoekAuteur = BoekAuteur
        pi.UitgeleendOp = UitgeleendOp
        pi.UitgeleendTot = UitgeleendTot
        Return pi
    End Function

    Public Structure PrintOutInfo
        Dim Gebruikersnaam As String
    End Structure

    Public Structure BookEntry
        Dim BoekNummer, BoekTitel, BoekAuteur, UitgeleendOp, UitgeleendTot As String
    End Structure

    Public Structure PrintOutOptions
        Dim tableBorderColor As BaseColor
        Dim fontName As String
        Dim normalFontSize As Integer
        Dim bigFontSize As Integer
        Dim backgroundColour As BaseColor
        Dim coloredTextColour As BaseColor
    End Structure
End Module
