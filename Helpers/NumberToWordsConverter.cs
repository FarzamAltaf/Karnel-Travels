namespace kernel.Helpers
{
    public class NumberToWordsConverter
    {
        public static string ConvertToWords(int number)
        {
            if (number == 1) return "One";
            if (number == 2) return "Two";
            if (number == 3) return "Three";
            if (number == 4) return "Four";
            if (number == 5) return "Five";
            return number.ToString(); 
        }
    }
}
