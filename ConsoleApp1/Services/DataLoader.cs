using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp1.Domain;

namespace ConsoleApp1.Services
{
    public static class DataLoader
    {
        public static List<Group> LoadGroups()
        {
            var groups = JsonSerializer.Deserialize<Dictionary<string, List<BasketballTeam>>>(File.ReadAllText("../Data/groups.json")) ?? new Dictionary<string, List<BasketballTeam>>();

            List<Group> groupList = new();
            foreach (var group in groups)
            {
                groupList.Add(new Group(group.Key, group.Value));
            }

            return groupList;
        }
    }
}