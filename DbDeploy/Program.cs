using DbUp;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

class Program
{
	static int Main(string[] args)
	{
		//var connectionString = "Server=DESKTOP-HBBGGUN;Database=MyAtzenda;User Id=sa;Password=as;Connection Timeout=30;Encrypt=False;";
		//"DefaultConnection": "Server=DESKTOP-HBBGGUN;Database=MyAtzenda;User Id=sa;Password=as;Connection Timeout=30;Encrypt=False;"

		var configuration = new ConfigurationBuilder()
	   .SetBasePath(AppContext.BaseDirectory)
	   .AddJsonFile("appsettings.json", optional: false)
	   .Build();

		var connectionString = configuration.GetConnectionString("DefaultConnection");



		var upgrader = DeployChanges.To
			.SqlDatabase(connectionString)
			.WithScriptsFromFileSystem("Scripts") // Looks into the folder
			.LogToConsole()
			.Build();

		var result = upgrader.PerformUpgrade();

		if (!result.Successful)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Script execution failed:");
			Console.WriteLine(result.Error);
			Console.ResetColor();
			return -1;
		}

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("All scripts executed successfully.");
		Console.ResetColor();
		return 0;
	}
}
