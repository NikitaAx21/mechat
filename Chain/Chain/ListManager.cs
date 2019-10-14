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
			var obj = isNeedToCreateSegment ? (Object)new Segment() : new Joint();

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

		private string path = "";

		public void Load(List<Object> ChainList, out List<Object> ChainList1) //из файла
		{
            ChainList1 = ChainList;

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
                List<Object> ProxyChainList = new List<Object> ();

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

                if (xNode.ChildNodes[0].Name != "Joint")//  
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

                        

                        if (ProxyChainList.Count>0 && (ProxyChainList.Last().GetType().Name == node.Name) )//  
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

                        ProxyChainList.Add(Obj);
					}

                    Delete(0);


                    ChainList1 = ProxyChainList;

                }
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Ошибка при загрузке фафла");
				}
			}
		}


        

            public void Save(List<Object> ChainList) //в файл
		{
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
                        if (property.Name != "IsSelected")
                        {
                            if (property.PropertyType.Name == "Boolean" || property.PropertyType.Name == "Double")
						    {
							    var Attrib = new XAttribute(property.Name, property.GetValue(Obj, null));
							    Element.Add(Attrib);
						    }
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
				//сохраняем документ
				xdoc.Save("SourceData.xml");
			}
			catch
			{
				MessageBox.Show("Не удалось сохранить/перезаписать файл."); //
			}
		}
	}
}