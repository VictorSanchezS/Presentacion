using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public class GetChartData : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SpaDB"].ConnectionString;

        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(@"
                SELECT 
                    MONTH(FechaPago) AS Mes, 
                    Producto, 
                    SUM(Cantidad) AS CantidadVendida
                FROM dbo.Pagos
                GROUP BY MONTH(FechaPago), Producto
                ORDER BY Mes, CantidadVendida DESC", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }

        var data = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, int>>();

        foreach (DataRow row in dt.Rows)
        {
            string mes = new DateTime(1, Convert.ToInt32(row["Mes"]), 1).ToString("MMMM");
            string producto = row["Producto"].ToString();
            int cantidad = Convert.ToInt32(row["CantidadVendida"]);

            if (!data.ContainsKey(mes))
                data[mes] = new System.Collections.Generic.Dictionary<string, int>();

            data[mes][producto] = cantidad;
        }

        context.Response.ContentType = "application/json";
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(data));
    }

    public bool IsReusable => false;
}