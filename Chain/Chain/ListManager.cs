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
		public List<Object> ChainList;
		private readonly Panel _panel;
		private string _path = "";

		public ListManager(Panel panel)
		{
			ChainList = new List<Object>();

			_panel = panel;
		}

		public void Add(Object objec = null)
		{
            if ((Calculations.CoordMas.Count == 0) || (Calculations.CoordMas.Count == 1))
            {
                Point p = new Point(0,0);
                Calculations.CoordMas.Add(p);
            }
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
			obj.ObjectChanged += ObjOnObjectChanged;
			ChainList.Add(obj);
		}

		public void Delete(int id)
		{
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

		private void ObjOnObjectChanged(Object obj)
		{
			if (obj is Joint)
            {
                var nj = obj as Joint;
                double a = nj.CurrentAngle;
                for (int i = obj.Id + 1; i < ChainList.Count; i += 2)
                {
                    if (ChainList[i] is Segment)
                    {
                        var s = ChainList[i] as Segment;
                        var j = ChainList[i - 1] as Joint;
                        TransformationMatrix tM = new TransformationMatrix(a, ChainList, i);
                        CoordinatesMatrix cM = new CoordinatesMatrix(s, j);
                        s.Vector = Calculations.GetCoord(tM, cM);
                        ChainList[i] = s;
                    }
                }
            }
            if (obj is Segment)
            {
                var ns = obj as Segment;
                var j = ChainList[obj.Id - 1] as Joint;
                var s = ChainList[obj.Id] as Segment;
                Point dR = new Point();
                TransformationMatrix tM = new TransformationMatrix(j.CurrentAngle, ChainList, obj.Id);
                CoordinatesMatrix cM = new CoordinatesMatrix(s, j);
                dR = Calculations.GetCoord(tM, cM);
                s.Vector = dR;
                ChainList[obj.Id] = s;
                double dX = dR.X - s.Vector.X;
                double dY = dR.Y - s.Vector.Y;
                for (int i = obj.Id + 2; i < ChainList.Count; i += 2)
                {
                    var seg = ChainList[i] as Segment;
                    seg.Vector.X += dX;
                    seg.Vector.Y += dY;
                    ChainList[i] = seg;
                }  
            }

			for (var i = obj.Id; i < ChainList.Count; i++)
			{
				var o = ChainList[i];
				if (o is Joint)
				{
					var visualJoint = o.Visual as VisualJoint;
					if (visualJoint == null)
						continue;

					if (i > 0)
					{
						var visualSegment = ChainList[i - 1].Visual as VisualSegment;
						if (visualSegment != null)
							visualJoint.Coordinate = visualSegment.End;
					}
					else
						visualJoint.PutOnCenter();
				}

				if (o is Segment)
				{
					var visualSegment = o.Visual as VisualSegment;
					var visualJoint = ChainList[i - 1].Visual as VisualJoint;
					if (visualSegment != null && visualJoint != null)
						visualSegment.Begin = visualJoint.Coordinate;
				}
			}
		}

		private void Select(VisualObject obj)
		{
			_panel.SelectedObject = obj.ParentObject;
		}
	}
}