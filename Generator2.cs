using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Generator
    {
        private static readonly Random rnd = new Random();

        public static string AddEmployee(int year, int month)
        {
            string[] names = { "Jacob" ,"Michael" ,"Joshua", "Matthew","Ethan", "Andrew", "Daniel","Anthony","Christopher" ,"Joseph","William",
                           "Alexander","Ryan","David","Nicholas","Tyler","James","John","Jonathan","Nathan","Samuel","Christian","Noah","Dylan",
                           "Benjamin","Logan","Brandon","Gabriel","Zachary","Jose","Elijah","Angel","Kevin","Jack","Caleb","Justin","Austin",
                           "Evan","Robert","Thomas","Luke","Mason","Aidan","Jackson","Isaiah","Jordan","Gavin","Connor","Aiden","Isaac"};
            string[] last_names = {"Smith","Johnson","Williams","Jones","Brown","Davis","Miller","Wilson","Moore","Taylor","Anderson",
                                   "Thomas","Jackson","White","Harris","Martin","Thompson","Garcia","Martinez","Robinson","Clark","Rodriguez",
                                   "Lewis","Lee","Walker","Hall","Allen","Young","Hernandez","King","Wright","Lopez","Hill","Scott","Green",
                                   "Adams","Baker","Gonzalez","Nelson","Carter","Mitchell","Perez","Roberts","Turner","Phillips","Campbell",
                                   "Parker","Evans","Edwards","Collins"};

            int gpi = rnd.Next(1, 13);
            int name = rnd.Next(50);
            int last_name = rnd.Next(50);

            return gpi.ToString() + "," + names[name] + " " + last_names[last_name] + "," + year.ToString() + "," + month.ToString() + "," + ",";
        }

        public static void GenerateEmployee()
        {
            int num_of_engineers = 10;
            StringBuilder csvcontent = new StringBuilder();

            for (int i = 1; i <= num_of_engineers; i++)
            {
                csvcontent.AppendLine(i.ToString() + "," + AddEmployee(1, 1));

            }

            string csvpath = @"C:\Users\Karol\Desktop\rty.csv";
            File.AppendAllText(csvpath, csvcontent.ToString());
        }

        static void Main(string[] args)
        {
            GenerateEmployee();
        }
    }
}
