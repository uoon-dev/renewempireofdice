using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemController : MonoBehaviour
{
    [SerializeField] GameObject itemGuideCanvas;
    [SerializeField] GameObject coins;
    [SerializeField] Transform moneyText;
    [SerializeField] Image guideCharacter;
    [SerializeField] Image goldMineCircle;
    [SerializeField] Image coin;
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
        blockController.ToggleBounceClickableBlocks(false);
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
                EffectGoldMine(targetBlock);
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

    private void EffectGoldMine(Block targetBlock)
    {
        blockController.ToggleBounceClickableBlock(true, targetBlock);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(goldMineCircle.transform.DOMove(targetBlock.transform.position, 0));
        sequence.AppendCallback(() => goldMineCircle.gameObject.SetActive(true));
        sequence.Append(goldMineCircle.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f));
        sequence.Append(goldMineCircle.transform.DOScale(new Vector3(1, 1, 1), 0.1f));
        sequence.AppendInterval(0.1f);
        sequence.AppendCallback(() => { 
            goldMineCircle.gameObject.SetActive(false); 
            goldMineCircle.transform.DOScale(new Vector3(0, 0, 0), 0);
            blockController.ToggleBounceClickableBlock(false, targetBlock);
            });
        sequence.Play();
        StartCoroutine(CloneCoin(targetBlock.transform));
    }

    private IEnumerator CloneCoin(Transform targetBlockPosition)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(moneyText.position.x - targetBlockPosition.position.x, 2f) + Mathf.Pow(moneyText.position.y - targetBlockPosition.position.y, 2));
        float coinSize = 23;
        int coinAmount = (int)(distance / coinSize) / 4;

        coins.SetActive(true);

        for (int i = 0; i < coinAmount; i++)
        {
            Sequence sequence = DOTween.Sequence();
            GameObject clonedCoin = Instantiate(coin.gameObject, targetBlockPosition.position, targetBlockPosition.rotation);
            clonedCoin.transform.SetParent(coins.transform, false);

            sequence.Append(clonedCoin.transform.DOMove(new Vector2(targetBlockPosition.position.x, targetBlockPosition.position.y), 0));
            sequence.AppendCallback(() => clonedCoin.SetActive(true));
            sequence.Append(clonedCoin.transform.DOMove(new Vector2(moneyText.position.x, moneyText.position.y), 0.5f));
            sequence.AppendCallback(() => Destroy(clonedCoin));
            sequence.Play();

            yield return new WaitForSeconds(0.1f);
        }
    }
}
