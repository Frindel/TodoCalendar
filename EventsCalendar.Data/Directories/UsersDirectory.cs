using EventsCalendar.Data.Entities;

namespace EventsCalendar.Data.Directories;

public class UsersDirectory : IDirectory<User>
{
	public List<User> GetAll(Func<User, bool> condition = null)
	{
		using (DataContext context = new DataContext())
		{
			return (condition == null) ? context.Users.ToList() : context.Users.Where(condition).ToList();
		}
	}

	public User Get(Func<User, bool> condition)
	{
		using (DataContext context = new DataContext())
		{
			return context.Users.Where(condition).FirstOrDefault();
		}
	}

	public void Add(User entity)
	{
		using (DataContext context = new DataContext())
		{
			context.Users.Add(entity);
			context.SaveChanges();
		}
	}

	public void Edit(User entity)
	{
		using (DataContext contex = new DataContext())
		{
			contex.Users.Update(entity);
			contex.SaveChanges();
		}
	}

	public void Remove(User entity)
	{
		using (DataContext context = new DataContext())
		{
			context.Users.Remove(entity);
			context.SaveChanges();
		}
	}
}