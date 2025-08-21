using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using TaskManagement.Application.Queries.GetTaskById;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;
using Xunit;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.Application.Tests.Queries
{
    public class GetTaskByIdQueryHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly GetTaskByIdQueryHandler _handler;

        public GetTaskByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _handler = new GetTaskByIdQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithExistingTask_ShouldReturnTaskDetailDto()
        {
            var taskId = Guid.NewGuid();
            var task = new TaskItem("Test Task", "Test Description", DateTime.UtcNow.AddDays(1), 3);
            var query = new GetTaskByIdQuery(taskId);

            _mockRepository.Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(task);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(task.Id, result.Id);
            Assert.Equal(task.Title, result.Title);
            Assert.Equal(task.Description, result.Description);
            Assert.Equal(task.DueDate, result.DueDate);
            Assert.Equal(task.Priority, result.Priority);
            Assert.False(result.IsOverdue); 
        }

        [Fact]
        public async Task Handle_WithOverdueTask_ShouldReturnTaskWithOverdueFlag()
        {
            var taskId = Guid.NewGuid();
            var task = new TaskItem("Overdue Task", "Description", DateTime.UtcNow.AddDays(-1), 3);
            var query = new GetTaskByIdQuery(taskId);

            _mockRepository.Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(task);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsOverdue);
        }

        [Fact]
        public async Task Handle_WithNonExistingTask_ShouldReturnNull()
        {
            var taskId = Guid.NewGuid();
            var query = new GetTaskByIdQuery(taskId);

            _mockRepository.Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((TaskItem?)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
