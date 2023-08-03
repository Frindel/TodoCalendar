using EventsCalendar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsCalendar.Data.Directories;

public class CategoriesDirectory : IDirectory<Category>
{
	public List<Category> GetAll(Func<Category, bool> condition = null)
	{
		using (DataContext context = new DataContext())
		{
			return (condition == null) ? context.Categories.ToList() : context.Categories.Where(condition).ToList();
		}
	}

	public Category Get(Func<Category, bool> condition)
	{
		using (DataContext context = new DataContext())
		{
			return context.Categories.Where(condition).FirstOrDefault();
		}
	}

	public void Add(Category entity)
	{
		using (DataContext context = new DataContext())
		{
			int? id = context.Categories.Where(c => c.UserId == entity.UserId)
				.OrderBy(c => c.Id).Select(c => c.Id).Take(1).FirstOrDefault();

			entity.Id = (id ?? 0) + 1;

			context.Categories.Add(entity);
			context.SaveChanges();
		}
	}

	public void Edit(Category entity)
	{
		using (DataContext contex = new DataContext())
		{
			contex.Categories.Update(entity);
			contex.SaveChanges();
		}
	}

	public void Remove(Category entity)
	{
		using (DataContext context = new DataContext())
		{
			context.Categories.Remove(entity);
			context.SaveChanges();
		}
	}
}