using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XmlToJsonConverter
{
    public class SchemaAwareJsonConverter
    {

        /// <summary>
        /// Callback required when loading an XmlSchema.
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="args">ValidationEventArgs</param>
        public static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            //if (args.Severity == XmlSeverityType.Warning)
            //    Console.Write("WARNING: ");
            //else if (args.Severity == XmlSeverityType.Error)
            //    Console.Write("ERROR: ");

            //Console.WriteLine(args.Message);
        }

        /// <summary>
        /// Convert a given XML document to JSON, respecting the rules of the provided XmlSchema.
        /// </summary>
        /// <param name="xml">XmlDocument</param>
        /// <param name="xsd">XmlSchema</param>
        /// <returns>JSON string</returns>
        public string Convert(XmlDocument xml, XmlSchema xsd)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            if (xsd == null)
            {
                throw new ArgumentNullException("xsd");
            }

            // Ensure that the schema is added to the xml document.
            xml.Schemas.Add(xsd);
            xml.Schemas.Compile();
            xml.Validate(ValidationCallback);

            // Convert the xml to json using the custom XmlNodeConverter.
            return JsonConvert.SerializeObject(xml, Newtonsoft.Json.Formatting.None, new XmlToJsonConverter.SchemaAwareXmlNodeConverter());
        }

        /// <summary>
        /// Convert a given XML text to JSON, respecting the rules of the provided XML schema text.
        /// </summary>
        /// <param name="xml">XML document as string</param>
        /// <param name="xsd">XML schema as string</param>
        /// <returns>JSON string</returns>
        public string Convert(string xml, string xsd)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            if (xsd == null)
            {
                throw new ArgumentNullException("xsd");
            }

            // Load the xml into an XmlDocument type.
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // Load the xsd into an XmlSchema type.
            using (var xmlReader = new XmlTextReader(new StringReader(xsd)))
            {
                var xmlSchema = XmlSchema.Read(xmlReader, ValidationCallback);
                return Convert(xmlDoc, xmlSchema);
            }
        }

        /// <summary>
        /// Convert a given XML file to JSON, respecting the rules of the provided XML schema file.
        /// </summary>
        /// <param name="xml">FileInfo object containing the path to the XML document</param>
        /// <param name="xsd">FileInfo object containing the path to the XML schema</param>
        /// <returns>JSON string</returns>
        public string Convert(FileInfo xmlFile, FileInfo xsdFile)
        {
            if (xmlFile == null)
            {
                throw new ArgumentNullException("xmlFile");
            }

            if (xsdFile == null)
            {
                throw new ArgumentNullException("xsdFile");
            }

            // Load the xml into an XmlDocument type.
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile.FullName);

            // Load the xsd into an XmlSchema type.
            using (var xmlReader = new XmlTextReader(xsdFile.FullName))
            {
                var xmlSchema = XmlSchema.Read(xmlReader, ValidationCallback);
                return Convert(xmlDoc, xmlSchema);
            }
        }

    }
}
