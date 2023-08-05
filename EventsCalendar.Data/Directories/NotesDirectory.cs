using EventsCalendar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsCalendar.Data.Directories;

public class NotesDirectory : IDirectory<Note>
{
	public List<Note> GetAll(Func<Note, bool> condition = null)
	{
		using (DataContext context = new DataContext())
		{
			var request = context.Notes.Include(n => n.Categories);
			return (condition == null) ? request.ToList() : request.Where(condition).ToList();
		}
	}

	public Note Get(Func<Note, bool> condition)
	{
		using (DataContext context = new DataContext())
		{
			return context.Notes.Include(n => n.Categories).Where(condition).FirstOrDefault();
		}
	}

	public void Add(Note entity)
	{
		using (DataContext context = new DataContext())
		{
			int? id = context.Notes.Where(n => n.UserId == entity.UserId)
				.OrderByDescending(n => n.Id).Select(n => n.Id).Take(1).FirstOrDefault();

			entity.Id = (id ?? 0) + 1;

			context.Notes.Add(entity);
			context.SaveChanges();
		}
	}

	public void Edit(Note entity)
	{
		using (DataContext context = new DataContext())
		{
			var currentNote = context.Notes.Include(n => n.Categories)
				.Where(n => n.UserId == entity.UserId && n.Id == entity.Id)
				.First();

			var existingCategories = currentNote.Categories;
			var newCategories = entity.Categories.ToList();
			
			if (entity.Categories.Count == 0)
				currentNote.Categories.Clear();

			for (int index = 0; index<existingCategories.Count; index++)
			{
				bool exist = false;

				foreach (var addedCategory in newCategories)
					if (addedCategory.Id == existingCategories[index].Id)
					{
						newCategories.Remove(addedCategory);
						exist = true;
						break;
					}

				if (!exist)
				{
					existingCategories.Remove(existingCategories[index]);
					index--;
				}
			}

			foreach (var addedCategory in newCategories)
				currentNote.Categories.Add(context.Categories.Find(addedCategory.Id, addedCategory.UserId));
			
			context.SaveChanges();
		}
	}

	public void Remove(Note entity)
	{
		using (DataContext context = new DataContext())
		{
			context.Notes.Remove(entity);
			context.SaveChanges();
		}
	}
}