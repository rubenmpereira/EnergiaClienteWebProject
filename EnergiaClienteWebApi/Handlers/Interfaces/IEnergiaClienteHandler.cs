using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels;

namespace EnergiaClienteWebApi.Handlers.Interfaces;

public interface IEnergiaClienteHandler
{
    public dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel);
    public dbResponse<Invoice> GetInvoices(GetInvoicesRequestModel requestModel);
    public dbResponse<Reading> GetReadingByDate(GetReadingByDateRequestModel requestModel);
    public dbResponse<decimal> GetUnpaidTotal(GetUnpaidTotalRequestModel requestModel);
    public dbResponse<decimal> UploadNewReading(Reading requestModel);
    public dbResponse<Reading> GetpreviousMonthReading(GetReadingByDateRequestModel requestModel);
    public void Billing(int habitationId, int billingMonth, int billingYear);
    public dbResponse<User> GetUserDetails(GetUserDetailsRequestModel requestModel);
    public dbResponse<Holder> GetHolderDetails(GetHolderDetailsRequestModel requestModel);
    public dbResponse<Habitation> GetHabitationDetails(GetHabitationDetailsRequestModel requestModel);
    public dbResponse<string> UpdateHabitationPower(UpdateHabitationPowerRequestModel requestModel);
    public dbResponse<string> UpdateHolderName(UpdateHolderNameRequestModel requestModel);
    public dbResponse<string> UpdateHolderNif(UpdateHolderNifRequestModel requestModel);
    public dbResponse<string> UpdateHolderContact(UpdateHolderContactRequestModel requestModel);
    public dbResponse<string> UpdateHabitationTensionLevel(UpdateHabitationTensionLevelRequestModel requestModel);
    public dbResponse<string> UpdateHabitationSchedule(UpdateHabitationScheduleRequestModel requestModel);
    public dbResponse<string> UpdateHabitationPhase(UpdateHabitationPhaseRequestModel requestModel);
    public dbResponse<int> GetHabitationIds();

}
