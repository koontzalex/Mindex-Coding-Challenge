using System.Net;
using System.Net.Http;
using CodeChallenge.Models;

using CodeChallenge.Tests.Integration.Extensions;
using CodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
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
        public void GetReportingStructureByEmployeeId_Returns_Ok()
        {
            // DbContext.Samples.FirstOrDefault(x => x.Name == "TestRecord").ShouldNotBeNull();
            // Arrange
            var reportingStructureEmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f";
            var expectedNumberOfReports = 2;
            var expectedFirstName = "Ringo";
            var expectedLastName = "Starr";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting_structure/show/{reportingStructureEmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ReportingStructure reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.AreEqual(expectedNumberOfReports, reportingStructure.NumberOfReports);
            Assert.AreEqual(expectedFirstName,reportingStructure.Employee.FirstName);
            Assert.AreEqual(expectedLastName, reportingStructure.Employee.LastName);
        }

        [TestMethod]
        public void GetReportingStructureByEmployeeId_HandlesMultipleReportLayers()
        {
            // Arrange
            var reportingStructureEmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedNumberOfReports = 4;
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting_structure/show/{reportingStructureEmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ReportingStructure reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.AreEqual(expectedNumberOfReports, reportingStructure.NumberOfReports);
            Assert.AreEqual(expectedFirstName,reportingStructure.Employee.FirstName);
            Assert.AreEqual(expectedLastName, reportingStructure.Employee.LastName);
        }

        [TestMethod]
        public void GetReportingStructureByEmployeeId_Returns_NotFound()
        {
            // Arrange
            var badReportingStructureEmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86a";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting_structure/show/{badReportingStructureEmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
