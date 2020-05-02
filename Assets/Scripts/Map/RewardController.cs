using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class RewardStatus {

    [Serializable]
    public class UIObject {
        public GameObject rewardBefore;
        public GameObject rewardAfter;
        public GameObject rewardEffect;
    }

    [Serializable]
    public class UITransform {
        public Transform box;

    }
}

public class RewardController : MonoBehaviour
{
    public RewardStatus.UIObject uiObject;
    public RewardStatus.UITransform uiTransform;
    public static Timer timer;
    private const int timerInterval = 500;
    private static int rewardTargetTimeStamp;
    private bool isNetworkConnected;
    private bool IsDeviceTimeValid = false;
    private static int isPossibleReward = 0;
    public static UIController UIController;
    public Sequence boxSequence;


    void Awake()
    {
        Initialize();    
    }

    void Start()
    {
        if (isPossibleReward == 0)
        {
            StartTimer();
        }
        UpdateRewardBox();
        AnimateRewardBox();
    }

    private void Initialize()
    {
        rewardTargetTimeStamp = PlayerPrefs.GetInt("RewardTargetTimeStamp");
        isPossibleReward = PlayerPrefs.GetInt("IsPossibleReward");
        boxSequence = DOTween.Sequence();

        InitializeTimer();
    }

    private void Update()
    {
        bool newIsNetworkConnected = Utils.IsNetworkConnected();

        if (isNetworkConnected && !newIsNetworkConnected) 
        {
            StopTimer();
        }
        else
        {
            if (isPossibleReward == 0) 
            {
                StartTimer();
            }
        }

        UpdateRewardBox();        
        
        isNetworkConnected = newIsNetworkConnected;
    }


    private void OnDestroy()
    {
        SaveRewardTargetTimeStamp(rewardTargetTimeStamp);
        SaveIsPossibleReward();
    }

    private void OnApplicationQuit()
    {
        SaveRewardTargetTimeStamp(rewardTargetTimeStamp);
        SaveIsPossibleReward();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            StopTimer();
            SaveRewardTargetTimeStamp(rewardTargetTimeStamp);
            SaveIsPossibleReward();
        }
        else
        {
            if (isPossibleReward == 0)
            {
                StartTimer();
            }
        }
    }    

    public void OnClickReward()
    {        
        AdsController.instance.PlayAds(AD_REWARD_TYPE.GET_REWARD_ITEM);
        // FindObjectOfType<UIController>().ShowToast("광고를 불러오지 못했어요. 잠시 후에 다시 시도해주세요");
        // uiObject.rewardEffect.SetActive(true);
    }

    public void StartTimer()
    {
        timer.Start();
    }

    private void StopTimer()
    {
        if (timer.Enabled) 
        {
            timer.Stop();
        }
    }

    private void InitializeTimer()
    {
        if (timer == null) {
            timer = new System.Timers.Timer(timerInterval);
            timer.Elapsed += Tick;
        }
    }

    private void SaveIsPossibleReward() {
        PlayerPrefs.SetInt("IsPossibleReward", isPossibleReward);
    }

    private void SaveRewardTargetTimeStamp(int timestamp) {
        PlayerPrefs.SetInt("RewardTargetTimeStamp", timestamp);
    }

    private void Tick(object sender, ElapsedEventArgs e)
    {
        int currentTimeStamp = Utils.GetTimeStamp();

        if (currentTimeStamp >= rewardTargetTimeStamp)
        {
            if (isPossibleReward == 0)
            {
                isPossibleReward = 1;

                StopTimer();
            }
        }
    }
    public int GetRewardTargetTimeStamp()
    {
        return rewardTargetTimeStamp;
    }

    public void SetRewardTargetTimeStamp()
    {
        isPossibleReward = 0;
        int currentTimeStamp = Utils.GetTimeStamp();
        rewardTargetTimeStamp = Constants.REWARD_CHARGE_SECONDS + currentTimeStamp;        
    }

    public void UpdateRewardBox()
    {
        if (isPossibleReward == 0)
        {
            uiObject.rewardBefore.SetActive(true);
            uiObject.rewardAfter.SetActive(false);

            if (boxSequence.IsPlaying())
            {
                boxSequence.Pause();
            }
        }
        else
        {
            uiObject.rewardBefore.SetActive(false);
            uiObject.rewardAfter.SetActive(true);
            
            if (!boxSequence.IsPlaying())
            {
                boxSequence.Restart();
            }
        }        
    }

    public void AnimateRewardBox()
    {
        boxSequence.Append(uiTransform.box.DORotate(new Vector3(0, 0, -3), 0.08f));
        boxSequence.Append(uiTransform.box.DORotate(new Vector3(0, 0, 3), 0.08f));
        boxSequence.Append(uiTransform.box.DORotate(new Vector3(0, 0, -3), 0.08f));
        boxSequence.Append(uiTransform.box.DORotate(new Vector3(0, 0, 3), 0.08f));
        boxSequence.AppendInterval(1.2f);
        boxSequence.SetLoops(-1, LoopType.Restart);
        boxSequence.Play();
    }
}
