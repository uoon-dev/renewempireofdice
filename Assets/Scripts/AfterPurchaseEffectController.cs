using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AfterPurchaseEffectController : MonoBehaviour
{   
    [SerializeField] GameObject AfterPurchaseEffectCanvas = null;
    [SerializeField] GameObject heartBar = null;
    DiamondController diamondController;
    LevelLoader levelLoader;
    Text effectText;
    Transform body;
    GameObject effectImage;
    GameObject heartReward;
    GameObject heartRewardAmount;
    GameObject diamondRward;
    GameObject diamondRewardAmount;
    GameObject goldMineRward;
    GameObject goldMineRewardAmount;
    GameObject explosiveWarehouseRward;
    GameObject explosiveWarehouseRewardAmount;

    void Start()
    {
        Initialize();
        AfterPurchaseEffectCanvas.GetComponent<CanvasGroup>().DOFade(0, 0);
    }

    private void Initialize()
    {
        body = AfterPurchaseEffectCanvas.transform.GetChild(0);
        diamondController = FindObjectOfType<DiamondController>();
        levelLoader = FindObjectOfType<LevelLoader>();

        effectText = body.GetComponentInChildren<Text>();
        effectImage = body.GetChild(4).gameObject;
        heartReward = body.GetChild(5).gameObject;
        heartRewardAmount = body.GetChild(5).transform.GetChild(0).gameObject;
        diamondRward = body.GetChild(6).gameObject;
        diamondRewardAmount = body.GetChild(6).transform.GetChild(0).gameObject;
        
        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.LEVEL)
        {
            goldMineRward = body.GetChild(7).gameObject;
            goldMineRewardAmount = body.GetChild(7).transform.GetChild(0).gameObject;
            explosiveWarehouseRward = body.GetChild(8).gameObject;
            explosiveWarehouseRewardAmount = body.GetChild(8).transform.GetChild(0).gameObject;
        }
    }

    public void ShowScreen(string type, int targetAmount)
    {
        // HideHeartShopController();

        DOTween.Kill(AfterPurchaseEffectCanvas.GetComponent<CanvasGroup>());
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
            heartReward.SetActive(true);
            heartRewardAmount.GetComponent<Text>().text = $"+{targetAmount}";
        } else if (type == "1") {
            effectText.text = "충전 속도가 빨라집니다";
            effectImage.SetActive(true);
        } else if (type == "2") {
            effectText.text = "별 3개 달성 보상!";
            heartReward.SetActive(true);
            heartRewardAmount.GetComponent<Text>().text = "2";
        } else if (type == "3") {
            effectText.text = "스테이지 보상!";
            heartReward.SetActive(true);
            heartRewardAmount.GetComponent<Text>().text = "FULL";
        } else if (type == "4") {
            effectText.text = "다이아가 충전됐어요!";
            effectText.transform.DOLocalMoveY(41f, 0);
            diamondRward.SetActive(true);
            diamondRewardAmount.GetComponent<Text>().text = $"+{targetAmount}";
        } else if (type == "5") {
            effectText.text = "황금광산이 충전됐어요!";
            effectText.transform.DOLocalMoveY(45f, 0);
            goldMineRward.SetActive(true);
            goldMineRewardAmount.GetComponent<Text>().text = $"+{targetAmount}";
        } else if (type == "6") {
            effectText.text = "화약고가 충전됐어요!";
            effectText.transform.DOLocalMoveY(45f, 0);
            explosiveWarehouseRward.SetActive(true);
            explosiveWarehouseRewardAmount.GetComponent<Text>().text = $"+{targetAmount}";
        }
//
        AfterPurchaseEffectCanvas.GetComponent<CanvasGroup>().DOFade(1, 0.2f).OnComplete(() => {
            AfterPurchaseEffectCanvas.GetComponent<CanvasGroup>().DOFade(0, 1f).SetDelay(1f).SetEase(Ease.Linear);
        });
    }

    public void HideHeartShopController() {
        var heartShopController = FindObjectOfType<HeartShopController>();
        if (heartShopController != null)
            heartShopController.ToggleHeartShopCanvas(false);
    }
}
