using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorDanych
{
    class Generator
    {
        SqlConnection sqlConnection;
        Dictionary<string, Tuple<string, string>[]> models = new Dictionary<string, Tuple<string, string>[]>() {
            { "idua", new Tuple<string, string>[]{ new Tuple<string, string>("A3-2", "krazawnik"), Tuple.Create("A6-012", "statek turystyczny") } },
            { "wmb", new Tuple<string, string>[]{ new Tuple<string, string>("K9-A46", "krazawnik"), new Tuple<string, string>("K2-931", "statek turystyczny"), new Tuple<string, string>("K20-V53", "transporter") } },
            { "lepo", new Tuple<string, string>[]{ new Tuple<string, string>("APOLLO-G20", "transporter"), new Tuple<string, string>("ZEUS-C", "statek turystyczny") } },
            { "Oowead", new Tuple<string, string>[]{ new Tuple<string, string>("Spaghetti-7" ,"krazawnik")} },
            { "sedecrem", new Tuple<string, string>[]{ new Tuple<string, string>("FALCON-42", "transporter"), new Tuple<string, string>("FALCON-533", "transporter") } },
        };
        List<string> colors = new List<string>() { "green", "red", "yellow", "blue", "pink", "white", "black", "purple", "orange", "lightblue", "darkblue", "gray" };
        int initialEngineersNumber=20;
        int t2EngineersNumber=10;
        int engineersFired=5;

        int partsNumber=100;

        int hangarsNumber=12;

        int spacecraftsNumber=100;
        int spacecraftsYearsFrom=1990;
        int spacecraftsYearsTo=1999;

        int refuelingsNumber=1000;
        int minRefuelingLiters=10;
        int maxRefuelingLiters=30;
        int minWaitingTime=0;
        int maxWaitingTime=10;
        int minRefuelingTime=1;
        int maxRefuelingTime=10;

        int fixesNumber=1000;
        int minFixCost=10;
        int maxFixCost=30;
        int minFixTime=3;
        int maxFixTime=30;
        int minFixPrice=20;
        int maxFixPrice=60;

        int kliB;
        int engineers;
        string csvName = "inzynierowie.csv";

        DateTime startDate = new DateTime(2012, 4, 10);
        DateTime endDate = new DateTime(2012, 10, 10);

        Random random;

        private string RandomString(int minLength, int maxLength)
        {
            string ret = "";
            int length = this.random.Next(minLength, maxLength);
            for (int i = 0; i < length; i++)
            {
                ret += (char)('a' + random.Next(26));
            }
            return ret;
        }

        private string DateToString(DateTime date)
        {
            return String.Format("{0:yyyy-MM-dd}", date);
        }

        public Generator(SqlConnection sqlConnection, int elo=1)
        {
            this.sqlConnection = sqlConnection;
            this.sqlConnection.Open();
            this.random = new Random();
            this.kliB = NextKliB();
        }
        public void Generate()
        {
            if (!File.Exists(csvName))
            {
                File.WriteAllText(csvName, "Id,GPI,Nazwisko,Id mistrza,Liczba firm,Rok przyjęcia,Miesiąc przyjęcia,Rok zwolnienia,Miesiąc zwolnienia");
            }
            var lines = File.ReadAllLines(csvName);
            engineers = lines.Length - 1;
            DateTime currentDate = startDate;
            for (int i = 0; i < this.partsNumber; i++)
            {
                this.AddRandomPart();
            }
            this.partsNumber = 0;
            Console.WriteLine("Dodano rekordy części.");
            for (int i = 0; i < this.hangarsNumber; i++)
            {
                this.AddRandomHangar();
            }
            this.hangarsNumber = 0;
            Console.WriteLine("Dodano rekordy hangarów.");
            for (int i = 0; i < this.spacecraftsNumber; i++)
            {
                this.AddRandomSpacecraft();
            }
            this.spacecraftsNumber = 0;
            Console.WriteLine("Dodano rekordy satków kosmicznych.");
            for (int i = 0; i < this.initialEngineersNumber; i++)
            {
                this.AddRandomEngineer(currentDate);
            }
            initialEngineersNumber = 0;
            Console.WriteLine("Dodano wstępne rekordy inżynierów.");
            int events = this.refuelingsNumber + this.fixesNumber + this.engineersFired + this.t2EngineersNumber;
            int maxEvents = events;
            int maxDays = this.endDate.Subtract(this.startDate).Days;
            int days = maxDays - 1;
            int percent = 0;
            while (events > 0)
            {
                if ((maxEvents - events) * 100.0 / maxEvents > percent)
                {
                    percent += 1;
                    Console.WriteLine("{0}% done", percent);
                }
                int action = random.Next(events);
                if ((events * 1.0 / maxEvents) < (days * 1.0 / maxDays))
                {
                    days -= 1;
                    currentDate = currentDate.AddDays(1);
                }
                events -= 1;
                if (action < refuelingsNumber)
                {
                    AddRandomRefueling();
                    refuelingsNumber -= 1;
                    continue;
                }
                else
                {
                    action -= refuelingsNumber;
                }
                if (action < fixesNumber)
                {
                    AddRandomFix(currentDate);
                    fixesNumber -= 1;
                    continue;
                }
                else
                {
                    action -= fixesNumber;
                }
                if (action < engineersFired)
                {
                    FireEngineer(currentDate);
                    engineersFired -= 1;
                    continue;
                }
                else
                {
                    action -= engineersFired;
                }
                if (action < t2EngineersNumber)
                {
                    AddRandomEngineer(currentDate);
                    t2EngineersNumber -= 1;
                    continue;
                }
                else
                {
                    action -= t2EngineersNumber;
                }
            }
        }
        public void CloseGenerator()
        {
            this.sqlConnection.Close();
        }
        public void TestGeneration()
        {
            int max = 0;
            int min = 10;
            for (int i = 0; i < 10000; i++)
            {
                int curr = random.Next(10);
                if (curr > max)
                {
                    max = curr;
                }
                if (curr < min)
                {
                    min = curr;
                }
            }
            Console.WriteLine(max);
            Console.WriteLine(min);
            int subtraction = this.endDate.Subtract(this.startDate).Days;
            Console.WriteLine(subtraction);
            Console.WriteLine((int)(this.endDate.Subtract(this.startDate).Days));
        }
        private void AddRandomPart()
        {
            this.SavePart(this.RandomString(5, 15), this.RandomString(10, 20));
        }
        private void AddRandomHangar()
        {
            this.SaveHangar(this.RandomString(5, 10));
        }
        private void AddRandomSpacecraft()
        {
            this.SaveSpacecraft(this.random.Next(this.spacecraftsYearsFrom, this.spacecraftsYearsTo));
        }
        private void AddRandomModel()
        {

        }
        private void AddRandomEngineer(DateTime engageDate)
        {
            int id = engineers;
            
            var csvFile = File.ReadAllLines(csvName);
            int gpi = random.Next(0, 100000);
            bool gpiNotUnique = false;
            foreach (var engineer in csvFile)
            {
                if (engineer.Split(',')[1] == String.Format("{0}", gpi))
                {
                    gpiNotUnique = true;
                    break;
                }
            }
            while (gpiNotUnique)
            {
                gpiNotUnique = false;
                gpi = random.Next(0, 100000);
                foreach (var engineer in csvFile)
                {
                    if (engineer.Split(',')[1] == String.Format("{0}", gpi))
                    {
                        gpiNotUnique = true;
                        break;
                    }
                }
            }
            string master = "";
            if (random.NextDouble() < 1f / 3f)
            {
                master = String.Format("{0}", GetRandomMaster());
            }
            string name = RandomString(5, 15);
            string line = '\n' + String.Format("{0},{1},{2},{3},{4:yyyy,MM}",id, gpi, name, master, engageDate);
            File.AppendAllText(csvName, line);
            engineers += 1;
        }
        private void AddRandomRefueling()
        {
            int spacecraftId = GetRandomSpacecraft();
            SaveRefueling(random.Next(minRefuelingLiters, maxRefuelingLiters), spacecraftId, RandomString(10, 20),
                random.Next(minRefuelingTime, maxRefuelingTime), random.Next(minWaitingTime, maxWaitingTime));
        }
        private void AddRandomFix(DateTime currentDate)
        {
            int spacecraftId = GetRandomSpacecraft();
            int partId = GetRandomPart();
            int engineerId = GetRandomEngineer();
            string hangarColour = GetRandomHangar();
            int modelId = GetRandomModel();
            int serviceId = GetRandomService();
            SaveFix(random.Next(minFixCost, maxFixCost), random.Next(minFixTime, maxFixTime), random.Next(minFixPrice, maxFixPrice),
                engineerId, kliB, DateToString(currentDate), spacecraftId, partId, hangarColour, modelId, serviceId);
            this.kliB += 1;
        }
        private void FireEngineer(DateTime fireDate)
        {
            var csvFile = File.ReadAllLines(csvName);
            int engineer = GetRandomEngineer();
            var engineerLine = csvFile[engineer];
            if (engineerLine[engineerLine.Length - 1] == '\n')
            {
                engineerLine = engineerLine.Remove(engineerLine.Length - 1);
            }
            engineerLine += String.Format(",{0:yyyy,MM}", fireDate);
            csvFile[engineer] = engineerLine;
            File.WriteAllText(csvName, String.Join("\n", csvFile));
        }
        private int NextKliB()
        {
            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "select count(*) from naprawy";
            return (int)cmd.ExecuteScalar();
        }
        private string GetRandomRow(string column, string table)
        {
            return String.Format("SELECT TOP 1 {0} FROM {1} ORDER BY NEWID()", column, table);
        }
        private int GetRandomSpacecraft()
        {
            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = GetRandomRow("id", "statki");
            return (int)cmd.ExecuteScalar();
        }
        private int GetRandomPart()
        {
            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = GetRandomRow("id", "czesci");
            return (int)cmd.ExecuteScalar();
        }
        private string GetRandomHangar()
        {
            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = GetRandomRow("kolor", "hangary");
            return (string)cmd.ExecuteScalar();
        }
        private int GetRandomModel()
        {
            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = GetRandomRow("id", "modele");
            return (int)cmd.ExecuteScalar();
        }
        private int GetRandomService()
        {
            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = GetRandomRow("id", "serwisy");
            return (int)cmd.ExecuteScalar();
        }
        private int GetRandomEngineer()
        {
            var csvFile = File.ReadAllLines(csvName);
            int engineer = random.Next(1, engineers);
            var engineerLine = csvFile[engineer];
            while (engineerLine.Split(',').Length > 7)
            {
                engineer = random.Next(1, engineers);
                engineerLine = csvFile[engineer];
            }
            return engineer;
        }
        private int GetRandomMaster()
        {
            var csvFile = File.ReadAllLines(csvName);
            int master = GetRandomEngineer();
            var masterLine = csvFile[master];
            while (masterLine.Split(',')[3] != "")
            {
                master = GetRandomEngineer();
                masterLine = csvFile[master];
            }
            return master;
        }
        private void SaveRefueling(int litres, int spacecraftId, string fuelType, int refuelingTime, int waitingTime)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Tankowania(litry, id_statku, rodzaj_paliwa, czas_tankowania, czas_oczekiwania) values('{0}', '{1}', '{2}', '{3}', '{4}');",
                litres, spacecraftId, fuelType, refuelingTime, waitingTime);
            cmd.ExecuteNonQuery();
        }
        private void SavePart(string producer, string type)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Czesci(producent, typ) values('{0}', '{1}');",
                producer, type);
            cmd.ExecuteNonQuery();
        }
        private void SaveHangar(string colour)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Hangary values('{0}');",
                colour);
            cmd.ExecuteNonQuery();
        }
        private void SaveSpacecraft(int year)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Statki(rocznik) values('{0}');",
                year);
            cmd.ExecuteNonQuery();
        }
        private void SaveFix(int cost, int time, int price, int engineerId, int kliB, string date, int spacecraftId, int partId, string hangarColour, int modelId, int serviceId)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Naprawy(koszt, czas, cena, id_inzyniera, numer_platnosci_kliB, data, id_statku, id_czesci, kolor_hangaru, id_modelu, id_serwisu) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');",
                cost, time, price, engineerId, kliB, date, spacecraftId, partId, hangarColour, modelId, serviceId);
            cmd.ExecuteNonQuery();
        }
        private void SaveModel(string name, string brand, string type)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Modele(nazwa, marka, rodzaj) values ('{0}', '{1}', {2}');",
                name, brand, type);
            cmd.ExecuteNonQuery();
        }
        private void SaveService(int wrenches, int drills, int screwdrivers, int grease, int screws)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Serwisy(klucze, wiertarki, srubokrety, smary, sruby values ('{0}', '{1}', '{2}', '{3}', '{4}');",
                wrenches, drills, screwdrivers, grease, screws);
            cmd.ExecuteNonQuery();
        }
    }
}
