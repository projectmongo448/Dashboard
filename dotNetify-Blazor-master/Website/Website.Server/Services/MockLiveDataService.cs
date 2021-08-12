using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Website.Server
{
   public interface ILiveDataService
   {
      IObservable<string> Download { get; }
      IObservable<string> Upload { get; }
      IObservable<string> Latency { get; }

      IObservable<string> Users { get; }
      IObservable<int[]> Traffic { get; }
      IObservable<int[]> ServerUsage { get; }
      IObservable<int[]> Utilization { get; }
      IObservable<Activity> RecentActivity { get; }
   }

   public class Activity
   {
      //private static readonly Dictionary<int, string> _activities = new Dictionary<int, string> {
      //      {1, "Offline"},
      //      {2, "Active"},
      //      {3, "Busy"},
      //      {4, "Away"},
      //      {5, "In a Call"}
      //  };

      public int Id { get; set; }
      public string PersonName { get; set; }
      public int StatusId { get; set; }
      //public string Status => _activities[StatusId];
   }

   public class MockLiveDataService : ILiveDataService
   {
        private readonly Random _random = new Random();
        private readonly IMongoCollection<Product> _download;
        private readonly IMongoCollection<List> _list;
        private readonly IMongoCollection<Mech> _Mech;

        public IObservable<string> Download { get; }

      public IObservable<string> Upload { get; }

      public IObservable<string> Latency { get; }
        public IObservable<string> Users { get; }


        public IObservable<int[]> Traffic { get; }

      public IObservable<int[]> ServerUsage { get; }

      public IObservable<int[]> Utilization { get; }

      public IObservable<Activity> RecentActivity { get; }

      public MockLiveDataService(ICustomerRepository customerRepository)
      {
            var clientMS = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            var databaseMS = clientMS.GetDatabase("MyShop");
            _download = databaseMS.GetCollection<Product>("Order");

            //var client = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            //var database = clientMS.GetDatabase("Manufacturing");
            //_list = database.GetCollection<List>("warehouse");

            var client = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            var database = client.GetDatabase("ALLtrainMescla5D");
            _Mech = database.GetCollection<Mech>("machine");


            Download = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Select(_ => $"{volt()}");

         Upload = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Select(_ => $"{rotate()}");

         Latency = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Select(_ => $"{pressure()}");

            Users = Observable
           .Interval(TimeSpan.FromMilliseconds(600))
           .StartWith(0)
           .Select(_ => $"{vibration()}");


            Traffic = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Select(_ => new int[] { GetData() });

            ServerUsage = Observable
               .Interval(TimeSpan.FromMilliseconds(400))
               .StartWith(0)
               .Select(_ => new int[] { volt(), rotate(), pressure(), vibration() });

            Utilization = Observable
            .Interval(TimeSpan.FromMilliseconds(800))
            .StartWith(0)

            .Select(_ => new int[] { volt(), rotate(), pressure(), vibration()});

            RecentActivity = Observable
            .Interval(TimeSpan.FromSeconds(2))
            .StartWith(0)
            .Select(_ => GetRandomCustomer(customerRepository))
            .Select(customer => new Activity
            {
               PersonName = ""
            })
            .StartWith(
               Enumerable.Range(1, 4)
               .Select(_ => GetRandomCustomer(customerRepository))
               .Select(customer => new Activity
               {

                  PersonName = "....."

               })
               .ToArray()
            );
      }

      private Customer GetRandomCustomer(ICustomerRepository customerRepository)
      {
         Customer record;
         while ((record = customerRepository.Get(_random.Next(1, 20))) == null)
            ;
         return record;
      }

        private int GetData()
        {
            var codeProduct = _random.Next(1, 9);
            var definevolt = _random.Next(1, 9);
            var definerotate = _random.Next(9, 15);
            var definepressure = _random.Next(9, 15);
            var definevibration = _random.Next(9, 15);
            //Insert
            _Mech.InsertOne(new Mech { machineId = $"Machine-{codeProduct}", voltmean = definevolt , rotatemean= definerotate, pressuremean=definepressure,vibrationmean=definevibration});

            //var Cost = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Cost).Sum());
            //var Income = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Income).Sum());
            var volt = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.voltmean).Last());

            return volt;

        }
        private int volt()
        {
            var volt = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.voltmean).Last());
            return volt;
        }
        private int rotate()
        {
            var volt = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.rotatemean).Last());
            return volt;
        }
        private int pressure()
        {
            var volt = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.pressuremean).Last());
            return volt;
        }
        private int vibration()
        {
            var volt = Convert.ToInt32(_Mech.Find(upload => true).ToList().Select(s => s.vibrationmean).Last());
            return volt;
        }

    }
}