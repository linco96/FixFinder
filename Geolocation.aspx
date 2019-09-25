<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Geolocation.aspx.cs" Inherits="FixFinder.Geolocation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
</head>
<body>
    <p id="demo">Click the button to get your position:</p>
    <button onclick="getLocation()">Get your Location</button>
    <div id="mapholder"></div>
    <script type="text/javascript">
        var x = document.getElementById("demo");
        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            }
            else { x.innerHTML = "Geolocation is not supported by this browser."; }
        }

        function showPosition(position) {
            var latlondata = position.coords.latitude + "," + position.coords.longitude;
            var latlon = "Your Latitude Position is:=" + position.coords.latitude + "," + "Your Longitude Position is:=" + position.coords.longitude;
            alert(latlon)

            var img_url = "http://maps.googleapis.com/maps/api/staticmap?center="
                + latlondata + "&zoom=14&size=400x300&sensor=false";
            document.getElementById("mapholder").innerHTML = "<img src='" + img_url + "' />";
        }

        function showError(error) {
            if (error.code == 1) {
                x.innerHTML = "User denied the request for Geolocation."
            }
            else if (err.code == 2) {
                x.innerHTML = "Location information is unavailable."
            }
            else if (err.code == 3) {
                x.innerHTML = "The request to get user location timed out."
            }
            else {
                x.innerHTML = "An unknown error occurred."
            }
        }
    </script>
    <form id="form1" runat="server">
        <div class="container">
            <div class="form-group">
                <asp:TextBox runat="server" ID="txt1" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:TextBox runat="server" ID="txt2" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:TextBox runat="server" ID="txt3" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_SeFude" Text="Vamo lá" OnClick="btn_SeFude_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </form>
</body>
</html>