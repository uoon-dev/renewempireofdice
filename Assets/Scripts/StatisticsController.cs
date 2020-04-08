using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class LevelCompleteCanvas {
    [Serializable]
    public class UIAnimator {
        public Animator star01Img;
        public Animator star01Txt;
        public Animator star02Img;
        public Animator star02Txt;
        public Animator star03Img;
        public Animator star03Txt;
        public Animator mapBtn;
        public Animator nextBtn;
    }

    [Serializable]
    public class UIImage {
        public Image star01;
        public Image star02;
        public Image star03;
    }

    [Serializable]
    public class UIText {
        public Text star01;
        public Text star02;
        public Text star03;
    }

    [Serializable]
    public class UIButton {
        public Button mapBtn;
        public Button nextBtn;
    }
}


public class StatisticsController : MonoBehaviour
{
    public ProductController[] controllers;
    public LevelCompleteCanvas levelCompleteCanvas;
    public LevelCompleteCanvas.UIAnimator uiAnimator;
    public LevelCompleteCanvas.UIButton uiButton;
    public LevelCompleteCanvas.UIImage uiImage;
    public LevelCompleteCanvas.UIText uiText;
    [SerializeField] Text factor01 = null;
    [SerializeField] Text factor02 = null;
    [SerializeField] Text factor03 = null;
    [SerializeField] GameObject star01 = null;
    [SerializeField] GameObject star02 = null;
    [SerializeField] GameObject star03 = null;
    [SerializeField] GameObject buttons = null;
    float clearedBlockCount = 0;
    float ddackCount = 0;
    float turnCount = 0;
    int currentLevelNumber = 0;
    int savedLevelStarCount = 0;
    int getStarCount = 0;
    int levelCleared = 0;
    LevelLoader levelLoader;
    NewHeartController newHeartController;
    DiamondController diamondController;
    ItemController itemController;
    AfterPurchaseEffectController afterPurchaseEffectController;
    ResetDiceController resetDiceController;
    ProductController productController;


    void Start()
    {
        Initialize();
        factor01.text = "0%";
        factor02.text = "0";
        factor03.text = "0";

        // star display image
        uiAnimator.star01Img.enabled = false;
        uiAnimator.star02Img.enabled = false;
        uiAnimator.star03Img.enabled = false;

        // star display text
        uiAnimator.star01Txt.enabled = false;
        uiAnimator.star02Txt.enabled = false;
        uiAnimator.star03Txt.enabled = false;

        // button display
        uiAnimator.mapBtn.enabled = false;
        uiAnimator.nextBtn.enabled = false;

        // for test
        // FindObjectOfType<LevelController>().WinLastBlock();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        newHeartController = FindObjectOfType<NewHeartController>();
        diamondController = FindObjectOfType<DiamondController>();
        itemController = FindObjectOfType<ItemController>();
        afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
        resetDiceController = FindObjectOfType<ResetDiceController>();
        productController = FindObjectOfType<ProductController>();

        currentLevelNumber = levelLoader.GetCurrentLevelNumber();
        savedLevelStarCount = PlayerPrefs.GetInt($"LevelStar {currentLevelNumber}");
        levelCleared = PlayerPrefs.GetInt($"Level {currentLevelNumber}");
    }

    IEnumerator HandleRewardUI()
    {
        bool inTurnLimit = resetDiceController.GetTurnCount() <= 30;

        SetStarCount();

        if (currentLevelNumber > 1)
        {
            uiAnimator.star01Img.enabled = true;
            uiAnimator.star01Txt.enabled = true;

            if (clearedBlockCount == ddackCount)
            {
                yield return new WaitForSeconds(0.7f);
                uiAnimator.star02Img.enabled = true;
                uiAnimator.star02Txt.enabled = true;
                uiText.star02.text = "딱뎀 100%";

                if (inTurnLimit)
                {
                    yield return new WaitForSeconds(0.7f);
                    uiAnimator.star03Img.enabled = true;
                    uiAnimator.star03Txt.enabled = true;

                    if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.LEVEL)
                    {
                        bool isHeartFullReward = false;
                        if (currentLevelNumber % 10 == 0 && levelCleared != 1)
                        {
                            isHeartFullReward = true;
                            yield return new WaitForSeconds(0.7f);
                            afterPurchaseEffectController.ShowScreen("3", 0);
                            SetRewardAtSpecialStage();
                        }
                        if (savedLevelStarCount < 3)
                        {
                            SetReward();
                            yield return new WaitForSeconds(isHeartFullReward ? 2f : 0.7f);
                            afterPurchaseEffectController.ShowScreen("2", 0);
                        }
                    }
                }
            } 
            else 
            {
                yield return new WaitForSeconds(0.7f);
                if (inTurnLimit) 
                {
                    uiAnimator.star02Img.enabled = true;
                    uiAnimator.star02Txt.enabled = true;
                    uiText.star02.text = "30턴 안에 클리어!";
                }
            }

            if (levelCleared != 1 && getStarCount < 3)
            {
                if (currentLevelNumber % 10 == 0)
                {
                    yield return new WaitForSeconds(0.7f);
                    afterPurchaseEffectController.ShowScreen("2", 0);
                }
            }
        }

        PlayerPrefs.SetInt($"Level {currentLevelNumber}", 1);

        if (savedLevelStarCount < getStarCount)
        {
            // 튜토리얼은 별 무조건 3개 주기
            if (currentLevelNumber == 1)
            {
                PlayerPrefs.SetInt($"LevelStar {currentLevelNumber}", 3);
            }
            else
            {
                PlayerPrefs.SetInt($"LevelStar {currentLevelNumber}", getStarCount);
            }
        }

        yield return new WaitForSeconds(getStarCount > 1 ? 1.5f : 0.7f);
        uiAnimator.mapBtn.enabled = true;
        uiButton.mapBtn.interactable = true;
        
        uiAnimator.nextBtn.enabled = true;
        uiButton.nextBtn.interactable = true;
    }

    public void SetStarCount()
    {
        bool inTurnLimit = resetDiceController.GetTurnCount() <= 30;
        getStarCount = 1;

        if (clearedBlockCount == ddackCount) 
        { 
            getStarCount = 2;
            if (inTurnLimit)
            {
                getStarCount = 3;
            }
        } else 
        {
            if (inTurnLimit)
            {
                getStarCount = 2;
            }
        }
    }

    public void SetRewardAtSpecialStage()
    {
        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.LEVEL) {
            if (currentLevelNumber % 10 == 0 && newHeartController.GetHeartAmount() < Constants.HEART_MAX_CHARGE_COUNT && levelCleared != 1) {
                float randomValue = UnityEngine.Random.value;

                if (randomValue <= 0.1f) {
                    controllers[2].GetReward(1);
                }
                else if (randomValue <= 0.55) {
                    controllers[1].GetReward(1);
                }
                else {
                    controllers[0].GetReward(1);
                }
            }
        }
    }

    public void SetReward()
    {
        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.LEVEL) {
            if (getStarCount == 3 && savedLevelStarCount < 3)
            {
                float randomValue = UnityEngine.Random.value;

                if (randomValue <= 0.1f) {
                    controllers[2].GetReward(1);
                }
                else if (randomValue <= 0.55) {
                    controllers[1].GetReward(1);
                }
                else {
                    controllers[0].GetReward(1);
                }
            }
        }
    }

    public void UpdateFactor01()
    {
        clearedBlockCount++;
        ddackCount++;
        factor01.text = Mathf.Floor((ddackCount/clearedBlockCount)*100).ToString() + "%";
        factor02.text = clearedBlockCount.ToString();
    }

    public void UpdateFactor02()
    {
        clearedBlockCount++;
        factor01.text = Mathf.Floor((ddackCount / clearedBlockCount) * 100).ToString() + "%";
        factor02.text = clearedBlockCount.ToString();
    }

    public void UpdateFactor03()
    {
        turnCount++;
        factor03.text = (turnCount + 1).ToString();
    }

    public void UpdateStarsStatisticDisplay()
    {
        StartCoroutine(HandleRewardUI());
    }
}
