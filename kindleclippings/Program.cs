using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace kindleclippings
{
    internal static class Program
    {
#pragma warning disable RCS1163 // Unused parameter.
        static void Main(string[] args)
#pragma warning restore RCS1163 // Unused parameter.
        {
            (string title, string author) ParseTitleLine(string line)
            {
                return (title: line.Substring(0, line.LastIndexOf("(")), line.Substring(line.LastIndexOf("(")));
            }

            using (var file = new System.IO.StreamReader(@"test.txt"))
            {
                List<Clipping> clippings = new List<Clipping>();

                string line;
                int lineCount = 0;

                var clipping = new Clipping();

                while ((line = file.ReadLine()) != null)
                {
                    lineCount++;

                    if (lineCount % 5 == 0)
                    {
                        clippings.Add(clipping);
                        clipping = new Clipping();
                    }
                    try
                    {
                        switch (lineCount % 5)
                        {
                            case 1:
                                (clipping.Title, clipping.Author) = ParseTitleLine(line);
                                break;
                            case 2:
                                var val = line.Split('|').Last();
                                clipping.HighlightDate = DateTime.Parse(val.Replace("Added on", "").Trim());
                                break;
                            case 3:
                                // Ln 3 is newline
                                continue;
                            case 4:
                                clipping.Quote = line.Trim();
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Failed to parse line {lineCount}");
                    }
                }

                using (StreamWriter outputFile = new StreamWriter("quotes.json"))
                {
                    outputFile.WriteLine(JsonConvert.SerializeObject(clippings));
                }
            }
        }
    }
}
