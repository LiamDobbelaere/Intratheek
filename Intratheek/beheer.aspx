<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="beheer.aspx.vb" Inherits="Intratheek.beheer" %>

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
				<li><a class="navlinkL" href="boeken.aspx">Boeken</a></li>
				<li><a class="navlinkR" href="bibliotheekkaart.aspx">Mijn bibliotheekkaart</a></li>
            </ul>
            <div id="signin">
                <asp:Literal ID="litAangemeld" runat="server"></asp:Literal>
            </div>
		</div>
            
		<div id="content">
		<h1>Beheer</h1>
            <asp:Label ID="lblBevoegd" runat="server" Text="Sorry, je bent niet bevoegd om deze pagina te bekijken." Visible="False"></asp:Label>
            <asp:Panel ID="pnlWrapper" runat="server">
            <div id="searchpanel">
            <h2>Boekbeheer</h2>
            <asp:Button ID="btnToevoegen" runat="server" CssClass="lenenKnop" Text="Boek toevoegen" Width="200px" Height="32px" /> &nbsp;
            <asp:Button ID="btnWijzigen" runat="server" CssClass="lenenKnop" Text="Boek wijzigen" Width="200px" Height="32px"/> &nbsp;
            <asp:Button ID="btnVerwijderen" runat="server" CssClass="lenenKnop" Text="Boek verwijderen" Width="200px" Height="32px"/>
            <br /><br />
            <asp:Button ID="btnExemplaren" runat="server" CssClass="lenenKnop" Text="Boekexemplaren beheren" Width="635px" Height="32px"/>
            </div>
            <div id="searchpanel">
            <h2>Uitleningenbeheer</h2>
            <asp:Button ID="btnUitleningenBeheer" runat="server" CssClass="lenenKnop" Text="Uitleningen beheren" Width="635" Height="32px"/>
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
