using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Unicode_Converter_plus
{
    internal class Program
    {
        static string ConvertFromUnicodeText(string input)
        {
            string unescapedText = null;
            try
            {
                unescapedText = Regex.Unescape(input);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("\t Некорректный формат");
            }
            return unescapedText.ToString();
        }
        static void ConvertFromUnicodeFile(string filePath)
        {
            try
            {
                string fileContent = File.ReadAllText(filePath);
                string unescapedContent = Regex.Unescape(fileContent);
                File.WriteAllText(filePath, unescapedContent);
                Console.WriteLine("\t Текст преобразован");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\t Произошла ошибка: " + ex.Message);
            }
        }
        static string ConvertToUnicodeText(string text)
        {
            StringBuilder unicodeText = new StringBuilder();

            foreach (char c in text)
            {
                unicodeText.Append("\\u");
                unicodeText.Append(((int)c).ToString("X4"));
            }

            return unicodeText.ToString();
        }
        static string ConvertToUnicodeFile(string text, bool translateAll)
        {
            StringBuilder unicodeText = new StringBuilder();

            foreach (char c in text)
            {
                if (translateAll || (int)c >= 0x0410 && (int)c <= 0x044F)
                {
                    unicodeText.Append("\\u");
                    unicodeText.Append(((int)c).ToString("X4"));
                }
                else
                {
                    unicodeText.Append(c);
                }
            }
            return unicodeText.ToString();
        }
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
        mainMenu:
            Console.Clear();
            Console.WriteLine("\tMenu");
            Console.WriteLine("Attention: files may be corrupted, create backups before converting");
            Console.WriteLine(" 1 - convert from unicode" + 
                                "\n 2 - convert to unicode");

            ConsoleKeyInfo choice;
            choice = Console.ReadKey(true);

            switch (choice.KeyChar)
            {
                //Конвертация из юникода
                case '1':
                    {

                        Console.Clear();
                        Console.WriteLine("\tConvert from Unicode ");
                        Console.WriteLine(" 1 - convert from file" +
                                            "\n 2 - convert from entered text" +
                                            "\n 3 - back to menu");
                        choice = Console.ReadKey(true);
                        switch (choice.KeyChar)
                        {
                            
                            case '1':
                                //Конвертация текста в юникод
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tConvert from Unicode ");
                                ConvertFromUnicodeFile:
                                    Console.WriteLine("\nEnter file path to convert all unicode in it");
                                    ConvertFromUnicodeFile(Console.ReadLine());

                                    Console.WriteLine("\n 1 - convert again " +
                                                       "\n 2 - back to menu");

                                    choice = Console.ReadKey(true);

                                    switch (choice.KeyChar)
                                    {
                                        case '1':
                                            Console.WriteLine();
                                            goto ConvertFromUnicodeFile;
                                        case '2':
                                            goto mainMenu;
                                    }
                                }
                                break;
                            case '2':
                                //Конвертация текста в юникод
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tConvert from unicode ");
                                ConvertFromUnicodeText:
                                    Console.WriteLine("Enter unicode text");
                                    Console.WriteLine("\nConverted: " +
                                                        "\n" + ConvertFromUnicodeText(Console.ReadLine()));

                                    Console.WriteLine("\n 1 - convert again " +
                                                        "\n 2 - back to menu");

                                    choice = Console.ReadKey(true);

                                    switch (choice.KeyChar)
                                    {
                                        case '1':
                                            Console.WriteLine();
                                            goto ConvertFromUnicodeText;
                                        case '2':
                                            goto mainMenu;
                                    }
                                }
                                break;
                            case '3':
                                goto mainMenu;
                            default: 
                                goto mainMenu;
                        }
                    }
                    break;

                //Конвертация в юникод
                case '2':
                    {
                        Console.Clear();
                        Console.WriteLine("\tConverting to unicode ");
                        Console.WriteLine(" 1 - convert from file" +
                                            "\n 2 - convert from entered text" +
                                            "\n 3 - back to menu");
                        choice = Console.ReadKey(true);
                        switch (choice.KeyChar)
                        {
                            //Конвертация файла в юникод
                            case '1':
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tConvert to unicode ");
                                ConvertToUnicodeFile:
                                    Console.WriteLine("Enter file path");
                                    bool convertEverySymbol;
                                    Console.CursorVisible = true;
                                    string filePath = Console.ReadLine();
                                    Console.CursorVisible = false;
                                    Console.WriteLine("\nConvert only russian characters or convert everything?" +
                                                        "\n 1 - convert only russian characters" +
                                                        "\n 2 - convert everything");
                                    choice = Console.ReadKey(true);
                                    switch (choice.KeyChar)
                                    {
                                        case '1':
                                            convertEverySymbol = false;
                                            break;
                                        case '2':
                                            convertEverySymbol = true;
                                            break;
                                        default:
                                            convertEverySymbol = true;
                                            break;
                                    }
                                    try
                                    {
                                        string[] fileLines = File.ReadAllLines(filePath, Encoding.UTF8);
                                        string[] unicodeLines = new string[fileLines.Length];

                                        for (int i = 0; i < fileLines.Length; i++)
                                        {
                                            unicodeLines[i] = ConvertToUnicodeFile(fileLines[i], convertEverySymbol);
                                        }

                                        File.WriteAllLines(filePath, unicodeLines, Encoding.UTF8);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("\t Error: " + ex.Message);
                                    }
                                    Console.WriteLine("\nConverted");
                                    Console.WriteLine("\n 1 - convert again " +
                                                         "\n 2 - back to menu");

                                    choice = Console.ReadKey(true);

                                    switch (choice.KeyChar)
                                    {
                                        case '1':
                                            Console.WriteLine();
                                            goto ConvertToUnicodeFile;
                                        case '2':
                                            goto mainMenu;
                                    }
                                }
                                break;
                            //Конвертация текста в юникод
                            case '2':
                                {


                                    Console.Clear();
                                    Console.WriteLine("\tConvert to unicode ");
                                ConvertToUnicodeText:

                                    Console.WriteLine("Enter text to convert");
                                    Console.WriteLine("\nConverted: " +
                                                        "\n" + ConvertToUnicodeText(Console.ReadLine()));

                                    Console.WriteLine("\n 1 - convert again " +
                                                        "\n 2 - back to menu");

                                    choice = Console.ReadKey(true);

                                    switch (choice.KeyChar)
                                    {
                                        case '1':
                                            Console.WriteLine();
                                            goto ConvertToUnicodeText;
                                        case '2':
                                            goto mainMenu;
                                    }
                                }
                                break;
                            //Назад
                            case '3':
                                goto mainMenu;

                            default:
                                goto mainMenu;
                        }
                    }
                    
                    break;

                default:
                    return;
            }
        }
    }
}