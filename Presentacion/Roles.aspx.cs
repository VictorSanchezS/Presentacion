using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class Roles : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPermisos();
                CargarRoles();
            }
        }

        private void CargarPermisos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.ListarPermisos", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                chkPermisos.DataSource = reader;
                chkPermisos.DataTextField = "NombrePermiso";
                chkPermisos.DataValueField = "PermisoID";
                chkPermisos.DataBind();
            }
        }

        private void CargarRoles()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.ListarRoles", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                RoleRepeater.DataSource = reader;
                RoleRepeater.DataBind();
            }
        }

        protected void btnGuardarRol_Click(object sender, EventArgs e)
        {
            string nombreRol = txtNombreRol.Text.Trim();
            int? rolID = null;

            // Si estamos editando, obtener el ID del rol
            if (!string.IsNullOrEmpty(Request.QueryString["RolID"]))
            {
                rolID = Convert.ToInt32(Request.QueryString["RolID"]);
            }

            // Verificar si el rol ya existe
            if (RolExiste(nombreRol, rolID))
            {
                lblMensaje.Visible = true;
                lblMensaje.Text = "El rol ya existe. Por favor, elige otro nombre.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand cmd;
                    // Si estamos editando, actualizar el rol
                    if (rolID.HasValue)
                    {
                        cmd = new SqlCommand("dbo.ActualizarRol", conn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RolID", rolID.Value);
                        cmd.Parameters.AddWithValue("@NombreRol", nombreRol);
                        cmd.ExecuteNonQuery();
                    }
                    // Si estamos creando, registrar un nuevo rol
                    else
                    {
                        cmd = new SqlCommand("dbo.RegistrarRol", conn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NombreRol", nombreRol);
                        cmd.ExecuteNonQuery();
                    }

                    // Actualizar los permisos asociados
                    int rolIDFinal = rolID ?? Convert.ToInt32(cmd.ExecuteScalar());  // Obtener el RolID, ya sea nuevo o editado

                    // Eliminar permisos existentes
                    SqlCommand cmdEliminarPermisos = new SqlCommand("dbo.EliminarPermisosPorRol", conn, transaction);
                    cmdEliminarPermisos.CommandType = CommandType.StoredProcedure;
                    cmdEliminarPermisos.Parameters.AddWithValue("@RolID", rolIDFinal);
                    cmdEliminarPermisos.ExecuteNonQuery();

                    // Registrar nuevos permisos seleccionados
                    foreach (ListItem item in chkPermisos.Items)
                    {
                        if (item.Selected)
                        {
                            SqlCommand cmdPermiso = new SqlCommand("dbo.RegistrarRolPermiso", conn, transaction);
                            cmdPermiso.CommandType = CommandType.StoredProcedure;
                            cmdPermiso.Parameters.AddWithValue("@RolID", rolIDFinal);
                            cmdPermiso.Parameters.AddWithValue("@PermisoID", Convert.ToInt32(item.Value));
                            cmdPermiso.ExecuteNonQuery();
                        }
                    }

                    // Confirmar la transacción si todo está bien
                    transaction.Commit();
                    lblMensaje.Visible = true;
                    lblMensaje.Text = "Rol y permisos actualizados exitosamente.";
                    CargarRoles();  // Recargar los roles para reflejar los cambios
                }
                catch (Exception ex)
                {
                    // Si hay un error, revertir la transacción
                    transaction.Rollback();
                    lblMensaje.Visible = true;
                    lblMensaje.Text = "Error al actualizar el rol y permisos: " + ex.Message;
                }
            }
        }


        private bool RolExiste(string nombreRol, int? rolID = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM dbo.Roles WHERE NombreRol = @NombreRol";

                // Excluir el rol actual si estamos editando
                if (rolID.HasValue)
                {
                    query += " AND RolID != @RolID";  // Excluye el rol que estamos editando
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NombreRol", nombreRol);

                if (rolID.HasValue)
                {
                    cmd.Parameters.AddWithValue("@RolID", rolID.Value);  // Solo si estamos editando, pasamos el rolID
                }

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;  // Si count > 0, significa que el rol ya existe
            }
        }



        protected void btnEditarRol_Click(object sender, EventArgs e)
        {
            Button btnEditar = (Button)sender;
            int rolID = Convert.ToInt32(btnEditar.CommandArgument);
            CargarRolParaEdicion(rolID);
        }

        private void CargarRolParaEdicion(int rolID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.ObtenerRol", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RolID", rolID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtNombreRol.Text = reader["NombreRol"].ToString();
                }
            }

            foreach (ListItem item in chkPermisos.Items)
            {
                item.Selected = false;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.ListarPermisosPorRol", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RolID", rolID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int permisoID = Convert.ToInt32(reader["PermisoID"]);

                    foreach (ListItem item in chkPermisos.Items)
                    {
                        if (item.Value == permisoID.ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        protected void btnEliminarRol_Click(object sender, EventArgs e)
        {
            Button btnEliminar = (Button)sender;
            int rolID = Convert.ToInt32(btnEliminar.CommandArgument);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.EliminarRolPermiso", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RolID", rolID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.EliminarRol", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RolID", rolID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            CargarRoles();
        }
    }
}
