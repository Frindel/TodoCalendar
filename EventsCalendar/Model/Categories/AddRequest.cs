using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Model.Categories;

public class AddRequest
{
	[Required]
	public string Name { get; set; } = null!;
}