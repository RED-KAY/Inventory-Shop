public class Item 
{
    private ItemEntry _Details;
    public ItemEntry Details { get { return _Details; } }
    public int _Amount;

    public Item(ItemEntry details, int amount = 1)
    {
        _Details = details; 
        _Amount = amount;
    }
}
