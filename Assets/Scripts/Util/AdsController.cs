using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using DG.Tweening;

public static class AD_REWARD_TYPE
{
    public const string GET_ALL_DICES = "getAllDices";
    public const string LOAD_CLICKED_MAP = "LoadClickedMap";
    public const string LOAD_LEVEL_SCENE = "LoadLevelScene";
    public const string GET_REWARD_ITEM = "getRewardItem";
}

public class AdsController : MonoBehaviour
{
    public static AdsController instance = null;
    private static bool isIntitialized;
    private string adsAppleId = "3259036";
    private string adsAndroidId = "3259037";
    private static string rewardType = "";
    private int targetLevel = 0;
    NewHeartController newHeartController;
    UIController uiController;
    NoDiceNoCoinController noDiceNoCoinController;
    ResetDiceController resetDiceController;
    MapController mapController;
    LevelLoader levelLoader;
    

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);        
    }

    void Start()
    {
        Initialize();
        if(isIntitialized) return; 
        isIntitialized = true;
        InitializeAds();        
    }

    public void Initialize()
    {
        if (newHeartController == null) newHeartController = FindObjectOfType<NewHeartController>();
        if (uiController == null) uiController = FindObjectOfType<UIController>();
        if (noDiceNoCoinController == null) noDiceNoCoinController = FindObjectOfType<NoDiceNoCoinController>();
        if (resetDiceController == null) resetDiceController = FindObjectOfType<ResetDiceController>();
        if (mapController == null) mapController = FindObjectOfType<MapController>();
        if (levelLoader == null) levelLoader = FindObjectOfType<LevelLoader>();
    }

    public void InitializeAds()
    {
        Yodo1U3dAds.InitializeSdk();
        SetListners();
    }

   
    public void SetListners()
    {
        SetRewaredAdsListner();
        SetInterstitialAdsListner();
    }

    public void SetInterstitialAdsListner() {
        Yodo1U3dSDK.setInterstitialAdDelegate((Yodo1U3dConstants.AdEvent adEvent, string error) => {
            Debug.Log("InterstitialAdDelegate:" + adEvent + "\n" + error);
            switch (adEvent)
            {
                case Yodo1U3dConstants.AdEvent.AdEventClick:
                    AnalyticsEvent.AdStart(false);
                    Debug.Log("Interstital ad has been clicked.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventClose:
                    //LevelLoader.LoadClickedMap(targetLevel);
                    Debug.Log("Interstital ad has been closed.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowSuccess:
                    //LevelLoader.LoadClickedMap(targetLevel);
                    AnalyticsEvent.AdComplete(false);
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowFail:
                    Debug.Log("Interstital ad has been show failed, the error message:" + error);
                    break;
            }
        });
    }

    public void SetRewaredAdsListner()
    {
        Yodo1U3dSDK.setRewardVideoDelegate((Yodo1U3dConstants.AdEvent adEvent, string error) =>
        {
            Debug.Log("RewardVideoDelegate:" + adEvent + "\n" + error);
            switch (adEvent)
            {
                case Yodo1U3dConstants.AdEvent.AdEventClick:
                    AnalyticsEvent.AdStart(true);
                    Debug.Log("Rewarded video ad has been clicked.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventClose:
                    Debug.Log("Rewarded video ad has been closed.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowSuccess:
                    AnalyticsEvent.AdComplete(true);
                    OnRewaredVideoSuccess();
                    Debug.Log("Rewarded video ad has shown successful.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowFail:
                    Debug.Log("Rewarded video ad show failed, the error message:" + error);
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventFinish:
                    Debug.Log("Rewarded video ad has been played finish, give rewards to the player.");
                    break;
            }

            // if (rewardType == AD_REWARD_TYPE.GET_REWARD_ITEM)
            // {
            //     rewardEffectBackground.SetActive(false);
            // }            
        });
    }

    public void PlayAds(string reward)
    {
        if (Yodo1U3dAds.VideoIsReady()) 
        {   
            rewardType = reward;
            // if (rewardType == AD_REWARD_TYPE.GET_REWARD_ITEM)
            // {
            //     rewardEffectBackground.SetActive(true);
            // }
            Yodo1U3dAds.ShowVideo();
        } else {
            if (uiController == null) 
            {
                uiController = FindObjectOfType<UIController>();
            } 
            else
            {
                uiController.ShowToastVideoIsNotReady();
            }
            
        }
    }

    public void PlayInterstitialAdsWithLevel(string reward, int level) {
        if (Yodo1U3dAds.InterstitialIsReady())
        {
            targetLevel = level;
            Yodo1U3dAds.ShowInterstitial();
        }
    }

    public void PlayInterstitialAds(string reward) {
        if (Yodo1U3dAds.InterstitialIsReady()) {
            Yodo1U3dAds.ShowInterstitial();
        }
    }
    private void OnRewaredVideoSuccess()
    {
        Initialize();
        switch(rewardType) {
            case AD_REWARD_TYPE.GET_REWARD_ITEM: {
                uiController.ShowRewardEffect();
                break;
            }
            case AD_REWARD_TYPE.GET_ALL_DICES: {
                noDiceNoCoinController.HideScreen();
                resetDiceController.AbleResetDiceButton();
                resetDiceController.ResetDices();
                break;
            }
            case AD_REWARD_TYPE.LOAD_CLICKED_MAP: {
                newHeartController.AddHeartAmount(1);
                mapController.OnClickMap();
                break;
            }
            case AD_REWARD_TYPE.LOAD_LEVEL_SCENE: {
                newHeartController.AddHeartAmount(1);
                if (levelLoader.GetIsGoingToNextLevel()) {
                    levelLoader.LoadNextLevel();
                    levelLoader.SetIsGoingToNextLevel(false);
                    return;
                } 
                levelLoader.LoadCurrentScene();
                break;
            }
        }
    }
}
    

