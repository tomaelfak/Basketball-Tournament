using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public class BasketballTeam
    {
        public required string Team { get; set; }

        public required string ISOCode { get; set; }
        public required int FIBARanking { get; set; }

        public int Points { get; set; } = 0;

        public int Wins { get; set; } = 0;

        public int Losses { get; set; } = 0;

        public int ScoredPoints { get; set; } = 0;

        public int ConcededPoints { get; set; } = 0;

        public int Form { get; set; } = 0;

        private Queue<char> formQueue = new Queue<char>();

        public void UpdateForm(char result)
        {
            if(formQueue.Count == 5)
            {
                formQueue.Dequeue();
            }
            formQueue.Enqueue(result);
            Form = formQueue.Count(c => c == 'W') - formQueue.Count(c => c == 'L');
        }

        

        public int PointsDifference () => ScoredPoints - ConcededPoints;

        public Dictionary<string, int> Results { get; set; } = new Dictionary<string, int>();

        public void AddResult(string opponent, int points)
        {
            if(Results.ContainsKey(opponent))
            {
                Results[opponent] += points;
            }
            else
            {
                Results.Add(opponent, points);
            }
        }



    }
}