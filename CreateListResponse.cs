public class CreateListResponse
{
    public string id { get; set; }
    public string name { get; set; }
    public bool closed { get; set; }
    public string idBoard { get; set; }
    public int pos { get; set; }
    public CreateLimits limits { get; set; }
}

public class CreateLimits
{
}