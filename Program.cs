using System;
using Npgsql;

namespace statcompare
{
    class Program
    {
        private Team GetStats(string team)
        {
            var teamStats = new Team();
            teamStats.TeamName = team;
            
            using (var conn =
                new NpgsqlConnection("Server=192.168.0.5; User Id =postgres; Password=password; Database =basketball"))
            {
                using(var query = new NpgsqlCommand())
                {
                    var str = "Select * From public.stats Where team_id = '" + team +"'";
                    query.Connection = conn;
                    query.CommandText = str;
                    var param = query.CreateParameter();
                    param.ParameterName = "team";
                    param.Value = team;
                    query.Parameters.Add(param);
                    
                    conn.Open();
                    var reader = query.ExecuteReader();
                    
                    while(reader.Read())
                    {
                        teamStats.Offense =Double.Parse( reader.GetString(1));
                        teamStats.Defense = Double.Parse(reader.GetString(2));
                        teamStats.Margin = Double.Parse(reader.GetString(3));
                        teamStats.Rebound = Double.Parse(reader.GetString(4));
                        teamStats.Assists = Double.Parse(reader.GetString(5));
                        teamStats.Blocks = Double.Parse(reader.GetString(6));
                        teamStats.Steals = Double.Parse(reader.GetString(7));
                        teamStats.Turnovers = Double.Parse(reader.GetString(8));
                        teamStats.AssistsTurnovers = Double.Parse(reader.GetString(9));
                        teamStats.FieldGoalPercent = Double.Parse(reader.GetString(10));
                        teamStats.FieldGoalDefense = Double.Parse(reader.GetString(11));
                        teamStats.ThreesPerGame = Double.Parse(reader.GetString(12));
                        teamStats.ThreesPercent = Double.Parse(reader.GetString(13));
                        teamStats.FreeThrowPercent = Double.Parse(reader.GetString(14));
                        teamStats.WinLossPercent = Double.Parse(reader.GetString(15));
                    }   
                    reader.Close();
                }
                
            }

            return teamStats;
        }

        private bool HigherIsBetter(double teamAStat, double teamBStat)
        {
            return teamAStat > teamBStat;
        }

        private bool LowerIsBetter(double teamAStat, double teamBStat)
        {
            return teamAStat < teamBStat;
        }

        private void RecordResult(string name, double score)
        {
            string result = String.Format("{0} {1}", name, score);
            System.IO.File.WriteAllText("Results.txt", result);
        }
        
        private void PredictWinner(Team teamOne, Team teamTwo)
        {            
            if (HigherIsBetter(teamOne.Offense , teamTwo.Offense))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (LowerIsBetter(teamOne.Defense, teamTwo.Defense))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.Margin, teamTwo.Margin))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.Rebound, teamTwo.Rebound))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.Assists, teamTwo.Assists))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.Blocks, teamTwo.Blocks))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.Steals, teamTwo.Steals))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.Turnovers, teamTwo.Turnovers))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.AssistsTurnovers, teamTwo.AssistsTurnovers))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.FieldGoalPercent, teamTwo.FieldGoalPercent))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (LowerIsBetter(teamOne.FieldGoalDefense, teamTwo.FieldGoalDefense))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.ThreesPerGame, teamTwo.ThreesPerGame))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.ThreesPercent, teamTwo.ThreesPercent))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }
            
            if (HigherIsBetter(teamOne.FreeThrowPercent, teamTwo.FreeThrowPercent))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }

            if (HigherIsBetter(teamOne.WinLossPercent, teamTwo.WinLossPercent))
                teamOne.Score += 6.67;
            else
            {
                teamTwo.Score += 6.67;
            }
            
            if(teamOne.Score > teamTwo.Score)
                RecordResult(teamOne.TeamName, teamOne.Score);
            else
            {
                RecordResult(teamTwo.TeamName, teamTwo.Score);
            }

        }
        
        static int Main(string[] args)
        {
            if (args.Length == 0 || args.Length > 1)
            {
                Console.WriteLine("Error: invalid number of arguments");
                return 1;
            }
            
            var teams = System.IO.File.ReadAllLines(args[0]);

            var program = new Program();
            for (var i = 0; i < teams.Length; i += 2)
            {
                var teamA= program.GetStats(teams[i]);
                var teamB = program.GetStats(teams[i + 1]);
                program.PredictWinner(teamA, teamB);
            }

            return 0;
        }
    }

    public struct Team
    {
        public string TeamName;
        public double Offense;
        public double Defense;
        public double Margin;
        public double Rebound;
        public double Assists;
        public double Blocks;
        public double Steals;
        public double Turnovers;
        public double AssistsTurnovers;
        public double FieldGoalPercent;
        public double FieldGoalDefense;
        public double ThreesPerGame;
        public double ThreesPercent;
        public double FreeThrowPercent;
        public double WinLossPercent;
        public double Score;
    }
}
