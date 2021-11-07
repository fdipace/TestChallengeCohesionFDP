using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;

namespace TestChallengeCohesionFDP
{
    [TestClass]
    public class TestChallengeCohesionTests
    {
        private string baseURL = "https://data.cityofchicago.org/resource/k7hf-8y75.json";

        [TestMethod]
        public void Story1()
        {
            string stationName = "Oak Street Weather Station";
            var client = new RestClient(baseURL);

            var request = new RestRequest("?station_name=" + stationName, Method.GET);
            var response = client.Execute(request);
            var deserialize = new JsonDeserializer();
            var output = deserialize.Deserialize<List<Dictionary<string, string>>>(response);

            foreach (var item in output)
            {
                var resultStationName = item["station_name"];

                Assert.IsTrue(resultStationName.Equals(stationName));
            }
        }
    }
}
