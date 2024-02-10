using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.Models.EnergiaCliente;
using EnergiaClienteWebApi.Models.User;

namespace EnergiaClienteWebApi.Databases.Interfaces;

public interface IEnergiaClienteDatabase
{
    public CostKwh GetCostKwh();
    public dbResponse<Reading> GetReadings(GetReadingsModel model);
    public dbResponse<Invoice> GetInvoices(GetInvoicesModel model);
    public dbResponse<Reading> GetRealReadings(GetReadingsModel model);
    public dbResponse<Reading> GetReadingByDate(GetReadingByDateModel model);
    public dbResponse<decimal> GetUnpaidTotal(GetUnpaidTotalModel model);
    public dbResponse<string> InsertReading(InsertReadingModel model);
    public dbResponse<string> InsertInvoice(InsertInvoiceModel model);
    public dbResponse<User> GetUserDetails(GetUserDetailsModel model);
    public dbResponse<Holder> GetHolderDetails(GetHolderDetailsModel model);
    public dbResponse<Habitation> GetHabitationDetails(GetHabitationDetailsModel model);
    public dbResponse<string> UpdateHabitationPower(UpdateHabitationPowerModel model);
    public dbResponse<string> UpdateHolderName(UpdateHolderNameModel model);
    public dbResponse<string> UpdateHolderNif(UpdateHolderNifModel model);
    public dbResponse<string> UpdateHolderContact(UpdateHolderContactModel model);
    public dbResponse<string> UpdateHabitationTensionLevel(UpdateHabitationTensionLevelModel model);
    public dbResponse<string> UpdateHabitationSchedule(UpdateHabitationScheduleModel model);
    public dbResponse<string> UpdateHabitationPhase(UpdateHabitationPhaseModel model);
    public dbResponse<int> GetHabitationIds();
    public dbResponse<bool> AutherizeHabitation(AutherizeHabitationModel model);
    public dbResponse<bool> AuthenticateUser(AuthenticateUserModel model);
}
