<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pagos.aspx.cs" Inherits="Presentacion.Pagos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pagar</title>
    <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function showPaymentForm() {
            var selectedPaymentMethod = document.getElementById('<%= MetodoPagoDropDown.ClientID %>').value;
            var formContainer = document.getElementById('FormularioPago');

            // Clear the form container
            formContainer.innerHTML = '';

            if (selectedPaymentMethod === 'tarjeta') {
                // Show tarjeta payment form
                formContainer.innerHTML =
                    '<h3 class="font-bold mb-4 text-2xl text-pink-600">Formulario de Pago con Tarjeta</h3>' +
                    '<label for="cardNumber" class="block text-pink-600">Número de tarjeta:</label>' +
                    '<input type="text" id="cardNumber" class="w-full p-3 mb-4 border rounded-lg shadow-md text-gray-800" placeholder="Ingresa tu número de tarjeta" required>' +
                    '<label for="expiryDate" class="block text-pink-600">Fecha de expiración:</label>' +
                    '<input type="text" id="expiryDate" class="w-full p-3 mb-4 border rounded-lg shadow-md text-gray-800" placeholder="MM/AA" required>' +
                    '<label for="cvv" class="block text-pink-600">CVV:</label>' +
                    '<input type="text" id="cvv" class="w-full p-3 mb-4 border rounded-lg shadow-md text-gray-800" placeholder="CVV" required>' +
                    '<label for="cardholderName" class="block text-pink-600">Nombre del titular:</label>' +
                    '<input type="text" id="cardholderName" class="w-full p-3 mb-4 border rounded-lg shadow-md text-gray-800" placeholder="Nombre del titular" required>' +
                    '<button type="button" onclick="procesarPago()" class="mt-4 p-3 bg-pink-500 text-white rounded-lg w-full hover:bg-pink-600 transition-colors">Procesar Pago</button>';
            } else if (selectedPaymentMethod === 'yape') {
                // Obtener el monto desde el servidor
                var totalAmount = '<%= TotalLabel.Text.Substring(3) %>'; // S/ 100.00 (quitamos "S/ " y usamos el monto)

                // Mostrar el código QR de Yape
                formContainer.innerHTML = 
                    '<h3 class="font-bold mb-4 text-2xl text-pink-600">Pago con Yape</h3>' +
                    '<p class="text-lg text-gray-700">Escanea el siguiente código QR para pagar:</p>' +
                    '<div class="relative mb-4">' +
                    '<img src="images/yape.png" alt="QR Yape" class="w-72 mx-auto border-4 border-pink-300 rounded-lg shadow-lg">' +
                    '</div>' +
                    '<p class="mt-4 text-xl text-pink-600 text-center">Monto total: <span class="font-bold">S/ ' + totalAmount + '</span></p>' +
                    '<button type="button" onclick="validarMonto()" class="mt-4 p-3 bg-pink-500 text-white rounded-lg w-full hover:bg-pink-600 transition-colors">Procesar Pago</button>' +
                    '<div id="mensajeValidacion" class="mt-4 text-center text-red-500"></div>';
            } else {
                formContainer.innerHTML = '';
            }
        }

        function procesarPago() {
            // Validar si todos los campos están completos
            if (!document.getElementById('cardNumber').value ||
                !document.getElementById('expiryDate').value ||
                !document.getElementById('cvv').value ||
                !document.getElementById('cardholderName').value) {
                alert("Por favor, complete todos los campos.");
                return;
            }
            // Simulación de pago exitoso
            alert("Pago realizado con éxito");
        }

        function validarMonto() {
            var montoTotal = parseFloat('<%= TotalLabel.Text.Substring(3) %>'); // Total a pagar desde el servidor
            // Validar el monto escaneado en el código QR
            var montoEscaneado = montoTotal; // El monto escaneado se considera igual al monto total
            var mensajeValidacion = document.getElementById('mensajeValidacion');

            if (montoEscaneado < montoTotal) {
                var diferencia = montoTotal - montoEscaneado;
                mensajeValidacion.textContent = "Monto insuficiente. Faltan S/ " + diferencia.toFixed(2) + " para completar el pago.";
                mensajeValidacion.classList.remove('text-green-500');
                mensajeValidacion.classList.add('text-red-500');
            } else if (montoEscaneado > montoTotal) {
                var diferencia = montoEscaneado - montoTotal;
                mensajeValidacion.textContent = "Sobrepasaste el monto. S/ " + diferencia.toFixed(2) + " de más.";
                mensajeValidacion.classList.remove('text-red-500');
                mensajeValidacion.classList.add('text-green-500');
            } else {
                mensajeValidacion.textContent = 'Pago realizado con éxito.';
                mensajeValidacion.classList.remove('text-red-500', 'text-green-500');
                mensajeValidacion.classList.add('text-blue-500');
            }
        }
    </script>
</head>
<body class="bg-pink-50">
    <form id="form1" runat="server">
        <div class="container mx-auto p-6">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
                <!-- Productos Seleccionados -->
                <div class="bg-white shadow-xl rounded-lg p-6">
                    <h2 class="text-2xl font-bold text-pink-600 mb-4">Productos seleccionados</h2>
                    <asp:Repeater ID="ProductRepeater" runat="server">
                        <ItemTemplate>
                            <div class="flex justify-between border-b py-3">
                                <span class="text-gray-800"><%# Eval("NombreProducto") %></span>
                                <span class="text-gray-600">S/ <%# Eval("Precio", "{0:F2}") %> x <%# Eval("Cantidad") %></span>
                                <span class="text-pink-600 font-bold">Subtotal: S/ <%# Eval("Subtotal", "{0:F2}") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="mt-6 font-bold text-xl text-pink-600">
                        Total a pagar: <asp:Label ID="TotalLabel" runat="server" Text=""></asp:Label>
                    </div>
                </div>

                <!-- Métodos de Pago -->
                <div class="bg-white shadow-xl rounded-lg p-6">
                    <h2 class="text-2xl font-bold text-pink-600 mb-4">Selecciona un método de pago</h2>
                    <asp:DropDownList ID="MetodoPagoDropDown" runat="server" 
                                      CssClass="w-full mb-6 p-3 border-2 rounded-lg shadow-md text-gray-800" 
                                      OnChange="showPaymentForm()">
                        <asp:ListItem Text="Yape" Value="yape"></asp:ListItem>
                        <asp:ListItem Text="Tarjeta de Crédito" Value="tarjeta"></asp:ListItem>
                    </asp:DropDownList>

                    <!-- Datos de Dirección y Ubicación -->
                    <label for="departamento" class="block text-pink-600">Departamento:</label>
                    <input type="text" id="departamento" class="w-full p-3 mb-4 border rounded-lg shadow-md text-gray-800">

                    <label for="provincia" class="block text-pink-600">Provincia:</label>
                    <input type="text" id="provincia" class="w-full p-3 mb-4 border rounded-lg shadow-md text-gray-800">

                    <label for="distrito" class="block text-pink-600">Distrito:</label>
                    <input type="text" id="distrito" class="w-full p-3 mb-4 border rounded-lg shadow-md text-gray-800">

                    <label for="direccion" class="block text-pink-600">Dirección:</label>
                    <input type="text" id="direccion" class="w-full p-3 mb-4 border rounded-lg shadow-md text-gray-800">

                    <div id="FormularioPago" class="mt-4">
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
