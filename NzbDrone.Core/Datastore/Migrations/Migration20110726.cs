﻿using System;
using System.Data;
using Migrator.Framework;

namespace NzbDrone.Core.Datastore.Migrations
{

    [Migration(20110726)]
    public class Migration20110726 : NzbDroneMigration
    {
        protected override void MainDbUpgrade()
        {
            Database.RemoveTable("ExternalNotificationSettings");

            Database.AddTable("ExternalNotificationDefinitions", new[]
                                                                  {
                                                                      new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
                                                                      new Column("Enable", DbType.Boolean, ColumnProperty.NotNull),
                                                                      new Column("ExternalNotificationProviderType", DbType.String, ColumnProperty.NotNull),
                                                                      new Column("Name", DbType.String, ColumnProperty.NotNull)
                                                                  });
        }

    }
}