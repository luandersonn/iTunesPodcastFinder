using iTunesPodcastFinder.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace iTunesPodcastFinder.Test
{
    public class SearchTests
    {
        PodcastFinder finder;
        [SetUp]
        public void Setup()
        {
            finder = new PodcastFinder();
        }

        [Test]
        public void ValidSearch()
        {
            int maxItems = 50;
            IEnumerable<Podcast> result = finder.SearchPodcastsAsync("The Verge", maxItems).Result;
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(x => x.Editor == "The Verge"));
            Assert.LessOrEqual(result.Count(), maxItems);

            result = finder.SearchPodcastsAsync("Manual", maxItems).Result;
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(x => x.Editor == "Manual do Usuário"));
            Assert.LessOrEqual(result.Count(), maxItems);
        }

        [Test]
        public void SearchThrowsExpectedException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => finder.SearchPodcastsAsync(null));
            Assert.ThrowsAsync<ArgumentNullException>(() => finder.SearchPodcastsAsync("CBN", country: null));
            Assert.ThrowsAsync<ArgumentException>(() => finder.SearchPodcastsAsync("CBN", maxItems: 0));
            Assert.ThrowsAsync<ArgumentException>(() => finder.SearchPodcastsAsync("CBN", maxItems: -10));
        }

		[Test]
		public void ValidSearchOffSet()
		{
			int step = 10;
			string term = "tech";
			string country = "us";
			Podcast[] mainResult = finder.SearchPodcastsAsync(queryTerm: term, maxItems: 100, country: country).Result.ToArray();
			int count = 0;
			for(int i = 0; i < mainResult.Length; i += step)
			{
				IEnumerable<Podcast> result = finder.SearchPodcastsAsync(queryTerm: term, maxItems: step, country: country, offset: i).Result;
				foreach (Podcast podcast in result)
				{
					if (count >= mainResult.Length)
						break;
					Assert.AreEqual(mainResult[count++].ItunesId, podcast.ItunesId);
					Debug.WriteLine($"Podcast {count}/{mainResult.Length}");					
				}
			}			
		}
    }
}
