namespace TinyGameServer.Models
{
    public class Player
    {

        public Player(string playerId)
        {
            Id = playerId;
        }
        
        public Player(string playerId, decimal gold)
        {
            Id = playerId;
            Gold = gold;
            Inventory = new List<ShopItem>();
        }

        public string Id { get; set; } 
        public decimal Gold { get; set; }
        public List<ShopItem> Inventory { get; set; } = new List<ShopItem>();
    }

}
