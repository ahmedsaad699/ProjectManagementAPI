using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Infrastructure.Configurations;

public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
{
    public void Configure(EntityTypeBuilder<UserProject> builder)
    {
        builder.ToTable("UserProjects");

        builder.HasKey(up => up.Id);

        builder.HasOne(up => up.User)
            .WithMany(u => u.UserProjects)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(up => up.Project)
            .WithMany(p => p.UserProjects)
            .HasForeignKey(up => up.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(up => new { up.UserId, up.ProjectId }).IsUnique();

        builder.HasQueryFilter(up => !up.IsDeleted);
    }
}