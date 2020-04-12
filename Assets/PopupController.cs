using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PopupObjects 
{
    public GameObject noDiamondPopup;
}


public class PopupController : MonoBehaviour
{
    public PopupObjects popupObjects;
    DiamondShopController diamondShopController;
    HeartShopController heartShopController;

    void Start()
    {
        diamondShopController = FindObjectOfType<DiamondShopController>();
        heartShopController = FindObjectOfType<HeartShopController>();
    }
    
    public void ToggleNoDiamindPopup(bool isActive)
    {
        popupObjects.noDiamondPopup.SetActive(isActive);
    }

    public void CloseOtherPopups()
    {
        diamondShopController.ToggleDiamondShopCanvas(false);
        heartShopController.ToggleHeartShopCanvas(false);
    }
}
