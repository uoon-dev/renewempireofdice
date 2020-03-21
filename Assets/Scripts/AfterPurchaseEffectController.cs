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

    void Start()
    {
        Initialize();
        AfterPurchaseEffectCanvas.GetComponent<CanvasGroup>().DOFade(0, 0);
    }

    private void Initialize()
    {
        diamondController = FindObjectOfType<DiamondController>();
    }

    public void ShowScreen(string type, int targetAmount)
    {
        // HideHeartShopController();

        Text effectText = AfterPurchaseEffectCanvas.transform.GetChild(0).GetComponentInChildren<Text>();
        GameObject effectImage = AfterPurchaseEffectCanvas.transform.GetChild(0).GetChild(4).gameObject;
        GameObject heartReward = AfterPurchaseEffectCanvas.transform.GetChild(0).GetChild(5).gameObject;
        GameObject heartRewardAmount = AfterPurchaseEffectCanvas.transform.GetChild(0).GetChild(5).transform.GetChild(0).gameObject;
        GameObject diamondAmount = AfterPurchaseEffectCanvas.transform.GetChild(0).GetChild(6).gameObject;
        GameObject diamondRewardAmount = AfterPurchaseEffectCanvas.transform.GetChild(0).GetChild(6).transform.GetChild(0).gameObject;
        DOTween.Kill(AfterPurchaseEffectCanvas.GetComponent<CanvasGroup>());

        if (type == "0") {
            effectText.text = "하트가 충전됐어요!";
            effectText.transform.DOLocalMoveY(41f, 0);
            heartBar.SetActive(false);
            effectImage.SetActive(false);
            heartReward.SetActive(true);
            diamondAmount.SetActive(false);
            heartRewardAmount.GetComponent<Text>().text = $"+{targetAmount}";
        } else if (type == "1") {
            effectText.text = "충전 속도가 빨라집니다";
            heartBar.SetActive(false);
            effectImage.SetActive(true);
            heartReward.SetActive(false);
            diamondAmount.SetActive(false);
        } else if (type == "2") {
            effectText.text = "별 3개 달성 보상!";
            heartBar.SetActive(false);
            effectImage.SetActive(false);
            heartReward.SetActive(true);
            heartRewardAmount.GetComponent<Text>().text = "2";
            diamondAmount.SetActive(false);
        } else if (type == "3") {
            effectText.text = "스테이지 보상!";
            heartBar.SetActive(false);
            effectImage.SetActive(false);
            heartReward.SetActive(true);
            heartRewardAmount.GetComponent<Text>().text = "FULL";
            diamondAmount.SetActive(false);
        } else if (type == "4") {
            effectText.text = "다이아가 충전됐어요!";
            effectText.transform.DOLocalMoveY(41f, 0);
            heartBar.SetActive(false);
            effectImage.SetActive(false);
            heartReward.SetActive(false);
            diamondAmount.SetActive(true);
            diamondRewardAmount.GetComponent<Text>().text = $"+{targetAmount}";
        }

        AfterPurchaseEffectCanvas.GetComponent<CanvasGroup>().DOFade(1, 0.2f).OnComplete(() => {
            AfterPurchaseEffectCanvas.GetComponent<CanvasGroup>().DOFade(0, 1f).SetDelay(1f).SetEase(Ease.Linear).OnComplete(() => {
                // AfterPurchaseEffectCanvas.gameObject.SetActive(false);
            });
        });
    }

    public void HideHeartShopController() {
        var heartShopController = FindObjectOfType<HeartShopController>();
        if (heartShopController != null)
            heartShopController.ToggleHeartShopCanvas(false);
    }
}
