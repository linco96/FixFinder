<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="teste_popup.aspx.cs" Inherits="FixFinder.teste_popup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <p>
            Press button to delete product
            <asp:Literal ID="LiteralID" runat="server" />.
        </p>
        <asp:Button ID="ButtonDelete" Text="Delete" OnClick="ButtonDelete_Click" runat="server" />

        <asp:PlaceHolder ID="PlaceWarning" Visible="false" EnableViewState="false" runat="server">
            <div id="warning">
                <p>
                    Are you sure?
                </p>
                <p>
                    <asp:Button ID="ButtonYes" Text="Continue" OnClick="ButtonYes_Click" runat="server" />
                    <asp:Button ID="ButtonNo" Text="Cancel" OnClick="ButtonNo_Click" runat="server" />
                </p>
            </div>

            <script type="text/javascript">
                // If you don't want any kind of javascript conformation, just the html confirmation above then you
                // can leave out this entire script block.

                // We want the form above to be there still because it houses our buttons, but it also means our
                // code still works without js.  With js disabled the "hide" never runs so the form stays visible
                // and functioning

                $("#warning").hide();

                if (confirm("Are you sure?")) {
                    $("#<%=ButtonYes.ClientID %>").click();
                }
                else {
                    // If your server-side code isn't doing anything on ButtonNo then you can leave this "else" out
                    $("#<%=ButtonNo.ClientID %>").click();
                }
            </script>
        </asp:PlaceHolder>
    </form>
</body>
</html>