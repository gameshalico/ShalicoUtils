namespace ShalicoUtils
{
    public static class FractionIntExtensions
    {
        public static FractionInt Rescale(this FractionInt fraction, int newDenominator)
        {
            return new FractionInt(fraction.Numerator * newDenominator, fraction.Denominator);
        }

        public static FractionInt RescaleByCeil(this FractionInt fraction, int newDenominator)
        {
            return new FractionInt(
                (fraction.Numerator + fraction.Denominator - 1) / fraction.Denominator * newDenominator,
                fraction.Denominator);
        }

        public static FractionInt RescaleByRound(this FractionInt fraction, int newDenominator)
        {
            return new FractionInt(
                (fraction.Numerator >= 0
                    ? fraction.Numerator + fraction.Denominator / 2
                    : fraction.Numerator - fraction.Denominator / 2) / fraction.Denominator * newDenominator,
                fraction.Denominator);
        }

        public static FractionInt RescaleByFloor(this FractionInt fraction, int newDenominator)
        {
            return new FractionInt(fraction.Numerator / fraction.Denominator * newDenominator, fraction.Denominator);
        }
    }
}