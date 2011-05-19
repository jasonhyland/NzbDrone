﻿using System;
using System.IO;
using System.Linq;
using AutoMoq;
using MbUnit.Framework;
using Moq;
using NzbDrone.Core.Model;
using NzbDrone.Core.Providers;
using NzbDrone.Core.Providers.Core;
using NzbDrone.Core.Repository.Quality;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    public class SabProviderTest : TestBase
    {
        [Test]
        public void AddByUrlSuccess()
        {
            //Setup
            const string sabHost = "192.168.5.55";
            const int sabPort = 2222;
            const string apikey = "5c770e3197e4fe763423ee7c392c25d1";
            const string username = "admin";
            const string password = "pass";
            const SabnzbdPriorityType priority = SabnzbdPriorityType.Normal;
            const string category = "tv";


            var mocker = new AutoMoqer();

            var fakeConfig = mocker.GetMock<ConfigProvider>();
            fakeConfig.SetupGet(c => c.SabHost)
                .Returns(sabHost);
            fakeConfig.SetupGet(c => c.SabPort)
                .Returns(sabPort);
            fakeConfig.SetupGet(c => c.SabApiKey)
                .Returns(apikey);
            fakeConfig.SetupGet(c => c.SabUsername)
                .Returns(username);
            fakeConfig.SetupGet(c => c.SabPassword)
                .Returns(password);
            fakeConfig.SetupGet(c => c.SabTvPriority)
                .Returns(priority);
            fakeConfig.SetupGet(c => c.SabTvCategory)
                .Returns(category);

            mocker.GetMock<HttpProvider>(MockBehavior.Strict)
                .Setup(
                    s =>
                    s.DownloadString(
                        "http://192.168.5.55:2222/api?mode=addurl&name=http://www.nzbclub.com/nzb_download.aspx?mid=1950232&priority=0&cat=tv&nzbname=This+is+an+Nzb&apikey=5c770e3197e4fe763423ee7c392c25d1&ma_username=admin&ma_password=pass"))
                .Returns("ok");

            //Act
            bool result = mocker.Resolve<SabProvider>().AddByUrl(
                "http://www.nzbclub.com/nzb_download.aspx?mid=1950232", "This is an Nzb");

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AddByUrlError()
        {
            //Setup
            string sabHost = "192.168.5.55";
            int sabPort = 2222;
            string apikey = "5c770e3197e4fe763423ee7c392c25d1";
            string username = "admin";
            string password = "pass";
            var priority = SabnzbdPriorityType.Normal;
            string category = "tv";

            var mocker = new AutoMoqer();

            var fakeConfig = mocker.GetMock<ConfigProvider>();
            fakeConfig.SetupGet(c => c.SabHost)
                .Returns(sabHost);
            fakeConfig.SetupGet(c => c.SabPort)
                .Returns(sabPort);
            fakeConfig.SetupGet(c => c.SabApiKey)
                .Returns(apikey);
            fakeConfig.SetupGet(c => c.SabUsername)
                .Returns(username);
            fakeConfig.SetupGet(c => c.SabPassword)
                .Returns(password);
            fakeConfig.SetupGet(c => c.SabTvPriority)
                .Returns(priority);
            fakeConfig.SetupGet(c => c.SabTvCategory)
                .Returns(category);
            mocker.GetMock<HttpProvider>()
                .Setup(s => s.DownloadString(It.IsAny<String>()))
                .Returns("error");

            //Act
            var sabProvider = mocker.Resolve<SabProvider>();
            var result = sabProvider.AddByUrl("http://www.nzbclub.com/nzb_download.aspx?mid=1950232", "This is an nzb");

            //Assert
            Assert.IsFalse(result);
            ExceptionVerification.ExcpectedWarns(1);
        }

        [Test]
        public void IsInQueue_True()
        {
            //Setup
            string sabHost = "192.168.5.55";
            int sabPort = 2222;
            string apikey = "5c770e3197e4fe763423ee7c392c25d1";
            string username = "admin";
            string password = "pass";

            var mocker = new AutoMoqer();

            var fakeConfig = mocker.GetMock<ConfigProvider>();
            fakeConfig.SetupGet(c => c.SabHost)
              .Returns(sabHost);
            fakeConfig.SetupGet(c => c.SabPort)
                .Returns(sabPort);
            fakeConfig.SetupGet(c => c.SabApiKey)
                .Returns(apikey);
            fakeConfig.SetupGet(c => c.SabUsername)
                .Returns(username);
            fakeConfig.SetupGet(c => c.SabPassword)
                .Returns(password);

            mocker.GetMock<HttpProvider>(MockBehavior.Strict)
                .Setup(s => s.DownloadString("http://192.168.5.55:2222/api?mode=queue&output=xml&apikey=5c770e3197e4fe763423ee7c392c25d1&ma_username=admin&ma_password=pass"))
                .Returns(File.ReadAllText(@".\Files\Queue.xml"));

            //Act
            bool result = mocker.Resolve<SabProvider>().IsInQueue("Ubuntu Test");

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsInQueue_False_Empty()
        {
            //Setup
            string sabHost = "192.168.5.55";
            int sabPort = 2222;
            string apikey = "5c770e3197e4fe763423ee7c392c25d1";
            string username = "admin";
            string password = "pass";

            var mocker = new AutoMoqer();

            var fakeConfig = mocker.GetMock<ConfigProvider>();
            fakeConfig.SetupGet(c => c.SabHost)
                .Returns(sabHost);
            fakeConfig.SetupGet(c => c.SabPort)
                .Returns(sabPort);
            fakeConfig.SetupGet(c => c.SabApiKey)
                .Returns(apikey);
            fakeConfig.SetupGet(c => c.SabUsername)
                .Returns(username);
            fakeConfig.SetupGet(c => c.SabPassword)
                .Returns(password);

            mocker.GetMock<HttpProvider>(MockBehavior.Strict)
                .Setup(s => s.DownloadString("http://192.168.5.55:2222/api?mode=queue&output=xml&apikey=5c770e3197e4fe763423ee7c392c25d1&ma_username=admin&ma_password=pass"))
                .Returns(File.ReadAllText(@".\Files\QueueEmpty.xml"));

            //Act
            bool result = mocker.Resolve<SabProvider>().IsInQueue(String.Empty);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        [ExpectedException(typeof(ApplicationException), Message = "API Key Incorrect")]
        public void IsInQueue_False_Error()
        {
            //Setup
            string sabHost = "192.168.5.55";
            int sabPort = 2222;
            string apikey = "5c770e3197e4fe763423ee7c392c25d1";
            string username = "admin";
            string password = "pass";

            var mocker = new AutoMoqer();

            var fakeConfig = mocker.GetMock<ConfigProvider>();
            fakeConfig.SetupGet(c => c.SabHost)
                .Returns(sabHost);
            fakeConfig.SetupGet(c => c.SabPort)
                .Returns(sabPort);
            fakeConfig.SetupGet(c => c.SabApiKey)
                .Returns(apikey);
            fakeConfig.SetupGet(c => c.SabUsername)
                .Returns(username);
            fakeConfig.SetupGet(c => c.SabPassword)
                .Returns(password);

            mocker.GetMock<HttpProvider>(MockBehavior.Strict)
                .Setup(s => s.DownloadString("http://192.168.5.55:2222/api?mode=queue&output=xml&apikey=5c770e3197e4fe763423ee7c392c25d1&ma_username=admin&ma_password=pass"))
                .Returns(File.ReadAllText(@".\Files\QueueError.xml"));


            //Act
            mocker.Resolve<SabProvider>().IsInQueue(String.Empty);
        }

        [Test]
        [Row(1, new[] { 2 }, "My Episode Title", QualityTypes.DVD, false, "My Series Name - 1x2 - My Episode Title [DVD]")]
        [Row(1, new[] { 2 }, "My Episode Title", QualityTypes.DVD, true, "My Series Name - 1x2 - My Episode Title [DVD] [Proper]")]
        [Row(1, new[] { 2 }, "", QualityTypes.DVD, true, "My Series Name - 1x2 -  [DVD] [Proper]")]
        [Row(1, new[] { 2, 4 }, "My Episode Title", QualityTypes.HDTV, false, "My Series Name - 1x2-1x4 - My Episode Title [HDTV]")]
        [Row(1, new[] { 2, 4 }, "My Episode Title", QualityTypes.HDTV, true, "My Series Name - 1x2-1x4 - My Episode Title [HDTV] [Proper]")]
        [Row(1, new[] { 2, 4 }, "", QualityTypes.HDTV, true, "My Series Name - 1x2-1x4 -  [HDTV] [Proper]")]
        public void sab_title(int seasons, int[] episodes, string title, QualityTypes quality, bool proper, string excpected)
        {
            var mocker = new AutoMoqer();

            var parsResult = new EpisodeParseResult()
            {
                SeriesId = 12,
                AirDate = DateTime.Now,
                Episodes = episodes.ToList(),
                Proper = proper,
                Quality = quality,
                SeasonNumber = seasons,
                EpisodeTitle = title,
                FolderName = "My Series Name"
            };

            //Act
            var actual = mocker.Resolve<SabProvider>().GetSabTitle(parsResult);

            //Assert
            Assert.AreEqual(excpected, actual);
        }
    }
}