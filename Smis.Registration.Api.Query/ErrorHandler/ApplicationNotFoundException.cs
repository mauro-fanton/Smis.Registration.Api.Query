namespace Smis.Registration.Api.Query.ErrorHandler
{
    public class ApplicationNotFoundException: Exception
    {
		public ApplicationNotFoundException(string message) : base(message) { }
	}
}

