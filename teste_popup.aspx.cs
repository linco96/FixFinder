using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder
{
    public partial class teste_popup : System.Web.UI.Page
    {
        private static string id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                id = Request.QueryString["ID"];
                // Just for demo purposes if you don't supply an ID in the querystring it will default to 123
                if (string.IsNullOrWhiteSpace(id))
                {
                    id = "123";
                }

                LiteralID.Text = id;

                ViewState["ID"] = id;
            }
            else
            {
                id = (string)ViewState["ID"];
            }
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            // Here we might want to do some pre-checks on product with an ID of this.id
            // to make sure it can be deleted, that the person has the rights or whatever.

            // Show the placeholder that contains our warning features
            PlaceWarning.Visible = true;
        }

        protected void ButtonYes_Click(object sender, EventArgs e)
        {
            // Complete the action
            System.Diagnostics.Debug.WriteLine("Delete product " + id);

            // I'm returning to the same page, but you might redirect somewhere else, or not
            // bother redirecting at all
            id = "Vai se matar, sua vaca. Brinks. Mas vai mesmo";
            Response.Redirect(System.IO.Path.GetFileName(Request.Path) + "?id=" + id);
        }

        protected void ButtonNo_Click(object sender, EventArgs e)
        {
            // We can implement some "undo" logic here if needed

            // I'm returning to the same page, but you might redirect somewhere else, or not
            // bother redirecting at all
            Response.Redirect(System.IO.Path.GetFileName(Request.Path) + "?id=" + id);
        }
    }
}