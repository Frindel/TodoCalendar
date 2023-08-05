using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Model.Notes;

public class EditNoteRequest
{
	[Required]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; } = null!;

	[Required]
	public string Description { get; set; } = null!;

	[Required]
	public DateOnly Date { get; set; }
}