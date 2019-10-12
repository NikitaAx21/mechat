using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Microsoft.Win32;
using System.Xml;
using System.Xml.Linq;
using System.Windows;

namespace Chain
{
    public class ListManager
    {
        public ListManager(List<Object> list, Panel panel)
        {
            _list = list;
            _panel = panel;
        }

        private readonly List<Object> _list;
        private readonly Panel _panel;

        public void Add()
        {
            var isNeedToCreateSegment = _list.Count != 0 && _list.Last() is Joint;
            var obj = isNeedToCreateSegment ? (Object) new Segment() : new Joint();

            obj.Id = _list.Count;
            obj.OnSelectedChanged += Select;
            _list.Add(obj);
        }

        private void Select(Object obj)
        {
            _panel.SelectedObject = obj;
        }

        public void Delete(int id)
        {
            _list.RemoveAll(o => o.Id >= id);
        }

        string path = "";

        public void Load(List<Object> ChainList)//из файла
        {
            OpenFileDialog fileChoose = new OpenFileDialog();
            if (fileChoose.ShowDialog() == true)
            {
                if (fileChoose.FileName.Split('.')[fileChoose.FileName.Split('.').Length - 1].ToLower() == "xml")
                    path = fileChoose.FileName;
                else
                    MessageBox.Show("Выберите файл с расширением \".xml\".");//
            }

            if (!String.IsNullOrEmpty(path))
            {
                XmlDocument dataXml = new XmlDocument();
                try
                {
                    dataXml.Load(path);

                    XmlElement xRoot = dataXml.DocumentElement;//SourceData
                    XmlNode xNode = xRoot.FirstChild;//Object
                    try
                    {
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            Object Obj= new Joint();

                            if (node.Name == "Joint")
                            {
                                Obj = new Joint();
                            }
                            if (node.Name == "Segment")
                            {
                                Obj = new Segment();
                            }
                            if ((node.Name != "Segment")&&(node.Name != "Joint")) //=================================================================exeption?
                            {
                                throw new Exception("Некорректное содержимое файла");
                            }


                            Type myClassType = Obj.GetType();
                            PropertyInfo[] properties = myClassType.GetProperties();

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
                            }
                            ChainList.Add(Obj);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Некорректное содержимое файла.");//
                    }
                }
                catch
                {
                    MessageBox.Show("Не удалось загрузить файл.");//
                }
            }
        }

        public void Save(List<Object> ChainList)//в файл
        {

            XDocument xdoc = new XDocument();//создаём документ

            XElement FirstElement = new XElement("Objects");// создаем первый элемент

            try
            {
                for (int i = 0; i < ChainList.Count; i++)
                {

                    Object Obj;

                    Obj = ChainList[i] as Joint;
                    string tag = "Joint";
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

                        if (property.PropertyType.Name == "Boolean" || property.PropertyType.Name == "Double")///???|| property.PropertyType.Name == "Point"
                        {

                            XAttribute Attrib = new XAttribute(property.Name, property.GetValue(Obj, null));
                            Element.Add(Attrib);

                        }
                    }

                    FirstElement.Add(Element);

                }
            }
            catch
            {
                MessageBox.Show("Ошибка исходных данных.");//
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
            catch
            {
                MessageBox.Show("Не удалось сохранить/перезаписать файл.");//
            }
        }
    }
}