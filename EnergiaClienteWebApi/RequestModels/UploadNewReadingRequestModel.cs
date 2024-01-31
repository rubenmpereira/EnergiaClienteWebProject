namespace EnergiaClienteWebApi.RequestModels
{
    public class UploadNewReadingRequestModel
    {
        public int habitation { get; set; }
        public int ponta { get; set; }
        public int cheias { get; set; }
        public int vazio { get; set; }
    }
}
