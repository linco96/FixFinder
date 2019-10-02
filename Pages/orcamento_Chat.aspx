<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orcamento_Chat.aspx.cs" Inherits="FixFinder.Pages.orcamento_Chat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chat</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="form-group">
                <label class="h4">Dados do seu orçamento</label>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="txt_Oficina" style="display: block">Oficina</label>
                    <asp:TextBox runat="server" ID="txt_Oficina" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-25 text-left pr-3 mb-1">
                    <label style="display: block">Status</label>
                    <asp:TextBox runat="server" ID="txt_Status" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-25 text-left mb-1">
                    <label style="display: block">Veículo</label>
                    <asp:TextBox runat="server" ID="txt_Veiculo" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
            </div>
            <div class="form-inline">
                <span class="w-25 text-left mb-1">
                    <label style="display: block">Mecânico Responsável</label>
                    <asp:TextBox runat="server" ID="txt_MecanicoResponsavel" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
            </div>

            <div class="container mt-3" align="center">
                <div class="overflow-auto bg-light w-75 rounded border" style="height: 350px">
                    <div class="bg-secondary ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-left text-light text-break">
                        <div class="font-italic">Enviado 08/03/2019 às 08:32 - Mecânico</div>
                        <br />
                        TESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEE
                    </div>
                    <div class="bg-info ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-right text-light text-break">
                        <div class="font-italic">Enviado 08/03/2019 às 08:32 - Eu</div>
                        <br />
                        TESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEE
                    </div>
                    
                </div>
                <div>
                    <asp:TextBox runat="server" CssClass="text-left w-75 bg-light border rounded mt-1 p-1" ID="btn_Mensagem" MaxLength="500" placeholder="Tecle enter para enviar sua mensagem..." required></asp:TextBox>
                    <asp:Button runat="server" ID="btn_EnviarMSG" Style="display: none" required />
                </div>
            </div>
        </div>
    </form>
</body>
</html>