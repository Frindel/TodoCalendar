using AutoMapper;
using EventsCalendar.Model.Categories;
using EventsCalendar.Data.Entities;

namespace EventsCalendar.Mappers;

public class CategoriesMappingProfile : Profile
{
	public CategoriesMappingProfile()
	{
		CreateMap<AddRequest, Category>();
		CreateMap<EditRequest, Category>();
		CreateMap<Category, CategoryResponse>();
	}
}