using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels;

namespace EnergiaClienteWebApi.Databases.Interfaces;

public interface IEnergiaClienteDatabase
{
    public CostKwh GetCostKwh();
    public dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel);
    public dbResponse<Invoice> GetInvoices(GetInvoicesRequestModel requestModel);
    public dbResponse<Reading> GetRealReadings(GetReadingsRequestModel requestModel);
    public dbResponse<Reading> GetReadingByDate(GetReadingByDateRequestModel requestModel);
    public dbResponse<decimal> GetUnpaidTotal(GetUnpaidTotalRequestModel requestModel);
    public dbResponse<string> InsertReading(InsertReadingRequestModel requestModel);
    public dbResponse<string> InsertInvoice(InsertInvoiceRequestModel requestModel);
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
