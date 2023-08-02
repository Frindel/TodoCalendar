using EventsCalendar.Data.Entities;

namespace EventsCalendar.Data.Directories;

public interface IDirectory<T> where T: BaseEntity
{
	public List<T> GetAll(Func<T, bool> condition = null);

	public T Get(Func<T, bool> condition);

	public T Add(T entity);

	public T Edit(T entity);

	public void Remove(T entity);
}