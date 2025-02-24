﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;



public class IAPManager : MonoBehaviour, IStoreListener
{
    [SerializeField] GameObject MaldivesButton = null;
    [SerializeField] GameObject GoldrushButton = null;
    [SerializeField] Text iapErrorReason = null;

    private const string IOSMaldivesDiceId = "maldivesdice";
    private const string AndroidMaldivesDiceId = "maldivesdice";

    private const string IOSGoldrushDiceId = "goldrushdice";
    private const string AndroidGoldrushDiceId = "goldrushdice";
    private const string IOSSmallHeartId = "smallheart";
    private const string AndroidSmallHeartId = "smallheart";
    private const string IOSLargeHeartId = "largeheart";
    private const string AndroidLargeHeartId = "largeheart";
    private const string IOSHeartRechargeSpeedUpId = "speedupheartrecharge1";
    private const string AndroidHeartRechargeSpeedUpId = "speedupheartrecharge1";

    private static IAPManager mInstance;

    public static IAPManager Instance
    {
        get
        {
            if (mInstance != null) return mInstance;

            mInstance = FindObjectOfType<IAPManager>();

            if (mInstance == null) mInstance = new GameObject("IAP Manager").AddComponent<IAPManager>();
            return mInstance;
        }
    }

    private IStoreController storeController; // 구매 과정을 제어하는 함수를 제공
    private IExtensionProvider storeExtensionProvider; // 여러 플랫폼을 위한 확장 처리를 제공

    public bool IsInitialized => storeController != null && storeExtensionProvider != null;
    NewHeartController newHeartController;
    DiamondController diamondController;
    AfterPurchaseEffectController afterPurchaseEffectController;
    HeartShopController heartShopController;
    UIController UIController;

    void Awake()
    {
        Initialize();
        if (mInstance != null && mInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        InitUnityIAP();
    }

    private void Initialize()
    {
        newHeartController = FindObjectOfType<NewHeartController>();
        diamondController = FindObjectOfType<DiamondController>();
        afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
        heartShopController = FindObjectOfType<HeartShopController>();
        UIController = FindObjectOfType<UIController>();
    }

    void InitUnityIAP()
    {
        if (IsInitialized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // 다이아몬드
        builder.AddProduct(
            Constants.DIAMOND_1, ProductType.Consumable,
            new IDs()
            {
                { Constants.DIAMOND_1, AppleAppStore.Name },
                { Constants.DIAMOND_1, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.DIAMOND_2, ProductType.Consumable,
            new IDs()
            {
                { Constants.DIAMOND_2, AppleAppStore.Name },
                { Constants.DIAMOND_2, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.DIAMOND_3, ProductType.Consumable,
            new IDs()
            {
                { Constants.DIAMOND_3, AppleAppStore.Name },
                { Constants.DIAMOND_3, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.DIAMOND_4, ProductType.Consumable,
            new IDs()
            {
                { Constants.DIAMOND_4, AppleAppStore.Name },
                { Constants.DIAMOND_4, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.DIAMOND_5, ProductType.Consumable,
            new IDs()
            {
                { Constants.DIAMOND_5, AppleAppStore.Name },
                { Constants.DIAMOND_5, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.DIAMOND_6, ProductType.Consumable,
            new IDs()
            {
                { Constants.DIAMOND_6, AppleAppStore.Name },
                { Constants.DIAMOND_6, GooglePlay.Name },
            }
            );            

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) 
    {
        Debug.Log("유니티 IAP 초기화 성공");
        storeController = controller;
        storeExtensionProvider = extensions;

        bool isNetworkConnected = Utils.IsNetworkConnected();

        if (isNetworkConnected)
        {
            // UIController.ActivePurchaseButtonInHeartShop();
            UIController.TogglePurchaseButtonInDiamondShop(false);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"유니티 IAP 초기화 실패 { error }");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"구매 성공 - ID : {args.purchasedProduct.definition.id}");
        var heartShopController = FindObjectOfType<HeartShopController>();
        var diamondShopController = FindObjectOfType<DiamondShopController>();

        switch(args.purchasedProduct.definition.id)
        {
            case Constants.MaldivesDice:
                {
                    Debug.Log("몰디브 주사위 획득...");

                    MaldivesButton.GetComponent<ButtonController>().HidePurchaseButtonGroup();
                    MaldivesButton.GetComponent<ButtonController>().ShowUseButtonGroup();
                    PlayerPrefs.SetInt($"purchased-{Constants.MaldivesDice}", 1);
                    break;
                }
            case Constants.GoldrushDice:
                {
                    Debug.Log("골드러시 주사위 획득...");

                    GoldrushButton.GetComponent<ButtonController>().HidePurchaseButtonGroup();
                    GoldrushButton.GetComponent<ButtonController>().ShowUseButtonGroup();
                    PlayerPrefs.SetInt($"purchased-{Constants.GoldrushDice}", 1);
                    break;
                }
            // case Constants.SmallHeart:
            //     {
            //         Debug.Log("하트 구매...");

            //         heartShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
            //         newHeartController.AddHeartAmount(15);
            //         afterPurchaseEffectController.ShowScreen("0", 15);
            //         break;
            //     }
            // case Constants.LargeHeart:
            //     {
            //         Debug.Log("하트 많이 구매...");

            //         heartShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
            //         newHeartController.AddHeartAmount(75);
            //         afterPurchaseEffectController.ShowScreen("0", 75);
            //         break;
            //     }
            // case Constants.HeartRechargeSpeedUp:
            //     {
            //         Debug.Log("하트 충전 속도 업...");
            //         heartShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
            //         newHeartController.UpgradeHeartRechargeSpeed(2);
            //         afterPurchaseEffectController.ShowScreen("1", 0);
            //         heartShopController.SetSpeedUpText();
            //         break;
            //     }
            case Constants.DIAMOND_1: {
                diamondShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
                diamondController.AddDiamondAmount(20);
                afterPurchaseEffectController.ShowScreen("4", 20);
                break;
            }
            case Constants.DIAMOND_2: {
                diamondShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
                diamondController.AddDiamondAmount(50);
                afterPurchaseEffectController.ShowScreen("4", 50);
                break;
            }
            case Constants.DIAMOND_3: {
                diamondShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
                diamondController.AddDiamondAmount(120);
                afterPurchaseEffectController.ShowScreen("4", 120);
                break;
            }
            case Constants.DIAMOND_4: {
                diamondShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
                diamondController.AddDiamondAmount(400);
                afterPurchaseEffectController.ShowScreen("4", 400);
                break;
            }
            case Constants.DIAMOND_5: {
                diamondShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
                diamondController.AddDiamondAmount(800);
                afterPurchaseEffectController.ShowScreen("4", 800);
                break;
            }
            case Constants.DIAMOND_6: {
                diamondShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);
                diamondController.AddDiamondAmount(1300);
                afterPurchaseEffectController.ShowScreen("4", 1300);
                break;
            }
            default: {
                break;
            };
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        // var heartShopController = FindObjectOfType<HeartShopController>();
        // heartShopController.TogglePurchaseButton(false, product.definition.id);
        // GameObject.Find("IAP Error Reason").GetComponent<Text>().text = reason.ToString();
        UIController.TogglePurchaseButtonInDiamondShop(false);
        // iapErrorReason.text = reason.ToString();
        Debug.LogWarning($"구매 실패 - {product.definition.id}, {reason}");
    }

    public void Purchase(string productId)
    {
        if (!IsInitialized) return;

        var product = storeController.products.WithID(productId);

        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"구매 시도 - {product.definition.id}");
            if (product.definition.id == Constants.HeartRechargeSpeedUp && RestorePurchase())
            {
                newHeartController.UpgradeHeartRechargeSpeed(2);
                afterPurchaseEffectController.ShowScreen("1", 0);
            }
            else {
                storeController.InitiatePurchase(product);
            }
        }
        else
        {
            Debug.Log($"구매 시도 불가 - {productId}");
        }
    }

    public bool RestorePurchase()
    {
        if (!IsInitialized) return false;

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            bool isRestored = false;
            var appleExt = storeExtensionProvider.GetExtension<IAppleExtensions>();
            appleExt.RestoreTransactions(
                result => { isRestored = true; }
                );
            return isRestored;
        }
        return false;
    }

    public Boolean HadPurchased(string productId)
    {
        if (!IsInitialized) return false;

        var product = storeController.products.WithID(productId);

        if (product != null)
        {
            return product.hasReceipt;
        }

        return false;
    }


    public string GetPrice(string productId)
    {
        if (!IsInitialized) return "";
        string price = storeController.products.WithID(productId).metadata.localizedPriceString;
        return price;
    }

    public bool isIAPInitialized()
    {
        return IsInitialized;
    }
    
}
