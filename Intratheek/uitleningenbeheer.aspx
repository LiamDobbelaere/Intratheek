<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="uitleningenbeheer.aspx.vb" Inherits="Intratheek.uitleningenbeheer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/style.css" rel="stylesheet" />
    <link href="favicon.ico" rel="shortcut icon" type="image/ico" />

    <title>Intratheek - BuSO</title>
</head>
<body>
    <form id="frmMain" runat="server">
    <div id="container">
		<div id="navbar">
			<div id="logo"><a href="default.aspx"><img src="img/logo.png" width="100%"/></a></div>
			<ul>
				<li><a class="navlinkL" href="boeken.aspx">Boeken</a></li>
				<li><a class="navlinkR" href="bibliotheekkaart.aspx">Mijn bibliotheekkaart</a></li>
            </ul>
            <div id="signin">
                <asp:Literal ID="litAangemeld" runat="server"></asp:Literal>
            </div>
		</div>
            
		<div id="content">
		<h1>Uitleningen beheren</h1>
            <asp:Label ID="lblBevoegd" runat="server" Text="Sorry, je bent niet bevoegd om deze pagina te bekijken." Visible="False"></asp:Label>
            <asp:Panel ID="pnlWrapper" runat="server">
            <div id="searchpanel">
                <asp:Label ID="lblZoekenGebr" runat="server" Font-Size="12pt" Text="Zoeken op gebruiker:" Width="200px"></asp:Label>
                <asp:TextBox ID="txtZoekenGebr" runat="server" CssClass="search"></asp:TextBox>
                <asp:Button ID="btnZoekGebr" runat="server" CssClass="searchbutton" Text="Zoeken"/>
                <br /><br />
                <asp:Label ID="lblZoekenBoek" runat="server" Font-Size="12pt" Text="Zoeken op boek:" Width="200px"></asp:Label>
                <asp:TextBox ID="txtZoekenBoek" runat="server" CssClass="search"></asp:TextBox>
                <asp:Button ID="btnZoekBoek" runat="server" CssClass="searchbutton" Text="Zoeken"/>
                <br /><br />
                <asp:CheckBox ID="chkToonAlle" runat="server" Text="Toon teruggebrachte boeken" AutoPostBack="True" Visible="True" />
                <br /><br />
                <asp:Button ID="btnLeeg" runat="server" CssClass="clearbutton" Text="Filters wissen"/>
            </div>
                <asp:Label ID="lblGeenRes" runat="server" Text="Er zijn geen boeken gevonden." Visible="False"></asp:Label><br />
                <asp:Label ID="lblFilter" runat="server" Text="Filter: " Visible="False"></asp:Label><br /><br />
                <asp:GridView ID="grdUitleningen" runat="server" AutoGenerateColumns="False" BackColor="#F3F3F3" BorderStyle="None" CellPadding="8" CssClass="BoekenLijst" DataSourceID="dtsUitleningen" Font-Names="Verdana" GridLines="Horizontal" style="margin-right: 0px" AllowSorting="True">
                    <AlternatingRowStyle BackColor="#E2E2E2" />
                    <Columns>
                        <asp:TemplateField HeaderText="Naam" SortExpression="ADGuid">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ADGuid") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUsername" runat="server" Text='<%# BitConverter.ToString(Eval("ADGuid")).Replace("-","").ToLower() %>' Font-Names="Verdana"></asp:Label>
                                <asp:Label ID="lblISBN" runat="server" Font-Names="Verdana" Text='<%# Eval("ISBN") %>' Visible="False"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="250px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nummer" SortExpression="BoekNummer">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("BoekNummer") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBoeknr" runat="server" Text='<%# Bind("BoekNummer") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="boeknr" HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Uitleendatum" SortExpression="Uitleendatum">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Uitleendatum") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUitleendatum" runat="server" Text='<%# Bind("Uitleendatum", "{0:d}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Terugbrengen" SortExpression="Terugbrengen tegen">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("[Terugbrengen tegen]") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTerugbrengen" runat="server" Text='<%# Bind("[Terugbrengen tegen]", "{0:d}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Terug?" SortExpression="Teruggebracht">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Teruggebracht") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Image ID="imgChkno" runat="server" Height="24px" ImageUrl="~/img/chk_off.png" Visible='<%# Not Eval("Teruggebracht") %>' Width="24px" />
                                <asp:Image ID="imgChkyes" runat="server" Height="24px" ImageUrl="~/img/chk_on.png" Visible='<%# Eval("Teruggebracht") %>' Width="24px" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select" Font-Bold="False" ForeColor="#990000" Text="Details.."></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="leenhead" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <RowStyle Height="64px" />
                    <SelectedRowStyle BackColor="#ECD7D7" />
                    <SortedAscendingHeaderStyle CssClass="leenhead" Font-Bold="False" Font-Underline="False" />
                    <SortedDescendingHeaderStyle CssClass="leenhead" />
                </asp:GridView>
                <asp:SqlDataSource ID="dtsUitleningen" runat="server" ConnectionString="<%$ ConnectionStrings:IntratheekConnectionString %>" SelectCommand="SELECT tblUitleningen.UitleningID, tblUitleningen.ADGuid, tblUitleningen.Uitleendatum, tblUitleningenBoeken.BoekNummer, tblUitleningenBoeken.[Terugbrengen tegen], tblUitleningenBoeken.Teruggebracht, tblBoeken.ISBN, tblBoeken.Titel, dbo.GetBookAuthors(tblBoeken.ISBN) AS Auteurs, dbo.GetBookGenres(tblBoeken.ISBN) AS Tags FROM tblUitleningenBoeken INNER JOIN tblUitleningen ON tblUitleningenBoeken.UitleningID = tblUitleningen.UitleningID INNER JOIN tblBibliotheek ON tblUitleningenBoeken.BoekNummer = tblBibliotheek.BoekNummer INNER JOIN tblBoeken ON tblBibliotheek.ISBN = tblBoeken.ISBN"></asp:SqlDataSource>
                <br /><br />
                <asp:FormView ID="fvBoekdetails" runat="server" DataKeyNames="ISBN" DataSourceID="dtsBoekDetails" Font-Names="Verdana" HorizontalAlign="Justify">
                <EditItemTemplate>
                    BoekNummer:
                    <asp:TextBox ID="BoekNummerTextBox" runat="server" Text='<%# Bind("BoekNummer") %>' />
                    <br />
                    ISBN:
                    <asp:Label ID="ISBNLabel1" runat="server" Text='<%# Eval("ISBN") %>' />
                    <br />
                    Titel:
                    <asp:TextBox ID="TitelTextBox" runat="server" Text='<%# Bind("Titel") %>' />
                    <br />
                    Auteurs:
                    <asp:TextBox ID="AuteursTextBox" runat="server" Text='<%# Bind("Auteurs") %>' />
                    <br />
                    Uitgeverij:
                    <asp:TextBox ID="UitgeverijTextBox" runat="server" Text='<%# Bind("Uitgeverij") %>' />
                    <br />
                    Bladzijden:
                    <asp:TextBox ID="BladzijdenTextBox" runat="server" Text='<%# Bind("Bladzijden") %>' />
                    <br />
                    Jaar:
                    <asp:TextBox ID="JaarTextBox" runat="server" Text='<%# Bind("Jaar") %>' />
                    <br />
                    Exemplaren:
                    <asp:TextBox ID="ExemplarenTextBox" runat="server" Text='<%# Bind("Exemplaren") %>' />
                    <br />
                    Flaptekst:
                    <asp:TextBox ID="FlaptekstTextBox" runat="server" Text='<%# Bind("Flaptekst") %>' />
                    <br />
                    Genres:
                    <asp:TextBox ID="GenresTextBox" runat="server" Text='<%# Bind("Genres") %>' />
                    <br />
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                    &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    BoekNummer:
                    <asp:TextBox ID="BoekNummerTextBox" runat="server" Text='<%# Bind("BoekNummer") %>' />
                    <br />
                    ISBN:
                    <asp:TextBox ID="ISBNTextBox" runat="server" Text='<%# Bind("ISBN") %>' />
                    <br />
                    Titel:
                    <asp:TextBox ID="TitelTextBox" runat="server" Text='<%# Bind("Titel") %>' />
                    <br />
                    Auteurs:
                    <asp:TextBox ID="AuteursTextBox" runat="server" Text='<%# Bind("Auteurs") %>' />
                    <br />
                    Uitgeverij:
                    <asp:TextBox ID="UitgeverijTextBox" runat="server" Text='<%# Bind("Uitgeverij") %>' />
                    <br />
                    Bladzijden:
                    <asp:TextBox ID="BladzijdenTextBox" runat="server" Text='<%# Bind("Bladzijden") %>' />
                    <br />
                    Jaar:
                    <asp:TextBox ID="JaarTextBox" runat="server" Text='<%# Bind("Jaar") %>' />
                    <br />
                    Exemplaren:
                    <asp:TextBox ID="ExemplarenTextBox" runat="server" Text='<%# Bind("Exemplaren") %>' />
                    <br />
                    Flaptekst:
                    <asp:TextBox ID="FlaptekstTextBox" runat="server" Text='<%# Bind("Flaptekst") %>' />
                    <br />
                    Genres:
                    <asp:TextBox ID="GenresTextBox" runat="server" Text='<%# Bind("Genres") %>' />
                    <br />
                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                </InsertItemTemplate>
                <ItemTemplate>
                    <div id="detailsA">
                    <asp:Image ID="imgCover" runat="server" CssClass="detailBook" Height="162px" ImageAlign="Left" ImageUrl='<%# "img/boeken/" & Eval("ISBN") & ".jpg"%>' Width="113px" />
                    <asp:Label ID="lblTitel" runat="server" Font-Size="15pt" Text='<%# Eval("Titel") %>'></asp:Label>
                    <br />
                    <asp:Label ID="lblAuteur" runat="server" Font-Size="12pt" ForeColor="#666666" Text='<%# Eval("Auteurs") %>'></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblUitgeverij" runat="server" Font-Names="Verdana" Font-Size="10pt" ForeColor="#666666" Text='<%# "Uitgeverij " & Eval("Uitgeverij") %>'></asp:Label>
                    <br /><br />
                        <br />
                    <br />
                    <br />
                     <br />
                    <asp:Label ID="Label3" runat="server" Font-Size="10pt" Text="ISBN:" Width="145px"></asp:Label>
                    <asp:Label ID="lblISBN" runat="server" Font-Size="10pt" Text='<%# Eval("ISBN") %>' Font-Bold="False" />
                    <br />
                    <asp:Label ID="Label2" runat="server" Font-Size="10pt" style="margin-bottom: 0px" Text="Aantal bladzijden:" Width="145px"></asp:Label>
                    <asp:Label ID="lblBladzijden" runat="server" Text='<%# Eval("Bladzijden") %>' Font-Size="10pt" />
                    <br />
                    <asp:Label ID="Label1" runat="server" Font-Size="10pt" Text="Jaar: " Width="145px"></asp:Label>
                    <asp:Label ID="lblJaar" runat="server" Text='<%# Eval("Jaar") %>' Font-Size="10pt" />
                        </div><div id="detailsB">
                    <asp:Label ID="lblFlaptekst" runat="server" Text='<%# Bind("Flaptekst") %>' />
                    <br /><br />
                    <asp:Literal ID="litGenres" runat="server" OnDataBinding="litGenres_DataBinding" Text='<%# Eval("Genres") %>'></asp:Literal></div>

                </ItemTemplate>
            </asp:FormView>
            <asp:SqlDataSource ID="dtsBoekDetails" runat="server" ConnectionString="<%$ ConnectionStrings:IntratheekConnectionString %>" SelectCommand="SELECT tblBoeken.ISBN, tblBoeken.Titel, dbo.GetBookAuthors(tblBoeken.ISBN) AS Auteurs, tblBoeken.Uitgeverij, tblBoeken.Bladzijden, tblBoeken.Jaar, COUNT(tblBibliotheek_1.BoekNummer) AS Exemplaren, tblBoeken.Flaptekst, dbo.GetBookGenres(tblBoeken.ISBN) AS Genres FROM tblBibliotheek AS tblBibliotheek_1 RIGHT OUTER JOIN tblBoeken ON tblBibliotheek_1.ISBN = tblBoeken.ISBN GROUP BY tblBoeken.ISBN, tblBoeken.Titel, tblBoeken.Uitgeverij, tblBoeken.Bladzijden, tblBoeken.Flaptekst, tblBoeken.Jaar ORDER BY tblBoeken.Titel"></asp:SqlDataSource>
            </asp:Panel>
        </div>
		<div id="footer">
			Intratheek &copy; 2015 Tom Dobbelaere
	    /div>
	</div>
    </form>
</body>
</html>
