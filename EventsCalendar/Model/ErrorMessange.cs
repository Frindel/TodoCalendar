using System.Text.Json.Serialization;

namespace EventsCalendar.Model;

public class ErrorMessage
{
	public static ErrorMessage FatalError { get; }
	public static ErrorMessage NoValid { get; }

	static ErrorMessage()
	{
		FatalError = new ErrorMessage("fatal error");
		NoValid = new ErrorMessage("No valid");
	}

	[JsonPropertyName("ErrorMessage")] 
	public string Text { get; set; } = null!;

	public ErrorMessage(string text)
	{
		Text = text;
	}
}