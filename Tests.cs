using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using RestSharp;

namespace BeachWeatherStations
{
    [TestFixture]
    public class Tests
    {
        [TestCase("63rd Street Weather Station")]
        [Description(@"As a user of the API I want to list all measurements taken by the station on Oak Street in json format.
            ○ GIVEN beach weather station sensor “Oak Street”
            ○ WHEN the user requests station data
            ○ THEN all data measurements correspond to only that station")]
        public void AllReturnedDataShouldCorrespondsToRequestedStation(string station)
        {
            var restClient = new RestClient("https://data.cityofchicago.org/");
            var request = new RestRequest(Method.GET) {Resource = "resource/k7hf-8y75.json"};
            request.AddParameter("station_name", station);
            restClient
                .Execute<List<Entry>>(request)
                .Data
                .Should()
                .OnlyContain(entry => entry.StationName == station,
                    because: $"all measurements must correspond to the requested station {station}");
        }        
        
        [TestCase("63rd Street Weather Station", 10)]
        [Description(@"As a user of the API I want to be able to page through json data sets of 2019 taken by the sensor on
        63rd Street.
            ○ GIVEN the beach weather station on 63rd Street’s sensor data of 2019
            ○ WHEN the user requests data for the first 10 measurements
            ○ AND the second page of 10 measurements
            ○ THEN the returned measurements of both pages should not repeat")]
        public void ReturnedMeasurementsOnDifferentPagesShouldNotRepeat(string station, int limit)
        {
            var restClient = new RestClient("https://data.cityofchicago.org/");
            var request = new RestRequest(Method.GET) {Resource = "resource/k7hf-8y75.json"};
            request.AddParameter("station_name", station);
            request.AddParameter("$limit", limit);
            var page = 0;
            var offset = page * limit;
            request.AddParameter("$offset", offset);
            var firstPageResult = restClient.Execute<List<Entry>>(request).Data;
            page = 1;
            offset = page * limit;
            request.AddOrUpdateParameter("$offset", offset);
            var secondPageResult = restClient.Execute<List<Entry>>(request).Data;
            firstPageResult.Should().NotIntersectWith(secondPageResult, because:"measurements on different pages should not repeat");
        }        
        
        [TestCase("63rd Street Weather Station", "battery_life < full")]
        [Description(@"As a user of the API I expect a SQL query to fail with an error message if I search using a malformed
        query. Note: This is a negative test. We want to make sure that the API throws an error when
        expected.
            ○ GIVEN all beach weather station sensor data of the station on 63rd Street
            ○ WHEN the user requests sensor data by querying battery_life values that are
            less than the text “full” ($where=battery_life < full)
            ○ THEN an error code “malformed compiler” with message “Could not parse SoQL
            query” is returned")]
        public void MalformedQueryShouldReturnAnError(string station, string badWhereQuery)
        {
            var restClient = new RestClient("https://data.cityofchicago.org/");
            var request = new RestRequest(Method.GET) {Resource = "resource/k7hf-8y75.json"};
            request.AddParameter("station_name", station);
            request.AddParameter("$where", badWhereQuery);
            var response = restClient.Execute<Error>(request);
            using (new AssertionScope())
            {
                response.IsSuccessful.Should().BeFalse(because: "malformed request should fail should fail");
                response.Data.Code.Should().Contain("query.compiler.malformed");
                response.Data.Message.Should().Contain("Could not parse SoQL query");   
            }
        }
    }
}