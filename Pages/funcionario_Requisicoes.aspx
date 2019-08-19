<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="funcionario_Requisicoes.aspx.cs" Inherits="FixFinder.Pages.funcionario_Requisicoes" %>

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
        <div class="container mt-5 row">
            <div class="card text-center w-25">
                <div class="card-header">
                    <h4 class="card-title pt-2">Paje Motors</h4>
                </div>
                <div class="card-body">
                    <h5 class="card-title">Cargo: Mecânico</h5>
                    <h5 class="card-title">Salário: R$ 10.000,00</h5>
                    <p class="card-text">
                        Dados bancários:<br />
                        Banco: 01<br />
                        Agência: 80019<br />
                        Conta: 458712
                    </p>
                    <a href="#" class="btn btn-primary">Aceitar</a>
                    <a href="#" class="btn btn-danger">Rejeitar</a>
                </div>
            </div>
            <div class="card text-center">
                <div class="card-header">
                    <h4 class="card-title pt-2">Penelope Motors</h4>
                </div>
                <div class="card-body">
                    <h5 class="card-title">Cargo: Mecânico</h5>
                    <h5 class="card-title">Salário: R$ 10.000,00</h5>
                    <p class="card-text">
                        Dados bancários:<br />
                        Banco: 01<br />
                        Agência: 80019<br />
                        Conta: 458712
                    </p>
                    <a href="#" class="btn btn-primary">Aceitar</a>
                    <a href="#" class="btn btn-danger">Rejeitar</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>