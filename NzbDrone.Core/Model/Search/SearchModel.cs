﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NzbDrone.Core.Model.Search
{
    public class SearchModel
    {
        public string SeriesTitle { get; set; }
        public int EpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }
        public DateTime AirDate { get; set; }
        public SearchType SearchType { get; set; }
    }
}