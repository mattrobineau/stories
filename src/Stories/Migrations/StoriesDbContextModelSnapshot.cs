using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Stories.Data.DbContexts;

namespace Stories.Migrations
{
    [DbContext(typeof(StoriesDbContext))]
    partial class StoriesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Stories.Data.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("ContentMarkdown");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEdited");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int?>("ParentCommentId");

                    b.Property<int?>("ScoreId");

                    b.Property<int>("StoryId");

                    b.Property<int>("Upvotes");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("StoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Stories.Data.Entities.CommentScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CommentId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<double>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CommentId")
                        .IsUnique();

                    b.ToTable("CommentScores");
                });

            modelBuilder.Entity("Stories.Data.Entities.Referral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Code");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<Guid>("ReferrerUserId");

                    b.HasKey("Id");

                    b.HasIndex("ReferrerUserId");

                    b.ToTable("Referrals");
                });

            modelBuilder.Entity("Stories.Data.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Stories.Data.Entities.Story", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("DescriptionMarkdown");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("Upvotes");

                    b.Property<string>("Url");

                    b.Property<Guid>("UserId");

                    b.Property<bool>("UserIsAuthor");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("Stories.Data.Entities.StoryScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("StoryId");

                    b.Property<double>("Value");

                    b.HasKey("Id");

                    b.HasIndex("StoryId")
                        .IsUnique();

                    b.ToTable("StoryScores");
                });

            modelBuilder.Entity("Stories.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<Guid>("EmailVerificationCode");

                    b.Property<bool>("IsBanned");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<string>("PasswordSalt")
                        .IsRequired();

                    b.Property<int?>("SettingsId");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.HasIndex("SettingsId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Stories.Data.Entities.UserBan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BannedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("ExpiryDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Notes");

                    b.Property<string>("Reason");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BannedByUserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBans");
                });

            modelBuilder.Entity("Stories.Data.Entities.UserRole", b =>
                {
                    b.Property<Guid>("RoleId");

                    b.Property<Guid>("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Stories.Data.Entities.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("Stories.Data.Entities.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CommentId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int?>("StoryId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("StoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("Stories.Data.Entities.Comment", b =>
                {
                    b.HasOne("Stories.Data.Entities.Comment", "ParentComment")
                        .WithMany("Replies")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("Stories.Data.Entities.Story", "Story")
                        .WithMany("Comments")
                        .HasForeignKey("StoryId");

                    b.HasOne("Stories.Data.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Stories.Data.Entities.CommentScore", b =>
                {
                    b.HasOne("Stories.Data.Entities.Comment", "Comment")
                        .WithOne("Score")
                        .HasForeignKey("Stories.Data.Entities.CommentScore", "CommentId");
                });

            modelBuilder.Entity("Stories.Data.Entities.Referral", b =>
                {
                    b.HasOne("Stories.Data.Entities.User", "Referrer")
                        .WithMany("Referrals")
                        .HasForeignKey("ReferrerUserId");
                });

            modelBuilder.Entity("Stories.Data.Entities.Story", b =>
                {
                    b.HasOne("Stories.Data.Entities.User", "User")
                        .WithMany("Stories")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Stories.Data.Entities.StoryScore", b =>
                {
                    b.HasOne("Stories.Data.Entities.Story", "Story")
                        .WithOne("Score")
                        .HasForeignKey("Stories.Data.Entities.StoryScore", "StoryId");
                });

            modelBuilder.Entity("Stories.Data.Entities.User", b =>
                {
                    b.HasOne("Stories.Data.Entities.UserSettings", "Settings")
                        .WithMany()
                        .HasForeignKey("SettingsId");
                });

            modelBuilder.Entity("Stories.Data.Entities.UserBan", b =>
                {
                    b.HasOne("Stories.Data.Entities.User", "BannedByUser")
                        .WithMany()
                        .HasForeignKey("BannedByUserId");

                    b.HasOne("Stories.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Stories.Data.Entities.UserRole", b =>
                {
                    b.HasOne("Stories.Data.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("Stories.Data.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Stories.Data.Entities.Vote", b =>
                {
                    b.HasOne("Stories.Data.Entities.Comment", "Comment")
                        .WithMany("Votes")
                        .HasForeignKey("CommentId");

                    b.HasOne("Stories.Data.Entities.Story", "Story")
                        .WithMany("Votes")
                        .HasForeignKey("StoryId");

                    b.HasOne("Stories.Data.Entities.User", "User")
                        .WithMany("Votes")
                        .HasForeignKey("UserId");
                });
        }
    }
}
