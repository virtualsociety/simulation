using System;
using System.IO;
using System.Text;
using static Vs.Simulation.Terminal2.People;

namespace Vs.Simulation.Terminal2.LiveReporting
{
    /// <summary>
    /// Real time data collection for population growth.
    /// </summary>
    public class PopulationGrowth
    {
        private readonly DateTime startDate;
        private readonly DateTime endDate;
        private readonly int StartYear; // for performance
        private readonly int EndYear;   // for performance
        private readonly int size;
        private readonly int sizeAge;

        /// <summary>
        /// Data containing number of people ordered in Gender, Year, Age
        /// </summary>
        public long[,,] Data;

        public PopulationGrowth(DateTime StartDate, DateTime EndDate)
        {
            startDate = StartDate;
            endDate = EndDate;
            StartYear = startDate.Year;
            EndYear = endDate.Year;
            size = EndYear - StartYear;
            sizeAge = 140;
            Data = new long[2, size+1, sizeAge];
        }

        public void AddPerson(Person person)
        {
            int from = person._data.Year - StartYear;
            int to = person._data.YearDod - StartYear;
            if (to > size)
                to = size; // not reported beyond simulation end year.
            byte gender = Convert.ToByte(person._data.Flags[Constants.idx_gender]);
            for (int year = from; year < to; year++)
            {
                // place the person in the correct age group.
                //for (int ageGroup=0;ageGroup< to; ageGroup++)
                //{
                    Data[gender, year, year-from]++;
                //}
            }
        }

        public void ToCsv(string path)
        {
            StringBuilder s = new StringBuilder();
            s.Append(",");
            for (int year = 0; year < size; year++)
            {
                s.Append($"{year + StartYear},");
            }
            s.Append("\r\n");
            for (int ageGroup = 0; ageGroup < sizeAge; ageGroup++)
            {
                // Add Age Group
                s.Append($"{ageGroup},");
                // Report totals per age group over the years
                for (int year = 0; year < size; year++)
                {
                    s.Append($"{Data[0, year, ageGroup]},");
                }
                s.Append("\r\n");
            }
            File.WriteAllText(path,s.ToString());
        }
    }
}
