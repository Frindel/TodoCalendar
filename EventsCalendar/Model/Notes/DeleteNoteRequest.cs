using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Model.Notes;

public class DeleteNoteRequest
{
	[Required]
	public int Id { get; set; }
}