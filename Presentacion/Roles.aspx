<%@ Page Title="Roles y Permisos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="Presentacion.Roles" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container my-5">
                <div class="form-container bg-pink-100 p-8 rounded-lg shadow-lg" style="max-width: 600px; margin: 0 auto;">
                    <h2 class="text-center text-pink-600 mb-4">Registrar Rol</h2>
                    <div class="mb-4">
                        <label for="txtNombreRol" class="form-label text-pink-600">Nombre del Rol:</label>
                        <asp:TextBox ID="txtNombreRol" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                    </div>

                    <div class="mb-4">
                        <label for="chkPermisos" class="form-label text-pink-600">Seleccionar Permisos:</label>
                        <asp:CheckBoxList ID="chkPermisos" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                    </div>

                    <div class="text-center">
                        <asp:Button ID="btnGuardarRol" runat="server" Text="Guardar Rol" OnClick="btnGuardarRol_Click" CssClass="btn bg-pink-500 text-white rounded-md py-2 px-6 hover:bg-pink-600 transition-all duration-300" />
                        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="False" CssClass="mt-3 text-pink-500"></asp:Label>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardarRol" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- Listado de Roles -->
    <div class="container my-5">
        <h2 class="text-center text-pink-600 mb-4">Lista de Roles</h2>
        <div class="table-responsive" style="max-width: 800px; margin: 0 auto;">
            <asp:Repeater ID="RoleRepeater" runat="server">
                <HeaderTemplate>
                    <table class="table table-striped">
                        <thead class="bg-pink-600 text-white">
                            <tr>
                                <th>Nombre del Rol</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("NombreRol") %></td>
                        <td class="text-end">
                            <asp:Button ID="btnEditarRol" runat="server" Text="Editar" CommandArgument='<%# Eval("RolID") %>' CssClass="btn bg-pink-500 text-white rounded-md hover:bg-pink-600 transition-all duration-300" OnClick="btnEditarRol_Click" />
                            <asp:Button ID="btnEliminarRol" runat="server" Text="Eliminar" CommandArgument='<%# Eval("RolID") %>' CssClass="btn bg-pink-500 text-white rounded-md hover:bg-pink-600 transition-all duration-300" OnClick="btnEliminarRol_Click" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
