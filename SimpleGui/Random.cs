using System.Collections.Generic;
using System.Linq;

namespace SimpleGui
{
    /// <summary>
    /// This class contains methods random number generation and other random-related methods
    /// </summary>
    public class Random
    {
        static System.Random rand;

        static Random()
        {
            Random.SetSeed();
        }

        /// <summary>Selects an item at random from the specified sequence.</summary>
        /// <typeparam name="T">The type of the elements in <paramref name="sequence"/></typeparam>
        /// <param name="sequence">The sequence from which a random element will be selected.</param>
        public static T Choice<T>(IEnumerable<T> sequence)
        {
            if (sequence != null && sequence.Any())
                return sequence.ElementAt(Random.Next(sequence.Count()));
            else
                return default(T);
        }

        /// <summary>Returns a non-negative random number.</summary>
        public static int Next()
        {
            return rand.Next();
        }

        /// <summary>Returns a non-negative random number less than the specified maximum.</summary>
        /// <param name="maxValue">
        /// The exclusive upper boundary of the random number to return.
        /// <paramref name="maxValue"/> must be greater than or equal to zero.
        /// </param>
        public static int Next(int maxValue)
        {
            return rand.Next(maxValue);
        }

        /// <summary>Returns a random number within a specified range.</summary>
        /// <param name="minValue">The inclusive lower boundary of the random number to return.</param>
        /// <param name="maxValue">
        /// The exclusive upper boundary of the random number to return.
        /// <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
        /// </param>
        public static int Next(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue);
        }

        /// <summary>Returns a random number between 0.0 and 1.0.</summary>
        public static double NextDouble()
        {
            return rand.NextDouble();
        }

        /// <summary>Sets the seed value which is used to generate future random values.</summary>
        /// <param name="seed">The seed value.</param>
        public static void SetSeed(int? seed = null)
        {
            if (seed.HasValue)
                rand = new System.Random(seed.Value);
            else
                rand = new System.Random();
        }

        /// <summary>Shuffles the items in the specified sequence.</summary>
        /// <typeparam name="T">The type of the items in <paramref name="sequence"/></typeparam>
        /// <param name="sequence">The sequence of items to be shuffled.</param>
        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> sequence)
        {
            // create an int array of same length as sequence, shuffle it and yield the items in the
            // sequence using the shuffled result
            foreach (var i in Random.Shuffle(Enumerable.Range(0, sequence.Count()).ToArray()))
                yield return sequence.ElementAt(i);
        }

        /// <summary>Shuffles the elements of the specified array in place.</summary>
        /// <typeparam name="T">The type of the items in <paramref name="seq"/></typeparam>
        /// <param name="seq">The array whose elements are to be shuffled.</param>
        public static T[] Shuffle<T>(T[] seq)
        {
            for (int i = 0; i < seq.Length; i++)
            {
                var n = rand.Next(i, seq.Length);
                T temp = seq[i];
                seq[i] = seq[n];
                seq[n] = temp;
            }

            return seq;
        }
    }
}
