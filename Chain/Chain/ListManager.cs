using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace Chain
{
	public class ListManager
	{
		public List<Object> ChainList;
		private string _path = "";

		public ListManager()
		{
			ChainList = new List<Object>();
		}

		public event Action<VisualObject> ObjectSelected;
		public event Action<Object> ObjectChanged;

		public void Add(Object objectForAdding = null)
		{
			Object obj;
			if (objectForAdding != null)
				obj = objectForAdding;

			else
			{
				var isNeedToCreateSegment = ChainList.Count != 0 && ChainList.Last() is Joint;
				obj = isNeedToCreateSegment ? (Object)new Segment() : new Joint();
			}

			obj.Id = ChainList.Count;
			obj.Visual.OnSelectedChanged += Select;
			obj.ObjectChanged += OnObjectChanged;
			ChainList.Add(obj);
		}

		public void Delete(int id)
		{
			foreach (var o in ChainList)
			{
				if (o.Id < id)
					continue;

				o.ObjectChanged -= OnObjectChanged;
				o.Visual.OnSelectedChanged -= Select;
			}

			ChainList.RemoveAll(o => o.Id >= id);
		}

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
				return;

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

						foreach (XmlNode attribute in node.Attributes)
						{
							if (property.Name != attribute.Name)
								continue;

							var valueType = property.PropertyType.Name;
							switch (valueType)
							{
								case "Double":
									var attr1 = double.Parse(attribute.Value, CultureInfo.InvariantCulture);
									property.SetValue(obj, attr1);
									break;

								case "Boolean":
									var attr2 = bool.Parse(attribute.Value);
									property.SetValue(obj, attr2);
									break;

								default:
									throw new Exception("Некорректное содержимое файла.");
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
				MessageBox.Show(e.Message, "Ошибка при загрузке файла");
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

					var xElement = new XElement(tag);

					var myClassType = obj.GetType();
					var properties = myClassType.GetProperties();

					foreach (var property in properties)
					{
						if (property.PropertyType.Name != "Boolean" && property.PropertyType.Name != "Double")
							continue;

						var attribute = new XAttribute(property.Name, property.GetValue(obj, null));
						xElement.Add(attribute);
					}

					firstElement.Add(xElement);
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

		private void Select(VisualObject obj)
		{
			ObjectSelected?.Invoke(obj);
		}


        public event Action<Thickness> MarginCM_Change;

		private void OnObjectChanged(Object obj)
		{
			ObjectChanged?.Invoke(obj);
            //=========================
            Point MC_coord = Calculations.Mass_center(ChainList);
            //=========================
            Thickness MarginCM = new Thickness(MC_coord.X, MC_coord.Y, 0,0);
            MarginCM_Change.Invoke(MarginCM);
        }
	}
}