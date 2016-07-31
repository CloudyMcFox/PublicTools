using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace iTunesLibrarySelector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) {
                OpenItunes();
                return;
            }
            string input = args[0];
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(input);
            string encodedString = System.Convert.ToBase64String(bytes);
            int i = 0;
            while (i < encodedString.Length) {
                encodedString = encodedString.Insert(i, "\n\t\t");
                i += 60 + 3;
            }

            string prefPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Apple Computer\iTunes\iTunesPrefs.xml";
            //string prefPath =@"c:\temp\iTunesPrefs.xml";
            string prefFile = File.ReadAllText(prefPath);

            string openTag = "\n\t\t<key>iTunes Library Location:1</key>";
            string closeTag = "\n\t\t</data>";

            if (prefFile.Contains(openTag)) {
                int startIndex = prefFile.IndexOf(openTag) + openTag.Length;
                int endIndex = prefFile.IndexOf(closeTag, startIndex);
                prefFile = prefFile.Remove(startIndex, endIndex - startIndex);
                prefFile = prefFile.Insert(startIndex, encodedString);
            } else {
                int startIndex = prefFile.IndexOf("<key>iTunes Library XML Location:1</key>");
                int endIndex = prefFile.IndexOf(closeTag, startIndex);
                endIndex += closeTag.Length;
                prefFile = prefFile.Insert(endIndex, openTag + encodedString + closeTag);
            }

            File.WriteAllText(prefPath, prefFile);
            OpenItunes();
        }
        static void OpenItunes()
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\iTunes\iTunes.exe");
        }
    }
}
