using iTunesPodcastFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace iTunesPodcastFinder.Helpers
{
	internal static class XmlHelper
	{
		public static IEnumerable<PodcastEpisode> ParseEpisodes(string xml)
		{
			(XmlNodeList entries, FeedType type) = GetXmlNodeList(xml);
			switch (type)
			{
				case FeedType.Atom:
					throw new NotSupportedException("Parse episodes from a AtomFeed is not supported");

				case FeedType.Rss1:
					if (entries[0] is XmlElement entry)
					{
						var items = entry.GetElementsByTagName("item");

						foreach (XmlNode item in items)
						{
							PodcastEpisode episode = new PodcastEpisode();
							episode.Editor = GetXmlElementValue(entry, "author");
							episode.Title = GetXmlElementValue(item, "title");
							episode.Summary = GetXmlElementValue(item, "description");
							episode.FileUrl = new Uri(GetXmlElementValue(item, "link"));
							DateTime.TryParse(GetXmlElementValue(item, "pubDate"), out DateTime pubDate);
							episode.PublishedDate = pubDate;
							yield return episode;
						}
					}
					break;

				case FeedType.Rss2:
					entry = entries[0] as XmlElement;
					if (entry != null)
					{
						var items = entry.GetElementsByTagName("item");
						foreach (XmlNode item in items)
						{
							PodcastEpisode episode = new PodcastEpisode();
							episode.Editor = GetXmlElementValue(entry, "title");
							episode.Title = GetXmlElementValue(item, "title");
							episode.Summary = GetXmlElementValue(item, "description");
							string link = GetXmlElementValue(item, "link");
							if (item["enclosure"] != null)
							{
								link = GetXmlAttribute(item["enclosure"], "url");
							}
							episode.FileUrl = new Uri(link);
							string date = GetXmlElementValue(item, "pubDate");
							DateTime.TryParse(date, out DateTime pubDate);
							episode.PublishedDate = pubDate;

							try
							{
								string durationString = GetXmlElementValue(item, "itunes:duration");
								episode.Duration = TimeSpan.Parse(durationString).TotalSeconds;
							}
							catch { }
							yield return episode;
						}
					}
					break;
			}
		}
										
		private static (XmlNodeList, FeedType) GetXmlNodeList(string xml)
		{
			var feedType = GetFeedType(xml);
			switch (feedType)
			{
				case FeedType.Atom:
					var doc = new XmlDocument();
					doc.LoadXml(xml);
					var feedNode = doc["feed"];
					return (feedNode.GetElementsByTagName("entry"), feedType);
				case FeedType.Rss1:
				case FeedType.Rss2:
					var xmlDocument = new XmlDocument();
					var bytes = Encoding.UTF8.GetBytes(xml);
					using (var stream = new MemoryStream(bytes))
						xmlDocument.Load(stream);
					return (xmlDocument.GetElementsByTagName("channel"), feedType);
				default:
					throw new InvalidOperationException("Could not determine feed type");
			}
		}

		private static FeedType GetFeedType(string xml)
		{
			FeedType type = FeedType.Atom;
			var doc = new XmlDocument();
			doc.LoadXml(xml);
			var rssTags = doc.GetElementsByTagName("rss");
			if (rssTags != null && rssTags.Count > 0)
			{
				string rssVersion = rssTags[0].Attributes["version"].Value;
				if (rssVersion == "1.0")				
					type = FeedType.Rss1;				
				if (rssVersion == "2.0")				
					type = FeedType.Rss2;				
			}
			return type;
		}

		private static string GetXmlElementValue(XmlNode parentNode, string elementName)
		{
			string value = string.Empty;
			if (parentNode[elementName] != null)			
				value = parentNode[elementName].InnerText;			
			return value;
		}
		private static string GetXmlAttribute(XmlNode xmlNode, string attributeName)
		{
			string attribute = string.Empty;
			if (xmlNode != null && xmlNode.Attributes != null && xmlNode.Attributes[attributeName] != null)
			{
				string value = xmlNode.Attributes[attributeName].Value;
				if (!string.IsNullOrEmpty(value))				
					attribute = value;				
			}
			return attribute;
		}

	}
}