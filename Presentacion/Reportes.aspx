<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Reportes.aspx.cs" Inherits="Presentacion.Reportes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Filtros y Botones -->
    <div class="flex flex-col items-center my-5">
        <asp:DropDownList ID="ddlYearFilter" runat="server" CssClass="form-select border-pink-400 text-pink-600 shadow-md w-60"></asp:DropDownList>
        <div class="flex mt-3">
            <asp:Button ID="btnLoadReport" runat="server" Text="Cargar Reporte" CssClass="btn btn-primary mx-2 bg-pink-500 border-pink-600 text-white hover:bg-pink-600" OnClick="btnLoadReport_Click" />
            <asp:Button ID="btnExportPdf" runat="server" Text="Exportar a PDF" CssClass="btn btn-danger mx-2 bg-pink-500 border-pink-600 text-white hover:bg-pink-600" OnClick="btnExportPdf_Click" />
            <asp:Button ID="btnExportExcel" runat="server" Text="Exportar a Excel" CssClass="btn btn-success mx-2 bg-pink-500 border-pink-600 text-white hover:bg-pink-600" OnClick="btnExportExcel_Click" />
        </div>
    </div>

    <!-- Tabla de Reporte -->
    <div class="overflow-x-auto">
        <asp:Repeater ID="ReporteRepeater" runat="server">
            <HeaderTemplate>
                <table class="table table-striped shadow-lg border-collapse w-full">
                    <thead class="bg-pink-500 text-white">
                        <tr>
                            <th class="px-4 py-2">Mes</th>
                            <th class="px-4 py-2">Año</th>
                            <th class="px-4 py-2">Producto</th>
                            <th class="px-4 py-2">Cantidad Vendida</th>
                            <th class="px-4 py-2">Total Vendido</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="hover:bg-pink-100">
                    <td class="border px-4 py-2 text-center"><%# Eval("Mes") %></td>
                    <td class="border px-4 py-2 text-center"><%# Eval("Año") %></td>
                    <td class="border px-4 py-2 text-center"><%# Eval("NombreProducto") %></td>
                    <td class="border px-4 py-2 text-center"><%# Eval("CantidadVendida") %></td>
                    <td class="border px-4 py-2 text-center"><%# Eval("TotalVendido", "{0:C2}") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
