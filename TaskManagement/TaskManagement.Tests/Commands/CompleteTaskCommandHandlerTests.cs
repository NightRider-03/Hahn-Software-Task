using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using TaskManagement.Application.Commands.CompleteTask;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Repositories;
using Xunit;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.Application.Tests.Commands
{
    public class CompleteTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly CompleteTaskCommandHandler _handler;

        public CompleteTaskCommandHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _handler = new CompleteTaskCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithExistingTask_ShouldCompleteTask()
        {
            var taskId = Guid.NewGuid();
            var existingTask = new TaskItem("Test Task", "Test Description");
            var command = new CompleteTaskCommand(taskId);

            _mockRepository.Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(existingTask);

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(TaskStatus.Completed, existingTask.Status);
            _mockRepository.Verify(x => x.UpdateAsync(existingTask, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistingTask_ShouldThrowTaskNotFoundException()
        {
            var taskId = Guid.NewGuid();
            var command = new CompleteTaskCommand(taskId);

            _mockRepository.Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((TaskItem?)null);

            await Assert.ThrowsAsync<TaskNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
