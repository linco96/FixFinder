<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oficina_Configuracoes.aspx.cs" Inherits="FixFinder.Pages.oficina_Configuracoes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Configurações Oficina</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_Configuracoes" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Configurações Oficina</h1>
            <div class="form-inline">
                <label class="h4">Oficina</label>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3">
                    <label for="txt_OficinaCNPJ" style="display: block">CNPJ</label>
                    <asp:TextBox runat="server" ID="txt_OficinaCNPJ" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3">
                    <label for="txt_OficinaNome" style="display: block">Nome</label>
                    <asp:TextBox runat="server" ID="txt_OficinaNome" CssClass="form-control w-100" autocomplete="off" required></asp:TextBox>
                </span>
            </div>

            <hr class="border border-primary border-bottom-0 mt-2 mb-1" />

            <%--<div class="form-inline">
                <label class="h3">Configurações</label>
            </div>--%>

            <%--ENDERECO--%>
            <div class="form-group">
                <div class="form-inline">
                    <label class="h4">Endereço</label>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-75 text-left pr-3">
                        <label for="txt_Rua" style="display: block">Rua</label>
                        <asp:TextBox runat="server" ID="txt_Rua" CssClass="form-control w-100" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left">
                        <label for="txt_Numero" style="display: block">Numero</label>
                        <asp:TextBox runat="server" ID="txt_Numero" CssClass="form-control w-100" required></asp:TextBox>
                    </span>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-25 text-left pr-3">
                        <label for="txt_Complemento" style="display: block">Complemento</label>
                        <asp:TextBox runat="server" ID="txt_Complemento" CssClass="form-control w-100"></asp:TextBox>
                    </span>
                    <span class="w-25 text-left pr-3">
                        <label for="txt_CEP" style="display: block">CEP</label>
                        <asp:TextBox runat="server" ID="txt_CEP" CssClass="form-control w-100" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left pr-3">
                        <label for="txt_Cidade" style="display: block">Cidade</label>
                        <asp:TextBox runat="server" ID="txt_Cidade" CssClass="form-control w-100" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left">
                        <label for="txt_UF" style="display: block">UF</label>
                        <asp:TextBox runat="server" ID="txt_UF" CssClass="form-control w-100" required></asp:TextBox>
                    </span>
                </div>
            </div>

            <hr class="border border-primary border-bottom-0 mt-1 mb-1" />

            <%--DADOS OFICINA--%>
            <div class="form-group">
                <div class="form-inline">
                    <label class="h4">Dados Oficina</label>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-25 text-left pr-3">
                        <label for="txt_Telefone" style="display: block">Telefone</label>
                        <asp:TextBox runat="server" ID="txt_Telefone" onkeypress="$(this).mask('(00) 0 0000-0000');" CssClass="form-control w-100" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left pr-3">
                        <label for="txt_Email" style="display: block">E-Mail</label>
                        <asp:TextBox runat="server" ID="txt_Email" type="email" CssClass="form-control w-100" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left pr-3">
                        <label for="txt_Abertura" style="display: block">Hora Abertura</label>
                        <asp:TextBox runat="server" ID="txt_Abertura" type="time" CssClass="form-control w-100" autocomplete="off" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left">
                        <label for="txt_Fechamento" style="display: block">Hora Fechamento</label>
                        <asp:TextBox runat="server" ID="txt_Fechamento" type="time" CssClass="form-control w-100" autocomplete="off" required></asp:TextBox>
                    </span>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-100 text-left">
                        <label for="txt_Fechamento" style="display: block">Descrição</label>
                        <asp:TextBox runat="server" ID="txt_Descrição" CssClass="form-control w-100" TextMode="MultiLine" autocomplete="off" Columns="100" Rows="3" MaxLength="500"></asp:TextBox>
                    </span>
                </div>
            </div>

            <hr class="border border-primary border-bottom-0 mt-1 mb-1" />

            <%--AGENDAMENTOS--%>
            <div class="form-group">
                <div class="form-inline">
                    <label class="h4">Agendamentos</label>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-50 text-left pr-3">
                        <label for="txt_DuracaoAtendimento" style="display: block">Duração do Atendimento</label>
                        <asp:TextBox runat="server" ID="txt_DuracaoAtendimento" CssClass="form-control w-100" type="time" autocomplete="off" required></asp:TextBox>
                    </span>
                    <span class="w-50 text-left pl-3">
                        <label for="txt_CapacidadeAtendimento" style="display: block">Capacidade de Atendimentos</label>
                        <asp:TextBox runat="server" ID="txt_CapacidadeAtendimento" CssClass="form-control w-100" onkeypress="$(this).mask('#.##0', {reverse: true});" autocomplete="off" required></asp:TextBox>
                    </span>
                </div>
            </div>

            <hr class="border border-primary border-bottom-0 mt-1 mb-1" />

            <div class="form-group">
                <div class="form-inline">
                    <label class="h4">Foto</label>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-25 pr-3">
                        <asp:Image runat="server" CssClass="Responsive image rounded border border-info" ID="img_Oficina" />
                    </span>
                    <span class="w-50 text-left">
                        <asp:Button runat="server" ID="btn_CarregarImagem" CssClass="btn btn-info btn-sm" formnovalidate Text="Alterar Foto" />
                    </span>
                </div>
            </div>
        </div>
    </form>
</body>
</html>