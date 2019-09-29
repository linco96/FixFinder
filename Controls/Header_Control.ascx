<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header_Control.ascx.cs" Inherits="FixFinder.Controls.Header_Control" %>
<nav class="navbar navbar-expand-lg navbar-light bg-dark">
    <%--<img id="img_logo" src="../Content/FixFinderIcone.png" width="30" height="30" class="d-inline-block bg-white rounded-circle align-top" />--%>

    <div class="collapse navbar-collapse" id="navbarSupportedContent">

        <ul class="navbar-nav mr-auto">
            <li class="nav-item ml-2 mt-2">
                <h5 class="text-white font-weight-normal">FixFinder</h5>
            </li>
        </ul>
        <ul class="nav navbar-nav navbar-right">
            <li class="nav-item mr-2">
                <a runat="server" id="btn_Pesquisar" href="../Pages/oficina_Pesquisar.aspx" class="text-white nav-link">Pesquisar Oficina</a>
                <%--<asp:Button runat="server" ID="btn_Pesquisar" CssClass="btn btn-dark text-white" Text="Pesquisar Oficina" OnClick="btn_Pesquisar_Click" formnovalidate />--%>
            </li>
            <li class="nav-item mr-2">
                <a runat="server" id="btn_Dashboard" href="~/Pages/home.aspx" class="text-white nav-link">Dashboard</a>
                <%--<asp:Button runat="server" ID="btn_Dashboard" CssClass="btn btn-dark text-white" Text="Dashboard" OnClick="btn_Dashboard_Click" formnovalidate />--%>
            </li>
            <li class="nav-item">
                <a runat="server" id="btn_Entrar" href="~/Pages/home.aspx" class="text-white nav-link">Entrar</a>
                <%--<asp:Button runat="server" ID="btn_Entrar" CssClass="btn btn-dark text-white" Text="Entrar" OnClick="btn_Entrar_Click" formnovalidate />--%>
            </li>
            <li class="nav-item">
                <a runat="server" onclick="" id="btn_Sair" href="~/Pages/home.aspx" class="text-white nav-link">Entrar</a>
                <%--<asp:Button runat="server" ID="btn_Sair" CssClass="btn btn-dark text-white" Style="display: none" Text="Sair" OnClick="btn_Sair_Click" formnovalidate />--%>
            </li>
        </ul>
    </div>
</nav>