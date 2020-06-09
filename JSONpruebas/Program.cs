using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONpruebas
{
    class Program
    {

        public class Fechas
        {
            public string fecha { get; set; }
            public string etapa { get; set; }
        }
        public class Prueba
        {
            public string auditoria { get; set; }
            public List<Fechas> fechas { get; set; }
        }
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DB_ASEBCv1Context"].ConnectionString;
            List<Prueba> _pruebas = new List<Prueba>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Tbl_ProgramaAuditoria a WHERE a.AuditoriaUI = '2016_1_28_1'", sqlConnection);
                SqlDataReader reader = sqlCmd.ExecuteReader();


                
                while (reader.Read())
                {
                    int j = 2;
                    int k = 3;
                    Prueba prueba = new Prueba();
                    prueba.auditoria = reader.GetString(26).ToString();
                    List<Fechas> _fe = new List<Fechas>();
                    for (int i = 0; i < 12; i++)
                    {
                        Fechas fechas = new Fechas();
                        fechas.etapa = reader.GetInt32(j).ToString();
                        fechas.fecha = reader.GetDateTime(k).ToString();
                        j = j + 2;
                        k = k + 2;
                        _fe.Add(fechas);
                        
                    }
                    prueba.fechas = _fe;
                    _pruebas.Add(prueba);
                }
                sqlConnection.Close();
            }
            string json = JsonConvert.SerializeObject(_pruebas, Formatting.Indented);
            Console.WriteLine(json.ToString());
            // [
            //   {
            //     "Title": "Json.NET is awesome!",
            //     "Author": {
            //       "Name": "James Newton-King",
            //       "Twitter": "JamesNK"
            //     },
            //     "Date": "2013-01-23T19:30:00",
            //     "BodyHtml": "&lt;h3&gt;Title!&lt;/h3&gt;&lt;p&gt;Content!&lt;/p&gt;"
            //   }
            // ]
        }
    }
}