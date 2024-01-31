namespace EnergiaClienteWebApi.RequestModels
{
    public class GetReadingByDateRequestModel
    {
        public int habitation { get; set; }
        public int month { get; set; }
        public int year { get; set; }

    }
}
