using System;

namespace sortingAlgorithms
{
    class Program
    {
        static readonly Random rand = new Random();

        static int arrayLength = 15;

        //static int shuffleRangeMin = 0;
        static readonly int shuffleRangeMax = arrayLength - 1;

        static readonly List<int> numbersOfPasses = [];

        class sortResult
        {
            public int sortNumber = 0;

            public int[] shuffledArray = [];
            //public int[] sortedArray = [];

            public int numberOfPasses;
        }

        static List<sortResult> sortResultsList = [];

        static int[] ShuffleArray(int[] inputArr)
        {

            int[] arr = (int[])inputArr.Clone();
            List<int> list = [];

            // Make a list with ordered numbers
            for (int i = 0; i < arrayLength; i++)
            {
                list.Add(i);
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

        static (int[], int) BubbleSort(int[] input)
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

            return (arr, numberOfPasses);
        }

        private static (int min, int index) FindLowestNumberOfPasses(List<sortResult> sortResults)
        {

            List<int> theList = sortResults.Select(sr => sr.numberOfPasses).ToList();

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

        static void WriteSortResultsToFile(List<sortResult> sortResults)
        {
            using (StreamWriter writer = new StreamWriter("output.txt", append: false))
            {
                foreach (var item in sortResults)
                {
                    writer.WriteLine($"Sort number {item.sortNumber}:");
                    writer.WriteLine($"shuffled list: {string.Join(", ", item.shuffledArray)}");
                    writer.WriteLine($"number of passes: {item.numberOfPasses} \n");

                }
            }
        }

        static void Main(string[] args)
        {
            arrayLength = 15;

            for (int i = 0; i < 5000000; i++)
            {

                //arrayLength = rand.Next(10, 20);

                sortResult sortResult = new();

                sortResult.sortNumber = i;

                int[] array = new int[arrayLength];

                array = ShuffleArray(array);
                sortResult.shuffledArray = (int[])array.Clone();

                (int[], int) result = BubbleSort(array);
                sortResult.numberOfPasses = result.Item2;

                sortResultsList.Add(sortResult);

            }

            Console.WriteLine($"Finished {sortResultsList.Count} sorts.\n");

            Console.WriteLine($"numbers per array: {arrayLength}");

            List<int> numbersOfPasses = sortResultsList.Select(sr => sr.numberOfPasses).ToList();

            Console.WriteLine($"average number of passes: {(float)numbersOfPasses.Average()}");

            (int, int) listMinResult = FindLowestNumberOfPasses(sortResultsList);
            Console.WriteLine($"lowest number of passes: {listMinResult.Item1}, at sort no. {listMinResult.Item2}");

            WriteSortResultsToFile(sortResultsList);

            Console.ReadKey();
        }

    }
}
