using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField] GameObject itemGuideCanvas;
    [SerializeField] Image guideCharacter;

    [SerializeField] Sprite goldMineIllust;
    [SerializeField] Sprite explosiveWarehouseIllust;
    BlockController blockController;
    public static class TYPE {
        public const string GOLD_MINE = "GOLD_MINE";
        public const string EXPLOSIVE_WAREHOUSE = "EXPLOSIVE_WAREHOUSE";
    };


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        blockController = FindObjectOfType<BlockController>();
    }

    public void OnClickedGoldMine()
    {
        guideCharacter.sprite = goldMineIllust;
        itemGuideCanvas.SetActive(true);
        blockController.ToggleBounceClickableBlocks(true, TYPE.GOLD_MINE); 
    }

    public void OnClickedExplosiveWarehouse()
    {
        guideCharacter.sprite = explosiveWarehouseIllust;
        itemGuideCanvas.SetActive(true);
        blockController.ToggleBounceClickableBlocks(true, TYPE.EXPLOSIVE_WAREHOUSE);
    }

    public void CloseItemGuide()
    {
        itemGuideCanvas.SetActive(false);
        blockController.ToggleBounceClickableBlocks(false, null); 
    }
}
