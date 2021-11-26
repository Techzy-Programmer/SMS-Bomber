namespace SMS_Bomber
{
    public interface ISender
    {
        string Provider { get; }
        int Failed { get; set; }
        int Attacked { get; set; }
        int TotalRequest { get; set; }
        System.Threading.Tasks.Task<bool> Attack(long To, bool Retry = false);
    }
}
