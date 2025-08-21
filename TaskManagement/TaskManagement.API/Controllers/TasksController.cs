using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Commands.CompleteTask;
using TaskManagement.Application.Commands.CreateTask;
using TaskManagement.Application.Commands.UpdateTask;
using TaskManagement.Application.Common;
using TaskManagement.Application.Queries.GetTaskById;
using TaskManagement.Application.Queries.GetTasks;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class TasksController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public TasksController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks([FromQuery] TaskStatus? status = null)
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>>();
            var query = new GetTasksQuery(status);
            var result = await handler.Handle(query, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDetailDto>> GetTask(Guid id)
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<GetTaskByIdQuery, TaskDetailDto?>>();
            var query = new GetTaskByIdQuery(id);
            var result = await handler.Handle(query, CancellationToken.None);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTask([FromBody] CreateTaskCommand command)
        {
            var validator = _serviceProvider.GetRequiredService<IValidator<CreateTaskCommand>>();
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var handler = _serviceProvider.GetRequiredService<ICommandHandler<CreateTaskCommand, Guid>>();
            var result = await handler.Handle(command, CancellationToken.None);

            return CreatedAtAction(nameof(GetTask), new { id = result }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
        {
            var command = new UpdateTaskCommand(id, request.Title, request.Description, request.DueDate, request.Priority,request.Status);

            var validator = _serviceProvider.GetRequiredService<IValidator<UpdateTaskCommand>>();
            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var handler = _serviceProvider.GetRequiredService<ICommandHandler<UpdateTaskCommand>>();
            await handler.Handle(command, CancellationToken.None);

            return NoContent();
        }

        [HttpPatch("{id}/complete")]
        public async Task<ActionResult> CompleteTask(Guid id)
        {
            var command = new CompleteTaskCommand(id);
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<CompleteTaskCommand>>();
            await handler.Handle(command, CancellationToken.None);

            return NoContent();
        }
    }

    public record UpdateTaskRequest(
        string Title,
        string Description,
        DateTime? DueDate,
        int Priority,
        TaskStatus? Status = null
    );
}
