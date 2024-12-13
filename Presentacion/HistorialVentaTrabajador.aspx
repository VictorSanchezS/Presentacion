<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HistorialVentaTrabajador.aspx.cs" Inherits="Presentacion.HistorialVentaTrabajador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center my-4 text-primary font-weight-bold display-4">
        <i class="fas fa-chart-line"></i>Historial de Ventas por Trabajador
    </h2>

    <asp:GridView
        ID="gvHistorialVentas"
        runat="server"
        AutoGenerateColumns="true"
        AllowPaging="true"
        PageSize="10"
        CssClass="table table-bordered table-striped"
        PagerStyle-CssClass="pagination justify-content-center"
        OnPageIndexChanging="gvHistorialVentas_PageIndexChanging">
    </asp:GridView>

</asp:Content>
