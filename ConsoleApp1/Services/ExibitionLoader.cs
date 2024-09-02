using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp1.Domain;

namespace ConsoleApp1.Services
{
    public class ExibitionLoader
    {
        public static void LoadExhibitions(string filePath, Dictionary<string, BasketballTeam> teams)
        {
            var exhibitions = JsonSerializer.Deserialize<Dictionary<string, List<Exhibition>>>(File.ReadAllText(filePath)) ?? new Dictionary<string, List<Exhibition>>();

            foreach (var country in exhibitions)
            {
                foreach (var match in country.Value)
                {
                    var result = match.Result.Split('-');
                    int homeScore = int.Parse(result[0]);
                    int awayScore = int.Parse(result[1]);

                    if (teams.ContainsKey(country.Key))
                    {
                        if (homeScore > awayScore)
                        {
                            teams[country.Key].UpdateForm('W');
                        }
                        else
                        {
                            teams[country.Key].UpdateForm('L');
                        }
                    }

                    if (teams.ContainsKey(match.Opponent))
                    {
                        if (awayScore > homeScore)
                        {
                            teams[match.Opponent].UpdateForm('W');
                        }
                        else
                        {
                            teams[match.Opponent].UpdateForm('L');
                        }
                    }
                }
            }
        }
    }
}