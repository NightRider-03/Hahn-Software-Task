using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("tasks"); // PostgreSQL convention: lowercase table names

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedNever(); // We generate GUIDs in the domain

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(1000);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>(); 

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Property(t => t.UpdatedAt);

            builder.Property(t => t.DueDate);

            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => t.DueDate);
            builder.HasIndex(t => t.Priority);
        }
    }

}
