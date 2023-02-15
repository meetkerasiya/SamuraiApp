using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
	class Program
	{
		private static SamuraiContext _context=new SamuraiContext();

        private static void Main(string[] args)
		{
			//_context.Database.EnsureCreated();

			//InsertNewSamuraiWithAQuote();

			//AddQuoteToExistingSamuraiWhileTracked();
			//AddQuoteToExistingSamuraiNotTracked(33);

			//EagerLoadSamuraiWithQuotes();
			//FilteringWithRelatedData();

			//ModifyingRelatedDataWhenNotTracked();

			QueryUsingRawSql();
            Console.WriteLine("Press any key...");
			Console.ReadKey();
		}

        private static void QueryUsingRawSql()
        {
			var samurais = _context.Samurais.FromSqlRaw("Select Id, Name from samurais")
				.Include(s=>s.Quotes).ToList();
			//var samurais = _context.Samurais.FromSqlRaw("Select * from samurais").ToList();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
			var samurai = _context.Samurais.Include(s => s.Quotes)
				.FirstOrDefault(s => s.Id == 33);
			var quote = samurai.Quotes[0];
			quote.Text += " ,Luffy said that to vivi";

			using var newContext = new SamuraiContext();
			//newContext.Quotes.Update(quote);
			newContext.Entry(quote).State = EntityState.Modified;
			newContext.SaveChanges();
        }

        private static void FilteringWithRelatedData()
		{
			var samurais=_context.Samurais
				.Where(s=>s.Quotes.Any(q=>q.Text.Contains("stupid")))
				.ToList();
		}
        private static void EagerLoadSamuraiWithQuotes()
        {
			var samuraiWithQuotes = _context.Samurais
				.Include(s => s.Quotes.Where(
				q=>q.Text.Contains("naïve. "))).ToList();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
			samurai.Quotes.Add(new Quote
			{
				Text = "When I leave this country you will always have fodd to eat."
			});
			using(var newContext=new SamuraiContext())
			{
				newContext.Samurais.Attach(samurai);
				//newContext.Samurais.Update(samurai);
				newContext.SaveChanges();
			}
			
        }
		 private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
			samurai.Quotes.Add(new Quote
			{
				Text = "Worry not! I am here."
			});
			_context.SaveChanges();
        }

        private static void InsertNewSamuraiWithAQuote()
		{
			var samurai = new Samurai
			{
				Name = "Roronoa Zoro",
				Quotes = new List<Quote>
				{
					new Quote {Text = "Scars on the back are a swordsman's shame."},
					new Quote {Text = "Only I can call my dream stupid!"}
				}
				//Name = "Monkey D. Luffy",
				//Quotes = new List<Quote>
				//{
				//	new Quote {Text = "You want to keep everyone from dying? That’s naïve. Its war. People die."}
				//}

			};
			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}
        private static void QueryAndUpdateBattles_Disconnected()
        {
			List<Battle> disconnectedBattles;
			using(var context1=new SamuraiContext())
			{
				disconnectedBattles = _context.Battles.ToList();
			}
			disconnectedBattles.ForEach(b =>
			{
				b.StartDate = new DateTime(2022, 04, 18);
				b.EndDate = new DateTime(2022, 04, 25);

			});

			using(var context2=new SamuraiContext())
			{
				context2.UpdateRange(disconnectedBattles);
				context2.SaveChanges();
			}
        }

        private static void RetrieveAndDeleteSamurai()
        {
			var name = "kin'emon";
            var samurai=_context.Samurais.Where(s=>s.Name==name).FirstOrDefault();
			_context.Samurais.Remove(samurai);
			_context.SaveChanges();
        }

        private static void QueryFilters()
		{
			var name = "raizo";
			var samurais=_context.Samurais.Where(s=>s.Name==name).ToList();
			//var samurais=_context.Samurais.Where(s=>s.Name=="raizo").ToList();
			//cause above commented way will convert query with hard code insted we use 
			//uncommented as it creates parameterized query

			samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "%e%")).ToList();

		}
        private static void QueryAggregates()
        {
			var name = "raizo";
			var samurai=_context.Samurais.FirstOrDefault(s=>s.Name==name);
			//we cannot use ToList in ubove way.
			//var samurais=_context.Samurais.Where(s=>s.Name==name).FirstOrDefault().ToList();
        }

        private static void RetrieveAndUpdateSamurai()
		{
			//var samurai = _context.Samurais.FirstOrDefault();
			//samurai.Name+="San";
			var samurais = _context.Samurais.Skip(1).Take(5).ToList();
			samurais.ForEach(s => s.Name += "San");
			_context.SaveChanges();
            
        }

        private static void AddVariousTypes()
		{
			_context.AddRange(
			
				new Samurai { Name = "otama" },
				new Samurai { Name = "Ryuma" },
			
				new Battle { Name="Battle of onigashima"},
				new Battle { Name="Battle of Thriller Bark"}
				);
			
			//_context.Samurais.AddRange(
			
			//	new Samurai { Name = "otama" },
			//	new Samurai { Name = "Ryuma" });
			//_context.Battles.AddRange(
			//	new Battle { Name="Battle of onigashima"},
			//	new Battle { Name="Battle of Thriller Bark"});
			_context.SaveChanges();
		}

        private static void AddSamuraisByName(params string[] names)
        {
			
			foreach(string name in names)
			{
				_context.Samurais.Add(new Samurai { Name= name } );	
			}
			_context.SaveChanges();


		}

		private static void GetSamurais()
        {
            var samurais=_context.Samurais
				.TagWith("ConsoleApp.Program.GetSamurais method")
				.ToList();
			Console.WriteLine($"Samurai cont is {samurais.Count}");
			foreach( var samurai in samurais )
			{
				Console.WriteLine(samurai.Name);
			}
        }
    }
}