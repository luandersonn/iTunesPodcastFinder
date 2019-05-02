using iTunesPodcastFinder.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace iTunesPodcastFinder.Test
{
	public class GetPodcastTests
	{
		PodcastFinder finder;
		[SetUp]
		public void Setup()
		{
			finder = new PodcastFinder();
		}

		[Test]		
		public void IDExtraction()
		{
			string url1 = @"https://podcasts.apple.com/br/podcast/mamilos/id942491627?uo=2";
			string id1 = finder.GetItunesID(url1);

			string url2 = "https://podcasts.apple.com/br/podcast/xadrez-verbal/id996967108?uo=2";
			string id2 = finder.GetItunesID(url2);

			Assert.AreEqual("942491627", id1);
			Assert.AreEqual("996967108", id2);
		}

		[Test]
		public void GetPodcastByID()
		{
			Podcast podcast1 = finder.GetPodcastAsync("942491627", "br").Result; // Mamilos
			Podcast podcast2 = finder.GetPodcastAsync("996967108", "br").Result; // Xadrez Verbal
			Podcast invalid = finder.GetPodcastAsync("000000000", "br").Result; // Invalid
			
			Assert.AreEqual("Mamilos", podcast1.Name);
			Assert.AreEqual("Xadrez Verbal", podcast2.Name);
			Assert.IsNull(invalid);
		}
		
		[Test]
		public void GetPodcastThrowsExpectedException()
		{
			Assert.ThrowsAsync<ArgumentNullException>(() => finder.GetPodcastAsync(null));
			Assert.ThrowsAsync<ArgumentNullException>(() => finder.GetPodcastAsync(null, null));
			Assert.ThrowsAsync<ArgumentNullException>(() => finder.GetPodcastAsync("", null));
			Assert.ThrowsAsync<ArgumentNullException>(() => finder.GetPodcastAsync("996967108", null));
			Assert.ThrowsAsync<HttpRequestException>(() => finder.GetPodcastAsync("abc1234343"));
		}
		[Test]
		public void GetTopPodcast()
		{
			IEnumerable<PodcastGenre> GetGenres()
			{
				foreach (PodcastGenre genre in Enum.GetValues(typeof(PodcastGenre)))
					yield return genre;
			}

			foreach(PodcastGenre genre in GetGenres())
			{
				IEnumerable<Podcast> podcasts = finder.GetTopPodcastsAsync("br", 150, genre).Result;
				Assert.AreEqual(150, podcasts.Count());
			}
		}
	}
}