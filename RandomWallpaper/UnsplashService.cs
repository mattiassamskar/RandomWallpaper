using System;
using System.Configuration;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace RandomWallpaper
{
  public class UnsplashService
  {
    public void DownloadPhoto(string filePath)
    {
      var unsplashUri = GetUnsplashUriFromConfig();
      using (var webClient = new WebClient())
      {
        var address = GetAddressToRandomPhoto(webClient, unsplashUri);
        webClient.DownloadFile(address, filePath);
      }
    }

    private string GetAddressToRandomPhoto(WebClient webClient, string unsplashUri)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      ServicePointManager.Expect100Continue = true;
      var json = webClient.DownloadString(unsplashUri);
      return JsonConvert.DeserializeObject<UnsplashPhoto>(json).Urls.Raw;
    }

    private string GetUnsplashUriFromConfig()
    {
      var uriBuilder = new UriBuilder(ConfigurationManager.AppSettings.Get("Address"));
      var query = HttpUtility.ParseQueryString(uriBuilder.Query);
      query["orientation"] = "landscape";
      query["client_id"] = Properties.Settings.Default.AccessKey;
      uriBuilder.Query = query.ToString();

      return uriBuilder.ToString();
    }

    public class UnsplashPhoto
    {
      public Url Urls { get; set; }

      public class Url
      {
        public string Raw { get; set; }
      }
    }
  }
}