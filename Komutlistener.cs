using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace task_2
{
    class Komutlistener
    {
        public NpgsqlConnection Con;
        public void KomutListener(NpgsqlConnection con)
        {
            Con = con;
            Console.WriteLine("Eğer mevcut komutları bilmiyorsanız '/help' yazarak komutlara ulaşabilirsiniz.");
            Console.WriteLine("kullanıcı adı ve şifrenizi girin");
            string komutVariable = Console.ReadLine();
            string password= Console.ReadLine();

            if (Program.players.ContainsKey(komutVariable) && password==Program.players[komutVariable].password)
            {
                while (true)
                {
                    Console.WriteLine("Komutu gir.");
                    string veri = Console.ReadLine();

                    if (veri.StartsWith("/"))
                    {
                        string komut = veri.Substring(1);
                        komutVariable = "";
                        password = "";
                        string[] splitVeri = komut.Split(new char[] { ' ' }, 2);

                        if (splitVeri.Length > 1)
                        {
                            komut = splitVeri[0];
                            komutVariable = splitVeri[1];
                        }
                        KomutHandle(komut, komutVariable,password);
                    }
                }
            }
            else
            {
                Console.WriteLine("Kullanıcı adı veya şifreyi yanlış girdiniz.");
                Console.ReadLine();
            }

        }
        public void KomutHandle(string komut, string komutVariable, string password)
        {

            switch (komut)
            {
                case "add":

                    if (Program.players.ContainsKey(komutVariable))
                    {
                        Program.players[komutVariable].AddScore();
                        using var cmd = new NpgsqlCommand("UPDATE users SET score=@score WHERE name=@player_name;", Con);
                        cmd.Parameters.AddWithValue("@player_name", komutVariable);
                        cmd.Parameters.AddWithValue("@score", Program.players[komutVariable].score);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine(komutVariable + " adlı kullanıcının skoru arttırıldı.");
                        cmd.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("böyle bir kullanıcı yok");
                    }
                    break;

                case "scores":

                    {
                        foreach (var score in Program.players)
                        {
                            score.Value.TotalScore();
                        }
                    }

                    break;

                case "create":


                    if (Program.players.ContainsKey(komutVariable))
                    {
                        Console.WriteLine("böyle bir kullanıcı zaten mevcut");

                    }
                    else
                    {
                        Console.WriteLine("Lütfen şifre belirleyiniz.");
                        password = Console.ReadLine();
                        using var cmd = new NpgsqlCommand("INSERT INTO users(name,score,password) VALUES(@player_name,0,@password)", Con);
                        cmd.Parameters.AddWithValue("player_name", komutVariable);
                        cmd.Parameters.AddWithValue("password",password);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine(komutVariable + " adlı kullanıcı oluşturuldu.");
                        cmd.Dispose();
                        new Player(komutVariable, 0, password);
                    }

                    break;

                case "message":

                    string[] splitVariable = komutVariable.Split(new char[] { ' ' }, 2);


                    if (Program.players.ContainsKey(splitVariable[0]))
                    {

                        Program.players[splitVariable[0]].SendMessage(splitVariable[1]);

                    }
                    else
                    {
                        Console.WriteLine("böyle bir kullanıcı yok");
                    }

                    break;

                case "help":

                    string text = "1-/add playername = Girdiğiniz oyuncunun skorunu 1 arttırır.,2-/create playername = Yeni oyuncu oluşturur.,3-/message playername girilen" +
                        "mesaj = Oyuncunun attığı mesajı gösterir.,4-/scores = Tüm oyuncuların mevcut skorlarını gösterir.";
                    string[] divide = text.Split(',');
                    Console.WriteLine(divide[0]);
                    Console.WriteLine(divide[1]);
                    Console.WriteLine(divide[2]);
                    Console.WriteLine(divide[3]);
                    break;

                case "delete":

                    if (Program.players.ContainsKey(komutVariable))
                    {
                        using var cmd = new NpgsqlCommand("DELETE FROM users WHERE name=@player_name", Con);
                        cmd.Parameters.AddWithValue("player_name", komutVariable);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine(komutVariable + " adlı kullanıcı silindi");
                        cmd.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("böyle bir kullanıcı bulunamadı...");
                    }
                    break;

            }

        }
    }
}
