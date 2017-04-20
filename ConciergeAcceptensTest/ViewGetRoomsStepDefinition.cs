using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MySql.Data.MySqlClient;

namespace ConciergeAcceptensTest
{
    [Binding]
    public sealed class ViewGetRoomsStepDefinition
    {
        //[Given(@"eConcierge backend is up and running")]
        //public void GivenEConciergeBackendIsUpAndRunning()
        //{
        //    HttpWebRequest request = WebRequest.Create("http://localhost:777/?rid=1&hid=1&md5=777") as HttpWebRequest;
        //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //    response.Close();
        //    Assert.IsNotNull(response, "Could find http://localhost:777/?rid=1&hid=1&md5=777");
        //}

        [Given(@"There are atleast (.*) room")]
        public void GivenThereAreAtleastRoom(int p0)
        {
            try
            {
                var myConnectionString = "Server = 127.0.0.1; Port = 3306; Database = hotel1; Uid = hotel; pwd = hotel ; database = hotel1";
                var conn = new MySqlConnection(myConnectionString);
                conn.Open();

                Console.WriteLine("Successfully connected to MariaDB");
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "app_hotel_room";
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.TableDirect;
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                Assert.IsNotNull(reader[0]);
                conn.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        //[When(@"The function getRooms is used")]
        //public void WhenTheFunctionGetRoomsIsUsed()
        //{
        //    ScenarioContext.Current.Pending();
        //}

        [Then(@"the correct information should be delivered when getRooms is used")]
        public void ThenTheCorrectInformationShouldBeDelivered()
        {         
            int tmpNum;
            WebClient client = new WebClient();
            string downloadString = client.DownloadString("http://localhost:777/api/v1/777/1/getRooms");
            List<string> content = downloadString.Split(',', '{', '}').ToList();

            int len = content[6].IndexOf('0');

            Assert.IsTrue(int.TryParse(content[3][29].ToString(), out tmpNum), "room_ID was not a number");
            Assert.IsTrue(int.TryParse(content[4][30].ToString(), out tmpNum), "hotel_ID was not a number");
            Assert.IsTrue(int.TryParse(content[5][28].ToString(), out tmpNum), "number was not a number");

            for (int i = 29; i <= 32; i++)
            {
                if(!int.TryParse(content[6][i].ToString(), out tmpNum))
                    Assert.IsTrue(false, "pincode was not a 4 digit number");
            }
        }
    }
}
