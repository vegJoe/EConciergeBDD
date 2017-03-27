using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MySql.Data.MySqlClient;

namespace ConciergeAcceptensTest
{
    [Binding]
    public sealed class ViewMyOrdersStepDefinition
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef
        private int roomID;
        private int hotelID;

        [Given(@"eConcierge backend is up and running")]
        public void GivenEConciergeBackendIsUpAndRunning()
        {
            HttpWebRequest request = WebRequest.Create("http://localhost:777/?rid=1&hid=1&md5=777") as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            response.Close();
            Assert.IsNotNull(response, "Could find http://localhost:777/?rid=1&hid=1&md5=777");
        }

        [Given(@"There exists atleast (.*) different products")]
        public void GivenThereExistsAtleastDifferentProducts(int minAmmountOfProducts = 1)
        {
            List<int> numOfProducts = new List<int>();

            try
            {
                var myConnectionString = "Server = 127.0.0.1; Port = 3306; Database = hotel1; Uid = hotel; pwd = hotel ; database = hotel1";
                var conn = new MySqlConnection(myConnectionString);
                conn.Open();

                Console.WriteLine("Successfully connected to MariaDB");
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "app_service_item_order";
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.TableDirect;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    numOfProducts.Add(Int32.Parse(reader[0].ToString()));
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            if (numOfProducts.Count < minAmmountOfProducts)
                Assert.Fail("Not enough products found");
        }

        [Given(@"The orderlist is clear")]
        public void GivenTheOrderlistIsClear()
        {
            try
            {
                var myConnectionString = "Server = 127.0.0.1; Port = 3306; Database = hotel1; Uid = hotel; pwd = hotel ; database = hotel1";
                var conn = new MySqlConnection(myConnectionString);
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
                            roomID = Int32.Parse(reader[2].ToString());
                            reader.Close();
                            cmd.CommandText = "DELETE FROM app_guest_order WHERE room_id = @id";
                            cmd.Parameters.AddWithValue("@id", roomID);
                            cmd.CommandType = System.Data.CommandType.Text;
                            int queryRetVal = cmd.ExecuteNonQuery();

                            if(queryRetVal != -1)
                            {
                                cmd.CommandText = "app_guest_order";
                                reader = cmd.ExecuteReader();

                                while (reader.Read() && conn.State == System.Data.ConnectionState.Open)
                                {
                                    Assert.IsNotNull(reader[roomID]);
                                }
                            }

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
        }

        [When(@"Hotel guest orders (.*) product with product id (.*)")]
        public void WhenHotelGuestOrdersProductWithProductId(int numOfProducts = 1, int productID = 8)
        {
            MySqlConnection mySqlConn = new MySqlConnection();
            MySqlCommand command = new MySqlCommand();
            try
            {
                mySqlConn.ConnectionString = "Server = localhost; Database = hotel1; Uid = hotel; Pwd = hotel;";
                mySqlConn.Open();
                command = mySqlConn.CreateCommand();
            }
            catch (Exception connectionException)
            {
                Assert.Fail(connectionException.Message);
            }
/*
            try
            {
                command.CommandText = "INSERT INTO `app_guest_order` (`hotel_ID`, `room_ID`, `item_ID`, `quantity`, `cost`, `currency`) " +
                                        "VALUES(1, 1, 8, 1, 1, 'AED')";
                //" VALUES(@hotel_ID, @room_ID, @item_ID, @quantity, @cost, @currency)";
                //command.Parameters.AddWithValue("?hotel_ID", MySqlDbType.Int64).Value = hotelID;
                //command.Parameters.AddWithValue("?room_ID", MySqlDbType.Int32).Value = roomID;
                //command.Parameters.AddWithValue("?item_ID", MySqlDbType.Int32).Value = productID;
                //command.Parameters.AddWithValue("?quantity", MySqlDbType.Int16).Value = numOfProducts;
                //command.Parameters.AddWithValue("?cost", MySqlDbType.Int16).Value = 2;
                //command.Parameters.AddWithValue("?currency", MySqlDbType.VarChar).Value = "'AED'";
                command.ExecuteNonQuery();
                mySqlConn.Close();
            }
            catch (Exception commandException)
            {
                Assert.Fail(commandException.Message);
            }
            */


            HttpWebRequest request = WebRequest.Create("http://localhost:777/api/v1/777/1/addorderitem/") as HttpWebRequest;
            //request.ContentType = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            string addOrderItem = "{\"category_ID\": \"2\"," +
                "item_name\": \"Coca Cola\"," +
                "description\": \"Coca Cola 33 cl\"," +
                "price\": \"1.25\"}";
            byte[] postBytes = Encoding.UTF8.GetBytes(addOrderItem);
            request.ContentLength = postBytes.Length;

            Stream dataStream = request.GetRequestStream();

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(postBytes, 0, postBytes.Length);
                dataStream.Close();

                //streamWriter.Write(addOrderItem);
                //streamWriter.Flush();
                //streamWriter.Close();
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            response.Close();

            Assert.IsNotNull(response);

        }


        [Then(@"Then (.*) line with (.*) item should be displayed")]
        public void ThenThenLineWithItemShouldBeDisplayed(int p0, int p1 = 1)
        {
            WebClient client = new WebClient();
            string downloadString = client.DownloadString("http://localhost:777/api/v1/777/1/getorderitems");
            List<string> content = downloadString.Split(',', '{', '}').ToList();
            List<string> newContent = new List<string>();
            bool sameProductFound = false;
 //           int ifProductOnMultipleLines = 1;

            foreach (var item in content)
            {
                if (item.Contains("item_ID"))
                {
                    newContent.Add(item);
                }
            }

            for (int i = 0; i < newContent.Count; i++)
            {
                for (int j = 1; j < newContent.Count; j++)
                {
                    if (newContent[i] == newContent[j] && !sameProductFound)
                    {
                        //ifProductOnMultipleLines++;
                        sameProductFound = true;
                        break;                        
                    }
                }
                if (sameProductFound)
                    break;
            }

            if (sameProductFound)
                Assert.Fail("Product found on multiple lines");            

            //Assert.AreNotEqual(ifProductOnMultipleLines, 1, "Product found on multiple lines");
        }

    }
}
