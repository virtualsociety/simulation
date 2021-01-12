namespace Vs.Simulation.Shared
{
    public static class Constants
    {
        public const bool gender_female = true;
        public const bool gender_male = false;

        public const int marital_single = 0;
        public const int marital_married = 1;
        public const int marital_partner = 2;

        public const byte idx_gender_female = 1;
        public const byte idx_gender_male = 0;

        public static int idx_gender = 0;
        public static int idx_married = 1;

        /// <summary>
        /// Triple Store Predicates
        /// 
        /// i.e.: person (1003991) -> triple_predicate_child_of  (0)  -> person (1992811)
        ///       person (1992811) -> triple_predicate_parent_of (4)  -> person (1003991)
        /// </summary>
        public static long triple_predicate_child_of = 0;
        public static long triple_predicate_married_to = 1;
        public static long triple_predicate_divorced_from = 2;
        public static long triple_predicate_died_of = 3;
        public static long triple_predicate_parent_of = 4;
        public static long triple_predicate_adult = 5;
        public static long triple_predicate_created = 6;
        public static long triple_predicate_widow_of = 7;
    }
}
