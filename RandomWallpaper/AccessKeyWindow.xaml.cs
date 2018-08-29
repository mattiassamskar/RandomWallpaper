using System.Windows;

namespace RandomWallpaper
{
  public partial class AccessKeyWindow : Window
  {
    public AccessKeyWindow()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Properties.Settings.Default.AccessKey = textBox.Text;
      Properties.Settings.Default.Save();
      Close();
    }
  }
}
