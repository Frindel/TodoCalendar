using EventsCalendar.Data.Entities;

namespace EventsCalendar.Data.Directories;

public interface IDirectory<T> where T: BaseEntity
{
	public List<T> GetAll(Func<T, bool> condition = null);

	public T Get(Func<T, bool> condition);

	public void Add(T entity);

	public void Edit(T entity);

	public void Remove(T entity);
}