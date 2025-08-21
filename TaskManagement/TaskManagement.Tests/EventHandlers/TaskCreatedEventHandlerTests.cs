using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagement.Application.Queries.EventHandlers;
using TaskManagement.Domain.Events;
using Xunit;

namespace TaskManagement.Application.Tests.EventHandlers
{
    public class TaskCreatedEventHandlerTests
    {
        private readonly Mock<ILogger<TaskCreatedEventHandler>> _mockLogger;
        private readonly TaskCreatedEventHandler _handler;

        public TaskCreatedEventHandlerTests()
        {
            _mockLogger = new Mock<ILogger<TaskCreatedEventHandler>>();
            _handler = new TaskCreatedEventHandler(_mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithTaskCreatedEvent_ShouldLogInformation()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var title = "Test Task";
            var domainEvent = new TaskCreatedEvent(taskId, title);

            // Act
            await _handler.Handle(domainEvent, CancellationToken.None);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Task created: {taskId} - {title}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
