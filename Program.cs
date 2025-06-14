using System;

namespace bubble_sort
{
    class Program
    {
        static readonly Random rand = new Random();

        static int arrayLength = 15;

        //static int shuffleRangeMin = 0;
        static readonly int shuffleRangeMax = arrayLength - 1;

        static readonly List<int> numbersOfPasses = [];

        static int[] ShuffleArray(int[] inputArr, List<int> inputList)
        {

            int[] arr = (int[])inputArr.Clone();
            List<int> list = inputList;

            // Make a list with ordered numbers
            for (int i = 0; i < arrayLength; i++)
            {
                list.Add(i);
                //Console.WriteLine($"shuffleList: {string.Join(", ", list)}");
            }

            // Randomly pick a number from the list and assign it to the current array slot
            for (int i = 0; i < arrayLength; i++)
            {
                int randomIndex = rand.Next(0, list.Count);

                arr[i] = list[randomIndex];
                list.RemoveAt(randomIndex);
            }

            return arr;
        }

        static int[] BubbleSort(int[] input, List<int> numbersOfPasses)
        {
            int[] arr = (int[])input.Clone();
            bool changesWereMade = true;
            int numberOfPasses = 0;

            while (changesWereMade)
            {

                changesWereMade = false;

                // Do one pass
                for (int j = 0; j < arrayLength - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;

                        changesWereMade = true;
                    }
                }

                numberOfPasses++;
            }

            numbersOfPasses.Add(numberOfPasses);

            return arr;
        }

        static void Main(string[] args)
        {
            bool isSortingFixedLengthArrays = true; ///////////////////////////////////////////////////////////////////////
            List<int> arrayLengths = new();

            arrayLength = 15;

            for (int i = 0; i < 100000; i++)
            {

                if (!isSortingFixedLengthArrays) { arrayLength = rand.Next(10, 20); }

                if (!isSortingFixedLengthArrays)
                {
                    arrayLengths.Add(arrayLength);
                }

                int[] array = new int[arrayLength];
                List<int> shuffleList = new List<int>(arrayLength);

                array = ShuffleArray(array, shuffleList);
                Console.WriteLine($"Shuffled array: {string.Join(", ", array)}");

                array = BubbleSort(array, numbersOfPasses);
                Console.WriteLine($"Sorted array: {string.Join(", ", array)}");

                Console.WriteLine($"Sorted no. {i + 1}, {numbersOfPasses[i]} passes.\n");
            }

            Console.WriteLine("Finished all sorts.\n");

            if (isSortingFixedLengthArrays)
            {
                Console.WriteLine($"numbers per array: {arrayLength}");
            }
            else
            {
                Console.WriteLine($"average amount of numbers per array: {(float)arrayLengths.Average()}");
            }

            Console.Write($"list of numbers of passes: ");

            if (!isSortingFixedLengthArrays)
            {
                for (int i = 0; i < numbersOfPasses.Count; i++)
                {
                    Console.Write($"({numbersOfPasses[i]} / {arrayLengths[i]})  ");
                }
            }
            else
            {
                Console.WriteLine($"{string.Join(", ", numbersOfPasses)}");
            }

            Console.WriteLine($"average number of passes: {(float)numbersOfPasses.Average()}");

            (int min, int index) = FindListMinimum(numbersOfPasses);
            Console.WriteLine($"lowest number of passes: {min}, at sort no. {index}");

            Console.ReadKey();
        }

        private static (int min, int index) FindListMinimum(List<int> theList)
        {
            if (theList.Count == 0)
            {
                throw new Exception("FindMin: List can't be empty");
            }

            (int min, int index) result = (theList[0], 0);

            for (int i = 1; i < theList.Count; i++)
            {
                if (theList[i] < result.min)
                {
                    result = (theList[i], i);
                }
            }

            return result;
        }
    }
}
