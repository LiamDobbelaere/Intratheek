<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="default.aspx.vb" Inherits="Intratheek._default" %>

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
		<h1>Intratheek - BuSO</h1>
		<p>Welkom op de Intratheek van Dominiek Savio BuSO!<br /><br /> Om van start te gaan, klik op de knop Boeken bovenaan de pagina.</p>
		</div>
		<div id="footer">
			Intratheek &copy; 2015 Tom Dobbelaere
		</div>
	</div>
    </form>
</body>
</html>
