using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chain
{
	public class Renderer
	{
		private readonly ListManager _listManager;
		private Point _centerPoint;

		public Renderer(ListManager listManager)
		{
			_listManager = listManager;
		}

		public Point GetCurrentCanvasParameters()
		{
			var res = new Point();
			var obj = _listManager.ChainList.FirstOrDefault();
			if (!(obj?.Visual.Parent is Canvas canvas))
				return res;

			res.X = canvas.ActualWidth;
			res.Y = canvas.ActualHeight;

			return res;
		}

		private bool IsItBig()
		{
			var canvasProperties = GetCurrentCanvasParameters();

			var flag = 0;
			foreach (var element in _listManager.ChainList)
			{
				if (element is Joint)
				{
					var vj = (VisualJoint)element.Visual;
					if ((vj.Coordinate.X > canvasProperties.X) || (vj.Coordinate.Y > canvasProperties.Y) ||
						(vj.Coordinate.X <= 0) || (vj.Coordinate.Y <= 0))
					{
						flag++;
						break;
					}
				}
				else
				{
					var vs = (VisualSegment)element.Visual;
					if ((vs.End.X > canvasProperties.X) || (vs.End.Y > canvasProperties.Y) || (vs.End.X <= 0) ||
						(vs.End.Y <= 0))
					{
						flag++;
						break;
					}
				}
			}

			return flag != 0;
		}

		private bool IsItSmall()
		{
			if (_listManager.ChainList.Count <= 1)
				return false;

			var canvasProperties = GetCurrentCanvasParameters();

			var flag = 0;
			foreach (var element in _listManager.ChainList)
			{
				if (element is Joint)
				{
					var vj = (VisualJoint)element.Visual;
					if (((vj.Coordinate.X <= 0.9 * canvasProperties.X) &&
						 (vj.Coordinate.X >= 0.1 * canvasProperties.X)) &&
						((vj.Coordinate.Y <= 0.9 * canvasProperties.Y) &&
						 (vj.Coordinate.Y >= 0.1 * canvasProperties.Y)))
					{
						flag++;
					}
				}
				else
				{
					var vs = (VisualSegment)element.Visual;
					if (((vs.End.X <= 0.9 * canvasProperties.X) && (vs.End.X >= 0.1 * canvasProperties.X)) &&
						((vs.End.Y <= 0.9 * canvasProperties.Y) && (vs.End.Y >= 0.1 * canvasProperties.Y)))
					{
						flag++;
					}
				}
			}

			return flag == _listManager.ChainList.Count;
		}

		private void SetInterseced()
		{
			var intercededElementsList = Calculations.GetInetersectionElements(_listManager.ChainList);
			foreach (var element in _listManager.ChainList)
			{
				element.Visual.Interseced = intercededElementsList.Contains(element.Id);
			}
		}

		public void OnObjectChanged(Object obj)
		{
			var cJ = _listManager.ChainList[0].Visual as VisualJoint;
			_centerPoint = Calculations.MinusPoint(cJ.Coordinate, _centerPoint);
			Calculations.ChangeMas(_centerPoint);
			_centerPoint = cJ.Coordinate;
			if (obj.Id != _listManager.ChainList.Count - 1)
			{
				switch (obj)
				{
					case Joint modifiedJoint:
					{
						var currentAngle = modifiedJoint.CurrentAngle;
						for (var i = modifiedJoint.Id + 1; i < _listManager.ChainList.Count; i += 2)
						{
							if (!(_listManager.ChainList[i] is Segment currentSegment))
								continue;

							if (!(_listManager.ChainList[i - 1] is Joint prevJoint))
								throw new ArgumentNullException(nameof(prevJoint));

							var tM = new TransformationMatrix(currentAngle, _listManager.ChainList, i);
							var cM = new CoordinatesMatrix(currentSegment, prevJoint);
							var visualSegment = (VisualSegment)_listManager.ChainList[i].Visual;

							var coordinates = Calculations.GetCoord(tM, cM);
							if (i + 1 < _listManager.ChainList.Count)
							{
								if (_listManager.ChainList[i + 1].Visual is VisualJoint vj)
									vj.Coordinate = coordinates;
							}

							visualSegment.End = coordinates;
							Calculations.CoordMas[i] = coordinates;
							if (i + 1 < Calculations.CoordMas.Count)
							{
								Calculations.CoordMas[i + 1] = coordinates;
							}
						}

						break;
					}

					case Segment modifiedSegment:
					{
						Calculations.LengthMas[obj.Id / 2] = modifiedSegment.Length * Calculations.CoefficientOfScale;
						if (!(_listManager.ChainList[obj.Id - 1] is Joint prevJoint))
							throw new ArgumentNullException(nameof(prevJoint));

						var visualSegment = (VisualSegment)modifiedSegment.Visual;
						var tM = new TransformationMatrix(prevJoint.CurrentAngle, _listManager.ChainList, obj.Id);
						var cM = new CoordinatesMatrix(modifiedSegment, prevJoint);
						var coordinates = Calculations.GetCoord(tM, cM);
						Calculations.CoordMas[obj.Id] = Calculations.GetCoord(tM, cM);
						Calculations.CoordMas[obj.Id + 1] = Calculations.GetCoord(tM, cM);

						var dX = coordinates.X - visualSegment.End.X;
						var dY = coordinates.Y - visualSegment.End.Y;
						visualSegment.End = coordinates;
						for (var i = obj.Id + 2; i < _listManager.ChainList.Count; i += 2)
						{
							if (!(_listManager.ChainList[i].Visual is VisualSegment currentVisualSegment))
								continue;

							var x = currentVisualSegment.End.X + dX;
							var y = currentVisualSegment.End.Y + dY;
							var newCoordinates = new Point(x, y);
							currentVisualSegment.End = newCoordinates;
							Calculations.CoordMas[i] = newCoordinates;
							if (i + 1 < Calculations.CoordMas.Count)
							{
								Calculations.CoordMas[i + 1] = newCoordinates;
							}
						}

						break;
					}
				}
			}
			else
			{
				if (obj.Id == 0)
				{
					if (!Calculations.CoordMas.Any())
					{
						var j = (VisualJoint)_listManager.ChainList[0].Visual;
						Calculations.CoordMas.Add(j.Coordinate);
					}
				}
				else
				{
					switch (obj)
					{
						case Joint _:
						{
							var newSegmentCoordinatesEnd = Calculations.CoordMas[obj.Id - 1];
							if (obj.Id < Calculations.CoordMas.Count)
								Calculations.CoordMas[obj.Id] = newSegmentCoordinatesEnd;
							else
								Calculations.CoordMas.Add(newSegmentCoordinatesEnd);
							break;
						}

						case Segment lastSegment:
						{
							if (!(_listManager.ChainList[obj.Id - 1] is Joint prevJoint))
								throw new ArgumentNullException(nameof(prevJoint));

							var lastVisualSegment = (VisualSegment)lastSegment.Visual;

							var tM = new TransformationMatrix(prevJoint.CurrentAngle, _listManager.ChainList, obj.Id);
							if (obj.Id / 2 == Calculations.LengthMas.Count)
								Calculations.LengthMas.Add(Calculations.CoefficientOfScale * lastSegment.Length);
							var cM = new CoordinatesMatrix(lastSegment, prevJoint);
							var coordinates = Calculations.GetCoord(tM, cM);
							lastVisualSegment.End = coordinates;
							if (obj.Id < Calculations.CoordMas.Count)
								Calculations.CoordMas[obj.Id] = coordinates;
							else
								Calculations.CoordMas.Add(coordinates);
							break;
						}
					}
				}
			}

			RescaleIfNeeded();
			ConnectObjects(obj.Id);
			SetInterseced();
		}

		private void ConnectObjects(int startId)
		{
			for (var i = startId; i < _listManager.ChainList.Count; i++)
			{
				switch (_listManager.ChainList[i])
				{
					case Joint currentJoint:
					{
						var visualJoint = (VisualJoint)currentJoint.Visual;
						if (i == 0)
							visualJoint.PutOnCenter();
						else
						{
							var prevVisualSegment = (VisualSegment)_listManager.ChainList[i - 1].Visual;
							visualJoint.Coordinate = prevVisualSegment.End;
						}

						break;
					}

					case Segment currentSegment:
					{
						var visualSegment = (VisualSegment)currentSegment.Visual;
						var prevVisualJoint = (VisualJoint)_listManager.ChainList[i - 1].Visual;
						visualSegment.Begin = prevVisualJoint.Coordinate;
						break;
					}
				}
			}
		}

		private void RescaleIfNeeded()
		{
			if (IsItBig())
			{
				Calculations.MinusScale();
				for (var i = 0; i < Calculations.LengthMas.Count; i++)
				{
					Calculations.LengthMas[i] *= Calculations.CoefficientOfScale;
				}

				_listManager.ChainList[0].OnObjectChanged();
			}
			//TEST

			if (IsItSmall())
			{
				if (Calculations.CoefficientOfScale >= 1)
					return;

				Calculations.PlusScale();
				for (var i = 0; i < Calculations.LengthMas.Count; i++)
				{
					Calculations.LengthMas[i] *= Calculations.CoefficientOfScale;
				}

				_listManager.ChainList[0].OnObjectChanged();
			}
		}
	}
}