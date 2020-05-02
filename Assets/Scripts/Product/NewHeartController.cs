using System;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class NewHeartController : ProductController
{
    public static Timer timer;
    private const int timerInterval = 500;
    private int heartRechargeSpeed = 1;
    private static int heartAmount;
    public static bool isHeartAmountUpdated = false;
    private static int heartTargetTimeStamp;
    private bool isNetworkConnected;
    private bool IsDeviceTimeValid = false;
    public static UIController UIController;
    public static LevelLoader levelLoader;
    public static HeartShopController heartShopController;

    private void Awake()
    {
        // if (Instance == null)
        // {
        //     Instance = this;
            Initialize();
            
        // }
        // else if (Instance != this) {
        //     Destroy(gameObject);
        // }
        // DontDestroyOnLoad(gameObject);        
    }

    private void Initialize()
    {
        if (PlayerPrefs.HasKey("HeartAmount")) 
        {
            heartAmount = PlayerPrefs.GetInt("HeartAmount");
        }
        else 
        {
            heartAmount = Constants.HEART_MAX_CHARGE_COUNT;
        }

        if (PlayerPrefs.HasKey("HeartRechargeSpeed") || IAPManager.Instance.HadPurchased(Constants.HeartRechargeSpeedUp))
        {
            heartRechargeSpeed = 2;
        } 
        else 
        {
            heartRechargeSpeed = 1;
        }

        heartTargetTimeStamp = PlayerPrefs.GetInt("HeartTargetTimeStamp");
        UIController = FindObjectOfType<UIController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        heartShopController = FindObjectOfType<HeartShopController>();

        InitializeTimer();

        int heartCharteRemainSecond = GetHeartTargetTimeStamp() - Utils.GetTimeStamp();
    }

    private void InitializeHeartBar() {
        UIController.HandleHeartBarUI();
    }

    private void InitializeTimer() {
        if (timer == null) {
            timer = new System.Timers.Timer(timerInterval);
            timer.AutoReset = false;
            timer.Elapsed += Tick;
        }
    }

    public void StartTimer() {
        timer.Start();
    }

    private void StopTimer() {
        if (timer.Enabled) 
        {
            timer.Stop();
        }
    }

    private async void Tick(object sender, ElapsedEventArgs e) {
        int currentTimeStamp = Utils.GetTimeStamp();
        if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
        {
            bool IsDeviceTimeValid = true;
            if (currentTimeStamp >= heartTargetTimeStamp)
            {
                IsDeviceTimeValid = await Utils.IsDeviceTimeValid();
                if (IsDeviceTimeValid)
                {
                    int targetDeltaCount = (currentTimeStamp - heartTargetTimeStamp) / (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed);
                    heartAmount += targetDeltaCount + 1;
                    if (heartAmount > Constants.HEART_MAX_CHARGE_COUNT)
                    {
                        heartAmount = Constants.HEART_MAX_CHARGE_COUNT;
                    }
                    heartTargetTimeStamp = heartTargetTimeStamp + (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed) * (targetDeltaCount + 1) - 1;
                    isHeartAmountUpdated = true;
                }
            }
            if (IsDeviceTimeValid)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }
        }
        else
        {
            StopTimer();
        }
    }

    private void Start()
    {
        if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT) {
            StartTimer();
        }
    }

    private void Update()
    {
        bool newIsNetworkConnected = Utils.IsNetworkConnected();

        if (isNetworkConnected && !newIsNetworkConnected) {
            StopTimer();
        }

        if (!isNetworkConnected && newIsNetworkConnected) {
            if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
            {
                StartTimer();
            }
        }
        
        isNetworkConnected = newIsNetworkConnected;
    }


    private void OnDestroy()
    {
        SaveHeartAmount(heartAmount);
        SaveHeartTargetTimeStamp(heartTargetTimeStamp);
    }

    private void OnApplicationQuit()
    {
        SaveHeartAmount(heartAmount);
        SaveHeartTargetTimeStamp(heartTargetTimeStamp);
    }

    // private void OnApplicationFocus(bool focus)
    // {
    //     if (focus)
    //     {
    //         if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
    //         {
    //             StartTimer();
    //         }
    //     }
    //     else {
    //         StopTimer();
    //         SaveHeartAmount(heartAmount);
    //         SaveHeartTargetTimeStamp(heartTargetTimeStamp);
    //     }
    // }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            StopTimer();
            SaveHeartAmount(heartAmount);
            SaveHeartTargetTimeStamp(heartTargetTimeStamp);
        }
        else
        {
            if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
            {
                StartTimer();
            }
        }
    }

    private void SaveHeartTargetTimeStamp(int timestamp) {
        PlayerPrefs.SetInt("HeartTargetTimeStamp", timestamp);
    }

    public bool CanUseHeart()
    {
        if (GetHeartAmount() <= 0) {
            UIController.ToggleNoHeartCanvas(true);
            return false;
        }
        SubtractHeartAmount(1);
        return true;
    }    


    public void OnClickUseHeart()
    {
        if (heartAmount <= 0)
        {
            UIController.ToggleNoHeartCanvas(true);
        } else {
            SubtractHeartAmount(1);
        }
    }    

    private void SaveHeartAmount(int targetHeartAmount) {
        PlayerPrefs.SetInt("HeartAmount", targetHeartAmount);
        PlayerPrefs.Save();
    }

    public void AddHeartAmount(int addedCount) {
        heartAmount += addedCount;
        SaveHeartAmount(heartAmount);
    }

    public void SubtractHeartAmount(int subtractedCount) {
        heartAmount -= subtractedCount;
        if (!timer.Enabled && heartAmount < Constants.HEART_MAX_CHARGE_COUNT) {
            heartTargetTimeStamp = Utils.GetTimeStamp() + (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed);
            StartTimer();
        }
    }

    public int GetHeartAmount()
    {
        return heartAmount;
    }

    public int GetHeartTargetTimeStamp()
    {
        return heartTargetTimeStamp;
    }

    public void UpgradeHeartRechargeSpeed(int speed)
    {
        heartRechargeSpeed = speed;
        PlayerPrefs.SetInt("HeartRechargeSpeed", speed);

        int heartCharteRemainSecond = GetHeartTargetTimeStamp() - Utils.GetTimeStamp();
        if (heartCharteRemainSecond > Constants.HEART_CHARGE_SECONDS / speed)
        {
            StopTimer();
            if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT) 
            {
                int currentTimeStamp = Utils.GetTimeStamp();
                int targetDeltaCount = (currentTimeStamp - heartTargetTimeStamp) / (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed);
                heartTargetTimeStamp = currentTimeStamp + (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed);
                StartTimer();
            }
        }
    }

    public override void GetReward(int targetAmount)
    {
        rewardType = Constants.REWARD_TYPE.HEART;
        AddHeartAmount(targetAmount);
        Debug.Log(rewardType + ":In child.rewardType");
    } 
}