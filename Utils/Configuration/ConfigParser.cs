using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NLog;

namespace Utils
{
    public static class ConfigParser
    {
        public static Config Parse(string filePath, Logger log)
        {
            if (!File.Exists(filePath))
                log.Fatal("Configuration file doesn't exist: " +
                    Path.GetFullPath(filePath),
                    new FileNotFoundException());

            string fileContent = File.ReadAllText(filePath);
            XDocument doc = XDocument.Parse(fileContent);

            //Expected config keys
            string ipAddressKey = "IPAddress";
            string portKey = "Port";
            string genSettingsKey = "GeneratorSettings";
            string minValueKey = "MinValue";
            string maxValueKey = "MaxValue";

            //Check configuration file
            string[][] names = new string[][]
            {
                new string[] { ipAddressKey },
                new string[] { portKey },
                new string[] { genSettingsKey, minValueKey },
                new string[] { genSettingsKey, maxValueKey }
            };
            CheckXmlNodes(doc.Root, names, log);
            
            //Retrieve xml elements
            string ipAddress = doc.Root.Element(ipAddressKey).Value;
            if (!int.TryParse(doc.Root.Element(portKey).Value, out int port))
                log.Fatal(new Exception(), "Invalid port value in configuration file");
            if (!double.TryParse(doc.Root.Element(genSettingsKey).Element(minValueKey).Value, 
                out double minValue))
                log.Fatal(new Exception(), "Cannot parse minimum generator value from configuration file");
            if (!double.TryParse(doc.Root.Element(genSettingsKey).Element(maxValueKey).Value,
                out double maxValue))
                log.Fatal(new Exception(), "Cannot parse maximum generator value from configuration file");

            return new Config(ipAddress, port, minValue, maxValue);
        }

        private static void CheckXmlNodes(XElement root, string[][] names, Logger log)
        {
            XElement currentElement;
            foreach (string[] currentNames in names)
            {
                currentElement = root;
                foreach (string name in currentNames)
                {
                    if (!currentElement.Elements(name).Any())
                        log.Fatal(new Exception(), 
                            "Required XmlPath not found in configuration file: " + name);
                    currentElement = currentElement.Element(name);
                }
            }
        }
    }
}
