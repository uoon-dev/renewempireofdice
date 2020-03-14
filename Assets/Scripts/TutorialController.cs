﻿namespace Controllers.TutorialController {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using RedBlueGames.Tools.TextTyper;
    using UnityEngine.Analytics;

    public class TutorialController : MonoBehaviour
    {
        [SerializeField] Sprite[] guiderSprites = null;
        public static Sprite[] staticGuiderSprites = null;
        public static int tutorialCount = 1;
        public static bool jumping = false;
        private static GameObject tutorialCanvas = null;
        private static GameObject arrow = null;
        private static GameObject highlight = null;
        private static GameObject moneyArea = null;
        private static GameObject dices = null;
        private static GameObject blocks = null;
        private static GameObject nextButton = null;
        private static int cloneArrowShowCount = 6;
        private static int isShownFisrtDiceArrow = 1;

        void Start()
        {
            tutorialCount = 1;
            tutorialCanvas = gameObject;
            staticGuiderSprites = guiderSprites;
            cloneArrowShowCount = 6;
            ToggleCanvasBody(0);
            TutorialController.HideNextButton();
        }

        public void RealStart()
        {
            AnalyticsEvent.TutorialStart();
            GameObject.Find("Intro Canvas").SetActive(false);
            ToggleCanvasBody(1);
            TextTyperTester.ShowScript();
            Vector3 firstPosition = new Vector3(-127, 68.7f, 1);
            GameObject.Find("Highlight 3").transform.DOLocalMove(firstPosition, 0).SetDelay(1f);
            GameObject.Find("Highlight 3").GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            GameObject.Find("Highlight 3").GetComponent<Rigidbody2D>().DORotate(360, 10).SetLoops(-1, LoopType.Restart);
        }

        public static void SetTutorialCount(int count) {
            AnalyticsEvent.TutorialStep(count);
            tutorialCount = count;
        }

        public static int GetTutorialCount() {
            return tutorialCount;
        }

        public static bool Jump() {
            return jumping;
        }

        public static void Jump(bool isJumping) {
            jumping = isJumping;
        }

        public static void AllowClickEventResetDiceScreen() {
            moneyArea = GameObject.Find("Money Area");
            moneyArea.GetComponent<Image>().raycastTarget = true;
        }

        public static void PreventClickEventResetDiceScreen() {
            moneyArea = GameObject.Find("Money Area");            
            moneyArea.GetComponent<Image>().raycastTarget = false;
        }

        public static void AllowClickEventNextButton() {
            nextButton = GameObject.Find("Description");
            nextButton.GetComponent<Button>().interactable = true;
        }

        public static void ShowNextButton() {
            GameObject.Find("Next Button").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        public static void PreventClickEventNextButton() {
            nextButton = GameObject.Find("Description");
            nextButton.GetComponent<Button>().interactable = false;
        }

        public static void HideNextButton() {
            GameObject.Find("Next Button").GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }

        
        public static void AllowClickEventDices() {
            dices = GameObject.Find("Dices");

            if (TutorialController.GetTutorialCount() == 4) {
                dices.transform.GetChild(0).GetComponentInChildren<Image>().raycastTarget = true;
            } else {
                if (TutorialController.GetTutorialCount() == 10) {
                    int generatedDiceIndex = 0;
                    for (int i = 0; i < dices.transform.childCount; i++) {
                        if (dices.transform.GetChild(i).GetComponentInChildren<Text>().text != "0")
                        {
                            generatedDiceIndex = i;
                        }
                    }
                    dices.transform.GetChild(generatedDiceIndex).GetComponentInChildren<Image>().raycastTarget = true;
                } else {
                    for (int i = 0; i < dices.transform.childCount; i++) {
                        dices.transform.GetChild(i).GetComponentInChildren<Image>().raycastTarget = true;
                    }
                }
            }
        }

        public static void PreventClickEventDices() {
            dices = GameObject.Find("Dices");

            for (int i = 0; i < dices.transform.childCount; i++) {
                dices.transform.GetChild(i).GetComponentInChildren<Image>().raycastTarget = false;
            }
        }

        public static void AllowClickEventBlocks() {
            blocks = GameObject.Find("Blocks");
            Debug.Log(TutorialController.GetTutorialCount());
            switch(TutorialController.GetTutorialCount()) {
                case 5: {
                    blocks.transform.GetChild(0).GetComponentInChildren<Button>().interactable = true;
                    break;
                }
                case 8: {
                    blocks.transform.GetChild(1).GetComponentInChildren<Button>().interactable = true;
                    break;
                }
                case 10: {
                    blocks.transform.GetChild(4).GetComponentInChildren<Button>().interactable = true;
                    break;
                }
                default: { 
                    for (int i = 0; i < blocks.transform.childCount; i++) {
                        if (blocks.transform.GetChild(i).GetComponent<Block>().isClickable) {
                            blocks.transform.GetChild(i).GetComponentInChildren<Button>().interactable = true;
                        }
                    }                    
                    break;
                }
            }
        }

        public static void PreventClickEventBlocks() {
            blocks = GameObject.Find("Blocks");

            for (int i = 0; i < blocks.transform.childCount; i++) {
                if (blocks.transform.GetChild(i).GetComponentInChildren<Button>() != null) {
                    blocks.transform.GetChild(i).GetComponentInChildren<Button>().interactable = false;
                }
            }
        }

        public static void ControllArrowUI() {
            moneyArea = GameObject.Find("Money Area");
            dices = GameObject.Find("Dices");
            // DOTween.KillAll();

            switch(TutorialController.GetTutorialCount()) {
                case 2: {
                    TutorialController.HideNextButton();
                    GameObject.Find("Highlight 3").GetComponent<CanvasGroup>().DOFade(0, 0);
                    
                    GameObject.Find("Arrow 1").GetComponent<CanvasGroup>().DOFade(0, 0);
                    Vector3 firstPosition = new Vector3(moneyArea.transform.localPosition.x - 42f, moneyArea.transform.localPosition.y + 45f, 1);
                    GameObject.Find("Arrow 1").transform.localPosition = firstPosition;
                    Vector3 secondPosition = new Vector3(moneyArea.transform.localPosition.x - 47f , moneyArea.transform.localPosition.y + 50f, 1);
                    GameObject.Find("Arrow 1").transform.DOLocalMove(secondPosition, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    GameObject.Find("Arrow 1").GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[3];
                    break;
                }
                case 3: {
                    GameObject.Find("Arrow 1").GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() => {
                        GameObject.Find("Arrow 1").gameObject.SetActive(false);
                    });
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[2];
                    break;
                }
                case 4: {
                    Vector3 firstPosition = new Vector3(dices.transform.GetChild(0).transform.localPosition.x - 50f , dices.transform.GetChild(0).transform.localPosition.y + 60f, 1);
                    Vector3 secondPosition = new Vector3(dices.transform.GetChild(0).transform.localPosition.x - 55f , dices.transform.GetChild(0).transform.localPosition.y + 65f, 1);
                    GameObject.Find("Arrow 2").transform.localPosition = firstPosition;   
                    GameObject.Find("Arrow 2").transform.DOLocalMove(secondPosition, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    GameObject.Find("Arrow 2").GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[1];
                    TutorialController.HideNextButton();
                    break;
                }
                case 5: {
                    GameObject.Find("Arrow 3").GetComponent<Rigidbody2D>().DORotate(-90, 0);
                    Vector3 firstPosition = new Vector3(blocks.transform.localPosition.x + 87f , blocks.transform.localPosition.y + 85f, 1);
                    Vector3 secondPosition = new Vector3(blocks.transform.transform.localPosition.x + 92f , blocks.transform.transform.localPosition.y + 90f, 1);
                    GameObject.Find("Arrow 3").transform.DOLocalMove(firstPosition, 0).SetDelay(0.1f);
                    GameObject.Find("Arrow 3").transform.DOLocalMove(secondPosition, 0.5f).SetDelay(0.1f).SetLoops(-1, LoopType.Yoyo);

                    // GameObject.Find("Arrow 3").GetComponent<CanvasGroup>().DOFade(1, 1f).SetDelay(0.2f);
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[0];
                    break;
                }
                case 6: {
                    GameObject.Find("Arrow 3").GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() => {
                        GameObject.Find("Arrow 3").GetComponent<Rigidbody2D>().DORotate(0, 0);
                    });
                    GameObject.Find("Highlight 1").GetComponent<CanvasGroup>().DOFade(0, 0.2f);

                    Vector3 firstPosition = new Vector3(blocks.transform.localPosition.x + 45f, blocks.transform.localPosition.y + 47f, 1);
                    GameObject.Find("Highlight 1").transform.DOLocalMove(firstPosition, 0);
                    GameObject.Find("Highlight 1").GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    GameObject.Find("Highlight 1").GetComponent<Rigidbody2D>().DORotate(360, 10).SetLoops(-1, LoopType.Restart);
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[5];
                    break;
                }
                case 7: {
                    TutorialController.HideNextButton();
                    GameObject.Find("Highlight 1").GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() => {
                        GameObject.Find("Highlight 1").gameObject.SetActive(false);
                    });
                    Vector3 firstPosition = new Vector3(180, 0, 1);
                    GameObject.Find("Highlight 2").transform.DOLocalMove(firstPosition, 0).SetDelay(0.1f);
                    GameObject.Find("Highlight 2").GetComponent<CanvasGroup>().DOFade(1, 0.2f).SetDelay(0.1f);
                    GameObject.Find("Highlight 2").GetComponent<Rigidbody2D>().DORotate(360, 10).SetLoops(-1, LoopType.Restart);

                    break;
                }
                case 8: {
                    TutorialController.HideNextButton();
                    GameObject.Find("Highlight 2").GetComponent<CanvasGroup>().DOFade(0, 0f).OnComplete(() => {
                        GameObject.Find("Highlight 2").gameObject.SetActive(false);
                    });

                    for (int i = 0; i < dices.transform.childCount; i++) {
                        var dice = dices.transform.GetChild(i);
                        var diceArrow = dice.transform.GetChild(3);

                        Vector3 firstPosition = new Vector3(diceArrow.localPosition.x , diceArrow.localPosition.y, 1);
                        Vector3 secondPosition = new Vector3(diceArrow.localPosition.x - 3f , diceArrow.localPosition.y + 3, 1);

                        diceArrow.transform.DOLocalMove(firstPosition, 0);
                        diceArrow.transform.DOLocalMove(secondPosition, 0.5f).SetLoops(-1, LoopType.Yoyo);

                        diceArrow.GetComponent<CanvasGroup>().DOFade(0, 0);
                        diceArrow.gameObject.SetActive(true);
                        diceArrow.GetComponent<CanvasGroup>().DOFade(1, 0.2f);                        
                    }

                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[0];

                    break;
                }
                case 9: {
                    TutorialController.HideNextButton();
                    Transform tutorialCanvasBox = tutorialCanvas.transform.GetChild(1);
                    Object.DestroyImmediate(tutorialCanvasBox.gameObject);
                 
                    ToggleCanvasBody(1);
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[5];
                    GameObject.Find("Arrow 3").SetActive(false);
                    break;
                }
                case 10: {
                    // go to dice
                    TutorialController.HideNextButton();
                    var dices = FindObjectsOfType<Dice>();
                    Dice newDice = null;
                    foreach (Dice dice in dices)
                    {
                        if (!dice.IsDestroyed())
                        {
                            newDice = dice;
                        }
                    }

                    DOTween.KillAll();

                    Vector3 firstPosition2 = new Vector3(newDice.transform.localPosition.x - 55f , newDice.transform.localPosition.y + 65f, 1);
                    Vector3 secondPosition2 = new Vector3(newDice.transform.localPosition.x - 60f , newDice.transform.localPosition.y + 70f, 1);
                    GameObject.Find("Arrow 2").transform.localPosition = firstPosition2;
                    GameObject.Find("Arrow 2").transform.DOLocalMove(secondPosition2, 0.5f).SetLoops(-1, LoopType.Yoyo);

                    // go to block
                    GameObject.Find("Arrow 4").GetComponent<CanvasGroup>().DOFade(0, 0);
                    GameObject.Find("Arrow 4").GetComponent<Rigidbody2D>().DORotate(-90, 0);
                    Vector3 firstPosition4 = new Vector3(blocks.transform.localPosition.x + 130f , blocks.transform.localPosition.y + 135f, 1);
                    Vector3 secondPosition4 = new Vector3(blocks.transform.transform.localPosition.x + 135f , blocks.transform.transform.localPosition.y + 140f, 1);
                    GameObject.Find("Arrow 4").transform.DOLocalMove(firstPosition4, 0);
                    GameObject.Find("Arrow 4").transform.DOLocalMove(secondPosition4, 0.5f).SetLoops(-1, LoopType.Yoyo);

                    ToggleDiceArrow();

                    if (jumping == false) {
                        ToggleCanvasBody(0);
                    }

                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[4];
                    break;
                }
                case 11: {                    
                    GameObject.Find("Arrow 4").GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() => {
                        GameObject.Find("Arrow 4").gameObject.SetActive(false);
                        tutorialCanvas.GetComponent<CanvasGroup>().DOFade(1, 0);
                    });

                    Vector3 firstPosition = new Vector3(-127, 68.7f, 1);
                    GameObject.Find("Highlight 3").transform.DOLocalMove(firstPosition, 0);
                    GameObject.Find("Highlight 3").GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                    GameObject.Find("Highlight 3").GetComponent<Rigidbody2D>().DORotate(360, 10).SetLoops(-1, LoopType.Restart);

                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[4];
                    break;
                }
                case 12: {
                    // go to dice
                    TutorialController.HideNextButton();
                    GameObject.Find("Arrow 5").GetComponent<CanvasGroup>().DOFade(0, 0);
                    Vector3 firstPosition = new Vector3(moneyArea.transform.localPosition.x - 45f, moneyArea.transform.localPosition.y + 45f, 1);
                    GameObject.Find("Arrow 5").transform.localPosition = firstPosition;
                    Vector3 secondPosition = new Vector3(moneyArea.transform.localPosition.x - 50f , moneyArea.transform.localPosition.y + 50f, 1);
                    GameObject.Find("Arrow 5").transform.DOLocalMove(secondPosition, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    GameObject.Find("Arrow 5").GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    GameObject.Find("Highlight 3").GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() => {
                        GameObject.Find("Highlight 3").gameObject.SetActive(false);
                    });
                    break;
                }
                case 13: {
                    if (GameObject.Find("Arrow 5") != null) {
                        GameObject.Find("Arrow 5").GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() => {
                            GameObject.Find("Arrow 5").gameObject.SetActive(false);
                        });
                    }
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[0];
                    break;
                }
                case 14: {
                    TutorialController.HideNextButton();
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[3];
                    break;
                }
                case 15: {
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[5];
                    break;
                }
                case 16: {
                    GameObject.Find("Guider").GetComponent<Image>().sprite = staticGuiderSprites[0];
                    // PreventClickEventDices();
                    break;
                }
                // case 10: {
                //     blocks.transform.GetChild(4).GetComponentInChildren<Button>().interactable = true;
                //     break;
                // }
                // default: { 
                //     for (int i = 0; i < blocks.transform.childCount; i++) {
                //         blocks.transform.GetChild(i).GetComponentInChildren<Button>().interactable = true;
                //     }                    
                //     break;
                // }
            }
        }

        public static void ToggleClonedArrow(int index)
        {
            var selectedDice = dices.transform.GetChild(index);
            var selectedDiceArrow = selectedDice.transform.GetChild(3);
            float isShow = selectedDiceArrow.GetComponent<CanvasGroup>().alpha;
            selectedDiceArrow.GetComponent<CanvasGroup>().DOFade(1 - isShow, 0.25f);

            if (isShow == 1) {
                cloneArrowShowCount--;
            } else {
                cloneArrowShowCount++;
            }
            
            if (cloneArrowShowCount == 0) {
                Vector3 firstPosition = new Vector3(blocks.transform.localPosition.x + 5, blocks.transform.localPosition.y + 135f, 1);
                Vector3 secondPosition = new Vector3(blocks.transform.transform.localPosition.x, blocks.transform.transform.localPosition.y + 140f, 1);
                GameObject.Find("Arrow 3").transform.DOLocalMove(firstPosition, 0);
                GameObject.Find("Arrow 3").transform.DOLocalMove(secondPosition, 0.5f).SetLoops(-1, LoopType.Yoyo);
                GameObject.Find("Arrow 3").GetComponent<CanvasGroup>().DOFade(1, 0.2f);
                ToggleCanvasBody(0);
            } else {
                GameObject.Find("Arrow 3").GetComponent<CanvasGroup>().DOFade(0, 0.2f);
            }
            
        }

        public static void ToggleDiceArrow()
        {
            float isShow = GameObject.Find("Arrow 2").GetComponent<CanvasGroup>().alpha;
            GameObject.Find("Arrow 2").GetComponent<CanvasGroup>().DOFade(1 - isShow, 0.2f);

            if (TutorialController.GetTutorialCount() == 5) 
            {
                GameObject.Find("Arrow 3").GetComponent<CanvasGroup>().DOFade(isShow, 0.2f);
            } else
            {
                GameObject.Find("Arrow 4").GetComponent<CanvasGroup>().DOFade(isShow, 0.2f);
            }
        }

        public static void ToggleCanvasBody(int isShow) 
        {
            Transform tutorialCanvasBody = tutorialCanvas.transform.GetChild(0);
            tutorialCanvasBody.GetComponent<CanvasGroup>().DOFade(isShow, 0);
        }
    }
}
