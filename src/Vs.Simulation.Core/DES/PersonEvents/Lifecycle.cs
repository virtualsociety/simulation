using SimSharp;
using System;
using System.Collections.Generic;
using Vs.Simulation.Core.Probabilities;
using Vs.Simulation.Shared;
using Vs.Simulation.Shared.Model;

namespace Vs.Simulation.Core
{
    public partial class Population
    {
        public partial class Person
        {
            private IEnumerable<Event> LifeCycle()
            {
                var idx = Convert.ToByte(_data.Flags[Constants.idx_gender]);
                _data.End = TimeSpan.FromDays(Environment.RandChoice(
                    AgeProbability.Source[idx, 0],
                    AgeProbability.Weights[idx, 0]) * 365);
                _data.Dod = _data.Dob + _data.End;
                _data.YearDod = _data.Dod.Year;
                Global._totalAge[idx] += _data.End.Days / 365;
                if (_data.parents != null)
                    populationGrowth.AddPerson(this);

                Environment.Process(Death());
                // Only Schedule the next process chain if the person is expected to reach maturity.
                if (_data.End.Days > 18 * 365)
                {
                    Statistics.ReachMaturity++; // indicating this person will reach maturity.
                    // Wait for adulthood.
                    yield return Environment.Timeout(TimeSpan.FromDays(18 * 365));
                    Events.Write(new Triple() { Subject = _data.Id, Predicate = Constants.triple_predicate_adult, Time = Environment.Now, Object = _data.Id });
                    // put this person in the unmarried queue
                    Unmarried[Convert.ToByte(_data.Flags[Constants.idx_gender])].Push(this);
                    // determine if this person will ever initiate a marriage process.
                    var status = Environment.RandChoice(MaritalStatusProbability.MaritalStatusSource,
                        MaritalStatusProbability.MaritalStatusWeights[Environment.Now.Year - 1950]);
                    if (status != Constants.marital_single)
                    {
                        // only people who do not stay single in their life cycle will be scheduled to marry.
                        var maritalAge = TimeSpan.FromDays(Environment.RandChoice(MaritalStatusProbability.MaritalAgeSource,
                            MaritalStatusProbability.MaritalAgeWeights[idx, (Environment.Now.Year - 1950)])) * 365;
                        // only people who do not die before the marital age are scheduled to marry.
                        if (maritalAge < _data.End)
                            Environment.Process(Marry(maritalAge));
                    }
                }
            }

        }
    }
}