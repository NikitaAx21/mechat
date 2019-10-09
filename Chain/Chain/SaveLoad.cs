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


            if (!String.IsNullOrEmpty(path))
            {

                XmlDocument dataXml = new XmlDocument();
                try
                {
                    dataXml.Load(path);
                }
                catch { }//=================================================================exeption?


                XmlElement xRoot = dataXml.DocumentElement;//SourceData
                XmlNode xNode = xRoot.FirstChild;//Object

                foreach (XmlNode node in xNode.ChildNodes)
                {
                    Object Obj;

                    if (node.Name == "Joint")
                    {
                            Obj = new Joint();
                    }
                    if (node.Name == "Segment")
                    {
                        Obj = new Segment();
                    }
                    else //=================================================================exeption?
                    {
                        Obj = new Segment();//???
                    }
                    try
                    {
                       
                        Type myClassType = Obj.GetType();
                        PropertyInfo[] properties = myClassType.GetProperties();

                        //int j = 0;
                        foreach (PropertyInfo property in properties)
                        {
                            foreach (XmlNode attribut in node.Attributes)
                            {
                                if (property.Name == attribut.Name)
                                {
                                    string value_type = property.PropertyType.Name;
                                    switch (value_type)
                                    {
                                        case "Double":
                                            var attr1 = double.Parse(attribut.Value);
                                            property.SetValue(Obj, attr1);
                                            break;
                                        case "Boolean":
                                            var attr2 = bool.Parse(attribut.Value);
                                            property.SetValue(Obj, attr2);
                                            break;
                                        default:

                                            break;//???
                                    }
                                }
                            }

                                /*/if (property.Name == "") { } else { }
                                if (property.Name == node.Attributes[j].Name)//цикл?
                            {

                                string value_type = property.PropertyType.Name;
                                switch (value_type)
                                {
                                    case "Double":
                                        var attr1 = double.Parse(node.Attributes[j].Value);
                                        property.SetValue(Obj, attr1);
                                        break;
                                    case "Boolean":
                                        var attr2 = bool.Parse(node.Attributes[j].Value);
                                        property.SetValue(Obj, attr2);
                                        break;
                                    default:

                                        break;//???
                                }
                            }

                            j++;*/

                        }
                    }
                    catch
                    { }//=============================================================exeption?

                    ChainList.Add(Obj);
                }
            }
        }

       

        public void Save(List<Object> ChainList)//в файл
        {

            XDocument xdoc = new XDocument();//создаём документ

            XElement FirstElement = new XElement("Objects");// создаем первый элемент

            for (int i = 0; i < ChainList.Count; i++)
            {

                Object Obj;

                Obj = ChainList[i] as Joint;
                string tag= "Joint";
                if (Obj == null)
                {
                    Obj = ChainList[i] as Segment;
                    tag = "Segment";
                }

                    XElement Element = new XElement(tag);

                    Type myClassType = Obj.GetType();
                    PropertyInfo[] properties = myClassType.GetProperties();

                    foreach (PropertyInfo property in properties)
                    {

                        if (property.PropertyType.Name== "Boolean" || property.PropertyType.Name == "Double")///???
                        {

                            //if (property.Name == "") { } else { } property.PropertyType.Name !=Boolean Double
                            XAttribute Attrib = new XAttribute(property.Name, property.GetValue(Obj, null));
                            Element.Add(Attrib);

                        }
                    }

                    FirstElement.Add(Element);
                
            }

            // создаем корневой элемент
            XElement SourceData = new XElement("SourceData");

            // добавляем в корневой элемент
            SourceData.Add(FirstElement);

            // добавляем корневой элемент в документ
            xdoc.Add(SourceData);
            try
            {            
                //сохраняем документ
                xdoc.Save("SourceData.xml");
            }
            catch { }
            
        }
    }
}
