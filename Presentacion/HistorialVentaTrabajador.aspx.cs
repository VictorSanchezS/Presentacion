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
    public partial class HistorialVentaTrabajador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarHistorialVentas();
            }
        }
        private void CargarHistorialVentas()
        {
            // Cadena de conexión desde Web.config
            string conexionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                using (SqlCommand comando = new SqlCommand("sp_ObtenerHistorialVentaTrabajador", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        conexion.Open();
                        SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                        DataTable dtHistorialVentas = new DataTable();
                        adaptador.Fill(dtHistorialVentas);

                        gvHistorialVentas.DataSource = dtHistorialVentas;
                        gvHistorialVentas.DataBind();
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                    }
                }
            }
        }

        protected void gvHistorialVentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistorialVentas.PageIndex = e.NewPageIndex; // Cambiar al nuevo índice de la página
            CargarHistorialVentas(); // Recargar los datos
        }

    }
}