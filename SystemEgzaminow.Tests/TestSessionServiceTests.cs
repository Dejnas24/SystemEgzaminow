using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.Tests
{
    public class TestSessionServiceTests
    {
        [Fact]
        public async Task FinishTestSessionAsync_BrakPytan_RzucaInvalidOperationException()
        {
            // Arrange
            var sessionDto = new TestSessionDto
            {
                Pytania = []
            };

            var service = new TestSessionService(null!);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.FinishTestSessionAsync(
                    sessionDto,
                    false,
                    false));

            // Assert
            Assert.Equal(
                "Nie można zakończyć testu bez pytań.",
                exception.Message);
        }

        [Fact]
        public async Task FinishTestSessionAsync_InnyPrzypadek_RzucaInvalidOperationException()
        {
            var service = new TestSessionService(null!);

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                service.FinishTestSessionAsync(
                    null,
                    false,
                    false));

            // Assert
            Assert.Equal("sessionDto", exception.ParamName);
        }
    }
}