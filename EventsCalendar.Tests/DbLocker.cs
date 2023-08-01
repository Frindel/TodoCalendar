namespace EventsCalendar.Tests;

public class DbLocker
{
	private bool _lockThisObj;
	static object _lock = new();

	public DbLocker()
	{
		_lockThisObj = false;
	}
	
	public void Lock()
	{
		Monitor.Enter(_lock);
		_lockThisObj = true;
	}

	public void Unlock()
	{
		if (!_lockThisObj)
			return;

		Monitor.Exit(_lock);
		_lockThisObj = false;
	}
}