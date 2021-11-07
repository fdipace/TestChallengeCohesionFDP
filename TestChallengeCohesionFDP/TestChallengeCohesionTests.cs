using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;

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
        }
    }
}
