using BlogWebApi.DataAccess;
using BlogWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.Add(new User()
                    {
                        FirstName = "Sparov",
                        MiddleName = "Peder",
                        LastName = "Karmolin"
                    });
                }

                if (!context.Sites.Any())
                {
                    context.Sites.Add(new Site()
                    {
                        Name = "My Blog Site Api",
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Name = "Post 1",
                                CreatedAt = DateTime.Now,
                                Body = "<p>Post 1 Body</p>",
                                Metadata = "tag1, tag2, tag3",
                                User = new User()
                                {
                                    FirstName = "Leonid",
                                    MiddleName = "Georgievch",
                                    LastName = "Samsonov"
                                },
                                Template = new Template()
                                {
                                    Name = "Template Sample 1",
                                    Body = "<div>Template 1 Sample</div>",
                                },
                               Attachments = new List<Attachment>()
                               {
                                   new Attachment()
                                   {
                                       Name = "Some Archive Sample 1",
                                       Link = "samplePath/archive.rar"
                                   },
                                   new Attachment()
                                   {
                                       Name = "Some Archive Sample 2",
                                       Link = "samplePath/sexPhotosArchive.rar"
                                   }
                               },
                               Comments = new List<Comment>()
                               {
                                   new Comment()
                                   {
                                       Text = "Some Comment Sample 1",
                                       CreatedDateTime = DateTime.Now,
                                       UpdatedDateTime = DateTime.Now,
                                       UserID = 2
                                   },
                                   new Comment()
                                   {
                                       Text = "Some Comment Sample 2",
                                       CreatedDateTime = DateTime.Now,
                                       UpdatedDateTime = DateTime.Now,
                                       UserID = 2
                                   }
                               }
                            }
                        }
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
