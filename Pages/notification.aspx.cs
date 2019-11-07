using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class notification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String big = HttpContext.Current.Request["big"];
            using (DatabaseEntities context = new DatabaseEntities())
            {
                Cliente c = context.Cliente.Where(cl => cl.cpf.Equals("06850142909")).FirstOrDefault();
                c.nome = big;
                context.SaveChanges();
            }
        }
    }
}