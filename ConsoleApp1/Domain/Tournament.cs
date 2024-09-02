using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public class Tournament
    {

        public void SimulateGroupStage(List<Group> groupList)
        {
            foreach (var group in groupList)
            {
                Console.WriteLine($"Grupna faza - {group.Name}");
                for (int i = 0; i < group.Teams.Count; i++)
                {
                    for (int j = i + 1; j < group.Teams.Count; j++)
                    {
                        var match = new Match(group.Teams[i], group.Teams[j]);
                        match.Simulate();
                        Console.WriteLine(match);
                    }
                }
                Console.WriteLine("-------------------------------------------------");
            }
        }
        public void PrintGroupStandings(List<Group> groups)
        {
            Console.WriteLine("Konačan plasman u grupama:");

            foreach (var group in groups)
            {
                Console.WriteLine($"    Grupa {group.Name} (Ime - pobede/porazi/bodovi/postignuti koševi/primljeni koševi/koš razlika):");

                var rankedTeams = group.RankTeams();

                for (int i = 0; i < rankedTeams.Count; i++)
                {
                    var team = rankedTeams[i];
                    Console.WriteLine($"        {i + 1}. {team.Team,-12} {team.Wins} / {team.Losses} / {team.Points} / {team.ScoredPoints} / {team.ConcededPoints} / {team.PointsDifference()}");
                }
            }
        }


        public void PrintFinalStandings(List<Group> groups)
        {
            Console.WriteLine("Konačan plasman:");

            var firstPlaceTeams = new List<BasketballTeam>();
            var secondPlaceTeams = new List<BasketballTeam>();
            var thirdPlaceTeams = new List<BasketballTeam>();


            foreach (var group in groups)
            {
                var rankedTeams = group.RankTeams();
                if (rankedTeams.Count > 0) firstPlaceTeams.Add(rankedTeams[0]);
                if (rankedTeams.Count > 1) secondPlaceTeams.Add(rankedTeams[1]);
                if (rankedTeams.Count > 2) thirdPlaceTeams.Add(rankedTeams[2]);
            }


            var rankedFirstPlaceTeams = RankTopTeams(firstPlaceTeams);
            var rankedSecondPlaceTeams = RankTopTeams(secondPlaceTeams);
            var rankedThirdPlaceTeams = RankTopTeams(thirdPlaceTeams);


            Console.WriteLine("Prvoplasirani timovi:");
            for (int i = 0; i < rankedFirstPlaceTeams.Count; i++)
            {
                var team = rankedFirstPlaceTeams[i];
                Console.WriteLine($"    {i + 1}. {team.Team,-12} {team.Points} / {team.PointsDifference()} / {team.ScoredPoints}");
            }

            Console.WriteLine("Drugoplasirani timovi:");
            for (int i = 0; i < rankedSecondPlaceTeams.Count; i++)
            {
                var team = rankedSecondPlaceTeams[i];
                Console.WriteLine($"    {i + 4}. {team.Team,-12} {team.Points} / {team.PointsDifference()} / {team.ScoredPoints}");
            }

            Console.WriteLine("Trećeplasirani timovi:");
            for (int i = 0; i < rankedThirdPlaceTeams.Count; i++)
            {
                var team = rankedThirdPlaceTeams[i];
                Console.WriteLine($"    {i + 7}. {team.Team,-12} {team.Points} / {team.PointsDifference()} / {team.ScoredPoints}");
            }
        }

        public List<BasketballTeam> RankTopTeams(List<BasketballTeam> teams)
        {
            return teams
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.PointsDifference())
                .ThenByDescending(t => t.ScoredPoints)
                .ToList();
        }

        public (List<BasketballTeam> potD, List<BasketballTeam> potE, List<BasketballTeam> potF, List<BasketballTeam> potG) CreatePots(List<Group> groups)
        {
            var firstPlaceTeams = new List<BasketballTeam>();
            var secondPlaceTeams = new List<BasketballTeam>();
            var thirdPlaceTeams = new List<BasketballTeam>();


            foreach (var group in groups)
            {
                var rankedTeams = group.RankTeams();
                if (rankedTeams.Count > 0) firstPlaceTeams.Add(rankedTeams[0]);
                if (rankedTeams.Count > 1) secondPlaceTeams.Add(rankedTeams[1]);
                if (rankedTeams.Count > 2) thirdPlaceTeams.Add(rankedTeams[2]);
            }


            var rankedFirstPlaceTeams = RankTopTeams(firstPlaceTeams);
            var rankedSecondPlaceTeams = RankTopTeams(secondPlaceTeams);
            var rankedThirdPlaceTeams = RankTopTeams(thirdPlaceTeams);


            var potD = rankedFirstPlaceTeams.Take(2).ToList();
            var potE = rankedFirstPlaceTeams.Skip(2).Take(1).Concat(rankedSecondPlaceTeams.Take(1)).ToList();
            var potF = rankedSecondPlaceTeams.Skip(1).Take(2).ToList();
            var potG = rankedThirdPlaceTeams.Take(2).ToList();

            return (potD, potE, potF, potG);
        }

        public void PrintPots(List<BasketballTeam> potD, List<BasketballTeam> potE,
        List<BasketballTeam> potF, List<BasketballTeam> potG)
        {
            void PrintPot(string potName, List<BasketballTeam> pot)
            {
                Console.WriteLine($"    {potName}");
                foreach (var team in pot)
                {
                    Console.WriteLine($"        {team.Team}");
                }
            }

            Console.WriteLine("Šeširi:");
            PrintPot("Šešir D", potD);
            PrintPot("Šešir E", potE);
            PrintPot("Šešir F", potF);
            PrintPot("Šešir G", potG);
        }

        public List<(BasketballTeam, BasketballTeam)> FormQuarterfinalPairs(List<BasketballTeam> potD, List<BasketballTeam> potE, List<BasketballTeam> potF, List<BasketballTeam> potG)
        {
            var quarterfinalPairs = new List<(BasketballTeam, BasketballTeam)>();
            var random = new Random();


            bool TryFormPairs(List<BasketballTeam> pot1, List<BasketballTeam> pot2, List<(BasketballTeam, BasketballTeam)> pairs)
            {
                if (pot1.Count == 0) return true; // All teams paired successfully

                var team = pot1[0];
                var remainingPot1 = pot1.Skip(1).ToList();

                foreach (var opponent in pot2.ToList())
                {
                    if (!HaveTeamsPlayedEachOther(team, opponent))
                    {
                        pairs.Add((team, opponent));
                        pot2.Remove(opponent);

                        if (TryFormPairs(remainingPot1, pot2, pairs))
                        {
                            return true;
                        }

                        // Backtrack
                        pairs.Remove((team, opponent));
                        pot2.Add(opponent);
                    }
                }

                return false;
            }


            bool FormAllPairs()
            {
                quarterfinalPairs.Clear();




                if (!TryFormPairs(potD, potG, quarterfinalPairs))
                {
                    return false;
                }


                if (!TryFormPairs(potE, potF, quarterfinalPairs))
                {
                    return false;
                }

                return true;
            }

            FormAllPairs();

            return quarterfinalPairs;
        }


        public List<(BasketballTeam, BasketballTeam, int, int)> SimulateKnockoutStage(string stageName, List<(BasketballTeam, BasketballTeam)> pairs)
        {
            var winners = new List<(BasketballTeam, BasketballTeam, int, int)>();
            foreach (var pair in pairs)
            {
                var match = new Match(pair.Item1, pair.Item2);
                var result = match.SimulateKnockout();
                winners.Add((result.winner, result.loser, result.winnerScore, result.loserScore));
            }
            PrintMatches(stageName, winners);
            return winners;
        }
        public List<(BasketballTeam, BasketballTeam)> FormSemifinalPairs(
    List<(BasketballTeam, BasketballTeam, int, int)> quarterfinalResults)
        {
            var semifinalPairs = new List<(BasketballTeam, BasketballTeam)>();


            if (quarterfinalResults.Count < 4)
            {
                throw new ArgumentException("Not enough quarterfinal results to form semifinals.");
            }

            // Extract winners from quarterfinal results
            var winners = quarterfinalResults.Select(result => result.Item3 > result.Item4 ? result.Item1 : result.Item2).ToList();

            // Form semifinal pairs
            semifinalPairs.Add((winners[0], winners[3]));
            semifinalPairs.Add((winners[2], winners[1]));

            return semifinalPairs;
        }



        public void PrintEliminationPhase(
            List<(BasketballTeam, BasketballTeam)> quarterfinalPairs
            )
        {
            Console.WriteLine("Eliminaciona faza:");

            Console.WriteLine("Četvrtfinale:");
            foreach (var pair in quarterfinalPairs)
            {
                Console.WriteLine($"    {pair.Item1.Team} - {pair.Item2.Team}");
            }

            Console.WriteLine();
        }

        private bool HaveTeamsPlayedEachOther(BasketballTeam teamA, BasketballTeam teamB)
        {
            return teamA.Results.ContainsKey(teamB.Team) || teamB.Results.ContainsKey(teamA.Team);
        }

        public void PrintMatches(string roundName, List<(BasketballTeam homeTeam, BasketballTeam awayTeam, int homeScore, int awayScore)> matches)
        {
            Console.WriteLine(roundName);

            foreach (var match in matches)
            {
                Console.WriteLine($"    {match.homeTeam.Team} {match.homeScore} - {match.awayScore} {match.awayTeam.Team}");
            }

            Console.WriteLine();
        }

        public void PrintMedalWinners(BasketballTeam gold, BasketballTeam silver, BasketballTeam bronze)
        {
            Console.WriteLine("Medalje:");

            Console.WriteLine($"    Zlato: {gold.Team}");
            Console.WriteLine($"    Srebro: {silver.Team}");
            Console.WriteLine($"    Bronza: {bronze.Team}");
        }

        public void SimulateBronzeAndGoldMatches(List<(BasketballTeam, BasketballTeam, int, int)> semiFinalWinners)
        {
            var bronzeMatch = new Match(semiFinalWinners[0].Item2, semiFinalWinners[1].Item2);
            var bronzeMatchResult = bronzeMatch.SimulateKnockout();
            PrintMatches("Utakmica za treće mesto", new List<(BasketballTeam, BasketballTeam, int, int)> { (bronzeMatchResult.winner, bronzeMatchResult.loser, bronzeMatchResult.winnerScore, bronzeMatchResult.loserScore) });

            Console.WriteLine("-------------------------------------------------");

            var goldMatch = new Match(semiFinalWinners[0].Item1, semiFinalWinners[1].Item1);
            var goldMatchResult = goldMatch.SimulateKnockout();
            PrintMatches("Utakmica za prvo mesto", new List<(BasketballTeam, BasketballTeam, int, int)> { (goldMatchResult.winner, goldMatchResult.loser, goldMatchResult.winnerScore, goldMatchResult.loserScore) });

            Console.WriteLine("-------------------------------------------------");

            PrintMedalWinners(goldMatchResult.winner, goldMatchResult.loser, bronzeMatchResult.winner);
        }



    }

}
