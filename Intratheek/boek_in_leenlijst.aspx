<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="boek_in_leenlijst.aspx.vb" Inherits="Intratheek.boek_in_leenlijst" %>

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
		<h1>Boek toegevoegd aan leenlijst</h1>
            <div id="searchpanel">
            <p>
                <asp:Image ID="Image1" runat="server" ImageAlign="Right" ImageUrl="~/img/book_added.png" />
                Het boek is toegevoegd aan de leenlijst, de uitlening moet nog bevestigd worden. Je kan verder gaan naar de leenlijst om je uitlening te bevestigen of nog boeken toevoegen aan de uitlening.<br />
                <br /><center><asp:Button ID="btnBoekenToevoegen" runat="server" CssClass="lenenKnop" Text="Nog boeken toevoegen" Height="32px" />&nbsp;
                <asp:Button ID="btnDoorgaanLeenlijst" runat="server" CssClass="lenenKnop" Text="Doorgaan naar leenlijst" Height="32px"/></center>
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
