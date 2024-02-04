namespace EnergiaClienteWebApi.Domains;

public class dbResponse<T>
{
    public dbResponse()
    {
        this.Result= new List<T>();
        this.Status = new StatusObject(false);
    }
    public dbResponse(T value)
    {
        this.Result = new List<T>() { value };
        this.Status = new StatusObject(false);
    }
    public dbResponse(List<T> values)
    {
        this.Result = values;
        this.Status = new StatusObject(false);
    }
    public StatusObject Status { get; set; }
    public List<T> Result { get; set; }
}

public class StatusObject
{
    public StatusObject() { }
    public StatusObject(bool _error)
    {
        this.Error = _error;
        this.ErrorMessage = "";
        if (_error == false)
            this.StatusCode = 200;
    }
    public StatusObject(int code)
    {
        this.StatusCode = code;
        switch (code)
        {
            case 200:
                this.Error = false;
                break;
            case 400:
                this.Error = true;
                this.ErrorMessage = "Bad request";
                break;
            case 401:
                this.Error = true;
                this.ErrorMessage = "Unautherized";
                break;
            case 403:
                this.Error = true;
                this.ErrorMessage = "Forbidden";
                break;
            case 404:
                this.Error = true;
                this.ErrorMessage = "Not found";
                break;
            case 500:
                this.Error = true;
                this.ErrorMessage = "Internal server error";
                break;
            default:

                break;
        }
    }
    public StatusObject(int code, string message)
    {
        this.StatusCode = code;
        this.Error = true;
        this.ErrorMessage = message;
    }
    public bool Error { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
}

public record Reading
{
    public int Id { get; set; }
    public int Vazio { get; set; }
    public int Ponta { get; set; }
    public int Cheias { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime ReadingDate { get; set; }
    public int HabitationId { get; set; }
    public bool Estimated { get; set; }
}

public record Habitation
{
    public string? userEmail { get; set; }
    public decimal power { get; set; }
    public string? phase { get; set; }
    public string? tensionLevel { get; set; }
    public string? schedule { get; set; }
    public CostKwh? costKwh { get; set; }
}

public record Invoice
{
    public string? Number { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Paid { get; set; }
    public decimal Value { get; set; }
    public DateTime LimitDate { get; set; }
    public int HabitationId { get; set; }
    public byte[]? Document { get; set; }
}

public record User
{
    public string? email { get; set; }
    public string? fullName { get; set; }
    public string? nif { get; set; }
    public string? contact { get; set; }
    public bool gender { get; set; }
}

public record Holder
{
    public int HabitationId { get; set; }
    public string? fullName { get; set; }
    public string? nif { get; set; }
    public string? contact { get; set; }
}

public record CostKwh(decimal costkwhPonta, decimal costkwhCheias, decimal costkwhVazio);
