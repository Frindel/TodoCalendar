using EventsCalendar.Data;
using EventsCalendar.Data.Directories;
using EventsCalendar.Data.Entities;

namespace EventsCalendar.Tests;

public class NotesDirectoryTests
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

			var note1 = new Note() { Name = "category 1", UserId = user1.Id };
			var note2 = new Note() { Name = "category 2", UserId = user2.Id };

			var notesRep = new NotesDirectory();
			notesRep.Add(note1);
			notesRep.Add(note2);

			var notes = notesRep.GetAll().ToList();

			Assert.Equal(notes[0].Id, notes[1].Id);
		}
		finally
		{
			locker.Unlock();
		}
	}
}