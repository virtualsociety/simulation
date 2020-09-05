using Xunit;

namespace Vs.Simulation.Core.Tests.State
{
    public class PersonStateTests
    {
        [Fact]
        public void ShouldTransition()
        {
            var person = new PersonState(LifeEvents.Born);
            person.Machine.Fire(LifeEventsTriggers.Adulthood);
            Assert.Equal(LifeEvents.Adult, person.Machine.State);
            person.Machine.Fire(LifeEventsTriggers.Mary);
            Assert.Equal(LifeEvents.Married, person.Machine.State);
            person.Machine.Fire(LifeEventsTriggers.Divorce);
            Assert.Equal(LifeEvents.Divorced, person.Machine.State);
            person.Machine.Fire(LifeEventsTriggers.Die);
            Assert.Equal(LifeEvents.Deceased, person.Machine.State);
            Assert.False(person.Machine.CanFire(LifeEventsTriggers.Birth));
        }
    }
}