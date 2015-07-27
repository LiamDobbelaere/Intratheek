<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="boekdetails.aspx.vb" Inherits="Intratheek.boekdetails" %>

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
		<h1>Boekdetails</h1>
            <asp:FormView ID="fvBoekdetails" runat="server" DataKeyNames="ISBN" DataSourceID="dtsBoekDetails" Font-Names="Verdana" HorizontalAlign="Justify">
                <EditItemTemplate>
                    BoekNummer:
                    <asp:TextBox ID="BoekNummerTextBox" runat="server" Text='<%# Bind("BoekNummer") %>' />
                    <br />
                    ISBN:
                    <asp:Label ID="ISBNLabel1" runat="server" Text='<%# Eval("ISBN") %>' />
                    <br />
                    Titel:
                    <asp:TextBox ID="TitelTextBox" runat="server" Text='<%# Bind("Titel") %>' />
                    <br />
                    Auteurs:
                    <asp:TextBox ID="AuteursTextBox" runat="server" Text='<%# Bind("Auteurs") %>' />
                    <br />
                    Uitgeverij:
                    <asp:TextBox ID="UitgeverijTextBox" runat="server" Text='<%# Bind("Uitgeverij") %>' />
                    <br />
                    Bladzijden:
                    <asp:TextBox ID="BladzijdenTextBox" runat="server" Text='<%# Bind("Bladzijden") %>' />
                    <br />
                    Jaar:
                    <asp:TextBox ID="JaarTextBox" runat="server" Text='<%# Bind("Jaar") %>' />
                    <br />
                    Exemplaren:
                    <asp:TextBox ID="ExemplarenTextBox" runat="server" Text='<%# Bind("Exemplaren") %>' />
                    <br />
                    Flaptekst:
                    <asp:TextBox ID="FlaptekstTextBox" runat="server" Text='<%# Bind("Flaptekst") %>' />
                    <br />
                    Genres:
                    <asp:TextBox ID="GenresTextBox" runat="server" Text='<%# Bind("Genres") %>' />
                    <br />
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                    &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    BoekNummer:
                    <asp:TextBox ID="BoekNummerTextBox" runat="server" Text='<%# Bind("BoekNummer") %>' />
                    <br />
                    ISBN:
                    <asp:TextBox ID="ISBNTextBox" runat="server" Text='<%# Bind("ISBN") %>' />
                    <br />
                    Titel:
                    <asp:TextBox ID="TitelTextBox" runat="server" Text='<%# Bind("Titel") %>' />
                    <br />
                    Auteurs:
                    <asp:TextBox ID="AuteursTextBox" runat="server" Text='<%# Bind("Auteurs") %>' />
                    <br />
                    Uitgeverij:
                    <asp:TextBox ID="UitgeverijTextBox" runat="server" Text='<%# Bind("Uitgeverij") %>' />
                    <br />
                    Bladzijden:
                    <asp:TextBox ID="BladzijdenTextBox" runat="server" Text='<%# Bind("Bladzijden") %>' />
                    <br />
                    Jaar:
                    <asp:TextBox ID="JaarTextBox" runat="server" Text='<%# Bind("Jaar") %>' />
                    <br />
                    Exemplaren:
                    <asp:TextBox ID="ExemplarenTextBox" runat="server" Text='<%# Bind("Exemplaren") %>' />
                    <br />
                    Flaptekst:
                    <asp:TextBox ID="FlaptekstTextBox" runat="server" Text='<%# Bind("Flaptekst") %>' />
                    <br />
                    Genres:
                    <asp:TextBox ID="GenresTextBox" runat="server" Text='<%# Bind("Genres") %>' />
                    <br />
                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                </InsertItemTemplate>
                <ItemTemplate>
                    <div id="detailsA">
                    <asp:Image ID="imgCover" runat="server" CssClass="detailBook" Height="162px" ImageAlign="Left" ImageUrl='<%# "img/boeken/" & Eval("ISBN") & ".jpg"%>' Width="113px" />
                    <asp:Label ID="lblTitel" runat="server" Font-Size="15pt" Text='<%# Eval("Titel") %>'></asp:Label>
                    <br />
                    <asp:Label ID="lblAuteur" runat="server" Font-Size="12pt" ForeColor="#666666" Text='<%# Eval("Auteurs") %>'></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblUitgeverij" runat="server" Font-Names="Verdana" Font-Size="10pt" ForeColor="#666666" Text='<%# "Uitgeverij " & Eval("Uitgeverij") %>'></asp:Label>
                    <br /><br />
                    <asp:Button ID="btnLenen" runat="server" CssClass="lenenKnop" Text="Dit boek lenen" OnClick="btnLenen_Click" OnPreRender="btnLenen_PreRender" />
                    <br />
                    <br />
                     <br />
                    <asp:Label ID="Label3" runat="server" Font-Size="10pt" Text="ISBN:" Width="145px"></asp:Label>
                    <asp:Label ID="lblISBN" runat="server" Font-Size="10pt" Text='<%# Eval("ISBN") %>' Font-Bold="False" />
                    <br />
                    <asp:Label ID="Label2" runat="server" Font-Size="10pt" style="margin-bottom: 0px" Text="Aantal bladzijden:" Width="145px"></asp:Label>
                    <asp:Label ID="lblBladzijden" runat="server" Text='<%# Eval("Bladzijden") %>' Font-Size="10pt" />
                    <br />
                    <asp:Label ID="Label1" runat="server" Font-Size="10pt" Text="Jaar: " Width="145px"></asp:Label>
                    <asp:Label ID="lblJaar" runat="server" Text='<%# Eval("Jaar") %>' Font-Size="10pt" />
                        </div><div id="detailsB">
                    <asp:Label ID="lblFlaptekst" runat="server" Text='<%# Bind("Flaptekst") %>' />
                    <br /><br />
                    <asp:Literal ID="litGenres" runat="server" OnDataBinding="litGenres_DataBinding" Text='<%# Eval("Genres") %>'></asp:Literal></div>

                </ItemTemplate>
            </asp:FormView>
            <asp:SqlDataSource ID="dtsBoekDetails" runat="server" ConnectionString="<%$ ConnectionStrings:IntratheekConnectionString %>" SelectCommand="SELECT tblBoeken.ISBN, tblBoeken.Titel, dbo.GetBookAuthors(tblBoeken.ISBN) AS Auteurs, tblBoeken.Uitgeverij, tblBoeken.Bladzijden, tblBoeken.Jaar, COUNT(tblBibliotheek_1.BoekNummer) AS Exemplaren, tblBoeken.Flaptekst, dbo.GetBookGenres(tblBoeken.ISBN) AS Genres FROM tblBibliotheek AS tblBibliotheek_1 RIGHT OUTER JOIN tblBoeken ON tblBibliotheek_1.ISBN = tblBoeken.ISBN GROUP BY tblBoeken.ISBN, tblBoeken.Titel, tblBoeken.Uitgeverij, tblBoeken.Bladzijden, tblBoeken.Flaptekst, tblBoeken.Jaar ORDER BY tblBoeken.Titel"></asp:SqlDataSource>
		</div>
		<div id="footer">
			Intratheek &copy; 2015 Tom Dobbelaere
	    </div>
    </div>
    </form>
</body>
</html>
