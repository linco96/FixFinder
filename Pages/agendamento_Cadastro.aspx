<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="agendamento_Cadastro.aspx.cs" Inherits="FixFinder.Pages.agendamento_Cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Novo agendamento</title>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txt_Data").change(function () {
                $("#btn_CarregarHorario").click();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="form-group">
                <h2 style="text-align: center"><span runat="server" id="lbl_Oficina" class="align-middle mr-2">Paje Motors</span>
                    <img class="align-middle ml-2" src="../Content/star_32.png" />
                    <asp:Label runat="server" ID="lbl_Reputacao" CssClass="align-middle">7/10</asp:Label></h2>
                <h5 runat="server" id="lbl_Endereco" class="text-muted" style="text-align: center">Rua Inferno na Torre, 2062<br />
                    Inferno, Curitiba</h5>
            </div>
            <hr class="border-primary" />
            <div class="form-group">
                <label for="txt_Data">Data</label>
                <asp:TextBox runat="server" ID="txt_Data" CssClass="form-control" type="date" required="required" ClientIDMode="Static"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txt_Hora">Hora</label>
                <asp:DropDownList runat="server" ID="txt_Horario" CssClass="custom-select form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="txt_Veiculo">Veículo</label>
                <asp:DropDownList runat="server" ID="txt_Veiculo" CssClass="custom-select form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Cadastrar" CssClass="btn btn-success" Text="Agendar" OnClick="btn_Cadastrar_Click" />
            </div>
            <div class="form-group">
                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-light" role="alert">
                    <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                </asp:Panel>
            </div>
        </div>
        <asp:Button runat="server" ID="btn_CarregarHorario" ClientIDMode="Static" OnClick="btn_CarregarHorario_Click" Style="display: none;" />
    </form>
</body>
</html>