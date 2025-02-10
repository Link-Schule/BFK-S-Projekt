using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaxTemp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static async Task Main(string selectedSensor, String tmpName)
        {
            // URL der CSV-Datei auf GitHub
            string fileUrl = "https://raw.githubusercontent.com/Link-Schule/BFK-S-Projekt/main/temps.csv";

            string csvContent;

            // CSV-Datei von der URL herunterladen
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    csvContent = await client.GetStringAsync(fileUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Herunterladen der Datei: " + ex.Message);
                return;
            }

            // Liste für die Sensordaten
            var sensorData = new List<(string Sensor, DateTime Zeit, double Temperatur)>();

            // CSV-Inhalt in strukturierte Daten umwandeln
            foreach (var line in csvContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split(',');
                if (parts.Length == 3)
                {
                    try
                    {
                        string sensor = parts[0];
                        DateTime zeit = DateTime.ParseExact(parts[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        double temperatur = double.Parse(parts[2], CultureInfo.InvariantCulture);

                        sensorData.Add((sensor, zeit, temperatur));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fehler beim Verarbeiten der Zeile: {line} /n Details: {ex.Message}");
                    }
                }
            }

            // Verfügbare Sensoren anzeigen
            var sensorNames = sensorData.Select(d => d.Sensor).Distinct().OrderBy(s => s).ToList();
            string ausgabe = "Verfügbare Sensoren:";
            foreach (var sensor in sensorNames)
            {
                ausgabe += $" {sensor}";
            }
            MessageBox.Show(ausgabe);

            if (selectedSensor.Equals("Alle", StringComparison.OrdinalIgnoreCase))
            {
                // Alle Sensoren auswerten
                Console.WriteLine("\nAuswertung aller Sensoren:");
                string resultat = $"{tmpName}:";

                foreach (var sensor in sensorNames)
                {
                    var filteredData = sensorData
                        .Where(d => d.Sensor.Equals(sensor, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(d => d.Zeit)
                        .ToList();

                    if (filteredData.Count > 0)
                    {
                        if ("Maximale Temperatur".Equals(tmpName))
                        {
                            var maxTemp = filteredData.Max(d => d.Temperatur);
                            resultat += $" {maxTemp} °C von {sensor},";
                        }
                        else if ("Minimale Temperatur".Equals(tmpName))
                        {
                            var minTemp = filteredData.Min(d => d.Temperatur);
                            resultat += $" {minTemp} °C von {sensor},";
                        }
                        else
                        {
                            var avgTemp = filteredData.Average(d => d.Temperatur);
                            resultat += $" {avgTemp} °C von {sensor},";
                        }
                    }
                }
                MessageBox.Show(resultat);
            }
            else
            {
                // Einzelnen Sensor auswerten
                var filteredData = sensorData
                    .Where(d => d.Sensor.Equals(selectedSensor, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(d => d.Zeit)
                    .ToList();
                string resultat = $"{tmpName}:";

                if (filteredData.Count == 0)
                {
                    MessageBox.Show($"Keine Daten für den Sensor '{selectedSensor}' gefunden.");
                    return;
                }

                foreach (var sensor in sensorNames)
                {
                    var filteredDataBySensor = sensorData
                        .Where(d => d.Sensor.Equals(selectedSensor))
                        .OrderBy(d => d.Zeit)
                        .ToList();

                    if (filteredData.Count > 0)
                    {
                        if ("Maximale Temperatur".Equals(tmpName))
                        {
                            var maxTemp = filteredDataBySensor.Max(d => d.Temperatur);
                            resultat += $" {maxTemp} °C von {selectedSensor},";
                        }
                        else if ("Minimale Temperatur".Equals(tmpName))
                        {
                            var minTemp = filteredDataBySensor.Min(d => d.Temperatur);
                            resultat += $" {minTemp} °C von {selectedSensor},";
                        }
                        else
                        {
                            var avgTemp = filteredDataBySensor.Average(d => d.Temperatur);
                            resultat += $" {avgTemp} °C von {selectedSensor},";
                        }
                    }
                }
                MessageBox.Show(resultat);
            }
        }


        /// <summary>
        /// Diese Routine (EventHandler des Buttons Auswerten) liest die Werte
        /// zeilenweise aus der Datei temps.csv aus, merkt sich den höchsten Wert
        /// und gibt diesen auf der Oberfläche aus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            //Zugriff auf Datei erstellen.
            string[] tmp = sender.ToString().Split(':');
            //Anfangswert setzen, um sinnvoll vergleichen zu können.
            string name = sensor.Text;
            Task task = Main(name, tmp[1].Trim());

            //In einer Schleife die Werte holen und auswerten. Den größten Wert "merken".


            //Datei wieder freigeben.

            //Höchstwert auf Oberfläche ausgeben.

            //kommentieren Sie die Exception aus.
            //throw new Exception("peng");
        }
    }
}