using System;
using System.Web;
using System.Web.UI;

namespace Presentacion
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null && !Page.IsPostBack)
            {
                Response.Redirect("~/login.aspx");
            }
        }

        protected void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();

            Response.Redirect("~/login.aspx");
        }
    }
}