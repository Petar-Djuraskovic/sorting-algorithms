using System;

namespace sortingAlgorithms
{
    class Program
    {

        static bool loggingResults = true;

        static readonly Random rand = new Random();

        static int arrayLength = 15;

        static readonly int shuffleRangeMax = arrayLength - 1;

        class sortResult
        {
            public int sortNumber = 0;

            public int[] shuffledArray = [];
            public int[] sortedArray = [];

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
                list.Add(i + 1);
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

            Console.Write("\x1b[?25l"); //hide cursor

            while (changesWereMade)
            {

                changesWereMade = false;

                // Do one pass
                for (int j = 0; j < arrayLength - 1; j++)
                {

                    VisualizeArray(arr, j, arr.Max());
                    //Thread.Sleep(100);

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

            Console.Write("\x1b[?25h"); // Show cursor
            return (arr, numberOfPasses);
        }

        static void VisualizeArray(int[] arr, int currentLine, int graphHeight)
        {
            Console.Write("\x1b[H"); // Move cursor to home position

            for (int row = graphHeight; row > 0; row--)
            {
                for (int col = 0; col < arr.Length; col++)
                {
                    if (arr[col] >= row)
                    {
                        if (col == currentLine || col == currentLine + 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("█");
                            Console.ResetColor();
                        }
                        else { Console.Write("█"); }
                    }
                    else { Console.Write(" "); }
                }
                Console.Write("\n");
            }
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
                    writer.WriteLine($"sorted list: {string.Join(", ", item.sortedArray)}");
                    writer.WriteLine($"number of passes: {item.numberOfPasses} \n");

                }
            }
        }

        static void Main(string[] args)
        {
            arrayLength = 10;

            for (int i = 0; i < 1000; i++)
            {

                //arrayLength = rand.Next(10, 20);

                sortResult sortResult = new();

                if (loggingResults) { sortResult.sortNumber = i; }

                int[] array = new int[arrayLength];

                array = ShuffleArray(array);
                if (loggingResults) { sortResult.shuffledArray = (int[])array.Clone(); }

                (int[], int) result = BubbleSort(array);
                if(loggingResults) { sortResult.sortedArray = (int[])result.Item1.Clone(); }

                sortResult.numberOfPasses = result.Item2;

                sortResultsList.Add(sortResult);
            }

            Console.WriteLine($"Finished {sortResultsList.Count} sorts.\n");

            Console.WriteLine($"numbers per array: {arrayLength}");

            List<int> numbersOfPasses = sortResultsList.Select(sr => sr.numberOfPasses).ToList();

            Console.WriteLine($"average number of passes: {(float)numbersOfPasses.Average()}");

            (int, int) listMinResult = FindLowestNumberOfPasses(sortResultsList);
            Console.WriteLine($"lowest number of passes: {listMinResult.Item1}, at sort no. {listMinResult.Item2}");

            if(loggingResults) { WriteSortResultsToFile(sortResultsList); }
            Console.ReadKey();
        }

    }
}
