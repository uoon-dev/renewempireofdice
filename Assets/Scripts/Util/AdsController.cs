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
    private static bool isIntitialized;
    private string adsAppleId = "3259036";
    private string adsAndroidId = "3259037";
    private static string rewardType = "";
    private int targetLevel = 0;
    NewHeartController newHeartController;
    UIController UIController;
    NoDiceNoCoinController noDiceNoCoinController;
    ResetDiceController resetDiceController;
    MapController mapController;
    LevelLoader levelLoader;
    [SerializeField] GameObject rewardEffect;
    [SerializeField] GameObject toast;
    [SerializeField] GameObject rewardEffectBackground;
    [SerializeField] Transform toastTransform;

    
    void Start()
    {
        Initialize();
        if(isIntitialized) return; 
        isIntitialized = true;
        InitializeAds();        
    }

    public void Initialize()
    {
        newHeartController = FindObjectOfType<NewHeartController>();
        UIController = FindObjectOfType<UIController>();
        noDiceNoCoinController = FindObjectOfType<NoDiceNoCoinController>();
        resetDiceController = FindObjectOfType<ResetDiceController>();
        mapController = FindObjectOfType<MapController>();
        levelLoader = FindObjectOfType<LevelLoader>();
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
        });
    }

    public void PlayAds(string reward)
    {
        if (Yodo1U3dAds.VideoIsReady()) {   
            rewardType = reward;
            if (rewardType == AD_REWARD_TYPE.GET_REWARD_ITEM)
            {
                rewardEffectBackground.SetActive(true);
            }
            Yodo1U3dAds.ShowVideo();
        } else {
            if (toast != null)
            {
                VideoIsNotReady();
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
                rewardEffect.SetActive(true);
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

    public void VideoIsNotReady()
    {
        Sequence sequence = DOTween.Sequence();
        if (!toast.activeSelf)
        {
            toast.SetActive(true);
            sequence.Append(toastTransform.DOLocalMoveY(0, 0.2f));
            sequence.AppendInterval(3f);
            sequence.AppendCallback(() => {
                toast.SetActive(false);
                sequence.Append(toastTransform.DOLocalMoveY(-20, 0));
            });
            sequence.Play();
        }
    }
}
    

