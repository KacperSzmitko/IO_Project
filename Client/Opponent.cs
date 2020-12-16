namespace Client.Models
{
    public class Opponent
    {
        private readonly string username;
        private readonly int elo;

        public string Username { get { return username; } }
        public int Elo { get { return elo; } }

        public Opponent(string username, int elo) {
            this.username = username;
            this.elo = elo;
        }
    }
}