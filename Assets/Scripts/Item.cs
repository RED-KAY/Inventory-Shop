public class Item 
{
    private ItemEntry m_Details;
    public ItemEntry Details { get { return m_Details; } }
    public int m_Amount;

    public Item(ItemEntry m_Details, int m_Amount = 1)
    {
        this.m_Details = m_Details; 
        this.m_Amount = m_Amount;
    }
}
