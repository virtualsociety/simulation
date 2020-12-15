using Deedle;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Vs.Simulation.Core.Probabilities;
using Xunit.Extensions.Ordering;


namespace Vs.Simulation.Core.Tests
{
    public class MaritalAgeProbabilityTest
    {
        [Fact, Order(1)]
        public void MaritalAgePropability()
        {
            //Arrange
            var env = new SimSharp.ThreadSafeSimulation(42);
            Frame<int, string> frame;
            frame = Frame.ReadCsv("../../../../../doc/data/MaritalAgeWeights.csv");
            List<double> femaleWeights;
            List<double> maleWeights;
            var womenList = new Dictionary<int, Dictionary<PartnerType, int>>();
            var menList = new Dictionary<int, Dictionary<PartnerType, int>>();
            var collection = new MaritalAgePropabilityCollection();
            var femaleAge = Age.FemaleWeights;
            var maleAge = Age.MaleWeights;

            //Act
            //In here we decide what marital status someone has at a certain age.
            //In our case it goes from 18 to 99 years old.
            for (int i = 18; i < 100; i++)
            {
                //We made a dictionary for both men and women.
                womenList.Add(i, new Dictionary<PartnerType, int>());
                womenList[i].Add(PartnerType.Single, 0);
                womenList[i].Add(PartnerType.Married, 0);
                womenList[i].Add(PartnerType.Partnership, 0);

                menList.Add(i, new Dictionary<PartnerType, int>());
                menList[i].Add(PartnerType.Single, 0);
                menList[i].Add(PartnerType.Married, 0);
                menList[i].Add(PartnerType.Partnership, 0);

                maleWeights = frame.GetColumn<double>(Convert.ToString(i)).Values.Select(c => Convert.ToDouble(c)).Take(3).ToList();
                femaleWeights = frame.GetColumn<double>(Convert.ToString(i)).Values.Select(c => Convert.ToDouble(c)).Skip(3).Take(3).ToList();

                //Female age is all the predictated ages of females in 2020.
                //This can be found in the probability class "Age".
                //First they get the weights from their age group. And with those weights deciding the results in a randomchoice, they get put into their dictionary.
                for (int j = 0; j < femaleAge[i]; j++)
                {
                    MaritalStatus.Weights = femaleWeights;
                    womenList[i][env.RandChoice(MaritalStatus.Source, MaritalStatus.Weights)]++;
                }

                //The exact same happens as stated above in women.
                for (int j = 0; j < maleAge[i]; j++)
                {
                    MaritalStatus.Weights = maleWeights;
                    menList[i][env.RandChoice(MaritalStatus.Source, MaritalStatus.Weights)]++;
                }

                //In here we sort everything into their respective classes. 
                //i here is yet again representing an age group.
                collection.Add(new MaritalAgeProbability()
                {
                    SingleWomen = womenList[i][PartnerType.Single],
                    MarriedWomen = womenList[i][PartnerType.Married],
                    DivorcedWomen = womenList[i][PartnerType.Partnership],
                    SingleMen = womenList[i][PartnerType.Single],
                    MarriedMen = womenList[i][PartnerType.Married],
                    DivorcedMen = womenList[i][PartnerType.Partnership],
                    Age = i
                });
            }
            //Putting the collection into a CSV which can be found in the location stated below.
            var export = Frame.FromRecords(collection);
            export.SaveCsv($"{Global.GetDataFolder()}maritalAgeProbability.csv");

            //Assert
            //We asserted a few ages to see if the test would keep on the same results as before.
            //For women we used ages 18, 36 and 98 as for men we used 30, 57 and 67.
            Assert.Equal(42679, womenList[36][PartnerType.Single]);
            Assert.Equal(92, womenList[18][PartnerType.Married]);
            Assert.Equal(741, womenList[98][PartnerType.Partnership]);
            Assert.Equal(9900, menList[67][PartnerType.Single]);
            Assert.Equal(81637, menList[57][PartnerType.Married]);
            Assert.Equal(1603, menList[30][PartnerType.Partnership]);
     

        }
        [Fact, Order(2)]
        public void ChildrenProbabilities()
        {
            //Arrange
            var env = new SimSharp.ThreadSafeSimulation(42);
            Frame<int, string> frame;
            frame = Frame.ReadCsv($"{Global.GetDataFolder()}maritalAgeProbability.csv");
            var womenAges = frame.GetColumn<double>("MarriedWomen").Values.Select(c => Convert.ToDouble(c)).ToList();
            Frame<int, string> MotherData = Frame.ReadCsv("../../../../../doc/data/motherbirthing_ages.csv");
            List<double> weights;
            List<double> childChoice;
            var childAmount = new Dictionary<int, List<double>>(); 
            var collection = new ChildrenProbabilityCollection();
            var collectionNew = new ChildrenProbabilityCollection();

            //Act
            //Here the mothers are randomly assigned to a designated amount of children.
            //The reults are first put in a list and after that they get sorted.
            for (int i = 0; i < 32; i++)
            {
                childAmount.Add(i, new List<double>());
                childChoice = new List<double>();
                int age = i + 18;
                weights = MotherData.GetColumn<double>(Convert.ToString(age)).Values.Select(c => Convert.ToDouble(c)).ToList();
                for (int j = 0; j < womenAges[i]; j++)
                {
                    Children.WeightGetChildren = weights;
                    childChoice.Add(env.RandChoice(Children.SourceGetChildren, Children.WeightGetChildren));
                }

                var children = childChoice.Where(p => p == 1).Count();
                //childAmount = new List<double>();

                for (int j = 0; j < children; j++)
                {
                    childAmount[i].Add(env.RandChoice(Children.SourceAmountChildren, Children.WeightsAmountChildren));
                }

                //Here they are getting sorted and added to a new collection
                collectionNew.Add(new ChildrenProbability()
                {
                    Age = age,
                    childrenAmount = { }
                    //WomenNoChildren = childChoice.Where(p => p == 0).Count(),
                    //MotherOfOneAgeFirstBorn = childAmount[i].Where(p => p == 1).Count(),
                    //MotherOfTwoAgeFirtBorn = childAmount[i].Where(p => p == 2).Count(),
                    //MotherOfThreeAgeFirstBorn = childAmount[i].Where(p => p == 3).Count(),
                    //BirthingMothersTotal = children
                });

            }

            //Both collections get exported out
            var exportNew = Frame.FromRecords(collectionNew);
            var export = Frame.FromRecords(collection);
            exportNew.SaveCsv($"{Global.GetDataFolder()}childrenProbabilityDifferent.csv");

            //Assert
            int expectedFirstChildren34 = 2564;
            int actualFirstChildren34 = childAmount[16].Where(p => p == 1).Count();
            Assert.Equal(expectedFirstChildren34, actualFirstChildren34);

            int expectedSecondChildren18 = 6;
            int actualSecondChildren18 = childAmount[0].Where(p => p == 2).Count();
            Assert.Equal(expectedSecondChildren18, actualSecondChildren18);

            int expectedThirdChildren43 = 73;
            int actualThirdChildren43 = childAmount[25].Where(p => p == 3).Count();
            Assert.Equal(expectedThirdChildren43, actualThirdChildren43);
        }
    }
}
