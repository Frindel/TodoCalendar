using EventsCalendar.Data;
using EventsCalendar.Data.Directories;
using EventsCalendar.Data.Entities;

namespace EventsCalendar.Tests;

public class CategoriesRepositoryTests
{
	[Fact]
	public void CreatingRepositoriesOfDifferentUsers()
	{
		DbLocker locker = new DbLocker();
		locker.Lock();

		try
		{
			User user1, user2;
			using (DataContext context = new DataContext(true))
			{
				user1 = new User() { RefreshToken = "123" };
				user2 = new User() { RefreshToken = "234" };

				context.Users.AddRange(user1, user2);
				context.SaveChanges();
			}

			var category1 = new Category() { Name = "category 1", UserId = user1.Id };
			var category2 = new Category() { Name = "category 2", UserId = user2.Id };

			var categoriesRep = new CategoriesDirectory();
			categoriesRep.Add(category1);
			categoriesRep.Add(category2);

			var categoryies = categoriesRep.GetAll();

			Assert.Equal(categoryies[0].Id, categoryies[1].Id);
		}
		finally
		{
			locker.Unlock();
		}
	}
}