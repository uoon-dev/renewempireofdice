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
    [SerializeField] SuperTextMesh guideText;
    BlockController blockController;
    ResetDiceController resetDiceController;
    public string onClickedType;
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
        resetDiceController = FindObjectOfType<ResetDiceController>();
    }

    public void OnClickedGoldMine()
    {
        guideCharacter.sprite = goldMineIllust;
        itemGuideCanvas.SetActive(true);
        guideText.text = "<b>황금 광산으로 만들 땅을 고르세요!</b><br><br><s=0.7><c=dimgray>딱뎀으로 땅을 즉시 점령하고 표시된 방어력 만큼 골드를 얻습니다.</c></s>";
        guideText.gameObject.SetActive(false);
        guideText.gameObject.SetActive(true);
        blockController.ToggleBounceClickableBlocks(true, TYPE.GOLD_MINE);
        onClickedType = TYPE.GOLD_MINE;
    }

    public void OnClickedExplosiveWarehouse()
    {
        guideCharacter.sprite = explosiveWarehouseIllust;
        itemGuideCanvas.SetActive(true);
        guideText.text = "<b>화약고를 터트릴 땅을 고르세요!</b><br><br><s=0.7><c=dimgray>선택된 땅에 사방으로 폭격기를 보내 모두 딱뎀으로 점령합니다. (단,마왕성은 점령되지 않습니다)</c></s>";
        guideText.gameObject.SetActive(false);
        guideText.gameObject.SetActive(true);        
        blockController.ToggleBounceClickableBlocks(true, TYPE.EXPLOSIVE_WAREHOUSE);
        onClickedType = TYPE.EXPLOSIVE_WAREHOUSE;
    }

    public void CloseItemGuide()
    {
        itemGuideCanvas.SetActive(false);
        blockController.ToggleBounceClickableBlocks(false, null); 
        onClickedType = string.Empty;
    }

    public void GetItemReward(Block targetBlock)
    {
        switch(onClickedType)
        {
            case TYPE.GOLD_MINE: 
            {
                CloseItemGuide();
                int targetAmount = int.Parse(targetBlock.blockText.text);
                resetDiceController.AddMoney(targetAmount);
                break;
            }
            case TYPE.EXPLOSIVE_WAREHOUSE: 
            {
                CloseItemGuide();
                var blocks = FindObjectsOfType<Block>();
                foreach (Block block in blocks)
                {
                    if (!block.isDestroyed && block.blocksType != "마왕성")
                    {
                        if (block.GetPosY() == targetBlock.GetPosY() 
                            || block.GetPosX() == targetBlock.GetPosX())
                            {
                                string targetNumber = block.blockText.text;
                                block.ReduceBlockGage(targetNumber, true);
                            }
                    }
                }
                break;
            }
        }
    }
}
