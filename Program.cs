using System;
using System.Diagnostics;

namespace SMS_Bomber
{
    class Program
    {
        static void Main(string[] BombArgs)
        {
            string UsMob, UsLimit;

            if (BombArgs.Length != 4)
            {
                Utility.Write("Enter a 10 digit valid Indian mobile number >"); UsMob = Console.ReadLine();
                Utility.Write("Enter limit of max SMS to bomb >"); UsLimit = Console.ReadLine();
                if (Verified(UsMob, UsLimit)) { goto END; }
            }
            else if (BombArgs[0] == "-m" && BombArgs[2] == "-l")
            {
                UsMob = BombArgs[1];
                UsLimit = BombArgs[3];
                if (Verified(UsMob, UsLimit)) { goto END; }
            }
            else Utility.Write("[ERROR] Invalid parameters supplied.\r\n", "R");

            Utility.Write("[HELP] =>=>=>=>", "G");
            Utility.Write("Utility : Bomber.exe -m [MOBILE NUMBER] -l [LIMIT]", "C");
            Utility.Write("Parameter : -m => Mobile number to perform SMS bombing.", "C");
            Utility.Write("Parameter : -l => Maximum number of messages to attack with.", "C");
            Utility.Write("----------------");

        END:;
            Utility.Write("\r\nPlease hit [Enter] key to exit!", "A");
            Console.ReadLine();
        }

        private static bool Verified(string UMob, string ULimit)
        {
            Console.Clear();
            if (UMob.Length == 10 && long.TryParse(UMob, out long Mobile) && (UMob.StartsWith("6")
                || UMob.StartsWith("7") || UMob.StartsWith("8") || UMob.StartsWith("9")))
            {
                if (int.TryParse(ULimit, out int Limit) &&
                    Limit > 0 && Limit <= 1000) return Start(Mobile, Limit);
                else Utility.Write("[ERROR] The value of 2nd parameter (Limit) " +
                    "should be a natural number not more than a max value of 1000\r\n", "R");
            }
            else Utility.Write("[ERROR] The value of 1st parameter (Mobile Number) shoud be a valid 10 digits indian number.\r\n", "R");
            return false;
        }

        private static bool Start(long Mob, int Tries)
        {
            Utility.Initialize(); Utility.Write("Starting Attack....", "Y");
            var SWTrack = Stopwatch.StartNew(); Utility.BombItUP(Mob, Tries).Wait(); SWTrack.Stop();
            Utility.Write("Bombing finished in => " + SWTrack.ElapsedMilliseconds + " ms", "B");
            return true;
        }
    }
}
