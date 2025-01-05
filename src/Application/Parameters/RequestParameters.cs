namespace Application.Parameters
{
    public class RequestParameter
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }

        public RequestParameter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public RequestParameter(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 20 ? 10 : pageSize;
        }

        public RequestParameter(DateTime initialDate, DateTime finalDate, int pageSize, int pageNumber)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 20 ? 10 : pageSize;
            InitialDate = initialDate;
            FinalDate = finalDate;
        }
    }
}