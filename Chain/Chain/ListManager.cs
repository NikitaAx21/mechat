using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Microsoft.Win32;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Globalization;

namespace Chain
{
	public class ListManager
	{
		public ListManager(/*List<Object> list,*/ Panel panel)
		{
            //_list = list;
            ChainList = new List<Object>();

            _panel = panel;
		}

		public  List<Object> ChainList;
		private readonly Panel _panel;

		public void Add(Object objec = null)
        {
            if (objec == null)
            {
                var isNeedToCreateSegment = ChainList.Count != 0 && ChainList.Last() is Joint;
			    var obj = isNeedToCreateSegment ? (Object)new Segment() : new Joint();

			    obj.Id = ChainList.Count;
			    obj.Visual.OnSelectedChanged += Select;
                ChainList.Add(obj);
                return;
            }
            else
            {

                objec.Id = ChainList.Count;
                objec.Visual.OnSelectedChanged += Select;
                ChainList.Add(objec);
                return;

            }
		}

		private void Select(VisualObject obj)
		{
			_panel.SelectedObject = obj.ParentObject;
		}

		public void Delete(int id)
		{
            ChainList.RemoveAll(o => o.Id >= id);
		}

		private string path = "";

		public void Load(/*List<Object> ChainList, out List<Object> ChainList1*/) //из файла
		{
            //ChainList1 = ChainList;
            //ChainList;

            OpenFileDialog fileChoose = new OpenFileDialog();
			if (fileChoose.ShowDialog() == true)
			{
				if (fileChoose.FileName.Split('.')[fileChoose.FileName.Split('.').Length - 1].ToLower() == "xml")
					path = fileChoose.FileName;
				else
					MessageBox.Show("Выберите файл с расширением \".xml\"."); //
			}

			if (!string.IsNullOrEmpty(path))
			{
				List<Object> ProxyChainList = new List<Object>();

				var dataXml = new XmlDocument();
				try
				{
					dataXml.Load(path);
				}
				catch
				{
					MessageBox.Show("Не удалось загрузить файл.");
					return;
				}

				var xRoot = dataXml.DocumentElement; //SourceData
				var xNode = xRoot.FirstChild; //Object

				if (xNode.ChildNodes[0].Name != "Joint") //  
				{
					throw new Exception("Некорректное содержимое файла.");
				}

				try
				{
					foreach (XmlNode node in xNode.ChildNodes)
					{
						if ((node.Name != "Segment") && (node.Name != "Joint"))
						{
							throw new Exception("Некорректное содержимое файла");
						}

						if (ProxyChainList.Count > 0 && (ProxyChainList.Last().GetType().Name == node.Name)) //  
						{
							throw new Exception("Некорректное содержимое файла.");
						}

						Object Obj = node.Name == "Joint" ? new Joint() : (Object)new Segment();

						var myClassType = Obj.GetType();
						var properties = myClassType.GetProperties();

						foreach (var property in properties)
						{
							foreach (XmlNode attribut in node.Attributes)
							{
								if (property.Name == attribut.Name)
								{
									var value_type = property.PropertyType.Name;
									switch (value_type)
									{
										case "Double":
											var attr1 = double.Parse(attribut.Value, CultureInfo.InvariantCulture);
											property.SetValue(Obj, attr1);
											break;

										case "Boolean":
											var attr2 = bool.Parse(attribut.Value);
											property.SetValue(Obj, attr2);
											break;

										default:

											break; //???
									}

									break;
								}
							}
						}

                        if (node.Name == "Joint")
                        {
                            Joint Joi = Obj as Joint;
                            if (Joi.IsAngleRestricted && !(Joi.AngleRestrictionLeft < Joi.CurrentAngle && Joi.CurrentAngle < Joi.AngleRestrictionRight && Joi.AngleRestrictionLeft < Joi.AngleRestrictionRight))//
                            {
                                throw new Exception("Некорректное содержимое файла. Не верные параметры углов.");
                            }

                        }

						ProxyChainList.Add(Obj);
					}

					Delete(0);

                    //_list = ProxyChainList;
                    foreach (Object element in ProxyChainList)
                    {
                        Add(element);
                    }

                    //ChainList1 = ProxyChainList;
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Ошибка при загрузке фафла");
				}
			}
		}

		public void Save() //в файл
		{


            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "SourceData"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension//?
            dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            { 
                //============================================================================================================================================

                var xdoc = new XDocument(); //создаём документ

			    var FirstElement = new XElement("Objects"); // создаем первый элемент

			try
			{
				foreach (Object element in ChainList)
				{
					Object Obj;

					Obj = element as Joint;
					string tag = "Joint";
					if (Obj == null)
					{
						Obj = element as Segment;
						tag = "Segment";
					}

					var Element = new XElement(tag);

					var myClassType = Obj.GetType();
					var properties = myClassType.GetProperties();

					foreach (PropertyInfo property in properties)
					{
						
							if (property.PropertyType.Name == "Boolean" || property.PropertyType.Name == "Double")
							{
								var Attrib = new XAttribute(property.Name, property.GetValue(Obj, null));
								Element.Add(Attrib);
							}
						
					}

					FirstElement.Add(Element);
				}
			}
			catch
			{
				MessageBox.Show("Ошибка исходных данных."); //
			}

			// создаем корневой элемент
			XElement SourceData = new XElement("SourceData");

			// добавляем в корневой элемент
			SourceData.Add(FirstElement);

			// добавляем корневой элемент в документ
			xdoc.Add(SourceData);


                try
                {
                    // Save document
                    string filename = dlg.FileName;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename, false))
                    {
                        file.WriteLine(xdoc);

                    }
                    ////сохраняем документ
                    //xdoc.Save("SourceData.xml");
                    ////xdoc.

                }
                catch
                {
                    MessageBox.Show("Не удалось сохранить/перезаписать файл."); //
                }

            }
            //====================================================================================================================

			//try
			//{
			//	//сохраняем документ
			//	xdoc.Save("SourceData.xml") ;
   //             //xdoc.

   //         }
			//catch
			//{
			//	MessageBox.Show("Не удалось сохранить/перезаписать файл."); //
			//}
		}
	}
}