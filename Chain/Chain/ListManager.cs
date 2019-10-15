using System.Collections.Generic;
using System.Linq;
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
		public ListManager(Panel panel)
		{
			ChainList = new List<Object>();

			_panel = panel;
		}

		public List<Object> ChainList;
		private readonly Panel _panel;

		public void Add(Object objec = null)
		{
			Object obj;
			if (objec != null)
				obj = objec;

			else
			{
				var isNeedToCreateSegment = ChainList.Count != 0 && ChainList.Last() is Joint;
				obj = isNeedToCreateSegment ? (Object)new Segment() : new Joint();
			}

			obj.Id = ChainList.Count;
			obj.Visual.OnSelectedChanged += Select;
			ChainList.Add(obj);
		}

		private void Select(VisualObject obj)
		{
			_panel.SelectedObject = obj.ParentObject;
		}

		public void Delete(int id)
		{
			foreach (var o in ChainList)
			{
				if (o.Id < id)
					continue;

				o.Visual.OnSelectedChanged -= Select;
				ChainList.Remove(o);
			}
		}

		private string _path = "";

		public void Load() //из файла
		{
			var fileChoose = new OpenFileDialog();
			if (fileChoose.ShowDialog() == true)
			{
				if (fileChoose.FileName.Split('.')[fileChoose.FileName.Split('.').Length - 1].ToLower() == "xml")
					_path = fileChoose.FileName;
				else
					MessageBox.Show("Выберите файл с расширением \".xml\".");
			}
			else
			{
				return;
			}

			if (string.IsNullOrEmpty(_path))
				return;

			var proxyChainList = new List<Object>();

			var dataXml = new XmlDocument();
			try
			{
				dataXml.Load(_path);
			}
			catch
			{
				MessageBox.Show("Не удалось загрузить файл.");
				return;
			}

			var xRoot = dataXml.DocumentElement;
			if (xRoot == null)
				return;

			var xNode = xRoot.FirstChild;

			if (xNode.ChildNodes[0].Name != "Joint")
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

					if (proxyChainList.Count > 0 && (proxyChainList.Last().GetType().Name == node.Name))
					{
						throw new Exception("Некорректное содержимое файла.");
					}

					var obj = node.Name == "Joint" ? new Joint() : (Object)new Segment();

					var myClassType = obj.GetType();
					var properties = myClassType.GetProperties();

					foreach (var property in properties)
					{
						if (node.Attributes == null)
							continue;

						foreach (XmlNode attribut in node.Attributes)
						{
							if (property.Name != attribut.Name)
								continue;

							var valueType = property.PropertyType.Name;
							switch (valueType)
							{
								case "Double":
									var attr1 = double.Parse(attribut.Value, CultureInfo.InvariantCulture);
									property.SetValue(obj, attr1);
									break;

								case "Boolean":
									var attr2 = bool.Parse(attribut.Value);
									property.SetValue(obj, attr2);
									break;

								default:

									break; //???
							}

							break;
						}
					}

					if (node.Name == "Joint")
					{
						if (!(obj is Joint joi))
							continue;

						if (joi.IsAngleRestricted &&
							!(joi.AngleRestrictionLeft < joi.CurrentAngle &&
							  joi.CurrentAngle < joi.AngleRestrictionRight &&
							  joi.AngleRestrictionLeft < joi.AngleRestrictionRight))
						{
							throw new Exception("Некорректное содержимое файла. Не верные параметры углов.");
						}
					}

					proxyChainList.Add(obj);
				}

				Delete(0);

				foreach (var element in proxyChainList)
				{
					Add(element);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Ошибка при загрузке фафла");
			}
		}

		public void Save() //в файл
		{
			var dlg = new SaveFileDialog
			{
				FileName = "SourceData",
				DefaultExt = ".text",
				Filter = "Text documents (.xml)|*.xml"
			};

			// Show save file dialog box
			var result = dlg.ShowDialog();
			if (result != true)
				return;

			var xdoc = new XDocument(); //создаём документ

			var firstElement = new XElement("Objects"); // создаем первый элемент

			try
			{
				foreach (var element in ChainList)
				{
					Object obj = element as Joint;
					var tag = "Joint";
					if (obj == null)
					{
						obj = element as Segment;
						tag = "Segment";
					}

					var Element = new XElement(tag);

					var myClassType = obj.GetType();
					var properties = myClassType.GetProperties();

					foreach (var property in properties)
					{
						if (property.PropertyType.Name != "Boolean" && property.PropertyType.Name != "Double")
							continue;

						var attrib = new XAttribute(property.Name, property.GetValue(obj, null));
						Element.Add(attrib);
					}

					firstElement.Add(Element);
				}
			}
			catch
			{
				MessageBox.Show("Ошибка исходных данных."); //
			}

			// создаем корневой элемент
			var sourceData = new XElement("SourceData");

			// добавляем в корневой элемент
			sourceData.Add(firstElement);

			// добавляем корневой элемент в документ
			xdoc.Add(sourceData);

			try
			{
				// Save document
				var filename = dlg.FileName;
				using (var file = new System.IO.StreamWriter(filename, false))
				{
					file.WriteLine(xdoc);
				}
			}
			catch
			{
				MessageBox.Show("Не удалось сохранить/перезаписать файл."); //
			}
		}
	}
}