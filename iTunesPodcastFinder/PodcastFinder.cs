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
		private static readonly string base_search_url = @"https://itunes.apple.com/search?term={0}&country={1}&entity=podcast&limit={2}";
		private static readonly string base_lookup_url = @"https://itunes.apple.com/{0}/lookup?id={1}&entity=podcast";
		private static readonly string base_top_url = @"https://itunes.apple.com/{0}/rss/toppodcasts/limit={1}/genre={2}/json";
		
		/// <summary>
		/// Get a list of iTunes podcasts based on the query term
		/// </summary>
		/// <param name="queryTerm">Podcast keyword you want to search for</param>
		/// <param name="maxItems">Maximum number of items you want to retrieve</param>
		/// <param name="country">Two-letter country code (ISO 3166-1 alpha-2)</param>		
		/// <returns>List of podcasts</returns>
		public async Task<IEnumerable<Podcast>> SearchPodcastsAsync(string queryTerm, int maxItems = 100, string country = "us")
		{			
			if (queryTerm == null)
				throw new ArgumentNullException(nameof(queryTerm));
			if (maxItems < 1)
				throw new ArgumentException("The maximum number of items must be greater than zero", nameof(maxItems));
			if (country == null)
				throw new ArgumentNullException(nameof(country));			
			Uri url = new Uri(string.Format(base_search_url, queryTerm, country, maxItems));
			string json = await WebRequestAsync(url);
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
			string json = await WebRequestAsync(url);
			return JsonHelper.DeserializePodcast(json).FirstOrDefault();
		}

		/// <summary>
		/// Get a list of top podcasts
		/// </summary>
		/// <param name="country">Two-letter country code (ISO 3166-1 alpha-2)</param>
		/// <param name="maxItems">Maximum number of items you want to retrieve</param>
		/// <param name="genre">Podcast genre</param>
		/// <returns>list of top podcasts</returns>
		public async Task<IEnumerable<Podcast>> GetTopPodcastsAsync(string country = "us", int maxItems = 100, PodcastGenre genre = PodcastGenre.All)
		{
			if (country == null)			
				throw new ArgumentNullException(nameof(country));
			if (maxItems < 1)
				throw new ArgumentException("The maximum number of items must be greater than zero", nameof(maxItems));

			Uri url = new Uri(string.Format(base_top_url, country, maxItems, (int)genre));
			string json = await WebRequestAsync(url);
			return JsonHelper.DeserializePodcast(json);
		}

		/// <summary>
		/// Extract the unique podcast ID from iTunes URL
		/// </summary>
		/// <param name="itunesLink">Podcast URL from iTunes</param>
		/// <returns>Podcast ID from iTunes</returns>
		public string GetItunesID(string itunesLink)
		{
			if (itunesLink == null)
				throw new ArgumentNullException(nameof(itunesLink));
			return Regex.Match(itunesLink, @"/id(?<ID>(\d)+)").Groups["ID"].Value;
		}

		private async Task<string> WebRequestAsync(Uri url)
		{
			HttpClient httpClient = new HttpClient();								
			var httpResponse = await httpClient.GetAsync(url);
			httpResponse.EnsureSuccessStatusCode();
			return await httpResponse.Content.ReadAsStringAsync();
		}
	}
}
