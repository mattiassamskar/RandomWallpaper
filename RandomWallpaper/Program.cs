using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace RandomWallpaper
{
  class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      try
      {
        if(string.IsNullOrEmpty(Properties.Settings.Default.AccessKey))
          AskUserForAccessKey();

        var filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\photo.jpg";
        new UnsplashService().DownloadPhoto(filePath);
        SetDesktopWallPaper(filePath);
      }
      catch (Exception e)
      {
        using (var eventLog = new EventLog("Application"))
        {
          eventLog.Source = "Application";
          eventLog.WriteEntry(
            e.Message + Environment.NewLine + e.InnerException?.Message,
            EventLogEntryType.Error);
        }
      }
    }

    private static void AskUserForAccessKey()
    {
      Application app = new Application();
      app.Run(new AccessKeyWindow());
    }

    private static void SetDesktopWallPaper(string filePath)
    {
      uint SPI_SETDESKWALLPAPER = 20;
      uint SPIF_UPDATEINIFILE = 0x1;

      SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, filePath, SPIF_UPDATEINIFILE);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
  }
}
