using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TaskManagement.Application.Commands.CreateTask;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;
using Xunit;

namespace TaskManagement.Application.Tests.Commands
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _handler = new CreateTaskCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateTaskAndReturnId()
        {
            var command = new CreateTaskCommand(
                "Test Task",
                "Test Description",
                DateTime.UtcNow.AddDays(1),
                3
            );

            _mockRepository.Setup(x => x.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result);
            _mockRepository.Verify(x => x.AddAsync(
                It.Is<TaskItem>(t =>
                    t.Title == command.Title &&
                    t.Description == command.Description &&
                    t.DueDate == command.DueDate &&
                    t.Priority == command.Priority),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
