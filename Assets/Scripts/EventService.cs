public class EventService
{
    private static EventService m_Instance;
    public static EventService Instance {
        get {
            if (m_Instance == null)
            {
                m_Instance = new EventService();
            }
            return m_Instance;
        }
    }

    public EventController<string, int> m_OnItemBought { get; private set; }

    public EventController<string, int> m_OnItemSold { get; private set; }

    public EventController<int> m_OnItemSoldAddMoney { get; private set; }

    public EventController m_OnItemsAddedToInventory { get; private set; }
    public EventController m_OnItemsRemovedToInventory { get; private set; }

    public EventControllerFunc<ItemsAddInfo[], ItemsAddInfoResult[]> m_TryAddItems { get; private set; }

    public EventService()
    {
        m_OnItemBought = new EventController<string, int>();
        m_OnItemSold = new EventController<string, int>();

        m_OnItemSoldAddMoney = new EventController<int>();

        m_OnItemsAddedToInventory = new EventController();
        m_OnItemsRemovedToInventory = new EventController();

        m_TryAddItems = new EventControllerFunc<ItemsAddInfo[], ItemsAddInfoResult[]>();
    }
}
