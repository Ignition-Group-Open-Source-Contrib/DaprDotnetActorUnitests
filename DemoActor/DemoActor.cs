// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using IDemoActor;

namespace DemoActor
{
    /// <summary>
    /// Actor Implementation.
    /// Following example shows how to use Actor Reminders as well.
    /// For Actors to use Reminders, it must derive from IRemindable.
    /// If you don't intend to use Reminder feature, you can skip implementing IRemindable and reminder specific methods which are shown in the code below.
    /// </summary>
    public class DemoActor : Actor, IDemoActor.IDemoActor, IRemindable
    {
        private readonly IHelloDaprWorld daprWorldService;
        private const string StateName = "my_data";
        private IActorReminder reminder;
        private readonly IRemindableWrapper remindableWrapper;
        private readonly IActorStateManager actorStateManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DemoActor"/> class.
        /// </summary>
        /// <param name="service">Actor Service hosting the actor.</param>
        /// <param name="actorId">Actor Id.</param>
        public DemoActor(ActorService service
            , ActorId actorId
            , IHelloDaprWorld daprWorldService
            , IRemindableWrapper remindableWrapper = null
            , IActorStateManager actorStateManager = null
            )
            : base(service, actorId)
        {
            this.daprWorldService = daprWorldService ?? throw new ArgumentNullException(nameof(daprWorldService));
            this.remindableWrapper = remindableWrapper ?? new RemindableWrapper(RegisterReminderAsync);
            this.actorStateManager = actorStateManager ?? this.StateManager; ;
        }

        /// <inheritdoc/>
        public async Task SaveData(MyData data)
        {
            Console.WriteLine($"This is Actor id {this.Id} with data {data}.");
            var sayHello = daprWorldService.SayHello();
            data.PropertyA = sayHello;
            // Set State using StateManager, state is saved after the method execution.
            await this.actorStateManager.SetStateAsync<MyData>(StateName, data);
        }

        /// <inheritdoc/>
        public Task<MyData> GetData()
        {
            // Get state using StateManager.
            return this.actorStateManager.GetStateAsync<MyData>(StateName);
        }

        /// <inheritdoc/>
        public Task TestThrowException()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task TestNoArgumentNoReturnType()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task RegisterReminder()
        {
            
            this.reminder = await this.remindableWrapper.RegisterReminderAsync("Test", null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        }

        /// <inheritdoc/>
        public Task UnregisterReminder()
        {
            return this.UnregisterReminderAsync("Test");//TODO 
        }

        /// <inheritdoc/>
        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            // This method is invoked when an actor reminder is fired.
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task RegisterTimer()
        {
            return this.RegisterTimerAsync("Test", this.TimerCallBack, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));//TODO 
        }

        /// <inheritdoc/>
        public Task UnregisterTimer()
        {
            return this.UnregisterTimerAsync("Test");//TODO 
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        protected override Task OnActivateAsync()
        {
            // Provides opportunity to perform some optional setup.
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called whenever an actor is deactivated after a period of inactivity.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        protected override Task OnDeactivateAsync()
        {
            // Provides Opportunity to perform optional cleanup.
            return Task.CompletedTask;
        }

        private Task TimerCallBack(object data)
        {
            // Code for time callback can be added here.
            return Task.CompletedTask;
        }
    }
}
