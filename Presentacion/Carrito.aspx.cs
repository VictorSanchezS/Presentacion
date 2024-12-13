using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class Carrito : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts(string searchQuery = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ObtenerProductos", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Si hay un término de búsqueda, lo pasamos como parámetro
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    cmd.Parameters.AddWithValue("@SearchQuery", searchQuery);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SearchQuery", DBNull.Value); // NULL si no hay búsqueda
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ProductRepeater.DataSource = dt;
                ProductRepeater.DataBind();
            }
        }


        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string searchQuery = SearchTextBox.Text.Trim();
            LoadProducts(searchQuery);
        }




        // Método que determina el mensaje de disponibilidad basado en el stock
        public string GetStockMessage(object stockObj)
        {
            int stock = Convert.ToInt32(stockObj);

            if (stock == 0)
            {
                return "Agotado";
            }
            else if (stock > 0 && stock <= 15)
            {
                return "Casi Agotado";
            }
            else
            {
                return "Disponible";
            }
        }

        // Método que devuelve el estilo (color) basado en el stock
        public string GetStockMessageStyle(object stockObj)
        {
            int stock = Convert.ToInt32(stockObj);

            if (stock == 0)
            {
                return "background-color: red; color: white;";
            }
            else if (stock > 0 && stock <= 15)
            {
                return "background-color: orange; color: white;";
            }
            else
            {
                return "background-color: green; color: white;";
            }
        }

        protected void AddToCartButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int productId = int.Parse(btn.CommandArgument);

            DataTable cart;
            if (Session["Cart"] == null)
            {
                cart = new DataTable();
                cart.Columns.Add("ProductoID", typeof(int));
                cart.Columns.Add("NombreProducto", typeof(string));
                cart.Columns.Add("Precio", typeof(decimal));
                cart.Columns.Add("Cantidad", typeof(int));
                cart.Columns.Add("Subtotal", typeof(decimal));
                cart.Columns.Add("ImagenURL", typeof(string));
                Session["Cart"] = cart;
            }
            else
            {
                cart = (DataTable)Session["Cart"];
            }

            string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("AgregarProductoAlCarrito", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@ProductoID", productId);

                // Parámetros de salida
                SqlParameter stockParam = new SqlParameter("@StockDisponible", SqlDbType.Int) { Direction = ParameterDirection.Output };
                SqlParameter precioParam = new SqlParameter("@Precio", SqlDbType.Decimal) { Direction = ParameterDirection.Output };
                SqlParameter nombreParam = new SqlParameter("@NombreProducto", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                SqlParameter imagenParam = new SqlParameter("@ImagenURL", SqlDbType.NVarChar, 200) { Direction = ParameterDirection.Output };
                SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 200) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(stockParam);
                cmd.Parameters.Add(precioParam);
                cmd.Parameters.Add(nombreParam);
                cmd.Parameters.Add(imagenParam);
                cmd.Parameters.Add(mensajeParam);

                cmd.ExecuteNonQuery();

                // Obtener los valores de salida
                int stockDisponible = (int)stockParam.Value;
                decimal precio = (decimal)precioParam.Value;
                string nombreProducto = nombreParam.Value.ToString();
                string imagenURL = imagenParam.Value.ToString();
                string mensaje = mensajeParam.Value.ToString();

                if (mensaje == "Producto encontrado y disponible.")
                {
                    DataRow[] rows = cart.Select("ProductoID = " + productId);
                    if (rows.Length == 0)
                    {
                        DataRow row = cart.NewRow();
                        row["ProductoID"] = productId;
                        row["NombreProducto"] = nombreProducto;
                        row["Precio"] = precio;
                        row["Cantidad"] = 1;
                        row["Subtotal"] = precio;
                        row["ImagenURL"] = imagenURL;
                        cart.Rows.Add(row);
                    }
                    else
                    {
                        rows[0]["Cantidad"] = (int)rows[0]["Cantidad"] + 1;
                        rows[0]["Subtotal"] = precio * (int)rows[0]["Cantidad"];
                    }

                    // Mostrar notificación de éxito
                    string script = $"toastr.success('Producto \"{nombreProducto}\" agregado al carrito.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
                }
                else
                {
                    // Mostrar notificación de error
                    string script = $"toastr.error('{mensaje}');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message", script, true);
                }
            }

            UpdateCart();
        }


        private void UpdateCart()
        {
            DataTable cart = (DataTable)Session["Cart"];
            CartRepeater.DataSource = cart;
            CartRepeater.DataBind();

            decimal total = 0;
            foreach (DataRow row in cart.Rows)
            {
                total += (decimal)row["Subtotal"];
            }
            TotalPrice.Text = total.ToString("F2");
            PayButton.Visible = cart.Rows.Count > 0;
        }

        protected void ViewCartButton_Click(object sender, EventArgs e)
        {
            CartPanel.Visible = !CartPanel.Visible;
        }

        protected void RemoveFromCartButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int productId = int.Parse(btn.CommandArgument);

            DataTable cart = (DataTable)Session["Cart"];
            DataRow[] rows = cart.Select("ProductoID = " + productId);

            if (rows.Length > 0)
            {
                string nombreProducto = rows[0]["NombreProducto"].ToString();
                cart.Rows.Remove(rows[0]);

                // Mensaje de eliminación con Toastr
                string script = $"toastr.warning('Producto \"{nombreProducto}\" eliminado del carrito.');";
                ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", script, true);

                UpdateCart();
            }
        }

        protected void PayButton_Click(object sender, EventArgs e)
        {
            // Calcular el total del carrito
            DataTable cart = (DataTable)Session["Cart"];
            decimal total = 0;

            foreach (DataRow row in cart.Rows)
            {
                total += (decimal)row["Subtotal"];
            }

            // Guardar el carrito en una sesión para enviarlo a la página de pagos
            Session["CartToPay"] = cart;

            // Redirigir a Pagos.aspx con el total como parámetro
            Response.Redirect($"Pagos.aspx?total={total:F2}");
        }

    }
}