using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartShopController : MonoBehaviour
{
    [SerializeField] GameObject rechargeItemPrice;
    [SerializeField] GameObject rechargeItemPurchaseButton;
    [SerializeField] Sprite defaultPurchaseButtonImage;
    [SerializeField] Sprite loadingButtonImage;

    LevelLoader levelLoader;
    UIController UIController;
    IAPManager iAPManager;
    NewHeartController newHeartController;
    DiamondController diamondController;
    DiamondShopController diamondShopController;
    AfterPurchaseEffectController afterPurchaseEffectController;

    void Awake()
    {
        Initialize();
        SetSpeedUpText();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        UIController = FindObjectOfType<UIController>();
        iAPManager = FindObjectOfType<IAPManager>();
        newHeartController = FindObjectOfType<NewHeartController>();
        diamondController = FindObjectOfType<DiamondController>();
        diamondShopController = FindObjectOfType<DiamondShopController>();
        afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
    }    

    public void SetSpeedUpText()
    {
        Text rechargeItemPriceText = rechargeItemPrice.GetComponent<Text>();
        Image heartRechargeSpeedImage = rechargeItemPurchaseButton.GetComponent<Image>();
        Button purchaseButton = rechargeItemPurchaseButton.GetComponent<Button>();
        Image diamondImage = 
            GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_RECHARGE_SPEED_UP_ITEM)
            .transform.Find(Constants.GAME_OBJECT_NAME.SHOP.DIAMOND_IMAGE).GetComponent<Image>();

        if (rechargeItemPriceText != null) {
            int heartRechargeSpeed = PlayerPrefs.GetInt("HeartRechargeSpeed");
            if (heartRechargeSpeed == 2 || IAPManager.Instance.HadPurchased(Constants.HeartRechargeSpeedUp))
            {
                rechargeItemPriceText.text = "구매함";
                rechargeItemPriceText.color = new Color32(0, 0, 0, 100);
                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
                {
                    rechargeItemPriceText.fontSize = 18;
                    rechargeItemPriceText.transform.DOLocalMoveX(120.5f ,0);
                }
                else
                {
                    rechargeItemPriceText.fontSize = 10;
                }
                
                heartRechargeSpeedImage.color = new Color32(255, 255, 255, 100);
                diamondImage.color = new Color32(255, 255, 255, 100);
                purchaseButton.interactable = false;
            }
        }   
    }

    public void ToggleHeartShopCanvas(bool isShow) {
        var body = this.gameObject.transform.GetChild(0);

        if (isShow) {
            this.gameObject.GetComponent<Image>().raycastTarget = true;

            diamondShopController.ToggleDiamondShopCanvas(false);
            UIController.ToggleNoHeartCanvas(false);

            if(levelLoader.GetCurrentSceneName() == "Map System") {
                this.gameObject.transform.DOMoveY(0, 0.25f);
                this.gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
                this.gameObject.GetComponent<CanvasGroup>().interactable = true;
                this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;                
                return;
            }
            body.transform.DOMoveY(Screen.height/2, 0.25f);
            return;
        }
        this.gameObject.GetComponent<Image>().raycastTarget = false;
        if(levelLoader.GetCurrentSceneName() == "Map System") {
            this.gameObject.transform.DOMoveY(-4, 0.25f);
            this.gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.01f);
            this.gameObject.GetComponent<CanvasGroup>().interactable = false;
            this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            return;
        }
        body.transform.DOMoveY(-Screen.height/2, 0.25f);
        return;
    }

    public void HandleClick(string targetProductId)
    {
        int currentDiamondAmount = diamondController.GetDiamondAmount();
        switch (targetProductId)
        {
            case Constants.SmallHeart: 
            {
                if(currentDiamondAmount < 15)
                {
                    return;
                }
                
                diamondController.SubtractDiamondAmount(15);
                newHeartController.AddHeartAmount(5);
                afterPurchaseEffectController.ShowScreen("0", 5);
                break;
            }
            case Constants.LargeHeart: 
            {
                if(currentDiamondAmount < 120)
                {
                    return;
                }

                diamondController.SubtractDiamondAmount(120);
                newHeartController.AddHeartAmount(50);
                afterPurchaseEffectController.ShowScreen("0", 50);
                break;
            }
            case Constants.HeartRechargeSpeedUp: 
            {
                if(currentDiamondAmount < 20)
                {
                    return;
                }

                diamondController.SubtractDiamondAmount(20);
                newHeartController.UpgradeHeartRechargeSpeed(2);
                afterPurchaseEffectController.ShowScreen("1", 0);
                SetSpeedUpText();
                break;
            }
        }
    }
}
