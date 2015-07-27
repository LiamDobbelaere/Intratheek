<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="leenlijst.aspx.vb" Inherits="Intratheek.leenlijst" %>

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
		<h1>Boeken in leenlijst</h1>
            <p>
                De boeken in deze lijst staan klaar om uitgeleend te worden. Klik op de knop hieronder om de uitlening af te ronden of voeg nog boeken toe.<br /><br />
                <asp:Button ID="btnUitlenen" runat="server" CssClass="lenenKnop" Text="Deze boeken lenen" />
            </p>
            <asp:GridView ID="grdBoeken" runat="server" AutoGenerateColumns="False" BackColor="#F3F3F3" BorderStyle="None" CellPadding="8" CssClass="BoekenLijst" DataSourceID="dtsBoekenLijst" Font-Names="Verdana" GridLines="Horizontal" PageSize="5" Width="800px">
                <AlternatingRowStyle BackColor="#E2E2E2" />
                <Columns>
                    <asp:TemplateField HeaderText="Boeknummer" SortExpression="BoekNummer">
                        <EditItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("BoekNummer") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("BoekNummer") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="leenhead" />
                        <ItemStyle CssClass="boeknr" HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" SortExpression="BoekNummer">
                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("BoekNummer") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="imbCover" runat="server" BorderColor="#990000" BorderStyle="Solid" BorderWidth="2px" Height="162px" ImageUrl='<%# "img/boeken/" & Eval("ISBN") & ".jpg" %>' Width="113px" CommandArgument='<%# Eval("ISBN") %>' CommandName="Details" OnCommand="imbCover_Command" />
                            <br />
                        </ItemTemplate>
                        <HeaderStyle CssClass="hideme" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Label ID="lblTitel" runat="server" Font-Size="15pt" Text='<%# Eval("Titel") %>'></asp:Label>
                            <br />
                            <asp:Label ID="lblAuteur" runat="server" Font-Size="12pt" ForeColor="#666666" Text='<%# Eval("Auteurs") %>'></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="lblUitgeverij" runat="server" Font-Names="Verdana" Font-Size="10pt" ForeColor="#666666" Text='<%# "Uitgeverij " & Eval("Uitgeverij") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="hideme" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnLenen" runat="server" CssClass="lenenKnop" OnClick="btnLenen_Click" OnPreRender="btnLenen_PreRender" Text="Dit boek lenen" />
                            <asp:Label ID="lblISBN" runat="server" Text='<%# Eval("ISBN") %>' Visible="False"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="hideme" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="White" BorderStyle="None" />
                <PagerSettings Position="TopAndBottom" />
                <PagerStyle CssClass="pagerStyle" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:GridView>
            <asp:SqlDataSource ID="dtsBoekenLijst" runat="server" ConnectionString="<%$ ConnectionStrings:IntratheekConnectionString %>" SelectCommand="SELECT tblBibliotheek_1.BoekNummer, tblBoeken.ISBN, tblBoeken.Titel, dbo.GetBookAuthors(tblBoeken.ISBN) AS Auteurs, tblBoeken.Uitgeverij, tblBoeken.Bladzijden, COUNT(tblBibliotheek_1.BoekNummer) AS Exemplaren, tblBoeken.Flaptekst FROM tblBibliotheek AS tblBibliotheek_1 INNER JOIN tblBoeken ON tblBibliotheek_1.ISBN = tblBoeken.ISBN GROUP BY tblBoeken.ISBN, tblBoeken.Titel, tblBoeken.Uitgeverij, tblBoeken.Bladzijden, tblBoeken.Flaptekst, tblBibliotheek_1.BoekNummer ORDER BY tblBoeken.Titel"></asp:SqlDataSource>
		</div>
		<div id="footer">
			Intratheek &copy; 2015 Tom Dobbelaere
	    </div>
    </div>
    </form>
</body>
</html>
