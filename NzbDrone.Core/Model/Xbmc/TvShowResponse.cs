﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NzbDrone.Core.Model.Xbmc
{
    public class TvShowResponse
    {
        public string Id { get; set; }
        public string JsonRpc { get; set; }
        public TvShowResult Result { get; set; }
    }
}
