using NMemory.Indexes;
using NMemory.Tables;

namespace Vs.Simulation.Core.Database
{
    public class PopulationDb : NMemory.Database
    {
        public PopulationDb()
        {
            var people = base.Tables.Create<PersonEntity, int>(p => p.Id);
            var bornIndex = people.CreateIndex(new RedBlackTreeIndexFactory(),p => p.DateOfBirth);
            var partnerIndex = people.CreateIndex(new RedBlackTreeIndexFactory(), p => p.LifeEvent);
            var sexIndex = people.CreateIndex(new RedBlackTreeIndexFactory(), p => p.Sex);
            People = people;
        }

        public ITable<PersonEntity> People { get; private set; }
    }
}
