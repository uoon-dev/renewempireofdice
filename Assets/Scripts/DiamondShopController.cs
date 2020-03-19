using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DiamondShopController : MonoBehaviour
{
    [SerializeField] Sprite defaultPurchaseButtonImage;
    [SerializeField] Sprite loadingButtonImage;    
    LevelLoader levelLoader;
    UIController UIController;
    IAPManager iAPManager;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();   
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        UIController = FindObjectOfType<UIController>();
        iAPManager = FindObjectOfType<IAPManager>();
    }

    public void ToggleDiamondShopCanvas(bool isShow) {
        var body = this.gameObject.transform.GetChild(0);

        if (isShow) {
            this.gameObject.GetComponent<Image>().raycastTarget = true;
 
            UIController.ToggleNoHeartCanvas(false);

            if(levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) {
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
        if(levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) {
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
        TogglePurchaseButton(true, targetProductId);
        IAPManager.Instance.Purchase(targetProductId);
    }

    public void TogglePurchaseButton(bool isLoading, string targetProductId)
    {

        Transform purchaseButton = 
            GameObject.Find(targetProductId)
                .transform.Find(Constants.GAME_OBJECT_NAME.SHOP.PRICE);
        Transform priceText = 
            purchaseButton.transform.Find(Constants.GAME_OBJECT_NAME.SHOP.TEXT);

        if (isLoading) 
        {
            // purchaseButton.GetComponent<Image>().sprite = loadingButtonImage;
            priceText.GetComponent<Text>().text = "";
            purchaseButton.GetComponent<Button>().interactable = false;
        }
        else 
        {
            purchaseButton.GetComponent<Image>().sprite = defaultPurchaseButtonImage;
            purchaseButton.GetComponent<Button>().interactable = true;
            priceText.GetComponent<Text>().text = IAPManager.Instance.GetPrice(targetProductId);
        }
    }

    public void SaveDiamond(int count)
    {
        PlayerPrefs.SetInt($"Diamond{count}", count);
    }    
}
