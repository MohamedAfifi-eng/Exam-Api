namespace Exam_Api.DTO
{
    public class ResponseDTO<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }

    }
}
