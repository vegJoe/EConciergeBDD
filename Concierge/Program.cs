using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.IO;

namespace Concierge
{
    class Program
    {
        static void Main(string[] args)
        {
            //WebClient client = new WebClient();
            //string downloadString = client.DownloadString("http://localhost:777/api/v1/777/1/getorderitem/8");
            //List<string> content = downloadString.Split(',', '{', '}').ToList();

            HttpWebRequest request = WebRequest.Create("http://localhost:777/api/v1/777/1/addorderitem/") as HttpWebRequest;
            request.ContentType = "application/json";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            Encoding enc = new UTF8Encoding();

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream(), enc))
                {
                    string addOrderItem = "{\"hotel_ID\": \"1\"," +
                        "\"room_ID\": \"1\"," +
                        "\"category_ID\": \"2\"," +
                        "\"item_ID\": \"8\"," +
                        "\"item_name_en\": \"Coca Cola\"," +
                        "\"item_desc_en\": \"Coca Cola 33 cl\"," +
                        "\"price\": \"1.25\"}";
                    //string addOrderItem = "{\n \"data\": [\n {\n \"item_ID\": \"8\",\n \"service_ID\": \"1\",\n \"category_ID\": \"2\",\n \"sla\": \"30\",\n \"item_name_en\": \"Coca Cola\",\n \"item_name_de\": null,\n \"item_name_sv\": null,\n \"item_name_ar\": null,\n\"item_desc_en\": \"Coca Cola 33 cl\",\n \"item_desc_de\": null,\n \"item_desc_sv\": null,\n \"item_desc_ar\": null,\n \"price\": \"1.25\",\n \"currency\": \"AED\",\n \"hotel_ID\": \"1\",\n \"enabled\": \"1\",\n \"icon\": null,\n       \"name_en\": \"Drinks\",\n \"name_de\": \"\",\n \"name_sv\": \"\",\n \"name_ar\": null,\n \"parent_id\": \"0\",\n \"javascript\": null\n }";

                    streamWriter.Write(addOrderItem);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            /*            
           HttpWebRequest request = WebRequest.Create("http://localhost:777/api/v1/777/1/addorderitem/") as HttpWebRequest;
           //request.ContentType = "application/json";
           request.ContentType = "application/x-www-form-urlencoded";
           request.Method = "POST";
           Encoding enc = new UTF8Encoding(false);

           try
           {
               using (var streamWriter = new StreamWriter(request.GetRequestStream(), enc))
               {
                   string addOrderItem = "{\"category_ID\":\"2\"," +
                       "item_name\":\"Coca Cola\"," +
                       "description\":\"Coca Cola 33 cl\"," +
                       "price\":\"1.25\"}";

                   streamWriter.Write(addOrderItem);
                   streamWriter.Flush();
                   streamWriter.Close();
               }

               HttpWebResponse response = request.GetResponse() as HttpWebResponse;
               response.Close();
           }
           catch (Exception)
           {
               throw;
           }


          try
                       {
                           int roomNumber;
                           var myConnectionString2 = "Server = 127.0.0.1; Port = 3306; Database = hotel1; Uid = hotel; pwd = hotel ; database = hotel1";
                           var conn = new MySqlConnection(myConnectionString2);
                           conn.Open();

                           Console.WriteLine("Successfully connected to MariaDB");
                           MySqlCommand cmd = new MySqlCommand();
                           cmd.CommandText = "app_guest_order";
                           cmd.Connection = conn;
                           cmd.CommandType = System.Data.CommandType.TableDirect;
                           MySqlDataReader reader = cmd.ExecuteReader();

                           while (reader.Read() && conn.State == System.Data.ConnectionState.Open)
                           {
                               if (reader[1] != null)
                               {
                                   if (reader[2] != null)
                                   {
                                       roomNumber = Int32.Parse(reader[2].ToString());
                                       reader.Close();
                                       cmd.CommandText = "DELETE FROM app_guest_order WHERE room_id = @id";
                                       cmd.Parameters.AddWithValue("@id", roomNumber);
                                       cmd.CommandType = System.Data.CommandType.Text;
                                       cmd.ExecuteNonQuery();
                                       conn.Close();
                                   }
                               }
                           }
                           if (conn.State == System.Data.ConnectionState.Open)
                               conn.Close();
                       }
                       catch (MySqlException ex)
                       {
                           Console.WriteLine(ex.Message);
                           Console.ReadLine();
                       }
                       */
            Console.ReadLine();
        }
    }
}


/*
            var myConnectionString = "server=127.0.0.1;uid=root;" + "Integrated Security = true;database=hotell;";
            var myConnectionString2 = "Server = 127.0.0.1; Port = 3306; Database = hotel1; Uid = hotel; pwd = hotel ; database = hotel1";
            var conn = new MySqlConnection(myConnectionString2);
            conn.Open();

            WebClient client = new WebClient();

            //HttpWebRequest request = WebRequest.Create("http://localhost:777/?rid=1&hid=1&md5=777") as HttpWebRequest;
            //HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            string downloadString = client.DownloadString("http://localhost:777/api/v1/777/1/getorderitems");
            List<string> content = downloadString.Split(',', '{', '}').ToList();
            List<string> newContent = new List<string>();
            bool sameProductFound = false;
            //response.Close();

            foreach (var item in content)
            {
                if (item.Contains("item_ID"))
                {
                    newContent.Add(item);
                }
            }

            for(int i = 0; i < newContent.Count; i++)
            {
                for (int j = 0; j < newContent.Count; j++)
                {
                    if (newContent[i] == newContent[j] && !sameProductFound)
                    {
                        sameProductFound = true;
                        break;
                    }
                }
                if (sameProductFound)
                    break;
            }

            //Console.Write(numOfProducs.ToString());
            Console.ReadKey();*/
