namespace PMES.Model.ApiResponse;

public class ResponseBase<T> where T : class, new()
{
    public T? data { get; set; }
    public Status status { get; set; }
}

public class Status
{
    public int code { get; set; }
    public string statusMsg { get; set; }
}