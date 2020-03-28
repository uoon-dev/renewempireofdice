using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] Sprite heartSpriteFull;
    [SerializeField] Sprite heartSpriteNormal;
    [SerializeField] Sprite heartSpriteEmpty;
    [SerializeField] Text goldMineAmountText;
    [SerializeField] Text explosiveWarehouseAmountText;    
    GameObject noHeartCanvas;
    GameObject afterPurchaseEffectCanvas;
    HeartShopController heartShopController;
    DiamondShopController diamondShopController;
    StartController startController;
    NewHeartController newHeartController;
    DiamondController diamondController;
    LevelLoader levelLoader;
    IAPManager iAPManager;
    ItemController itemController;
    Image heartImage;
    Text heartTimerText;
    Text heartTimerTextInNoHeartCanvas;
    Text heartTimerTextInShop;
    Text heartShopTimer;
    Text heartAmountText;
    Text timerTitle;
    Text titleInHeartShop;
    Text iapInitTest;
    private int prevHeartAmount = -1;
    private bool isNetworkConnected;
    private bool isIAPInitialized;
    private bool isSetSpeedUp = false;



    void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        bool newIsNetworkConnected = Utils.IsNetworkConnected();
        isIAPInitialized = iAPManager.isIAPInitialized();
        
        // 네트워크 핸들링
        if (isNetworkConnected && !newIsNetworkConnected) 
        {
            // DeactivePurchaseButtonInHeartShop();
            TogglePurchaseButtonInDiamondShop(true);
        }

        if (!isNetworkConnected && newIsNetworkConnected && isIAPInitialized) 
        {
            // ActivePurchaseButtonInHeartShop();
            TogglePurchaseButtonInDiamondShop(false);
        }

        // 하트 충전속도 핸들링
        if (isIAPInitialized && !isSetSpeedUp 
            && IAPManager.Instance.HadPurchased(Constants.HeartRechargeSpeedUp))
        {
            newHeartController.UpgradeHeartRechargeSpeed(2);
            heartShopController.SetSpeedUpText();
            isSetSpeedUp = true;
        }

        isNetworkConnected = newIsNetworkConnected;

        UpdateTimerText();
        
        // 하트바 + 다이아몬드바 핸들링
        // if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
        // {
            HandleHeartBarUI();
            HandleDiamondBar();
        // }

        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.LEVEL) 
        {
            HandleItemBar();
        }

        int heartAmount = newHeartController.GetHeartAmount();
        if (heartAmount > 0)
        {
            ToggleNoHeartCanvas(false);
        }
    }

    private void Initialize()
    {
        bool newIsNetworkConnected = Utils.IsNetworkConnected();

        heartShopController = FindObjectOfType<HeartShopController>();
        diamondShopController = FindObjectOfType<DiamondShopController>();
        startController = FindObjectOfType<StartController>();
        newHeartController = FindObjectOfType<NewHeartController>();
        diamondController = FindObjectOfType<DiamondController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        iAPManager = FindObjectOfType<IAPManager>();
        itemController = FindObjectOfType<ItemController>();
        
        afterPurchaseEffectCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.AFTER_PURCHASE_EFFECT_CANVAS);
        // if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
        // {
        heartTimerText = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT).GetComponent<Text>();
        heartAmountText = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_STATUS).GetComponent<Text>();
        heartImage = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_IMAGE).GetComponent<Image>();
        // }
        heartTimerTextInNoHeartCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT_IN_NO_HEART_CANVAS).GetComponent<Text>();
        heartTimerTextInShop = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT_IN_SHOP).GetComponent<Text>();
        timerTitle = GameObject.Find(Constants.GAME_OBJECT_NAME.TIMER_TITLE_IN_SHOP).GetComponent<Text>();
        titleInHeartShop = GameObject.Find(Constants.GAME_OBJECT_NAME.TITLE_IN_SHOP).GetComponent<Text>();

        if (newIsNetworkConnected && isIAPInitialized)
        {
            TogglePurchaseButtonInDiamondShop(false);
        }
        else 
        {
            TogglePurchaseButtonInDiamondShop(true);
        }
    }


    public void UpdateTimerText()
    {
        int heartAmount = newHeartController.GetHeartAmount();
        int heartCharteRemainSecond = heartAmount < Constants.HEART_MAX_CHARGE_COUNT ? 
            newHeartController.GetHeartTargetTimeStamp() - Utils.GetTimeStamp() : 0;

        if (Utils.IsNetworkConnected())
        {
            string remainTime = string.Format("{0:0}:{1:00}", heartCharteRemainSecond / 60, heartCharteRemainSecond % 60);

            // if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
            // {
                heartTimerText.text = (heartAmount < Constants.HEART_MAX_CHARGE_COUNT && heartCharteRemainSecond > 0) ? remainTime : "full";
                // heartTimerText.color = new Color32(0, 0, 0, 255);
            // }
            // else
            // {
                heartTimerTextInNoHeartCanvas.fontSize = 14;
                heartTimerTextInShop.fontSize = 14;
            // }

            if (heartCharteRemainSecond > 0)
            {
                heartTimerTextInNoHeartCanvas.text = remainTime;
                heartTimerTextInShop.text = remainTime;
                timerTitle.text = "다음하트";

                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
                {
                    titleInHeartShop.transform.localPosition = new Vector2(titleInHeartShop.transform.localPosition.x, 27);
                }
                else
                {
                    titleInHeartShop.transform.localPosition = new Vector2(titleInHeartShop.transform.localPosition.x, 30);
                }
            }
            else
            {
                heartTimerTextInNoHeartCanvas.text = "";
                heartTimerTextInShop.text = "";
                timerTitle.text = "";
                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
                {
                    titleInHeartShop.transform.localPosition = new Vector2(titleInHeartShop.transform.localPosition.x, 24);
                }
                else
                {
                    titleInHeartShop.transform.localPosition = new Vector2(titleInHeartShop.transform.localPosition.x, 27);
                }
            }
        }
        else
        {
            if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
            {                
                heartTimerText.fontSize = 24;
                // heartTimerText.color = new Color32(193, 193, 193, 255);
            }
            else
            {
                heartTimerTextInNoHeartCanvas.fontSize = 12;
                heartTimerTextInShop.fontSize = 12;
            }
            heartTimerText.text = "오프라인";
            heartTimerTextInNoHeartCanvas.text = "오프라인";
            heartTimerTextInShop.text = "오프라인";
        }
    }

    public void TogglePurchaseButtonInDiamondShop(bool isActive)
    {
        for (int i = 0; i < 6; i++)
        {
            string targetDiamond = Constants.SAVED_DATA.DIAMOND + (i + 1);
            diamondShopController.TogglePurchaseButton(isActive, targetDiamond);
        }
    }

    public void HandleHeartBarUI() 
    {
        int heartAmount = newHeartController.GetHeartAmount();
        int copiedHeartAmount = heartAmount;

        // 하트 개수가 달라질 때만 heart bar 업데이트 하기
        if (prevHeartAmount == heartAmount) return;

        Debug.Log("heart updated");
        prevHeartAmount = heartAmount;

        heartAmountText.text = heartAmount.ToString();
        if (heartAmount < 1)
        {
            heartImage.sprite = heartSpriteEmpty;
        }
        else if (heartAmount < 5)
        {
            heartImage.sprite = heartSpriteNormal;
        }
        else
        {
            heartImage.sprite = heartSpriteFull;
        }
    }

    public void ToggleNoHeartCanvas(bool isShow) {
        noHeartCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.NO_HEART_CANVAS);
        startController = FindObjectOfType<StartController>();
        var body = noHeartCanvas.transform.GetChild(0);

        if (isShow) {
            if (heartShopController != null)
                heartShopController.ToggleHeartShopCanvas(false);

            if (startController != null)
                startController.HideScreen();

            if(levelLoader.GetCurrentSceneName() == "Map System") {
                noHeartCanvas.transform.DOMoveY(0, 0.25f);
                return;
            }
            body.transform.DOMoveY(Screen.height/2 - 20, 0.25f);
            return;
        }

        if(levelLoader.GetCurrentSceneName() == "Map System") {
            noHeartCanvas.transform.DOMoveY(-3, 0.25f);
            return;
        }

        body.transform.DOMoveY(-Screen.height/2, 0.25f);
        return;
    }

    public void HandleDiamondBar()
    {
        Text diamondAmount = GameObject.Find(Constants.GAME_OBJECT_NAME.DIAMOND_AMOUNT).GetComponent<Text>();
        diamondAmount.text = diamondController.GetDiamondAmount().ToString();
    }

    public void HandleItemBar()
    {
        goldMineAmountText.text = itemController.GetItemAmount(ItemController.TYPE.GOLD_MINE).ToString();
        explosiveWarehouseAmountText.text = itemController.GetItemAmount(ItemController.TYPE.EXPLOSIVE_WAREHOUSE).ToString();
    }
}
