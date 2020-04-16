using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using DG.Tweening;

[Serializable]
public class RightArea {
    public Transform field;
    public Transform moneyArea;
    public Transform moneyImage;
    public Transform attackPowerText;
    public Transform attackPowerImage;
    public Canvas resetDiceScreen;
    public Canvas status;
    public Canvas attackGage;
}

[Serializable]
public class GuideItems {
    public CanvasGroup indicateArrowCanvasGroup;
    public CanvasGroup arrowCanvasGroup;
    public CanvasGroup outlineCanvasGroup;
    public CanvasGroup outlineCircleCanvasGroup;
    public CanvasGroup outlineRectCanvasGroup;
    public CanvasGroup ovalCanvasGroup;
    public CanvasGroup toastCanvasGroup;
    public Transform arrowTransform;
}

[Serializable]
public class GuideBox {
    public CanvasGroup subDialogueContainerCanvasGroup;
    public CanvasGroup subGuiderImageCanvasGroup;
    public LayoutElement subSuperTextLayoutElement;
    public VerticalLayoutGroup subTextBoxVerticalLayoutGroup;
    public Image subGuiderImage;
}

public class NewTutorialController : MonoBehaviour
{
    TutorialDialogueController tutorialDialogueController; 
    BlockController blockController;
    DiceController diceController;
    ResetDiceController resetDiceController;
    [SerializeField]
    GameObject tutorialGuideCanvas;
    [SerializeField]
    GameObject pannelSetting;
    [SerializeField]
    GameObject subSuperText;
    [SerializeField]
    GameObject subTextBox;

    [SerializeField]
    Sprite[] guiderImages;
    GameObject introCanvas;
    GameObject attackGage;
    [SerializeField] 
    Canvas leftArea;

    [SerializeField]
    GameObject MainDialogueContainer;
    [SerializeField]
    GameObject SubDialogueContainer;
    GameObject turn;
    GameObject guideItem;
    GameObject toast;
    GameObject oval;
    GameObject indicateArrow;
    GameObject arrow;
    GameObject outline;
    GameObject outlineDice;
    GameObject outlineCircle;
    GameObject outlineRect;
    GameObject outlineFullRect;
    GameObject blocks;
    public RightArea rightArea;
    public GuideItems guideItems;
    public GuideBox guideBox;
    public bool dialogueUpdated = false;
    public bool isOver = true; // turn이 초과된 경우

    // Start is called before the first frame update
    void Start()
    {
        introCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.INTRO_CANVAS);
        // Initialize();
    }

    void Update()
    {
        if (tutorialDialogueController != null && dialogueUpdated)
        {
            int dialogueTurn = TutorialDialogueController.dialogueTurn;
            switch (dialogueTurn)
            {
                case 2: 
                {
                    var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);
                    var lastBlockPosition = lastBlock.transform.position;
                    var dialogues = FindObjectsOfType<TutorialDialogueController>();

                    SubDialogueContainer.transform.DOMove(new Vector2(1.7f * lastBlockPosition.x, lastBlockPosition.y - lastBlockPosition.y / 5), 0f);
                    MainDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0f)
                        .OnComplete(() => {
                            dialogues[0].lines[0] = "<d=5>주군, 이 정도면 수업은 충분한 것 같습니다! 이제 주군의 지혜와 강력한 주사위로 마왕성을 무찔러주십시오!";
                            MainDialogueContainer.SetActive(false);
                            SubDialogueContainer.SetActive(true);

                            if (isOver)
                            {
                                TutorialDialogueController.dialogueTurn = dialogueTurn - 1;
                                isOver = false;
                            }

                            lastBlock.GetComponent<Canvas>().overrideSorting = true;
                            lastBlock.GetComponent<Canvas>().sortingOrder = 102;
                            
                            oval.transform.position =
                                new Vector2(lastBlock.transform.position.x, lastBlock.transform.position.y + 10);
                            oval.GetComponent<Rigidbody2D>().DORotate(360, 10).SetLoops(-1, LoopType.Restart);
                            guideItems.ovalCanvasGroup.DOFade(1, 0f);
                        });

                    break;
                }
                case 3:
                {
                    var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);
                    var rightAreaPosition = rightArea.field.transform.position;
                    var firstDice = diceController.GetOneDice("Dice (1)");
                    var secondDice = diceController.GetOneDice("Dice (2)");
                    var thirdDice = diceController.GetOneDice("Dice (3)");

                    lastBlock.GetComponent<Canvas>().overrideSorting = false;
                    lastBlock.GetComponent<Canvas>().sortingOrder = 5;
                    guideItems.ovalCanvasGroup.DOFade(0, 0f);
                    // TutorialDialogueController.isClickable = false;

                    // subTextBox.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 0);

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(0, 0f));
                    sequence.AppendCallback(() => {
                        guideBox.subGuiderImage.transform.localScale = new Vector2(1, 1);
                        guideBox.subSuperTextLayoutElement.minWidth = 250;
                        subSuperText.gameObject.SetActive(false);
                        subSuperText.gameObject.SetActive(true);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(3.8f, 0);
                    });
                    sequence.Append(guideBox.subGuiderImage.transform.DOLocalMove(new Vector2(-84.8f, 27.7f), 0));
                    sequence.Join(guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(250f, 0), 0f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x * 0.8f, rightAreaPosition.y * 0.85f), 0f));
                    sequence.AppendInterval(0f);
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(1, 0f));
                    sequence.AppendCallback(() => {
                        diceController.BounceDices();
                        firstDice.ClickDice();
                        firstDice.ToggleAllowClick(false);
                        secondDice.ClickDice();
                        secondDice.ToggleAllowClick(false);
                        thirdDice.ClickDice();
                        thirdDice.ToggleAllowClick(false);
                        rightArea.attackGage.overrideSorting = true;
                        rightArea.attackGage.sortingOrder = 102;
                        oval.transform.DOMove(new Vector3(attackGage.transform.position.x + 10, attackGage.transform.position.y + 10, 1), 0f);
                        oval.GetComponent<RectTransform>().sizeDelta = new Vector2(103, 103);
                        guideItems.ovalCanvasGroup.DOFade(1, 0f);
                        guideItems.indicateArrowCanvasGroup.alpha = 1;
                    });
                    sequence.Play(); 
                    break;
                }
                case 4: 
                {
                    var middleBlock = blockController.GetOneBlock(Constants.TYPE.MIDDLE_BLOCK);
                    var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);
                    var lastBlockPosition = lastBlock.transform.position;
                    var dicePosition = middleBlock.transform.position;

                    guideBox.subSuperTextLayoutElement.minWidth = 280;
                    subSuperText.gameObject.SetActive(false);
                    subSuperText.gameObject.SetActive(true);
                    rightArea.attackGage.overrideSorting = false;
                    rightArea.attackGage.sortingOrder = 6;
                    diceController.UnbounceDices();
                    guideItems.ovalCanvasGroup.DOFade(0, 0f);
                    guideItems.indicateArrowCanvasGroup.alpha = 0;

                    leftArea.overrideSorting = true;
                    leftArea.sortingOrder = 102;
                    outline.transform.position = 
                        new Vector2(middleBlock.transform.position.x - 5, middleBlock.transform.position.y - 5);

                    guideBox.subTextBoxVerticalLayoutGroup.padding.right = 75;

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(0, 0f));
                    sequence.AppendCallback(() => {
                        guideItems.outlineCanvasGroup.DOFade(1, 0f);
                        guideBox.subGuiderImage.transform.localScale = new Vector2(-1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(2f, 0);
                    });
                    sequence.Append(guideBox.subGuiderImage.transform.DOLocalMove(new Vector2(213.76f, 30.6f), 0));
                    sequence.Join(guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(280f, 0), 0f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2((Screen.width < 1000 ? 1.75f : 1.85f) * lastBlockPosition.x, dicePosition.y), 0f));
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(1, 0f));
                    sequence.Play(); 

                    break;
                }
                case 5: 
                {
                    subTextBox.transform.DOScaleY(0.7f, 0);
                    subSuperText.transform.DOScaleY(1.4f, 0);
                    guideBox.subGuiderImage.transform.DOLocalMoveY(19.5f, 0f);
                    guideItems.outlineCanvasGroup.DOFade(0, 0f);

                    break;
                }
                case 7:
                {
                    GameObject toast = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TOAST);
                    Sequence sequence = DOTween.Sequence();
                    var fisrtDice = diceController.GetOneDice("Dice (1)");
                    var dicePosition = fisrtDice.transform.position;
                    TutorialDialogueController.isClickable = false;
                    tutorialGuideCanvas.GetComponent<CanvasGroup>().interactable = false;
                    tutorialGuideCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    
                    diceController.ToggleDicesClick(false);
                    diceController.ToggleOneDiceClick("Dice (1)", true);

                    guideItems.arrowTransform.DOMove(new Vector3(
                        dicePosition.x - dicePosition.x / 15, 
                        dicePosition.y + dicePosition.y / 5, 
                        dicePosition.z), 0);

                    guideItems.toastCanvasGroup.DOFade(1, 0f);

                    sequence.Append(guideBox.subDialogueContainerCanvasGroup.DOFade(0, 0f));
                    sequence.AppendCallback(() => {
                        pannelSetting.SetActive(false);
                        rightArea.attackGage.overrideSorting = true;
                        rightArea.attackGage.sortingOrder = 102;
                        guideItems.arrowCanvasGroup.DOFade(1, 0f);
                        guideItems.arrowCanvasGroup.alpha = 1;
                    });
                    sequence.Append(guideItems.toastCanvasGroup.DOFade(1, 0f));
                    sequence.AppendCallback(() => {
                        guideItems.arrowTransform.DOMove(new Vector3(
                            dicePosition.x - dicePosition.x / 15 - 10, 
                            dicePosition.y + dicePosition.y / 5 + 10,
                            dicePosition.z), 0.2f).SetLoops(-1, LoopType.Yoyo);
                        Debug.Log(guideItems.arrowTransform.position);
                        Debug.Log(dicePosition);
                    });
                    sequence.Play();
                    break;
                }
                case 8:
                {
                    guideBox.subGuiderImage.sprite = guiderImages[2];

                    guideBox.subDialogueContainerCanvasGroup.DOFade(1, 0f);
                    pannelSetting.SetActive(true);
                    guideItems.arrowCanvasGroup.DOFade(0, 0f);
                    guideItems.toastCanvasGroup.DOFade(0, 0f);
                    
                    rightArea.attackGage.overrideSorting = false;
                    rightArea.attackGage.sortingOrder = 6;
                    rightArea.status.sortingOrder = 102;
                    
                    TutorialDialogueController.isClickable = true;

                    break;
                }
                case 9:
                {
                    guideBox.subGuiderImage.sprite = guiderImages[4];
                    var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);

                    outlineCircle.transform.position =
                        new Vector2(lastBlock.transform.position.x, lastBlock.transform.position.y + 10);
                    outlineCircle.GetComponent<Rigidbody2D>().DORotate(360, 10).SetLoops(-1, LoopType.Restart);
                    guideItems.outlineCircleCanvasGroup.DOFade(1, 0f);

                    outlineRect.transform.position =
                        new Vector2(turn.transform.position.x, turn.transform.position.y);
                    guideItems.outlineRectCanvasGroup.DOFade(1, 0f);

                    guideBox.subGuiderImage.transform.DOLocalMove(new Vector2(225.5f, 28.2f), 0f);
                    guideBox.subTextBoxVerticalLayoutGroup.padding.bottom = 70;
                    guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(300f, 0), 0f);

                    break;
                }
                case 10:
                {
                    guideBox.subGuiderImage.sprite = guiderImages[0];
                    guideBox.subTextBoxVerticalLayoutGroup.padding.bottom = 70;
                    guideItems.outlineCircleCanvasGroup.DOFade(0, 0f);
                    guideItems.outlineRectCanvasGroup.DOFade(0, 0f);
                    break;
                }
                case 11:
                {
                    var description = toast.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.DESC);
                    var secondDice = diceController.GetOneDice("Dice (2)");
                    var secondDicePosition = secondDice.transform.position;

                    var thirdDice = diceController.GetOneDice("Dice (3)");
                    var thirdDicePosition = thirdDice.transform.position;
                    GameObject clonedArrow = Instantiate(guideItems.arrowTransform.gameObject, transform.position, transform.rotation);

                    clonedArrow.transform.SetParent(guideItem.transform, false); 
                    pannelSetting.SetActive(false);
                    description.GetComponent<Text>().text = "주사위를 잘 선택한 뒤 공격해보세요!";
                    Block fisrtBlock = blockController.GetOneBlock(Constants.TYPE.FIRST_BLOCK);
                    fisrtBlock.ToggleAllowClick(false);

                    DOTween.Kill(guideItems.arrowTransform);
                    guideItems.arrowTransform.DOMove(new Vector3(
                        secondDicePosition.x - secondDicePosition.x / 17, 
                        secondDicePosition.y + secondDicePosition.y / 5, 
                        secondDicePosition.z), 0);

                    clonedArrow.transform.DOMove(new Vector3(
                        thirdDicePosition.x - thirdDicePosition.x / 17, 
                        thirdDicePosition.y + thirdDicePosition.y / 5, 
                        thirdDicePosition.z), 0);
                    
                    guideItems.arrowCanvasGroup.DOFade(1, 0f);
                    clonedArrow.GetComponent<CanvasGroup>().DOFade(1, 0f);

                    guideBox.subDialogueContainerCanvasGroup.DOFade(0, 0f).OnComplete(() => {
                        guideItems.toastCanvasGroup.DOFade(1, 0f);

                        guideItems.arrowTransform.DOMove(new Vector3(
                            secondDicePosition.x - secondDicePosition.x / 17 - 10, 
                            secondDicePosition.y + secondDicePosition.y / 5 + 10,
                            secondDicePosition.z), 0.2f).SetLoops(-1, LoopType.Yoyo);

                        clonedArrow.transform.DOMove(new Vector3(
                            thirdDicePosition.x - thirdDicePosition.x / 17 - 10, 
                            thirdDicePosition.y + thirdDicePosition.y / 5 + 10,
                            thirdDicePosition.z), 0.2f).SetLoops(-1, LoopType.Yoyo);
                    });

                    diceController.ToggleOneDiceClick("Dice (2)", true);
                    diceController.ToggleOneDiceClick("Dice (3)", true);
                    break;
                }
                case 12:
                {
                    rightArea.status.GetComponent<Canvas>().sortingOrder = 102;
                    
                    guideBox.subGuiderImage.sprite = guiderImages[2];
                    guideBox.subDialogueContainerCanvasGroup.DOFade(1, 0f);
                    pannelSetting.SetActive(true);
                    // turn.GetComponent<Canvas>().sortingOrder = 5;
                    guideItems.arrowCanvasGroup.DOFade(0, 0f);
                    guideItems.toastCanvasGroup.DOFade(0, 0f);

                    guideBox.subSuperTextLayoutElement.minWidth = 250;
                    subSuperText.gameObject.SetActive(false);
                    subSuperText.gameObject.SetActive(true);
                    guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(250f, 0), 0f);
                    guideBox.subGuiderImage.transform.DOLocalMove(new Vector2(197.4f, 27.7f), 0);

                    outlineRect.transform.position =
                        new Vector2(rightArea.moneyImage.transform.position.x + rightArea.moneyImage.transform.position.x / 50, rightArea.moneyImage.transform.position.y);
                    guideItems.outlineRectCanvasGroup.DOFade(1, 0f);

                    break;
                }
                case 13:
                {
                    guideBox.subGuiderImage.sprite = guiderImages[0];
                    rightArea.status.sortingOrder = 5;
                    leftArea.overrideSorting = false;
                    leftArea.sortingOrder = 5;                    
                    guideItems.outlineRectCanvasGroup.DOFade(0, 0f);
                    break;
                }
                case 15:
                {
                    var description = toast.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.DESC);
                    var fourDice = diceController.GetOneDice("Dice (4)");
                    var fourDicePosition = fourDice.transform.position;

                    var fiveDice = diceController.GetOneDice("Dice (5)");
                    var fiveDicePosition = fiveDice.transform.position;

                    var sixDice = diceController.GetOneDice("Dice (6)");
                    var sixDicePosition = sixDice.transform.position;
                    GameObject clonedArrow1 = Instantiate(guideItems.arrowTransform.gameObject, transform.position, transform.rotation);
                    GameObject clonedArrow2 = Instantiate(guideItems.arrowTransform.gameObject, transform.position, transform.rotation);

                    clonedArrow1.transform.SetParent(guideItem.transform, false); 
                    clonedArrow2.transform.SetParent(guideItem.transform, false);
                    clonedArrow2.name = clonedArrow2.name + "2";
                    pannelSetting.SetActive(false);
                    description.GetComponent<Text>().text = "주사위를 잘 선택한 뒤 공격해보세요!";
                    Block fisrtBlock = blockController.GetOneBlock(Constants.TYPE.FIRST_BLOCK);
                    fisrtBlock.ToggleAllowClick(false);

                    DOTween.Kill(guideItems.arrowTransform);
                    guideItems.arrowTransform.DOMove(new Vector3(
                        fourDicePosition.x - fourDicePosition.x / 17, 
                        fourDicePosition.y + fourDicePosition.y / 4, 
                        fourDicePosition.z), 0);
                    guideItems.arrowCanvasGroup.DOFade(1, 0f);

                    clonedArrow1.transform.DOMove(new Vector3(
                        fiveDicePosition.x - fiveDicePosition.x / 17, 
                        fiveDicePosition.y + fiveDicePosition.y / 4, 
                        fiveDicePosition.z), 0);
                    clonedArrow1.GetComponent<CanvasGroup>().DOFade(1, 0f);

                    clonedArrow2.transform.DOMove(new Vector3(
                        sixDicePosition.x - sixDicePosition.x / 17, 
                        sixDicePosition.y + sixDicePosition.y / 4, 
                        sixDicePosition.z), 0);
                    clonedArrow2.GetComponent<CanvasGroup>().DOFade(1, 0f);

                    guideBox.subDialogueContainerCanvasGroup.DOFade(0, 0f).OnComplete(() => {
                        TutorialDialogueController.isClickable = false;
                        guideItems.toastCanvasGroup.DOFade(1, 0f);

                        guideItems.arrowTransform.DOMove(new Vector3(
                            fourDicePosition.x - fourDicePosition.x / 17 - 10, 
                            fourDicePosition.y + fourDicePosition.y / 4 + 10,
                            fourDicePosition.z), 0.2f).SetLoops(-1, LoopType.Yoyo);

                        clonedArrow1.transform.DOMove(new Vector3(
                            fiveDicePosition.x - fiveDicePosition.x / 17 - 10, 
                            fiveDicePosition.y + fiveDicePosition.y / 4 + 10,
                            fiveDicePosition.z), 0.2f).SetLoops(-1, LoopType.Yoyo);

                        clonedArrow2.transform.DOMove(new Vector3(
                            sixDicePosition.x - sixDicePosition.x / 17 - 10, 
                            sixDicePosition.y + sixDicePosition.y / 4 + 10,
                            sixDicePosition.z), 0.2f).SetLoops(-1, LoopType.Yoyo);
                    });

                    diceController.ToggleOneDiceClick("Dice (4)", true);
                    diceController.ToggleOneDiceClick("Dice (5)", true);
                    diceController.ToggleOneDiceClick("Dice (6)", true);

                    blockController.GetOneBlock(Constants.TYPE.BOTTOM_MIDDLE_BLOCK).ToggleAllowClick(false);
                    blockController.GetOneBlock(Constants.TYPE.LEFT_MIDDLE_BLOCK).ToggleAllowClick(false);
                    break;
                }
                case 16:
                {
                    guideBox.subDialogueContainerCanvasGroup.DOFade(1, 0f);
                    guideItems.toastCanvasGroup.DOFade(0, 0f);
                    leftArea.overrideSorting = true;
                    leftArea.sortingOrder = 102;
                    pannelSetting.SetActive(true);
                    guideBox.subGuiderImage.sprite = guiderImages[2];
                    TutorialDialogueController.isClickable = true;                    
                    break;
                }
                case 17:
                {
                    Sequence sequence = DOTween.Sequence();
                    var sixDice = diceController.GetOneDice("Dice (6)");
                    var dicePosition = sixDice.transform.position;
                    var rightAreaPosition = rightArea.field.transform.position;
                    blockController.GetOneBlock(Constants.TYPE.BOTTOM_MIDDLE_BLOCK).ToggleAllowClick(true);
                    leftArea.sortingOrder = 5;
                                    
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(0, 0f));
                    sequence.AppendCallback(() => {
                        guideBox.subGuiderImage.sprite = guiderImages[0];
                        guideBox.subGuiderImage.transform.localScale = new Vector2(1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(3.6f, 0);
                    });
                    sequence.Append(guideBox.subGuiderImage.transform.DOLocalMove(new Vector2(-75.6f, 27.7f), 0));
                    sequence.Join(guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(220f, 0), 0f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x * 1.05f, rightAreaPosition.y * 0.9f), 0f));
                    sequence.AppendInterval(0f);
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(1, 0f));
                    sequence.AppendCallback(() => {
                        diceController.BounceDices();
                        rightArea.attackGage.overrideSorting = true;
                        rightArea.attackGage.sortingOrder = 102;
                        resetDiceController.ResetOneDice();
                        outlineFullRect.transform.position = 
                            new Vector2(dicePosition.x, dicePosition.y);
                        outlineFullRect.GetComponent<CanvasGroup>().DOFade(1, 0f);                        
                    });
                    sequence.AppendInterval(1f);
                    sequence.AppendCallback(() => {
                        subTextBox.GetComponent<Button>().interactable = true;
                    });
                    sequence.Play();

                    break;
                }
                case 18:
                {
                    DOTween.Kill(guideBox.subGuiderImage.transform);
                    guideBox.subGuiderImage.sprite = guiderImages[3];
                    guideBox.subGuiderImage.transform.DOLocalMove(new Vector3(-102.97f, 33.2f, 1), 0f);
                    guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(270f, 0), 0f);
                    outlineFullRect.GetComponent<CanvasGroup>().DOFade(0, 0f);
                    break;
                }
                case 19:
                {
                    Sequence sequence = DOTween.Sequence();
                    var rightAreaPosition = rightArea.field.transform.position;
                    
                    rightArea.status.sortingOrder = 102;
                    rightArea.resetDiceScreen.overrideSorting = true;
                    rightArea.resetDiceScreen.sortingOrder = 102;

                    ToggleClickEventResetDiceScreen(true);
                    TutorialDialogueController.isClickable = false;

                    DOTween.Kill(guideItems.arrowTransform);
                    guideItems.arrowTransform.DOMove(new Vector3(
                        rightArea.moneyArea.position.x - rightArea.moneyArea.position.x / 14,
                        rightArea.moneyArea.position.y + rightArea.moneyArea.position.y / 3f, 
                        rightArea.moneyArea.position.z), 0);

                    guideBox.subGuiderImage.sprite = guiderImages[0];
                    sequence.Append(guideBox.subGuiderImage.transform.DOLocalMove(new Vector2(-117.48f, 38.4f), 0f));
                    sequence.Join(guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(300f, 0), 0f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x, rightAreaPosition.y * 0.9f), 0f));
                    sequence.Append(guideItems.arrowCanvasGroup.DOFade(1, 0f));
                    sequence.AppendCallback(() => {
                        sequence.Append(guideItems.arrowCanvasGroup.DOFade(1, 0f));
                        guideItems.arrowTransform.DOMove(new Vector3(
                            rightArea.moneyArea.position.x - rightArea.moneyArea.position.x / 14 - 10, 
                            rightArea.moneyArea.position.y + rightArea.moneyArea.position.y / 3f + 10, 
                            rightArea.moneyArea.position.z), 0.2f).SetLoops(-1, LoopType.Yoyo);            
                    });

                    break;
                }
                case 20:
                {
                    Sequence sequence = DOTween.Sequence();
                    
                    var dicePosition = diceController.GetOneDice("Dice (2)").transform.position;
                    var rightAreaPosition = rightArea.field.transform.position;
                    
                    rightArea.resetDiceScreen.sortingOrder = 5;
                    rightArea.status.sortingOrder = 5;
                    TutorialDialogueController.isClickable = true;

                    outlineDice.transform.DOMove(new Vector3(dicePosition.x, dicePosition.y - dicePosition.y / 7, 1), 0);
                    outlineRect.transform.DOMove(new Vector3(rightArea.attackPowerText.position.x - rightArea.attackPowerText.position.x / 50, rightArea.attackPowerText.position.y, 1), 0);

                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(0, 0f));
                    sequence.AppendCallback(() => {
                        guideBox.subGuiderImage.transform.localScale = new Vector2(1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(3.6f, 0);
                        guideBox.subSuperTextLayoutElement.minWidth = 270;
                        subSuperText.gameObject.SetActive(false);
                        subSuperText.gameObject.SetActive(true);
                        guideItems.arrowCanvasGroup.DOFade(0, 0f);
                   });
                    sequence.AppendInterval(0f);
                    sequence.AppendCallback(() => {
                        resetDiceController.ResetDices();
                    });
                    sequence.Append(guideBox.subGuiderImage.transform.DOLocalMove(new Vector2(-103.8f, 45f), 0));
                    sequence.Join(guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(270f, 0), 0f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x * 0.7f, rightAreaPosition.y * 0.9f), 0f));
                    sequence.AppendInterval(0f);
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(1, 0f));
                    sequence.AppendCallback(() => {
                        diceController.BounceDices();
                        rightArea.attackGage.overrideSorting = true;
                        rightArea.attackGage.sortingOrder = 102;
                        rightArea.status.sortingOrder = 102;
                        outlineDice.GetComponent<CanvasGroup>().DOFade(1, 0f);
                        guideItems.outlineRectCanvasGroup.DOFade(1, 0f);
                    });
                    sequence.AppendInterval(1f);
                    sequence.AppendCallback(() => {
                        subTextBox.GetComponent<Button>().interactable = true;
                    });
                    sequence.Play();                     
                    break;
                }
                case 21:
                {
                    var attackPowerText = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_POWER_TEXT);
                    var attackPowerImage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_POWER_IMAGE);
                    var rightAreaPosition = rightArea.field.transform.position;
                    Sequence sequence = DOTween.Sequence();

                    rightArea.attackGage.overrideSorting = false;
                    rightArea.attackGage.sortingOrder = 5;

                    diceController.UnbounceDices();
                    leftArea.sortingOrder = 102;

                    sequence.Append(outlineDice.GetComponent<CanvasGroup>().DOFade(0, 0f));
                    sequence.Join(guideItems.outlineRectCanvasGroup.DOFade(0, 0f));
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(0, 0f));
                    sequence.AppendCallback(() => {
                        guideBox.subGuiderImage.sprite = guiderImages[4];                        
                        guideBox.subGuiderImage.transform.localScale = new Vector2(-1, 1);
                        subSuperText.GetComponent<SuperTextMesh>().baseOffset = new Vector2(1.8f, 0);
                    });
                    sequence.Append(guideBox.subGuiderImage.transform.DOLocalMove(new Vector2(188.27f, 23.2f), 0));
                    sequence.Join(guideBox.subSuperTextLayoutElement.DOMinSize(new Vector2(230f, 0), 0f));
                    sequence.Join(SubDialogueContainer.transform.DOMove(new Vector2(rightAreaPosition.x, rightAreaPosition.y * 0.9f), 0f));
                    sequence.AppendInterval(0f);
                    sequence.Append(guideBox.subGuiderImageCanvasGroup.DOFade(1, 0f));
                    sequence.AppendCallback(() => {
                        var lastBlock = blockController.GetOneBlock(Constants.TYPE.LAST_BLOCK);

                        outlineCircle.transform.position =
                            new Vector2(lastBlock.transform.position.x, lastBlock.transform.position.y + 10);
                        guideItems.outlineCircleCanvasGroup.DOFade(1, 0f);
                        outlineRect.transform.position =
                            new Vector2(turn.transform.position.x, turn.transform.position.y);
                        guideItems.outlineRectCanvasGroup.DOFade(1, 0f);
                    });

                    sequence.Play();

                    break;
                }
                case 22:
                {
                    leftArea.sortingOrder = 5;
                    rightArea.status.sortingOrder = 5;
                    guideItems.outlineCircleCanvasGroup.DOFade(0, 0f);
                    guideItems.outlineRectCanvasGroup.DOFade(0, 0f);                            
                    guideBox.subDialogueContainerCanvasGroup.DOFade(0, 0f)
                        .OnComplete(() => {
                            MainDialogueContainer.SetActive(true);
                            tutorialDialogueController.Apply();
                            MainDialogueContainer.GetComponent<CanvasGroup>().DOFade(1, 0f);
                            SubDialogueContainer.SetActive(false);
                        });
                    break;
                }
                case 24:
                {
                    MainDialogueContainer.GetComponent<CanvasGroup>().DOFade(0, 0f).OnComplete(() => {
                        diceController.ToggleDicesClick(true);
                        tutorialGuideCanvas.SetActive(false);
                        leftArea.overrideSorting = false;
                        rightArea.status.overrideSorting = false;
                        rightArea.resetDiceScreen.overrideSorting = false;
                    });
                    break;
                }                
            }
            dialogueUpdated = false;
        }   
    }

    private void Initialize()
    {
        TutorialDialogueController.dialogueTurn = 0;
        TutorialDialogueController.isClickable = true;

        blockController = FindObjectOfType<BlockController>();
        diceController = FindObjectOfType<DiceController>();
        resetDiceController = FindObjectOfType<ResetDiceController>();

        // tutorialGuideCanvas = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TUTORIAL_GUIDE_CANVAS);
        attackGage = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.ATTACK_GAGE);
        blocks = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.BLOCKS);
        guideItem = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.GUIDE_ITEM);
        turn = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TURN);
        toast = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.TOAST);

        oval = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.OVAL);
        outline = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE);
        outlineDice = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE_DICE);
        outlineCircle = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE_CIRCLE);
        outlineRect = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE_RECT);
        outlineFullRect = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.OUTLINE_FULL_RECT);
        indicateArrow = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.INDICATE_ARROW);

        ToggleClickEventResetDiceScreen(false);
    }

    public void ActiveDialogue()
    {
        introCanvas.SetActive(false);
        tutorialGuideCanvas.SetActive(true);
        tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
        Initialize();
    }

    public void MoveArrowToBlock(Block block)
    {
        Sequence sequence = DOTween.Sequence();
        block.ToggleAllowClick(true);
        var blockPosition = block.transform.position;        
        DOTween.Kill(guideItems.arrowTransform);
        DOTween.Kill(guideItems.arrowCanvasGroup);
        // sequence.AppendCallback(() => DOTween.Kill(guideItems.arrowTransform));
        // sequence.AppendInterval(0.1f);
        sequence.Append(guideItems.arrowCanvasGroup.DOFade(0, 0f));
        sequence.AppendCallback(() => {
            guideItems.arrowTransform.DOMove(new Vector3(
                blockPosition.x - blockPosition.x / 3,
                blockPosition.y + blockPosition.y / 5, 
                blockPosition.z), 0);

            guideItems.arrowCanvasGroup.DOFade(1, 0f);
            guideItems.arrowTransform.DOMove(new Vector3(
                blockPosition.x - blockPosition.x / 3 - 10,
                blockPosition.y + blockPosition.y / 5 + 10, 
                blockPosition.z), 0.2f).SetLoops(-1, LoopType.Yoyo);
        });
        sequence.Play();
    }

    public static void ToggleClickEventResetDiceScreen(bool isAllow) {
        var moneyArea = GameObject.Find("Money Area");
        moneyArea.GetComponent<Image>().raycastTarget = isAllow;
    }
}
