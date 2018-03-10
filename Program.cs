using System;
using Npgsql;

namespace statcompare
{
    class Program
    {
        private Team GetStats(string team)
        {
            var teamStats = new Team();
            teamStats.teamName = team;
            
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
                        Console.WriteLine(reader.GetString(1));
                        Console.WriteLine(reader.GetString(2));
                        teamStats.offense = Int32.Parse(reader.GetString(0));
                    }   
                    reader.Close();
                }
                
            }

            return teamStats;
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
                program.GetStats(teams[i]);

            return 0;
        }
    }

    public class Team
    {
        public string teamName;
        public int offense;
        public int defense;
        public int margin;
        public int rebound;
        public int assists;
        public int blocks;
        public int shots;
        public int steals;
        public int turnovers;
        public int assistsTurnovers;
        public int fieldGoalPercent;
        public int fieldGoalDefense;
        public int threesPerGame;
        public int threesPercent;
        public int freeThrowPercent;
        public int winLossPercent;
    }
}
