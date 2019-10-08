﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orcamento_Chat.aspx.cs" Inherits="FixFinder.Pages.orcamento_Chat" %>

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

    <script src="../Scripts/jquery.signalR-2.4.1.min.js"></script>
    <script src="signalr/hubs"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="form-group" align="center">
                <h2 style="text-align: center">
                    <span runat="server" id="lbl_Oficina" class="align-middle text-info">Paje Motors</span>
                </h2>

                <asp:Table runat="server" ID="tbl_Dados">
                    <asp:TableRow CssClass="border-bottom border-info">
                        <asp:TableCell CssClass="h5 text-center w-50 ">
                            <asp:Label runat="server" CssClass="align-middle text-right">Status Orçamento</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell CssClass="h5 text-center w-50 ">
                            <asp:Label runat="server" ID="lbl_Status" CssClass="align-middle text-secondary">Aprovado</asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="border-bottom border-info">
                        <asp:TableCell CssClass="h5 text-center w-50">
                            <asp:Label runat="server" CssClass="align-middle">Veículo</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell CssClass="h5 text-center w-50">
                            <asp:Label runat="server" ID="lbl_Veiculo" CssClass="align-middle text-center text-secondary">Corolla - AYK-3929</asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="border-bottom border-info">
                        <asp:TableCell CssClass="h5 text-center w-50">
                            <asp:Label runat="server" CssClass="align-middle">Mecânico Responsável</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell CssClass="h5 text-center w-50">
                            <asp:Label runat="server" ID="lbl_MecanicoResponsavel" CssClass="align-middle text-center text-secondary">Um Fera ae</asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <hr class="border border-dark border-bottom-0 mt-1 mb-1 w-75" />

            <div class="container mt-3" align="center">
                <%--<asp:Panel runat="server" ID="pnl_Mensagens" CssClass="overflow-auto bg-light w-75 rounded border" Style="height: 350px">--%>
                <div runat="server" id="pnl_Mensagens" class="overflow-auto bg-light w-75 rounded border" style="height: 350px">

                    <%--Mecanico--%>
                    <%--<div class="bg-secondary ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-left text-light text-break">
                        <div class="font-italic">Enviado 08/03/2019 às 08:32 - Mecânico</div>
                        <br />
                        TESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEE
                    </div>--%>
                    <%--Eu--%>
                    <%--<div class="bg-info ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-right text-light text-break">
                        <div class="font-italic">Enviado 08/03/2019 às 08:32 - Eu</div>
                        <br />
                        TESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEE
                    </div>--%>
                </div>
                <%--</asp:Panel>--%>
                <div>
                    <asp:TextBox runat="server" CssClass="text-left w-75 bg-light border rounded mt-1 p-1" ID="txt_Mensagem" MaxLength="500" placeholder="Tecle enter para enviar sua mensagem..." required autocomplete="off"></asp:TextBox>
                    <asp:Button runat="server" ID="btn_EnviarMSG" OnClick="btn_EnviarMSG_Click" Style="display: none" required />
                </div>
                <div>
                    <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger mt-2 w-75 text-left" role="alert">
                        <asp:Label ID="lbl_Alert" runat="server">Porra jovem</asp:Label>
                    </asp:Panel>
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btn_Voltar" CssClass="btn btn-danger" Text="Voltar" OnClick="btn_Voltar_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>