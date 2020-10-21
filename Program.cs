using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace tomLehrerBackup
{
    static class Program
    {
        
        static async Task Main(string[] args)
        {
            var wc = new WebClient();
            var lines = wc.DownloadString("https://tomlehrersongs.com/").Split(new char[]{'>'}, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim() + ">");
            var links = lines.Where(l => l.Contains("<a href")).Select(s => s.Substring(s.IndexOf("href=\"") + 6)).Select(s => s.Substring(0,s.IndexOf("\""))).Where(s => s.StartsWith('/'));
            links = links.Take(links.Count() - 1)
            //.Take(3)
            ;
            foreach(var link in links)
            {
                var songHtml = wc.DownloadString("https://tomlehrersongs.com" + link).Split(new char[]{'>'}, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim() + ">");
                var songDir = link.Replace("/", "");
                Directory.CreateDirectory(songDir);
                var songLinks = songHtml.Where(l => l.Contains("<a href")).Select(s => s.Substring(s.IndexOf("href=\"") + 6)).Select(s => s.Substring(0,s.IndexOf("\""))).Where(s => s.StartsWith('/')).Where(s => Regex.Match(s, "\\.[A-Za-z0-9]+").Success);
                foreach(var songLink in songLinks)
                {
                    var songPdfLink ="https://tomlehrersongs.com" + songLink;
                    var songFileLoc = songDir + "\\" + songLink.Split('/').Last();
                    Console.WriteLine(songPdfLink);
                    Console.WriteLine(songFileLoc);
                    wc.DownloadFile(songPdfLink, songFileLoc);
                }
            }
          
        }
    }
}
