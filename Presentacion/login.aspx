<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Presentacion.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SpaBellezaTotal - Login</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.4/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
    <style>
        @keyframes gradientAnimation {
            0% {
                background-position: 0% 50%;
            }
            50% {
                background-position: 100% 50%;
            }
            100% {
                background-position: 0% 50%;
            }
        }

        body {
            background: linear-gradient(45deg, #ff66b2, #ff4da6, #ff80bf);
            background-size: 300% 300%;
            animation: gradientAnimation 10s ease infinite;
            overflow: hidden;
            position: relative;
        }

        @keyframes moveUpAndRotate {
            0% {
                transform: translateY(100vh) rotate(0deg);
                opacity: 0;
            }
            100% {
                transform: translateY(-100vh) rotate(360deg);
                opacity: 1;
            }
        }

        .animated-box {
            position: absolute;
            width: 150px;
            height: 150px;
            background-color: rgba(255, 255, 255, 0.2);
            border-radius: 8px;
            animation: moveUpAndRotate 8s linear infinite;
            z-index: 0; 
        }

        .box-1 {
            bottom: 0;
            left: 20%;
            width: 100px;
            height: 100px;
            animation-duration: 4s;
        }

        .box-2 {
            bottom: 0;
            left: 50%;
            width: 200px;
            height: 200px;
            animation-duration: 8s;
        }

        .box-3 {
            bottom: 0;
            left: 30%;
            width: 120px;
            height: 120px;
            animation-duration: 10s;
        }

        .box-4 {
            bottom: 0;
            left: 70%;
            width: 180px;
            height: 180px;
            animation-duration: 12s;
        }

        .card {
            border: none;
            border-radius: 15px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            position: relative;
            z-index: 10;
            border: 3px solid #ff66b2;
            box-shadow: 0 0 15px rgba(255, 102, 178, 0.8);
        }

        .card-body {
            background-color: rgba(255, 255, 255, 0.8);
            border-radius: 15px;
            padding: 30px;
        }

        .form-control {
            margin-bottom: 15px;
            border-radius: 10px;
        }

        .btn-custom {
            background-color: #ff66b2;
            color: #ffffff;
            border: none;
            border-radius: 10px;
            width: 100%;
        }

        .btn-custom:hover {
            background-color: #ff4da6;
        }

        .profile-image {
            width: 200px;
            height: 200px;
            border-radius: 50%;
            object-fit: cover;
            margin-bottom: 20px;
            border: 3px solid #ff66b2;
            box-shadow: 0 0 15px rgba(255, 102, 178, 0.8);
            position: relative;
            z-index: 10;
        }

        .register-link {
            color: #ff66b2;
            text-decoration: none;
            font-size: 16px;
        }

        .register-link:hover {
            color: #ff4da6;
        }
    </style>
</head>
<body>
    <div class="container d-flex flex-column align-items-center justify-content-start min-vh-100" style="margin-top: 100px; position: relative; z-index: 10;">
        <img src="https://scontent.fcix2-1.fna.fbcdn.net/v/t39.30808-6/374651653_633790118863007_6617318750706831752_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=6ee11a&_nc_eui2=AeFPNClzrJCjyXHViE7lFazN7WxKl8GmogLtbEqXwaaiAiP6hKeh9iQuQjRPg7OTD2VqDiiWDSGkkbz5ksOeO4aG&_nc_ohc=R8tr0fexo9MQ7kNvgEPXNE0&_nc_zt=23&_nc_ht=scontent.fcix2-1.fna&_nc_gid=Al1HEjogQPMyEZF57xnFmYm&oh=00_AYC33e75F7kA5JKOOjH_jJ4YyQDUrLMSGCZkw2aL7PFZkA&oe=674B0A19" alt="Profile Image" class="profile-image" />
        <div class="card w-100" style="max-width: 400px;">
            <div class="card-body">
                <h2 class="text-center mb-4" style="color: #ff66b2;">Inicio de Sesión</h2>
                <form id="form1" runat="server">
                    <div>
                        <asp:TextBox ID="TxtUser" runat="server" CssClass="form-control" placeholder="Ingrese Usuario"></asp:TextBox>
                    </div>
                    <div>
                        <asp:TextBox ID="TxtPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese Contraseña"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button ID="BtnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-custom" OnClick="BtnIngresar_Click" />
                    </div>
                    <div class="mt-3">
                        <asp:Label ID="Mensaje" runat="server" CssClass="d-block text-center"></asp:Label>
                    </div>
                </form>
                <div class="text-center mt-3">
                    <a href="registro.aspx" class="register-link">¿No tienes cuenta? Regístrate aquí</a>
                </div>
            </div>
        </div>
    </div>

    <div class="animated-box box-1"></div>
    <div class="animated-box box-2"></div>
    <div class="animated-box box-3"></div>
    <div class="animated-box box-4"></div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>

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

</body>
</html>
