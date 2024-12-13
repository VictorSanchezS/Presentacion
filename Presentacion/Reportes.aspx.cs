using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;

namespace Presentacion
{
    public partial class Reportes : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarAños();
                CargarReporte();
            }
        }

        protected void btnLoadReport_Click(object sender, EventArgs e)
        {
            // Verificar si el filtro de año está seleccionado
            if (!string.IsNullOrEmpty(ddlYearFilter.SelectedValue))
            {
                Console.WriteLine("Año seleccionado: " + ddlYearFilter.SelectedValue);
            }
            else
            {
                Console.WriteLine("Año seleccionado: Todos los años");
            }

            // Recargar el reporte con el nuevo filtro
            CargarReporte();
        }



        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            DataTable dt = ObtenerDatosReporte();
            if (dt.Rows.Count > 0)
            {
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    Document pdfDoc = new Document(PageSize.A4, 20, 20, 50, 50);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    writer.PageEvent = new PdfFooter();
                    pdfDoc.Open();

                    // Agregar encabezado
                    Paragraph header = new Paragraph("SpaBellezaTotal", FontFactory.GetFont("Arial", 20, Font.BOLD, new BaseColor(255, 105, 180)))
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    pdfDoc.Add(header);
                    pdfDoc.Add(new Paragraph("\n"));

                    // Agregar subtítulo
                    Paragraph subHeader = new Paragraph("Reporte de Productos Vendidos por Mes", FontFactory.GetFont("Arial", 14, Font.BOLD))
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    pdfDoc.Add(subHeader);
                    pdfDoc.Add(new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 10)));

                    PdfPTable pdfTable = new PdfPTable(dt.Columns.Count)
                    {
                        WidthPercentage = 100
                    };

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE)))
                        {
                            BackgroundColor = new BaseColor(255, 105, 180),
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        pdfTable.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (object cell in row.ItemArray)
                        {
                            pdfTable.AddCell(new PdfPCell(new Phrase(cell.ToString(), FontFactory.GetFont("Arial", 10)))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }
                    }

                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();

                    byte[] bytes = memoryStream.ToArray();
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=ReporteProductosVendidos.pdf");
                    Response.BinaryWrite(bytes);
                    Response.End();
                }
            }
        }


        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = ObtenerDatosReporte();
            if (dt.Rows.Count > 0)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Reporte");

                    // Encabezados
                    worksheet.Cells["A1"].Value = "SpaBellezaTotal";
                    worksheet.Cells["A1"].Style.Font.Size = 18;
                    worksheet.Cells["A1"].Style.Font.Bold = true;
                    worksheet.Cells["A1"].Style.Font.Color.SetColor(System.Drawing.Color.HotPink);

                    worksheet.Cells["A2"].Value = "Reporte de Productos Vendidos por Mes";
                    worksheet.Cells["A3"].Value = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");

                    // Cargar datos
                    worksheet.Cells["A5"].LoadFromDataTable(dt, true);
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=ReporteProductosVendidos.xlsx");
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.End();
                }
            }
        }


        private DataTable ObtenerDatosReporte()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ObtenerVentas", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Agregar el parámetro si está seleccionado
                if (!string.IsNullOrEmpty(ddlYearFilter.SelectedValue))
                {
                    cmd.Parameters.AddWithValue("@Year", ddlYearFilter.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Year", DBNull.Value);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    da.Fill(dt);

                    // Depuración: Verifica los registros cargados
                    Console.WriteLine($"Registros encontrados: {dt.Rows.Count}");
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                }
            }

            return dt;
        }




        private void CargarAños()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT YEAR(FechaPago) AS Año FROM dbo.Pagos ORDER BY Año DESC\r\n", conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                ddlYearFilter.Items.Clear();
                ddlYearFilter.Items.Add(new System.Web.UI.WebControls.ListItem("Todos los años", ""));

                while (reader.Read())
                {
                    ddlYearFilter.Items.Add(new System.Web.UI.WebControls.ListItem(reader["Año"].ToString(), reader["Año"].ToString()));
                }
            }
        }

        private void CargarReporte()
        {
            DataTable dt = ObtenerDatosReporte();
            if (dt.Rows.Count > 0)
            {
                ReporteRepeater.DataSource = dt;
                ReporteRepeater.DataBind();
            }
            else
            {
                // Si no hay datos, limpiar el Repeater
                ReporteRepeater.DataSource = null;
                ReporteRepeater.DataBind();
            }
        }



        // Clase para pie de página en el PDF
        public class PdfFooter : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable footer = new PdfPTable(1)
                {
                    TotalWidth = document.PageSize.Width - 40,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };

                PdfPCell cell = new PdfPCell(new Phrase("Juan Cuglievan 458, Chiclayo, Perú - Cel: 984460893", FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.GRAY)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                footer.AddCell(cell);

                footer.WriteSelectedRows(0, -1, 20, 30, writer.DirectContent);
            }
        }
    }
}
