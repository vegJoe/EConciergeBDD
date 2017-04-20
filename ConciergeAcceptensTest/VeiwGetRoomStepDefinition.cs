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
    public sealed class VeiwGetRoomStepDefinition
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        [Then(@"the correct information should be delivered when getRoom is used")]
        public void ThenTheCorrectInformationShouldBeDeliveredWhenGetRoomIsUsed()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
