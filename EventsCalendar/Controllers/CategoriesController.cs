using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventsCalendar.Data.Directories;
using EventsCalendar.Data.Entities;
using EventsCalendar.Model;
using EventsCalendar.Model.Categories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventsCalendar.Controllers;

[Authorize]
[Route("api/categories")]
public class CategoriesController : Controller
{
	private IDirectory<Category> _categoriesDirectory;
	private IMapper _mapper;

	private int _userId
	{
		get => int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
	}

	public CategoriesController(IDirectory<Category> categoriesDirectory, IMapper mapper)
	{
		_categoriesDirectory = categoriesDirectory;
		_mapper = mapper;
	}

	[HttpGet]
	public IActionResult GetAll()
	{
		List<Category> categories = _categoriesDirectory.GetAll(c => c.UserId == _userId);

		return Ok(_mapper.Map<List<CategoryResponse>>(categories));
	}

	[HttpGet("{id}")]
	public IActionResult Get([FromRoute] int id)
	{
		try
		{
			var category = _categoriesDirectory.Get(c => c.UserId == _userId && c.Id == id);

			if (category == null)
				return BadRequest(new ErrorMessage("category is not found"));

			return Ok(_mapper.Map<CategoryResponse>(category));
		}
		catch (Exception)
		{
			return BadRequest(ErrorMessage.FatalError);
		}
	}

	[HttpPost]
	public IActionResult Add([FromBody] AddRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ErrorMessage.NoValid);

		try
		{
			Category newCategory = _mapper.Map<Category>(request);
			newCategory.UserId = _userId;
			_categoriesDirectory.Add(newCategory);

			return Ok(_mapper.Map<CategoryResponse>(newCategory));
		}
		catch (Exception)
		{
			return BadRequest(ErrorMessage.FatalError);
		}
	}

	[HttpPut]
	public IActionResult Edit([FromBody] EditRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ErrorMessage.NoValid);

		try
		{
			Category editableCategory = _mapper.Map<Category>(request);
			editableCategory.UserId = _userId;

			if (_categoriesDirectory.Get(c => c.Id == editableCategory.Id
			                                  && editableCategory.UserId == _userId) == null)
				return BadRequest(new ErrorMessage("category is not found"));

			_categoriesDirectory.Edit(editableCategory);
			
			return Ok(_mapper.Map<CategoryResponse>(editableCategory));
		}
		catch (Exception)
		{
			return BadRequest(ErrorMessage.FatalError);
		}
	}

	[HttpDelete]
	public IActionResult Delete([FromBody] DeleteRequest request)
	{
		if (!ModelState.IsValid)
			return BadRequest(ErrorMessage.NoValid);
			
		try
		{
			Category removedCategory = _categoriesDirectory.Get(c => c.UserId == _userId && c.Id == request.Id);

			if (removedCategory == null)
				return BadRequest(new ErrorMessage("category is not found"));

			_categoriesDirectory.Remove(removedCategory);

			return Ok();
		}
		catch (Exception)
		{
			return BadRequest(ErrorMessage.FatalError);
		}
	}
}