using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class Productos : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;
        private int currentPage = 1;
        private const int itemsPerPage = 5;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                currentPage = 1;
                ViewState["CurrentPage"] = currentPage;
                CargarProductos();
            }
            else
            {
                currentPage = (int)ViewState["CurrentPage"];
            }

            ActualizarEstadoBotones();
        }

        private void CargarProductos()
        {
            currentPage = (int)ViewState["CurrentPage"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("CargarProductosPaginados", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros para la paginación
                cmd.Parameters.AddWithValue("@StartRow", (currentPage - 1) * itemsPerPage + 1);
                cmd.Parameters.AddWithValue("@EndRow", currentPage * itemsPerPage + 1);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                ProductRepeater.DataSource = dt;
                ProductRepeater.DataBind();
            }

            lblPaginaActual.Text = "Página: " + currentPage;
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string imagePath = string.Empty;

                // Subir imagen si es necesario
                if (FileUpload1.HasFile)
                {
                    string folderPath = Server.MapPath("~/Images/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    imagePath = "~/Images/" + fileName;
                    FileUpload1.SaveAs(Server.MapPath(imagePath));
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd;

                    if (string.IsNullOrEmpty(lblIDProducto.Text) || lblIDProducto.Text == "Generado automáticamente")
                    {
                        // Guardar nuevo producto
                        cmd = new SqlCommand("RegistrarProducto", connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        string successScript = "toastr.success('Producto agregado con éxito.');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Notification", successScript, true);
                    }
                    else
                    {
                        // Actualizar producto existente
                        cmd = new SqlCommand("EditarProducto", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductoID", int.Parse(lblIDProducto.Text));

                        string successScript = "toastr.info('Producto actualizado con éxito.');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Notification", successScript, true);
                    }

                    cmd.Parameters.AddWithValue("@NombreProducto", txtNombreProducto.Text);
                    cmd.Parameters.AddWithValue("@DescripcionProducto", txtDescripcionProducto.Text);
                    cmd.Parameters.AddWithValue("@Marca", txtMarca.Text);
                    cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));
                    cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@ImagenURL", imagePath);

                    cmd.ExecuteNonQuery();
                }

                CargarProductos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CerrarModal", "$('#modalRegistrarProducto').modal('hide');", true);
            }
            catch (Exception ex)
            {
                string errorScript = $"toastr.error('Error: {ex.Message}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notification", errorScript, true);
            }
        }

        private void LimpiarCampos()
        {
            lblIDProducto.Text = "Generado automáticamente";
            txtNombreProducto.Text = string.Empty;
            txtDescripcionProducto.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtStock.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            FileUpload1.Attributes.Clear();
            imgProducto.ImageUrl = "~/Images/default.png";
            lblMensaje.Visible = false;
        }


        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            int productoID = int.Parse(button.CommandArgument);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("ObtenerProductoPorID", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", productoID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblIDProducto.Text = reader["ProductoID"].ToString();
                    txtNombreProducto.Text = reader["NombreProducto"].ToString();
                    txtDescripcionProducto.Text = reader["DescripcionProducto"].ToString();
                    txtMarca.Text = reader["Marca"].ToString();
                    txtStock.Text = reader["Stock"].ToString();
                    txtPrecio.Text = reader["Precio"].ToString();
                    imgProducto.ImageUrl = reader["ImagenURL"].ToString();

                    btnGuardar.Text = "Actualizar";
                }
            }

            string script = "toastr.info('Producto seleccionado.');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Notification", script, true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#modalRegistrarProducto').modal('show');", true);
        }



        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ViewState["CurrentPage"] = currentPage;
                CargarProductos();
            }
            ActualizarEstadoBotones();
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            currentPage++;
            ViewState["CurrentPage"] = currentPage;
            CargarProductos();
            ActualizarEstadoBotones();
        }

        private void ActualizarEstadoBotones()
        {
            btnAnterior.Enabled = currentPage > 1;
        }

        protected void btnAbrirModal_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            btnGuardar.Text = "Guardar";
            ViewState["ProductoID"] = null;

            // Mostrar el modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#modalRegistrarProducto').modal('show');", true);
        }
    }
}