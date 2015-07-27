<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="bibliotheekkaart.aspx.vb" Inherits="Intratheek.bibliotheekkaart" %>

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
				<li><a class="navlinkRS" href="bibliotheekkaart.aspx">Mijn bibliotheekkaart</a></li>
            </ul>
            <div id="signin">
                <asp:Literal ID="litAangemeld" runat="server"></asp:Literal>
            </div>
		</div>
            
		<div id="content">
		<h1>Mijn bibliotheekkaart</h1>
            <asp:Label ID="lblLeeg" runat="server" Text=""></asp:Label>
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
                        <HeaderStyle CssClass="bkhead" />
                        <ItemStyle CssClass="boeknr" HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" HeaderText="Boek">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlTitel" runat="server" CssClass="hyperkaart" Font-Bold="True" Font-Size="12pt" NavigateUrl='<%# "boekdetails.aspx?isbn=" & Eval("ISBN") %>' Text='<%# Eval("Titel") %>'></asp:HyperLink>
                            <br />
                            <asp:Label ID="lblAuteur" runat="server" Font-Size="11pt" Text='<%# Eval("Auteurs") %>'></asp:Label>
                            <br />
                        </ItemTemplate>
                        <HeaderStyle CssClass="bkhead" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Uitleendatum" SortExpression="Uitleendatum">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Uitleendatum") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUitleendatum" runat="server" Text='<%# Eval("Uitleendatum", "{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="bkhead" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Terugbrengen" SortExpression="Terugbrengen tegen">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("[Terugbrengen tegen]") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTerugbrengen" runat="server" Text='<%# Bind("[Terugbrengen tegen]", "{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle CssClass="bkhead" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnVerlengen" runat="server" CommandArgument='<%# Eval("ISBN") %>' CssClass="verlengKnop" OnClick="btnVerlengen_Click" Text="Verlengen" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="hideme" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnTerugbrengen" runat="server" CommandArgument='<%# Eval("ISBN") %>' CssClass="lenenKnop" OnClick="btnTerugbrengen_Click" Text="Terugbrengen" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="hideme" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="White" BorderStyle="None" />
                <PagerSettings Position="TopAndBottom" />
                <PagerStyle CssClass="pagerStyle" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:GridView>
            <asp:SqlDataSource ID="dtsBoekenLijst" runat="server" ConnectionString="<%$ ConnectionStrings:IntratheekConnectionString %>" SelectCommand="SELECT tblUitleningenBoeken.BoekNummer, tblBoeken.ISBN, tblBoeken.Titel, dbo.GetBookAuthors(tblBoeken.ISBN) AS Auteurs, tblBoeken.Uitgeverij, tblUitleningen.Uitleendatum, tblUitleningenBoeken.[Terugbrengen tegen], tblUitleningenBoeken.Teruggebracht FROM tblUitleningen INNER JOIN tblUitleningenBoeken ON tblUitleningen.UitleningID = tblUitleningenBoeken.UitleningID INNER JOIN tblBibliotheek ON tblBibliotheek.BoekNummer = tblUitleningenBoeken.BoekNummer INNER JOIN tblBoeken ON tblBibliotheek.ISBN = tblBoeken.ISBN WHERE (tblUitleningenBoeken.Teruggebracht = 0)"></asp:SqlDataSource>
		</div>
		<div id="footer">
			Intratheek &copy; 2015 Tom Dobbelaere
	    </div>
    </form>
</body>
</html>
