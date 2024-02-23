// Decompiled with JetBrains decompiler
// Type: VideoLibrary.YouTubeVideo
// Assembly: libvideo, Version=3.1.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 17817EC5-58AD-49FB-80DB-1FFC57084213
// Assembly location: C:\Users\Admin\Desktop\RE\EasyFetch\libvideo.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VideoLibrary.Helpers;

#nullable disable
namespace VideoLibrary
{
  public class YouTubeVideo : Video
  {
    private readonly string jsPlayerUrl;
    private string jsPlayer;
    private string uri;
    private readonly Query _uriQuery;
    private bool _encrypted;
    private bool _needNDescramble;

    internal YouTubeVideo(VideoInfo info, UnscrambledQuery query, string jsPlayerUrl)
    {
      this.Info = info;
      this.Title = info?.Title;
      this.uri = query.Uri;
      this._uriQuery = new Query(this.uri);
      this.jsPlayerUrl = jsPlayerUrl;
      this._encrypted = query.IsEncrypted;
      this._needNDescramble = this._uriQuery.ContainsKey("n");
      this.FormatCode = int.Parse(this._uriQuery["itag"]);
    }

    public override string Title { get; }

    public override VideoInfo Info { get; }

    public override WebSites WebSite => WebSites.YouTube;

    public override string Uri => this.GetUriAsync().GetAwaiter().GetResult();

    public string GetUri(Func<DelegatingClient> makeClient)
    {
      return this.GetUriAsync(makeClient).GetAwaiter().GetResult();
    }

    public override Task<string> GetUriAsync()
    {
      return this.GetUriAsync((Func<DelegatingClient>) (() => new DelegatingClient()));
    }

    public async Task<string> GetUriAsync(Func<DelegatingClient> makeClient)
    {
      ConfiguredTaskAwaitable<string> configuredTaskAwaitable;
      if (this._encrypted)
      {
        configuredTaskAwaitable = this.DecryptAsync(this.uri, makeClient).ConfigureAwait(false);
        string str = await configuredTaskAwaitable;
        this.uri = str;
        str = (string) null;
        this._encrypted = false;
      }
      if (this._needNDescramble)
      {
        configuredTaskAwaitable = this.NDescrambleAsync(this.uri, makeClient).ConfigureAwait(false);
        string str = await configuredTaskAwaitable;
        this.uri = str;
        str = (string) null;
        this._needNDescramble = false;
      }
      return this.uri;
    }

    public int FormatCode { get; }

    public long? ContentLength
    {
      get
      {
        if (this._contentLength.HasValue)
          return this._contentLength;
        this._contentLength = this.GetContentLength(this._uriQuery).Result;
        return this._contentLength;
      }
    }

    public bool IsEncrypted => this._encrypted;

    private long? _contentLength { get; set; }

    private async Task<long?> GetContentLength(Query query)
    {
      string clen;
      if (query.TryGetValue("clen", out clen))
        return new long?(long.Parse(clen));
      using (VideoClient client = new VideoClient())
      {
        long? contentLengthAsync = await client.GetContentLengthAsync(this.uri);
        return contentLengthAsync;
      }
    }

    private async Task<string> DecryptAsync(string uri, Func<DelegatingClient> makeClient)
    {
      Query query = new Query(uri);
      string signature;
      if (!query.TryGetValue(YouTube.GetSignatureKey(), out signature))
        return uri;
      if (string.IsNullOrWhiteSpace(signature))
        throw new Exception("Signature not found.");
      if (this.jsPlayer == null)
      {
        string str = await makeClient().GetStringAsync(this.jsPlayerUrl).ConfigureAwait(false);
        this.jsPlayer = str;
        str = (string) null;
      }
      query[YouTube.GetSignatureKey()] = this.DecryptSignature(this.jsPlayer, signature);
      return query.ToString();
    }

    private string DecryptSignature(string js, string signature)
    {
      Regex regex = new Regex("\\w+(?:.|\\[)(\\\"?\\w+(?:\\\")?)\\]?\\(");
      string[] decryptionFunctionLines = this.GetDecryptionFunctionLines(js);
      YouTubeVideo.Decryptor decryptor = new YouTubeVideo.Decryptor();
      string str1 = Regex.Match(string.Join(";", decryptionFunctionLines), "([\\$_\\w]+).\\w+\\(\\w+,\\d+\\);").Groups[1].Value;
      if (string.IsNullOrEmpty(str1))
        throw new Exception("Could not find signature decipher definition name. Please report this issue to us.");
      string js1 = Regex.Match(js, "var\\s+" + Regex.Escape(str1) + "=\\{(\\w+:function\\(\\w+(,\\w+)?\\)\\{(.*?)\\}),?\\};", RegexOptions.Singleline).Groups[0].Value;
      if (string.IsNullOrEmpty(js1))
        throw new Exception("Could not find signature decipher definition body. Please report this issue to us.");
      foreach (string input in decryptionFunctionLines)
      {
        if (!decryptor.IsComplete)
        {
          Match match = regex.Match(input);
          if (match.Success)
            decryptor.AddFunction(js1, match.Groups[1].Value);
        }
        else
          break;
      }
      foreach (string str2 in decryptionFunctionLines)
      {
        Match match = regex.Match(str2);
        if (match.Success)
          signature = decryptor.ExecuteFunction(signature, str2, match.Groups[1].Value);
      }
      return signature;
    }

    private string[] GetDecryptionFunctionLines(string js)
    {
      Match match = Regex.Match(js, "(\\w+)=function\\(\\w+\\){(\\w+)=\\2\\.split\\(\\x22{2}\\);.*?return\\s+\\2\\.join\\(\\x22{2}\\)}");
      string[] decryptionFunctionLines;
      if (!match.Success)
        decryptionFunctionLines = (string[]) null;
      else
        decryptionFunctionLines = match.Groups[0].Value.Split(';');
      return decryptionFunctionLines;
    }

    private async Task<string> NDescrambleAsync(string uri, Func<DelegatingClient> makeClient)
    {
      Query query = new Query(uri);
      string signature;
      if (!query.TryGetValue("n", out signature))
        return uri;
      if (string.IsNullOrWhiteSpace(signature))
        throw new Exception("N Signature not found.");
      if (this.jsPlayer == null)
      {
        string str = await makeClient().GetStringAsync(this.jsPlayerUrl).ConfigureAwait(false);
        this.jsPlayer = str;
        str = (string) null;
      }
      query["n"] = this.DescrambleNSignature(this.jsPlayer, signature);
      return query.ToString();
    }

    private string DescrambleNSignature(string js, string signature)
    {
      Match match = new Regex("\\.get\\(\\\"n\\\"\\)\\)&&\\(\\w=([a-zA-Z0-9$]{3})\\([a-zA-Z0-9]\\)").Match(js);
      if (match.Success)
      {
        string str = match.Groups[1].Value;
        string descrambleFunctionLines = this.GetDescrambleFunctionLines(str, js);
        if (!string.IsNullOrWhiteSpace(descrambleFunctionLines))
        {
          Context context = new Context();
          context.Eval("var " + descrambleFunctionLines);
          return context.GetVariable(str).As<Function>().Call(new Arguments()
          {
            (JSValue) signature
          }).Value.ToString();
        }
      }
      return signature;
    }

    private string GetDescrambleFunctionLines(string functionName, string js)
    {
      Match match1 = Regex.Match(js, functionName + "=function\\((\\w)\\){var\\s+\\w=\\1.split\\(\\x22{2}\\),\\w=");
      Match match2 = Regex.Match(js, "\\+a}return\\s\\w.join\\(\\x22{2}\\)};");
      return match1.Success && match2.Success ? js.Substring(match1.Index, match2.Index + match2.Length - match1.Index) : (string) null;
    }

    public int Fps
    {
      get
      {
        switch (this.FormatCode)
        {
          case 18:
          case 22:
          case 37:
          case 43:
          case 59:
          case 133:
          case 134:
          case 135:
          case 136:
          case 137:
          case 138:
          case 160:
          case 242:
          case 243:
          case 244:
          case 247:
          case 248:
          case 264:
          case 266:
          case 271:
          case 278:
          case 313:
          case 394:
          case 395:
          case 396:
          case 397:
            return 30;
          case 272:
          case 298:
          case 299:
          case 302:
          case 303:
          case 304:
          case 305:
          case 308:
          case 315:
          case 330:
          case 331:
          case 332:
          case 333:
          case 334:
          case 335:
          case 336:
          case 337:
          case 398:
          case 399:
          case 400:
          case 401:
          case 402:
          case 571:
            return 60;
          default:
            return -1;
        }
      }
    }

    public bool IsAdaptive => this.AdaptiveKind != 0;

    public AdaptiveKind AdaptiveKind
    {
      get
      {
        switch (this.FormatCode)
        {
          case 18:
          case 22:
          case 37:
          case 43:
          case 59:
          case 133:
          case 134:
          case 135:
          case 136:
          case 137:
          case 138:
          case 160:
          case 242:
          case 243:
          case 244:
          case 247:
          case 248:
          case 264:
          case 266:
          case 271:
          case 272:
          case 298:
          case 299:
          case 302:
          case 303:
          case 304:
          case 305:
          case 308:
          case 313:
          case 315:
          case 330:
          case 331:
          case 332:
          case 333:
          case 334:
          case 335:
          case 336:
          case 337:
          case 394:
          case 395:
          case 396:
          case 397:
          case 398:
          case 399:
          case 400:
          case 401:
          case 402:
          case 571:
            return AdaptiveKind.Video;
          case 139:
          case 140:
          case 141:
          case 171:
          case 172:
          case 249:
          case 250:
          case 251:
          case 256:
          case 258:
          case 327:
          case 338:
            return AdaptiveKind.Audio;
          default:
            return AdaptiveKind.None;
        }
      }
    }

    public int AudioBitrate
    {
      get
      {
        switch (this.FormatCode)
        {
          case 18:
            return 96;
          case 22:
          case 256:
            return 192;
          case 37:
          case 43:
          case 59:
          case 140:
          case 171:
          case 251:
            return 128;
          case 139:
          case 249:
          case 250:
            return 48;
          case 141:
          case 172:
          case 327:
            return 256;
          case 258:
            return 384;
          case 338:
            return 480;
          default:
            return -1;
        }
      }
    }

    public int Resolution
    {
      get
      {
        switch (this.FormatCode)
        {
          case 18:
          case 43:
          case 134:
          case 243:
          case 332:
          case 396:
            return 360;
          case 22:
          case 136:
          case 247:
          case 298:
          case 302:
          case 334:
          case 398:
            return 720;
          case 37:
          case 137:
          case 248:
          case 299:
          case 303:
          case 335:
          case 399:
            return 1080;
          case 59:
          case 135:
          case 244:
          case 333:
          case 397:
            return 480;
          case 133:
          case 242:
          case 331:
          case 395:
            return 240;
          case 138:
          case 272:
          case 402:
          case 571:
            return 4320;
          case 160:
          case 278:
          case 330:
          case 394:
            return 144;
          case 264:
          case 271:
          case 304:
          case 308:
          case 336:
          case 400:
            return 1440;
          case 266:
          case 305:
          case 313:
          case 315:
          case 337:
          case 401:
            return 2160;
          default:
            return -1;
        }
      }
    }

    public override VideoFormat Format
    {
      get
      {
        switch (this.FormatCode)
        {
          case 18:
          case 22:
          case 37:
          case 59:
          case 133:
          case 134:
          case 135:
          case 136:
          case 137:
          case 138:
          case 160:
          case 264:
          case 266:
          case 298:
          case 299:
          case 304:
          case 305:
          case 394:
          case 395:
          case 396:
          case 397:
          case 398:
          case 399:
          case 400:
          case 401:
          case 402:
          case 571:
            return VideoFormat.Mp4;
          case 43:
          case 242:
          case 243:
          case 244:
          case 247:
          case 248:
          case 271:
          case 272:
          case 302:
          case 303:
          case 308:
          case 313:
          case 315:
          case 330:
          case 331:
          case 332:
          case 333:
          case 334:
          case 335:
          case 336:
          case 337:
            return VideoFormat.WebM;
          default:
            return VideoFormat.Unknown;
        }
      }
    }

    public AudioFormat AudioFormat
    {
      get
      {
        switch (this.FormatCode)
        {
          case 18:
          case 22:
          case 37:
          case 59:
          case 139:
          case 140:
          case 141:
          case 256:
          case 258:
          case 327:
            return AudioFormat.Aac;
          case 43:
          case 249:
          case 250:
          case 251:
          case 338:
            return AudioFormat.Opus;
          case 171:
          case 172:
            return AudioFormat.Vorbis;
          default:
            return AudioFormat.Unknown;
        }
      }
    }

    private class Decryptor
    {
      private static readonly Regex ParametersRegex = new Regex("\\(\\w+,(\\d+)\\)");
      private readonly Dictionary<string, YouTubeVideo.Decryptor.FunctionType> _functionTypes = new Dictionary<string, YouTubeVideo.Decryptor.FunctionType>();
      private readonly StringBuilder _stringBuilder = new StringBuilder();

      public bool IsComplete
      {
        get
        {
          return this._functionTypes.Count == Enum.GetValues(typeof (YouTubeVideo.Decryptor.FunctionType)).Length;
        }
      }

      public void AddFunction(string js, string function)
      {
        string str = Regex.Escape(function);
        YouTubeVideo.Decryptor.FunctionType? nullable = new YouTubeVideo.Decryptor.FunctionType?();
        if (Regex.IsMatch(js, "(\\\")?" + str + "(\\\")?:\\bfunction\\b\\([a],b\\).(\\breturn\\b)?.?\\w+\\."))
          nullable = new YouTubeVideo.Decryptor.FunctionType?(YouTubeVideo.Decryptor.FunctionType.Slice);
        else if (Regex.IsMatch(js, "(\\\")?" + str + "(\\\")?:\\bfunction\\b\\(\\w+\\,\\w\\).\\bvar\\b.\\bc=a\\b"))
          nullable = new YouTubeVideo.Decryptor.FunctionType?(YouTubeVideo.Decryptor.FunctionType.Swap);
        if (Regex.IsMatch(js, "(\\\")?" + str + "(\\\")?:\\bfunction\\b\\(\\w+\\){\\w+\\.reverse"))
          nullable = new YouTubeVideo.Decryptor.FunctionType?(YouTubeVideo.Decryptor.FunctionType.Reverse);
        if (!nullable.HasValue)
          return;
        this._functionTypes[function] = nullable.Value;
      }

      public string ExecuteFunction(string signature, string line, string function)
      {
        YouTubeVideo.Decryptor.FunctionType functionType;
        if (!this._functionTypes.TryGetValue(function, out functionType))
          return signature;
        switch (functionType)
        {
          case YouTubeVideo.Decryptor.FunctionType.Reverse:
            return this.Reverse(signature);
          case YouTubeVideo.Decryptor.FunctionType.Slice:
          case YouTubeVideo.Decryptor.FunctionType.Swap:
            int index = int.Parse(YouTubeVideo.Decryptor.ParametersRegex.Match(line).Groups[1].Value, NumberStyles.AllowThousands, (IFormatProvider) NumberFormatInfo.InvariantInfo);
            return functionType == YouTubeVideo.Decryptor.FunctionType.Slice ? this.Slice(signature, index) : this.Swap(signature, index);
          default:
            throw new ArgumentOutOfRangeException("type");
        }
      }

      private string Reverse(string signature)
      {
        this._stringBuilder.Clear();
        for (int index = signature.Length - 1; index >= 0; --index)
          this._stringBuilder.Append(signature[index]);
        return this._stringBuilder.ToString();
      }

      private string Slice(string signature, int index) => signature.Substring(index);

      private string Swap(string signature, int index)
      {
        this._stringBuilder.Clear();
        this._stringBuilder.Append(signature);
        this._stringBuilder[0] = this._stringBuilder[index % this._stringBuilder.Length];
        this._stringBuilder[index % this._stringBuilder.Length] = signature[0];
        return this._stringBuilder.ToString();
      }

      private enum FunctionType
      {
        Reverse,
        Slice,
        Swap,
      }
    }
  }
}
