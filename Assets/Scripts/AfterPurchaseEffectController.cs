using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class RewardSprite {
    public Sprite spHeart; 
    public Sprite spDiamond; 
    public Sprite spGoldMine;
    public Sprite spDynamite; 
}

[Serializable]
public class AfterPurchaseCanvas {
    public CanvasGroup canvasGroup;
    public Transform transform;

    [Serializable]
    public class UIImage {
        public Image heartReward;
        public Image diamondReward;
        public Image goldMineReward;
        public Image dynamiteReward;
    }

    [Serializable]
    public class UIText {
        public Text heartRewardAmount;
        public Text diamondRewardAmount;
        public Text goldMineRewardAmount;
        public Text dynamiteRewardAmount;
    }
}

public class AfterPurchaseEffectController : MonoBehaviour
{   
    public RewardSprite rewardSprite;
    public AfterPurchaseCanvas afterPurchaseEffectCanvas;
    public AfterPurchaseCanvas.UIImage uiImage;
    public AfterPurchaseCanvas.UIText uiText;
    // [SerializeField] GameObject afterPurchaseEffectCanvas = null;
    [SerializeField] GameObject heartBar = null;
    ProductController productController;
    DiamondController diamondController;
    LevelLoader levelLoader;
    Text effectText;
    Transform body;
    GameObject effectImage;

    void Start()
    {
        Initialize();
        afterPurchaseEffectCanvas.canvasGroup.DOFade(0, 0);
    }

    private void Initialize()
    {
        body = afterPurchaseEffectCanvas.transform.GetChild(0);
        diamondController = FindObjectOfType<DiamondController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        productController = FindObjectOfType<ProductController>();

        effectText = body.GetComponentInChildren<Text>();
        effectImage = body.GetChild(4).gameObject;
    }

    public void ShowScreen(string type, int targetAmount)
    {
        // HideHeartShopController();

        DOTween.Kill(afterPurchaseEffectCanvas.canvasGroup);
        for (int i = 3; i < body.childCount; i++)
        {
            Transform childObject = body.GetChild(i);

            if (childObject && childObject.gameObject.active)
            {
                childObject.gameObject.SetActive(false);
            }
        }
        
        if (type == "0") {
            effectText.text = "하트가 충전됐어요!";
            effectText.transform.DOLocalMoveY(41f, 0);
            uiImage.heartReward.sprite = rewardSprite.spHeart;
            uiImage.heartReward.gameObject.SetActive(true);
            uiText.heartRewardAmount.text = $"+{targetAmount}";
        } else if (type == "1") {
            effectText.text = "충전 속도가 빨라집니다";
            effectImage.SetActive(true);
        } else if (type == "2") {
            effectText.text = "별 3개 랜덤 보상!";
            effectText.transform.DOLocalMoveY(45f, 0);
            SetRewardSprite();
            uiImage.heartReward.gameObject.SetActive(true);
            uiText.heartRewardAmount.text = "+1";
        } else if (type == "3") {
            effectText.text = "특별 스테이지 랜덤 보상!!";
            effectText.transform.DOLocalMoveY(45f, 0);
            SetRewardSprite();
            uiImage.heartReward.gameObject.SetActive(true);
            uiText.heartRewardAmount.text = "+1";
        } else if (type == "4") {
            effectText.text = "다이아가 충전됐어요!";
            effectText.transform.DOLocalMoveY(41f, 0);
            uiImage.diamondReward.gameObject.SetActive(true);
            uiText.diamondRewardAmount.text = $"+{targetAmount}";
        } else if (type == "5") {
            effectText.text = "황금광산이 충전됐어요!";
            effectText.transform.DOLocalMoveY(45f, 0);
            uiImage.goldMineReward.gameObject.SetActive(true);
            uiText.goldMineRewardAmount.text = $"+{targetAmount}";
        } else if (type == "6") {
            effectText.text = "화약고가 충전됐어요!";
            effectText.transform.DOLocalMoveY(45f, 0);
            uiImage.dynamiteReward.gameObject.SetActive(true);
            uiText.dynamiteRewardAmount.text = $"+{targetAmount}";
        }
        afterPurchaseEffectCanvas.canvasGroup.DOFade(1, 0.2f).OnComplete(() => {
            afterPurchaseEffectCanvas.canvasGroup.DOFade(0, 1f).SetDelay(1f).SetEase(Ease.Linear);
        });
    }

    public void HideHeartShopController() 
    {
        var heartShopController = FindObjectOfType<HeartShopController>();
        if (heartShopController != null)
            heartShopController.ToggleHeartShopCanvas(false);
    }

    public void SetRewardSprite()
    {
        switch (ProductController.rewardType)
        {
            case Constants.REWARD_TYPE.HEART: 
            {
                uiImage.heartReward.sprite = rewardSprite.spHeart;
                break;
            }
            case Constants.REWARD_TYPE.DIAMOND:
            {
                uiImage.heartReward.sprite = rewardSprite.spDiamond;
                break;
            }
            case Constants.REWARD_TYPE.GOLD_MINE:
            {
                uiImage.heartReward.sprite = rewardSprite.spGoldMine;
                break;
            }
            case Constants.REWARD_TYPE.EXPLOSIVE_WAREHOUSE:
            {
                uiImage.heartReward.sprite = rewardSprite.spDynamite;
                break;
            }
        }
    }
}
