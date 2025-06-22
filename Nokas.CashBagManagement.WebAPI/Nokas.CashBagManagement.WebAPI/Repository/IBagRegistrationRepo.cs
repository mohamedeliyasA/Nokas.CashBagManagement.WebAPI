using Microsoft.AspNetCore.Mvc;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI.Repository
{
    public interface IBagRegistrationRepo
    {
        Task<BagRegistrationRequest?> GetBagByNumberForClientAsync(string bagNumber, string clientId);

        Task<BagRegistrationRequest> CreateBagRegistration(BagRegistrationRequest bagRegistrationRequest);

        Task<BagRegistrationRequest> UpdateBagRegistration(BagRegistrationRequest bagUpdate);
    }
}
