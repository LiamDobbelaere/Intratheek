<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="boek_wijzigen.aspx.vb" Inherits="Intratheek.boek_wijzigen" %>

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
		<h1>Boek wijzigen</h1>
            <asp:Label ID="lblBevoegd" runat="server" Text="Sorry, je bent niet bevoegd om deze pagina te bekijken." Visible="False"></asp:Label>
            <asp:Label ID="lblBoekBestaat" runat="server" Text="Het gevraagde boek met dit ISBN bestaat niet." Visible="False"></asp:Label>
            <asp:Panel ID="pnlWrapper" runat="server">
            <div id="searchpanel">
            <center><asp:Image ID="imgCover" runat="server" BorderColor="#990000" BorderStyle="Solid" BorderWidth="2px" Height="162px" ImageAlign="Top" Width="113px" /></center>
                <br /><br />
            <asp:Label ID="lblCover" runat="server" Text="Afbeelding cover:" Width="200px"></asp:Label>
            <asp:FileUpload ID="fuCover" runat="server" EnableTheming="True" />
            
                <asp:Label ID="lblCoverForm" runat="server" ForeColor="#990000" Text="* Ongeldig bestand geselecteerd (enkel .jpg of .jpeg toegestaan)" Visible="False"></asp:Label>
                <asp:Label ID="lblCoverSize" runat="server" ForeColor="#990000" Text="* Geslecteerd bestand is te groot (max. 1 MB)" Visible="False"></asp:Label>
            
            <br /><br />
            <asp:Label ID="lblISBN" runat="server" Text="ISBN:" Width="200px"></asp:Label>
            <asp:TextBox ID="txtISBN" runat="server" CssClass="search" MaxLength="13"></asp:TextBox>
            
                <asp:Label ID="lblBestaat" runat="server" ForeColor="#990000" Text="* Een boek bestaat al met dit ISBN" Visible="False"></asp:Label>
            
                <asp:RegularExpressionValidator ID="revISBN" runat="server" ControlToValidate="txtISBN" ErrorMessage="* Een ISBN bestaat uit 13 cijfers" ValidationExpression="\d{13}" ForeColor="#990000" Display="Dynamic"></asp:RegularExpressionValidator>
            
                <asp:RequiredFieldValidator ID="rfvISBN" runat="server" ControlToValidate="txtISBN" ErrorMessage="* Vul een ISBN in" ForeColor="#990000" Display="Dynamic"></asp:RequiredFieldValidator>
            
            <br /><br /><br />
            <asp:Label ID="lblTitel" runat="server" Text="Titel:" Width="200px"></asp:Label>
            <asp:TextBox ID="txtTitel" runat="server" CssClass="search"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvTitel" runat="server" ControlToValidate="txtTitel" ErrorMessage="* Vul een titel in" ForeColor="#900000"></asp:RequiredFieldValidator>

            <br /><br />
            <asp:Label ID="lblAuteurs" runat="server" Text="Auteur(s):" Width="200px"></asp:Label>
            <asp:TextBox ID="txtAuteurA" runat="server" CssClass="search"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAuteur" runat="server" ControlToValidate="txtAuteurA" ErrorMessage="* Vul minstens één auteur in" ForeColor="#990000"></asp:RequiredFieldValidator>
                <br /><br />
            <asp:Label ID="lblAuteursFill" runat="server" Text="" Width="200px"></asp:Label>
            <asp:TextBox ID="txtAuteurB" runat="server" CssClass="search"></asp:TextBox>

            <br /><br /><br />
            <asp:Label ID="lblJaar" runat="server" Text="Jaar:" Width="200px"></asp:Label>
            <asp:TextBox ID="txtJaar" runat="server" CssClass="search" Width="42px" MaxLength="4"></asp:TextBox>
                        
                <asp:RegularExpressionValidator ID="revJaar" runat="server" ControlToValidate="txtJaar" ErrorMessage="* Vul een geldig jaartal in" ValidationExpression="\d{4}" ForeColor="#990000"></asp:RegularExpressionValidator>
            
            <br /><br />
            <asp:Label ID="lblUitgeverij" runat="server" Text="Uitgeverij:" Width="200px"></asp:Label>
            <asp:TextBox ID="txtUitgeverij" runat="server" CssClass="search"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvUitgeverij" runat="server" ErrorMessage="* Vul een uitgeverij in" ForeColor="#990000" ControlToValidate="txtUitgeverij"></asp:RequiredFieldValidator>

            <br /><br />
            <asp:Label ID="lblBladzijden" runat="server" Text="Bladzijden:" Width="200px"></asp:Label>
            <asp:TextBox ID="txtBladzijden" runat="server" CssClass="search" Width="42px" MaxLength="4"></asp:TextBox>
                                        
                <asp:RegularExpressionValidator ID="revBladzijden" runat="server" ControlToValidate="txtBladzijden" ErrorMessage="* Vul een geldig aantal bladzijden in" ValidationExpression="\d+" ForeColor="#990000"></asp:RegularExpressionValidator>
            
            <br /><br />
            <asp:Label ID="lblTags" runat="server" Text="Tags, gescheiden met een spatie:" Width="200px"></asp:Label>
            <asp:TextBox ID="txtTags" runat="server" CssClass="search" Width="359px"></asp:TextBox>

            <br /><br />
            <asp:Label ID="lblFlaptekst" runat="server" Text="Flaptekst:" Width="200px"></asp:Label>
            <asp:TextBox ID="txtFlaptekst" runat="server" CssClass="search" Height="100px" TextMode="MultiLine" Width="359px" Font-Names="Verdana"></asp:TextBox>
                <br /><br />
                <center><asp:Button ID="btnBijwerken" runat="server" Text="Boek bijwerken" CssClass="lenenKnop" /></center>
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
