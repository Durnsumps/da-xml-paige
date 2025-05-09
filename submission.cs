﻿using System;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Program
    {
        public static const string xmlURL = "https://durnsumps.github.io/da-xml-paige/Hotels.xml";
        public static const string xmlErrorURL = "https://durnsumps.github.io/da-xml-paige/HotelsErrors.xml";
        public static const string xsdURL = "https://durnsumps.github.io/da-xml-paige/Hotels.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, xsdUrl);

                string errorMessage = "No Error";
                settings.ValidationEventHandler += (object sender, ValidationEventArgs args) =>
                {
                    errorMessage = $"Validation Error: {args.Message}";
                    if (args.Exception != null)
                    {
                        errorMessage += $"\nLine: {args.Exception.LineNumber}, Position: {args.Exception.LinePosition}";
                    }
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings)) {
                    while (reader.Read()) { }
                }
                
                return errorMessage;
            }
            catch (Exception ex)
            {
                return $"Exception occurred during validation: {ex.Message}";
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XDocument doc = XDocument.Load(xmlUrl);

                string jsonText = JsonConvert.SerializeXNode(doc, Formatting.Indented);

                // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. 
                jsonText = jsonText.Replace("\"@Rating\"", "\"_Rating\"")
                              .Replace("\"@NearestAirport\"", "\"_NearestAirport\"");

                JsonConvert.DeserializeXmlNode(jsonText);
                return jsonText;
            }
            catch (Exception ex)
            {
                return $"Error converting XML to JSON: {ex.Message}";
            }
        }
    }

}
