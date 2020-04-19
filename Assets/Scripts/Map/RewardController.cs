using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public static Timer timer;
    private const int timerInterval = 500;
    private static int rewardTargetTimeStamp;
    private bool isNetworkConnected;
    private bool IsDeviceTimeValid = false;
    private int isPossibleReward = 0;
    public static UIController UIController;
    public static AdsController adsController;

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
        Debug.Log(rewardTargetTimeStamp);
        Debug.Log(Utils.GetTimeStamp());
    }

    private void Initialize()
    {
        rewardTargetTimeStamp = PlayerPrefs.GetInt("RewardTargetTimeStamp");
        isPossibleReward = PlayerPrefs.GetInt("IsPossibleReward");
        adsController = FindObjectOfType<AdsController>();

        InitializeTimer();
    }

    private void Update()
    {
        bool newIsNetworkConnected = Utils.IsNetworkConnected();

        if (isNetworkConnected && !newIsNetworkConnected) 
        {
            StopTimer();
        }

        if (isPossibleReward == 0) 
        {
            StartTimer();
        }
        
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
        isPossibleReward = 0;
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
                int targetDeltaCount = (currentTimeStamp - rewardTargetTimeStamp) / Constants.REWARD_CHARGE_SECONDS;
                isPossibleReward = 1;
                rewardTargetTimeStamp = rewardTargetTimeStamp + (Constants.REWARD_CHARGE_SECONDS) * (targetDeltaCount + 1) - 1;
                
                Debug.Log(isPossibleReward + ":isPossibleReward");
            }
        }
    }

    public int GetRewardTargetTimeStamp()
    {
        return rewardTargetTimeStamp;
    }    
}
