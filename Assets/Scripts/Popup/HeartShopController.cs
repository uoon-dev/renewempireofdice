using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class HeartShopCanvas {
    public Image image;
    public Canvas canvas;
    public CanvasGroup canvasGroup;
}

public class HeartShopController : PopupController
{
    [SerializeField] GameObject rechargeItemPrice;
    [SerializeField] GameObject rechargeItemPurchaseButton;
    [SerializeField] Sprite defaultPurchaseButtonImage;
    [SerializeField] Sprite loadingButtonImage;
    public HeartShopCanvas heartShopCanvas;

    LevelLoader levelLoader;
    UIController UIController;
    IAPManager iAPManager;
    NewHeartController newHeartController;
    DiamondController diamondController;
    DiamondShopController diamondShopController;
    AfterPurchaseEffectController afterPurchaseEffectController;
    PopupController popupController;

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
        popupController = FindObjectOfType<PopupController>();

        heartShopSiblingIndex = transform.GetSiblingIndex();
        Debug.Log(heartShopSiblingIndex + ":heartShopSiblingIndex");
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
                    rechargeItemPriceText.fontSize = 9;
                    rechargeItemPriceText.transform.DOLocalMoveX(120.5f ,0);
                    rechargeItemPriceText.transform.DOLocalMoveY(2.3f ,0);
                }
                
                heartRechargeSpeedImage.color = new Color32(255, 255, 255, 100);
                diamondImage.color = new Color32(255, 255, 255, 100);
                purchaseButton.interactable = false;
            }
        }   
    }

    public void ToggleHeartShopCanvas(bool isShow) {
        var body = transform.GetChild(0);

        if (isShow) {
            // heartShopCanvas.image.raycastTarget = true;
            UIController.ToggleNoHeartCanvas(false);
            transform.SetSiblingIndex(diamondShopSiblingIndex);

            if(levelLoader.GetCurrentSceneName() == "Map System") {
                transform.DOMoveY(0, 0.25f);
                return;
            }
            body.transform.DOMoveY(Screen.height/2, 0.25f);
            return;
        }
        // heartShopCanvas.image.raycastTarget = false;
        if(levelLoader.GetCurrentSceneName() == "Map System") {
            transform.DOMoveY(-4, 0.25f);
            return;
        }
        body.transform.DOMoveY(-Screen.height/2, 0.25f);
        return;
    }

    public void SetCanvasOrder(int order)
    {
        heartShopCanvas.canvas.sortingOrder = order;
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
                    base.ToggleNoDiamondPopup(true);
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
                    base.ToggleNoDiamondPopup(true);
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
                    base.ToggleNoDiamondPopup(true);
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
