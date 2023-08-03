using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Model.Categories;

public class EditRequest
{
	[Required]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; } = null!;
}