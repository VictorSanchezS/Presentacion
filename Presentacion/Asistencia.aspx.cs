using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class Asistencia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTrabajadores(); // Cargar trabajadores al cargar la página
            }
        }

        private void LoadTrabajadores(string searchQuery = null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_ObtenerTrabajadores", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    cmd.Parameters.AddWithValue("@SearchQuery", searchQuery);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SearchQuery", DBNull.Value);
                }

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlTrabajadores.Items.Clear(); // Limpiar el DropDownList
                    ddlTrabajadores.Items.Add(new ListItem("Seleccione un trabajador", "")); // Opción predeterminada

                    bool found = false; // Verificar si se encontró algún trabajador

                    while (reader.Read())
                    {
                        string nombre = reader["NombreCompleto"].ToString();
                        string id = reader["TrabajadorID"].ToString();

                        ListItem item = new ListItem(nombre, id);
                        ddlTrabajadores.Items.Add(item);

                        // Si se está buscando, selecciona automáticamente el trabajador
                        if (!string.IsNullOrEmpty(searchQuery) &&
                            nombre.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            ddlTrabajadores.SelectedValue = id;
                            found = true;
                        }

                    }

                    // Si no se encontró, mostrar alerta
                    if (!found && !string.IsNullOrEmpty(searchQuery))
                    {
                        ClientScript.RegisterStartupScript(
                            this.GetType(),
                            "alert",
                            $"alert('No se encontró ningún trabajador con el nombre \"{searchQuery}\".');",
                            true
                        );
                    }
                }
                catch (Exception ex)
                {
                    lblBuscarTrabajador.Text = "Error al cargar trabajadores: " + ex.Message;
                }
            }
        }


        protected void btnRegistrarEntrada_Click(object sender, EventArgs e)
        {
            // Verifica si hay un trabajador seleccionado en el DropDownList
            if (ddlTrabajadores.SelectedValue == null || string.IsNullOrEmpty(ddlTrabajadores.SelectedValue))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Por favor, seleccione un trabajador antes de registrar la entrada.');", true);
                return;
            }

            // Obtiene los valores del formulario
            int trabajadorID = int.Parse(ddlTrabajadores.SelectedValue);
            string fechaAsistencia = DateTime.Now.ToString("yyyy-MM-dd");
            string horaEntrada = DateTime.Now.ToString("HH:mm:ss");

            // Conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Configuración del comando SQL para llamar al procedimiento almacenado
                    using (SqlCommand cmd = new SqlCommand("RegistrarEntrada", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros al procedimiento almacenado
                        cmd.Parameters.AddWithValue("@TrabajadorID", trabajadorID);
                        cmd.Parameters.AddWithValue("@FechaAsistencia", fechaAsistencia);
                        cmd.Parameters.AddWithValue("@HoraEntrada", horaEntrada);

                        // Ejecutar el procedimiento
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Entrada registrada correctamente.');", true);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No se pudo registrar la entrada. Ya se registro en esta fecha.');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error al registrar la entrada: {ex.Message}');", true);
                }
            }
        }

        protected void btnRegistrarSalida_Click(object sender, EventArgs e)
        {
            // Verifica si hay un trabajador seleccionado en el DropDownList
            if (ddlTrabajadores.SelectedValue == null || string.IsNullOrEmpty(ddlTrabajadores.SelectedValue))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Por favor, seleccione un trabajador antes de registrar la salida.');", true);
                return;
            }

            // Obtiene los valores necesarios
            int trabajadorID = int.Parse(ddlTrabajadores.SelectedValue);
            string fechaAsistencia = DateTime.Now.ToString("yyyy-MM-dd");
            string horaSalida = DateTime.Now.ToString("HH:mm:ss");

            // Conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Configuración del comando SQL para llamar al procedimiento almacenado
                    using (SqlCommand cmd = new SqlCommand("RegistrarSalida", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros al procedimiento almacenado
                        cmd.Parameters.AddWithValue("@TrabajadorID", trabajadorID);
                        cmd.Parameters.AddWithValue("@FechaAsistencia", fechaAsistencia);
                        cmd.Parameters.AddWithValue("@HoraSalida", horaSalida);

                        // Ejecutar el procedimiento
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Salida registrada correctamente.');", true);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ya se ha registrado la salida para este trabajador en la fecha indicada.');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error al registrar la salida: {ex.Message}');", true);
                }
            }
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string searchQuery = txtBuscarTrabajador.Text.Trim();
            LoadTrabajadores(searchQuery); // Buscar trabajadores
        }
    }
}