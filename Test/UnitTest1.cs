using NUnit.Framework;
using Services.Contracts;
using Moq;
using Repos.Contracts;
using Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Implementations;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestSpin()
        {
            //Arrange
            var mockBetRepo = new Mock<IBetRepo>();
            var mockSpinService = new Mock<ISpinService>();
            var bets = new List<Bet>
            {
                new Bet
                {
                    Id = 1,
                    Amount = 10,
                    BetType = "colour",
                    BetValue = "red",
                    IsWinningBet = false,
                    WasPayedout = false
                },
                new Bet
                {
                    Id = 2,
                    Amount = 50,
                    BetType = "colour",
                    BetValue = "black",
                    IsWinningBet = false,
                    WasPayedout = false
                },
                new Bet
                {
                    Id = 3,
                    Amount = 25,
                    BetType = "number",
                    BetValue = "15",
                    IsWinningBet = false,
                    WasPayedout = false
                }
            };

            mockBetRepo.Setup(x => x.GetAllNonCompletedBets()).ReturnsAsync(bets);
            mockSpinService.Setup(x => x.SaveSpin(It.IsAny<SpinResult>())).ReturnsAsync(1);
            var mockBetService = new BetService(mockBetRepo.Object, mockSpinService.Object);

            //Act
            var spinResult = await mockBetService.Spin();

            //Assert
            Assert.IsNotNull(spinResult);
            Assert.True(!string.IsNullOrEmpty(spinResult.Colour));
        }

        [Test]
        public async Task TestPlaceBet()
        {
            //Arrange
            var mockBetRepo = new Mock<IBetRepo>();
            var mockSpinService = new Mock<ISpinService>();

            var bets = new List<Bet>
            {
                new Bet
                {
                    Id = 1,
                    Amount = 10,
                    BetType = "colour",
                    BetValue = "red",
                    IsWinningBet = false,
                    WasPayedout = false
                },
                new Bet
                {
                    Id = 2,
                    Amount = 50,
                    BetType = "colour",
                    BetValue = "black",
                    IsWinningBet = false,
                    WasPayedout = false
                },
                new Bet
                {
                    Id = 3,
                    Amount = 25,
                    BetType = "number",
                    BetValue = "15",
                    IsWinningBet = false,
                    WasPayedout = false
                }
            };

            var betCount = 1;
            List<int> betIds = new List<int>();
            var mockBetService = new BetService(mockBetRepo.Object, mockSpinService.Object);
            foreach (var bet in bets)
            {
                mockBetRepo.Setup(x => x.PlaceBet(bet)).ReturnsAsync(betCount);

                //Act
                betIds.Add(await mockBetService.PlaceBet(bet));
                betCount++;
            }

            //Assert
            Assert.IsTrue(betIds.Count == 3);
        }
    }
}