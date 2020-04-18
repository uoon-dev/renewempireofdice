using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using RedBlueGames.Tools.TextTyper;

[Serializable]
public class BlockComponent {

    [Serializable]
    public class UIAnimator {
        public Animator backgroundImageAnimator = null;
    }


    [Serializable]
    public class UITransform {
        public RectTransform backgroundImageRect = null;
    }

    [Serializable]
    public class UISprite {
        public Sprite lastBlockNormal = null;
        public Sprite lastBlockClickable = null;
        public Sprite lastBlockDestoryable = null;
    }

    [Serializable]
    public class UIImage {
        public Image backgroundImage = null;
    }

    [Serializable]
    public class Controller {
        public CameraShaker cameraShaker = null;
    }
}

public class Block : ControllerManager
{

    public BlockComponent blockComponent;
    public BlockComponent.UIAnimator uiAnimator;
    public BlockComponent.UIImage uiImage;
    public BlockComponent.UISprite uiSprite;
    public BlockComponent.UITransform uiTransform;
    public BlockComponent.Controller _controller;
    #region Sprite
    [SerializeField] Sprite clearLandImage = null;
    [SerializeField] Sprite clearLandOccupiedImage = null;
    [SerializeField] Sprite destroyableBlockImage = null;
    [SerializeField] Sprite mineImage = null;
    [SerializeField] Sprite dungeonImage = null;
    [SerializeField] Sprite armyImage = null;
    [SerializeField] Sprite armySlotImage = null;
    [SerializeField] Sprite wizardImage = null;
    [SerializeField] Sprite wizardSlotImage = null;
    [SerializeField] Sprite relicsImage = null;
    [SerializeField] Sprite relicsSlotImage = null;
    [SerializeField] Sprite horizontalImage = null;
    [SerializeField] Sprite verticalImage = null;
    [SerializeField] Sprite bombImage = null;
    [SerializeField] Sprite mineBackgroundImage = null;
    [SerializeField] Sprite dungeonBackgroundImage = null;
    [SerializeField] Sprite armyBackgroundImage = null;
    [SerializeField] Sprite wizardBackgroundImage = null;
    [SerializeField] Sprite relicsBackgroundImage = null;
    [SerializeField] Sprite horizontalBackgroundImage = null;
    [SerializeField] Sprite verticalBackgroundImage = null;
    [SerializeField] Sprite bombBackgroundImage = null;
    [SerializeField] Sprite lastBlockFinalImage = null;
    #endregion
    #region GameObject
    [SerializeField] GameObject backsideTooltipImageObject = null;
    [SerializeField] GameObject tooltip = null;
    [SerializeField] GameObject ddackBody = null;
    [SerializeField] GameObject specialBlockEffect = null;
    #endregion
    #region Image
    [SerializeField] Image backgroundImage = null;
    [SerializeField] Image backgroundImageWrapper = null;
    [SerializeField] Image specialBlockImage = null;
    #endregion
    int minNumber = 1;
    int maxNumber = 6;
    public int posX, posY;
    int blockSize = 46;
    int blocksLength;
    int destroyedDiceCount = 0;
    public bool isDestroyed = false;
    public bool isClickable = false;
    public string blocksType = "";
    public Text blockText;
    private static int slotOrder1 = 0;
    private static int slotOrder2 = 0;
    private static Vector3 firstBlockPosition;
    private static Vector3 lastBlockPosition;
    private static int relicsAnimationTurn = 0;
    private GameObject wizardAnimationImage;
    private GameObject armyAnimationImage;

    private void Start() 
    {
        wizardAnimationImage = GameObject.Find("Wizard Image");
        armyAnimationImage = GameObject.Find("Army Image");
        uiAnimator.backgroundImageAnimator.enabled = false;

        if (tooltip != null)
        {
            HideTooltip();
        }

        slotOrder1 = 0;
        slotOrder2 = 0;  
        // SetBlocksValue();
        Init();
        // GetComponent<>
    }

    public void Init()
    {
        posX = (int)transform.localPosition.x / blockSize;
        posY = (int)transform.localPosition.y / blockSize;
        // blockText = GetComponentInChildren<TMPro>();
    }

    public void UpdateBlocksUI()
    {
        if (blocksType == string.Empty)
        {
            uiImage.backgroundImage.enabled = false;
            uiImage.backgroundImage.color = new Color32(255, 255, 255, 0);
            specialBlockImage.enabled = false;
            specialBlockImage.color = new Color32(255, 255, 255, 0);
        }

        // Set First block clickable
        if (posX == 1 && posY == 1)
        {
            isClickable = true;
            uiImage.backgroundImage.enabled = true;
            uiImage.backgroundImage.overrideSprite = destroyableBlockImage;
            uiImage.backgroundImage.color = new Color32(255, 255, 255, 255);
            blockText.color = new Color32(32, 32, 32, 255);
            firstBlockPosition = transform.position;
        }

        // Set Last block text
        if (posX == BlockController.GetBoardSize() && posY == BlockController.GetBoardSize())
        {            
            specialBlockImage.color = new Color32(255, 255, 255, 0);
            uiImage.backgroundImage.overrideSprite = uiSprite.lastBlockNormal;
            uiImage.backgroundImage.color = new Color32(255, 255, 255, 255);
            uiImage.backgroundImage.transform.localPosition = new Vector3(1.6f, 2.79f, 1);
            uiTransform.backgroundImageRect.sizeDelta = new Vector2(68, 68);
            blockText.color = new Color32(255, 255, 255, 255);

            lastBlockPosition = transform.position;
            blockController.SetLastBlock(this);
            blocksType = "마왕성";
        }
    }

    public int GetTutorialBlocksValue(int posX, int poxY)
    {
        if (posX == 1 && posY == 1)
        {
            return 11;
        }

        if (posX == 2 && posY == 1)
        {
            return 5;
        }

        if (posX == 3 && posY == 1)
        {
            return 9;
        }

        if (posX == 1 && posY == 2)
        {
            return 11;
        }

        if (posX == 2 && posY == 2)
        {
            return 14;
        }

        if (posX == 3 && posY == 2)
        {
            return 16;
        }

        if (posX == 1 && posY == 3)
        {
            return 15;
        }

        if (posX == 2 && posY == 3)
        {
            return 18;
        }

        if (posX == 3 && posY == 3)
        {
            return 20;
        }

        return 1;
    }

    public void OnClickButton()
    {
        var diceController = FindObjectOfType<DiceController>();
        var attackGageDisplay = FindObjectOfType<AttackGageDisplay>();
        int clickedDiceCount = diceController.GetClickedDiceCount();
        
        // 블록을 공격하는 경우
        if (isClickable == true)
        {
            // 아이템 사용해서 블록을 공격하는 경우
            if(itemController && itemController.onClickedType.Length > 0 && blocksType != "마왕성")
            {
                // diceController.UnbounceDices();
                ReduceBlockGage(blockText.text, true);
            }
            // 아이템 사용 없이 블록을 공격하는 경우
            else if (clickedDiceCount > 0)
            {
                ReduceBlockGage(attackGageDisplay.GetAttackGage());
            }
            // 툴팁을 누르는 경우
            else
            {
                if (blocksType != string.Empty && blocksType != "마왕성")
                {
                    ToggleTooltip();
                }
            }
        }
        // 툴팁을 누르는 경우
        else
        {
            if (blocksType != string.Empty && blocksType != "마왕성")
            {
                ToggleTooltip();
            }
        }
    }

    public void SetTooltipInfo()
    {
        if (tooltip == null) return;

        Text tooltipText = tooltip.transform.GetChild(0).GetComponent<Text>();

        switch (blocksType)
        {
            case "광산":
                {
                    tooltipText.text = "광산: 주사위 굴리기 비용 1 감소";
                    tooltip.SetActive(true);
                    break;
                }
            case "용병":
                {
                    tooltipText.text = "용병: 원할 때 모든 주사위들의 눈을 1 더해주는 아이템 획득";
                    tooltip.SetActive(true);
                    break;
                }
            case "마법사":
                {
                    tooltipText.text = "마법사: 원할 때 모든 주사위들의 눈을 2배로 만들어주는 아이템 획득";
                    tooltip.SetActive(true);
                    break;
                }
            case "유물":
                {
                    tooltipText.text = "유물: 원할 때 10배의 눈을 가진 주사위 한 개를 굴려주는 아이템 획득";
                    tooltip.SetActive(true);
                    break;
                }
            case "던전":
                {
                    tooltipText.text = "던전: 주사위의 최대 눈 1 증가";
                    tooltip.SetActive(true);
                    break;
                }
            case "공습":
                {
                    tooltipText.text = "공습: 위아래 모든 땅의 방어력을 반으로 깎음";
                    tooltip.SetActive(true);
                }
                break;
            case "기병대":
                {
                    tooltipText.text = "기병대: 양옆 모든 땅의 방어력을 반으로 깎음";
                    tooltip.SetActive(true);
                    break;
                }
            case "폭탄":
                {
                    tooltipText.text = "폭탄: 주변 여덟 땅의 방어력을 반으로 깎음";
                    tooltip.SetActive(true);
                    break;
                }
        }
    }

    IEnumerator WaitForSecond()
    {
        yield return new WaitForSeconds(3);
    }

    public void ToggleTooltip()
    {
        bool isShownTooltip = tooltip.GetComponent<CanvasGroup>().blocksRaycasts;
        var diceController = FindObjectOfType<DiceController>();
        if (isShownTooltip)
        {
            CloseTooltip();
        }
        else
        {
            int clickedDiceCount = diceController.GetClickedDiceCount();
            if (!(clickedDiceCount > 0 && isClickable)) 
            {
                ShowTooltip();
            }
        }
    }

    public void ShowTooltip()
    {
        tooltip.transform.DOMoveY(tooltip.transform.position.y - 4, 0);
        tooltip.transform.DOMoveZ(1, 0);
        tooltip.GetComponent<CanvasGroup>().blocksRaycasts = true;
        tooltip.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        backsideTooltipImageObject.transform.DOLocalMoveX(tooltip.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x / 2 - 35, 0);
        tooltip.GetComponent<CanvasGroup>().DOFade(1, 0);
        tooltip.transform.DOMoveY(tooltip.transform.position.y, 0.1f);
    }

    public void HideTooltip()
    {
        if (tooltip == null) return;
        tooltip.GetComponent<CanvasGroup>().DOFade(0, 0);
        tooltip.GetComponent<CanvasGroup>().blocksRaycasts = false;
        tooltip.transform.DOMoveY(tooltip.transform.position.y + 8, 0);
    }

    public void CloseTooltip()
    {
        if (tooltip == null) return;
        tooltip.transform.DOMoveY(tooltip.transform.position.y - 8, 0.1f).OnComplete(() => {
            HideTooltip();
        });
    }

    public void ReduceBlockGage(string attackGage, bool isItemEffect = false, bool isBombed = false)
    {
        var diceController = FindObjectOfType<DiceController>();
        var resetDiceController = FindObjectOfType<ResetDiceController>();
        var speicalBlockController = FindObjectOfType<SpeicalBlockController>();

        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.TUTORIAL)
        {
            HandleTutorialTurn();
        }

        diceController.DestroyDices();
        // 블록의 남은 게이지
        int resultGage = int.Parse(blockText.text) - int.Parse(attackGage);
        if (resultGage <= 0)
        {
            ChangeDestroyedBlockDisplay(resultGage, isItemEffect, isBombed);

            if (!isItemEffect)
            {
                resetDiceController.AddMoneyAfterKill();
            }
            isClickable = false;
        }
        else
        {
            if (EffectSoundController.instance != null)
                EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.ATTACK_BLOCK);

            SetBlockValue(resultGage.ToString());
            blockController.HandleLastBlock(blocksType, resultGage);
        }

        
        if (!isItemEffect && blocksType != "마왕성")
        {
            speicalBlockController.IncreaseLastBlockGage();
            resetDiceController.IncreaseTurnCount();
        }
        
        Invoke("UpdateResetDiceButton", isItemEffect ? 0.7f : 0);

        destroyedDiceCount = 0;
    }

    private void UpdateResetDiceButton()
    {
        resetDiceController.ToggleResetDiceButton();
        // 남은 주사위 개수가 0이고 돈이 없으면 No Dice Screen 띄우기
        noDiceNoCoinController.ToggleScreen();
    } 

    private void HandleLastBlock(int resultGage)
    {
        blockController.HandleLastBlock(blocksType, resultGage);
    }

    private void HandleTutorialTurn()
    {
        var tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
        var dialogueTurn = TutorialDialogueController.dialogueTurn;

        if (dialogueTurn == 7 || 
            dialogueTurn == 11 || 
            dialogueTurn == 15)
        {
            if (dialogueTurn == 15)
            {
                var arrow = GameObject.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.MINT_ARROW);
                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(0.6f);
                sequence.AppendCallback(() => {
                    tutorialDialogueController.Apply();
                });

                arrow.GetComponent<CanvasGroup>().DOFade(0, 0.15f);
            }
            else
            {
                tutorialDialogueController.Apply();
            }
        }        
    }

    private void ChangeDestroyedBlockDisplay(int resultGage, bool isItemEffect = false, bool isBombed = false)
    {
        isDestroyed = true;

        // todo ddack!
        if (resultGage == 0)
        {
            string onClickedItemType = itemController.onClickedType;

            if (itemController && onClickedItemType.Length > 0)
            {
                GetItemReward();
                Invoke("GetSpecialBlockReward", 0.4f);
            }

            if (EffectSoundController.instance != null)
                EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.GET_LAND_PERFECT);

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(isItemEffect ? 0.3f : 0);
            sequence.AppendCallback(() => {
                uiImage.backgroundImage.color = new Color32(255, 255 , 255, 255);
                if (blocksType != "마왕성") {
                    uiImage.backgroundImage.enabled = true;
                    uiImage.backgroundImage.overrideSprite = clearLandOccupiedImage;
                }
                SetBlockValue(string.Empty);
                MakeNextBlockClickable();

                if (isBombed)
                {
                    onClickedItemType = ItemController.TYPE.EXPLOSIVE_WAREHOUSE;
                }

                if (blocksType != "마왕성")
                {
                    SetDdackEffectAnimation(onClickedItemType);
                }

                if (!isItemEffect)
                {
                    GetSpecialBlockReward();
                    resetDiceController.ResetOneDice();
                }
                statisticsController.UpdateFactor01();
            });
            sequence.Play();
        }
        else
        {
            if (EffectSoundController.instance != null)
                EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.GET_LAND);

            uiImage.backgroundImage.overrideSprite = clearLandImage;
            SetBlockValue(string.Empty);
            MakeNextBlockClickable();
            GetSpecialBlockReward();
            statisticsController.UpdateFactor02();
        }
    }

    public void SetBlockValue(string targetValue)
    {
        blockText.text = targetValue;
    }

    public void SetDdackEffectAnimation(string onClickedItemType)
    {
        int randomNumber = UnityEngine.Random.Range(0, 8);

        if (onClickedItemType == ItemController.TYPE.GOLD_MINE)
        {
            randomNumber = 9;
        }
        else if (onClickedItemType == ItemController.TYPE.EXPLOSIVE_WAREHOUSE)
        {
            randomNumber = 10;
        }

        ddackBody.GetComponentsInChildren<Image>()[randomNumber].enabled = true;
        ddackBody.GetComponentsInChildren<Image>()[randomNumber].color = new Color32(255, 255 , 255, 255);

        var ddack = GameObject.Find("Ddack");
        ddack.GetComponent<Animator>().SetTrigger("isAnimated");
        Invoke("SetDemEffectAnimation", 0.3f);        
    }

    public void SetDemEffectAnimation()
    {
        var dem = GameObject.Find("Dem");
        dem.GetComponent<Animator>().SetTrigger("isAnimated");
        Invoke("ResetDdackDemEffectAnimation", 0.3f);
    }

    public void ResetDdackDemEffectAnimation()
    {
        var ddack = GameObject.Find("Ddack");
        ddack.GetComponent<Animator>().ResetTrigger("isAnimated");        
        var dem = GameObject.Find("Dem");
        dem.GetComponent<Animator>().ResetTrigger("isAnimated");
    }

    private void GetSpecialBlockReward()
    {
        var powerUpController = FindObjectOfType<PowerUpController>();
        Dice[] dices = FindObjectsOfType<Dice>();
        Block[] blocks = FindObjectsOfType<Block>();

        switch (blocksType)
        {
            case "광산":
                if (EffectSoundController.instance != null)
                    EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.REWARD_MINE);
                int currentCost = resetDiceController.GetCost();
                AnimateMine(currentCost);
                if (currentCost == 1)
                {
                    // 주사위 굴리는 비용이 1이면 더 삭감하지 않기
                    resetDiceController.SetCost(1);
                }
                else
                {
                    resetDiceController.SetCost(--currentCost);
                }
                break;
            case "용병":
                {
                    Transform selectedSlot = GetSelectedSlot();
                    Button button = selectedSlot.transform.GetChild(1).GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => IncreaseDiceNumberBySpeicalBlock(1, selectedSlot));
                    button.enabled = true;

                    Image image = selectedSlot.transform.GetChild(1).GetComponent<Image>();
                    image.sprite = armySlotImage;

                    var canvasGroup = selectedSlot.transform.GetChild(1).GetComponent<CanvasGroup>();
                    canvasGroup.DOFade(1, 0);
                    break;
                }
            case "마법사":
                {
                    Transform selectedSlot = GetSelectedSlot();

                    Button button = selectedSlot.transform.GetChild(1).GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => IncreaseDiceNumberBySpeicalBlock(2, selectedSlot));
                    button.enabled = true;

                    Image image = selectedSlot.transform.GetChild(1).GetComponent<Image>();
                    image.sprite = wizardSlotImage;

                    var canvasGroup = selectedSlot.transform.GetChild(1).GetComponent<CanvasGroup>();
                    canvasGroup.DOFade(1, 0);
                    break;
                }
            case "유물":
                {
                    Transform selectedSlot = GetSelectedSlot();

                    Button button = selectedSlot.transform.GetChild(1).GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => MultiplyDiceNumberBySpeicalBlock(selectedSlot));
                    button.enabled = true;

                    Image image = selectedSlot.transform.GetChild(1).GetComponent<Image>();
                    image.sprite = relicsSlotImage;

                    var canvasGroup = selectedSlot.transform.GetChild(1).GetComponent<CanvasGroup>();
                    canvasGroup.DOFade(1, 0);

                    break;
                }
            case "던전":
                {
                    if (EffectSoundController.instance != null)
                        EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.REWARD_DUNGEON);

                    AnimateDungeon();

                    resetDiceController.SetAttackPower(dices[0].GetMaxNumber());

                    foreach (Dice dice in dices)
                    {
                        dice.AddMaxNumber();
                    }
                    break;
                }
            case "기병대":
                if (EffectSoundController.instance != null)
                    EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.REWARD_HORSE);
                foreach (Block block in blocks)
                {
                    if (!block.isDestroyed)
                    {
                        if (block.GetPosY() == posY)
                        {
                            string targetValue = Mathf.Ceil(float.Parse(block.GetBlockValue()) / 2f).ToString();
                            Sequence sequence = DOTween.Sequence();
                            sequence.AppendInterval(0.3f);
                            sequence.AppendCallback(() => block.SetBlockValue(targetValue));
                            sequence.Play();
                            
                            StartCoroutine(block._controller.cameraShaker.ShakeBlock(0.2f, 4f));
                        }
                    }
                }
                ShowEffectHorizontalBlock();
                break;
            case "공습":
                if (EffectSoundController.instance != null)
                    EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.REWARD_HORSE);

                foreach (Block block in blocks)
                {
                    if (!block.isDestroyed)
                    {
                        if (block.GetPosX() == posX)
                        {
                            string targetValue = Mathf.Ceil(float.Parse(block.GetBlockValue()) / 2f).ToString();
                            Sequence sequence = DOTween.Sequence();
                            sequence.AppendInterval(0.3f);
                            sequence.AppendCallback(() => block.SetBlockValue(targetValue));
                            sequence.Play();

                            StartCoroutine(block._controller.cameraShaker.ShakeBlock(0.2f, 4f));
                        }
                    }
                }
                ShowEffectVerticalBlock();
                break;
            case "폭탄":
                if (EffectSoundController.instance != null)
                    EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.REWARD_HORSE);
                foreach (Block block in blocks)
                {
                    if (!block.isDestroyed)
                    {
                        if (
                            (block.GetPosX() - 1 == posX && block.GetPosY() == posY) ||
                            (block.GetPosX() - 1 == posX && block.GetPosY() + 1 == posY) ||
                            (block.GetPosX() - 1 == posX && block.GetPosY() - 1 == posY) ||
                            (block.GetPosX() == posX && block.GetPosY() + 1 == posY) ||
                            (block.GetPosX() == posX && block.GetPosY() - 1 == posY) ||
                            (block.GetPosX() + 1 == posX && block.GetPosY() == posY) ||
                            (block.GetPosX() + 1 == posX && block.GetPosY() + 1 == posY) ||
                            (block.GetPosX() + 1 == posX && block.GetPosY() - 1 == posY)
                        )
                        {
                            string targetValue = Mathf.Ceil(float.Parse(block.GetBlockValue()) / 2f).ToString();
                            Sequence sequence = DOTween.Sequence();
                            sequence.AppendInterval(0.3f);
                            sequence.AppendCallback(() => block.SetBlockValue(targetValue));
                            sequence.Play();
                            StartCoroutine(block._controller.cameraShaker.ShakeBlock(0.2f, 4f));
                        }
                    }
                }

                ShowEffectBombBlock();

                break;
        }
    }

    public void GetItemReward()
    {
        itemController.GetItemReward(this);
    }

    public void AnimateDungeon()
    {
        // ResetDdackDemEffectAnimation();
        var ddack = GameObject.Find("Ddack");
        var dungeonImage = GameObject.Find("Dungeon Image");
        var backAttackPowerText = GameObject.Find("Back Attack Power Text");

        Sequence dungeonSequence = DOTween.Sequence();
        if (ddack.GetComponent<Animator>().GetBool("isAnimated"))
        {
            dungeonSequence.AppendInterval(0.7f);
        }
        dungeonSequence.Append(dungeonImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
        dungeonSequence.Join(dungeonImage.transform.DOScale(0.4f, 0.1f));
        dungeonSequence.Append(backAttackPowerText.transform.DOLocalMoveY(-30, 0.1f));
        dungeonSequence.Join(backAttackPowerText.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
        dungeonSequence.Append(backAttackPowerText.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
        dungeonSequence.Join(backAttackPowerText.transform.DOLocalMoveY(46, 0).OnComplete(() => {
            backAttackPowerText.GetComponent<Text>().text = (int.Parse(backAttackPowerText.GetComponent<Text>().text) + 1).ToString();
        }));
        dungeonSequence.Append(backAttackPowerText.transform.DOLocalMoveY(-5, 0.1f));
        dungeonSequence.AppendInterval(0.25f);
        dungeonSequence.Append(dungeonImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
        dungeonSequence.Play();
    }

    public void AnimateMine(int currentCost)
    {
        // ResetDdackDemEffectAnimation();
        var ddack = GameObject.Find("Ddack");
        var mineImage = GameObject.Find("Mine Image");
        var goldText = GameObject.Find("Gold Text");
        goldText.GetComponent<Text>().text = currentCost.ToString();
        Sequence mineSequence = DOTween.Sequence();
        if (ddack.GetComponent<Animator>().GetBool("isAnimated"))
        {
            mineSequence.AppendInterval(0.7f);
        }        
        mineSequence.Append(mineImage.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
        mineSequence.Join(mineImage.transform.DOScale(0.4f, 0.1f));
        mineSequence.Append(goldText.transform.DOLocalMoveY(-30, 0.1f));
        mineSequence.Join(goldText.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
        mineSequence.Append(goldText.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
        mineSequence.Join(goldText.transform.DOLocalMoveY(46, 0).OnComplete(() => {
            goldText.GetComponent<Text>().text = (currentCost > 1 ? currentCost - 1 : 1).ToString();
        }));
        mineSequence.Append(goldText.transform.DOLocalMoveY(-5, 0.1f));
        mineSequence.AppendInterval(0.25f);
        mineSequence.Append(mineImage.GetComponent<CanvasGroup>().DOFade(0, 0.1f));
        mineSequence.Play();
    }

    public void ShowEffectHorizontalBlock()
    {
        specialBlockEffect.GetComponent<CanvasGroup>().DOFade(0, 0);
        specialBlockEffect.transform.DOMoveX(firstBlockPosition.x + 100, 0);
        specialBlockEffect.GetComponentsInChildren<Image>()[0].enabled = true;
        specialBlockEffect.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        specialBlockEffect.transform.DOMoveX(lastBlockPosition.x - 80, 1f).OnComplete(() => {
            specialBlockEffect.GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() => {
                specialBlockEffect.transform.DOMoveX(firstBlockPosition.x + 100, 0);
                specialBlockEffect.GetComponentsInChildren<Image>()[0].enabled = false;
            });
        });
    }

    public void ShowEffectVerticalBlock()
    {
        specialBlockEffect.GetComponent<CanvasGroup>().DOFade(0, 0);
        specialBlockEffect.transform.DOMoveY(firstBlockPosition.y, 0);
        specialBlockEffect.GetComponentsInChildren<Image>()[1].enabled = true;
        specialBlockEffect.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        specialBlockEffect.transform.DOMoveY(lastBlockPosition.y - 180, 1f).OnComplete(() => {
            specialBlockEffect.GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() => {
                specialBlockEffect.transform.DOMoveY(firstBlockPosition.y, 0);
                specialBlockEffect.GetComponentsInChildren<Image>()[1].enabled = false;
            });
        });
    }

    public void ShowEffectBombBlock()
    {
        specialBlockEffect.GetComponent<CanvasGroup>().DOFade(0, 0);
        specialBlockEffect.GetComponentsInChildren<Image>()[2].enabled = true;
        specialBlockEffect.GetComponentsInChildren<Image>()[2].transform.DOScale(new Vector3(0.15f, 0.15f, 0.15f), 0);
        specialBlockEffect.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        specialBlockEffect.GetComponentsInChildren<Image>()[2].transform.DOScale(new Vector3(0.27f, 0.27f, 0.27f), 0.3f).OnComplete(() => {
            specialBlockEffect.GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() => {
                specialBlockEffect.GetComponentsInChildren<Image>()[2].enabled = false;
            });
        });
    }

    private void AddNewOneDice()
    {
        int diceCount = 0;

        var dices = FindObjectsOfType<Dice>();
        foreach (Dice dice in dices)
        {
            if (diceCount == 0)
            {
                if (dice.IsDestroyed())
                {
                    dice.ResetDice();
                    diceCount++;
                }
            }
        }
    }

    private void MakeNextBlockClickable()
    {
        var blocks = FindObjectsOfType<Block>();

        if (posX == (int)Mathf.Sqrt(blocks.Length) && posY == (int)Mathf.Sqrt(blocks.Length))
        {
            uiImage.backgroundImage.transform.localPosition = new Vector3(0, 0, 1);
            FindObjectOfType<LevelController>().WinLastBlock();
        }
        else
        {
            foreach (Block block in blocks)
            {
                if (!block.isDestroyed)
                {
                    if ((block.GetPosX() == posX && block.GetPosY() == posY + 1) ||
                    (block.GetPosX() == posX + 1 && block.GetPosY() == posY) ||
                    (block.GetPosX() == posX - 1 && block.GetPosY() == posY) ||
                    (block.GetPosX() == posX && block.GetPosY() == posY - 1))
                    {
                        // block.GetComponent<BoxCollider2D>().enabled = true;
                        block.GetComponentInChildren<Button>().interactable = true;
                        block.isClickable = true;
                        // 마왕성을 제외하고 나머지만 이미지 변경
                        if (blocksType != "마왕성")
                        {
                            block.uiImage.backgroundImage.color = new Color32(255, 255, 255, 255);
                            block.uiImage.backgroundImage.overrideSprite = destroyableBlockImage;
                            if (block.blocksType == string.Empty) 
                            {
                                block.GetComponentInChildren<Text>().color = new Color32(32, 32, 32, 255);
                                block.uiImage.backgroundImage.enabled = true;
                            }                            
                        }
                        else
                        {
                            block.uiImage.backgroundImage.color = new Color32(255, 255, 255, 255);
                            block.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
                            block.transform.Find("Last Block Oval").gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        blockController.HandleLastBlock(blocksType, 0);
    }

    public Transform GetSelectedSlot()
    {
        var itemSlot = GameObject.Find("Item Slot");
        var slot1 = itemSlot.transform.GetChild(0);
        var slot2 = itemSlot.transform.GetChild(1);
        float isOnSlot1 = slot1.transform.GetChild(1).GetComponent<CanvasGroup>().alpha;

        if (isOnSlot1 == 0)
        {
            slot1.transform.GetChild(1).GetComponent<Button>().enabled = true;
            slot1.transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(1, 0);
        }
        else
        {
            slot2.transform.GetChild(1).GetComponent<Button>().enabled = true;
            slot2.transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(1, 0);
        }

        Transform selectedSlot = slotOrder1 > slotOrder2 ? slot2 : slot1;

        if (slotOrder1 > slotOrder2)
        {
            slotOrder2 = slotOrder2 + 1;
        }
        else
        {
            slotOrder1 = slotOrder1 + 1;
        }

        return selectedSlot;
    }

    public void ResetSelectedSlot(Transform slot)
    {
        slot.transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0, 0);
        slot.transform.GetChild(1).GetComponent<Button>().enabled = false;

        if (slot.gameObject.name == "Slot 1")
        {
            slotOrder1 = 0;
            slotOrder2 = 0;
        }
        else
        {
            slotOrder1 = 1;
            slotOrder2 = 0;
            var itemSlot = GameObject.Find("Item Slot");
            var slot1 = itemSlot.transform.GetChild(0);
            float isOnSlot1 = slot1.transform.GetChild(1).GetComponent<CanvasGroup>().alpha;

            if (isOnSlot1 == 0)
            {
                slotOrder1 = 0;
            }
        }
    }

    public int GetPosX()
    {
        return posX;
    }

    public int GetPosY()
    {
        return posY;
    }

    public void SetBlockType(string type)
    {
        if(blocksType == "마왕성") return;

        blocksType = type;
        specialBlockImage.color = new Color32(255, 255, 255, 255);
        uiImage.backgroundImage.enabled = true;
        uiImage.backgroundImage.color = new Color32(255, 255, 255, 255);

        switch (blocksType)
        {
            case "광산":
                specialBlockImage.sprite = mineImage;
                uiImage.backgroundImage.overrideSprite = mineBackgroundImage;
                GetComponentsInChildren<Text>()[0].color = new Color32(191, 155, 48, 255);
                blockText.text = (int.Parse(blockText.text) + 6).ToString();
                break;
            case "던전":
                specialBlockImage.sprite = dungeonImage;
                uiImage.backgroundImage.overrideSprite = dungeonBackgroundImage;
                GetComponentsInChildren<Text>()[0].color = new Color32(231, 134, 134, 255);
                blockText.text = (int.Parse(blockText.text) + 4).ToString();
                break;
            case "용병":
                specialBlockImage.sprite = armyImage;
                uiImage.backgroundImage.overrideSprite = armyBackgroundImage;
                GetComponentsInChildren<Text>()[0].color = new Color32(113, 110, 110, 255);
                blockText.text = (int.Parse(blockText.text) + 4).ToString();
                break;
            case "마법사":
                specialBlockImage.sprite = wizardImage;
                uiImage.backgroundImage.overrideSprite = wizardBackgroundImage;
                GetComponentsInChildren<Text>()[0].color = new Color32(146, 100, 172, 255);
                blockText.text = (int.Parse(blockText.text) + 6).ToString();
                break;
            case "유물":
                specialBlockImage.sprite = relicsImage;
                uiImage.backgroundImage.overrideSprite = relicsBackgroundImage;
                GetComponentsInChildren<Text>()[0].color = new Color32(82, 119, 132, 255);
                blockText.text = (int.Parse(blockText.text) + 3).ToString();
                break;
            case "기병대":
                specialBlockImage.sprite = horizontalImage;
                uiImage.backgroundImage.overrideSprite = horizontalBackgroundImage;
                GetComponentsInChildren<Text>()[0].color = new Color32(128, 120, 168, 255);
                blockText.text = (int.Parse(blockText.text) + 6).ToString();
                break;
            case "공습":
                specialBlockImage.sprite = verticalImage;
                uiImage.backgroundImage.overrideSprite = verticalBackgroundImage;
                GetComponentsInChildren<Text>()[0].color = new Color32(128, 120, 168, 255);
                blockText.text = (int.Parse(blockText.text) + 6).ToString();
                break;
            case "폭탄":
                specialBlockImage.sprite = bombImage;
                uiImage.backgroundImage.overrideSprite = bombBackgroundImage;
                GetComponentsInChildren<Text>()[0].color = new Color32(128, 120, 168, 255);
                blockText.text = (int.Parse(blockText.text) + 5).ToString();
                break;
        }
    }

    public string GetBlockType()
    {
        return blocksType;
    }

    public string GetBlockValue()
    {
        return blockText.text;
    }

    public void IncreaseDiceNumberBySpeicalBlock(int powerUpGage, Transform slot)
    {
        var diceController = FindObjectOfType<DiceController>();
        int destroyedDiceCount = diceController.GetDestroyedDiceCount();
                
        if (destroyedDiceCount < 6)
        {
            var speicalBlockController = FindObjectOfType<SpeicalBlockController>();
            speicalBlockController.IncreaseDiceNumber(powerUpGage);
            var attackGageDisplay = FindObjectOfType<AttackGageDisplay>();
            attackGageDisplay.SumAttackGage();

            if (powerUpGage == 1)
            {
                if (EffectSoundController.instance != null)
                    EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.USE_ARMY);

                SetArmyAnimation();                    
            }
            else
            {
                if (EffectSoundController.instance != null)
                    EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.USE_WIZARD_ITEM);

                SetWizardAnimation();
            }

            ResetSelectedSlot(slot);
        }
    }

    public void MultiplyDiceNumberBySpeicalBlock(Transform slot)
    {
        var diceController = FindObjectOfType<DiceController>();
        int destroyedDiceCount = diceController.GetDestroyedDiceCount();

        if (destroyedDiceCount > 0)
        {
            if (EffectSoundController.instance != null)
                EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.USE_RELICS_ITEM);

            var attackGageDisplay = FindObjectOfType<AttackGageDisplay>();
            attackGageDisplay.SumAttackGage();

            AnimateRelics();

            resetDiceController.ResetOneDice(10);
            ResetSelectedSlot(slot);
        }
    }

    public void AnimateRelics()
    {
        var relicsImage = GameObject.Find("Relics Image");

        if (relicsImage != null) 
        {
            GameObject clonedRelicsImage = Instantiate(relicsImage, transform.position, transform.rotation);
            clonedRelicsImage.transform.SetParent(GameObject.Find("Right Area").transform, false);
            clonedRelicsImage.transform.localPosition = relicsImage.transform.localPosition;
            relicsImage.name = "Relics Image Updated";

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(relicsImage.GetComponent<CanvasGroup>().DOFade(1, 0));
            mySequence.Join(relicsImage.GetComponent<Rigidbody2D>().DORotate(360, 2.5f));
            mySequence.Join(relicsImage.transform.DOLocalMoveX(420f, 2.5f));
            mySequence.Append(relicsImage.GetComponent<CanvasGroup>().DOFade(0, 0));
            mySequence.Play().OnComplete(() => {
                DestroyObject(relicsImage);
                clonedRelicsImage.name = "Relics Image";
            });
        }
    }

    private void SetArmyAnimation()
    {
        armyAnimationImage.GetComponent<CanvasGroup>().DOFade(1, 0f);
        armyAnimationImage.GetComponent<Animator>().SetTrigger("isAnimated");
        Invoke("ResetArmyAnimation", 1.31f);
    }

    private void ResetArmyAnimation()
    {
        armyAnimationImage.GetComponent<CanvasGroup>().DOFade(0, 0f);
        armyAnimationImage.GetComponent<Animator>().ResetTrigger("isAnimated");
    }
 
    private void SetWizardAnimation()
    {
        wizardAnimationImage.GetComponent<CanvasGroup>().DOFade(1, 0f);
        wizardAnimationImage.GetComponent<Animator>().SetTrigger("isAnimated");
        Invoke("ResetWizardAnimation", 0.9f);
    }

    private void ResetWizardAnimation()
    {
        wizardAnimationImage.GetComponent<CanvasGroup>().DOFade(0, 0f);
        wizardAnimationImage.GetComponent<Animator>().ResetTrigger("isAnimated");
    }

    public void ToggleAllowClick(bool isAllow)
    {
        var button = this.transform.Find(Constants.TUTORIAL.GAME_OBJECT_NAME.BACKGROUND_IMAGE).GetComponent<Button>();
        button.interactable = isAllow;
    }    
}
