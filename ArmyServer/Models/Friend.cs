namespace ArmyServer.Models
{
    public class Friend
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Friend(string id, string name)
        {
            Id = id;
            Name = name;
        }

        // You can include other methods, properties, or functionality as needed here.
    }
}