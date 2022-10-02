using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreConsoleAppSpringer
{
    internal class Program
    {

        private string folderPath = @"C:\Users\hamzakhanzada\source\repos\CoreConsoleAppSpringer\CoreConsoleAppSpringer\files\";
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Excel formula ="""https://doi.org/"&<cellname>&""","
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            //https://citation.crosscite.org/docs.html (Supported Content Types)
            client.DefaultRequestHeaders.Add("Accept", "application/x-research-info-systems");

            //List of DOI for which a .RIS file is needed
            var input = new List<string>()
            {
                "https://doi.org/10.1007/s10664-019-09756-z",
"https://doi.org/10.1007/s11771-019-4185-5",
"https://doi.org/10.1007/s10664-019-09765-y",
"https://doi.org/10.1007/s10462-021-10031-1",
"https://doi.org/10.1007/s10664-018-9659-9",
"https://doi.org/10.1007/s10664-016-9458-0",
"https://doi.org/10.1007/s11219-016-9350-6",
"https://doi.org/10.1007/s10664-020-09808-9",
"https://doi.org/10.1007/s10664-018-9614-9",
"https://doi.org/10.1007/s11219-016-9344-4",
"https://doi.org/10.1007/s11432-018-9854-3",
"https://doi.org/10.1007/s00607-020-00833-6",
"https://doi.org/10.1007/s11280-014-0303-3",
"https://doi.org/10.1007/s10664-015-9379-3",
"https://doi.org/10.1007/s10664-021-10086-2",
"https://doi.org/10.1007/s12652-016-0394-z",
"https://doi.org/10.1007/s11280-019-00770-1",
"https://doi.org/10.1007/s10664-020-09910-y",
"https://doi.org/10.1007/s10664-021-10009-1",
"https://doi.org/10.1007/s10664-019-09785-8",
"https://doi.org/10.1007/s10664-019-09750-5",
"https://doi.org/10.1007/s10664-015-9417-1",
"https://doi.org/10.1007/s10664-016-9443-7",
"https://doi.org/10.1007/s10270-022-00983-5",
"https://doi.org/10.1057/s41275-017-0075-5",




            };

            var program = new Program();
            await program.GetRISForEachDOI(input);

        }

        private async Task GetRISForEachDOI(List<string> doiIds)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var url in doiIds)
            {
                var filePath = System.IO.Path.Combine(folderPath, $"{GetUniqueFileIdFromUrl(url)}.txt");

                //if the local storage already contains that file then skip calling the API
                if (System.IO.File.Exists(filePath))
                    stringBuilder.AppendLine(System.IO.File.ReadAllText(filePath));

                else
                {
                    Console.WriteLine($"Saving {url}");

                    var responseString = await client.GetStringAsync(url);

                   // Thread.Sleep(TimeSpan.FromSeconds(1));

                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        stringBuilder.AppendLine(responseString);
                        WriteResponseInFile(
                            content: responseString,
                            fileName: $"{GetUniqueFileIdFromUrl(url)}",
                            extension: "txt");
                    }
                }
            }

            WriteResponseInFile(content: stringBuilder.ToString(), fileName: "springer", extension: "ris");
        }

        /// <summary>
        /// Gets the unique file id from a DOI url
        /// </summary>
        /// <param name="url">Example Input: https://doi.org/10.1007/978-3-319-07692-8_30 </param>
        /// <returns>Example Output: 978-3-319-07692-8_30 </returns>
        private string GetUniqueFileIdFromUrl(string url)
        {
            //input => https://doi.org/10.1007/978-3-319-07692-8_30
            //output => 978-3-319-07692-8_30

            var items = url.Split("/");
            return items[items.Length - 1];
        }

        /// <summary>
        /// Used to Create Files on local storage
        /// </summary>
        /// <param name="content">The content of the file</param>
        /// <param name="fileName">The name of the file without extension and including full path</param>
        /// <param name="extension">Extension of the file to create</param>
        private void WriteResponseInFile(string content, string fileName, string extension)
        {
            var filePath = System.IO.Path.Combine(folderPath, $"{fileName}.{extension}");
            System.IO.File.WriteAllText(filePath, content);
        }

    }

}
