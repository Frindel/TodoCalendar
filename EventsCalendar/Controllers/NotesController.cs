using System.Security.Claims;
using AutoMapper;
using EventsCalendar.Data.Directories;
using Microsoft.AspNetCore.Mvc;
using EventsCalendar.Data.Entities;
using EventsCalendar.Model.Notes;
using Microsoft.AspNetCore.Authorization;
using EventsCalendar.Model;
using EventsCalendar.Model.Categories;

namespace EventsCalendar.Controllers;

[Authorize]
[Route("api/notes")]
public class NotesController : Controller
{
	private IDirectory<Note> _notesDirectory;
	private IDirectory<Category> _categoriesDirectory;
	private IMapper _mapper;

	private int _userId
	{
		get => int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
	}

	public NotesController(IDirectory<Note> notesDirectory, IDirectory<Category> categoriesDirectory, IMapper mapper)
	{
		_notesDirectory = notesDirectory;
		_categoriesDirectory = categoriesDirectory;
		_mapper = mapper;
	}

	[HttpGet]
	public IActionResult GetAll()
	{
		List<Note> notes = _notesDirectory.GetAll(c => c.UserId == _userId);
		return Ok(_mapper.Map<List<NoteResponse>>(notes));
	}

	[HttpGet("{id}")]
	public IActionResult Get([FromRoute] int id)
	{
		try
		{
			var note = _notesDirectory.Get(c => c.UserId == _userId && c.Id == id);
			if (note == null)
				return BadRequest(new ErrorMessage("note is not found"));

			return Ok(_mapper.Map<NoteResponse>(note));
		}
		catch (Exception)
		{
			return BadRequest(ErrorMessage.FatalError);
		}
	}

	[HttpPost]
	public IActionResult Add([FromBody] AddNoteRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ErrorMessage.NoValid);

		try
		{
			Note newNote = _mapper.Map<Note>(request);
			newNote.UserId = _userId;
			_notesDirectory.Add(newNote);

			return Ok(_mapper.Map<NoteResponse>(newNote));
		}
		catch (Exception)
		{
			return BadRequest(ErrorMessage.FatalError);
		}
	}

	[HttpPut]
	public IActionResult Edit([FromBody] EditNoteRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ErrorMessage.NoValid);

		try
		{
			Note editableNote = _mapper.Map<Note>(request);
			editableNote.UserId = _userId;
			if (_notesDirectory.Get(c => c.Id == editableNote.Id
			                             && editableNote.UserId == _userId) == null)
				return BadRequest(new ErrorMessage("note is not found"));

			_notesDirectory.Edit(editableNote);

			return Ok(_mapper.Map<NoteResponse>(editableNote));
		}
		catch (Exception)
		{
			return BadRequest(ErrorMessage.FatalError);
		}
	}

	[HttpDelete]
	public IActionResult Delete([FromBody] DeleteNoteRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ErrorMessage.NoValid);

		try
		{
			Note removedNote = _notesDirectory.Get(c => c.UserId == _userId && c.Id == request.Id);
			if (removedNote == null)
				return BadRequest(new ErrorMessage("category is not found"));

			_notesDirectory.Remove(removedNote);

			return Ok();
		}
		catch (Exception)
		{
			return BadRequest(ErrorMessage.FatalError);
		}
	}

	[HttpGet("{id}/categories")]
	public IActionResult GetCategories([FromRoute] int id)
	{
		Note note = _notesDirectory.Get(n => n.UserId == _userId && n.Id == id);
		if (note == null)
			return BadRequest(new ErrorMessage("note not found"));

		return Ok(_mapper.Map<List<CategoryResponse>>(note.Categories));
	}

	[HttpPost("{id}/categories")]
	public IActionResult AddCategory([FromRoute] int id, [FromBody] NoteCategoryRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ErrorMessage.NoValid);

		Note note = _notesDirectory.Get(n => n.UserId == _userId && n.Id == id);
		if (note == null)
			return BadRequest(new ErrorMessage("note not found"));

		Category category = _categoriesDirectory.Get(c => c.Id == request.Id && c.UserId == _userId);
		if (category == null)
			return BadRequest(new ErrorMessage("category not found"));

		if (note.Categories.Where(c => c.Id == category.Id && c.UserId == category.UserId).FirstOrDefault() != null)
			return Ok(new ErrorMessage("category is already set"));

		note.Categories.Add(category);
		_notesDirectory.Edit(note);

		return Ok();
	}

	[HttpDelete("{id}/categories")]
	public IActionResult DeleteCategory([FromRoute] int id, [FromBody] NoteCategoryRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ErrorMessage.NoValid);

		Note note = _notesDirectory.Get(n => n.UserId == _userId && n.Id == id);
		if (note == null)
			return BadRequest(new ErrorMessage("note not found"));

		Category category = _categoriesDirectory.Get(c => c.Id == request.Id);
		if (category == null)
			return BadRequest(new ErrorMessage("category not found"));

		Category removedCategory = note.Categories.Where(c => c.Id == category.Id && c.UserId == category.UserId)
			.FirstOrDefault();
		if (removedCategory == null)
			return Ok(new ErrorMessage("category is unset"));

		note.Categories.Remove(removedCategory);
		_notesDirectory.Edit(note);

		return Ok();
	}
}