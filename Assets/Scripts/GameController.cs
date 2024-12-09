using UnityEngine;

public class GameController : GenericMonoSingleton<GameController>
{
    [SerializeField] private ShopView _ShopView;
    private ShopController _ShopController;

    public Sprite[] _Rarities;

    private void Start()
    {
        ShopModel model = new ShopModel();
        _ShopController = new ShopController(model, _ShopView);
    }


}
