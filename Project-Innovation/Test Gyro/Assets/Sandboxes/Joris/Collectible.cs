public class Collectible
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public bool IsCollected { get; set; }

    public Collectible(int id, string name)
    {
        ID = id;
        Name = name;
        IsCollected = false;
    }
}