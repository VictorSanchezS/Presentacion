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
    public partial class RegistroAsistencia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarAsistencias(); // Mostrar todos los registros inicialmente
            }
        }

        private void CargarAsistencias(string fecha = null, string horaInicio = null, string horaFin = null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ConsultarAsistenciasPorRango", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fecha", string.IsNullOrEmpty(fecha) ? DBNull.Value : (object)fecha);
                cmd.Parameters.AddWithValue("@HoraInicio", string.IsNullOrEmpty(horaInicio) ? DBNull.Value : (object)horaInicio);
                cmd.Parameters.AddWithValue("@HoraFin", string.IsNullOrEmpty(horaFin) ? DBNull.Value : (object)horaFin);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAsistencias.DataSource = dt;
                gvAsistencias.DataBind();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtener los parámetros de la búsqueda
            string fecha = txtFecha.Text.Trim();
            string horaInicio = txtHoraInicio.Text.Trim();
            string horaFin = txtHoraFin.Text.Trim();

            // Validación de que la Hora Fin sea mayor que la Hora Inicio
            if (!string.IsNullOrEmpty(horaInicio) && !string.IsNullOrEmpty(horaFin))
            {
                // Convertir las horas a DateTime para poder compararlas
                DateTime horaInicioDate = DateTime.ParseExact(horaInicio, "HH:mm", null);
                DateTime horaFinDate = DateTime.ParseExact(horaFin, "HH:mm", null);

                // Validar que la hora fin sea mayor que la hora inicio
                if (horaFinDate <= horaInicioDate)
                {
                    lblErrorHora.Visible = true;  // Mostrar el mensaje de error
                    return;  // Detener el proceso, no ejecutar la búsqueda
                }
                else
                {
                    lblErrorHora.Visible = false;  // Ocultar el mensaje de error si la validación pasa
                }
            }

            // Llamar al procedimiento almacenado para obtener los resultados de la búsqueda
            string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ConsultarAsistenciasPorRango", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fecha", string.IsNullOrEmpty(fecha) ? (object)DBNull.Value : fecha);
                cmd.Parameters.AddWithValue("@HoraInicio", string.IsNullOrEmpty(horaInicio) ? (object)DBNull.Value : horaInicio);
                cmd.Parameters.AddWithValue("@HoraFin", string.IsNullOrEmpty(horaFin) ? (object)DBNull.Value : horaFin);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Mostrar los resultados en el GridView
                if (dt.Rows.Count > 0)
                {
                    gvAsistencias.DataSource = dt;
                    gvAsistencias.DataBind();
                    lblNoRegistros.Visible = false; // Ocultar mensaje de no registros
                }
                else
                {
                    gvAsistencias.DataSource = null; // Limpiar los datos previos
                    gvAsistencias.DataBind();
                    lblNoRegistros.Visible = true; // Mostrar mensaje si no hay registros
                }
            }
        }



        protected void gvAsistencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAsistencias.PageIndex = e.NewPageIndex;

            // Repetimos la búsqueda en caso de que haya filtros activos
            btnBuscar_Click(sender, e);
        }
    }
}