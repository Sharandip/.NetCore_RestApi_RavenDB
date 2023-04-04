using AGDATAApi.Controllers;
using AGDATAApi.Models;
using AGDATAApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace AGDATAApi_NUnitTest
{
    [TestFixture]
    public class LocationTests
    {
        private Mock<ILocationService> locationServiceMock;
        private LocationsController locationsControllerMock;

        [SetUp]
        public void Setup()
        {
            locationServiceMock = new Mock<ILocationService>();
            locationsControllerMock = new LocationsController(locationServiceMock.Object);
        }

        [Test]
        public async Task TestGetAllLocations_ReturnAllLocations()
        {
            locationServiceMock.Setup(loc => loc.GetAllLocations()).ReturnsAsync(GetLocations());
            //var controller = new LocationsController(locationServiceMock.Object);
            var result = await locationsControllerMock.Get();
            Assert.NotNull(result);
        }

        [Test]
        public async Task Getlocation_withId_ReturnvalidLocation()
        {
            locationServiceMock.Setup(loc => loc.GetLocation("20")).ReturnsAsync(GetSingleLocation());
            locationServiceMock.Setup(loc => loc.IsLocationExists("20")).ReturnsAsync(true);
            var result = await locationsControllerMock.Get("20");
            var objectresult = result as OkObjectResult;
            Assert.NotNull(objectresult);
            Assert.AreEqual(StatusCodes.Status200OK, objectresult.StatusCode);
        }

        [Test]
        public async Task Getlocation_withInvalidId_ReturnNotFound()
        {
            locationServiceMock.Setup(loc => loc.IsLocationExists("20")).ReturnsAsync(false);
            locationServiceMock.Setup(loc => loc.GetLocation("20")).ReturnsAsync(GetSingleLocation());
            var result = await locationsControllerMock.Get("20");
            var objectresult = result as NotFoundObjectResult;
            Assert.AreEqual(StatusCodes.Status404NotFound, objectresult.StatusCode);
        }

        [Test]
        public async Task AddLocation_withNewId_ReturnStatus201Created()
        {
            locationServiceMock.Setup(loc => loc.IsDuplicateNameExists("Lisa")).ReturnsAsync(false);
            var result = await locationsControllerMock.Post(GetSingleLocation());
            var objectresult = result as StatusCodeResult;
            Assert.That(StatusCodes.Status201Created, Is.EqualTo(objectresult.StatusCode));
        }

        [Test]
        public async Task AddLocation_DuplicateName_ReturnBadRequest()
        {
            locationServiceMock.Setup(loc => loc.IsDuplicateNameExists("Lisa")).ReturnsAsync(true);
            var result = await locationsControllerMock.Post(GetSingleLocation());
            var objectresult = result as BadRequestObjectResult;
            Assert.That(StatusCodes.Status400BadRequest, Is.EqualTo(objectresult.StatusCode));
        }

        [Test]
        public async Task UpdateLocation_ValidId_ReturnOkResponse()
        {
            locationServiceMock.Setup(loc => loc.IsLocationExists("20")).ReturnsAsync(true);
            var result = await locationsControllerMock.Put("20", GetSingleLocation());
            var objectresult = result as OkObjectResult;
            Assert.That(StatusCodes.Status200OK, Is.EqualTo(objectresult.StatusCode));
        }

        [Test]
        public async Task UpdateLocation_ValidId_ReturnNotFoundResponse()
        {
            locationServiceMock.Setup(loc => loc.IsLocationExists("20")).ReturnsAsync(false);
            var result = await locationsControllerMock.Put("20", GetSingleLocation());
            var objectresult = result as NotFoundObjectResult;
            Assert.That(StatusCodes.Status404NotFound, Is.EqualTo(objectresult.StatusCode));
        }

        [Test]
        public async Task DeleteLocation_ValidId_ReturnOkResponse()
        {
            locationServiceMock.Setup(loc => loc.IsLocationExists("20")).ReturnsAsync(true);
            var result = await locationsControllerMock.Delete("20");
            var objectresult = result as OkObjectResult;
            Assert.That(StatusCodes.Status200OK, Is.EqualTo(objectresult.StatusCode));
        }

        [Test]
        public async Task DeleteLocation_InvalidId_ReturnNotFoundResponse()
        {
            locationServiceMock.Setup(loc => loc.IsLocationExists("20")).ReturnsAsync(false);
            var result = await locationsControllerMock.Delete("20");
            var objectresult = result as NotFoundObjectResult;
            Assert.That(StatusCodes.Status404NotFound, Is.EqualTo(objectresult.StatusCode));
        }

        public Location GetSingleLocation()
        {
            return new Location() {
                Id = "20",
                City = "WP",
                Name = "Lisa",
                PostalCode = "R6H 6J8",
                Province = "MB",
                StreetName = "Portage" };
        }

        public List<Location> GetLocations()
        {
            List<Location> locations = new()
            {
             new Location(){ Id ="0", City = "WP", Name="Saira", PostalCode="R6H 6J8", Province="MB", StreetName="Portage"},
             new Location(){ Id ="2", City = "WP", Name="Kaisi", PostalCode="R6H 6J8", Province="MB", StreetName="Portage"}
            };

            return locations;
        }

    }
}
