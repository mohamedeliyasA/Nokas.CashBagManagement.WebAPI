using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nokas.CashBagManagement.WebAPI.DBContext;
using Nokas.CashBagManagement.WebAPI.Entities;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI.Repository
{
    public class SQLBagRegistrationRepo : IBagRegistrationRepo
    {
        private readonly BagRegistrationDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SQLBagRegistrationRepo> _logger;

        public SQLBagRegistrationRepo(BagRegistrationDBContext context, IMapper mapper, ILogger<SQLBagRegistrationRepo> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BagRegistrationRequest?> GetBagByNumberForClientAsync(string bagNumber, string clientId)
        {
            var entity = await _context.BagRegistration
                .Where(b => b.BagRegistration.BagNumber == bagNumber && b.ClientId == clientId)
                .FirstOrDefaultAsync();

            return entity == null ? null : _mapper.Map<BagRegistrationRequest>(entity);
        }

        public async Task<BagRegistrationRequest> CreateBagRegistration(BagRegistrationRequest bagRegistrationRequest)
        {
            if (bagRegistrationRequest == null)
                throw new ArgumentNullException(nameof(bagRegistrationRequest));

            try
            {

                var entity = _mapper.Map<BagRegistrationEntity>(bagRegistrationRequest);
                
                entity.Status = "In-Progress"; // Default status
                _context.BagRegistration.Add(entity);
                await _context.SaveChangesAsync();
                return _mapper.Map<BagRegistrationRequest>(entity);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "DB constraint violation: {Message}", dbEx.GetBaseException()?.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating bag registration.");
                throw;
            }
        }

    }
}
