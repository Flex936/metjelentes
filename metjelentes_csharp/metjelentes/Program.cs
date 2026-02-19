namespace metjelentes
{
    class Weather
    {
        public string Settlement { get; set; }
        public TimeSpan Time { get; set; }
        public string WindDirection { get; set; }
        public string WindSpeed { get; set; }
        public int Temperature { get; set; }

        public Weather() {
            Settlement = "";
            Time = TimeSpan.Zero;
            WindDirection = "";
            WindSpeed = "";
            Temperature = 0;
        }

        public Weather(string line)
        {
            string[] data = line.Split(' ');

            Settlement = data[0];

            Time = TimeSpan.Parse(data[1].Insert(2, ":"));

            string wind = data[2];
            WindDirection = wind.Substring(0, 3);
            WindSpeed = wind.Substring(3);

            Temperature = int.Parse(data[3]);
        }

        public string FormatTime()
        {
            return Time.ToString(@"hh\:mm");
        }
    }

    internal class Program
    {
        public static void Debug(List<Weather> weathers)
        {
            foreach (Weather weather in weathers)
                Console.WriteLine($"{weather.Settlement} {weather.FormatTime()} {weather.WindDirection} {weather.WindSpeed} {weather.Temperature}");
        }
        public static void PrintSeperator()
        {
            Console.WriteLine(new string('-', 20));
        }

        static void Main()
        {
            string[] file = File.ReadAllLines("tavirathu13.txt");
            List<Weather> weathers = new List<Weather>();
            foreach (string line in file)
                weathers.Add(new Weather(line));

            // 2. feladat
            Console.WriteLine("2. feladat");
            Console.Write("Adja meg egy település kódját! Település: ");
            string settlement = Console.ReadLine() ?? "";
            Weather? lastWeather = weathers.LastOrDefault(w => w.Settlement == settlement);
            if (lastWeather != null)
                Console.WriteLine($"Az utolsó mérési adat időpontja: {lastWeather.FormatTime()}");
            else
                Console.WriteLine("Nincs ilyen település!");

            // 3. feladat
            PrintSeperator();
            Console.WriteLine("3. feladat");
            Weather minTemp = weathers.MinBy(w => w.Temperature);
            Weather maxTemp = weathers.MaxBy(w => w.Temperature);
            Console.WriteLine($"Legalacsonyabb hőmérséklet: {minTemp.Settlement} {minTemp.FormatTime()} {minTemp.Temperature} fok");
            Console.WriteLine($"Legmagasabb hőmérséklet: {maxTemp.Settlement} {maxTemp.FormatTime()} {maxTemp.Temperature} fok");

            // 4. feladat
            PrintSeperator();
            Console.WriteLine("4. feladat");
            List<Weather> noWind = weathers.Where(w => w.WindSpeed == "00").ToList();
            if (noWind.Count > 0)
            {
                Console.WriteLine("Szélcsendes időszakok:");
                foreach (Weather weather in noWind)
                    Console.WriteLine($"{weather.Settlement} {weather.FormatTime()}");
            }
            else
                Console.WriteLine("Nem volt szélcsendes időszak!");

            // 5. feladat
            PrintSeperator();
            Console.WriteLine("5. feladat");
            var settlementGroups = weathers.GroupBy(w => w.Settlement);
            foreach (var group in settlementGroups)
            {
                string settlementName = group.Key;
                var targetHoursData = group.Where(w => new[] { 1, 7, 13, 19 }.Contains(w.Time.Hours)).ToList();
                var distinctHoursFound = targetHoursData.Select(w => w.Time.Hours).Distinct().Count();
                string averageDisplay;
                if (distinctHoursFound == 4)
                {
                    double averageTemp = targetHoursData.Average(w => w.Temperature);
                    averageDisplay = $"Középhőmérséklet: {averageTemp:F0}";
                }
                else
                    averageDisplay = "Középhőmérséklet: N/A";

                int tempFluctuation = group.Max(w => w.Temperature) - group.Min(w => w.Temperature);

                Console.WriteLine($"{settlementName} {averageDisplay}; Hőmérséklet-ingadozás: {tempFluctuation}");
            }

            // 6. feladat
            PrintSeperator();
            Console.WriteLine("6. feladat");
            foreach (var group in settlementGroups)
            {
                string settlementName = group.Key;
                string fileName = $"{settlementName}.txt";

                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine(settlementName);

                    foreach (var report in group)
                    {
                        string windGraph = new string('#', int.Parse(report.WindSpeed));
                        writer.WriteLine($"{report.FormatTime()} {windGraph}");
                    }
                }
            }
            Console.WriteLine("Fájlok sikeresen létrehozva.");

            Console.ReadLine();
        }
    }
}
