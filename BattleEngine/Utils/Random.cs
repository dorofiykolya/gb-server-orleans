using System;

namespace BattleEngine.Utils
{
    public class Random
    {
        protected const int int_MAX_VALUE = 2147483647;
        protected const int int_MIN_VALUE = -2147483648;
        private const int MBIG = int_MAX_VALUE;
        private const int MSEED = 161803398;
        private const int MZ = 0;

        private int inext;
        private int inextp;
        private int[] seedArray = new int[56];

        /**
         * constructor
         * @param seed - int value
         */
        public Random(int seed)
        {
            int i;
            int ii;
            int mj;
            int mk;

            //Initialize our Seed array.
            //This algorithm comes from Numerical Recipes in C (2nd Ed.)
            var subtraction = (seed == int_MIN_VALUE) ? int_MAX_VALUE : Math.Abs(seed);
            mj = MSEED - subtraction;
            seedArray[55] = mj;
            mk = 1;
            for (i = 1; i < 55; i++)
            { //Apparently the range [1..55] is special (Knuth) and so we're wasting the 0'th position.
                ii = (21 * i) % 55;
                seedArray[ii] = mk;
                mk = mj - mk;
                if (mk < 0)
                    mk += MBIG;
                mj = seedArray[ii];
            }
            for (var k = 1; k < 5; k++)
            {
                for (i = 1; i < 56; i++)
                {
                    seedArray[i] -= seedArray[1 + (i + 30) % 55];
                    if (seedArray[i] < 0)
                        seedArray[i] += MBIG;
                }
            }
            inext = 0;
            inextp = 21;
            seed = 1;
        }

        /**
         * [0..1]
         * @return [0..1]
         */
        public virtual double next()
        {
            return sample();
        }

        /**
         *
         * @return [0..1]
         */
        protected double sample()
        {
            return (internalSample() * (1.0 / MBIG));
        }

        /**
         *
         * @return [0..int.MAX_VALUE]
         */
        protected int sampleInt()
        {
            return internalSample();
        }

        /**
         *
         * @return [0 .. 2 * int.MAX_VALUE - 1]
         */
        protected double sampleLarge()
        {
            return getSampleForLargeRange();
        }

        private int internalSample()
        {
            int retVal;
            int locINext = inext;
            int locINextp = inextp;

            if (++locINext >= 56)
                locINext = 1;
            if (++locINextp >= 56)
                locINextp = 1;

            retVal = seedArray[locINext] - seedArray[locINextp];

            if (retVal == MBIG)
                retVal--;
            if (retVal < 0)
                retVal += MBIG;

            seedArray[locINext] = retVal;

            inext = locINext;
            inextp = locINextp;

            return retVal;
        }

        private double getSampleForLargeRange()
        {
            // The distribution of double value returned by Sample 
            // is not distributed well enough for a large range.
            // If we use Sample for a range [Int32.MinValue..Int32.MaxValue)
            // We will end up getting even numbers only.

            var result = internalSample();
            // Note we can't use addition here. The distribution will be bad if we do that.
            var negative = (internalSample() % 2 == 0); // decide the sign based on second sample
            if (negative)
            {
                result = -result;
            }
            double d = result;
            d += (int_MAX_VALUE - 1); // get a number in range [0 .. 2 * Int32MaxValue - 1)
            d /= (2 * (uint)(int_MAX_VALUE)) - 1;
            return d;
        }
    }
}
