using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace StageCoder
{
	public partial class FileNameDialog : Window
	{
		private const string DEFAULT_TEXT = "Enter a name for the snippet";
		private static readonly List<string> _tips = new List<string> {
			"Tip: The name you choose is the same name you can use to trigger the snippet",
			"Tip: The snippets gets saved into a folder caller snippets in the solution folder"
		};

		public FileNameDialog()
		{
			InitializeComponent();

			Loaded += (s, e) =>
			{
				//Icon = BitmapFrame.Create(new Uri("pack://application:,,,/AddAnyFile;component/Resources/icon.png", UriKind.RelativeOrAbsolute));
				Title = "Save selection as snippet";
				SetRandomTip();

				Name.Focus();
				Name.CaretIndex = 0;
				Name.Text = DEFAULT_TEXT;
				Name.Select(0, Name.Text.Length);

				Name.PreviewKeyDown += (a, b) =>
				{
					if (b.Key == Key.Escape)
					{
						if (string.IsNullOrWhiteSpace(Name.Text) || Name.Text == DEFAULT_TEXT)
						{
							Close();
						}
						else
						{
							Name.Text = string.Empty;
						}
					}
					else if (Name.Text == DEFAULT_TEXT)
					{
						Name.Text = string.Empty;
						btnCreate.IsEnabled = true;
					}
				};

			};
		}

		public string Input => Name.Text.Trim();

		private void SetRandomTip()
		{
			Random rnd = new Random(DateTime.Now.GetHashCode());
			int index = rnd.Next(_tips.Count);
			lblTips.Content = _tips[index];
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}
	}
}
