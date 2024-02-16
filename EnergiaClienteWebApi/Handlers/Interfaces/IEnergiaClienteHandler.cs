using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels.EnergiaCliente;
using EnergiaClienteWebApi.RequestModels.User;
using EnergiaClienteWebApi.Models.EnergiaCliente;
using EnergiaClienteWebApi.Models.User;

namespace EnergiaClienteWebApi.Handlers.Interfaces;

public interface IEnergiaClienteHandler
{
    public int habitation { get; set; }
    public dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel);
    public dbResponse<Invoice> GetInvoices();
    public dbResponse<Reading> GetReadingByDate(GetReadingByDateRequestModel requestModel);
    public dbResponse<decimal> GetUnpaidTotal();
    public dbResponse<decimal> UploadNewReading(UploadNewReadingRequestModel requestModel);
    public dbResponse<Reading> GetpreviousMonthReading(GetReadingByDateRequestModel requestModel);
    public dbResponse<string> Billing(BillingRequestModel requestModel);
    public dbResponse<User> GetUserDetails(GetUserDetailsRequestModel requestModel);
    public dbResponse<Holder> GetHolderDetails();
    public dbResponse<Habitation> GetHabitationDetails();
    public dbResponse<string> UpdateHabitationPower(UpdateHabitationPowerRequestModel requestModel);
    public dbResponse<string> UpdateHolderName(UpdateHolderNameRequestModel requestModel);
    public dbResponse<string> UpdateHolderNif(UpdateHolderNifRequestModel requestModel);
    public dbResponse<string> UpdateHolderContact(UpdateHolderContactRequestModel requestModel);
    public dbResponse<string> UpdateHabitationTensionLevel(UpdateHabitationTensionLevelRequestModel requestModel);
    public dbResponse<string> UpdateHabitationSchedule(UpdateHabitationScheduleRequestModel requestModel);
    public dbResponse<string> UpdateHabitationPhase(UpdateHabitationPhaseRequestModel requestModel);
    public dbResponse<int> GetHabitationIds();
    public dbResponse<bool> AutherizeHabitation(AutherizeHabitationModel requestModel);
    public dbResponse<bool> AuthenticateUser(AuthRequestModel requestModel);
}
