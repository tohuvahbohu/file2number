using System;
using System.IO;
using System.Text;

namespace file2number
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args[0] != "/?" || args[0] != "-?")
            {
                try
                {
                    byte[] buffer;

                    // Check the source file path
                    CheckUri(args[0]);
                    if (!File.Exists(args[0]))
                    {
                        throw new FileNotFoundException();
                    }
                    FileStream fileStream = new FileStream(args[0], FileMode.Open, FileAccess.Read);
                    var destinationUriFilePath = "";
                    if (args.Length > 1)
                    {
                        // Check the destination file path
                        CheckUri(args[1]);
                        destinationUriFilePath = args[1];
                    }
                    else
                    {
                        destinationUriFilePath = Directory.GetCurrentDirectory() + "\\" + "output.txt";
                    }
                    var outputFile = File.CreateText(destinationUriFilePath);
                    int length = (int)fileStream.Length;
                    buffer = new byte[length];
                    int count;
                    int sum = 0;

                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    {
                        sum += count;
                        var convertedToString = Encoding.UTF8.GetString(buffer);
                        foreach (var c in convertedToString)
                        {
                            var number = Convert.ToInt32(c);
                            outputFile.Write(number.ToString());
                        }
                    }
                    fileStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex);
                }
            }
            else
            {
                Console.WriteLine("\nUsage: file2number <path to file to convert>\nExample: wav2txt C:\\Users\\jsmith\\Documents\\sound.wav");
            }
        }

        public static void CheckUri(string uriPath)
        {
            if (!Uri.IsWellFormedUriString(uriPath, UriKind.RelativeOrAbsolute))
            {
                throw new MalformedUriException("Error: Cannot parse argument 0 into a Uri string\nArgument must be a valid absolute or relative path");
            }
        }
    }

    public class MalformedUriException : Exception
    {
        public MalformedUriException()
        {
        }

        public MalformedUriException(string message) : base(message)
        {
        }

        public MalformedUriException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
