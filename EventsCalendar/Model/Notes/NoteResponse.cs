namespace EventsCalendar.Model.Notes;

public class NoteResponse
{

	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;
	
	public DateOnly Date { get; set; }
}