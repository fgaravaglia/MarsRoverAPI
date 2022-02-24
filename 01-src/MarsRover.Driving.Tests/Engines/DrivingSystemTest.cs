using MarsRover.Driving.Engines;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace MarsRover.Driving.Tests.Engines
{
    public class DrivingSystemTest
    {
        Coordinates _actualPosition;

        [SetUp]
        public void Setup()
        {
            this._actualPosition = null;
        }

        [Test]
        public void StartDoesNotThorwExceptionWIthoutObstaclesInsideRadarRadius()
        {
            //*******| Given
            DrivingSystem engine = InstanceNewSystem(new List<Obstacle>());
            Coordinates startingPoint = new Coordinates(0.0, 0.0);

            //*******| When
            engine.Start(startingPoint);

            //*******| Then
            Assert.Pass();
        }

        #region Tests without obstacles
        [Test]
        public void StartDoesNotThorwExceptionWithObstaclesInsideRadarRadius()
        {
            //*******| Given
            var obstacles = new List<Obstacle>()
            {
                new Obstacle(){ Position = new Coordinates(2,1)}
            };
            DrivingSystem engine = InstanceNewSystem(obstacles);
            Coordinates startingPoint = new Coordinates(0.0, 0.0);

            //*******| When
            engine.Start(startingPoint);

            //*******| Then
            Assert.Pass();
        }

        [Test]
        public void MoveForwardOnLeftWithoutObstacles_Returns_Success()
        {
            //*******| Given
            var obstacles = new List<Obstacle>();
            DrivingSystem engine = InstanceNewSystem(obstacles);
            Coordinates startingPoint = new Coordinates(0.0, 0.0);
            engine.Start(startingPoint);
            var targetDirection = "F";
            var targetSide = "L";

            //*******| When
            var result = engine.MoveTo(targetDirection, targetSide);

            //*******| Then
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.HasMoved);
            Assert.AreEqual("OK", result.ResultCode);
            Assert.IsNotNull(result.TargetPosition);
            Assert.AreEqual(-1.0, result.TargetPosition.positionX);
            Assert.AreEqual(1.0, result.TargetPosition.positionY);
            Assert.Pass();
        }

        [Test]
        public void MoveBackOnLeftWithoutObstacles_Returns_Success()
        {
            //*******| Given
            var obstacles = new List<Obstacle>();
            DrivingSystem engine = InstanceNewSystem(obstacles);
            Coordinates startingPoint = new Coordinates(0.0, 0.0);
            engine.Start(startingPoint);
            var targetDirection = "B";
            var targetSide = "L";

            //*******| When
            var result = engine.MoveTo(targetDirection, targetSide);

            //*******| Then
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.HasMoved);
            Assert.AreEqual("OK", result.ResultCode);
            Assert.IsNotNull(result.TargetPosition);
            Assert.AreEqual(-1.0, result.TargetPosition.positionX);
            Assert.AreEqual(-1.0, result.TargetPosition.positionY);
            Assert.Pass();
        }

        [Test]
        public void MoveForwardWithoutObstacles_Returns_Success()
        {
            //*******| Given
            var obstacles = new List<Obstacle>();
            DrivingSystem engine = InstanceNewSystem(obstacles);
            Coordinates startingPoint = new Coordinates(0.0, 0.0);
            engine.Start(startingPoint);
            var targetDirection = "F";
            var targetSide = "";

            //*******| When
            var result = engine.MoveTo(targetDirection, targetSide);

            //*******| Then
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.HasMoved);
            Assert.AreEqual("OK", result.ResultCode);
            Assert.IsNotNull(result.TargetPosition);
            Assert.AreEqual(0.0, result.TargetPosition.positionX, "Failed Target X position");
            Assert.AreEqual(1.0, result.TargetPosition.positionY, "Failed Target Y position");
            Assert.Pass();
        }
        #endregion

        #region Tests with obstacles
        [Test]
        public void MoveForwardOnLeftWithObstacles_Returns_Failed()
        {
            //*******| Given
            var obstacles = new List<Obstacle>()
            {
                new Obstacle(){ Position = new Coordinates(0.25, 0.25)}
            };
            DrivingSystem engine = InstanceNewSystem(obstacles);
            Coordinates startingPoint = new Coordinates(0.0, 0.0);
            engine.Start(startingPoint);
            var targetDirection = "F";
            var targetSide = "L";

            //*******| When
            var result = engine.MoveTo(targetDirection, targetSide);

            //*******| Then
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.HasMoved, "Wrong flag for moved");
            Assert.AreEqual("ERR-100", result.ResultCode, "Wrong ErrorCode!!");
            Assert.IsNotNull(result.TargetPosition);
            Assert.IsNotNull(result.Obstacle, "Obstacle must to be notified back");
            Assert.Pass();
        }

        [Test]
        public void MoveBackOnLeftWithObstacles_Returns_Failed()
        {
            //*******| Given
            var obstacles = new List<Obstacle>()
            {
                new Obstacle(){ Position = new Coordinates(0.25, -0.25)}
            };
            DrivingSystem engine = InstanceNewSystem(obstacles);
            Coordinates startingPoint = new Coordinates(0.0, 0.0);
            engine.Start(startingPoint);
            var targetDirection = "B";
            var targetSide = "L";

            //*******| When
            var result = engine.MoveTo(targetDirection, targetSide);

            //*******| Then
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.HasMoved, "Wrong flag for moved");
            Assert.AreEqual("ERR-100", result.ResultCode, "Wrong ErrorCode!!");
            Assert.IsNotNull(result.TargetPosition);
            Assert.IsNotNull(result.Obstacle, "Obstacle must to be notified back");
            Assert.Pass();
        }
        #endregion

        private  IObstacleRepository MockIObstacleRepository(List<Obstacle> obstacles)
        {
            var mockObstacleRepository = new Mock<IObstacleRepository>();
            mockObstacleRepository.Setup(x => x.GetAll()).Returns(obstacles);
            return mockObstacleRepository.Object;
        }

        private  IGeoLocalizer MockIGeoLocalizer()
        {
            var mock = new Mock<IGeoLocalizer>(MockBehavior.Loose);
            mock.Setup(x => x.GetPosition()).Returns(() => { return this._actualPosition; });
            mock.Setup( x => x.SavePosition(It.IsAny<Coordinates>())).Callback<Coordinates>(x => {  this._actualPosition = x; });
            return mock.Object;
        }

        private  DrivingSystem InstanceNewSystem(List<Obstacle> obstacles)
        {
            var radar = new RadarSystem(MockIObstacleRepository(obstacles));
            DrivingSystem engine = new DrivingSystem(radar, MockIGeoLocalizer());
            return engine;
        }
    }

}