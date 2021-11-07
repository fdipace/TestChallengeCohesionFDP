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
                var resultMeasurementId = item["measurement_id"];

                Assert.IsTrue(resultStationName.Equals(stationName), "Station Name '" + resultStationName + "' does not match with expected station name '." + stationName + "'");
                Assert.IsTrue(resultMeasurementId.ToLower().Contains(stationName.ToLower().Replace(" ", "")), "Measurement Station Id '" + resultMeasurementId + "' does not correspond with expected Measurement Id for Station Name '." + stationName + "'");

            }
        }

        [TestMethod]
        public void Story2()
        {
            string stationName = "63rd Street Weather Station";
            var client = new RestClient(baseURL);

            var request1 = new RestRequest("?$where=measurement_timestamp between '2019-01-01' and '2019-12-31'&station_name=" + stationName + "&$limit=10&$offset=0", Method.GET);
            var request2 = new RestRequest("?$where=measurement_timestamp between '2019-01-01' and '2019-12-31'&station_name=" + stationName + "&$limit=10&$offset=1", Method.GET);
            var response1 = client.Execute(request1);
            var response2 = client.Execute(request2);
            var deserialize = new JsonDeserializer();
            var output1 = deserialize.Deserialize<List<Dictionary<string, string>>>(response1);
            var output2 = deserialize.Deserialize<List<Dictionary<string, string>>>(response2);

            foreach (var a in output1)
            {
                var resultStationNameA = a["station_name"];
                var resultMeasurementIdA = a["measurement_id"];

                foreach (var b in output2)
                {
                    var resultStationNameB = b["station_name"];
                    var resultMeasurementIdB = b["measurement_id"];

                    Assert.IsTrue(resultStationNameA.Equals(resultStationNameB), "Station Name '" + resultStationNameA + "' does not match with expected station name '." + resultStationNameB + "'");
                    Assert.IsFalse(resultMeasurementIdA.ToLower().Equals(resultMeasurementIdB.ToLower()), "Measurement Station Id '" + resultMeasurementIdA + "' in page 1 is equal to Measurement Id '." + resultMeasurementIdB + "' on page 2");
                }

            }
        }
    }
}
