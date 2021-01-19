using SimSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Vs.Simulation.Core.Probabilities;
using Vs.Simulation.Shared;
using Vs.Simulation.Shared.Model;

namespace Vs.Simulation.Core
{
    public partial class Population
    {
        public partial class Person
        {
            private IEnumerable<Event> Death()
            {
                yield return Environment.Timeout(_data.End);
                Population.Events.Write(new Triple()
                {
                    Subject = _data.Id,
                    Predicate = Constants.triple_predicate_died_of,
                    Object = _data.Id,
                    Time = Environment.Now
                });
                Statistics.Deaths++;

                //If the person was married, make the partner a widow and available for marriage again.
                if (_data.Flags[Constants.idx_married] == true)
                {

                    _data.Flags[Constants.idx_married] = false;
                    _data.partners.Last().Object._data.Flags[Constants.idx_married] = false;
                    _data.partners.Last().Object._data.partners.Last().End = Environment.Now;
                    _data.partners.Last().End = Environment.Now;

                    //Checks if the widowed partner will remarry
                    var idx = Convert.ToByte(_data.partners.Last().Object._data.Flags[Constants.idx_gender]);
                    if (Environment.RandChoice(MaritalStatusProbability.MaritalAgeSource,
                                MaritalStatusProbability.MaritalAgeWeights[idx, (Environment.Now.Year - 1950)]) == 1)
                    {
                        Remarry[Convert.ToByte(_data.Flags[Constants.idx_gender])].Push(_data.partners.Last().Object);
                    }

                    Population.Events.Write(new Triple()
                    {
                        Subject = _data.partners.Last().Object._data.Id,
                        Predicate = Constants.triple_predicate_widow_of,
                        Object = _data.Id,
                        Time = Environment.Now
                    });
                }
            }
        }
    }
}