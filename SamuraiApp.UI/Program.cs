using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
	class Program
	{
		private static SamuraiContext _context=new SamuraiContext();

		private static void Main(string[] args)
		{
			_context.Database.EnsureCreated();
			GetSamurais("Before Add:");
			AddSamurai();
			GetSamurais("After Add:");
			Console.WriteLine("Press any key...");
			Console.ReadKey();
		}

        private static void AddSamurai()
        {
			//var samurai = new Samurai { Name = "Kin'emon" };
			var samurai = new Samurai { Name = "Nekomumashi" };
			_context.Samurais.Add(samurai);
			_context.SaveChanges();


		}

		private static void GetSamurais(string v)
        {
            var samurais=_context.Samurais.ToList();
			Console.WriteLine($"Samurai cont is {samurais.Count}");
			foreach( var samurai in samurais )
			{
				Console.WriteLine(samurai.Name);
			}
        }
    }
}