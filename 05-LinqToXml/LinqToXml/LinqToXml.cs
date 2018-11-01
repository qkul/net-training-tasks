﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace LinqToXml
{
    public static class LinqToXml
    {
        /// <summary>
        /// Creates hierarchical data grouped by category
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation (refer to CreateHierarchySourceFile.xml in Resources)</param>
        /// <returns>Xml representation (refer to CreateHierarchyResultFile.xml in Resources)</returns>
        public static string CreateHierarchy(string xmlRepresentation)
        {
            //throw new NotImplementedException();
            var elem = XElement.Parse(xmlRepresentation);      
            var result = new XElement("Root", elem.Elements("Data").
                GroupBy(x => x.Element("Category").Value).
                Select(x =>
                {
                    return new XElement("Group", new XAttribute("ID", x.Key), x);
                }));     
            result.Descendants("Category").Remove();
            return result.ToString();
        }

        /// <summary>
        /// Get list of orders numbers (where shipping state is NY) from xml representation
        /// </summary>
        /// <param name="xmlRepresentation">Orders xml representation (refer to PurchaseOrdersSourceFile.xml in Resources)</param>
        /// <returns>Concatenated orders numbers</returns>
        /// <example>
        /// 99301,99189,99110
        /// </example>
        public static string GetPurchaseOrders(string xmlRepresentation)
        {
            XNamespace aw = "http://www.adventure-works.com";
            return string.Join(",", XElement.Parse(xmlRepresentation)
                .Elements(aw + "PurchaseOrder")
                .Where(x => x.Elements(aw + "Address")
                             .Where(y => y.Attribute(aw + "Type").Value == "Shipping")
                             .ElementAtOrDefault(0)
                             .Element(aw + "State")
                             .Value == "NY").Select(x => (string)x.Attribute(aw + "PurchaseOrderNumber")));
        }

        /// <summary>
        /// Reads csv representation and creates appropriate xml representation
        /// </summary>
        /// <param name="customers">Csv customers representation (refer to XmlFromCsvSourceFile.csv in Resources)</param>
        /// <returns>Xml customers representation (refer to XmlFromCsvResultFile.xml in Resources)</returns>
        public static string ReadCustomersFromCsv(string customers)
        {
            var customer = new XElement("Root");
            foreach (var item in Regex.Split(customers, "\r\n").Select(c => c.Split(',')))
            {
                customer.Add(new XElement("Customer",
                    new XAttribute("CustomerID", item[0]),
                    new XElement("CompanyName", item[1]),
                    new XElement("ContactName", item[2]),
                    new XElement("ContactTitle", item[3]),
                    new XElement("Phone", item[4]),
                    new XElement("FullAddress",
                        new XElement("Address", item[5]),
                        new XElement("City", item[6]),
                        new XElement("Region", item[7]),
                        new XElement("PostalCode", item[8]),
                        new XElement("Country", item[9])
                        )
                    ));
            }
            return customer.ToString();
        }
        /// <summary>
        /// Gets recursive concatenation of elements
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation of document with Sentence, Word and Punctuation elements. (refer to ConcatenationStringSource.xml in Resources)</param>
        /// <returns>Concatenation of all this element values.</returns>
        public static string GetConcatenationString(string xmlRepresentation)
        {
            return XDocument.Parse(xmlRepresentation).Root.Value;
        }

        /// <summary>
        /// Replaces all "customer" elements with "contact" elements with the same childs
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with customers (refer to ReplaceCustomersWithContactsSource.xml in Resources)</param>
        /// <returns>Xml representation with contacts (refer to ReplaceCustomersWithContactsResult.xml in Resources)</returns>
        public static string ReplaceAllCustomersWithContacts(string xmlRepresentation)
        {
            var elem = XElement.Parse(xmlRepresentation);
            var result = new XElement("Document", elem.Elements("customer").Select((x) => 
            {
                x.Name = "contact";
                return x;
            }));
            return result.ToString();
        }

        /// <summary>
        /// Finds all ids for channels with 2 or more subscribers and mark the "DELETE" comment
        /// </summary>
        /// <param name="xmlRepresentation">Xml representation with channels (refer to FindAllChannelsIdsSource.xml in Resources)</param>
        /// <returns>Sequence of channels ids</returns>
        public static IEnumerable<int> FindChannelsIds(string xmlRepresentation)
        {
            var elem = XElement.Parse(xmlRepresentation);
            return elem.Descendants("channel").Where(x => x.Elements("subscriber").Count() >= 2 &&
                  x.DescendantNodes().OfType<XComment>().
                  Any(y => y.Value == "DELETE")).
            Select(x => Convert.ToInt32(x.Attribute("id").Value));
        }

        /// <summary>
        /// Sort customers in docement by Country and City
        /// </summary>
        /// <param name="xmlRepresentation">Customers xml representation (refer to GeneralCustomersSourceFile.xml in Resources)</param>
        /// <returns>Sorted customers representation (refer to GeneralCustomersResultFile.xml in Resources)</returns>
        public static string SortCustomers(string xmlRepresentation)
        {
            var elem = XElement.Parse(xmlRepresentation);
            var result = new XElement("Root", elem.Descendants("Customers").
                OrderBy(x => x.Descendants("Country").First().Value + 
                             x.Descendants("City").First().Value).
               ToArray());
            System.Diagnostics.Trace.WriteLine(result.ToString());
            return result.ToString();
        }

        /// <summary>
        /// Gets XElement flatten string representation to save memory
        /// </summary>
        /// <param name="xmlRepresentation">XElement object</param>
        /// <returns>Flatten string representation</returns>
        /// <example>
        ///     <root><element>something</element></root>
        /// </example>
        public static string GetFlattenString(XElement xmlRepresentation)
        {
            var strXml = xmlRepresentation.ToString();
            var res = string.Empty;
            var unnecessaryElements = new[] { ' ', '\t', '\r', '\n', '\\' };

            for (int i = 0; i < strXml.Length; i++)
            {
                if (!(unnecessaryElements.Contains(strXml[i])))
                    res += strXml[i];            
            }
            return res;
        }

        /// <summary>
        /// Gets total value of orders by calculating products value
        /// </summary>
        /// <param name="xmlRepresentation">Orders and products xml representation (refer to GeneralOrdersFileSource.xml in Resources)</param>
        /// <returns>Total purchase value</returns>
        public static int GetOrdersValue(string xmlRepresentation)
        {
            var document = XDocument.Parse(xmlRepresentation);
            int sumValue = 0;           
            var root = document.Element("Root");
            foreach (var order in root.Element("Orders").Elements("Order"))
            {
                foreach (var product in root.Element("products").Descendants())
                {
                    if (product.Attribute("Id").Value == order.Element("product").Value)
                    {
                        sumValue += Convert.ToInt32(product.Attribute("Value").Value);
                        break;
                    }
                }
            }
            return sumValue;
        }
    }
}