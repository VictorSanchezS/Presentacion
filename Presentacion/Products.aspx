<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Products.aspx.cs" Inherits="Presentacion.Productos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Botón para abrir el modal -->
            <div class="text-center mb-4">
                <asp:Button ID="btnAbrirModal" runat="server" Text="Agregar Producto" CssClass="btn bg-pink-500 text-white rounded-md py-2 px-6 hover:bg-pink-600 transition-all duration-300" OnClick="btnAbrirModal_Click" />
            </div>

            <!-- Modal para registrar producto -->
            <div class="modal fade" id="modalRegistrarProducto" tabindex="-1" aria-labelledby="modalRegistrarProductoLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title text-pink-600" id="modalRegistrarProductoLabel">Registrar Producto</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanelModal" runat="server">
                                <ContentTemplate>
                                    <div class="form-container">
                                        <div class="mb-4">
                                            <label for="lblIDProducto" class="form-label text-pink-600">ID Producto:</label>
                                            <asp:Label ID="lblIDProducto" runat="server" CssClass="form-control readonly bg-gray-200 text-gray-600" Text="Generado automáticamente" />
                                        </div>
                                        <div class="mb-4">
                                            <label for="txtNombreProducto" class="form-label text-pink-600">Nombre del Producto:</label>
                                            <asp:TextBox ID="txtNombreProducto" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                        </div>
                                        <div class="mb-4">
                                            <label for="txtDescripcionProducto" class="form-label text-pink-600">Descripción:</label>
                                            <asp:TextBox ID="txtDescripcionProducto" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                        </div>
                                        <div class="mb-4">
                                            <label for="txtMarca" class="form-label text-pink-600">Marca:</label>
                                            <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                        </div>
                                        <div class="mb-4">
                                            <label for="txtStock" class="form-label text-pink-600">Stock:</label>
                                            <asp:TextBox ID="txtStock" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                        </div>
                                        <div class="mb-4">
                                            <label for="txtPrecio" class="form-label text-pink-600">Precio:</label>
                                            <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                        </div>
                                        <asp:Image ID="imgProducto" runat="server" Width="150px" Height="150px" CssClass="mx-auto block mb-4" />
                                        <div class="mb-4">
                                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control border-2 border-pink-500 rounded-md focus:ring-pink-500" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn bg-pink-500 text-white rounded-md py-2 px-6 hover:bg-pink-600 transition-all duration-300" />
                            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="False" CssClass="mt-3 text-pink-500"></asp:Label>
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Listado de productos -->
            <div class="container my-5">
                <h2 class="text-center text-pink-600 mb-4">Lista de Productos</h2>
                <asp:Repeater ID="ProductRepeater" runat="server">
                    <HeaderTemplate>
                        <div class="table-responsive mt-4">
                            <table class="table table-striped">
                                <thead class="bg-pink-600 text-white">
                                    <tr>
                                        <th>Imagen</th>
                                        <th>Nombre</th>
                                        <th>Descripción</th>
                                        <th>Marca</th>
                                        <th>Stock</th>
                                        <th>Precio</th>
                                        <th>Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                    </HeaderTemplate>
                   <ItemTemplate>
    <tr>
        <td>
            <img src='<%# ResolveUrl(Eval("ImagenURL").ToString()) %>' alt='Producto' class='img-thumbnail' style='width: 80px; height: 80px;' />
        </td>
        <td><%# Eval("NombreProducto") %></td>
        <td><%# Eval("DescripcionProducto") %></td>
        <td><%# Eval("Marca") %></td>
        <td><%# Eval("Stock") %></td>
        <td>S/ <%# Eval("Precio", "{0:F2}") %></td>
        <!-- Columna para las acciones -->
        <td>
            <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" 
                CommandArgument='<%# Eval("ProductoID") %>' CssClass="btn bg-pink-500 text-white rounded-md hover:bg-pink-600 transition-all duration-300" 
                OnClick="btnSeleccionar_Click" />
        </td>
    </tr>
</ItemTemplate>

                    <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                    <!-- Controles de paginación -->
                    <div class="text-center my-3">
                        <asp:Button ID="btnAnterior" runat="server" Text="Anterior" OnClick="btnAnterior_Click" CssClass="btn bg-pink-500 text-white rounded-md py-2 px-4 hover:bg-pink-600 transition-all duration-300" />
                        <asp:Label ID="lblPaginaActual" runat="server" Text="Página: 1" CssClass="mx-3 text-pink-600 font-bold" />
                        <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" OnClick="btnSiguiente_Click" CssClass="btn bg-pink-500 text-white rounded-md py-2 px-4 hover:bg-pink-600 transition-all duration-300" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardar" />
        </Triggers>
    </asp:UpdatePanel>

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