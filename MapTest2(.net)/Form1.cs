using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Added Microsoft.Web.WebView2 reference grom nuget

namespace MapTest2_.net_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.ScriptErrorsSuppressed = false;
            InitializeWebView();
        }

        private void InitializeWebView()
        {
            string filePath = @"C:\Users\Mizsa\source\repos\MapTest2(.net)\asd.html";            
            webBrowser1.Navigate(filePath);                       
        }



        //private void InitializeWebView()
        //{
        //    // Létrehozunk egy Panel-t a Formon
        //    Panel webPanel = new Panel();
        //    webPanel.Size = new System.Drawing.Size(1000, 500);
        //    webPanel.Location = new System.Drawing.Point(50, 50);
        //    Controls.Add(webPanel);

        //    // Létrehozunk egy WebView2-t és hozzáadjuk a Panel-hez
        //    WebView2 webView = new WebView2();
        //    webView.Dock = DockStyle.Fill;
        //    webPanel.Controls.Add(webView);

        //    // Debrecen koordinátái
        //    double debrecenLatitude = 47.529984;
        //    double debrecenLongitude = 21.639022;

        //    string debrecenLatitudeString = debrecenLatitude.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);
        //    string debrecenLongitudeString = debrecenLongitude.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);

        //    // Bing Maps URL létrehozása a Debrecen koordinátái alapján
        //    string bingMapsUrl = $"https://www.bing.com/maps?cp={debrecenLatitudeString}~{debrecenLongitudeString}&lvl=12";

        //    // WebView betöltése a Bing Maps URL-jével
        //    webView.Source = new Uri(bingMapsUrl);
        //}

    }
}
