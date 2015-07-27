<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="lening_klaar.aspx.vb" Inherits="Intratheek.lening_klaar" %>

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
		<h1>Veel leesplezier!</h1>
            <div id="searchpanel">
            <p>
                De lening is verwerkt! Je kan hieronder een PDF bestand verkrijgen om af te drukken of je kan je bibliotheekkaart bekijken.<br />
                <br /><center><asp:Button ID="btnBibliotheekkaart" runat="server" CssClass="lenenKnop" Text="Ga naar bibliotheekkaart" Height="32px" />&nbsp;
                <asp:Button ID="btnOpenPDF" runat="server" CssClass="lenenKnop" Text="PDF openen" Height="32px"/></center>
            </p>
            </div>
        </div>
		<div id="footer">
			Intratheek &copy; 2015 Tom Dobbelaere
	    </div>
    </div>
    </form>
</body>
</html>
