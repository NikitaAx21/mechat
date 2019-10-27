using System.Linq;
using System.Windows;
using System.ComponentModel;


namespace Chain
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
    {
		public MainWindow()
		{
			InitializeComponent();
			LManager = new ListManager();
			_renderer = new Renderer(LManager);
			LManager.ObjectSelected += OnObjectSelected;
			LManager.ObjectChanged += _renderer.OnObjectChanged;

			LManager.Add();
			var center = LManager.ChainList.Last() as Joint;
			if (center == null)
				return;

			Panel.SelectedObject = center;
			Canvas.Children.Add(center.Visual);
            LManager.MarginCM_Change += MarginCM_Change;

            CMexist = true;

        }
		private Renderer _renderer;

		public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Thickness _marginCM;
        public Thickness MarginCM
        {
            get { return _marginCM; }
            set
            {
                _marginCM = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(nameof(MarginCM)));

            }
        }

        public void MarginCM_Change(Thickness e)
        {
            MarginCM = e;
        }

        private bool _cmexist;
        public bool CMexist
        {
            get { return _cmexist; }
            set
            {
                _cmexist = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(nameof(CMexist)));

            }
        }

        private bool CMexist_Check()
        {
            return LManager.ChainList.Count == 0 ? false : true;
        }

        public ListManager LManager;

		private void SaveList(object sender, RoutedEventArgs e)
		{
			LManager.Save();
		}

		private void LoadList(object sender, RoutedEventArgs e)
		{
			foreach (var o in LManager.ChainList)
			{
				Canvas.Children.Remove(o.Visual);
			}

			LManager.Load();

			foreach (var o in LManager.ChainList)
			{
				Canvas.Children.Add(o.Visual);
			}

			Panel.SelectedObject = LManager.ChainList.FirstOrDefault();

			CenterCircleSetPosition();
		}

		private void CenterCircleSetPosition(object sender = null, RoutedEventArgs e = null)
		{
			if (LManager.ChainList.FirstOrDefault() is Joint joint)
			{
				var visualJoint = joint.Visual as VisualJoint;
				visualJoint?.PutOnCenter();
				joint.OnObjectChanged();
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
            //=====================================


            MarginCM_Change(LManager.MarginCM());

            CMexist = CMexist_Check();


        }

        private void AddObject(object sender, RoutedEventArgs e)
		{
			LManager.Add();
			var obj = LManager.ChainList.Last();
			Canvas.Children.Add(obj.Visual);
			obj.OnObjectChanged();

            CMexist = CMexist_Check();

        }

		private void OnObjectSelected(VisualObject obj)
		{
			Panel.SelectedObject = obj.ParentObject;
		}
	}
}