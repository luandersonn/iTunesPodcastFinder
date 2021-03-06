﻿using iTunesPodcastFinder.Models;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace iTunesPodcastFinder.Test
{
    public class GetEpisodesTests
    {
        PodcastFinder finder;
        [SetUp]
        public void Setup()
        {
            finder = new PodcastFinder();
        }

        [Test]
        public void GetEpisodesThrowsExpectedException()
        {
            string feedUrl = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => finder.GetPodcastEpisodesAsync(feedUrl));

            feedUrl = "invalidURLteste";
            Assert.ThrowsAsync<UriFormatException>(() => finder.GetPodcastEpisodesAsync(feedUrl));

            feedUrl = "https://www.invalidURL.com/asdalsdkd";
            Assert.ThrowsAsync<HttpRequestException>(() => finder.GetPodcastEpisodesAsync(feedUrl));
        }

        [Test]
        public void ValidPodcastEpisodeInfos()
        {
            // Random feeds
            string feed1 = @"https://feeds.buzzsprout.com/177739.rss";
            string feed2 = @"http://www.workshop.com.br/itunes/podcast.xml";
            string feed3 = @"https://podcasts.files.bbci.co.uk/p02pc9zn.rss";
            string feed4 = @"http://audio.globoradio.globo.com/podcast/feed/97/mundo-corporativo";
            string feed5 = @"http://feeds.soundcloud.com/users/soundcloud:users:219522981/sounds.rss";
			string feed6 = @"https://www.carreirasemfronteiras.com.br/feed/podcast";
			string feed7 = @"https://feeds.feedburner.com/inglesbasicotodososdias";
			string feed8 = @"http://forodeteresina.libsyn.com/rss";
			string feed9 = @"https://feed.podbean.com/gvcast/feed.xml";

			CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed1).Result);
            CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed2).Result);
            CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed3).Result);
            CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed4).Result);
            CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed5).Result);
			CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed6).Result);
			CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed7).Result);
			CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed8).Result);
			CheckPodcastInfos(finder.GetPodcastEpisodesAsync(feed9).Result);
		}

        private void CheckPodcastInfos(PodcastRequestResult requestResult)
        {
            Assert.IsNotNull(requestResult.Podcast.Name);
            int episodesCount = requestResult.Episodes.Count();
            foreach (PodcastEpisode episode in requestResult.Episodes)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(episode.Title));
                Assert.AreEqual(episodesCount--, episode.EpisodeNumber);
                Assert.IsNotNull(episode.FileUrl);
                Assert.IsFalse(string.IsNullOrWhiteSpace(episode.InnerXml));
				Assert.AreNotEqual(episode.Duration, default(TimeSpan));
            }
            Assert.AreEqual(requestResult.Episodes.Count(), requestResult.Podcast.EpisodesCount);
            Assert.IsFalse(string.IsNullOrWhiteSpace(requestResult.Podcast.InnerXml));
        }
    }
}
