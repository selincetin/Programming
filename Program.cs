using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;

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
                new Player(rdr.GetString(rdr.GetOrdinal("name")), rdr.GetInt32(rdr.GetOrdinal("score")), rdr.GetString(rdr.GetOrdinal("password")));
            }
            rdr.Dispose();
            HttpListener(new[] { "https://127.0.0.1:3000/" });
            var t = Task.Run(() => new Komutlistener().KomutListener(con));
            t.Wait();
            
        }

        public static void HttpListener(string[] prefixes)
                 
        {
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("Prefixes needed");

            HttpListener listener = new HttpListener();

            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening..");
            
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string responseString = "<HTML><BODY> Test </BODY></HTML>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            listener.Stop();
        }
        private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(2000000);
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


























