using NMemory.Indexes;
using NMemory.Tables;

namespace Vs.Simulation.Core.Database
{
    public class Predicate 
    {
        public int ObjectId {get;set;}
        public int SubjectId { get; set; }
    }

    public interface IIdentifiable
    {
        int Id { get; set; }
    }

    public class PopulationDb : NMemory.Database
    {
        public PopulationDb()
        {
            var people = base.Tables.Create<PersonEntity, int>(p => p.Id);
            var bornIndex = people.CreateIndex(new RedBlackTreeIndexFactory(),p => p.DateOfBirth);
            var partnerIndex = people.CreateIndex(new RedBlackTreeIndexFactory(), p => p.LifeEvent);
            var sexIndex = people.CreateIndex(new RedBlackTreeIndexFactory(), p => p.Sex);
            People = people;

          //  var predicates = base.Tables.Create<Predicate<PersonEntity, PersonEntity>, int>(p => p.Object.Id);
           // var predicates = base.Tables.Create<Predicate<PersonEntity, PersonEntity>, int>(p => p.Object.Id);

          //  Tables.CreateRelation<>
        }

        public ITable<PersonEntity> People { get; private set; }
        public ITable<Predicate> Predicates { get; private set; }
    }
}
