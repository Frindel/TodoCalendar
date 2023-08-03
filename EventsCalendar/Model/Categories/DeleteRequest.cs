using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Model.Categories;

public class DeleteRequest
{
	[Required]
	public int Id { get; set; }
}