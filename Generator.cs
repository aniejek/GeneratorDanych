using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorDanych
{
    class Generator
    {
        SqlConnection sqlConnection;

        int initialEngineersNumber;
        int endingEngineersNumber;
        int engineersFired;

        int partsNumber;

        int hangarsNumber;

        int spacecraftsNumber;
        int spacecraftsYearsFrom;
        int spacecraftsYearsTo;

        int refuelingsNumber;
        int minRefuelingLiters;
        int maxRefuelingLiters;
        int minWaitingTime;
        int maxWaitingTime;
        int minRefuelingTime;
        int maxRefuelingTime;

        int fixesNumber;
        int minFixCost;
        int maxFixCost;
        int minFixTime;
        int maxFixTime;
        int minFixPrice;
        int maxFixPrice;

        DateTime startDate;
        DateTime endDate;

        private string DateToString(DateTime date)
        {
            return String.Format("{0:yyyy-MM-dd}", date);
        }

        public Generator(SqlConnection sqlConnection, int elo=1)
        {
            this.sqlConnection = sqlConnection;
            this.sqlConnection.Open();
        }
        public void Generate()
        {
            var dateTime = new DateTime(2018, 11, 10);
            Console.WriteLine(this.DateToString(dateTime));
            var newDate = dateTime.AddDays(100);
            Console.WriteLine(this.DateToString(newDate));
        }
        public void CloseGenerator()
        {
            this.sqlConnection.Close();
        }
        public void TestGeneration()
        {
            this.SavePart(1, "jakis", "jakas");
            this.SaveHangar("blu");
            this.SaveSpacecraft(1, "jakis", 1990);
            this.SaveRefueling(1, 1, 1, "ropa", 1, 1);
            this.SaveFix(1, 1, 1, 1, 1, 1, "2018-10-10", 1, 1, "blu");
        }
        private void SaveRefueling(int id, int litres, int spacecraftId, string fuelType, int refuelingTime, int waitingTime)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Tankowania(id, litry, id_statku, rodzaj_paliwa, czas_tankowania, czas_oczekiwania) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                id, litres, spacecraftId, fuelType, refuelingTime, waitingTime);
            cmd.ExecuteNonQuery();
        }
        private void SavePart(int id, string producer, string type)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Czesci(id, producent, typ) values('{0}', '{1}', '{2}');",
                id, producer, type);
            cmd.ExecuteNonQuery();
        }
        private void SaveHangar(string colour)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Hangary values('{0}');",
                colour);
            cmd.ExecuteNonQuery();
        }
        private void SaveSpacecraft(int id, string model, int year)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Statki(id, model, rocznik) values('{0}', '{1}', '{2}');",
                id, model, year);
            cmd.ExecuteNonQuery();
        }
        private void SaveFix(int id, int cost, int time, int price, int engineerId, int kliB, string date, int spacecraftId, int partId, string hangarColour)
        {
            var cmd = this.sqlConnection.CreateCommand();
            cmd.CommandText = String.Format("insert into Naprawy(id, koszt, czas, cena, id_inzyniera, numer_platnosci_kliB, data, id_statku, id_czesci, kolor_hangaru) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');",
                id, cost, time, price, engineerId, kliB, date, spacecraftId, partId, hangarColour);
            cmd.ExecuteNonQuery();
        }
    }
}
