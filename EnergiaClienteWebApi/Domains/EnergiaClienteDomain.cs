namespace EnergiaClienteWebApi.Domains;

public class dbResponse<T>
{
    public dbResponse()
    {
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
    public List<T>? Result { get; set; }
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
    public decimal CostPontaKwh { get; set; }
    public decimal CostCheiasKwh { get; set; }
    public decimal CostVazioKwh { get; set; }

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

public record CostKwh(decimal costkwhPonta, decimal costkwhCheias, decimal costkwhVazio);
