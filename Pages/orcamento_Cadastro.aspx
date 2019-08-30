<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orcamento_Cadastro.aspx.cs" Inherits="FixFinder.Pages.orcamento_Cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Criar orçamento</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">

            <div class="form-group">
                <h4>Cliente</h4>
            </div>
            <div class="form-group">
                <label for="txt_CPF">CPF</label>
                <asp:TextBox runat="server" ID="txt_CPF" CssClass="form-control" minlength="14" onkeypress="$(this).mask('000.000.000-00');" ClientIDMode="Static"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Nome">Nome completo</label>
                <asp:TextBox runat="server" ID="txt_Nome" CssClass="form-control" minlength="4" required="required" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Veiculo">Veículo</label>
                <asp:DropDownList runat="server" ID="txt_Veiculo" disabled="disabled" CssClass="form-control custom-select">
                </asp:DropDownList>
            </div>

            <div class="form-group align-content-center mt-5">
                <h4>Serviços</h4>
                <div>
                    <asp:Button runat="server" ID="btn_NovoServico" Text="Novo serviço" CssClass="btn btn-outline-primary btn-sm mt-1" aria-pressed="true" OnClick="btn_NovoServico_Click" formnovalidate="true" />
                </div>
            </div>
            <div runat="server" id="form_CadastroServico" visible="false">
                <div class="form-group">
                    <label for="txt_Descricao">Descrição</label>
                    <asp:TextBox runat="server" ID="txt_Descricao" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txt_Valor">Valor</label>
                    <asp:TextBox runat="server" ID="txt_Valor" CssClass="form-control" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btn_CadastrarServico" CssClass="btn btn-primary" OnClick="btn_CadastrarServico_Click" Text="Cadastrar" />
                    <asp:Button runat="server" ID="btn_CancelarCadastroServico" Text="Cancelar" CssClass="btn btn-danger" OnClick="btn_CancelarCadastroServico_Click" formnovalidate="true" />
                </div>
                <hr class="border-primary" />
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList runat="server" ID="txt_ServicoSelecionado" CssClass="form-control custom-select width100">
                    </asp:DropDownList>
                    <span class="input-group-append">
                        <asp:Button runat="server" ID="btn_AdicionarServico" Text="Adicionar" CssClass="btn btn-primary" OnClick="btn_AdicionarServico_Click" />
                    </span>
                </div>
            </div>
            <div class="form-group">
                <div class="table-responsive-xl">
                    <asp:Table runat="server" ID="tbl_Servicos" Visible="false" CssClass="table border rounded-lg">
                        <asp:TableHeaderRow runat="server" ID="tblH_Servicos" CssClass="thead-light">
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Descrição</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Valor</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>

            <div class="form-group align-content-center mt-5">
                <h4>Produtos</h4>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList runat="server" ID="txt_ProdutoSelecionado" CssClass="form-control custom-select width100">
                    </asp:DropDownList>
                    <span class="input-group-append">
                        <asp:Button runat="server" ID="btn_AdicionarProduto" Text="Adicionar" CssClass="btn btn-primary" OnClick="btn_AdicionarProduto_Click" />
                    </span>
                </div>
            </div>
            <div class="form-group">
                <div class="table-responsive-xl">
                    <asp:Table runat="server" ID="tbl_Produtos" Visible="false" CssClass="table border rounded-lg">
                        <asp:TableHeaderRow runat="server" ID="tblH_Produtos" CssClass="thead-light">
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Descrição</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Marca</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Preço</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Validade</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Quantidade em<br />estoque</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>

            <div class="form-group mt-5">
                <label for="txtDesconto">Desconto/Acréscimo</label>
                <div class="input-group">
                    <span class="input-group-prepend">
                        <asp:DropDownList runat="server" ID="txt_DescontoAcrescimo" CssClass="form-control custom-select bg-light" Style="border-top-right-radius: 0; border-bottom-right-radius: 0;">
                            <asp:ListItem Value="d">Desconto</asp:ListItem>
                            <asp:ListItem Value="a">Acréscimo</asp:ListItem>
                        </asp:DropDownList>
                    </span>
                    <asp:TextBox runat="server" ID="txt_Desconto" CssClass="form-control" ClientIDMode="Static" onkeypress="$(this).mask('#.##0,00', { reverse: true });"></asp:TextBox>
                </div>
                <h3 runat="server" id="lbl_ValorTotal" class="mt-2 text-center text-primary">Valor total: R$ 150,00</h3>
            </div>

            <div class="form-group mt-5 text-center">

                <asp:Button runat="server" ID="btn_Cadastro" CssClass="btn btn-primary btn-lg" OnClick="btn_Cadastro_Click" Text="Criar orçamento" />
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