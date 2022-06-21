using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net;

namespace DVPProjectBored
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HandleClientWindowSize();

            //Set the initial values of the form controls.
            cmb_Category.Text = "Random";
            num_Participants.Value = 1;
            cmb_Price.Text = "Any";
            cmb_Acc.Text = "Any";
        }

        WebClient webConnection = new WebClient();

        //Returns a random activity if nothing is added.
        string baseAPI = ("http://www.boredapi.com/api/activity?");

        //Used to hold the decided options as a finished string to attach to baseAPI.
        string fullAPI = "";
    
        //This holds the returned api object.
        JObject returnedJSON = new JObject();

        //Creates the fullAPI string by getting the form data.
        //and putting them into the correct format.
        private void CreateAPI()
        {
            string category = "";
            string participants = "";
            string price = "";
            string access = "";

            if (cmb_Category.Text != "Random"){ category = $@"type={cmb_Category.Text.ToLower()}";}

            switch(cmb_Price.Text)
            {
                case "Any":
                    price = @"&minprice=0&maxprice=1.0";
                    break;
                case "Free":
                    price = @"&minprice=0&maxprice=0.0";
                    break;
                case "Low":
                    price = @"&minprice=0&maxprice=0.2";
                    break;
                case "Medium":
                    price = @"&minprice=0&maxprice=0.5";
                    break;
                case "High":
                    price = @"&minprice=0&maxprice=1";
                    break;
            }

            switch (cmb_Acc.Text)
            {
                case "Any":
                    access = @"&minaccessibility=0&maxaccessibility=1.0";
                    break;
                case "Easy":
                    access = @"&minaccessibility=0&maxaccessibility=0.3";
                    break;
                case "Medium":
                    access = @"&minaccessibility=0&maxaccessibility=0.6";
                    break;
                case "Hard":
                    access = @"&minaccessibility=0&maxaccessibility=0.1";
                    break;
            }

            decimal part = num_Participants.Value;
            participants = $@"&participants={part.ToString()}";

            //Build the api string.
            fullAPI = $"{category}" + $"{participants}" + $"{price}" + $"{access}";

            //Combine the base and full api for sending.
            fullAPI = baseAPI + fullAPI;

        }

        //Create call and send to web.
        private void btn_GetIdea_Click(object sender, EventArgs e)
        {
            CreateAPI();
            ReadData();
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            cmb_Category.Text = "Random";
            num_Participants.Value = 1;
            cmb_Price.Text = "Any";
            cmb_Acc.Text = "Any";
        }

        //Get and read JSON data
        private void ReadData()
        {
            for (int i = 0; i < 10; i++)
            {
                webConnection = new WebClient();

                string apiData = fullAPI;

                try
                {
                    apiData = webConnection.DownloadString(fullAPI);
                }
                catch (WebException e)
                {
                    MessageBox.Show("Failed to Connect");
                    return;
                }

                //Parse data string to jObject
                returnedJSON = JObject.Parse(apiData);

                //Show in the text area the returned activity or the error text.
                if(returnedJSON.ContainsKey("activity"))
                {
                    txt_Idea.Text = ("Activity: " + returnedJSON["activity"].ToString() +".");
                }
                else
                {
                    txt_Idea.Text = "BoredAPI is limited at this time and has not found an activity for you with the selected perameters...";
                }
            }
        }
        
       
        void HandleClientWindowSize()
        {
            float HeightValueToChange = 1.4f;
            float WidthValueToChange = 6.0f;

            int height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Size.Height / HeightValueToChange);
            int width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Size.Width / WidthValueToChange);
            if (height < Size.Height)
                height = Size.Height;
            if (width < Size.Width)
                width = Size.Width;
            this.Size = new Size(width, height);
            //this.Size = new Size(376, 720);
        }
    }
}
