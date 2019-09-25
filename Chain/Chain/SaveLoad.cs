using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.Xml;
using System.Xml.Linq;
namespace Chain
{
    class SaveLoad
    {


        List<Object> Objects;//-------------------------------------------------------------------------------------
        string path = "";

        public void Save()
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
                Objects = new List<Object>();


                XmlDocument dataXml = new XmlDocument();
                dataXml.Load(path);

                XmlElement xRoot = dataXml.DocumentElement;//SourceData
                XmlNode xNode = xRoot.FirstChild;//Object

                int i = 0;

                while (i<xNode.ChildNodes.Count)
                {
                    if (xNode.ChildNodes[i].Name == "Joint")
                    {
                        Joint J = new Joint();

                        //piket2 = xNode.ChildNodes[i].ChildNodes.Count;

                        J.IsMassCenterVisible = bool.Parse(xNode.ChildNodes[i].Attributes[0].Value);

                        J.Mass = double.Parse(xNode.ChildNodes[i].Attributes[1].Value);

                        J.CurrentAngle = double.Parse(xNode.ChildNodes[i].Attributes[2].Value);

                        J.IsAngleRestricted = bool.Parse(xNode.ChildNodes[i].Attributes[3].Value);

                        J.AngleRestrictionLeft = double.Parse(xNode.ChildNodes[i].Attributes[4].Value);

                        J.AngleRestrictionRight = double.Parse(xNode.ChildNodes[i].Attributes[5].Value);

                        Objects.Add(J);

                    }
                    if (xNode.ChildNodes[i].Name == "Segment")
                    {
                        Segment S = new Segment();

                        //piket2 = xNode.ChildNodes[i].ChildNodes.Count;

                        S.IsMassCenterVisible = bool.Parse(xNode.ChildNodes[i].Attributes[0].Value);

                        S.Mass = double.Parse(xNode.ChildNodes[i].Attributes[1].Value);

                        S.Vector.X = double.Parse(xNode.ChildNodes[i].Attributes[2].Value);

                        S.Vector.Y = double.Parse(xNode.ChildNodes[i].Attributes[3].Value);

                        S.Visibility = bool.Parse(xNode.ChildNodes[i].Attributes[4].Value);

                        S.Efemerik = bool.Parse(xNode.ChildNodes[i].Attributes[5].Value);

                        Objects.Add(S);

                    }
                    i++;
                }
            }
        }


        public void Load()
        {

            XDocument xdoc = new XDocument();

            // создаем первый элемент
            XElement FirstElement = new XElement("Objects");

            for (int i = 0; i < Objects.Count; i++)
            {
                var J = Objects[i] as Joint;
                if (J != null)
                {

                    XElement Element = new XElement("Joint");
                    
                    // создаем атрибут
                    XAttribute IsMassCenterVisible = new XAttribute("IsMassCenterVisible", J.IsMassCenterVisible);
                    XAttribute Mass = new XAttribute("Mass", J.Mass);
                    XAttribute CurrentAngle = new XAttribute("CurrentAngle", J.CurrentAngle);
                    XAttribute IsAngleRestricted = new XAttribute("IsAngleRestricted", J.IsAngleRestricted);
                    XAttribute AngleRestrictionLeft = new XAttribute("AngleRestrictionLeft", J.AngleRestrictionLeft);
                    XAttribute AngleRestrictionRight = new XAttribute("AngleRestrictionRight", J.AngleRestrictionRight);


                    // добавляем атрибут и элементы в первый элемент
                    Element.Add(IsMassCenterVisible);
                    Element.Add(Mass);
                    Element.Add(CurrentAngle);
                    Element.Add(IsAngleRestricted);
                    Element.Add(AngleRestrictionLeft);
                    Element.Add(AngleRestrictionRight);

                    FirstElement.Add(Element);

                }
                else
                {
                    Segment S = Objects[i] as Segment;

                    XElement Element = new XElement("Segment");

                    // создаем атрибут
                    XAttribute IsMassCenterVisible = new XAttribute("IsMassCenterVisible", S.IsMassCenterVisible);
                    XAttribute Mass = new XAttribute("Mass", S.Mass);
                    XAttribute X = new XAttribute("X", S.Vector.X);
                    XAttribute Y = new XAttribute("Y", S.Vector.Y);
                    XAttribute Visibility = new XAttribute("Visibility", S.Visibility);
                    XAttribute Efemerik = new XAttribute("Efemerik", S.Efemerik);


                    // добавляем атрибут и элементы в первый элемент
                    Element.Add(IsMassCenterVisible);
                    Element.Add(Mass);
                    Element.Add(X);
                    Element.Add(Y);
                    Element.Add(Visibility);
                    Element.Add(Efemerik);

                    FirstElement.Add(Element);
                }
            }
            // создаем корневой элемент===============================================================
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
