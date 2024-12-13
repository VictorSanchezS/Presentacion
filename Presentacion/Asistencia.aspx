<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Asistencia.aspx.cs" Inherits="Presentacion.Asistencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">

        <div class="row mb-4">
            <div class="col-md-12 text-center">
                <h1 class="display-4">Asistencia de Trabajadores</h1>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-md-10">
                <asp:Label ID="lblBuscarTrabajador" runat="server" Text="Buscar Trabajador:" CssClass="form-label" />
                <asp:TextBox ID="txtBuscarTrabajador" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-2 d-flex align-items-end">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btn btn-primary w-100" />
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-md-6">
                <asp:Label ID="lblSeleccionTrabajador" runat="server" Text="Seleccionar Trabajador:" CssClass="form-label" />
                <asp:DropDownList ID="ddlTrabajadores" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <asp:Button ID="btnRegistrarEntrada" runat="server" Text="Registrar Entrada" OnClick="btnRegistrarEntrada_Click" CssClass="btn btn-success w-100" />
            </div>
            <div class="col-md-6">
                <asp:Button ID="btnRegistrarSalida" runat="server" Text="Registrar Salida" OnClick="btnRegistrarSalida_Click" CssClass="btn btn-danger w-100" />
            </div>
        </div>
    </div>
</asp:Content>
