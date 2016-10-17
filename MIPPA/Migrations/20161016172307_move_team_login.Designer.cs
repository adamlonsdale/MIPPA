using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Mippa.Models;

namespace MIPPA_Angular.Migrations
{
    [DbContext(typeof(MippaContext))]
    [Migration("20161016172307_move_team_login")]
    partial class move_team_login
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mippa.Models.Manager", b =>
                {
                    b.Property<int>("ManagerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("ManagerId");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("Mippa.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("PlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Mippa.Models.PlayerMatch", b =>
                {
                    b.Property<int>("PlayerMatchId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayPlayerId");

                    b.Property<int>("AwayPlayerScoreId");

                    b.Property<int>("HomePlayerId");

                    b.Property<int>("HomePlayerScoreId");

                    b.Property<bool>("Saved");

                    b.Property<int>("ScorecardId");

                    b.HasKey("PlayerMatchId");

                    b.HasIndex("AwayPlayerId");

                    b.HasIndex("AwayPlayerScoreId");

                    b.HasIndex("HomePlayerId");

                    b.HasIndex("HomePlayerScoreId");

                    b.HasIndex("ScorecardId");

                    b.ToTable("PlayerMatches");
                });

            modelBuilder.Entity("Mippa.Models.PlayerResult", b =>
                {
                    b.Property<int>("PlayerResultId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdjustedScore");

                    b.Property<int>("Dues");

                    b.Property<int>("Format");

                    b.Property<int>("Handicap");

                    b.Property<int>("PlayCount");

                    b.Property<int>("PlayerId");

                    b.Property<int>("Score");

                    b.Property<int>("ScorecardId");

                    b.Property<int>("Wins");

                    b.HasKey("PlayerResultId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("ScorecardId");

                    b.ToTable("PlayerResults");
                });

            modelBuilder.Entity("Mippa.Models.PlayerScore", b =>
                {
                    b.Property<int>("PlayerScoreId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Score");

                    b.HasKey("PlayerScoreId");

                    b.ToTable("PlayerScores");
                });

            modelBuilder.Entity("Mippa.Models.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Date");

                    b.Property<int>("SessionId");

                    b.Property<string>("Time");

                    b.HasKey("ScheduleId");

                    b.HasIndex("SessionId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Mippa.Models.Scorecard", b =>
                {
                    b.Property<int>("ScorecardId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Format");

                    b.Property<int>("State");

                    b.Property<int>("TeamMatchId");

                    b.HasKey("ScorecardId");

                    b.HasIndex("TeamMatchId");

                    b.ToTable("Scorecards");
                });

            modelBuilder.Entity("Mippa.Models.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Format");

                    b.Property<int>("ManagerId");

                    b.Property<int>("MatchupType");

                    b.Property<string>("Name");

                    b.Property<bool>("ScheduleCreated");

                    b.HasKey("SessionId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Mippa.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("SessionId");

                    b.Property<string>("UserName");

                    b.HasKey("TeamId");

                    b.HasIndex("SessionId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Mippa.Models.TeamMatch", b =>
                {
                    b.Property<int>("TeamMatchId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayTeamId");

                    b.Property<int>("HomeTeamId");

                    b.Property<int>("ScheduleId");

                    b.HasKey("TeamMatchId");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("TeamMatches");
                });

            modelBuilder.Entity("Mippa.Models.TeamResult", b =>
                {
                    b.Property<int>("TeamResultId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Dues");

                    b.Property<int>("Score");

                    b.Property<int>("ScorecardId");

                    b.Property<int>("TeamId");

                    b.Property<double>("Wins");

                    b.HasKey("TeamResultId");

                    b.HasIndex("ScorecardId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamResults");
                });

            modelBuilder.Entity("Mippa.Models.TeamRoster", b =>
                {
                    b.Property<int>("TeamRosterId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Handicap");

                    b.Property<int>("PlayerId");

                    b.Property<int>("TeamId");

                    b.HasKey("TeamRosterId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamRosters");
                });

            modelBuilder.Entity("Mippa.Models.Venue", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("State");

                    b.Property<string>("Zip");

                    b.HasKey("VenueId");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("Mippa.Models.PlayerMatch", b =>
                {
                    b.HasOne("Mippa.Models.Player", "AwayPlayer")
                        .WithMany()
                        .HasForeignKey("AwayPlayerId");

                    b.HasOne("Mippa.Models.PlayerScore", "AwayPlayerScore")
                        .WithMany()
                        .HasForeignKey("AwayPlayerScoreId");

                    b.HasOne("Mippa.Models.Player", "HomePlayer")
                        .WithMany()
                        .HasForeignKey("HomePlayerId");

                    b.HasOne("Mippa.Models.PlayerScore", "HomePlayerScore")
                        .WithMany()
                        .HasForeignKey("HomePlayerScoreId");

                    b.HasOne("Mippa.Models.Scorecard", "Scorecard")
                        .WithMany("PlayerMatches")
                        .HasForeignKey("ScorecardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mippa.Models.PlayerResult", b =>
                {
                    b.HasOne("Mippa.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mippa.Models.Scorecard", "Scorecard")
                        .WithMany("PlayerResults")
                        .HasForeignKey("ScorecardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mippa.Models.Schedule", b =>
                {
                    b.HasOne("Mippa.Models.Session", "Session")
                        .WithMany("Schedules")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mippa.Models.Scorecard", b =>
                {
                    b.HasOne("Mippa.Models.TeamMatch", "TeamMatch")
                        .WithMany("Scorecards")
                        .HasForeignKey("TeamMatchId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mippa.Models.Session", b =>
                {
                    b.HasOne("Mippa.Models.Manager", "Manager")
                        .WithMany("Sessions")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mippa.Models.Team", b =>
                {
                    b.HasOne("Mippa.Models.Session", "Session")
                        .WithMany("Teams")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mippa.Models.TeamMatch", b =>
                {
                    b.HasOne("Mippa.Models.Team", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("Mippa.Models.Team", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId");

                    b.HasOne("Mippa.Models.Schedule", "Schedule")
                        .WithMany("Matches")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mippa.Models.TeamResult", b =>
                {
                    b.HasOne("Mippa.Models.Scorecard", "Scorecard")
                        .WithMany("TeamResults")
                        .HasForeignKey("ScorecardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mippa.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("Mippa.Models.TeamRoster", b =>
                {
                    b.HasOne("Mippa.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mippa.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");
                });
        }
    }
}
