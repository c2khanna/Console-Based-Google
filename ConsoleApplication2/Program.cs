using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace ConsoleApplication2
{
    public class SearchResult
    {
        public string url;
        public string head;
    }
 
    class Program
    {
        static string getQuery()
        {
            string query;
            Console.WriteLine("what do you want google to do?");
            query = Console.ReadLine();
            return query;
        }

        static string executeGoogleSearch(string query)
        {
            string url = "http://www.google.co.in/search?q=";
            string queryEncoded = Uri.EscapeDataString(query);
            string finalurl = url + queryEncoded;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalurl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(resStream, Encoding.UTF8);

            Console.WriteLine("Response stream received.");

            return readStream.ReadToEnd();
        }

        public static List<SearchResult> parseQueryResult(string htmlData)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlData);

            List<SearchResult> Results = new List<SearchResult>();

            var divs = htmlDoc.DocumentNode.SelectNodes("//h3/a");
            foreach (var div in divs)
            {
                string head = null, url = null;
                if (div != null)
                {
                    head = div.InnerText;
                }

                if (div.Attributes["href"] != null)
                {
                    url = div.Attributes["href"].Value;
                }
                Results.Add(new SearchResult { url = url, head = head });
            }
            return Results;
        }

        static void DisplayResults(List<SearchResult> Results)
        {
            foreach(var Result in Results)
            {
                Console.WriteLine("{0} \n,{1}", Result.head , Result.url);
            }
        }

        static void Main(string[] args)
        {
            List<SearchResult> Result = new List<SearchResult>();

            string query = getQuery();
            string htmlData = executeGoogleSearch(query);
            Result = parseQueryResult(htmlData);
            DisplayResults(Result);
            Console.Read();
        }
    }
}
