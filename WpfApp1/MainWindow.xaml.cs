using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace BoundingChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int frame = 1;

        public int Frame
        {
            get
            {
                return frame;
            }
            set
            {
                if (value < 1 || value >= Images.Count)
                    return;
                frame = value;
                OnPropertyChanged();
                UpdateImage();
            }
        }

        public int Offset { get; set; }

        public List<BoundingImage> Images { get; set; }

        public MainWindow()
        {
            Offset = 10;
            InitializeComponent();
            LoadData(null, null);
            Frame = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadData(object sender, RoutedEventArgs e)
        {
            Images = new List<BoundingImage>();
            foreach (string filePath in Directory.EnumerateFiles(@".\Image", "*.png"))
            {
                var boundingImage = new BoundingImage()
                {
                    Path = filePath,
                    XmlIn = new XmlDocument(),
                    XmlOut = new XmlDocument()
                };
                boundingImage.XmlIn.Load(System.IO.Path.Combine(@".\Input", System.IO.Path.GetFileNameWithoutExtension(filePath) + ".xml"));
                boundingImage.XmlOut.Load(System.IO.Path.Combine(@".\Output", System.IO.Path.GetFileNameWithoutExtension(filePath) + ".xml"));
                Images.Add(boundingImage);
            }
        }

        private void UpdateImage()
        {
            var image = Images[Frame - 1];
            InputImg.Source = new BitmapImage(new Uri(image.Path, UriKind.Relative));
            OutputImg.Source = new BitmapImage(new Uri(image.Path, UriKind.Relative));
            var result = image.GetResult(Offset);
            resultGrid.DataContext = result;
        }

        private void NextFrame(object sender, RoutedEventArgs e)
        {
            Frame++;
        }

        private void PrevFrame(object sender, RoutedEventArgs e)
        {
            Frame--;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}