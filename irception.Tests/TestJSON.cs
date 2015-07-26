using irception.Domain;
using NUnit.Framework;
using System.IO;

namespace irception.Tests
{
    [TestFixture]
    public class TestJSON
    {
        [Test]
        public void TestYouTube()
        {
            string json = File.ReadAllText("..\\..\\JSON\\URLPost.txt");
            var requestParams = API.GetURLRequestParams(json);

            Assert.That(requestParams, Is.Not.Null);
            Assert.That(requestParams.ChannelID, Is.EqualTo(2));
            Assert.That(requestParams.Nick, Is.EqualTo("nirgle_"));
            Assert.That(requestParams.YouTube, Is.Not.Null);

            var yt = requestParams.YouTube;
            Assert.That(yt.kind, Is.EqualTo("youtube#videoListResponse"));
            Assert.That(yt.items.Count, Is.EqualTo(1));
            Assert.That(yt.items[0].id, Is.EqualTo("p2nYjRCNII4"));
            Assert.That(yt.items[0].kind, Is.EqualTo("youtube#video"));
            Assert.That(yt.items[0].snippet, Is.Not.Null);
            Assert.That(yt.items[0].snippet.title, Is.EqualTo("Blinded By The Light (Time Ramesside - Part 2)"));
            Assert.That(yt.items[0].snippet.description, Is.EqualTo("Jason Welge is not responsible for any injury sustained from prolonged exposure to Time Ramesside's lighting.\n\n*Sorry for the long delay in the second part's release. Got side-tracked with other projects and it just fell on the backburner.\n\nPart One: https://www.youtube.com/watch?v=WWoagy78Oi4\n\n- Music -\n\"Hush\", \"Cephalopod\", & \"Opportunity Walks\" by Kevin MacLeod http://incompetech.com/\nLicense: CC: By Attribution 3.0\n\n• Twitter: https://twitter.com/AllShamNoWow\n• Live Stream: http://www.twitch.tv/allshamnowow"));

            Assert.That(yt.items[0].snippet.thumbnails.ContainsKey("default"), Is.True);
            Assert.That(yt.items[0].snippet.thumbnails.ContainsKey("medium"), Is.True);
            Assert.That(yt.items[0].snippet.thumbnails.ContainsKey("high"), Is.True);
            Assert.That(yt.items[0].snippet.thumbnails.ContainsKey("standard"), Is.True);
            Assert.That(yt.items[0].snippet.thumbnails.ContainsKey("maxres"), Is.True);

            Assert.That(yt.items[0].snippet.thumbnails["default"].url, Is.EqualTo("https://i.ytimg.com/vi/p2nYjRCNII4/default.jpg"));
            Assert.That(yt.items[0].snippet.thumbnails["default"].width, Is.EqualTo(120));
            Assert.That(yt.items[0].snippet.thumbnails["default"].height, Is.EqualTo(90));
        }

        [Test]
        public void TestNoYouTube()
        {
            string json = File.ReadAllText("..\\..\\JSON\\URLPostNoYouTube.txt");
            var requestParams = API.GetURLRequestParams(json);

            Assert.That(requestParams, Is.Not.Null);
            Assert.That(requestParams.ChannelID, Is.EqualTo(2));
            Assert.That(requestParams.Nick, Is.EqualTo("nirgle_"));
            Assert.That(requestParams.YouTube, Is.Null);
        }
    }
}
