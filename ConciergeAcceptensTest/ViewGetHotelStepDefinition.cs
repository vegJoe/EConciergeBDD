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
    public sealed class ViewGetHotelStepDefinition
    {
        //[Given(@"eConcierge backend is up and running")]
        //public void GivenEConciergeBackendIsUpAndRunning()
        //{
        //    HttpWebRequest request = WebRequest.Create("http://localhost:777/?rid=1&hid=1&md5=777") as HttpWebRequest;
        //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //    response.Close();
        //    Assert.IsNotNull(response, "Could find http://localhost:777/?rid=1&hid=1&md5=777");
        //}

        [Given(@"There are atleast (.*) hotel")]
        public void GivenThereAreAtleastHotel(int p0)
        {
            try
            {
                var myConnectionString = "Server = 127.0.0.1; Port = 3306; Database = hotel1; Uid = hotel; pwd = hotel ; database = hotel1";
                var conn = new MySqlConnection(myConnectionString);
                conn.Open();

                Console.WriteLine("Successfully connected to MariaDB");
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "app_hotel";
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

        [When(@"getHotel is used")]
        public void WhenGetHotelIsUsed()
        {
            ScenarioContext.Current.Pending();
        }


        [Then(@"the correct information should be delivered via getHotel")]
        public void ThenTheCorrectInformationShouldBeDeliveredViaGetHotel()
        {
            int tmpNum;
            WebClient client = new WebClient();
            string downloadString = client.DownloadString("http://localhost:777/api/v1/777/1/getHotel");
            List<string> content = downloadString.Split(',', '{', '}').ToList();

            int len = content[14].IndexOf('5');

            Assert.IsTrue(int.TryParse(content[3][30].ToString(), out tmpNum), "hotel_ID was not a number");
            Assert.IsTrue(int.TryParse(content[8][29].ToString(), out tmpNum), "pincode was not a number");
            Assert.IsTrue(int.TryParse(content[14][27].ToString(), out tmpNum), "stars was not a number");
        }
    }
}
