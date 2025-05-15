using DbUp;
using System;
using System.Reflection;

class Program
{
	static int Main(string[] args)
	{
		var connectionString = "Server=localhost;Database=AztendaDb;Trusted_Connection=True;";

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
