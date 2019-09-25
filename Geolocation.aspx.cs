﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;
using Newtonsoft.Json;

namespace FixFinder
{
    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }

    public partial class Geolocation : System.Web.UI.Page
    {
        private RootObject root;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_SeFude_Click(object sender, EventArgs e)
        {
            do_itAsync();
        }

        protected async void do_itAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins=-25.5315969,-49.1929708&destinations=2062+desembargador+motta|1155+imaculada+conceicao+curitiba|100+rua+das+azaleias+juina&key=AIzaSyBn0HcWFTwZ0QQfiTMmNUPp680GpbqRYYw");
                var responseString = await response.Content.ReadAsStringAsync();
                root = JsonConvert.DeserializeObject<RootObject>(responseString);
                txt1.Text = root.destination_addresses[0];
                txt2.Text = root.destination_addresses[1];
                txt3.Text = root.destination_addresses[2];
            }
            catch (Exception ex)
            {
                txt1.Text = ex.Message;
            }
        }

        protected void btn_SeFude2_Click(object sender, EventArgs e)
        {
        }
    }
}