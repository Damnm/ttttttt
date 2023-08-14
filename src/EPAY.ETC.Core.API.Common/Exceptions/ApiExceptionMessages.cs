namespace EPAY.ETC.Core.API.Core.Exceptions
{
    public class ApiExceptionMessages
    {
        public static string ExceptionMessages(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex.Message;
            }
            return ex.Message + "  " + ExceptionMessages(ex.InnerException);
        }
    }
}
