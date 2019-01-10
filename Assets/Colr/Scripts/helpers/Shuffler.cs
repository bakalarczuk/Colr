using System;

namespace Habtic.Games.Colr
{
    public class Shuffler
    {

        /// <summary>Shuffles the specified array.</summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="array">The array to shuffle.</param>

        public static T[] Shuffle<T>(T[] array)
        {
            System.Random random = new Random();

            for (int n = array.Length; n > 1;)
            {
                int k = random.Next(n);
                --n;
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            return array;
        }
    }
}
