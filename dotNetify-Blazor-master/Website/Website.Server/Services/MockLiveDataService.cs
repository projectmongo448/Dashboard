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

        public IObservable<string> Download { get; }

      public IObservable<string> Upload { get; }

      public IObservable<string> Latency { get; }


      public IObservable<int[]> Traffic { get; }

      public IObservable<int[]> ServerUsage { get; }

      public IObservable<int[]> Utilization { get; }

      public IObservable<Activity> RecentActivity { get; }

      public MockLiveDataService(ICustomerRepository customerRepository)
      {
            var clientMS = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            var databaseMS = clientMS.GetDatabase("MyShop");
            _download = databaseMS.GetCollection<Product>("Order");

            var client = new MongoClient(MongoClientSettings.FromConnectionString("mongodb+srv://localhost:root@cluster0.q9nwx.mongodb.net/myFirstDatabase?retryWrites=true&w=majority"));
            var database = clientMS.GetDatabase("Manufacturing");
            _list = database.GetCollection<List>("warehouse");

            Download = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Select(_ => $"{Profit()}");

         Upload = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Select(_ => $"{Cost()}");

         Latency = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Select(_ => $"{Income()}");


         Traffic = Observable
            .Interval(TimeSpan.FromMilliseconds(600))
            .StartWith(0)
            .Select(_ => new int[] { GetData() });

         ServerUsage = Observable
            .Interval(TimeSpan.FromMilliseconds(400))
            .StartWith(0)
            .Select(_ => Enumerable.Range(1, 10).Select(i => _random.Next(1, 100)).ToArray());

         Utilization = Observable
            .Interval(TimeSpan.FromMilliseconds(800))
            .StartWith(0)
            .Select(_ => Enumerable.Range(1, 3).Select(i => 10).ToArray());

            RecentActivity = Observable
            .Interval(TimeSpan.FromSeconds(2))
            .StartWith(0)
            .Select(_ => GetRandomCustomer(customerRepository))
            .Select(customer => new Activity
            {
               PersonName = $"{Product()}"
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
            var defineCost = _random.Next(1, 9);
            var defineIncome = _random.Next(9, 15);
            //Insert
            _list.InsertOne(new List { Product_Name = $"Product-{codeProduct}", Cost = defineCost , Income= defineIncome});

            var Cost = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Cost).Sum());
            var Income = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Income).Sum());


            return Income - Cost;

        }

        private int Profit()
        {
            var Cost = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Cost).Sum());
            var Income = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Income).Sum());
            return Income - Cost;
        }
        private int Cost()
        {
            var Cost = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Cost).Sum());
            return Cost;
        }
        private int Income()
        {
            var Income = Convert.ToInt32(_list.Find(upload => true).ToList().Select(s => s.Income).Sum());
            return Income;
        }
        private string Product()
        {
            var Name = _list.Find(upload => true).ToList().Select(s => s.Product_Name).Last();
            return Name;
        }
    }
}