using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Model.Notes;

public class NoteCategoryRequest
{
	[Required]
	public int Id { get; set; }
}