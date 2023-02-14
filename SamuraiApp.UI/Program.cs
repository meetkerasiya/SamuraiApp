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

			//AddSamurai("Kin'emon","raizo","kawamatsu");
			GetSamurais();
			Console.WriteLine("Press any key...");
			Console.ReadKey();
		}

        private static void AddSamurai(params string[] names)
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