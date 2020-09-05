namespace Vs.Simulation.Core
{
    public class PersonState : BaseStateMachine<LifeEvents, LifeEventsTriggers>
    {
        public PersonState(LifeEvents initialState) : base(initialState)
        {
            Machine.Configure(LifeEvents.Born)
                .Permit(LifeEventsTriggers.Die, LifeEvents.Deceased)
                .Permit(LifeEventsTriggers.Adulthood, LifeEvents.Adult);
            Machine.Configure(LifeEvents.Adult)
                .Permit(LifeEventsTriggers.Die, LifeEvents.Deceased)
                .Permit(LifeEventsTriggers.Mary, LifeEvents.Married);
            Machine.Configure(LifeEvents.Married)
                .Permit(LifeEventsTriggers.Die, LifeEvents.Deceased)
                .Permit(LifeEventsTriggers.Divorce, LifeEvents.Divorced);
            Machine.Configure(LifeEvents.Divorced)
                .Permit(LifeEventsTriggers.Die, LifeEvents.Deceased)
                .Permit(LifeEventsTriggers.Mary, LifeEvents.Married);
            Machine.Activate();
        }
    }

    public enum LifeEvents
    {
        Born,
        Adult,
        Deceased,
        Married,
        Divorced
    }

    public enum LifeEventsTriggers
    {
        Birth,
        Adulthood,
        Die,
        Mary,
        Divorce
    }

}
