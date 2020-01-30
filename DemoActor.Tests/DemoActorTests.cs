using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using IDemoActor;

namespace DemoActor.Tests
{
    [TestClass]
    public class DemoActorTests
    {
        private DemoActor CreateTestDemoActor(IRemindableWrapper remindableWrapper, IActorStateManager actorStateManager, IHelloDaprWorld daprWorld = null)
        {
            var actorTypeInformation = ActorTypeInformation.Get(typeof(DemoActor));
            Func<ActorService, ActorId, DemoActor> actorFactory = (service, id) =>
                new DemoActor(service, id, daprWorld, remindableWrapper, actorStateManager);
            var actorService = new ActorService(actorTypeInformation, actorFactory);
            var demoActor = actorFactory.Invoke(actorService, ActorId.CreateRandom());
            return demoActor;
        }

        [TestMethod]
        public void Constructor_Initialize()
        {
            //Arrange


            //Act
            var testDemoActor = CreateTestDemoActor(new Mock<IRemindableWrapper>().Object, new Mock<IActorStateManager>().Object, new HelloDaprWorld());
            //Assert

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Given_Null_HelloDaprWorld()
        {
            //Arrange


            //Act
            var testDemoActor = CreateTestDemoActor(new Mock<IRemindableWrapper>().Object, new Mock<IActorStateManager>().Object,null);
            //Assert

        }
        
        [TestMethod]
        public void SaveData_Given_State_Expect_SetStateAsync()
        {
            //Arrange
            var reminder = new Mock<IRemindableWrapper>();
            var stateManager = new Mock<IActorStateManager>();
            var myData = new MyData();
            var mockHelloWorld = new Mock<IHelloDaprWorld>();
            stateManager.Setup(manager => manager.SetStateAsync<MyData>("my_data",myData,It.IsAny<CancellationToken>())).Verifiable();
            mockHelloWorld.Setup(world => world.SayHello()).Verifiable();
            var testDemoActor = CreateTestDemoActor(reminder.Object, stateManager.Object, mockHelloWorld.Object);
            //Act
            
            testDemoActor.SaveData(myData).GetAwaiter().GetResult();
            //Assert
            stateManager.VerifyAll();
            mockHelloWorld.VerifyAll();
        }
    }
}
