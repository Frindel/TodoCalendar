using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventsCalendar.Data.Entities;

[PrimaryKey(nameof(Id),nameof(UserId))]
public class Category : BaseEntity
{
	[Column(Order = 0)]
	public new int Id { get; set; }

	[Column(Order = 1)]
	public int UserId { get; set; }

	public string Name { get; set; } = null!;
}