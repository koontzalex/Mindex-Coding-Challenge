
using System;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var compensation = new Compensation()
            {
                Employee = "b7839309-3348-463b-a7e3-5de1c168beb3",
                Salary = 500.5,
                EffectiveDate = new DateTime(2024, 2, 15),
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.AreEqual(compensation.Employee, newCompensation.Employee);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void CreateCompensation_Returns_422()
        {
            // Arrange
            var compensation = new Compensation()
            {
                Employee = "Not a valid employee id",
                Salary = 500.5,
                EffectiveDate = new DateTime(2024, 2, 15),
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            var compensationId = "84e80f79-6328-4f44-afbb-2ffbda969b1c";
            var expectedSalary = 500.5;
            var expectedEmployee = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{compensationId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(expectedSalary, compensation.Salary);
            Assert.AreEqual(expectedEmployee, compensation.Employee);
        }

        [TestMethod]
        public void UpdateCompensation_Returns_Ok()
        {
            // Arrange
            var compensation = new Compensation()
            {
                CompensationId = "84e80f79-6328-4f44-afbb-2ffbda969b1c",
                Salary = 100,
                EffectiveDate = new DateTime(2024, 2, 16),
                Employee = "16a596ae-edd3-4847-99fe-c4518e82c86f",
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var putRequestTask = _httpClient.PutAsync($"api/compensation/{compensation.CompensationId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var putResponse = putRequestTask.Result;
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
            var newCompensation = putResponse.DeserializeContent<Compensation>();

            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.Employee, newCompensation.Employee);
        }

        [TestMethod]
        public void UpdateCompensation_Returns_NotFound()
        {
            // Arrange
            var compensation = new Compensation()
            {
                CompensationId = "Invalid_Id",
                Employee = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                Salary = 15,
                EffectiveDate = new DateTime(2024, 2, 15),
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/compensation/{compensation.CompensationId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void UpdateCompensation_Returns_422()
        {
            // Arrange
            var compensation = new Compensation()
            {
                CompensationId = "84e80f79-6328-4f44-afbb-2ffbda969b1c",
                Employee = "invalid employee id",
                Salary = 15,
                EffectiveDate = new DateTime(2024, 2, 15),
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/compensation/{compensation.CompensationId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }
    }
}
