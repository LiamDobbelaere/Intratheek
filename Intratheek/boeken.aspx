<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="boeken.aspx.vb" Inherits="Intratheek.boeken" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
				<li><a class="navlinkLS" href="boeken.aspx">Boeken</a></li>
				<li><a class="navlinkR" href="bibliotheekkaart.aspx">Mijn bibliotheekkaart</a></li>
            </ul>
            <div id="signin">
                <asp:Literal ID="litAangemeld" runat="server"></asp:Literal>
            </div>
		</div>
            
		<div id="content">
		<h1>Boeken</h1>
            <div id="searchpanel">
                <asp:Label ID="lblZoeken" runat="server" Font-Size="12pt" Text="Zoeken:" Width="100px"></asp:Label>
                <asp:TextBox ID="txtZoeken" runat="server" CssClass="search"></asp:TextBox>
                <asp:Button ID="btnZoek" runat="server" CssClass="searchbutton" Text="Zoeken"/>
                <br /><br />
                <asp:Label ID="lblTaal" runat="server" Font-Size="12pt" Text="Categorie:" Width="100px"></asp:Label>
                <asp:DropDownList ID="ddlTalen" runat="server" CssClass="combotaal" AutoPostBack="True">
                    <asp:ListItem Value="0">Geen filter</asp:ListItem>
                    <asp:ListItem Value="1">Poëzie</asp:ListItem>
                    <asp:ListItem Value="2">Jeugd</asp:ListItem>
                    <asp:ListItem Value="3">Volwassenen</asp:ListItem>
                    <asp:ListItem Value="4">Engels</asp:ListItem>
                    <asp:ListItem Value="5">Duits</asp:ListItem>
                    <asp:ListItem Value="6">Frans</asp:ListItem>
                </asp:DropDownList>
                <br /><br />
                <asp:CheckBox ID="chkToonAlle" runat="server" Text="Toon niet-toegekende boeken" AutoPostBack="True" Visible="False" />
                <br /><br />
                <asp:Button ID="btnLeeg" runat="server" CssClass="clearbutton" Text="Filters wissen"/>
            </div>
            <asp:Label ID="lblGeenRes" runat="server" Text="Er zijn geen boeken gevonden." Visible="False"></asp:Label><br />
            <asp:Label ID="lblFilter" runat="server" Text="Filter: " Visible="False"></asp:Label><br /><br />
            <asp:GridView ID="grdBoeken" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="#F3F3F3" BorderStyle="None" CellPadding="8" CssClass="BoekenLijst" DataSourceID="dtsBoekenLijst" Font-Names="Verdana" GridLines="Horizontal" PageSize="5" ShowHeader="False">
                <AlternatingRowStyle BackColor="#E2E2E2" />
                <Columns>
                    <asp:TemplateField ShowHeader="False" SortExpression="ISBN" HeaderText="Cover">
                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ISBN") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="imbCover" runat="server" BorderColor="#990000" BorderStyle="Solid" BorderWidth="2px" Height="162px" ImageUrl='<%# "img/boeken/" & Eval("ISBN") & ".jpg" %>' Width="113px" CommandArgument='<%# Eval("ISBN") %>' CommandName="Details" OnCommand="imbCover_Command" />
                            <br />
                            <asp:HyperLink ID="hplWijzigen" runat="server" Font-Bold="False" Font-Underline="True" ForeColor="#990000" NavigateUrl='<%# "boek_wijzigen.aspx?isbn=" & Eval("ISBN") %>' Visible="False">Wijzigen</asp:HyperLink>
                            <asp:HyperLink ID="hplVerwijderen" runat="server" Font-Bold="False" Font-Underline="True" ForeColor="#990000" NavigateUrl='<%# "boek_verwijderen.aspx?isbn=" & Eval("ISBN") %>' Visible="False">Verwijderen</asp:HyperLink>
                            <asp:HyperLink ID="hplExemplaren" runat="server" Font-Bold="False" Font-Underline="True" ForeColor="#990000" NavigateUrl='<%# "exemplarenbeheer.aspx?isbn=" & Eval("ISBN") %>' Visible="False">Exemplaren beheren</asp:HyperLink>
                        </ItemTemplate>
                        <HeaderStyle CssClass="leenhead" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Info">
                        <ItemTemplate>
                            <asp:Label ID="lblTitel" runat="server" Font-Size="15pt" Text='<%# Eval("Titel") %>'></asp:Label>
                            <br />
                            <asp:Label ID="lblAuteur" runat="server" Font-Size="12pt" ForeColor="#666666" Text='<%# Eval("Auteurs") %>'></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="lblUitgeverij" runat="server" Font-Names="Verdana" Font-Size="10pt" ForeColor="#666666" Text='<%# "Uitgeverij " & Eval("Uitgeverij") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="leenhead" />
                        <ItemStyle Width="250px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Flaptekst" SortExpression="Flaptekst">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Flaptekst") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Flaptekst") %>'></asp:Label>
                            <asp:HyperLink ID="hplMeer" runat="server" Font-Bold="True" Font-Underline="False" ForeColor="#3366FF" Visible="False" NavigateUrl='<%# "boekdetails.aspx?isbn=" & Eval("ISBN")%>'>meer »</asp:HyperLink>
                        </ItemTemplate>
                        <HeaderStyle CssClass="leenhead" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Position="TopAndBottom" />
                <PagerStyle CssClass="pagerStyle" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:GridView>
            <asp:SqlDataSource ID="dtsBoekenLijst" runat="server" ConnectionString="<%$ ConnectionStrings:IntratheekConnectionString %>" SelectCommand="SELECT (SELECT TOP (1) BoekNummer FROM tblBibliotheek WHERE (ISBN = tblBoeken.ISBN)) AS BoekNummer, tblBoeken.ISBN, tblBoeken.Titel, dbo.GetBookAuthors(tblBoeken.ISBN) AS Auteurs, tblBoeken.Uitgeverij, tblBoeken.Bladzijden, COUNT(tblBibliotheek_1.BoekNummer) AS Exemplaren, tblBoeken.Flaptekst, dbo.GetBookGenres(tblBoeken.ISBN) AS Tags FROM tblBibliotheek AS tblBibliotheek_1 RIGHT OUTER JOIN tblBoeken ON tblBibliotheek_1.ISBN = tblBoeken.ISBN GROUP BY tblBoeken.ISBN, tblBoeken.Titel, tblBoeken.Uitgeverij, tblBoeken.Bladzijden, tblBoeken.Flaptekst ORDER BY tblBoeken.Titel"></asp:SqlDataSource>
		</div>
		<div id="footer">
			Intratheek &copy; 2015 Tom Dobbelaere
	    </div>
	</div>
    </form>
</body>
</html>
