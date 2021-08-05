﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DotNetify;
using DotNetify.Elements;
using DotNetify.Routing;

namespace Website.Server
{
   public class Dashboard : BaseVM, IRoutable
   {
      private IDisposable _subscription;

      public RoutingState RoutingState { get; set; } = new RoutingState();

      public Dashboard(ILiveDataService liveDataService)
      {
         AddProperty<string>("Download")
            .WithAttribute(new { Label = "กำไรที่คาดว่าจะได้รับรวม(บาท)", Icon = "payments" })
            .SubscribeTo(liveDataService.Download);

         AddProperty<string>("Upload")
            .WithAttribute(new { Label = "ต้นทุนการผลิตรวม(บาท)", Icon = "attach_money" })
            .SubscribeTo(liveDataService.Upload);

         AddProperty<string>("Latency")
            .WithAttribute(new { Label = "รายรับที่คาดว่าจะได้รับรวม(บาท)", Icon = "monetization_on" })
            .SubscribeTo(liveDataService.Latency);

         AddProperty<int[]>("Traffic").SubscribeTo(liveDataService.Traffic);

         AddProperty<int[]>("Utilization")
            .WithAttribute(new ChartAttribute { Labels = new string[] { "กำไรรวม", "ต้นทุนรวม", "รายรับรวม" } })
            .SubscribeTo(liveDataService.Utilization);

         AddProperty<int[]>("ServerUsage").SubscribeTo(liveDataService.ServerUsage)
            .WithAttribute(new ChartAttribute { Labels = new string[] { "dns", "sql", "nethst", "w2k", "ubnt", "uat", "ftp", "smtp", "exch", "demo" } });

         AddProperty<Activity[]>("RecentActivities")
            .SubscribeTo(liveDataService.RecentActivity.Select(value =>
            {
               var activities = new Queue<Activity>(Get<Activity[]>("RecentActivities")?.Reverse() ?? new Activity[] { });
               activities.Enqueue(value);
               if (activities.Count > 4)
                  activities.Dequeue();

               return activities.Reverse().ToArray();
            }));

         // Regulate data update interval to no less than every 200 msecs.
         _subscription = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Subscribe(_ => PushUpdates());
      }

      public override void Dispose()
      {
         _subscription?.Dispose();
         base.Dispose();
      }
   }
}