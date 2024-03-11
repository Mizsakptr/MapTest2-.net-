using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Device.Location;


namespace MapTest2_.net_
{
    public partial class Form1 : Form
    {

        private class Coordinates
        {
            public double xCoordinate { get; set; }
            public double yCoordinate { get; set; }
            public int contamination { get; set; }
        }

        private DataGridView dataGridView;
        List<Tuple<double, double, int>> airpollution_o_meters = new List<Tuple<double, double, int>>
        {
            Tuple.Create(47.543222, 21.641693, 3),     //IK
            Tuple.Create(47.553589, 21.621519, 2),     //Főépület
            Tuple.Create(47.543014, 21.624529, 1)      //Fizika
            
        };

        Tuple<double, double> userCurrentCoords;


        private GeoCoordinateWatcher watcher;       



        public Form1()
        {

            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            InitializeWebView();            

            Thread serverThread = new Thread(StartWebServer);
            serverThread.Start();

            dataGridView1.Columns.Add("latitude", "latitude");
            dataGridView1.Columns.Add("longitude", "longitude");
            dataGridView1.Columns.Add("contamination", "contamination");


            foreach (var tuple in airpollution_o_meters)
            {
                dataGridView1.Rows.Add(tuple.Item1, tuple.Item2, tuple.Item3);
            }


        }

        private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {

            GeoCoordinate coordinate = e.Position.Location;
            double latitude = coordinate.Latitude;
            double longitude = coordinate.Longitude;


            
            label1.Text = latitude + "," + longitude;
            userCurrentCoords = new Tuple<double, double>(latitude, longitude);


        }
        private void GeoButton_Click(object sender, EventArgs e)
        {
            
            
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            watcher.PositionChanged += Watcher_PositionChanged;
            watcher.Start();
            Thread serverThread = new Thread(StartWebServerUser);
            serverThread.Start();

            webBrowser1.Document.InvokeScript("toUser");
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            watcher.Stop();
        }

        private void InitializeWebView()
        {

            string solutionDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(solutionDirectory, "MapView.html");

            Console.WriteLine(filePath);
            webBrowser1.Navigate(filePath);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }  

        private void StartWebServer()
        {

            string url = "http://localhost:8080/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);



            try
            {

                listener.Start();
                Console.WriteLine("Webserver fut: " + url);

                while (true)
                {

                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;


                    List<Tuple<double, double, int>> dataList = GetVariable();
                    string jsonString = JsonConvert.SerializeObject(dataList);

                    byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
                    HttpListenerResponse response = context.Response;
                    response.ContentType = "application/json"; 
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.Close();



                }
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine($"HTTPListenerException: {ex.Message}");
            }

            finally
            {

                listener.Close();
                Console.WriteLine("Webserver leállítva.");

            }
        }







        private void StartWebServerUser()
        {          

            string url = "http://localhost:8081/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);

            try
            {
               
                listener.Start();
                Console.WriteLine("Webserver fut: " + url);

                while (true)
                {

                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    
                    string jsonString = JsonConvert.SerializeObject(userCurrentCoords);

                    byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
                    HttpListenerResponse response = context.Response;
                    response.ContentType = "application/json"; 
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.Close();


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a webszerver indításakor: " + ex.Message);
            }            
           
                listener.Close();
                Console.WriteLine("Webserver2 leállítva.");        


        } 
        

       




        private List<Tuple<double, double, int>> GetVariable()
        {
            return airpollution_o_meters;
        }
    

        
        private List<Coordinates> getCoordinates()
        {
            List<Coordinates> CoordinateList = new List<Coordinates>();
            return CoordinateList;
        }

        private void button5_Click(object sender, EventArgs e)
        {            
            webBrowser1.Document.InvokeScript("toUser");
        }


    }


}
