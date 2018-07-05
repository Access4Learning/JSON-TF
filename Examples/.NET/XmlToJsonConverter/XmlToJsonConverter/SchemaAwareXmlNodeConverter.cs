using Newtonsoft.Json;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace XmlToJsonConverter
{
    /// <summary>
    /// Class that overrides the Newtonsoft implementation of converting XML to JSON
    /// so that the desired JSON format can be achieved.
    /// </summary>
    public class SchemaAwareXmlNodeConverter : Newtonsoft.Json.Converters.XmlNodeConverter
    {
        /// <summary>
        /// Entry point to start the JSON serialization process.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var xmlNode = value as XmlDocument;

            writer.WriteStartObject();
            WriteXmlNodeToJson(writer, xmlNode, serializer);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Convert an Xml Node to JSON.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="node"></param>
        /// <param name="serializer"></param>
        public virtual void WriteXmlNodeToJson(JsonWriter writer, XmlNode node, JsonSerializer serializer)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Document:

                    // Loop through each child node of the document.
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        WriteXmlNodeToJson(writer, childNode, serializer);
                    }

                    break;

                case XmlNodeType.Element:

                    // Element conversion.

                    // Handle the document element differently.
                    if (node.OwnerDocument.DocumentElement == node)
                    {
                        foreach (XmlAttribute attribute in node.Attributes)
                        {
                            WriteXmlAttributeToJson(writer, attribute, serializer);
                        }

                        // Loop through each child node of the document.
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            WriteXmlNodeToJson(writer, childNode, serializer);
                        }
                        // writer.WriteEnd();
                    }
                    else
                    {
                        // Output element.
                        WriteXmlElementToJson(writer, node, serializer);
                    }

                    break;

                case XmlNodeType.XmlDeclaration:

                    WriteXmlDeclarationToJson(writer, node, serializer);
                    break;

                case XmlNodeType.Comment:

                    WriteXmlCommentToJson(writer, node, serializer);
                    break;

            }
        }

        public virtual void WriteXmlDeclarationToJson(JsonWriter writer, XmlNode node, JsonSerializer serializer)
        {
            // Do nothing.
        }

        public virtual void WriteXmlCommentToJson(JsonWriter writer, XmlNode node, JsonSerializer serializer)
        {
            // Do nothing.
        }

        public virtual void WriteXmlElementToJson(JsonWriter writer, XmlNode node, JsonSerializer serializer)
        {
            writer.WritePropertyName(node.LocalName);

            // Find the schema type.
            if (IsArray(node))
            {
                WriteXmlArrayToJson(writer, node, serializer);
                return;
            }

            // Determine if the element is a complex of simple type.
            var complexType = (IsComplexType(node) || HasChildNodes(node));

            if (complexType)
            {
                WriteXmlComplexTypeToJson(writer, node, serializer);
            }
            else
            {
                WriteXmlSimpleTypeToJson(writer, node, serializer);
            }
        }

        public virtual void WriteXmlSimpleTypeToJson(JsonWriter writer, XmlNode node, JsonSerializer serializer)
        {
            WriteValue(writer, node);
        }

        public virtual void WriteValue(JsonWriter writer, XmlNode node)
        {
            WriteValue(writer, node.InnerText, node.SchemaInfo.SchemaType);
        }

        public virtual void WriteValue(JsonWriter writer, string value, XmlSchemaType schemaType)
        {
            if (schemaType != null && (IsNumber(schemaType) || IsBoolean(schemaType)))
            {
                writer.WriteRawValue(value);
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    writer.WriteRawValue("null");
                }
                else
                {
                    writer.WriteValue(value);
                }
            }
        }

        public virtual void WriteValue(JsonWriter writer, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                writer.WriteRawValue("null");
            }
            else
            {
                writer.WriteValue(value);
            }
        }

        public virtual void WriteXmlComplexTypeToJson(JsonWriter writer, XmlNode node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            // Write out any attributes.
            if (node.Attributes != null && node.Attributes.Count > 0)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    WriteXmlAttributeToJson(writer, attr, serializer);
                }
            }

            XmlSchemaComplexType complexType = node.SchemaInfo.SchemaType as XmlSchemaComplexType;

            if (complexType.ContentType == XmlSchemaContentType.TextOnly)
            {
                writer.WritePropertyName("value");
                WriteValue(writer, node);
            }

            // Loop through each child node of the document.
            foreach (XmlNode childNode in node.ChildNodes)
            {
                WriteXmlNodeToJson(writer, childNode, serializer);
            }

            writer.WriteEndObject();
        }

        public virtual void WriteXmlArrayToJson(JsonWriter writer, XmlNode node, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            if (!string.IsNullOrEmpty(node.InnerText))
            {
                var values = node.InnerText.Split(' ');
                writer.WriteRawValue(string.Format("\"{0}\"", string.Join("\",\"", values)));
            }

            writer.WriteEndArray();
        }

        public virtual void WriteXmlAttributeToJson(JsonWriter writer, XmlAttribute attribute, JsonSerializer serializer)
        {
            if (attribute.Name.StartsWith("xmlns", StringComparison.OrdinalIgnoreCase)
                || attribute.Name == "xsi:nil")
            {
                return;
            }

            writer.WritePropertyName(string.Format("@{0}", attribute.Name));
            WriteValue(writer, attribute.Value, attribute.SchemaInfo.SchemaType);
        }


        public virtual bool IsComplexType(XmlNode node)
        {
            return (node.SchemaInfo.SchemaType.GetType() == typeof(XmlSchemaComplexType));
        }

        public virtual bool IsArray(XmlNode node)
        {
            return (node.SchemaInfo.SchemaType.Datatype != null && node.SchemaInfo.SchemaType.Datatype.ValueType.IsArray)
                || node.SchemaInfo.SchemaElement.MaxOccurs > 1;
        }


        private bool IsNumber(XmlSchemaType schemaType)
        {
            var typeCode = schemaType.TypeCode;
            return (typeCode == XmlTypeCode.Decimal
                || typeCode == XmlTypeCode.Double
                || typeCode == XmlTypeCode.Float
                || typeCode == XmlTypeCode.Int
                || typeCode == XmlTypeCode.Integer
                || typeCode == XmlTypeCode.NegativeInteger
                || typeCode == XmlTypeCode.NonNegativeInteger
                || typeCode == XmlTypeCode.PositiveInteger
                || typeCode == XmlTypeCode.NonPositiveInteger
                || typeCode == XmlTypeCode.Short
                || typeCode == XmlTypeCode.UnsignedInt
                || typeCode == XmlTypeCode.UnsignedLong
                || typeCode == XmlTypeCode.UnsignedShort);
        }

        private bool IsBoolean(XmlSchemaType schemaType)
        {
            var typeCode = schemaType.TypeCode;
            return (typeCode == XmlTypeCode.Boolean);
        }

        public virtual bool HasChildNodes(XmlNode node)
        {
            return node.HasChildNodes && node.ChildNodes.OfType<XmlNode>().Any(x => x.NodeType != XmlNodeType.Text);
        }

    }
}
