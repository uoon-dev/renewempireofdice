using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Serializable]
// public class PopupObjects 
// {
//     public GameObject noDiamondPopup;
// }


public class PopupController : MonoBehaviour
{
    // public PopupObjects popupObjects;
    public static Transform noDiamondPopup;
    public static int heartShopSiblingIndex;
    public static int diamondShopSiblingIndex;

    void Start()
    {
        noDiamondPopup = transform.Find("No Diamond Popup");
        // heartShopSiblingIndex = 
    }
    
    public void ToggleNoDiamondPopup(bool isActive)
    {
        if (noDiamondPopup == null) return;
        noDiamondPopup.gameObject.SetActive(isActive);
    }

    public void CloseOtherPopups()
    {
    }
}
