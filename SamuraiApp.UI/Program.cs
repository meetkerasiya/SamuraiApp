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
			//AddVariousTypes();
			//AddSamuraisByName("shimada","okamoto","kikuchio","wataru");
			//AddSamuraisByName("oden");
			//GetSamurais();
			//QueryFilters();
			//QueryAggregates();
			RetrieveAndUpdateSamurai();
            Console.WriteLine("Press any key...");
			Console.ReadKey();
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