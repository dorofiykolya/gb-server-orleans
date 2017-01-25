using System;

namespace BattleEngine.Utils
{
    public class IntRandom : Random
    {
        /**
         * An int [0..int.MAX_VALUE]
         * @return An int [0..int.MAX_VALUE]
         */

        public IntRandom(int seed) : base(seed)
        {
        }

        public override double next()
        {
            return sampleInt();
        }

        /**
         * An int [minvalue..maxvalue]
         * @param minValue
         * @param maxValue
         * @return An int [minvalue..maxvalue]
         */
        public int nextInRange(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("minValue > maxValue");
            }

            var range = maxValue - minValue;
            return (int)(sample() * range) + minValue;
        }

        /**
         * An int [0..maxValue]
         * @param maxValue
         * @return An int [0..maxValue]
         */
        public int nextInMax(int maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentException("maxValue < 0");
            }
            return (int)(sample() * maxValue);
        }

    }
}
