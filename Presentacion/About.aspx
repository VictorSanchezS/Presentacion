<%@ Page Title="Acerca de Nosotros" Language="C#" MasterPageFile="~/Carrito.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Presentacion.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <main aria-labelledby="title" class="bg-pink-50 p-5 md:p-10 rounded-lg shadow-lg">
        <div class="container mx-auto">
            <!-- Title Section with Hero Image -->
            <div class="relative mb-10">
                <!-- Usando la URL proporcionada para la imagen -->
                <img src="https://d2e5ushqwiltxm.cloudfront.net/wp-content/uploads/sites/60/2016/11/04085854/Queenstown-So-Spa-Hero-shot-Large.jpg" alt="Imagen de spa relajante con naturaleza y animales" class="w-full h-64 object-cover rounded-lg shadow-lg">
                <div class="absolute inset-0 bg-pink-700 bg-opacity-50 flex items-center justify-center rounded-lg">
                    <h2 id="title" class="text-4xl font-bold text-white text-center"><%: Title %> - Bienvenidos a Nuestro Spa</h2>
                </div>
            </div>

            <!-- Mission Section -->
            <section class="mb-10">
                <div class="flex flex-col md:flex-row items-center">
                    <!-- URL para la imagen de Misión -->
                    <img src="https://image-tc.galaxy.tf/wijpeg-afk7gbo5khgd50apkonar8vds/ellysspavilla.jpg?width=1920" alt="Imagen representando la misión del spa" class="w-full md:w-1/2 h-64 object-cover rounded-lg shadow-lg mb-5 md:mb-0 md:mr-5">
                    <div>
                        <h4 class="text-3xl font-semibold text-pink-600 mb-3">Misión</h4>
                        <p class="text-lg text-gray-700">
                            Ofrecer una experiencia única de relajación y bienestar a nuestros clientes a través de tratamientos personalizados y servicios de calidad, enfocados en mejorar la salud y la tranquilidad.
                        </p>
                    </div>
                </div>
            </section>

            <!-- Vision Section -->
            <section class="mb-10">
                <div class="flex flex-col md:flex-row-reverse items-center">
                    <!-- URL para la imagen de Visión -->
                    <img src="https://serapool.fra1.cdn.digitaloceanspaces.com/media/4752/1700134437065.png" alt="Imagen representando la visión del spa" class="w-full md:w-1/2 h-64 object-cover rounded-lg shadow-lg mb-5 md:mb-0 md:ml-5">
                    <div>
                        <h4 class="text-3xl font-semibold text-pink-600 mb-3">Visión</h4>
                        <p class="text-lg text-gray-700">
                            Convertirnos en un spa de referencia en el cuidado integral del cuerpo y la mente, reconocido por nuestras innovaciones y compromiso con el bienestar holístico.
                        </p>
                    </div>
                </div>
            </section>

            <!-- Values Section -->
            <section class="mb-10 bg-pink-100 p-5 rounded-lg shadow-md">
                <h4 class="text-3xl font-semibold text-pink-600 mb-3 text-center">Valores</h4>
                <ul class="list-disc list-inside text-lg text-gray-700 space-y-2 pl-5">
                    <li>Compromiso con el cliente</li>
                    <li>Sostenibilidad y respeto al medio ambiente</li>
                    <li>Profesionalismo y ética</li>
                    <li>Respeto y cuidado personal</li>
                </ul>
            </section>

            <!-- History Section -->
            <section class="mb-10">
                <div class="flex flex-col md:flex-row items-center">
                    <!-- URL para la imagen de Historia -->
                    <img src="https://scontent.fcix2-1.fna.fbcdn.net/v/t1.6435-9/204835620_124507393130006_3812121586104867395_n.jpg?_nc_cat=101&ccb=1-7&_nc_sid=833d8c&_nc_eui2=AeGHb3R_k4Adjk_vTe8oIij7fgfprZO8Mpx-B-mtk7wynBySKP7QzMZ-0BAb1Mst7RDxGX6_XDFxH1Sg35HIUUrV&_nc_ohc=VySEpqm7YLkQ7kNvgEkc0Gd&_nc_zt=23&_nc_ht=scontent.fcix2-1.fna&_nc_gid=AuheKXo3vS1mvVeAGpH6K-G&oh=00_AYCcPMWflrrgHnjMgjw5uM3MZjLTaOotVwtaX-DIddVCgA&oe=675C6C8F" alt="Imagen representando la historia del spa" class="w-full md:w-1/2 h-64 object-cover rounded-lg shadow-lg mb-5 md:mb-0 md:mr-5">
                    <div>
                        <h4 class="text-3xl font-semibold text-pink-600 mb-3">Nuestra Historia</h4>
                        <p class="text-lg text-gray-700">
                            Fundado en 2019, nuestro spa nació con la visión de crear un espacio donde nuestros clientes puedan encontrar paz y relajación. A lo largo de los años, hemos crecido, pero nuestra misión de promover el bienestar integral sigue siendo la misma.
                        </p>
                    </div>
                </div>
            </section>

            <!-- Team Section -->
            <section class="mb-10">
                <div class="flex flex-col md:flex-row-reverse items-center">
                    <!-- URL para la imagen de Equipo -->
                    <img src="https://scontent.fcix2-1.fna.fbcdn.net/v/t1.6435-9/176177359_101101438803935_8704477222307402694_n.jpg?_nc_cat=105&ccb=1-7&_nc_sid=cc71e4&_nc_eui2=AeEK239CuuPWAk-9TuCIGsTetyoO3lRvTjm3Kg7eVG9OOTZTTbpA3wcxLCymQ2Eq2iKSdVTftpAlp-2oWUVGEhTH&_nc_ohc=U6eZG-TtXDQQ7kNvgEz0lLr&_nc_zt=23&_nc_ht=scontent.fcix2-1.fna&_nc_gid=AkEo0ix9HrjD2Mj-qWb9dTx&oh=00_AYDYZufd5XtqyKE16QosCZpulutWu4X2fScL8dwxMN_eNw&oe=675C4F57" alt="Imagen representando el equipo del spa" class="w-full md:w-1/2 h-64 object-cover rounded-lg shadow-lg mb-5 md:mb-0 md:ml-5">
                    <div>
                        <h4 class="text-3xl font-semibold text-pink-600 mb-3">Nuestro Equipo</h4>
                        <p class="text-lg text-gray-700">
                            Nuestro equipo de profesionales altamente capacitados está comprometido con brindar un servicio de excelencia. Con especialistas en diversas áreas de la salud y el bienestar, cada miembro de nuestro equipo trabaja para asegurar una experiencia de calidad para cada cliente.
                        </p>
                    </div>
                </div>
            </section>

            <!-- Customer Experience Section -->
            <section class="mb-10 bg-pink-100 p-5 rounded-lg shadow-md">
                <h4 class="text-3xl font-semibold text-pink-600 mb-3 text-center">Compromiso con la Experiencia del Cliente</h4>
                <p class="text-lg text-gray-700 text-center">
                    En nuestro spa, priorizamos la comodidad y satisfacción de nuestros clientes, brindando un ambiente diseñado para maximizar la relajación y el bienestar en cada visita.
                </p>
            </section>
        </div>
    </main>
</asp:Content>
