using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Xml;


namespace FrebViewer
{
    public class Files
    {
        public string output;

        public IEnumerable<FrebViewer.Models.Header> GetHeaders(string searchQuery = "")
        {
            List<FrebViewer.Models.Header> headers = new List<FrebViewer.Models.Header>();

            try
            {
                string Verb = "";
                int TimeTaken = 0;
                string URL;
                string AppPoolName;
                int StatusCode;
                int contains = 1;
                bool flag = true;

                if (searchQuery == "")
                    flag = false;



                string fileLocation = WebConfigurationManager.AppSettings["location"];
                FileInfo[] files = new DirectoryInfo(fileLocation).GetFiles("fr*.xml", SearchOption.AllDirectories);
                if (files.Length == 0)
                {
                    //Error
                }
                else
                {
                    FileInfo[] array = files;
                    for (int i = 0; i < array.Length; i++)
                    {
                        FileInfo fileInfo = array[i];
                        if(flag)
                            contains = File.ReadAllText(fileInfo.FullName).ToLower().Contains(searchQuery.ToLower()) ? 1 : 0;

                        if (contains == 1)
                        {
                            this.GetDetailsFromFREBFile(fileInfo.FullName, out URL, out Verb, out AppPoolName, out StatusCode, out TimeTaken);

                            headers.Add(new FrebViewer.Models.Header
                            {
                                FileName = fileInfo.Name,
                                URL = URL,
                                Verb = Verb,
                                AppPoolName = AppPoolName,
                                StatusCode = StatusCode,
                                TimeTaken = TimeTaken
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something bad happened. Please report it to rajrang@microsoft.com. Message : " + ex.Message + " Stack : " + ex.StackTrace);
            }

            return headers;
        }

        public string GetFileContent(string filename)
        {
            string fileLocation = WebConfigurationManager.AppSettings["location"];
            string[] filePaths = Directory.GetFiles(fileLocation, filename, SearchOption.AllDirectories);

            string inputXml = File.ReadAllText(filePaths[0]);
            inputXml = inputXml.Replace("freb.xsl", "GetFile?filename=freb.xsl");

            return inputXml.ToString();
        }

        private void GetDetailsFromFREBFile(string p, out string url, out string verb, out string appPool, out int statusCode, out int timeTaken)
        {
            string text;
            verb = (text = "");
            string text2;
            appPool = (text2 = text);
            url = text2;
            statusCode = (timeTaken = 0);
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(p);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("failedRequest");
                url = xmlNode.Attributes["url"].Value;
                verb = xmlNode.Attributes["verb"].Value;
                appPool = xmlNode.Attributes["appPoolId"].Value;
                timeTaken = int.Parse(xmlNode.Attributes["timeTaken"].Value);
                statusCode = int.Parse(xmlNode.Attributes["statusCode"].Value.Split(new char[]
				{
					'.'
				})[0]);
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.GetType().ToString() == "System.Xml.XmlException")
                    {
                        string text3 = " ";
                        string text4 = " ";
                        string text5 = " ";
                        TextReader textReader = new StreamReader(p);
                        while (!text3.Contains("failedRequest url"))
                        {
                            text3 = textReader.ReadLine();
                            if (text3 == null || text3.Contains("xmlns:freb="))
                            {
                                break;
                            }
                        }
                        text3 = text3.Substring(20).Replace('"', ' ');
                        while (!appPool.Contains("appPoolId="))
                        {
                            appPool = textReader.ReadLine();
                            if (appPool.Contains("xmlns:freb="))
                            {
                                break;
                            }
                        }
                        appPool = appPool.Substring(27).Replace('"', ' ').Trim();
                        while (!verb.Contains("verb="))
                        {
                            verb = textReader.ReadLine();
                            if (verb.Contains("xmlns:freb="))
                            {
                                break;
                            }
                        }
                        verb = verb.Substring(20).Replace('"', ' ').Trim();
                        while (!text4.Contains("statusCode="))
                        {
                            text4 = textReader.ReadLine();
                            if (text4.Contains("xmlns:freb="))
                            {
                                break;
                            }
                        }
                        text4 = text4.Substring(27).Replace('"', ' ');
                        while (!text5.Contains("timeTaken="))
                        {
                            text5 = textReader.ReadLine();
                            if (text5.Contains("xmlns:freb="))
                            {
                                break;
                            }
                        }
                        text5 = text5.Substring(25).Replace('"', ' ');
                        url = text3.Trim();
                        statusCode = int.Parse(text4.Trim());
                        timeTaken = int.Parse(text5.Trim());
                    }
                    else
                    {
                        throw new Exception("Something bad happened. Please report it to rajrang@microsoft.com. Message : " + ex.Message + " Stack : " + ex.StackTrace);
                    }
                }
                catch (Exception ex2)
                {
                    throw new Exception("Something bad happened. Please report it to rajrang@microsoft.com. Message : " + ex2.Message + " Stack : " + ex2.StackTrace);
                }
            }
        }
    }
}