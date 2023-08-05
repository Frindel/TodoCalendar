using AutoMapper;
using EventsCalendar.Model.Notes;
using EventsCalendar.Data.Entities;

namespace EventsCalendar.Mappers;

public class NotesMappingProfile : Profile
{
	public NotesMappingProfile()
	{
		CreateMap<AddNoteRequest, Note>();
		CreateMap<EditNoteRequest, Note>();
		CreateMap<Note, NoteResponse>();
	}
}