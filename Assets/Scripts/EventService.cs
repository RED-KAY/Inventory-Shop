public class EventService
{
    private static EventService instance;
    public static EventService Instance {
        get {
            if (instance == null)
            {
                instance = new EventService();
            }
            return instance;
        }
    }

    public EventController<string, int> _OnItemBought { get; private set; }

    public EventController<string, int> _OnItemSold { get; private set; }

    public EventController<int> _OnItemSoldAddMoney { get; private set; }

    public EventService()
    {
        _OnItemBought = new EventController<string, int>();
        _OnItemSold = new EventController<string, int>();

        _OnItemSoldAddMoney = new EventController<int>();
    }
}
