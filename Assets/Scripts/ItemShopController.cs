using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class ItemShopCanvas {
    public Image image;
    public CanvasGroup canvasGroup;
    public Canvas canvas;
    public Button panelSetting;
    
}

[Serializable]
public class Header {
    public GameObject diamondBar;
    public GameObject heartBar;
    
}

public class ItemShopController : MonoBehaviour
{
    public ItemShopCanvas itemShopCanvas;
    public Header header;
    [SerializeField] Text title;
    [SerializeField] Text description;
    [SerializeField] Image itemImage1;
    [SerializeField] Image itemImage2;
    [SerializeField] Button itemButton1;
    [SerializeField] Button itemButton2;
    [SerializeField] Sprite goldMineItemIllust;
    [SerializeField] Sprite explosiveWarehouseItemIllust;
    [SerializeField] Button closeButton;
    LevelLoader levelLoader;
    DiamondController diamondController;
    ItemController itemController;
    AfterPurchaseEffectController afterPurchaseEffectController;    
    PopupController popupController;    

    // Start is called before the first frame update
    void Start()
    {
        Initialize();        
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        diamondController = FindObjectOfType<DiamondController>();
        itemController = FindObjectOfType<ItemController>();
        afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
        popupController = FindObjectOfType<PopupController>();
        
        itemShopCanvas.panelSetting.onClick.AddListener(() => ToggleItemShopCanvas(false));
        closeButton.onClick.AddListener(() => ToggleItemShopCanvas(false));
        ToggleItemShopCanvas(false);
    }

    public void ToggleItemShopCanvas(bool isShow, string type = null) {
        var body = this.gameObject.transform.GetChild(0);
        switch(type)
        {
            case ItemController.TYPE.GOLD_MINE:
            {
                title.text = Constants.TEXT.GOLD_MINE_ITEM_TITLE;
                description.text = Constants.TEXT.GOLD_MINE_ITEM_DESC;
                description.lineSpacing = 1.4f;

                itemImage1.sprite = goldMineItemIllust;
                itemImage2.sprite = goldMineItemIllust;

                itemButton1.onClick.RemoveAllListeners();
                itemButton1.onClick.AddListener(() => HandleClick(Constants.GOLD_MINE_1));
                itemButton2.onClick.RemoveAllListeners();
                itemButton2.onClick.AddListener(() => HandleClick(Constants.GOLD_MINE_5));
                break;
            }
            case ItemController.TYPE.EXPLOSIVE_WAREHOUSE:
            {
                title.text = Constants.TEXT.EXPLOSIVE_WAREHOUSE_ITEM_TITLE;
                description.text = Constants.TEXT.EXPLOSIVE_WAREHOUSE_ITEM_DESC;
                description.lineSpacing = 1.1f;

                itemImage1.sprite = explosiveWarehouseItemIllust;
                itemImage2.sprite = explosiveWarehouseItemIllust;

                itemButton1.onClick.RemoveAllListeners();
                itemButton1.onClick.AddListener(() => HandleClick(Constants.EXPLOSIVE_WAREHOUSE_1));
                itemButton2.onClick.RemoveAllListeners();
                itemButton2.onClick.AddListener(() => HandleClick(Constants.EXPLOSIVE_WAREHOUSE_5));                
                break;
            }
        }

        if (isShow) {
            itemShopCanvas.image.raycastTarget = true;
            itemShopCanvas.canvasGroup.DOFade(1, 0.1f);
            itemShopCanvas.canvasGroup.interactable = true;
            itemShopCanvas.canvasGroup.blocksRaycasts = true;
            itemShopCanvas.canvas.sortingOrder = 7;
            header.diamondBar.SetActive(true);
            header.heartBar.SetActive(true);
            return;
        }
        itemShopCanvas.image.raycastTarget = false;
        itemShopCanvas.canvasGroup.DOFade(0, 0.01f);
        itemShopCanvas.canvasGroup.interactable = false;
        itemShopCanvas.canvasGroup.blocksRaycasts = false;
        itemShopCanvas.canvas.sortingOrder = 5;
        header.diamondBar.SetActive(false);
        header.heartBar.SetActive(false);        
        return;
    }

    public void HandleClick(string targetProductId)
    {
        int currentDiamondAmount = diamondController.GetDiamondAmount();
        switch (targetProductId)
        {
            case Constants.GOLD_MINE_1: 
            {
                if(currentDiamondAmount < 10)
                {
                    popupController.ToggleNoDiamindPopup(true);
                    return;
                }
                itemController.AddItemAmount(ItemController.TYPE.GOLD_MINE, 1);
                diamondController.SubtractDiamondAmount(10);
                afterPurchaseEffectController.ShowScreen("5", 1);
                break;
            }
            case Constants.GOLD_MINE_5: 
            {
                if(currentDiamondAmount < 40)
                {
                    popupController.ToggleNoDiamindPopup(true);
                    return;
                }

                itemController.AddItemAmount(ItemController.TYPE.GOLD_MINE, 5);
                diamondController.SubtractDiamondAmount(40);
                afterPurchaseEffectController.ShowScreen("5", 5);
                break;
            }
            case Constants.EXPLOSIVE_WAREHOUSE_1: 
            {
                if(currentDiamondAmount < 10)
                {
                    popupController.ToggleNoDiamindPopup(true);
                    return;
                }

                itemController.AddItemAmount(ItemController.TYPE.EXPLOSIVE_WAREHOUSE, 1);
                diamondController.SubtractDiamondAmount(10);
                afterPurchaseEffectController.ShowScreen("6", 1);
                break;
            }
            case Constants.EXPLOSIVE_WAREHOUSE_5: 
            {
                if(currentDiamondAmount < 40)
                {
                    popupController.ToggleNoDiamindPopup(true);
                    return;
                }

                itemController.AddItemAmount(ItemController.TYPE.EXPLOSIVE_WAREHOUSE, 5);
                diamondController.SubtractDiamondAmount(40);
                afterPurchaseEffectController.ShowScreen("6", 5);
                break;
            }
        }
    }    
}
