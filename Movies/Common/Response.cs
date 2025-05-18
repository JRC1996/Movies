namespace Movies.Common
{
    public class Response
    {
        public Response() 
        {
            this.Success = false;

        }
        public bool Success { get; set; }

        public string Message { get; set; }

        public Object Data { get; set; }
    }
}

