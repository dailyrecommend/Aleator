namespace SystemLayer
{
    public static class GameStartParams
    {
        public static string diceId = "default_die";
        public static void SetDice(string id) { diceId = id; }
    }
}