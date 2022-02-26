using MarsRover.Driving.Engines;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace MarsRover.Driving.Tests
{
    public class DrivingServiceTests
    {
        string _SettingsFolderPath;
        ISettingsService _SettingsService;
        IDataService _DataService;
        DriverSettings _CartesianSettings;
        DriverSettings _SphericalSettings;

        [SetUp]
        public void Setup()
        {
            this._SettingsFolderPath = @"C:\Temp";
            this._SettingsService = new SettingsService(this._SettingsFolderPath);
            this._DataService = new JsonDataService(this._SettingsFolderPath);
            this._CartesianSettings = new DriverSettings() 
            { 
                ReferenceSystem = Coordinates.SystemsEnum.Cartesian.ToString(),
                DataPath = this._SettingsFolderPath
            };
            this._SphericalSettings = new DriverSettings() 
            { 
                ReferenceSystem = Coordinates.SystemsEnum.Spherical.ToString(),
                DataPath = this._SettingsFolderPath
            };
        }

        [TearDown]
        public void Dispose()
        {
            this._DataService.SetObstacles(new List<CoordinatesDTO>());
        }


        [Test]
        [Category("DomainService")]
        public void MoveToForwardOnRightInCartesianReference_ReturnsExpectedPosition()
        {
            //*******| Given
            var service = new DrivingService(this._CartesianSettings);
            var startingPoint = new CoordinatesDTO()
            {
                positionX = 0, positionY = 0
            };
            var commands = new List<DrivingCommandDTO>()
            {
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"}
            };
            
            //*******| When
            var feedback = service.Move(commands);

            //*******| Then
            Assert.IsNotNull(feedback);
            Assert.IsTrue(feedback.HasMoved, "Rover didn't moved at all!");
            Assert.IsNotNull(feedback.ResultCode);
            Assert.AreEqual( "OK", feedback.ResultCode, "Unexpected error occurred, since Rover didn't replied the OK Code");
            Assert.IsNull(feedback.ResultMessage);
            Assert.Pass();
        }

        [Test]
        [Category("DomainService")]
        public void MoveToForwardOnRightInSphericalReference_ReturnsExpectedPosition()
        {
            //*******| Given
            var service = new DrivingService(this._SphericalSettings);
            var startingPoint = new CoordinatesDTO()
            {
                positionX = 0, positionY = 0, positionZ = 0
            };
            var commands = new List<DrivingCommandDTO>()
            {
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"}
            };

            //*******| When
            var feedback = service.Move(commands);

            //*******| Then
            Assert.IsNotNull(feedback);
            Assert.IsTrue(feedback.HasMoved, "Rover didn't moved at all!");
            Assert.IsNotNull(feedback.ResultCode);
            Assert.AreEqual( "OK", feedback.ResultCode, "Unexpected error occurred, since Rover didn't replied the OK Code");
            Assert.IsNull(feedback.ResultMessage);
            Assert.Pass();
        }

        [Test]
        [Category("DomainService")]
        public void MoveISphericalReference_ReturnsExpectedPosition()
        {
            //*******| Given
            var service = new DrivingService(this._SphericalSettings);
            var startingPoint = new CoordinatesDTO()
            {
                positionX = 10, positionY = 2, positionZ = 0
            };
            var commands = new List<DrivingCommandDTO>()
            {
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"}
            };
            
            //*******| When
            var feedback = service.Move(commands);

            //*******| Then
            Assert.IsNotNull(feedback);
            Assert.IsTrue(feedback.HasMoved, "Rover didn't moved at all!");
            Assert.IsNotNull(feedback.ResultCode);
            Assert.AreEqual( "OK", feedback.ResultCode, "Unexpected error occurred, since Rover didn't replied the OK Code");
            Assert.IsNull(feedback.ResultMessage);
            Assert.IsNotNull(feedback.TargetPosition);
            Assert.IsNotNull(feedback.TargetPosition.positionZ);
 
            Assert.Pass();
        }

        [Test]
        [Category("DomainService")]
        public void Move_ReturnsExpectedPosition()
        {
            //*******| Given
            var service = new DrivingService(this._CartesianSettings);
            var startingPoint = new CoordinatesDTO()
            {
                positionX = 0, positionY = 0
            };
            var commands = new List<DrivingCommandDTO>()
            {
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"},
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Direction = "F"}
            };
            this._DataService.SetObstacles(new List<CoordinatesDTO>());

            //*******| When
            var feedback = service.Move(commands);

            //*******| Then
            Assert.IsNotNull(feedback);
            Assert.IsTrue(feedback.HasMoved, "Rover didn't moved at all!");
            Assert.IsNotNull(feedback.ResultCode);
            Assert.AreEqual( "OK", feedback.ResultCode, "Unexpected error occurred, since Rover didn't replied the OK Code");
            Assert.IsNull(feedback.ResultMessage);
            Assert.IsNotNull(feedback.TargetPosition);
            Assert.AreEqual(1, feedback.TargetPosition.positionX);
            Assert.AreEqual(2, feedback.TargetPosition.positionY);
            Assert.Pass();
        }

        [Test]
        [Category("DomainService")]
        public void MoveWithObstacle_ReturnsObstacleFoundAtSecondMoveAndLastValidPosition()
        {
            //*******| Given
            var service = new DrivingService(this._CartesianSettings);
            var startingPoint = new CoordinatesDTO()
            {
                positionX = 0, positionY = 0
            };
            var commands = new List<DrivingCommandDTO>()
            {
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"},
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"}
            };
            this._DataService.SetObstacles(new List<CoordinatesDTO>(){ new CoordinatesDTO(){ positionX = 1.15, positionY = 1.15 }});
            
            //*******| When
            var feedback = service.Move(commands);

            //*******| Then
            Assert.IsNotNull(feedback);
            Assert.IsTrue(feedback.HasMoved);
            Assert.IsNotNull(feedback.ResultCode);
            Assert.AreEqual( "ERR-100", feedback.ResultCode, "Unexpected error occurred, since Rover didn't replied the OK Code");
            Assert.AreEqual(1, feedback.TargetPosition.positionX);
            Assert.AreEqual(1, feedback.TargetPosition.positionY);
            Assert.IsNotNull(feedback.Obstacle);
            Assert.Pass();
        }

        [Test]
        [Category("DomainService")]
        public void MoveWithObstacle_ReturnsObstacleFoundAtFirstMoveAndLastValidPosition()
        {
            //*******| Given
            var service = new DrivingService(this._CartesianSettings);
            var startingPoint = new CoordinatesDTO()
            {
                positionX = 0, positionY = 0
            };
            var commands = new List<DrivingCommandDTO>()
            {
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"},
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"}
            };
            this._DataService.SetObstacles(new List<CoordinatesDTO>(){ new CoordinatesDTO(){ positionX = 0.25, positionY = 0.25 }});
            
            //*******| When
            var feedback = service.Move(commands);

            //*******| Then
            Assert.IsNotNull(feedback);
            Assert.IsFalse(feedback.HasMoved);
            Assert.IsNotNull(feedback.ResultCode);
            Assert.AreEqual( "ERR-100", feedback.ResultCode, "Unexpected error occurred, since Rover didn't replied the OK Code");
            Assert.AreEqual(0, feedback.TargetPosition.positionX);
            Assert.AreEqual(0, feedback.TargetPosition.positionY);
            Assert.IsNotNull(feedback.Obstacle);
            Assert.Pass();
        }

        [Test]
        [Category("DomainService")]
        public void MoveWithObstacleAtBeginning_ReturnsObstacleFoundAndInitialPosition()
        {
            //*******| Given
            var service = new DrivingService(this._CartesianSettings);
            var startingPoint = new CoordinatesDTO()
            {
                positionX = 0, positionY = 0
            };
            var commands = new List<DrivingCommandDTO>()
            {
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Side = "R", Direction = "F"},
                new DrivingCommandDTO(){ StartingPoint = startingPoint, Direction = "F"}
            };
            this._DataService.SetObstacles(new List<CoordinatesDTO>(){ new CoordinatesDTO(){ positionX = 0.25, positionY = 0.25 }});
            
            //*******| When
            var feedback = service.Move(commands);

            //*******| Then
            Assert.IsNotNull(feedback);
            Assert.IsFalse(feedback.HasMoved);
            Assert.IsNotNull(feedback.ResultCode);
            Assert.AreEqual( "ERR-100", feedback.ResultCode, "Unexpected error occurred, since Rover didn't replied the OK Code");
            Assert.AreEqual(startingPoint.positionX, feedback.TargetPosition.positionX);
            Assert.AreEqual(startingPoint.positionY, feedback.TargetPosition.positionY);
            Assert.AreEqual(1, feedback.TargetPosition.positionZ);
            Assert.IsNotNull(feedback.Obstacle);
            Assert.Pass();
        }
    }
}