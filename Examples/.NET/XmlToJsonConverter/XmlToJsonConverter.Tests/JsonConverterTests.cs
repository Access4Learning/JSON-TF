using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace XmlToJsonConverter.Tests
{
    [TestClass]
    public class JsonConverterTests
    {

        [TestMethod]
        public void UsingFilePaths_ConvertXmlToJson_Success()
        {
            // Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;
            string expectedJson = File.ReadAllText("expected-json.txt");

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void UsingStrings_ConvertXmlToJson_Success()
        {
            // Arrange.
            var xml = File.ReadAllText("jsontrans.xml");
            var xsd = File.ReadAllText("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;
            string expectedJson = File.ReadAllText("expected-json.txt");

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void XmlHeader_ConvertXmlToJson_IsNotIncluded()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);
            Assert.IsFalse(json.Contains("?xml"), "?xml property should not be included in JSON");
            Assert.IsFalse(json.Contains("@encoding"), "@encoding should not be included in JSON");
        }

        [TestMethod]
        public void XmlComment_ConvertXmlToJson_IsNotIncluded()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);
            Assert.IsFalse(json.Contains("single instance should be"), "Xml comments should not be included in JSON output.");
        }

        [TestMethod]
        public void RepeatableElement_ConvertXmlToJson_IsJsonArray()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(JObject));

            var value = obj.GetValue("element13") as JArray;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(JArray));
        }

        [TestMethod]
        public void DelimitedArray_ConvertXmlToJson_IsJsonArray()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(JObject));

            var value = obj.GetValue("element2") as JArray;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(JArray));

            Assert.AreEqual(3, value.Count);
        }

        [TestMethod]
        public void ComplexTypeWithAttributes_ConvertXmlToJson_OutputsAttributesFollowedByValue()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(JObject));

            var value = obj.GetValue("element4") as JObject;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(JObject));

            foreach (var child in value.Children())
            {
                Assert.IsNotNull(child);
            }
        }

        [TestMethod]
        public void NumericSimpleType_ConvertXmlToJson_OutputsNumeric()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(JObject));

            var value = obj.GetValue("element5") as JValue;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(JValue));
            Assert.IsTrue(value.Type == JTokenType.Integer);
        }

        [TestMethod]
        public void NumericAttributeValue_ConvertXmlToJson_OutputsNumeric()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(JObject));

            var value = obj.GetValue("element4") as JObject;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(JObject));

            var attrValue = value.GetValue("@attribute2") as JValue;
            Assert.IsTrue(attrValue.Type == JTokenType.Integer);
        }

        [TestMethod]
        public void EmptyArrayElement_ConvertXmlToJson_OutputsEmptyArray()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(JObject));

            var value = obj.GetValue("element13") as JArray;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(JArray));
            Assert.AreEqual(0, value.Count);
        }

        [TestMethod]
        public void EmptyComplexElement_ConvertXmlToJson_OutputsEmptyType()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(JObject));

            var value = obj.GetValue("element14") as JObject;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(JObject));
            Assert.IsFalse(value.HasValues);
        }

        [TestMethod]
        public void NullComplexTypeWithAttribute_ConvertXmlToJson_Success()
        {
            //Arrange.
            var xml = new FileInfo("jsontrans.xml");
            var xsd = new FileInfo("jsontrans.xsd");
            var converter = new SchemaAwareJsonConverter();
            string json = null;

            // Act.
            json = converter.Convert(xml, xsd);

            // Assert.
            Assert.IsNotNull(json);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as JObject;
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(JObject));

            var value = obj.GetValue("element3") as JObject;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(JObject));
        }
    }
}
