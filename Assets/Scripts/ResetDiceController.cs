using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RedBlueGames.Tools.TextTyper;
using Controllers.TutorialController;

public class ResetDiceController : MonoBehaviour
{
    [SerializeField] GameObject diceBox = null;
    [SerializeField] GameObject turnText = null;
    [SerializeField] Sprite disabledResetDiceButtonImage = null;
    [SerializeField] Sprite abledResetDiceButtonImage = null;

    Text[] money; 
    GameObject moneyArea;
    Image moneyIconImage;
    Text moneyText;
    Text costText;
    Text attackPowerText;
    int cost = 5;
    int attackPower = 6;
    LevelLoader levelLoader;
    TutorialDialogueController tutorialDialogueController;

    void Start()
    {   
        Initialize();

        if (levelLoader.GetCurrentLevelNumber() <= 5) {
            costText.text = "25";
        } else if (levelLoader.GetCurrentLevelNumber() <= 19) {
            costText.text = "20";
        } else {
            costText.text = "15";
        }

        moneyText.text = cost.ToString();
        attackPowerText.text = "1-6";


        if (levelLoader.GetCurrentSceneName() == "Level")
            DisableResetDiceButton();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        // tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
        // Debug.Log(tutorialDialogueController);
        money = GetComponentsInChildren<Text>();
        moneyArea = GameObject.Find(Constants.GAME_OBJECT_NAME.STAGE.MONEY_AREA);
        moneyText = GameObject.Find(Constants.GAME_OBJECT_NAME.STAGE.MONEY_TEXT).GetComponent<Text>();
        moneyIconImage = GameObject.Find(Constants.GAME_OBJECT_NAME.STAGE.MONEY_ICON).GetComponent<Image>();
        costText = GameObject.Find(Constants.GAME_OBJECT_NAME.STAGE.COST_TEXT).GetComponent<Text>();
        attackPowerText = GameObject.Find(Constants.GAME_OBJECT_NAME.STAGE.ATTACK_POWER_TEXT).GetComponent<Text>();
    }

    public void AddMoneyAfterKill()
    {
        costText.text = (int.Parse(costText.text) + 1).ToString();
    }

    public void OnClickButton() {
        int currentMoney = int.Parse(costText.text);
        if (currentMoney >= cost)
        {
            if (EffectSoundController.instance != null)
                EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.GET_NEW_DICE);
            costText.text = (int.Parse(costText.text) - cost).ToString();
            ResetDices();

            if (TutorialDialogueController.dialogueTurn == 19)
            {
                tutorialDialogueController = FindObjectOfType<TutorialDialogueController>();
                tutorialDialogueController.Apply();
            }
        }        
    }

    public void ResetDices()
    {
        if (TutorialDialogueController.dialogueTurn == 19) return;

        var dices = FindObjectsOfType<Dice>();
        int destroyedDiceCount = 0;
        foreach (Dice dice in dices)
        {
            if (dice.IsDestroyed() == true) 
            {
                dice.ResetDice();
                destroyedDiceCount++;
            }
        }

        bool wasReset = destroyedDiceCount > 0 ? true : false;
        if (wasReset) {
            IncreaseTurnCount();
        }

        ToggleResetDiceButton();    

        FindObjectOfType<SpeicalBlockController>().IncreaseLastBlockGage();
        FindObjectOfType<AttackGageDisplay>().SumAttackGage();
    }

    public void IncreaseTurnCount() 
    {
        int turnCount = int.Parse(turnText.GetComponent<Text>().text.Split('턴')[0]);
        turnText.GetComponent<Text>().text = $"{(turnCount + 1).ToString()}턴째";
        FindObjectOfType<StatisticsController>().UpdateFactor03();
    }

    public int GetTurnCount()
    {
        return int.Parse(turnText.GetComponent<Text>().text.Split('턴')[0]);
    }

    private static void SetSpeicalDice(Dice[] dices)
    {
        if (StartController.GetCurrentDiceType() == Constants.MaldivesDice)
        {
            dices[Random.Range(0, 6)].EffectMaldivesDice();
        }
    }

    // 딱댐 -> 주사위 1개 보너스
    public void ResetOneDice()
    {
        if (TutorialDialogueController.dialogueTurn == 15) return;

        var dices = FindObjectsOfType<Dice>();
        foreach (Dice dice in dices)
        {
            if (TutorialDialogueController.dialogueTurn == 17)
            {
                if (dice.name == "Dice (6)")
                {
                    dice.ResetDice();
                    ToggleResetDiceButton();
                    return;
                }
            }
            else
            {
                if (dice.IsDestroyed())
                {
                    dice.ResetDice();
                    ToggleResetDiceButton();                    
                    return;
                }
            } 
        }
    }

    // 주사위 추가될 때 배수 이상으로 숫자가 세팅되게 만들기
    public void ResetOneDice(int timesNumber)
    {
        var dices = FindObjectsOfType<Dice>();
        foreach (Dice dice in dices)
        {
            if (dice.IsDestroyed())
            {
                dice.ResetDice(timesNumber);
                ToggleResetDiceButton();
                return;
            }
        }    
    }

    public int GetDestroyedDiceCount()
    {
        int destroyedDiceCount = 0;
        var dices = FindObjectsOfType<Dice>();
        foreach (Dice dice in dices)
        {
            if (dice.IsDestroyed())
            {
                destroyedDiceCount++;
            }
        }

        return destroyedDiceCount;
    }

    public void ToggleResetDiceButton()
    {
        if (GetDestroyedDiceCount() > 0 && GetCurrentMoney() >= cost)
        {
            AbleResetDiceButton();
        } 
        else 
        {
            DisableResetDiceButton();
        }
    }

    public void SetCost(int newCost)
    {
        cost = newCost;
        moneyText.text = cost.ToString();
    }

    public int GetCost()
    {
        return cost;
    }

    public void SpendCurrentMoney(int spendedCost)
    {
        int currentMoney = int.Parse(costText.text);
        if (currentMoney >= spendedCost)
        {
            costText.text = (int.Parse(costText.text) - spendedCost).ToString();
        }
    }

    public int GetCurrentMoney()
    {
        return int.Parse(costText.text);
    }

    public void SetAttackPower(int power)
    {
        attackPower = power;
        attackPowerText.text = "1-" + attackPower.ToString();
    }

    public void DisableResetDiceButton()
    {
        if (moneyArea.GetComponent<Button>().enabled == true)
        {
            moneyArea.GetComponent<Button>().enabled = false;
            moneyArea.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            moneyIconImage.color = new Color32(255, 255, 255, 100);
            moneyText.color = new Color32(82, 77, 74, 80);
        }
    }
    public void AbleResetDiceButton()
    {
        if (moneyArea.GetComponent<Button>().enabled == false) 
        {
            moneyArea.GetComponent<Button>().enabled = true;
            moneyArea.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            moneyIconImage.color = new Color32(255, 255, 255, 255);
            moneyText.color = new Color32(0, 0, 0, 255);
        }
    }
} 
