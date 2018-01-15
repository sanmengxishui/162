using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using System.IO;

namespace Common
{
    class FileOperation
    {
        public static bool SaveData(string filePath, string fnodename, string cnodename, string cnodetext)
        {
            bool booladddata = true;
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldel;
            XmlNode root;
            XmlNode node1;
            XmlNode node2;
            if (!File.Exists(filePath))
            {
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
            else
            {
                xml.Load(filePath);
                root = xml.SelectSingleNode("Parameter");
                if (root == null)
                {
                    File.Delete(filePath);
                    booladddata = false;
                    return booladddata;
                }
            }

            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    node2.InnerText = cnodetext;
                }
                else
                {
                    node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                    node2.InnerText = cnodetext;
                    node1.AppendChild(node2);
                }
            }
            else
            {
                node1 = xml.CreateNode(XmlNodeType.Element, fnodename, null);
                root.AppendChild(node1);
                node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                node2.InnerText = cnodetext;
                node1.AppendChild(node2);
            }
            xml.Save(filePath);
            return booladddata;
        }

        public static bool ReadData(string filePath, string fnodename, string cnodename, ref string cnodetext)
        {
            if (!File.Exists(filePath))
            {
                cnodetext = "0";
                return false;
            }

            bool boolreaddata = true;
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            XmlNode node1;
            XmlNode node2;
            XmlNode root = xml.SelectSingleNode("Parameter");
            if (root == null)
            {
                boolreaddata = false;
                return boolreaddata;
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    if (node2.InnerText != "")
                    {
                        cnodetext = node2.InnerText;
                    }
                    else
                    {
                        cnodetext = "0";
                    }
                }
                else
                {
                    cnodetext = "0";
                    boolreaddata = false;
                }
            }
            else
            {
                cnodetext = "0";
                boolreaddata = false;
            }
            return boolreaddata;
        }

        public static List<string> GetfileNames(string Folderpath)
        {
            string filePath = Folderpath;
            List<string> resName = new List<string>();
            string[] names = Directory.GetFiles(filePath, "*.xml").Select(path => Path.GetFileName(path)).ToArray();
            for (int i = 0; i < names.Count(); i++)
            {
                names[i] = names[i].Replace(".xml", "");
                //if (names[i] != FSParamer.machineSettingsFilePath)
                resName.Add(names[i]);
            }
            return resName;
        }

        public static string ExtractFileName(string FilePath)
        {
            return Path.GetFileNameWithoutExtension(FilePath);
        }

    }
}
