using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDiceNoCoinController : ControllerManager
{
    void Start()
    {
        HideScreen();
    }

    void Init()
    {
        HideScreen();        
    }

    public void ToggleScreen()
    {
        var dices = FindObjectsOfType<Dice>();
        var resetDiceController = FindObjectOfType<ResetDiceController>();
        int destroyedDiceCount = 0;

        foreach (Dice dice in dices)
        {
            if (dice.IsDestroyed())
            {
                destroyedDiceCount++;
            }
        }

        Debug.Log(resetDiceController.GetCurrentMoney());
        Debug.Log(resetDiceController.GetCost());

        if (destroyedDiceCount == 6 && resetDiceController.GetCurrentMoney() < resetDiceController.GetCost())
        {
            ShowScreen();
        } else 
        {
            HideScreen();
        }
    }

    public void ShowScreen()
    {
        this.gameObject.SetActive(true);
        // this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        // this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void HideScreen()
    {
        this.gameObject.SetActive(false);
        // this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        // this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
