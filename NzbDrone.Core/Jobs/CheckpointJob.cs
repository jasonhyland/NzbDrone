﻿using System;
using System.Linq;
using Ninject;
using NzbDrone.Core.Providers;

namespace NzbDrone.Core.Jobs
{
    public class CheckpointJob : IJob
    {
        private readonly AnalyticsProvider _analyticsProvider;

        [Inject]
        public CheckpointJob(AnalyticsProvider analyticsProvider)
        {
            _analyticsProvider = analyticsProvider;
        }

        public CheckpointJob()
        {
            
        }

        public string Name
        {
            get { return "Checkpoint Job"; }
        }

        public TimeSpan DefaultInterval
        {
            get { return TimeSpan.FromDays(1); }
        }

        public virtual void Start(int targetId, int secondaryTargetId)
        {
            _analyticsProvider.Checkpoint();
        }
    }
}