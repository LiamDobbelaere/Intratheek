<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="exemplarenbeheer.aspx.vb" Inherits="Intratheek.exemplarenbeheer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/style.css" rel="stylesheet" />
    <link href="favicon.ico" rel="shortcut icon" type="image/ico" />

    <title>Intratheek - BuSO</title>
<!-- my god<script language="JavaScript">
    window.onbeforeunload = confirmExit;
    function confirmExit()
    {
        return "Pas op, als je deze pagina verlaat, gaan de ingevulde gegevens verloeren!";
    }
</script>-->
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
		<h1>Exemplaren beheren</h1>
            <asp:Label ID="lblBevoegd" runat="server" Text="Sorry, je bent niet bevoegd om deze pagina te bekijken." Visible="False"></asp:Label>
            <asp:Panel ID="pnlWrapper" runat="server">
            <div id="searchpanel">
                <div id="exfloatleft">
            <center><asp:Image ID="imgCover" runat="server" BorderColor="#990000" BorderStyle="Solid" BorderWidth="2px" Height="162px" ImageAlign="Top" Width="113px" /></center>
                <center><asp:Literal ID="litBoekInfo" runat="server"></asp:Literal></center>
                </div>
                <div id="exmiddle">
                <asp:GridView ID="grdExemplaren" runat="server" AutoGenerateColumns="False" CssClass="BoekenLijst" DataKeyNames="BoekNummer" DataSourceID="dtsBoekExemplaren" BorderStyle="None" CellPadding="16" Font-Names="Verdana" GridLines="Horizontal" Height="57px" Width="202px">
                    <AlternatingRowStyle BackColor="#E2E2E2" />
                    <Columns>
                        <asp:TemplateField HeaderText="Boeknummer" SortExpression="BoekNummer">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("BoekNummer") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblNummer" runat="server" Text='<%# Bind("BoekNummer") %>'></asp:Label>
                                &nbsp;<asp:HyperLink ID="hplVerwijderen" runat="server" Font-Names="Verdana" Font-Underline="True" ForeColor="#990000" NavigateUrl='<%# "exemplaar_verwijderen.aspx?isbn=" & Eval("ISBN") & "&boeknummer=" & Eval("BoekNummer") %>'>Verwijderen</asp:HyperLink>
                            </ItemTemplate>
                            <HeaderStyle CssClass="leenhead" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle CssClass="boeknr" HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="dtsBoekExemplaren" runat="server" ConnectionString="<%$ ConnectionStrings:IntratheekConnectionString %>" SelectCommand="SELECT * FROM [tblBibliotheek]"></asp:SqlDataSource>
                <br />
                <asp:Label ID="lblNieuw" runat="server" Text="Nieuw exemplaar:" Width="200px"></asp:Label><br /><br />
                    <asp:DropDownList ID="ddlTalen" runat="server" AutoPostBack="True" CssClass="combotaal">
                        <asp:ListItem Value="1">1 - Poëzie</asp:ListItem>
                        <asp:ListItem Value="2">2 - Jeugd</asp:ListItem>
                        <asp:ListItem Value="3">3 - Volwassenen</asp:ListItem>
                        <asp:ListItem Value="4">4 - Engels</asp:ListItem>
                        <asp:ListItem Value="5">5 - Duits</asp:ListItem>
                        <asp:ListItem Value="6">6 - Frans</asp:ListItem>
                    </asp:DropDownList>
                <asp:TextBox ID="txtExNummer" runat="server" CssClass="search" MaxLength="3" Width="50px"></asp:TextBox><br /><br />
                <asp:Button ID="btnNieuwEx" runat="server" CssClass="lenenKnop" Text="Exemplaar toevoegen" Height="32px" /><br /><br />
                <asp:Button ID="btnAutomEx" runat="server" CssClass="lenenKnop" Text="Automatisch exemplaar toevoegen" Height="32px" /><br />
            
                <br /><asp:Label ID="lblBestaat" runat="server" ForeColor="#990000" Text="* Dit boeknummer bestaat al" Visible="false"></asp:Label>
                <br /><asp:RequiredFieldValidator ID="rfvNummer" runat="server" ControlToValidate="txtExNummer" ErrorMessage="RequiredFieldValidator" ForeColor="#990000">* Vul een nieuw boeknummer in</asp:RequiredFieldValidator>
                <br /><asp:RegularExpressionValidator ID="revNummer" runat="server" ControlToValidate="txtExNummer" ErrorMessage="RegularExpressionValidator" ForeColor="#990000" ValidationExpression="\d+">* Het nummer moet uit 3 cijfers bestaan</asp:RegularExpressionValidator>
                </div>
            </div>
            </asp:Panel>
        </div>
		<div id="footer">
			Intratheek &copy; 2015 Tom Dobbelaere
	    </div>
	</div>
    </form>
</body>
</html>
