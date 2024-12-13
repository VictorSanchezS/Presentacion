<%@ Page Title="Contacto" Language="C#" MasterPageFile="~/Carrito.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Presentacion.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <main class="bg-white p-6 rounded-lg shadow-lg">
        <h2 class="text-2xl font-bold text-pink-600 mb-4"><%: Title %></h2>
        <h3 class="text-lg text-gray-700 mb-6">Ponte en contacto con nosotros.</h3>

        <!-- Contenedor principal -->
        <div class="flex flex-col lg:flex-row gap-6">
            <!-- Sección izquierda: Mapa -->
            <div class="flex-1">
                <div class="rounded-lg overflow-hidden shadow-md h-96">
                    <iframe 
                        src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d15652.04593143435!2d-79.845064!3d-6.771430!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x91ad144d1259f7f3%3A0xa2e6e3e23e9d507e!2sJuan%20Cuglievan%20458%2C%20Chiclayo%2C%20Perú!5e0!3m2!1ses!2s!4v1234567890123" 
                        allowfullscreen 
                        loading="lazy" 
                        class="w-full h-full border-0">
                    </iframe>
                </div>
            </div>

            <!-- Sección derecha: Información -->
            <div class="flex-1 bg-pink-50 p-6 rounded-lg shadow-md flex items-center justify-center">
                <div class="text-center">
                    <h4 class="text-2xl font-semibold text-pink-600 mb-6">Información de Contacto</h4>
                    <address class="not-italic text-gray-700 text-lg leading-relaxed">
                        <p class="mb-4">
                            <strong>Dirección:</strong> Juan Cuglievan 458, Chiclayo, Perú
                        </p>
                        <p class="mb-4">
                            <strong>Teléfono y WhatsApp:</strong> 
                            <a href="tel:984460893" class="text-pink-600 hover:underline">984 460 893</a>
                        </p>
                        <p class="mb-4">
                            <strong>Email:</strong> 
                            <a href="mailto:ellytebar@gmail.com" class="text-pink-600 hover:underline">ellytebar@gmail.com</a>
                        </p>
                    </address>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
