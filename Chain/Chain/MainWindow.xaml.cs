using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Chain
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			LManager = new ListManager(Panel);

			LManager.Add();
			var center = LManager.ChainList.Last() as Joint;
			if (center == null)
				return;

			Panel.SelectedObject = center;
			Canvas.Children.Add(center.Visual);
		}

		//public List<Object> ChainList = new List<Object>();
		public ListManager LManager;

		private void SaveList(object sender, RoutedEventArgs e)
		{
			LManager.Save(LManager.ChainList);
		}

		private void LoadList(object sender, RoutedEventArgs e)
		{
			foreach (var o in LManager.ChainList)
			{
				Canvas.Children.Remove(o.Visual);
			}

			//LManager.Delete(0);

			LManager.Load(LManager.ChainList, out LManager.ChainList);

			//==============================

			foreach (var o in LManager.ChainList)
			{
				Canvas.Children.Add(o.Visual);
			}

			Panel.SelectedObject = LManager.ChainList.FirstOrDefault();

			//if (ChainList.Count == 1)//?
			CenterCircleSetPosition();
		}

		private void CenterCircleSetPosition(object sender = null, RoutedEventArgs e = null)
		{
			if (LManager.ChainList.FirstOrDefault() is Joint joint)
			{
				var visualJoint = joint.Visual as VisualJoint;
				visualJoint?.PutOnCenter();
			}
		}

		private void DeleteObject(object sender, RoutedEventArgs e)
		{
			foreach (var o in LManager.ChainList.Where(o => o.Id >= Panel.SelectedObject.Id))
			{
				Canvas.Children.Remove(o.Visual);
			}

			LManager.Delete(Panel.SelectedObject.Id);
			Panel.SelectedObject = LManager.ChainList.LastOrDefault();
		}

		private void AddObject(object sender, RoutedEventArgs e)//=====
		{
			LManager.Add();
			var obj = LManager.ChainList.Last();
			Canvas.Children.Add(obj.Visual);

			if (LManager.ChainList.Count == 1)
				CenterCircleSetPosition();
		}
	}
}