using MarsRover.Driving.Engines;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace MarsRover.Driving.Tests
{
    public class SettingsServiceTests
    {
        string _SettingsFolderPath;

        [SetUp]
        public void Setup()
        {
            this._SettingsFolderPath = @"C:\Temp";
        }

        [Test]
        [Category("DomainService")]
        public void Get_ReturnsNotNullSettings()
        {
            //*******| Given
            ISettingsService service = new SettingsService(this._SettingsFolderPath);

            //*******| When
            var settings = service.Get();

            //*******| Then
            Assert.IsNotNull(settings);
            Assert.IsNotNull(settings.ReferenceSystem, "System cannot be null!");
            Assert.IsNotNull(settings.DataPath, "DataPath cannot be null!");
            Assert.Pass();
        }

        [Test]
        [Category("DomainService")]
        public void SaveCartesianCoordinates_CreatesValidSettings()
        {
            //*******| Given
            ISettingsService service = new SettingsService(this._SettingsFolderPath);

            //*******| When
            service.Save(Coordinates.SystemsEnum.Cartesian.ToString());
            var newSettings = service.Get();

            //*******| Then
            Assert.IsNotNull(newSettings);
            Assert.IsNotNull(newSettings.ReferenceSystem, "System cannot be null!");
            Assert.IsNotNull(newSettings.DataPath, "DataPath cannot be null!");
            Assert.AreEqual(Coordinates.SystemsEnum.Cartesian.ToString(), newSettings.ReferenceSystem);
            Assert.AreEqual(this._SettingsFolderPath, newSettings.DataPath);
            Assert.Pass();
        }

        [Test]
        [Category("DomainService")]
        public void SaveSphericalCoordinates_CreatesValidSettings()
        {
            //*******| Given
            ISettingsService service = new SettingsService(this._SettingsFolderPath);

            //*******| When
            service.Save(Coordinates.SystemsEnum.Spherical.ToString());
            var newSettings = service.Get();

            //*******| Then
            Assert.IsNotNull(newSettings);
            Assert.IsNotNull(newSettings.ReferenceSystem, "System cannot be null!");
            Assert.IsNotNull(newSettings.DataPath, "DataPath cannot be null!");
            Assert.AreEqual("Spherical", newSettings.ReferenceSystem);
            Assert.AreEqual(this._SettingsFolderPath, newSettings.DataPath);
            Assert.Pass();
        }
    }
}