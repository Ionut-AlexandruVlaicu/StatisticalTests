using System;
using System.Collections.Generic;
using MathNet.Numerics;


namespace StatisticalTests
{
    public static class GlobalVar
    {
        public static string bitsSequence;
        public static int sequenceLenght, blockLenght;
        public static double numberOfBlocks;
        public static List<string> bitsBlocksList = new List<string>();
        public static List<double> proportionsList = new List<double>();
    }
    class Program
    {
        static bool IsBinaryOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '1')
                    return false;
            }

            return true;
        }
        static void GetBitsSequence()
        {
            Console.WriteLine("Please insert the sequence of bits...");
            GlobalVar.bitsSequence = Console.ReadLine();
            if (string.IsNullOrEmpty(GlobalVar.bitsSequence))
            {
                Console.WriteLine("The sequence cannot be empty. Please insert a sequence that contains only 0 and 1.");
                GetBitsSequence();
            }
            else
            {
                if (IsBinaryOnly(GlobalVar.bitsSequence) == true)
                {
                    Console.WriteLine("The sequence is good");
                }
                else
                {
                    Console.WriteLine("The sequence is not good. Please insert a sequence that contains only 0 and 1.");
                    GetBitsSequence();
                }
            }
            GlobalVar.sequenceLenght = GlobalVar.bitsSequence.Length;
        }
        static void GetBlockLength()
        {
            Console.WriteLine("Please insert the lenght of the bit-blocks...");
            try
            {
                GlobalVar.blockLenght = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Wrong input, please provide numeric input.");
            }

            if (GlobalVar.blockLenght > GlobalVar.sequenceLenght || GlobalVar.blockLenght < 2)
            {
                Console.WriteLine("The targeted bit-block length must be at least 2 and less than the length of the Bits Sequence given earlier...");
                GetBlockLength();
            }
        }
        static void SplitBitsSequence()
        {
            int j = 0;
            int wholeLength = (GlobalVar.bitsSequence.Length / GlobalVar.blockLenght) * GlobalVar.blockLenght;
            string wholeSequence = GlobalVar.bitsSequence.Substring(0, wholeLength);
            Console.WriteLine(wholeSequence);
            for (int i = 0; i <= wholeLength - GlobalVar.blockLenght; i += GlobalVar.blockLenght)
            {
                GlobalVar.bitsBlocksList.Add(wholeSequence.Substring(i, GlobalVar.blockLenght));
                j++;
            }
            GlobalVar.numberOfBlocks = j;
        }
        static void GetBlocksProportions()
        {
            for (int i = 0; i<GlobalVar.bitsBlocksList.Count; i++)
            {
                double ones = 0;
                foreach(char ch in GlobalVar.bitsBlocksList[i])
                {
                    if (ch.Equals('1'))
                    {
                        ones++;
                    }
                }
                GlobalVar.proportionsList.Add(ones / GlobalVar.blockLenght);
            }
        }
        static double GetPValue()
        {
            double sumUp = 0, element =0.5;
            for (int i = 0; i< GlobalVar.proportionsList.Count; i++)
            {
                sumUp = sumUp + ((GlobalVar.proportionsList[i] - element)* (GlobalVar.proportionsList[i] - element));
            }
            double referenceDistribution = 4*GlobalVar.blockLenght*sumUp;
            double pValue = SpecialFunctions.GammaUpperRegularized((GlobalVar.numberOfBlocks / 2), (referenceDistribution / 2));
            return pValue;
        }
        static void Main(string[] args)
        {
            GetBitsSequence();
            GetBlockLength();
            SplitBitsSequence();
            if (GetPValue() >= 0.01)
            {
                Console.WriteLine("The given sequence is random");
            }
            else
            {
                Console.WriteLine("The given sequence is not random.");
            }
        }
    }
}
