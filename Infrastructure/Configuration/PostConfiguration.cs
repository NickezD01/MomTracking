using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {

            //Post-CommentEntity relationship
            builder.HasMany(c => c.Comments)
                 .WithOne(c => c.Post)
                 .HasForeignKey(c => c.PostId)
                 .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
