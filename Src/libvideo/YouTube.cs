// Decompiled with JetBrains decompiler
// Type: VideoLibrary.YouTube
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\libvideo.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VideoLibrary.Exceptions;
using VideoLibrary.Helpers;

namespace VideoLibrary
{
  public class YouTube : ServiceBase<YouTubeVideo>
  {
    private const string Playback = "videoplayback";
    private static string _signatureKey;
    public const string YoutubeUrl = "https://youtube.com/";

    public static YouTube Default { get; } = new YouTube();

    internal override async Task<IEnumerable<YouTubeVideo>> GetAllVideosAsync(
      string videoUri,
      Func<string, Task<string>> sourceFactory)
    {
      if (!this.TryNormalize(videoUri, out videoUri))
        throw new ArgumentException("URL is not a valid YouTube URL!");
      string source = await sourceFactory(videoUri).ConfigureAwait(false);
      IEnumerable<YouTubeVideo> videos = this.ParseVideos(source);
      source = (string) null;
      return videos;
    }

    private bool TryNormalize(string videoUri, out string normalized)
    {
      normalized = (string) null;
      videoUri = new StringBuilder(videoUri).Replace("youtu.be/", "youtube.com/watch?v=").Replace("youtube.com/embed/", "youtube.com/watch?v=").Replace("/v/", "/watch?v=").Replace("/watch#", "/watch?").ToString();
      string str;
      if (!new Query(videoUri).TryGetValue("v", out str))
        return false;
      normalized = "https://youtube.com/watch?v=" + str;
      return true;
    }

    private IEnumerable<YouTubeVideo> ParseVideos(string source)
    {
      string jsPlayer = this.ParseJsPlayer(source);
      if (jsPlayer != null)
      {
        JToken playerResponseJson = JToken.Parse(VideoLibrary.Helpers.Json.Extract(this.ParsePlayerJson(source)));
        JToken jtoken1 = playerResponseJson.SelectToken("playabilityStatus.status");
        if (string.Equals(jtoken1 != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken1) : (string) null, "error", StringComparison.OrdinalIgnoreCase))
          throw new UnavailableStreamException("Video has unavailable stream.");
        JToken jtoken2 = playerResponseJson.SelectToken("playabilityStatus.reason");
        string errorReason = jtoken2 != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken2) : (string) null;
        if (!string.IsNullOrWhiteSpace(errorReason))
          throw new UnavailableStreamException("Error caused by Youtube.(" + errorReason + "))");
        JToken jtoken3 = playerResponseJson.SelectToken("videoDetails.isLive");
        bool isLiveStream = jtoken3 != null && Extensions.Value<bool>((IEnumerable<JToken>) jtoken3);
        JToken jtoken4 = playerResponseJson.SelectToken("videoDetails.title");
        string title = jtoken4 != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken4) : (string) null;
        JToken jtoken5 = playerResponseJson.SelectToken("videoDetails.lengthSeconds");
        int? second = jtoken5 != null ? new int?(Extensions.Value<int>((IEnumerable<JToken>) jtoken5)) : new int?();
        JToken jtoken6 = playerResponseJson.SelectToken("videoDetails.author");
        string author = jtoken6 != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken6) : (string) null;
        VideoInfo videoInfo = new VideoInfo(title, second, author);
        if (isLiveStream)
          throw new UnavailableStreamException("This is live stream so unavailable stream.");
        string map = VideoLibrary.Helpers.Json.GetKey("url_encoded_fmt_stream_map", source);
        IEnumerable<UnscrambledQuery> queries;
        if (!string.IsNullOrWhiteSpace(map))
        {
          queries = ((IEnumerable<string>) map.Split(',')).Select<string, UnscrambledQuery>(new Func<string, UnscrambledQuery>(this.Unscramble));
          foreach (UnscrambledQuery unscrambledQuery in queries)
          {
            UnscrambledQuery query = unscrambledQuery;
            yield return new YouTubeVideo(videoInfo, query, jsPlayer);
            query = new UnscrambledQuery();
          }
        }
        else
        {
          List<JToken> streamObjects = new List<JToken>();
          JToken streamFormat = playerResponseJson.SelectToken("streamingData.formats");
          if (streamFormat != null)
            streamObjects.AddRange((IEnumerable<JToken>) ((IEnumerable<JToken>) streamFormat).ToArray<JToken>());
          JToken streamAdaptiveFormats = playerResponseJson.SelectToken("streamingData.adaptiveFormats");
          if (streamAdaptiveFormats != null)
            streamObjects.AddRange((IEnumerable<JToken>) ((IEnumerable<JToken>) streamAdaptiveFormats).ToArray<JToken>());
          foreach (JToken jtoken7 in streamObjects)
          {
            JToken item = jtoken7;
            JToken jtoken8 = item.SelectToken("url");
            string urlValue = jtoken8 != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken8) : (string) null;
            if (!string.IsNullOrEmpty(urlValue))
            {
              UnscrambledQuery query = new UnscrambledQuery(urlValue, false);
              yield return new YouTubeVideo(videoInfo, query, jsPlayer);
            }
            else
            {
              string cipherValue = Extensions.Value<string>((IEnumerable<JToken>) ((item.SelectToken("cipher") ?? item.SelectToken("signatureCipher")) ?? JToken.op_Implicit(string.Empty)));
              if (!string.IsNullOrEmpty(cipherValue))
                yield return new YouTubeVideo(videoInfo, this.Unscramble(cipherValue), jsPlayer);
              urlValue = (string) null;
              cipherValue = (string) null;
              item = (JToken) null;
            }
          }
          streamObjects = (List<JToken>) null;
          streamFormat = (JToken) null;
          streamAdaptiveFormats = (JToken) null;
        }
        string adaptiveMap = VideoLibrary.Helpers.Json.GetKey("adaptive_fmts", source);
        if (!string.IsNullOrWhiteSpace(adaptiveMap))
        {
          queries = ((IEnumerable<string>) adaptiveMap.Split(',')).Select<string, UnscrambledQuery>(new Func<string, UnscrambledQuery>(this.Unscramble));
          foreach (UnscrambledQuery unscrambledQuery in queries)
          {
            UnscrambledQuery query = unscrambledQuery;
            yield return new YouTubeVideo(videoInfo, query, jsPlayer);
            query = new UnscrambledQuery();
          }
        }
        else
        {
          string dashmpdMap = VideoLibrary.Helpers.Json.GetKey("dashmpd", source);
          if (!string.IsNullOrWhiteSpace(adaptiveMap))
          {
            using (HttpClient hc = new HttpClient())
            {
              IEnumerable<string> uris = (IEnumerable<string>) null;
              try
              {
                dashmpdMap = WebUtility.UrlDecode(dashmpdMap).Replace("\\/", "/");
                string manifest = hc.GetStringAsync(dashmpdMap).GetAwaiter().GetResult().Replace("\\/", "/");
                uris = Html.GetUrisFromManifest(manifest);
                manifest = (string) null;
              }
              catch (Exception ex)
              {
                throw new UnavailableStreamException(ex.Message);
              }
              if (uris != null)
              {
                foreach (string v in uris)
                  yield return new YouTubeVideo(videoInfo, this.UnscrambleManifestUri(v), jsPlayer);
              }
              uris = (IEnumerable<string>) null;
            }
          }
          dashmpdMap = (string) null;
        }
        videoInfo = (VideoInfo) null;
        map = (string) null;
        adaptiveMap = (string) null;
      }
    }

    private string ParsePlayerJson(string source)
    {
      string str = (string) null;
      string pattern1 = "\\s*var\\s*ytInitialPlayerResponse\\s*=\\s*(\\{\\\"responseContext\\\".*\\});";
      string pattern2 = "\\[\\\"ytInitialPlayerResponse\\\"\\]\\s*=\\s*(\\{.*\\});";
      string pattern3 = "ytplayer\\.config\\s*=\\s*(\\{\\\".*\\\"\\}\\});";
      Match match;
      string target;
      if ((match = Regex.Match(source, pattern3)).Success && VideoLibrary.Helpers.Json.TryGetKey("player_response", match.Groups[1].Value, out target))
        str = Regex.Unescape(target);
      if (string.IsNullOrWhiteSpace(str) && (match = Regex.Match(source, pattern1)).Success)
        str = match.Groups[1].Value;
      if (string.IsNullOrWhiteSpace(str) && (match = Regex.Match(source, pattern2)).Success)
        str = match.Groups[1].Value;
      return !string.IsNullOrWhiteSpace(str) ? str.Replace("\\u0026", "&").Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty) : throw new UnavailableStreamException("Player json has no found.");
    }

    private string ParseJsPlayer(string source)
    {
      string target;
      string jsPlayer;
      if (VideoLibrary.Helpers.Json.TryGetKey("jsUrl", source, out target) || VideoLibrary.Helpers.Json.TryGetKey("PLAYER_JS_URL", source, out target))
      {
        jsPlayer = target.Replace("\\/", "/");
      }
      else
      {
        Match match = Regex.Match(source, "<script\\s*src=\"([-a-zA-Z0-9()@:%_\\+.~#?&//=]*)\".*name=\"player_ias/base\".*>\\s*</script>");
        if (!match.Success)
          return (string) null;
        jsPlayer = match.Groups[1].Value.Replace("\\/", "/");
      }
      if (jsPlayer.StartsWith("/yts") || jsPlayer.StartsWith("/s"))
        return "https://www.youtube.com" + jsPlayer;
      if (!jsPlayer.StartsWith("http"))
        jsPlayer = "https:" + jsPlayer;
      return jsPlayer;
    }

    private UnscrambledQuery Unscramble(string queryString)
    {
      queryString = queryString.Replace("\\u0026", "&");
      Query query = new Query(queryString);
      string encodedValue = query["url"];
      query.TryGetValue("sp", out YouTube._signatureKey);
      bool encrypted = false;
      string signature;
      if (query.TryGetValue("s", out signature))
      {
        encrypted = true;
        encodedValue += this.GetSignatureAndHost(YouTube.GetSignatureKey(), signature, query);
      }
      else if (query.TryGetValue("sig", out signature))
        encodedValue += this.GetSignatureAndHost(YouTube.GetSignatureKey(), signature, query);
      string uri = WebUtility.UrlDecode(WebUtility.UrlDecode(encodedValue));
      if (!new Query(uri).ContainsKey("ratebypass"))
        uri += "&ratebypass=yes";
      return new UnscrambledQuery(uri, encrypted);
    }

    private string GetSignatureAndHost(string key, string signature, Query query)
    {
      string signatureAndHost = "&" + key + "=" + signature;
      string str;
      if (query.TryGetValue("fallback_host", out str))
        signatureAndHost = signatureAndHost + "&fallback_host=" + str;
      return signatureAndHost;
    }

    private UnscrambledQuery UnscrambleManifestUri(string manifestUri)
    {
      int num = manifestUri.IndexOf("videoplayback") + "videoplayback".Length;
      string str = manifestUri.Substring(0, num);
      string[] strArray = manifestUri.Substring(num, manifestUri.Length - num).Split(new char[1]
      {
        '/'
      }, StringSplitOptions.RemoveEmptyEntries);
      StringBuilder stringBuilder = new StringBuilder(str);
      stringBuilder.Append("?");
      for (int index = 0; index < strArray.Length; index += 2)
      {
        stringBuilder.Append(strArray[index]);
        stringBuilder.Append('=');
        stringBuilder.Append(strArray[index + 1].Replace("%2F", "/"));
        if (index < strArray.Length - 2)
          stringBuilder.Append('&');
      }
      return new UnscrambledQuery(stringBuilder.ToString(), false);
    }

    public static string GetSignatureKey() => string.IsNullOrWhiteSpace(YouTube._signatureKey) ? "signature" : YouTube._signatureKey;
  }
}
