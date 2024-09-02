using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public class Group
    {
        public string Name { get; set; }

        public List<BasketballTeam> Teams { get; set; }

        public Group(string name, List<BasketballTeam> teams)
        {
            Name = name;
            Teams = teams;
        }

        public List<BasketballTeam> RankTeams()
        {
            var rankedTeams = Teams.OrderByDescending(x => x.Points).ToList();

            // Handle ties for two teams
            for (int i = 0; i < rankedTeams.Count - 1; i++)
            {
                var teamA = rankedTeams[i];
                var teamB = rankedTeams[i + 1];

                if (teamA.Points == teamB.Points)
                {
                    if (teamA.Results.ContainsKey(teamB.Team))
                    {
                        if (teamA.Results[teamB.Team] < 0)
                        {
                            rankedTeams[i] = teamB;
                            rankedTeams[i + 1] = teamA;
                        }
                    }
                }
            }

            // Handle ties for three or more teams
            var tiedGroups = rankedTeams.GroupBy(t => t.Points)
                                        .Where(g => g.Count() > 2)
                                        .ToList();

            foreach (var group in tiedGroups)
            {
                var tiedTeams = group.ToList();

                tiedTeams.Sort((teamA, teamB) =>
                {
                    int differenceA = 0;
                    int differenceB = 0;

                    foreach (var opponent in tiedTeams)
                    {
                        if (opponent != teamA && teamA.Results.ContainsKey(opponent.Team))
                        {
                            differenceA += teamA.Results[opponent.Team];
                        }

                        if (opponent != teamB && teamB.Results.ContainsKey(opponent.Team))
                        {
                            differenceB += teamB.Results[opponent.Team];
                        }
                    }

                    return differenceB.CompareTo(differenceA);
                });

                // Update the rankedTeams list with the sorted tied teams
                int startIndex = rankedTeams.IndexOf(tiedTeams.First());
                for (int j = 0; j < tiedTeams.Count; j++)
                {
                    if (startIndex + j < rankedTeams.Count)
                    {
                        rankedTeams[startIndex + j] = tiedTeams[j];
                    }
                }
            }

            return rankedTeams;
        }



    }
}