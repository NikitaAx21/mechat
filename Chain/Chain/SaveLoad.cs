using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.Xml;
using System.Xml.Linq;

using System.Reflection;//

namespace Chain
{
    class SaveLoad
    {


        //List<Object> ChainList;//-------------------------------------------------------------------------------------
        string path = "";

        public void Load(List<Object> ChainList)//из файла
        {
            /*OpenFileDialog fileChoose = new OpenFileDialog();
            if (fileChoose.ShowDialog() == true)
            {
                if (fileChoose.FileName.Split('.')[fileChoose.FileName.Split('.').Length - 1].ToLower() == "xml")
                    path = fileChoose.FileName;
                else
                    MessageBox.Show("Выберите файл с расширением \".xml\".");
            }*/


            if (path != "")
            {
                //Objects = new List<Object>();


                XmlDocument dataXml = new XmlDocument();
                dataXml.Load(path);

                XmlElement xRoot = dataXml.DocumentElement;//SourceData
                XmlNode xNode = xRoot.FirstChild;//Object

                int i = 0;

                while (i < xNode.ChildNodes.Count)
                {
                    if (xNode.ChildNodes[i].Name == "Joint")
                    {
                        Joint J = new Joint();

                        Type myClassType = J.GetType();
                        PropertyInfo[] properties = myClassType.GetProperties();

                        int j = 0;
                        foreach (PropertyInfo property in properties)
                        {
                            //if (property.Name == "") { } else { }
                            if (property.Name == xNode.ChildNodes[i].Attributes[j].Name)//=============================================================exeption?
                            {

                                string type = property.PropertyType.Name;
                                switch (type)
                                {
                                    case "Double":
                                        var attr1 = double.Parse(xNode.ChildNodes[i].Attributes[j].Value);
                                        property.SetValue(J, attr1);
                                        break;
                                    case "Boolean":
                                        var attr2 = bool.Parse(xNode.ChildNodes[i].Attributes[j].Value);
                                        property.SetValue(J, attr2);
                                        break;
                                    case "ssdsdsd"://=================================================================
                                        var attr3 = double.Parse(xNode.ChildNodes[i].Attributes[j].Value);
                                        property.SetValue(J, attr3);
                                        break;
                                }
                            }
                            j++;
                        }

                        ChainList.Add(J);

                    }
                    if (xNode.ChildNodes[i].Name == "Segment")
                    {
                        Segment S = new Segment();

                        Type myClassType = S.GetType();
                        PropertyInfo[] properties = myClassType.GetProperties();

                        int j = 0;
                        foreach (PropertyInfo property in properties)
                        {
                            //if (property.Name == "") { } else { }
                            if (property.Name == xNode.ChildNodes[i].Attributes[j].Name)//=============================================================exeption?
                            {

                                string type = property.PropertyType.Name;
                                switch (type)
                                {
                                    case "Double":
                                        var attr1 = double.Parse(xNode.ChildNodes[i].Attributes[j].Value);
                                        property.SetValue(S, attr1);
                                        break;
                                    case "Boolean":
                                        var attr2 = bool.Parse(xNode.ChildNodes[i].Attributes[j].Value);
                                        property.SetValue(S, attr2);
                                        break;
                                    case "ssdsdsd"://=================================================================
                                        var attr3 = double.Parse(xNode.ChildNodes[i].Attributes[j].Value);
                                        property.SetValue(S, attr3);
                                        break;
                                }
                            }
                            j++;
                        }

                        ChainList.Add(S);
                    }
                    i++;
                }
            }
        }


        public void Save(List<Object> ChainList)//в файл
        {

            XDocument xdoc = new XDocument();//создаём документ

            XElement FirstElement = new XElement("Objects");// создаем первый элемент

            for (int i = 0; i < ChainList.Count; i++)
            {
                var J = ChainList[i] as Joint;
                if (J != null)
                {

                    XElement Element = new XElement("Joint");

                    Type myClassType = J.GetType();
                    PropertyInfo[] properties = myClassType.GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        //if (property.Name == "") { } else { }
                        XAttribute Attrib = new XAttribute(property.Name, property.GetValue(J, null));
                        Element.Add(Attrib);
                    }
                    FirstElement.Add(Element);
                }
                else
                {
                    Segment S = ChainList[i] as Segment;

                    XElement Element = new XElement("Segment");

                    Type myClassType = S.GetType();
                    PropertyInfo[] properties = myClassType.GetProperties();

                    foreach (PropertyInfo property in properties)
                    {

                        //if (property.Name == "") { } else { }

                        XAttribute Attrib = new XAttribute(property.Name, property.GetValue(S, null));
                        Element.Add(Attrib);
                    }
                    FirstElement.Add(Element);
                }
            }

            // создаем корневой элемент
            XElement SourceData = new XElement("SourceData");

            // добавляем в корневой элемент
            SourceData.Add(FirstElement);

            // добавляем корневой элемент в документ
            xdoc.Add(SourceData);

            //сохраняем документ
            xdoc.Save("SourceData.xml");
        }
    }
}
