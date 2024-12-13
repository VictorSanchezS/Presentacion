using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class Clientes : System.Web.UI.Page
    {
        // Cadena de conexión
        string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
            }
        }

        // Cargar la lista de clientes desde la base de datos
        private void CargarClientes()
        {
            DataTable dtClientes = ListarClientes();
            ClientRepeater.DataSource = dtClientes;
            ClientRepeater.DataBind();
        }

        // Obtener los clientes desde la base de datos utilizando el procedimiento almacenado ListarClientes
        private DataTable ListarClientes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.ListarClientes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // Guardar un cliente nuevo o actualizar uno existente
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreCliente.Text;
            string email = txtEmailCliente.Text;
            string telefono = txtTelefonoCliente.Text;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telefono))
            {
                // Mostrar error si algún campo está vacío
                string script = "toastr.error('Todos los campos son obligatorios.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
                return;
            }

            try
            {
                if (ViewState["ClienteID"] != null)
                {
                    // Editar cliente
                    int clienteID = Convert.ToInt32(ViewState["ClienteID"]);
                    EditarCliente(clienteID, nombre, email, telefono);

                    string script = "toastr.success('Cliente actualizado correctamente.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
                }
                else
                {
                    // Agregar cliente
                    RegistrarCliente(nombre, email, telefono);

                    string script = "toastr.success('Cliente registrado correctamente.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
                }

                // Limpiar los campos y recargar los clientes
                txtNombreCliente.Text = "";
                txtEmailCliente.Text = "";
                txtTelefonoCliente.Text = "";
                CargarClientes();

                // Restablecer estado del botón y ViewState
                btnGuardar.Text = "Guardar";
                ViewState["ClienteID"] = null;

                // Cerrar el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "$('#modalRegistrarCliente').modal('hide');", true);
            }
            catch (Exception ex)
            {
                // Mostrar error en caso de excepción
                string script = $"toastr.error('Error: {ex.Message}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
            }
        }

        // Procedimiento para registrar un nuevo cliente
        private void RegistrarCliente(string nombre, string email, string telefono)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.RegistrarCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Procedimiento para actualizar un cliente
        private void EditarCliente(int clienteID, string nombre, string email, string telefono)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.EditarCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Eliminar un cliente
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            Button btnEliminar = (Button)sender;
            int clienteID = Convert.ToInt32(btnEliminar.CommandArgument);

            try
            {
                EliminarCliente(clienteID);

                // Notificación de éxito
                string script = "toastr.success('Cliente eliminado correctamente.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);

                // Recargar la lista de clientes
                CargarClientes();
            }
            catch (Exception ex)
            {
                // Mostrar error en caso de excepción
                string script = $"toastr.error('Error al eliminar cliente: {ex.Message}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
            }
        }

        // Procedimiento para eliminar un cliente
        private void EliminarCliente(int clienteID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.EliminarCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Abre el modal con los datos de un cliente para editar
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            Button btnEditar = (Button)sender;
            int clienteID = Convert.ToInt32(btnEditar.CommandArgument);

            Cliente cliente = ObtenerCliente(clienteID);

            if (cliente != null)
            {
                txtNombreCliente.Text = cliente.Nombre;
                txtEmailCliente.Text = cliente.Email;
                txtTelefonoCliente.Text = cliente.Telefono;

                ViewState["ClienteID"] = cliente.ClienteID;
                btnGuardar.Text = "Actualizar";

                // Abrir modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#modalRegistrarCliente').modal('show');", true);

                // Mostrar notificación
                string script = "toastr.info('Cliente cargado para edición.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
            }
            else
            {
                string script = "toastr.error('No se encontró el cliente.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
            }
        }

        // Obtener los datos de un cliente desde la base de datos
        private Cliente ObtenerCliente(int clienteID)
        {
            Cliente cliente = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerClientePorID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            ClienteID = Convert.ToInt32(reader["ClienteID"]),
                            Nombre = reader["Nombre"].ToString(),
                            Email = reader["Email"].ToString(),
                            Telefono = reader["Telefono"].ToString()
                        };
                    }
                }
            }
            return cliente;
        }

        // Abre el modal para agregar un nuevo cliente
        protected void btnAbrirModal_Click(object sender, EventArgs e)
        {
            // Limpiar los campos y mostrar el modal
            txtNombreCliente.Text = "";
            txtEmailCliente.Text = "";
            txtTelefonoCliente.Text = "";

            // Restablecer el texto del botón a "Guardar" en caso de que estemos en modo edición
            btnGuardar.Text = "Guardar";
            ViewState["ClienteID"] = null;

            // Mostrar el modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#modalRegistrarCliente').modal('show');", true);
        }

        // Clase Cliente para representar un cliente
        public class Cliente
        {
            public int ClienteID { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public string Telefono { get; set; }
        }
    }
}
