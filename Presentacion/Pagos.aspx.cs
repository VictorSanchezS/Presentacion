using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Presentacion
{
    public partial class Pagos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCarrito();
            }
        }

        private void CargarCarrito()
        {
            if (Session["CartToPay"] != null)
            {
                DataTable cart = (DataTable)Session["CartToPay"];
                ProductRepeater.DataSource = cart;
                ProductRepeater.DataBind();

                decimal total = 0;
                foreach (DataRow row in cart.Rows)
                {
                    total += (decimal)row["Subtotal"];
                }

                TotalLabel.Text = $"S/ {total:F2}";
            }
            else
            {
                TotalLabel.Text = "No hay productos en el carrito.";
                Session["CartToPay"] = new DataTable(); // Evitar null en el carrito
            }
        }

        protected void ProcesarPago_Click(object sender, EventArgs e)
        {
            // Capturar los datos del formulario
            string metodoPago = MetodoPagoDropDown.SelectedValue;
            decimal total = decimal.Parse(TotalLabel.Text.Replace("S/ ", ""));
            string departamento = Request.Form["departamento"] ?? string.Empty;
            string provincia = Request.Form["provincia"] ?? string.Empty;
            string distrito = Request.Form["distrito"] ?? string.Empty;
            string direccion = Request.Form["direccion"] ?? string.Empty;

            DateTime fechaEntrada = DateTime.Now; // Definir fecha de entrada
            DateTime fechaVenta = DateTime.Now;   // Definir fecha de venta

            try
            {
                // Validar que haya productos en el carrito
                if (Session["CartToPay"] == null || ((DataTable)Session["CartToPay"]).Rows.Count == 0)
                {
                    // Aquí puedes agregar un mensaje de error o manejar la situación.
                    return;
                }

                // Conexión a la base de datos
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("RegistrarPagoConProductos", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros del pago
                        cmd.Parameters.Add(new SqlParameter("@MetodoPago", SqlDbType.VarChar) { Value = metodoPago });
                        cmd.Parameters.Add(new SqlParameter("@Total", SqlDbType.Decimal) { Value = total });
                        cmd.Parameters.Add(new SqlParameter("@Departamento", SqlDbType.VarChar) { Value = departamento ?? (object)DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@Provincia", SqlDbType.VarChar) { Value = provincia ?? (object)DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@Distrito", SqlDbType.VarChar) { Value = distrito ?? (object)DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@Direccion", SqlDbType.NVarChar) { Value = direccion ?? (object)DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@FechaEntrada", SqlDbType.Date) { Value = fechaEntrada });
                        cmd.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.Date) { Value = fechaVenta });

                        // Crear el DataTable para @CarritoDeCompra
                        DataTable cartTable = new DataTable();
                        cartTable.Columns.Add("ProductoID", typeof(int));
                        cartTable.Columns.Add("Cantidad", typeof(int));

                        // Llenar el DataTable con los productos en el carrito
                        DataTable cart = (DataTable)Session["CartToPay"];
                        foreach (DataRow row in cart.Rows)
                        {
                            cartTable.Rows.Add(row["ProductoID"], row["Cantidad"]);
                        }

                        // Crear el parámetro para el DataTable
                        cmd.Parameters.Add(new SqlParameter("@CarritoDeCompra", SqlDbType.Structured)
                        {
                            TypeName = "dbo.CarritoDeCompraType", // Nombre del tipo de tabla en SQL
                            Value = cartTable
                        });

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();
                    }
                }

                // Confirmación
                Response.Redirect("ConfirmacionPago.aspx");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, tal vez un mensaje al usuario.
                Response.Write($"Error: {ex.Message}");
            }
        }
    }
}
