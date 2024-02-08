namespace EnergiaClienteWebApi.RequestModels
{
    public class InsertReadingRequestModel
    {
        public int Vazio { get; set; }
        public int Ponta { get; set; }
        public int Cheias { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime ReadingDate { get; set; }
        public int HabitationId { get; set; }
        public bool Estimated { get; set; }
    }
}