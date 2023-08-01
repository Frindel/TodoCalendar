using EventsCalendar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventsCalendar.Data;

public class DataContext : DbContext
{
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Category> Categories { get; set; } = null!;
	public DbSet<Note> Notes { get; set; } = null!;

	public DataContext(bool cleanDb = false)
	{
		if (cleanDb)
			Database.EnsureDeleted();
		Database.EnsureCreated();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
	{
		var builder = new ConfigurationBuilder();
		
		builder.SetBasePath(Directory.GetCurrentDirectory());
		builder.AddJsonFile("appsettings.json");
		var config = builder.Build();

		// получение строки подключения
		string connectionString = config.GetConnectionString("DefaultConnection");

		optBuilder.UseSqlite(connectionString);
	}
}