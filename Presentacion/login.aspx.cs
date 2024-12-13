using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace Presentacion
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] != null)
            {
                Response.Redirect("Principal.aspx");
            }
        }

        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;
            string usuario = TxtUser.Text.Trim();
            string contraseña = TxtPass.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
            {
                string script = "toastr.error('Por favor, complete todos los campos.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_ValidarUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NombreUsuario", usuario);
                        command.Parameters.AddWithValue("@Contraseña", contraseña);

                        object resultado = command.ExecuteScalar();

                        if (resultado != null && Convert.ToInt32(resultado) > 0)
                        {
                            Session["Usuario"] = usuario;

                            // Notificación de éxito
                            string script = "toastr.success('Inicio de sesión exitoso. Redirigiendo...');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);

                            // Redirigir después de un breve retraso
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", "setTimeout(function() { window.location.href = 'Principal.aspx'; }, 2000);", true);
                        }
                        else
                        {
                            // Notificación de error
                            string script = "toastr.error('Usuario o contraseña incorrectos.');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Notificación de error por problemas en la base de datos
                    string script = $"toastr.error('Error al conectar con la base de datos: {ex.Message}');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
                }
            }
        }
    }
}