using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FFXIVPlayerWardrobe
{
    class VersionCheck
    {
        private const string GitRepo = "https://raw.githubusercontent.com/goaaats/FFXIVPlayerWardrobe/master/FFXIVPlayerWardrobe";

        public EventHandler<VersionCheckEventArgs> GotVersionInfo;

        private readonly WebClient _client = new WebClient();

        public VersionCheck()
        {
            _client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            _client.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs args)
            {
                if (args.Error == null)
                {
                    var m = Regex.Match(args.Result, "AssemblyFileVersion\\(\\\"(.*?)\\\"\\)");
                    var version = m.Groups[1].Value;
                    Debug.WriteLine($"Got version: {m.Groups[0].ToString()}");

                    GotVersionInfo(this, new VersionCheckEventArgs(version == Assembly.GetExecutingAssembly().GetName().Version.ToString(), version));
                }
            };
        }

        public class VersionCheckEventArgs
        {
            public VersionCheckEventArgs(bool current, string newVersion)
            {
                Current = current;
                NewVersion = newVersion;
            }

            public readonly bool Current = false;
            public readonly string NewVersion;
        }

        public void Run()
        {
            Debug.WriteLine($"Downloading {GitRepo + "/Properties/AssemblyInfo.cs"}");
            _client.DownloadStringAsync(new Uri(GitRepo + "/Properties/AssemblyInfo.cs"));
        }
    }
}
