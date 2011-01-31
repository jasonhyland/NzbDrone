﻿using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using NzbDrone.Core.Repository.Quality;

namespace NzbDrone.Core.Test
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    public class ParserTest
    {
        [Test]
        [Row("WEEDS.S03E01-06.DUAL.BDRip.XviD.AC3.-HELLYWOOD", 3, 1)]
        [Row("Two.and.a.Half.Me.103.720p.HDTV.X264-DIMENSION", 1, 3)]
        [Row("Chuck.4x05.HDTV.XviD-LOL", 4, 5)]
        [Row("The.Girls.Next.Door.S03E06.DVDRip.XviD-WiDE", 3, 6)]
        [Row("Degrassi.S10E27.WS.DSR.XviD-2HD", 10, 27)]
        [Row(@"z:\tv shows\battlestar galactica (2003)\Season 3\S03E05 - Collaborators.mkv", 3, 5)]
        [Row(@"z:\tv shows\modern marvels\Season 16\S16E03 - The Potato.mkv", 16, 3)]
        [Row(@"z:\tv shows\robot chicken\Specials\S00E16 - Dear Consumer - SD TV.avi", 0, 16)]
        public void episode_parse(string path, int season, int episode)
        {
            var result = Parser.ParseEpisodeInfo(path);
            Assert.Count(1, result);
            Assert.AreEqual(season, result[0].SeasonNumber);
            Assert.AreEqual(episode, result[0].EpisodeNumber);
        }

        [Test]
        [Row("WEEDS.S03E01-06.DUAL.BDRip.XviD.AC3.-HELLYWOOD", QualityTypes.DVD)]
        [Row("WEEDS.S03E01-06.DUAL.BDRip.AC3.-HELLYWOOD", QualityTypes.Bluray)]
        [Row("Two.and.a.Half.Men.S08E05.720p.HDTV.X264-DIMENSION", QualityTypes.HDTV)]
        [Row("Chuck.S04E05.HDTV.XviD-LOL", QualityTypes.TV)]
        [Row("The.Girls.Next.Door.S03E06.DVDRip.XviD-WiDE", QualityTypes.DVD)]
        [Row("Degrassi.S10E27.WS.DSR.XviD-2HD", QualityTypes.TV)]
        [Row("Sonny.With.a.Chance.S02E15.720p.WEB-DL.DD5.1.H.264-SURFER", QualityTypes.WEBDL)]
        [Row("Sonny.With.a.Chance.S02E15.720p", QualityTypes.HDTV)]
        [Row("Sonny.With.a.Chance.S02E15.mkv", QualityTypes.HDTV)]
        [Row("Sonny.With.a.Chance.S02E15.avi", QualityTypes.TV)]
        [Row("Sonny.With.a.Chance.S02E15.xvid", QualityTypes.TV)]
        [Row("Sonny.With.a.Chance.S02E15.divx", QualityTypes.TV)]
        [Row("Sonny.With.a.Chance.S02E15", QualityTypes.Unknown)]
        [Row("S01E04 - So Old - Playdate - 720p TV.mkv", QualityTypes.HDTV)]
        [Row("S22E03 - MoneyBART - HD TV.mkv", QualityTypes.HDTV)]
        [Row("S01E03 - Come Fly With Me - 720p BluRay.mkv", QualityTypes.Bluray)]
        [Row("S11E06 - D-Yikes! - 720p WEB-DL.mkv", QualityTypes.WEBDL)]
        public void quality_parse(string path, object quality)
        {
            var result = Parser.ParseQuality(path);
            Assert.AreEqual(quality, result);
        }

        [Test]
        [Row(@"c:\test\", @"c:\test")]
        [Row(@"c:\\test\\", @"c:\test")]
        [Row(@"C:\\Test\\", @"c:\test")]
        [Row(@"C:\\Test\\Test\", @"c:\test\test")]
        [Row(@"\\Testserver\Test\", @"\\testserver\test")]
        public void Normalize_Path(string dirty, string clean)
        {
            var result = Parser.NormalizePath(dirty);
            Assert.AreEqual(clean, result);
        }
    }
}