<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oficina_Cadastro.aspx.cs" Inherits="FixFinder.Pages.oficina_Cadastro" %>

<%@ Register TagPrefix="uc" TagName="Header_Control" Src="~/Controls/Header_Control.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de oficina</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc:Header_Control runat="server" ID="Header_Control"></uc:Header_Control>
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Cadastro de Oficina</h1>
            <div class="form-group">
                <label for="txt_cnpj">CNPJ</label>
                <asp:TextBox runat="server" ID="txt_CNPJ" CssClass="form-control" minlength="18" onkeypress="$(this).mask('00.000.000/0000-00');" required="required"></asp:TextBox>
                <%--    32.154.993/0001-42      --%>
            </div>
            <div class="form-group">
                <label for="txt_nome">Nome</label>
                <asp:TextBox runat="server" ID="txt_Nome" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Telefone">Telefone</label>
                <asp:TextBox runat="server" ID="txt_Telefone" CssClass="form-control" onkeypress="$(this).mask('(00) 0 0000-0000');" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Email">E-mail</label>
                <asp:TextBox runat="server" ID="txt_Email" CssClass="form-control" type="email" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <h4>Segunda à Sexta</h4>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="txt_HorarioAberturaUtil" style="display: block">Horário de abertura</label>
                    <asp:TextBox runat="server" ID="txt_HorarioAberturaUtil" CssClass="form-control w-100" type="time"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3 mb-1">
                    <label for="txt_HorarioFechamentoUtil" style="display: block">Horário de fechamento</label>
                    <asp:TextBox runat="server" ID="txt_HorarioFechamentoUtil" CssClass="form-control w-100" type="time"></asp:TextBox>
                </span>
            </div>
            <div class="form-group mt-3">
                <h4>Sábado</h4>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="txt_HorarioAberturaSabado" style="display: block">Horário de abertura</label>
                    <asp:TextBox runat="server" ID="txt_HorarioAberturaSabado" CssClass="form-control w-100" type="time"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3 mb-1">
                    <label for="txt_HorarioFechamentoSabado" style="display: block">Horário de fechamento</label>
                    <asp:TextBox runat="server" ID="txt_HorarioFechamentoSabado" CssClass="form-control w-100" type="time"></asp:TextBox>
                </span>
            </div>
            <div class="form-group mt-3">
                <h4>Domingo</h4>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="txt_HorarioAberturaDomingo" style="display: block">Horário de abertura</label>
                    <asp:TextBox runat="server" ID="txt_HorarioAberturaDomingo" CssClass="form-control w-100" type="time"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3 mb-1">
                    <label for="txt_HorarioFechamentoDomingo" style="display: block">Horário de fechamento</label>
                    <asp:TextBox runat="server" ID="txt_HorarioFechamentoDomingo" CssClass="form-control w-100" type="time"></asp:TextBox>
                </span>
            </div>
            <div class="form-group">
                <small id="horario_Help" class="form-text text-muted">Caso a oficina não abra no dia especificado, deixe os campos vazios</small>
            </div>
            <div class="form-group mt-3">
                <h4>Agendamentos</h4>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="num_Agendamentos" style="display: block">Capacidade de agendamentos</label>
                    <asp:TextBox runat="server" ID="num_Agendamentos" CssClass="form-control w-100" type="number" required="required"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3 mb-1">
                    <label for="txt_TempoAtendimento" style="display: block">Tempo por atendimento</label>
                    <asp:TextBox runat="server" ID="txt_TempoAtendimento" CssClass="form-control w-100" type="time" required="required"></asp:TextBox>
                </span>
            </div>
            <div class="form-group">
                <small id="agendamento_Help" class="form-text text-muted">Informe quantos clientes a sua oficina pode atender simultâneamente, e qual é o intervalo de tempo para cada atendimento</small>
            </div>

            <%--ENDERECO--%>
            <div class="form-group">
                <div class="form-inline">
                    <label class="h4">Endereço</label>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-75 text-left pr-3">
                        <label for="txt_Rua" style="display: block">Rua</label>
                        <asp:TextBox runat="server" ID="txt_Rua" CssClass="form-control w-100" MaxLength="200" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left">
                        <label for="txt_Numero" style="display: block">Numero</label>
                        <asp:TextBox runat="server" ID="txt_Numero" CssClass="form-control w-100" required></asp:TextBox>
                    </span>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-25 text-left pr-3">
                        <label for="txt_Complemento" style="display: block">Complemento</label>
                        <asp:TextBox runat="server" ID="txt_Complemento" CssClass="form-control w-100" MaxLength="50"></asp:TextBox>
                    </span>
                    <span class="w-25 text-left pr-3">
                        <label for="txt_CEP" style="display: block">CEP</label>
                        <asp:TextBox runat="server" ID="txt_CEP" CssClass="form-control w-100" onkeypress="$(this).mask('00000-000', {reverse: true});" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left pr-3">
                        <label for="txt_Cidade" style="display: block">Cidade</label>
                        <asp:TextBox runat="server" ID="txt_Cidade" CssClass="form-control w-100" MaxLength="50" required></asp:TextBox>
                    </span>
                    <span class="w-25 text-left">
                        <label for="txt_UF" style="display: block">UF</label>
                        <asp:TextBox runat="server" ID="txt_UF" CssClass="form-control w-100" MaxLength="2" required></asp:TextBox>
                    </span>
                </div>
            </div>

            <%--BUTAO--%>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Cadastrar" CssClass="btn btn-primary" OnClick="btn_Cadastrar_Click" Text="Cadastrar" />
            </div>
            <div class="form-group">
                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                    <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>