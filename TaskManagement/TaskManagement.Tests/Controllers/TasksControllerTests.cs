using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.API.Controllers;
using TaskManagement.Application.Commands.CompleteTask;
using TaskManagement.Application.Commands.CreateTask;
using TaskManagement.Application.Commands.UpdateTask;
using TaskManagement.Application.Common;
using TaskManagement.Application.Queries.GetTaskById;
using TaskManagement.Application.Queries.GetTasks;
using Xunit;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;
using FluentValidation.Results;


namespace TaskManagement.API.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _controller = new TasksController(_serviceProviderMock.Object);
        }

        #region GetTasks Tests

        [Fact]
        public async Task GetTasks_WithoutStatus_ReturnsAllTasks()
        {
            var expectedTasks = new List<TaskDto>
        {
            new(Guid.NewGuid(), "Task 1", "Description 1", TaskStatus.Pending, null, 1, DateTime.UtcNow, null),
            new(Guid.NewGuid(), "Task 2", "Description 2", TaskStatus.Completed, null, 2, DateTime.UtcNow, DateTime.UtcNow)
        };

            var queryHandlerMock = new Mock<IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>>();
            queryHandlerMock
                .Setup(x => x.Handle(It.IsAny<GetTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>)))
                .Returns(queryHandlerMock.Object);

            var result = await _controller.GetTasks();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualTasks = Assert.IsAssignableFrom<IEnumerable<TaskDto>>(okResult.Value);
            Assert.Equal(expectedTasks.Count(), actualTasks.Count());

            queryHandlerMock.Verify(x => x.Handle(
                It.Is<GetTasksQuery>(q => q.Status == null),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTasks_WithStatus_ReturnsFilteredTasks()
        {
            var status = TaskStatus.Pending;
            var expectedTasks = new List<TaskDto>
        {
            new(Guid.NewGuid(), "Task 1", "Description 1", TaskStatus.Pending, null, 1, DateTime.UtcNow, null)
        };

            var queryHandlerMock = new Mock<IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>>();
            queryHandlerMock
                .Setup(x => x.Handle(It.IsAny<GetTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>)))
                .Returns(queryHandlerMock.Object);

            var result = await _controller.GetTasks(status);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualTasks = Assert.IsAssignableFrom<IEnumerable<TaskDto>>(okResult.Value);
            Assert.Single(actualTasks);

            queryHandlerMock.Verify(x => x.Handle(
                It.IsAny<GetTasksQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region GetTask Tests

        [Fact]
        public async Task GetTask_ExistingId_ReturnsTask()
        {
            var taskId = Guid.NewGuid();
            var expectedTask = new TaskDetailDto(
                taskId,
                "Test Task",
                "Test Description",
                TaskStatus.Pending,
                null,
                1,
                DateTime.UtcNow,
                null,
                false);

            var queryHandlerMock = new Mock<IQueryHandler<GetTaskByIdQuery, TaskDetailDto?>>();
            queryHandlerMock
                .Setup(x => x.Handle(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTask);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IQueryHandler<GetTaskByIdQuery, TaskDetailDto?>)))
                .Returns(queryHandlerMock.Object);

            var result = await _controller.GetTask(taskId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualTask = Assert.IsType<TaskDetailDto>(okResult.Value);
            Assert.Equal(expectedTask.Id, actualTask.Id);
            Assert.Equal(expectedTask.Title, actualTask.Title);

            queryHandlerMock.Verify(x => x.Handle(
                It.Is<GetTaskByIdQuery>(q => q.Id == taskId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTask_NonExistingId_ReturnsNotFound()
        {
            var taskId = Guid.NewGuid();

            var queryHandlerMock = new Mock<IQueryHandler<GetTaskByIdQuery, TaskDetailDto?>>();
            queryHandlerMock
                .Setup(x => x.Handle(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskDetailDto?)null);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IQueryHandler<GetTaskByIdQuery, TaskDetailDto?>)))
                .Returns(queryHandlerMock.Object);

            var result = await _controller.GetTask(taskId);

            Assert.IsType<NotFoundResult>(result.Result);

            queryHandlerMock.Verify(x => x.Handle(
                It.Is<GetTaskByIdQuery>(q => q.Id == taskId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region CreateTask Tests

        [Fact]
        public async Task CreateTask_ValidCommand_ReturnsCreatedResult()
        {
            var command = new CreateTaskCommand("Test Task", "Test Description", DateTime.UtcNow.AddDays(1), 1);
            var expectedId = Guid.NewGuid();

            var validatorMock = new Mock<IValidator<CreateTaskCommand>>();
            validatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var commandHandlerMock = new Mock<ICommandHandler<CreateTaskCommand, Guid>>();
            commandHandlerMock
                .Setup(x => x.Handle(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedId);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IValidator<CreateTaskCommand>)))
                .Returns(validatorMock.Object);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(ICommandHandler<CreateTaskCommand, Guid>)))
                .Returns(commandHandlerMock.Object);

            var result = await _controller.CreateTask(command);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(TasksController.GetTask), createdResult.ActionName);
            Assert.Equal(expectedId, createdResult.Value);
            Assert.Equal(expectedId, createdResult.RouteValues!["id"]);

            validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            commandHandlerMock.Verify(x => x.Handle(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateTask_InvalidCommand_ReturnsBadRequest()
        {
            var command = new CreateTaskCommand("", "Test Description", DateTime.UtcNow.AddDays(1), 1);
            var validationErrors = new List<ValidationFailure>
        {
            new("Title", "Title is required")
        };

            var validatorMock = new Mock<IValidator<CreateTaskCommand>>();
            validatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationErrors));

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IValidator<CreateTaskCommand>)))
                .Returns(validatorMock.Object);

            var result = await _controller.CreateTask(command);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(validationErrors, badRequestResult.Value);

            validatorMock.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region UpdateTask Tests

        [Fact]
        public async Task UpdateTask_ValidRequest_ReturnsNoContent()
        {
            var taskId = Guid.NewGuid();
            var request = new UpdateTaskRequest("Updated Task", "Updated Description", DateTime.UtcNow.AddDays(2), 2, TaskStatus.InProgress);

            var validatorMock = new Mock<IValidator<UpdateTaskCommand>>();
            validatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var commandHandlerMock = new Mock<ICommandHandler<UpdateTaskCommand>>();
            commandHandlerMock
                .Setup(x => x.Handle(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IValidator<UpdateTaskCommand>)))
                .Returns(validatorMock.Object);

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(ICommandHandler<UpdateTaskCommand>)))
                .Returns(commandHandlerMock.Object);

            var result = await _controller.UpdateTask(taskId, request);

            Assert.IsType<NoContentResult>(result);

            validatorMock.Verify(x => x.ValidateAsync(
                It.Is<UpdateTaskCommand>(c => c.Id == taskId && c.Title == request.Title),
                It.IsAny<CancellationToken>()), Times.Once);

            commandHandlerMock.Verify(x => x.Handle(
                It.Is<UpdateTaskCommand>(c => c.Id == taskId && c.Title == request.Title),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTask_InvalidRequest_ReturnsBadRequest()
        {
            var taskId = Guid.NewGuid();
            var request = new UpdateTaskRequest("", "Updated Description", DateTime.UtcNow.AddDays(2), 2, TaskStatus.InProgress);
            var validationErrors = new List<ValidationFailure>
        {
            new("Title", "Title is required")
        };

            var validatorMock = new Mock<IValidator<UpdateTaskCommand>>();
            validatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationErrors));

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IValidator<UpdateTaskCommand>)))
                .Returns(validatorMock.Object);

            var result = await _controller.UpdateTask(taskId, request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(validationErrors, badRequestResult.Value);

            validatorMock.Verify(x => x.ValidateAsync(
                It.Is<UpdateTaskCommand>(c => c.Id == taskId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region CompleteTask Tests

        [Fact]
        public async Task CompleteTask_ValidId_ReturnsNoContent()
        {
            var taskId = Guid.NewGuid();

            var commandHandlerMock = new Mock<ICommandHandler<CompleteTaskCommand>>();
            commandHandlerMock
                .Setup(x => x.Handle(It.IsAny<CompleteTaskCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Unit.Value));

            _serviceProviderMock
                .Setup(x => x.GetService(typeof(ICommandHandler<CompleteTaskCommand>)))
                .Returns(commandHandlerMock.Object);

            var result = await _controller.CompleteTask(taskId);

            Assert.IsType<NoContentResult>(result);

            commandHandlerMock.Verify(x => x.Handle(
                It.Is<CompleteTaskCommand>(c => c.Id == taskId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Integration Tests Setup

        [Fact]
        public void Controller_Constructor_InitializesCorrectly()
        {
            var controller = new TasksController(_serviceProviderMock.Object);

            Assert.NotNull(controller);
        }

        #endregion
    }
}
