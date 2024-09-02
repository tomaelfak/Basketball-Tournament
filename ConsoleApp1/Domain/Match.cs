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

        //nije implementirano da tim moze da se preda, to daje 0 poena
        //nije implementirano da se gledaju prethodni mecevi odnosno forma tima

        void UpdatePoints(BasketballTeam winningTeam, BasketballTeam losingTeam )
        {
            winningTeam.Points += 2;
            losingTeam.Points += 1;
        }
        public void Simulate()
        {

            var random = new Random();

            HomeTeamScore = random.Next(60, 90) + 20 - HomeTeam.FIBARanking;
            AwayTeamScore = random.Next(60, 90) + 20 - AwayTeam.FIBARanking;

            if(HomeTeamScore == AwayTeamScore)
            {
                while(HomeTeamScore == AwayTeamScore)
                {
                    HomeTeamScore += random.Next(5, 15);
                    AwayTeamScore += random.Next(5, 15);
                }
            }

            if(HomeTeamScore > AwayTeamScore)
            {
                UpdatePoints(HomeTeam, AwayTeam);
                HomeTeam.Wins++;
                AwayTeam.Losses++;
            }
            else
            {
                UpdatePoints(AwayTeam, HomeTeam);
                AwayTeam.Wins++;
                HomeTeam.Losses++;
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

            int teamAScore = random.Next(60, 90) + 20 - HomeTeam.FIBARanking;
            int teamBScore = random.Next(60, 90) + 20 - AwayTeam.FIBARanking;

            if(teamAScore == teamBScore)
            {
                while(teamAScore == teamBScore)
                {
                    teamAScore += random.Next(5, 15);
                    teamBScore += random.Next(5, 15);
                }
            }

            if(teamAScore > teamBScore)
            {
                return (HomeTeam, AwayTeam, teamAScore, teamBScore);
            }
            else
            {
                return (AwayTeam, HomeTeam, teamBScore, teamAScore);
            }



        }

        public override string ToString()
        {
            return $"{HomeTeam.Team} - {AwayTeam.Team} ({HomeTeamScore} : {AwayTeamScore}) ";
        }
    }
}