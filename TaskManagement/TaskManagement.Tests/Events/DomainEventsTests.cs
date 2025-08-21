using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Events;
using Xunit;

namespace TaskManagement.Domain.Tests.Events
{
    public class DomainEventsTests
    {
        [Fact]
        public void TaskCreatedEvent_ShouldHaveCorrectProperties()
        {
            var taskId = Guid.NewGuid();
            var title = "Test Task";

            var domainEvent = new TaskCreatedEvent(taskId, title);

            Assert.Equal(taskId, domainEvent.TaskId);
            Assert.Equal(title, domainEvent.Title);
            Assert.NotEqual(Guid.Empty, domainEvent.Id);
            Assert.True(domainEvent.OccurredOn <= DateTime.UtcNow);
            Assert.True(domainEvent.OccurredOn > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void TaskCompletedEvent_ShouldHaveCorrectProperties()
        {
            var taskId = Guid.NewGuid();
            var title = "Test Task";
            var completedAt = DateTime.UtcNow;

            var domainEvent = new TaskCompletedEvent(taskId, title, completedAt);

            Assert.Equal(taskId, domainEvent.TaskId);
            Assert.Equal(title, domainEvent.Title);
            Assert.Equal(completedAt, domainEvent.CompletedAt);
            Assert.NotEqual(Guid.Empty, domainEvent.Id);
            Assert.True(domainEvent.OccurredOn <= DateTime.UtcNow);
        }

        [Fact]
        public void TaskUpdatedEvent_ShouldHaveCorrectProperties()
        {
            var taskId = Guid.NewGuid();
            var title = "Updated Task";

            var domainEvent = new TaskUpdatedEvent(taskId, title);

            Assert.Equal(taskId, domainEvent.TaskId);
            Assert.Equal(title, domainEvent.Title);
            Assert.NotEqual(Guid.Empty, domainEvent.Id);
            Assert.True(domainEvent.OccurredOn <= DateTime.UtcNow);
        }
    }
}
