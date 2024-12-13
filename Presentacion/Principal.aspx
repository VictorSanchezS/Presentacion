<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="Presentacion.Principal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Título grande de bienvenida -->
    <div class="text-center py-16 bg-gradient-to-r from-pink-500 to-pink-700 text-white">
        <h1 class="text-4xl md:text-6xl font-extrabold leading-tight">Bienvenidos a Spa Belleza Total</h1>
        <p class="mt-4 text-lg md:text-xl">Disfruta de una experiencia única de belleza y bienestar</p>
    </div><br />

    <!-- Slider de imágenes -->
    <div class="relative">
        <div id="carouselExample" class="carousel slide relative" data-bs-ride="carousel" data-bs-interval="2000">
            <!-- Las imágenes del slider -->
            <div class="carousel-inner relative w-full overflow-hidden">
                <div class="carousel-item active">
                    <img src="images/principal1.jpeg" class="d-block w-[800px] h-[600px] object-cover mx-auto" alt="Imagen 1">
                </div>
                <div class="carousel-item">
                    <img src="images/principal2.jpeg" class="d-block w-[800px] h-[600px] object-cover mx-auto" alt="Imagen 2">
                </div>
                <div class="carousel-item">
                    <img src="images/principal3.jpeg" class="d-block w-[800px] h-[600px] object-cover mx-auto" alt="Imagen 3">
                </div>
            </div>
            
            <!-- Controles del slider -->
            <button class="carousel-control-prev" type="button" data-bs-target="#carouselExample" data-bs-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Previous</span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#carouselExample" data-bs-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Next</span>
            </button>
        </div>
    </div>

</asp:Content>
