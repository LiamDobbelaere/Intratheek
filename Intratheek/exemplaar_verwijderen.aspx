<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="exemplaar_verwijderen.aspx.vb" Inherits="Intratheek.exemplaar_verwijderen" %>

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
		<h1>Exemplaar verwijderen</h1>
            <asp:Label ID="lblBevoegd" runat="server" Text="Sorry, je bent niet bevoegd om deze pagina te bekijken." Visible="False"></asp:Label>
            <asp:Panel ID="pnlWrapper" runat="server">
            <div id="searchpanel">
            <center><asp:Image ID="imgCover" runat="server" BorderColor="#990000" BorderStyle="Solid" BorderWidth="2px" Height="162px" ImageAlign="Top" Width="113px" /></center>
            <br /><br />
            <b>Let op!</b> U staat op het punt <asp:Literal ID="litBoekNaam" runat="server"></asp:Literal> te verwijderen. Dit wil zeggen dat alle uitleningen van dit exemplaar ook teniet worden gedaan. Dit kan niet ongedaan worden gemaakt.<br /> Wilt u doorgaan?
            <br /><br />    
            <asp:Button ID="btnNeen" runat="server" CssClass="lenenKnop" Text="Neen" Width="166px" Height="36px" />
                &nbsp;<asp:Button ID="btnJa" runat="server" CssClass="lenenKnop" Height="36px" Text="Ja" Width="41px" />
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
