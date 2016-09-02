using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieNews.Data;
using System.Linq;

namespace MovieNews.Test
{
    [TestClass]
    public class MovieDataTests
    {
        [TestMethod]
        public void CanParseNetflixFeed()
        {
            var z = Netflix.getTop100();
            var rss =
                  @"<?xml version=""1.0"" encoding=""UTF - 8"" standalone=""yes""?>
                    <rss version = ""2.0"" xmlns:atom=""http://www.w3.org/2005/Atom"">
                        <channel>
                            <title> Netflix Top 100 </title>
                            <ttl> 20160 </ttl>
                            <link>http://dvd.netflix.com</link>
                            <description> Top 100 Netflix movies, published every 2 weeks.</description>
                            <language> en - us </language>
                            <cf:treatAs xmlns:cf = ""http://www.microsoft.com/schemas/rss/core/2005"">list</cf:treatAs>
                            <atom:link href = ""http://dvd.netflix.com/Top100RSS"" rel = ""self"" type = ""application/rss+xml"" />
                            <item>
                                <title>The Martian</title>
                                <link>http://dvd.netflix.com/Movie/The-Martian/80058399</link>
                                <guid isPermaLink = ""true"">http://dvd.netflix.com/Movie/The-Martian/80058399</guid>
                                <description>&lt;a href=&quot;http://dvd.netflix.com/Movie/The-Martian/80058399&quot;&gt;&lt;img src=&quot;//secure.netflix.com/us/boxshots/small/80058399.jpg&quot;/&gt;&lt;/a&gt;&lt;br&gt;Abandoned on the surface of Mars after his crew concludes that he perished in a dust storm, astronaut Mark Watney must find a way to survive the planet's harsh environment -- despite having only 28 days of supplies left.</description>
                            </item>
                            <item>
                                <title> Bridge of Spies </title>
                                <link>http://dvd.netflix.com/Movie/Bridge-of-Spies/80050060</link>
                                <guid isPermaLink = ""true"">http://dvd.netflix.com/Movie/Bridge-of-Spies/80050060</guid>
                                <description>&lt;a href=&quot;http://dvd.netflix.com/Movie/Bridge-of-Spies/80050060&quot;&gt;&lt;img src=&quot;//secure.netflix.com/us/boxshots/small/80050060.jpg&quot;/&gt;&lt;/a&gt;&lt;br&gt;At the height of the Cold War in 1960, the downing of an American spy plane and the pilot's subsequent capture by the Soviets draws Brooklyn attorney James Donovan into the middle of an intense effort to secure the aviator's release.</description>
                            </item>
                        </channel>
                    </rss>";
            var feed = Netflix.parseMovies(rss);
            Assert.AreEqual(2, feed.Count());

            var first = feed.First();
            Assert.AreEqual("The Martian", first.Title);

            Assert.AreEqual("//secure.netflix.com/us/boxshots/small/80058399.jpg", first.Thumbnail.Value);
        }

        [TestMethod]
        public void CanParseNYTReviews()
        {
            var response =
                @"{
                   ""status"":""OK"",
                   ""copyright"":""Copyright (c) 2016 The New York Times Company. All Rights Reserved."",
                   ""has_more"":false,
                   ""num_results"":1,
                   ""results"":[
                      {
                         ""display_title"":""Paddington"",
                         ""mpaa_rating"":""PG"",
                         ""critics_pick"":0,
                         ""byline"":""JEANNETTE CATSOULIS"",
                         ""headline"":""Adventures of a Peruvian Immigrant (the Furry Variety)"",
                         ""summary_short"":""The beloved bear of children&#8217;s books makes his big-screen debut in Paul King&#8217;s &#8220;Paddington.&#8221;"",
                         ""publication_date"":""2015-01-15"",
                         ""opening_date"":""2015-01-16"",
                         ""date_updated"":""2016-03-30 07:08:50"",
                         ""link"":{
                            ""type"":""article"",
                            ""url"":""http:\/\/www.nytimes.com\/2015\/01\/16\/movies\/paddington-an-adaptation-of-michael-bonds-books.html"",
                            ""suggested_link_text"":""Read the New York Times Review of Paddington""
                         },
                         ""multimedia"":{
                            ""type"":""mediumThreeByTwo210"",
                            ""src"":""https:\/\/static01.nyt.com\/images\/2015\/01\/16\/arts\/16PADDINGTON\/16PADDINGTON-mediumThreeByTwo210-v2.jpg"",
                            ""width"":210,
                            ""height"":140
                         }
                      }
                   ]
                }";

            var actual = NewYorkTimes.tryPickReviewByName("Paddington", response).Value;

            var expected = new Review(
                            published: new DateTime(2015, 01, 15),
                            summary: "The beloved bear of children&#8217;s books makes his big-screen debut in Paul King&#8217;s &#8220;Paddington.&#8221;",
                            link: "http://www.nytimes.com/2015/01/16/movies/paddington-an-adaptation-of-michael-bonds-books.html",
                            linkText: "Read the New York Times Review of Paddington");         

            Assert.AreEqual(expected, actual);
        }
    }
}
