<%@ Page Title="Clientes" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Clientes.aspx.cs" Inherits="Presentacion.Clientes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Botón para abrir el modal -->
            <div class="text-center">
                <asp:Button ID="btnAbrirModal" runat="server" Text="Registrar Cliente" 
                    OnClick="btnAbrirModal_Click" CssClass="btn bg-pink-500 text-white rounded-md py-2 px-6 hover:bg-pink-600 transition-all duration-300" />
            </div>

            <!-- Modal de Registro de Cliente -->
            <div class="modal fade" id="modalRegistrarCliente" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">Registrar Cliente</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="container">
                                <div class="form-container bg-pink-100 p-8 rounded-lg shadow-lg">
                                    <div class="mb-4">
                                        <label for="lblIDCliente" class="form-label text-pink-600">ID Cliente:</label>
                                        <asp:Label ID="lblIDCliente" runat="server" CssClass="form-control readonly bg-gray-200 text-gray-600" Text="Generado automáticamente" />
                                    </div>
                                    <div class="mb-4">
                                        <label for="txtNombreCliente" class="form-label text-pink-600">Nombre del Cliente:</label>
                                        <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                    </div>
                                    <div class="mb-4">
                                        <label for="txtEmailCliente" class="form-label text-pink-600">Email:</label>
                                        <asp:TextBox ID="txtEmailCliente" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                    </div>
                                    <div class="mb-4">
                                        <label for="txtTelefonoCliente" class="form-label text-pink-600">Teléfono:</label>
                                        <asp:TextBox ID="txtTelefonoCliente" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                    </div>
                                    <div class="text-center">
                                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn bg-pink-500 text-white rounded-md py-2 px-6 hover:bg-pink-600 transition-all duration-300" />
                                        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="False" CssClass="mt-3 text-pink-500"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardar" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- Listado de Clientes -->
    <div class="container my-5">
        <h2 class="text-center text-pink-600 mb-4">Lista de Clientes</h2>
        <asp:Repeater ID="ClientRepeater" runat="server">
            <HeaderTemplate>
                <div class="table-responsive mt-4">
                    <table class="table table-striped">
                        <thead class="bg-pink-600 text-white">
                            <tr>
                                <th>Nombre</th>
                                <th>Email</th>
                                <th>Teléfono</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Nombre") %></td>
                    <td><%# Eval("Email") %></td>
                    <td><%# Eval("Telefono") %></td>
                    <td>
                        <asp:Button ID='btnEditar' runat='server' Text='Editar' 
                            CommandArgument='<%# Eval("ClienteID") %>' CssClass='btn bg-pink-500 text-white rounded-md hover:bg-pink-600 transition-all duration-300' 
                            OnClick='btnEditar_Click' />

                        <asp:Button ID='btnEliminar' runat='server' Text='Eliminar' 
                            CommandArgument='<%# Eval("ClienteID") %>' CssClass='btn bg-pink-500 text-white rounded-md hover:bg-pink-600 transition-all duration-300' 
                            OnClick='btnEliminar_Click' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <script>
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
</script>

</asp:Content>