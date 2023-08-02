using EventsCalendar.Data.Entities;

namespace EventsCalendar.Data.Directories;

public class NotesDirectory : IDirectory<Note>
{
	public List<Note> GetAll(Func<Note, bool> condition = null)
	{
		using (DataContext context = new DataContext())
		{
			return (condition == null) ? context.Notes.ToList() : context.Notes.Where(condition).ToList();
		}
	}

	public Note Get(Func<Note, bool> condition)
	{
		using (DataContext context = new DataContext())
		{
			return context.Notes.Where(condition).FirstOrDefault();
		}
	}

	public Note Add(Note entity)
	{
		using (DataContext context = new DataContext())
		{
			int? id = context.Notes.Where(n => n.UserId == entity.UserId)
				.OrderByDescending(n => n.Id).Select(n => n.Id).Take(1).FirstOrDefault();

			entity.Id = (id ?? 0) + 1;

			context.Notes.Add(entity);
			context.SaveChanges();

			return entity;
		}
	}

	public Note Edit(Note entity)
	{
		using (DataContext contex = new DataContext())
		{
			contex.Notes.Update(entity);
			contex.SaveChanges();

			return entity;
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