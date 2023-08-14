namespace EPAY.ETC.Core.API.Core.Exceptions
{
    public class ETCEPAYCoreAPIException : Exception
    {
        public int DbExceptionCode { get; set; }
        public string ErrorMessage { get; set; }

        public ETCEPAYCoreAPIException(int dbExceptionCode, string errorMessage)
        {
            DbExceptionCode = dbExceptionCode;
            ErrorMessage = errorMessage;
        }
    }
}
