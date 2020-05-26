using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SqliteLib;
using SqliteLib.Model;
using System.ComponentModel;
using System.IO;
using System.Globalization;

namespace WebClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GenerateData(100);
            //Test();
        }

        private static void GenerateData(int cnt)
        {
            List<Note> notes = new List<Note>();
            Random random = new Random();
            for(int i=0;i<cnt; i++)
            {
                Note note = new Note();
                note.DateTime = DateTime.Now;
                note.Indication = random.NextDouble() / 2;
                note.Latitude = random.Next(0, 360);
                note.Longtitude = random.Next(0, 360);
                notes.Add(note);
            }

            FileStream fileStream = new FileStream(@"data.txt", FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            int j=0;
            foreach(var n in notes)
            {   j++;
                streamWriter.WriteLine(n.DateTime.ToString());
                streamWriter.WriteLine(n.Indication.ToString("N3",CultureInfo.InvariantCulture));
                streamWriter.WriteLine(n.Latitude);     
                if(j==cnt)
                    streamWriter.Write(n.Longtitude);
                else 
                    streamWriter.WriteLine(n.Longtitude);
                
            }
            
            streamWriter.Close();
            fileStream.Close();
        }

        private static void Test()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.BaseAddress = new Uri("https://localhost:44373");
            //ListOfNotes listOfNotes = new ListOfNotes();
            //List<Note> notes = new List<Note>();
            //for (int i = 0; i < 10; i++)
            //{
            //    Note note = new Note();
            //    note.DateTime = new DateTime(2000+i, 12, 12, 12, 12, 12);
            //    note.Indication = (double)36 % i;
            //    note.Latitude = (double)56 / i;
            //    note.Longtitude = (double)57 * i;
            //    notes.Add(note);
            //}
            //listOfNotes.notes = notes;
            //var json = JsonConvert.SerializeObject(listOfNotes);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");
            //var request = httpClient.PostAsync("/api/Note", data);
            //request.Wait();
            //var responce = request.Result;
            Note note = new Note();
            note.DateTime = new DateTime(2000, 12, 12, 12, 12, 12);
            note.Indication = (double)36;
            note.Latitude = (double)56;
            note.Longtitude = (double)57;
            var json = JsonConvert.SerializeObject(note);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var request = httpClient.PostAsync("/api/Note", data);
            request.Wait();
            var responce = request.Result;

        }

        private static void Test2()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.BaseAddress = new Uri("https://localhost:44373");
            ListOfNotes listOfNotes = new ListOfNotes();

            for (int i = 0; i < 10; i++)
            {
                listOfNotes.notes[i] = new Note();
                listOfNotes.notes[i].DateTime = new DateTime(2000 + i, 12, 12, 12, 12, 12);
                listOfNotes.notes[i].Indication = (double)36 % (i+1);
                listOfNotes.notes[i].Latitude = (double)56 / (i+1);
                listOfNotes.notes[i].Longtitude = (double)57 * (i+1);
            }
            var json = JsonConvert.SerializeObject(listOfNotes, Formatting.Indented);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");
            var request = httpClient.PostAsJsonAsync("/api/Note/Items", json);
            request.Wait();
            var responce = request.Result;


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // Test2();
        }
    }
}
