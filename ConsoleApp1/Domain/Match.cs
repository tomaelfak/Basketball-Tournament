using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public class Match
    {
        public BasketballTeam HomeTeam { get; set; }

        public BasketballTeam AwayTeam { get; set; }

        public int HomeTeamScore { get; set; }

        public int AwayTeamScore { get; set; }

        public Match(BasketballTeam homeTeam, BasketballTeam awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }



        void UpdatePoints(BasketballTeam winningTeam, BasketballTeam losingTeam, bool isForfeit = false)
        {
            if (isForfeit)
            {
                winningTeam.Points += 2;
                losingTeam.Points += 0;
            }
            else
            {
                winningTeam.Points += 2;
                losingTeam.Points += 1;
            }
        }
        
        public void Simulate()
        {

            var random = new Random();

            int forfeitChance = random.Next(0, 200);



            if (forfeitChance < 2) 
            {
                HomeTeamScore = 0;
                AwayTeamScore = 30;
                UpdatePoints(AwayTeam, HomeTeam, true);
                HomeTeam.Losses++;
                AwayTeam.Wins++;
                HomeTeam.UpdateForm('L');
                AwayTeam.UpdateForm('W');
                return;
            }
            else if (forfeitChance < 4) 
            {
                AwayTeamScore = 0;
                HomeTeamScore = 30;
                UpdatePoints(HomeTeam, AwayTeam, true);
                AwayTeam.Losses++;
                HomeTeam.Wins++;
                AwayTeam.UpdateForm('L');
                HomeTeam.UpdateForm('W');
                return;
            }

            HomeTeamScore = random.Next(60, 90) + 20 - HomeTeam.FIBARanking + HomeTeam.Form;
            AwayTeamScore = random.Next(60, 90) + 20 - AwayTeam.FIBARanking + AwayTeam.Form;

            if (HomeTeamScore == AwayTeamScore)
            {
                while (HomeTeamScore == AwayTeamScore)
                {
                    HomeTeamScore += random.Next(5, 15);
                    AwayTeamScore += random.Next(5, 15);
                }
            }

            if (HomeTeamScore > AwayTeamScore)
            {
                UpdatePoints(HomeTeam, AwayTeam);
                HomeTeam.Wins++;
                AwayTeam.Losses++;
                HomeTeam.UpdateForm('W');
                AwayTeam.UpdateForm('L');
            }
            else
            {
                UpdatePoints(AwayTeam, HomeTeam);
                AwayTeam.Wins++;
                HomeTeam.Losses++;
                AwayTeam.UpdateForm('W');
                HomeTeam.UpdateForm('L');
            }

            HomeTeam.ScoredPoints += HomeTeamScore;
            HomeTeam.ConcededPoints += AwayTeamScore;
            AwayTeam.ScoredPoints += AwayTeamScore;
            AwayTeam.ConcededPoints += HomeTeamScore;

            HomeTeam.AddResult(AwayTeam.Team, HomeTeamScore - AwayTeamScore);
            AwayTeam.AddResult(HomeTeam.Team, AwayTeamScore - HomeTeamScore);


        }

        public (BasketballTeam winner, BasketballTeam loser, int winnerScore, int loserScore) SimulateKnockout()
        {
            var random = new Random();

            int teamAScore = random.Next(60, 90) + 20 - HomeTeam.FIBARanking + HomeTeam.Form;
            int teamBScore = random.Next(60, 90) + 20 - AwayTeam.FIBARanking + AwayTeam.Form;

            if (teamAScore == teamBScore)
            {
                while (teamAScore == teamBScore)
                {
                    teamAScore += random.Next(5, 15);
                    teamBScore += random.Next(5, 15);
                }
            }

            if (teamAScore > teamBScore)
            {
                HomeTeam.UpdateForm('W');
                AwayTeam.UpdateForm('L');
                return (HomeTeam, AwayTeam, teamAScore, teamBScore);
            }
            else
            {
                HomeTeam.UpdateForm('L');
                AwayTeam.UpdateForm('W');
                return (AwayTeam, HomeTeam, teamBScore, teamAScore);
            }



        }

        public override string ToString()
        {
            return $"{HomeTeam.Team} - {AwayTeam.Team} ({HomeTeamScore} : {AwayTeamScore}) ";
        }
    }
}