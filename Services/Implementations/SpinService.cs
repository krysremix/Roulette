using Infrastructure;
using Microsoft.Extensions.Logging;
using Repos.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class SpinService : ISpinService
    {
        private readonly ISpinRepo _spinRepo;
        private readonly ILogger<SpinService> _logger;

        public SpinService(ILogger<SpinService> logger, ISpinRepo spinRepo)
        {
            _logger = logger;
            _spinRepo = spinRepo;
        }

        public async Task<int> SaveSpin(SpinResult spin)
        {
            return await _spinRepo.SaveSpin(spin);
        }

        public async Task<List<Spin>> ShowPreviousSpins()
        {
            return (await _spinRepo.ShowPreviousSpins()).ToList();
        }
    }
}
