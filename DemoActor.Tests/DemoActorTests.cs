using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Dapr.Actors;
using Dapr.Actors.Runtime;

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
    }
}
