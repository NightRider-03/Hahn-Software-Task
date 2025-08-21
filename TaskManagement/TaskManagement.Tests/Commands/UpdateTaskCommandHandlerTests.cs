using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TaskManagement.Application.Commands.UpdateTask;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Repositories;
using Xunit;

namespace TaskManagement.Application.Tests.Commands
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _handler = new UpdateTaskCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithExistingTask_ShouldUpdateTask()
        {
            var taskId = Guid.NewGuid();
            var existingTask = new TaskItem("Original Title", "Original Description");
            var command = new UpdateTaskCommand(
                taskId,
                "Updated Title",
                "Updated Description",
                DateTime.UtcNow.AddDays(2),
                4
            );

            _mockRepository.Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingTask);

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(command.Title, existingTask.Title);
            Assert.Equal(command.Description, existingTask.Description);
            Assert.Equal(command.DueDate, existingTask.DueDate);
            Assert.Equal(command.Priority, existingTask.Priority);
            _mockRepository.Verify(x => x.UpdateAsync(existingTask, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistingTask_ShouldThrowTaskNotFoundException()
        {
            var taskId = Guid.NewGuid();
            var command = new UpdateTaskCommand(
                taskId,
                "Updated Title",
                "Updated Description",
                DateTime.UtcNow.AddDays(2),
                4
            );

            _mockRepository.Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((TaskItem?)null);

            await Assert.ThrowsAsync<TaskNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
