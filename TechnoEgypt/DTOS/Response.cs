namespace TechnoEgypt.DTOS
{
    public enum ResponseCode
    {
        success = 200,
        notFound = 404
    }
    public class Response<T>
    {
        public ResponseCode StatusCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
