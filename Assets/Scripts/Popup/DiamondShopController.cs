using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class DiamondShopCanvas {
    public Image image;
    public Canvas canvas;
    public CanvasGroup canvasGroup;
    public GameObject[] loadingImages;
    public Button[] purchaseButtons;
    public Image[] purchaseButtonImages;
    public Text[] priceTexts;
}

public class DiamondShopController : PopupController
{
    [SerializeField] Sprite defaultPurchaseButtonImage;
    [SerializeField] Sprite loadingButtonImage;    
    public DiamondShopCanvas diamondShopCanvas;
    LevelLoader levelLoader;
    UIController UIController;
    IAPManager iAPManager;
    HeartShopController heartShopController;
    PopupController popupController;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        UIController = FindObjectOfType<UIController>();
        heartShopController = FindObjectOfType<HeartShopController>();
        // popupController = FindObjectOfType<PopupController>();

        diamondShopSiblingIndex = transform.GetSiblingIndex();
        Debug.Log(diamondShopSiblingIndex + ":diamondShopSiblingIndex");
    }

    public void ToggleDiamondShopCanvas(bool isShow) {
        var body = transform.GetChild(0);

        if (isShow) {
            // diamondShopCanvas.image.raycastTarget = true;
            base.ToggleNoDiamondPopup(false);
            transform.SetSiblingIndex(heartShopSiblingIndex + 1);

            if(levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
            {
                transform.DOMoveY(0, 0.25f);
                return;
            }
            body.transform.DOMoveY(Screen.height/2, 0.25f);
            return;
        }
        
        // diamondShopCanvas.image.raycastTarget = false;

        if(levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) {
            transform.DOMoveY(-4, 0.25f);
            return;
        }
        body.transform.DOMoveY(-Screen.height/2, 0.25f);
        return;
    }

    public void SetCanvasOrder(int order)
    {
        diamondShopCanvas.canvas.sortingOrder = order;
    }

    public void HandleClick(string targetProductId)
    {
        TogglePurchaseButton(true, targetProductId);
        IAPManager.Instance.Purchase(targetProductId);
    }

    public void TogglePurchaseButton(bool isLoading, string targetProductId)
    {
        int index = int.Parse(Regex.Match(targetProductId, @"\d+").Value) - 1;

        if (isLoading) 
        {
            diamondShopCanvas.purchaseButtons[index].interactable = false;
            diamondShopCanvas.priceTexts[index].text = "";
            diamondShopCanvas.loadingImages[index].SetActive(true);
        }
        else 
        {
            diamondShopCanvas.purchaseButtonImages[index].sprite = defaultPurchaseButtonImage;
            diamondShopCanvas.purchaseButtons[index].interactable = true;
            diamondShopCanvas.priceTexts[index].text = IAPManager.Instance.GetPrice(targetProductId);
            diamondShopCanvas.loadingImages[index].SetActive(false);
        }
    }

    public void SaveDiamond(int count)
    {
        PlayerPrefs.SetInt($"Diamond{count}", count);
    }    
}
