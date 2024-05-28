using Infrastructure;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Roulette.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Controllers
{
    [ApiController]
    [Route("roulette")]
    public class RouletteController : ControllerBase
    {
        private readonly IBetService _betService;
        private readonly ISpinService _spinService;
        private readonly ILogger<RouletteController> _logger;

        public RouletteController(IBetService betService, ILogger<RouletteController> logger, ISpinService spinService)
        {
            _betService = betService;
            _logger = logger;
            _spinService = spinService;
        }

        [HttpPost]
        [Route("placebet")]
        public async Task<IActionResult> PlaceBet(PlaceBetRequest request)
        {
            var bet = new Bet
            {
                Amount = request.Amount,
                BetType = request.BetType,
                BetValue = request.BetValue
            };

            var betId = await _betService.PlaceBet(bet);

            return Ok(new
            {
                Message = "Success",
                BetId = betId
            });
        }

        [HttpPost]
        [Route("spin")]
        public async Task<IActionResult> Spin()
        {
            try
            {
                var result = await _betService.Spin();

                if (result == null)
                    return StatusCode(500);

                return Ok(result);
            }
            catch (NoBetPlacedException _)
            {
                return Ok("No bet were placed for this spin");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while spinning", ex);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("payout")]
        public async Task<IActionResult> Payout()
        {
            try
            {
                await _betService.PayoutBets();
                return Ok(new { Message = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while paying out", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("showPreviousSpins")]
        public async Task<IActionResult> ShowPreviousSpins()
        {
            var spins = await _spinService.ShowPreviousSpins();
            var previousSpins = spins.Select(x => new PreviousSpinsResponse
            {
                Colour = x.Colour,
                Number = x.Number,
                Time = x.TimeValue
            }).ToList();
            return Ok(previousSpins);
        }
    }
}
