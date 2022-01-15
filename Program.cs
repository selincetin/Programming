using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;




namespace task_2
{
    public class Program
    {
        public static Dictionary<string, Player> players = new Dictionary<string, Player> { };
        private static System.Timers.Timer aTimer;

        static void Main()
        {
            SetTimer();


            var cs = "Host=localhost;Username=postgres;Password=mysecretpassword;Database=players";
            using var con = new NpgsqlConnection(cs);

            con.Open();

            string sql = "SELECT * FROM users";
            using var cmd = new NpgsqlCommand(sql, con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                new Player(rdr.GetString(rdr.GetOrdinal("name")),rdr.GetInt32(rdr.GetOrdinal("score")),rdr.GetString(rdr.GetOrdinal("password")));
            }
            rdr.Dispose();            
            var t = Task.Run(() => new Komutlistener().KomutListener(con));
            t.Wait();
                     }

        private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(20000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            foreach (var item in players)
            {
                Console.WriteLine(item.Value.PlayerName, e.SignalTime);
            }

        }
    }
}




















