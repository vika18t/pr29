using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace pr29
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		BitmapSource original;

		public MainWindow()
		{
			InitializeComponent();
		}


		private void Open_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Изображения|*.jpg;*.png;*.bmp";

			if (dialog.ShowDialog() == true)
			{
				BitmapImage bitmap = new BitmapImage(
					new Uri(dialog.FileName));

				original = bitmap;
				myImg.Source = bitmap;

				redImg.Source = ChangeImage(0);
				greenImg.Source = ChangeImage(1);
				blueImg.Source = ChangeImage(2);
				grayImg.Source = ChangeImage(3);
			}
		}

		private BitmapSource ChangeImage(int mode)
		{
			WriteableBitmap wb = new WriteableBitmap(original);

			int stride = wb.PixelWidth * 4;

			byte[] pixels = new byte[wb.PixelHeight * stride];

			wb.CopyPixels(pixels, stride, 0);

			for (int y = 0; y < wb.PixelHeight; y++)
			{
				for (int x = 0; x < wb.PixelWidth; x++)
				{
					int index = y * stride + x * 4;

					byte b = pixels[index];
					byte g = pixels[index + 1];
					byte r = pixels[index + 2];

					if (mode == 0)
					{
						pixels[index] = 0;
						pixels[index + 1] = 0;
					}

					if (mode == 1)
					{
						pixels[index] = 0;
						pixels[index + 2] = 0;
					}

					if (mode == 2)
					{
						pixels[index + 1] = 0;
						pixels[index + 2] = 0;
					}

					if (mode == 3)
					{
						byte gray = (byte)((r + g + b) / 3);

						pixels[index] = gray;
						pixels[index + 1] = gray;
						pixels[index + 2] = gray;
					}

					if (mode == 4)
					{
						pixels[index] = (byte)(255 - b);
						pixels[index + 1] = (byte)(255 - g);
						pixels[index + 2] = (byte)(255 - r);
					}
				}
			}

			wb.WritePixels(
				new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight),
				pixels, stride, 0);

			return wb;
		}
		private void Red_Click(object sender, RoutedEventArgs e)
		{
			if (original != null)
				myImg.Source = ChangeImage(0);
		}

		private void Green_Click(object sender, RoutedEventArgs e)
		{
			if (original != null)
				myImg.Source = ChangeImage(1);
		}

		private void Blue_Click(object sender, RoutedEventArgs e)
		{
			if (original != null)
				myImg.Source = ChangeImage(2);
		}

		private void Negative_Click(object sender, RoutedEventArgs e)
		{
			if (original != null)
				myImg.Source = ChangeImage(4);
		}
		
	}
}