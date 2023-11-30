using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace WEATHERSAMY1
{
    public class Clouds
    {
        public int all { get; set; }
    }

    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Root
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public partial class MainWindow : Window
    {
        private Dictionary<string, Root> weatherData = new Dictionary<string, Root>();

        public MainWindow()
        {
            InitializeComponent();
            PopulateComboBox();
           // GetWeather(); // Appel initial avec la première ville par défaut
        }

        private void PopulateComboBox()
        {
            ComboBoxCities.Items.Add("Annecy");
            ComboBoxCities.Items.Add("Lyon");
            ComboBoxCities.Items.Add("Paris");
            ComboBoxCities.Items.Add("Marseille");
            ComboBoxCities.SelectedIndex = 0; // Sélectionnez Annecy par défaut
        }

        public async Task<string> GetWeather( string ville)
        {
            try
            {

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={ville.ToLower()},fr&appid=c21a75b667d6f7abb81f118dcf8d4611&units=metric");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Root root = JsonConvert.DeserializeObject<Root>(content);

                    if (weatherData.ContainsKey(ville))
                        weatherData[ville] = root;
                    else
                        weatherData.Add(ville, root);

                    Dispatcher.Invoke(() => UpdateUI(root));
                }
                else
                {

                }

                return "";
            }
            catch (Exception ex)
            {

            }

            return "";
        }

        private void UpdateUI(Root root)
        {
            Main main = root.main;

            TB_Temperature.Text = main.temp.ToString();
            TB_TemperatureMin.Text = main.temp_min.ToString();
            TB_TemperatureMax.Text = main.temp_max.ToString();
            TB_Feels.Text = main.feels_like.ToString();
        }

        private void ComboBoxCities_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string selectedCity = ComboBoxCities.SelectedItem.ToString();


            string city = selectedCity;

            _: GetWeather(city);
        }
    }
}
