<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="veiculo_Lista.aspx.cs" Inherits="FixFinder.Pages.veiculo_Lista" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seus Veículos</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_ListaVeiculo" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Seus Veículos</h1>
            <hr class="border-primary" />
            <div>
                <asp:Button runat="server" ID="btn_CadastrarVeiculo" Text="Cadastrar Veículo" CssClass="btn btn-outline-primary btn-sm mb-2" aria-pressed="true" OnClick="btn_CadastrarVeiculo_Click" />
            </div>
            <div class="table-responsive-xl">
                <asp:Table runat="server" ID="tbl_Veiculos" CssClass="table border rounded-lg">
                    <asp:TableHeaderRow runat="server" ID="tblH_Veiculos" CssClass="thead-dark">
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Marca</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Modelo</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ano</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Placa</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>
                    </asp:TableHeaderRow>

                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="6" CssClass="text-center align-middle font-weight-bold text-primary">Você não tem nenhum veículo cadastrado.</asp:TableCell>
                    </asp:TableRow>

                    <%--<asp:TableRow>
                        <asp:TableCell CssClass="text-center align-middle">Volkswagen</asp:TableCell>
                        <asp:TableCell CssClass="text-center align-middle">Passat</asp:TableCell>
                        <asp:TableCell CssClass="text-center align-middle">2003</asp:TableCell>
                        <asp:TableCell CssClass="text-center align-middle">AAA-9999</asp:TableCell>
                        <asp:TableCell CssClass="text-center align-middle">
                            <button class="btn btn-primary">Editar</button>
                            <button class="btn btn-danger" style="margin-left:5px">Excluir</button>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="text-center align-middle">Chevrolet</asp:TableCell>
                        <asp:TableCell CssClass="text-center align-middle">Onix</asp:TableCell>
                        <asp:TableCell CssClass="text-center align-middle">2013</asp:TableCell>
                        <asp:TableCell CssClass="text-center align-middle">BBB-9999</asp:TableCell>
                        <asp:TableCell CssClass="text-center align-middle">
                            <button class="btn btn-primary">Editar</button>
                            <button class="btn btn-danger" style="margin-left:5px">Excluir</button>
                        </asp:TableCell>
                    </asp:TableRow>--%>
                </asp:Table>
            </div>
        </div>
    </form>
</body>
</html>