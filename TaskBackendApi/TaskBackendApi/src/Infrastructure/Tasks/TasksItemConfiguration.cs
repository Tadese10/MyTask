using Domain.Tasks;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Tasks;

internal sealed class TasksItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.DueDate).HasConversion(d => d != null ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);

        builder.HasOne<User>().WithMany().HasForeignKey(t => t.UserId);
    }
}
