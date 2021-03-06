﻿using iTunesPodcastFinder.Helpers;
using iTunesPodcastFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iTunesPodcastFinder
{
    public class PodcastFinder
    {
        private static readonly string base_search_url = @"https://itunes.apple.com/search?term={0}&country={1}&entity=podcast&limit={2}&offset={3}";
        private static readonly string base_lookup_url = @"https://itunes.apple.com/{0}/lookup?id={1}&entity=podcast";
        private static readonly string base_top_url = @"https://itunes.apple.com/{0}/rss/toppodcasts/limit={1}/genre={2}/json";
        public static HttpClient HttpClient { get; set; } = new HttpClient();
		/// <summary>
		/// Get a list of iTunes podcasts based on the query term
		/// </summary>
		/// <param name="queryTerm">Podcast keyword you want to search for</param>
		/// <param name="maxItems">Maximum number of items you want to retrieve</param>
		/// <param name="country">Two-letter country code (ISO 3166-1 alpha-2)</param>		
		/// <param name="offset">Zero-based offset of results</param>		
		/// <returns>List of podcasts</returns>
		public async Task<IEnumerable<Podcast>> SearchPodcastsAsync(string queryTerm, int maxItems = 100, string country = "us", int offset = 0)
        {
            if (queryTerm == null)
                throw new ArgumentNullException(nameof(queryTerm));
            if (maxItems < 1)
                throw new ArgumentException("The maximum number of items must be greater than zero", nameof(maxItems));
            if (country == null)
                throw new ArgumentNullException(nameof(country));
            Uri url = new Uri(string.Format(base_search_url, queryTerm, country, maxItems, offset));
            string json = await WebRequestAsync(url).ConfigureAwait(false);
            return JsonHelper.DeserializePodcast(json);
        }

        /// <summary>
        /// Get iTunes podcast by ID
        /// </summary>
        /// <param name="iTunesID">Unique iTunes Podcast ID</param>
        /// <param name="country">Two-letter country code (ISO 3166-1 alpha-2)</param>
        /// <returns>Podcast associated with this ID</returns>
        public async Task<Podcast> GetPodcastAsync(string iTunesID, string country = "us")
        {
            if (iTunesID == null)
                throw new ArgumentNullException(nameof(iTunesID));
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            Uri url = new Uri(string.Format(base_lookup_url, country, iTunesID));
            string json = await WebRequestAsync(url).ConfigureAwait(false);
            return JsonHelper.DeserializePodcast(json).FirstOrDefault();
        }

        /// <summary>
        /// Get a list of top podcasts
        /// </summary>
        /// <param name="genre">Podcast genre</param>
        /// <param name="maxItems">Maximum number of items you want to retrieve</param>
        /// <param name="country">Two-letter country code (ISO 3166-1 alpha-2)</param>		
        /// <returns>list of top podcasts</returns>
        public async Task<IEnumerable<Podcast>> GetTopPodcastsAsync(PodcastGenre genre = PodcastGenre.All, int maxItems = 100, string country = "us")
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));
            if (maxItems < 1)
                throw new ArgumentException("The maximum number of items must be greater than zero", nameof(maxItems));

            Uri url = new Uri(string.Format(base_top_url, country, maxItems, (int)genre));
            string json = await WebRequestAsync(url).ConfigureAwait(false);
            return JsonHelper.DeserializePodcast(json);
        }

        /// <summary>
        /// Extract the unique podcast ID from iTunes URL
        /// </summary>
        /// <param name="itunesLink">Podcast URL from iTunes</param>
        /// <returns>Podcast ID from iTunes</returns>
        public static string GetItunesID(string itunesLink)
        {
            if (itunesLink == null)
                throw new ArgumentNullException(nameof(itunesLink));
            return Regex.Match(itunesLink, @"/id(?<ID>(\d)+)").Groups["ID"].Value;
        }

        /// <summary>
        /// Get a podcast and the list of episodes
        /// </summary>
        /// <param name="feedUrl">The podcast RSS feed</param>
        /// <returns></returns>
        public async Task<PodcastRequestResult> GetPodcastEpisodesAsync(string feedUrl)
        {
            if (feedUrl == null)
                throw new ArgumentNullException(nameof(feedUrl));
            Uri url = new Uri(feedUrl);

            string xml = await WebRequestAsync(url).ConfigureAwait(false);
			PodcastRequestResult result = XmlHelper.ParsePodcast(xml);
            result.Podcast.FeedUrl = feedUrl;
            return result;
        }

        private async Task<string> WebRequestAsync(Uri url)
        {
            HttpResponseMessage httpResponse = await HttpClient.GetAsync(url).ConfigureAwait(false);
            httpResponse.EnsureSuccessStatusCode();
			try
			{
				return await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			}
			catch (Exception)
			{
				byte[] bytes = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
				return System.Text.Encoding.UTF8.GetString(bytes);
			}
		}
	}
}
