using FunctionZero.MvvmZero;
using Moq;
using TrilogyAvivaTest.Bootstrap;
using TrilogyAvivaTest.Mvvm.PageViewModels;
using TrilogyAvivaTest.Services.Alert;
using TrilogyAvivaTest.Services.Logging;
using TrilogyAvivaTest.Services.Persistence;
using TrilogyAvivaTest.Services.Rest;

namespace TrilogyAvivaUnitTests
{
    [TestClass]
    public class HomePageVmTests
    {
        /// <summary>
        /// This is to test a real-world bug I encountered, where saving a location would incorrectly
        /// flag we can load a location when the location already matches the saved location.
        /// Some may argue it should be broken into smaller tests, and I agree, but life's too short and this isn't production code.
        /// </summary>
        [TestMethod]
        public void TestLocation_Load()
        {
            var testLocation = "Edinburgh";

            // This setup could be moved out into an initialisation. 

            var mockLogger = new Mock<ILogger>();
            var mockKeyStore = new Mock<IKeyStore>();
            var mockPageService = new Mock<IPageServiceZero>();
            var mockAlertService = new Mock<IAlertService>();
            var mockRestService = new Mock<IRestService>();

            mockKeyStore.Setup(store => store.ReadStringAsync(Constants.CityNameKey).Result)
                .Returns(testLocation);

            // HomePageVm does not have an interface, because page-view-models are typically never 
            // derived from, nor are they swapped out for different implementations by the container.
            // This makes Moq tests a little ugly, but my opinion leads to less development friction outside of tests.
            // Of course an interface can quickly and easily be put in place if the need ever arises.

            var testSubject = new HomePageVm(
                mockLogger.Object,
                mockKeyStore.Object,
                new TrilogyAvivaTest.Services.Api.OpenWeatherService(mockRestService.Object, "banana"),
                mockPageService.Object,
                mockAlertService.Object
                );

            // Pretend the vm has been presented ...
            testSubject.OnOwnerPagePushed(false);

            // Assert the saved location has been loaded ...
            Assert.AreEqual(testLocation, testSubject.CityName);
        }

        [TestMethod]
        public void TestLocation_Set_Save_Modify_Load()
        {
            var firstTestLocation = "Manchester";
            var secondTestLocation = "Bristol";

            // This setup could be moved out into an initialisation. 

            var mockLogger = new Mock<ILogger>();
            var mockKeyStore = new Mock<IKeyStore>();
            var mockPageService = new Mock<IPageServiceZero>();
            var mockAlertService = new Mock<IAlertService>();
            var mockRestService = new Mock<IRestService>();

            mockKeyStore.SetupSequence(store => store.ReadStringAsync(Constants.CityNameKey).Result)
                .Returns(string.Empty)
                .Returns(firstTestLocation);

            mockKeyStore.Setup(store => store.WriteStringAsync(Constants.CityNameKey, It.IsAny<string>()).Result).Returns(true);

            // HomePageVm does not have an interface, because page-view-models are typically never 
            // derived from, nor are they swapped out for different implementations by the container.
            // This makes Moq tests a little ugly, but my opinion leads to less development friction outside of tests.
            // Of course an interface can quickly and easily be put in place if the need ever arises.

            var testSubject = new HomePageVm(
                mockLogger.Object,
                mockKeyStore.Object,
                new TrilogyAvivaTest.Services.Api.OpenWeatherService(mockRestService.Object, "banana"),
                mockPageService.Object,
                mockAlertService.Object
                );

            testSubject.CityName = firstTestLocation;

            // Confirm our starting state ...
            Assert.AreEqual(firstTestLocation, testSubject.CityName);
            Assert.AreEqual(String.Empty, testSubject.SavedCityName);
            Assert.AreEqual(testSubject.CanSaveCity(), true);

            // Save our location ...
            testSubject.SaveCityNameCommand.Execute(null);

            // Assert our 'saved' state ...
            Assert.AreEqual(firstTestLocation, testSubject.CityName);
            Assert.AreEqual(testSubject.CityName, testSubject.SavedCityName);
            Assert.AreEqual(testSubject.CanSaveCity(), false);


            // Swap to a new location ...
            testSubject.CityName = secondTestLocation;

            // Assert the new location is not saved ...
            Assert.AreEqual(secondTestLocation, testSubject.CityName);
            Assert.AreNotEqual(testSubject.CityName, testSubject.SavedCityName);
            Assert.AreEqual(testSubject.CanSaveCity(), true);

            // Load our previously saved location ...
            testSubject.LoadCityNameCommand.Execute(null);

            // Assert our first location is saved ...
            Assert.AreEqual(firstTestLocation, testSubject.CityName);
            Assert.AreEqual(testSubject.CityName, testSubject.SavedCityName);
            Assert.AreEqual(testSubject.CanSaveCity(), false);


            mockKeyStore.Verify(x => x.WriteStringAsync(Constants.CityNameKey, It.IsAny<string>()), Times.Once);
            mockKeyStore.Verify(x => x.ReadStringAsync(Constants.CityNameKey), Times.Never);
        }
    }
}