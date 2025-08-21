using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using TaskManagement.Application.Queries.GetTasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;
using Xunit;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.Application.Tests.Queries
{
    public class GetTasksQueryHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly GetTasksQueryHandler _handler;

        public GetTasksQueryHandlerTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _handler = new GetTasksQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithoutStatusFilter_ShouldReturnAllTasks()
        {
            var tasks = new List<TaskItem>
        {
            new("Task 1", "Description 1"),
            new("Task 2", "Description 2")
        };

            var query = new GetTasksQuery();

            _mockRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(tasks);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count());
            Assert.Contains(result, dto => dto.Title == "Task 1");
            Assert.Contains(result, dto => dto.Title == "Task 2");
            _mockRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithStatusFilter_ShouldReturnFilteredTasks()
        {
            var tasks = new List<TaskItem>
        {
            new("Pending Task", "Description")
        };

            var query = new GetTasksQuery(TaskStatus.Pending);

            _mockRepository.Setup(x => x.GetByStatusAsync(TaskStatus.Pending, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(tasks);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Pending Task", result.First().Title);
            _mockRepository.Verify(x => x.GetByStatusAsync(TaskStatus.Pending, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
