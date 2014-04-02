using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using Parser;

namespace File
{
    /***
     * Author: Justin Tang
     * 
     * Main method for the FileParser
     * Asks for an input of a file
     * Text file must be in unicode
     * 
     * Parses the file using the fileParser class and prints the total number of words,
     * the top 10 most used words, the amount of times each word is used, and the percentage 
     * of the occurrence of the word in the the file
     * */
    public class FileParser
    {
        static String filename; //filename of file to be parsed
        public static void Main()
        {
            Console.WriteLine("Input file name: ");
            filename = Console.ReadLine();
            fileParser parser = new fileParser(filename);
            List<wordStruct> top10 = parser.getTop10();
            int i = 0;
            Console.WriteLine();
            Console.WriteLine("Total Number of Words : {0}", parser.getTotal());
            Console.WriteLine();
            Console.WriteLine("**********************************");
            Console.WriteLine("Top 10 Most Used Words in the File");
            Console.WriteLine("**********************************");
            foreach (wordStruct ws in top10)
            {
                i++;
                float percentage = ((float)ws.occurrence / (float)parser.getTotal()) * 100;
                Console.WriteLine("{0}. {1}    {2}   {3}%", i, ws.word, ws.occurrence, percentage);
            }
            Console.ReadLine();

        } //main method

    } //class FileParser
} //namespace File

namespace Parser
{
    /***
     * Author: Justin Tang
     * 
     * This class parses a file and stores the unique words and the number of occurrences of the word
     * It also keeps the total number of words in the file as well as determining the top 10 most used words in that file
     * 
     * To do this, the class uses two Lists, one for words and one for occurrences
     * These are parallel Lists, index 0 of both Lists correspond to the same attribute, word+occurrence combination
     * */
    public class fileParser
    {
        private string filename; //name of file to be parsed
        private List<string> _mostFrequent; //list of the words in the file
        private List<int> _occurrences; //number of times words are used in the file, parallel to _mostFrequent
        private static int total; //total number of words in the file
        private static string word; //word in the file being created through single character parsing

        /*****
         * Constructor for fileParser
         * Takes in a filename for its parameter
         *****/
        public fileParser(string filename)
        {
            this.filename = filename;
            total = 0;
            _mostFrequent = new List<string>();
            _occurrences = new List<int>();
        } //fileParser constructor

        /**********
         * Stores each word in the file in an ArrayList and has a parallel ArrayList for the amount of occurrences of that word
         **********/
        private void parseFile()
        {
            StreamReader sr = new StreamReader(filename);
            word = "";
            int character = sr.Read();
            bool keepGoing = true;
            while (keepGoing)
            {
                if ((character >= 65 && character <= 90) || (character >= 97 && character <= 122))
                    word += (char)character;
                else
                {
                    flush();
                }
                if (sr.Peek() > -1)
                    character = sr.Read();
                else
                {
                    keepGoing = false;
                    flush();
                }
            } //while
            sr.Close();
        } //parseFile

        /********
         * Adds word to word list if it is currently not in the list and adds a 1 to its parallel List _occurrences
         * Increases the count of the word in _occurrences if it is in the word list at its corresponding index
         ********/
        private void flush()
        {
            word = word.ToLower();
            if (word == "") { }
            else if (!_mostFrequent.Contains(word))
            {
                _mostFrequent.Add(word);
                _occurrences.Add(1);
                word = "";
                total++;
            }
            else
            {
                int position = _mostFrequent.IndexOf(word);
                _occurrences[position] = _occurrences[position] + 1;
                word = "";
                total++;
            }
        } //flush

        /*********
         * Gets the 10 most used words in the file
         * Returns a list of wordStructs, which contain the word and occurrences of the word
         * of the top 10 most used words in the file
         *********/
        public List<wordStruct> getTop10()
        {
            parseFile();
            List<wordStruct> top10 = new List<wordStruct>();
            for (int j = 0; j < 10; j++)
            {
                if (_occurrences.Count == 0) break;
                wordStruct ws = new wordStruct();
                int i = 0;
                int y = 0;
                int index = 0;
                while (y < _occurrences.Count)
                {
                    if (_occurrences[y] > i) //finds the largest occurrence
                    {
                        i = _occurrences[y];
                        index = y;
                    }
                    y++;
                }
                ws.word = _mostFrequent[index];
                ws.occurrence = i;
                top10.Add(ws);
                _mostFrequent.RemoveAt(index);
                _occurrences.RemoveAt(index);
            } // for counter 10
            return top10;
        } //getTop10

        /*************
         * Returns the total number of words in the file
         *************/
        public int getTotal()
        {
            return total;
        }
    } //class fileParser

    /******
     * Structure to contain the word and its respective occurrence
     ******/
    public struct wordStruct
    {
        public string word;
        public int occurrence;
    } // struct wordStruct

} // namepsace Parser
