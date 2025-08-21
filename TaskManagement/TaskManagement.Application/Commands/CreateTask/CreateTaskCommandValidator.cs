using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TaskManagement.Application.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future")
                .When(x => x.DueDate.HasValue);

            RuleFor(x => x.Priority)
                .InclusiveBetween(1, 5).WithMessage("Priority must be between 1 and 5");
        }
    }
}
