namespace SolarService.DataContract;

public class Response
{
    public int Code { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public bool Status { get; set; } = false;
}