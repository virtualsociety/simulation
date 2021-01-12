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
            /// <summary>
            /// Schedule a marriage
            /// </summary>
            /// <param name="when"></param>
            /// <returns></returns>
            private IEnumerable<Event> Marry(TimeSpan when)
            {
                // We need to schedule a marriage now
                yield return Environment.Timeout(when);
                if (!_data.Flags[Constants.idx_married]) // Otherwise married by another person within this timespan.
                {
                    //ToDo: Add a check where people can remarry

                    Person partner = null;
                    _data.Flags[Constants.idx_married] = true;

                    if (_data.partners != null && Remarry[Convert.ToByte(!_data.Flags[Constants.idx_gender])].Count > 0)
                    {
                        partner = Remarry[Convert.ToByte(!_data.Flags[Constants.idx_gender])].Pop();
                        Statistics.Remarried++;
                    }

                    while (partner == null || partner._data.Flags[Constants.idx_married])
                    {
                        partner = Unmarried[Convert.ToByte(!_data.Flags[Constants.idx_gender])].Pop();
                    }
                    partner._data.Flags[Constants.idx_married] = true;
                    if (_data.partners == null)
                        _data.partners = new List<DateRange<Person>>();
                    _data.partners.Add(new DateRange<Person>(partner, Environment.Now));
                    if (partner._data.partners == null)
                        partner._data.partners = new List<DateRange<Person>>();
                    partner._data.partners.Add(new DateRange<Person>(this, Environment.Now));
                    Events.Write(new Triple()
                    {
                        Subject = _data.Id,
                        Predicate = Constants.triple_predicate_married_to,
                        InvPredicate = Constants.triple_predicate_married_to,
                        Time = Environment.Now,
                        Object = partner._data.Id
                    });
                    Statistics.Couples++;

                    //Chooses the marriageDuration and the marriage ending date
                    var marriageDuration = new TimeSpan((long)Environment.RandChoice(MaritalDuration.Source,
                           MaritalDuration.Weights) * 365);
                    var marriageEndDate = Environment.Now.Add(marriageDuration);

                    //Checks if they will get children
                    if (_data.Flags[Constants.idx_gender] == Constants.gender_female && (int)SimulationAge < 49)
                    {
                        if (Environment.RandChoice(
                            Children.MotherChildSource, Children.MotherWeights[(int)SimulationAge - 18]) == 1)
                        {
                            List<TimeSpan> birthSchedules;
                            DetermineNumberOfChildren(marriageEndDate, out birthSchedules);
                            ScheduleLabor(birthSchedules);
                        }
                    }
                    else if ((int)partner.SimulationAge < 49)
                    {
                        if (Environment.RandChoice(
                            Children.MotherChildSource, Children.MotherWeights[(int)partner.SimulationAge - 18]) == 1)
                        {
                            List<TimeSpan> birthSchedules;
                            DetermineNumberOfChildren(marriageEndDate, out birthSchedules);
                            ScheduleLabor(birthSchedules);
                        }
                    }

                    if (Environment.Now < _data.Dod && Environment.Now < partner._data.Dod)
                    {
                        Environment.Process(Divorcing(marriageDuration));
                    }
                }
            }

            /// <summary>
            /// Schedule a divorce
            /// </summary>
            /// <param name="marriageDuration"></param>
            /// <returns></returns>

            private IEnumerable<Event> Divorcing(TimeSpan marriageDuration)
            {
                yield return Environment.Timeout(marriageDuration);
                //Takes away their married status
                _data.Flags[Constants.idx_married] = false;
                _data.partners.Last().Object._data.Flags[Constants.idx_married] = false;
                _data.partners.Last().Object._data.partners.Last().End = Environment.Now;
                _data.partners.Last().End = Environment.Now;
                //Writes that the divorce happened
                Events.Write(new Triple()
                {
                    Subject = _data.Id,
                    Predicate = Constants.triple_predicate_divorced_from,
                    InvPredicate = Constants.triple_predicate_divorced_from,
                    Time = Environment.Now,
                    Object = _data.partners.Last().Object._data.Id
                });

                //Checks if this person will remarry.
                CheckRemarriage(this);
                //Checks if ex partner will remarry.
                CheckRemarriage(_data.partners.Last().Object);
            }

            /// <summary>
            /// Determine the number of children during the marriage duration.
            /// </summary>
            /// <param name="marriageDuration"></param>
            public void DetermineNumberOfChildren(DateTime marriageDuration, out List<TimeSpan> birthSchedules)
            {
                birthSchedules = new List<TimeSpan>();
                int year = Environment.Now.Year - 1950;
                var labourSchedule = TimeSpan.FromDays((1) * 2 * 365);
                for (int i = 0; i < Environment.RandChoice(Children.SourceAmountChildren, Children.ChildAmountWeights[year]); i++)
                {
                    // Determines whether the parents has not died and is within marriage duration
                    if (Environment.Now.Add(labourSchedule) > _data.Dod && Environment.Now.Add(labourSchedule) > marriageDuration)
                        break;
                    birthSchedules.Add(labourSchedule);
                    //Checks the number of the child and get's the correct labor date
                    if (i < 3)
                    {
                        labourSchedule = TimeSpan.FromDays(Children.LabourYears[year][i] * 365);
                    }
                }
            }

            /// <summary>
            /// Check the probability if a person will remarry.
            /// </summary>
            /// <param name="person"></param>
            public void CheckRemarriage(Person person)
            {
                var idx = Convert.ToByte(person._data.Flags[Constants.idx_gender]);
                if (Environment.RandChoice(MaritalStatus.RemarriageSource,
                            MaritalStatus.RemarriageWeights[idx, (Environment.Now.Year - 1950)]) == 1)
                {
                    Remarry[Convert.ToByte(_data.Flags[Constants.idx_gender])].Push(person);
                }
            }
        }
    }
}