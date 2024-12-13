<%@ Page Title="Registro de Asistencia" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistroAsistencia.aspx.cs" Inherits="Presentacion.RegistroAsistencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row mb-4">
            <div class="col-md-12 text-center">
                <h1 class="display-4">Registro Asistencia de Trabajadores</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="lblFecha" runat="server" Text="Fecha:" CssClass="form-label" />
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblHoraInicio" runat="server" Text="Hora Inicio:" CssClass="form-label" />
                <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control" TextMode="Time" />
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblHoraFin" runat="server" Text="Hora Fin:" CssClass="form-label" />
                <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control" TextMode="Time" />
            </div>



        </div>
        <div class="row mt-3">
            <div class="col-md-12">
                <asp:Label ID="lblErrorHora" runat="server" Text="La hora de fin debe ser mayor que la de inicio." CssClass="alert alert-danger" Visible="False" />
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-md-12">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100" OnClick="btnBuscar_Click" />
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-md-12">
                <asp:GridView ID="gvAsistencias" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvAsistencias_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="NombreTrabajador" HeaderText="Nombre del Trabajador" />
                        <asp:BoundField DataField="FechaAsistencia" HeaderText="Fecha Asistencia" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="HoraEntrada" HeaderText="Hora Entrada" />
                        <asp:BoundField DataField="HoraSalida" HeaderText="Hora Salida" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <asp:Label ID="lblNoRegistros" runat="server" Text="No existe registro entre los parámetros" CssClass="alert alert-warning" Visible="False" />
    </div>


</asp:Content>
