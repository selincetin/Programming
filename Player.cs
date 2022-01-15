using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace task_2
{
    public class Player
    {
        public Player(string playerName, int score,string password)
        {

            PlayerName = playerName;
            this.score = score;
            this.password = password;
            Program.players.Add(PlayerName, this);

        }

        public int score = 0;
        public string PlayerName = "";
        public string password = "";
        public void AddScore()
        {
            score++;
        }
        public void TotalScore()
        {
            Console.WriteLine(PlayerName + " adlı kullanıcının toplam skoru:" + score);
        }
        public void SendMessage(string message)
        {
            Console.WriteLine(PlayerName + ":" + message);
        }


    }
}
