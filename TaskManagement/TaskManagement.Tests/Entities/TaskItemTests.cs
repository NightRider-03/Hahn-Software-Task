using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Events;
using Xunit;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Tests.Entities
{
    public class TaskItemTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateTaskItem()
        {
            var title = "Test Task";
            var description = "Test Description";
            var dueDate = DateTime.UtcNow.AddDays(1);
            var priority = 3;

            var task = new TaskItem(title, description, dueDate, priority);

            Assert.Equal(title, task.Title);
            Assert.Equal(description, task.Description);
            Assert.Equal(TaskStatus.Pending, task.Status);
            Assert.Equal(dueDate, task.DueDate);
            Assert.Equal(priority, task.Priority);
            Assert.Single(task.DomainEvents);
            Assert.IsType<TaskCreatedEvent>(task.DomainEvents.First());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidTitle_ShouldThrowArgumentException(string invalidTitle)
        {
            Assert.Throws<ArgumentException>(() => new TaskItem(invalidTitle, "Description"));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(6, 5)]
        [InlineData(-1, 1)]
        [InlineData(10, 5)]
        public void Constructor_WithInvalidPriority_ShouldClampToBounds(int inputPriority, int expectedPriority)
        {
            var task = new TaskItem("Title", "Description", priority: inputPriority);

            Assert.Equal(expectedPriority, task.Priority);
        }

        [Fact]
        public void UpdateDetails_WithValidParameters_ShouldUpdateTask()
        {
            var task = new TaskItem("Original Title", "Original Description");
            var newTitle = "Updated Title";
            var newDescription = "Updated Description";
            var newDueDate = DateTime.UtcNow.AddDays(2);
            var newPriority = 4;

            task.UpdateDetails(newTitle, newDescription, newDueDate, newPriority);

            Assert.Equal(newTitle, task.Title);
            Assert.Equal(newDescription, task.Description);
            Assert.Equal(newDueDate, task.DueDate);
            Assert.Equal(newPriority, task.Priority);
            Assert.NotNull(task.UpdatedAt);
            Assert.Equal(2, task.DomainEvents.Count);
            Assert.Contains(task.DomainEvents, e => e is TaskUpdatedEvent);
        }

        [Fact]
        public void UpdateDetails_WithNoChanges_ShouldNotAddDomainEvent()
        {
            var title = "Title";
            var description = "Description";
            var task = new TaskItem(title, description);
            var initialEventCount = task.DomainEvents.Count;

            task.UpdateDetails(title, description, null, 1);

            Assert.Equal(initialEventCount, task.DomainEvents.Count);
            Assert.Null(task.UpdatedAt);
        }

        [Fact]
        public void MarkAsCompleted_WithPendingTask_ShouldCompleteTask()
        {
            var task = new TaskItem("Title", "Description");

            task.MarkAsCompleted();

            Assert.Equal(TaskStatus.Completed, task.Status);
            Assert.NotNull(task.UpdatedAt);
            Assert.Equal(2, task.DomainEvents.Count);
            Assert.Contains(task.DomainEvents, e => e is TaskCompletedEvent);
        }

        [Fact]
        public void MarkAsCompleted_WithAlreadyCompletedTask_ShouldNotChanges()
        {
            var task = new TaskItem("Title", "Description");
            task.MarkAsCompleted();
            var eventCountAfterFirstCompletion = task.DomainEvents.Count;

            task.MarkAsCompleted();

            Assert.Equal(TaskStatus.Completed, task.Status);
            Assert.Equal(eventCountAfterFirstCompletion, task.DomainEvents.Count);
        }

        [Fact]
        public void MarkAsInProgress_WithPendingTask_ShouldChangeStatus()
        {
            var task = new TaskItem("Title", "Description");

            task.MarkAsInProgress();

            Assert.Equal(TaskStatus.InProgress, task.Status);
            Assert.NotNull(task.UpdatedAt);
        }

       

        [Fact]
        public void Cancel_WithPendingTask_ShouldCancelTask()
        {
            var task = new TaskItem("Title", "Description");

            task.Cancel();

            Assert.Equal(TaskStatus.Cancelled, task.Status);
            Assert.NotNull(task.UpdatedAt);
        }

       
    }
}
