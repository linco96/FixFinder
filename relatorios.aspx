<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="relatorios.aspx.cs" Inherits="FixFinder.relatorios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://canvasjs.com/assets/script/canvasjs.min.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            var chart = new CanvasJS.Chart("chartContainer", {
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                animationEnabled: true,
                zoomEnabled: true,
                title: {
                    text: "Try Zooming and Panning"
                },
                data: [{
                    type: "line",
                    dataPoints: <%= jsonString %>
                }]
            });

            chart.render();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="chartContainer" style="height: 370px; width: 100%;"></div>
    </form>
</body>
</html>