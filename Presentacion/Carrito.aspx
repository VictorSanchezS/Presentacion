<%@ Page Title="Carrito" Language="C#" MasterPageFile="~/Carrito.master" AutoEventWireup="true" CodeFile="Carrito.aspx.cs" Inherits="Presentacion.Carrito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
    <div class="container text-center">
        <!-- Campo de búsqueda alineado a la derecha -->
        <div class="mb-4 text-right">
            <asp:TextBox ID="SearchTextBox" runat="server" CssClass="border px-4 py-2 rounded-lg shadow-md" Placeholder="Buscar productos..." />
            <asp:Button ID="SearchButton" runat="server" Text="Buscar" CssClass="bg-pink-500 text-white hover:bg-pink-700 px-4 py-2 rounded-lg" OnClick="SearchButton_Click" />
        </div>

        <!-- Botón para ver carrito -->
        <asp:Button ID="ViewCartButton" runat="server" Text="Ver Carrito" CssClass="bg-pink-500 text-white hover:bg-pink-700 mt-3 px-5 py-3 rounded-lg font-semibold" OnClick="ViewCartButton_Click" />

        <!-- Panel del carrito -->
        <asp:Panel ID="CartPanel" runat="server" Visible="false" CssClass="mt-4 bg-white shadow-lg rounded-lg p-4">
            <h2 class="text-2xl font-bold text-pink-500">Carrito de Compras</h2>
            <asp:Repeater ID="CartRepeater" runat="server">
                <HeaderTemplate>
                    <div class="table-responsive mt-4">
                        <table class="table table-striped">
                            <thead class="bg-pink-100">
                                <tr>
                                    <th>Imagen</th>
                                    <th>Producto</th>
                                    <th>Cantidad</th>
                                    <th>Precio Unitario</th>
                                    <th>Subtotal</th>
                                    <th>Eliminar</th>
                                </tr>
                            </thead>
                            <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="text-center">
                            <div class="flex justify-center">
                                <asp:Image ID="ProductImage" runat="server" Width="100" Height="100" ImageUrl='<%# Eval("ImagenURL") %>' CssClass="img-fluid rounded transform transition-transform hover:scale-110" />
                            </div>
                        </td>
                        <td><%# Eval("NombreProducto") %></td>
                        <td><%# Eval("Cantidad") %></td>
                        <td>S/ <%# Eval("Precio", "{0:F2}") %></td>
                        <td>S/ <%# Eval("Subtotal", "{0:F2}") %></td>
                        <td>
                            <asp:Button ID="RemoveFromCartButton" runat="server" Text="Eliminar" CommandArgument='<%# Eval("ProductoID") %>' CssClass="bg-red-500 text-white hover:bg-red-700 px-4 py-2 rounded" OnClick="RemoveFromCartButton_Click" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </div>
            </FooterTemplate>
            </asp:Repeater>

            <div class="mt-4">
                <h4 class="font-semibold text-pink-500">Total del Carrito: S/ <asp:Label ID="TotalPrice" runat="server"></asp:Label></h4>
            </div>

            <div class="mt-3">
                <asp:Button ID="PayButton" runat="server" Text="Pagar" CssClass="bg-pink-500 text-white hover:bg-pink-700 px-5 py-3 rounded-lg font-semibold" OnClick="PayButton_Click" Visible="false" />
            </div>
        </asp:Panel>

        <!-- Repeater para los productos -->
        <h2 class="mt-5 text-pink-500 font-bold text-xl">Productos</h2><br />
        <div class="row justify-content-center">
            <asp:Repeater ID="ProductRepeater" runat="server">
                <ItemTemplate>
                    <div class="col-md-4 mb-4">
                        <div class="card shadow-lg rounded-lg">
                            <div class="card-body p-4">
                                <div class="flex justify-center mb-3">
                                    <asp:Image ID="ProductImage" runat="server" Width="200" Height="200" ImageUrl='<%# Eval("ImagenURL") %>' CssClass="img-fluid rounded transform transition-transform hover:scale-110" />
                                </div>

                                <div class="availability-status absolute top-0 left-0 m-2 p-1 text-sm font-bold rounded" style='<%# GetStockMessageStyle(Eval("Stock")) %>'>
                                    <%# GetStockMessage(Eval("Stock")) %>
                                </div>

                                <h5 class="card-title text-pink-500 font-semibold text-center"><%# Eval("NombreProducto") %></h5>
                                <p class="card-text text-center">Descripción: <%# Eval("DescripcionProducto") %></p>
                                <p class="card-text text-center">Marca: <%# Eval("Marca") %></p>
                                <p class="card-text text-center">Stock: <%# Eval("Stock") %></p>
                                <p class="card-text text-center">Precio: S/ <%# Eval("Precio") %></p>

                                <asp:Button ID="AddToCartButton" runat="server" Text="Agregar al Carrito" CommandArgument='<%# Eval("ProductoID") %>' CssClass="bg-pink-500 text-white hover:bg-pink-700 mt-3 px-5 py-3 rounded-lg" OnClick="AddToCartButton_Click" />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <!-- Botón flotante de WhatsApp -->
    <div class="fixed bottom-4 right-4">
        <a href="https://wa.me/51984460893" target="_blank" 
           class="bg-green-500 hover:bg-green-600 text-white px-4 py-4 rounded-full shadow-lg flex items-center justify-center transform transition-transform hover:scale-110">
            <img src="https://img.icons8.com/color/48/whatsapp.png" alt="WhatsApp" class="w-8 h-8" />
        </a>
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
