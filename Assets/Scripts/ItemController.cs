using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class ExplosiveWarehouseEffect {
    public Image dynamiteImage;
    public Image smogX;
    public Image smogY;
    public CanvasGroup smogXCanvasGroup;
    public CanvasGroup smogYCanvasGroup;
    public Animator dynamiteAnimator;
};

public class ItemController : MonoBehaviour
{
    public ExplosiveWarehouseEffect explosiveWarehouseEffect;
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
    ItemShopController itemShopController;
    public string onClickedType;
    private int goldMineAmount;
    private int explosiveWarehouseAmount;

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
        
        itemShopController = FindObjectOfType<ItemShopController>();
        blockController = FindObjectOfType<BlockController>();
        resetDiceController = FindObjectOfType<ResetDiceController>();

        LoadItem();
    }

    private void OnDestroy()
    {
        SaveItem(TYPE.GOLD_MINE, goldMineAmount);
        SaveItem(TYPE.EXPLOSIVE_WAREHOUSE, explosiveWarehouseAmount);
    }

    private void OnApplicationQuit()
    {
        SaveItem(TYPE.GOLD_MINE, goldMineAmount);
        SaveItem(TYPE.EXPLOSIVE_WAREHOUSE, explosiveWarehouseAmount);
    }

    private void OnApplicationPause(bool pause)
    {
        SaveItem(TYPE.GOLD_MINE, goldMineAmount);
        SaveItem(TYPE.EXPLOSIVE_WAREHOUSE, explosiveWarehouseAmount);
    }    

    public void OnClickedGoldMine()
    {
        if (goldMineAmount < 1)
        {
            itemShopController.ToggleItemShopCanvas(true, TYPE.GOLD_MINE);
            return;
        }

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
        if (explosiveWarehouseAmount < 1)
        {
            itemShopController.ToggleItemShopCanvas(true, TYPE.EXPLOSIVE_WAREHOUSE);
            return;
        }

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
                SubtractItemAmount(TYPE.GOLD_MINE, 1);
                EffectGoldMine(targetBlock);
                break;
            }
            case TYPE.EXPLOSIVE_WAREHOUSE: 
            {
                CloseItemGuide();
                SubtractItemAmount(TYPE.EXPLOSIVE_WAREHOUSE, 1);
                EffectExplosiveWarehouse(targetBlock);

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
        int targetAmount = int.Parse(targetBlock.blockText.text);
        Sequence sequence = DOTween.Sequence();

        blockController.ToggleBounceClickableBlock(true, targetBlock);
        sequence.Append(goldMineCircle.transform.DOMove(targetBlock.transform.position, 0));
        sequence.AppendCallback(() => goldMineCircle.gameObject.SetActive(true));
        sequence.Append(goldMineCircle.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f));
        sequence.Append(goldMineCircle.transform.DOScale(new Vector3(1, 1, 1), 0.1f));
        sequence.AppendInterval(0.3f);
        sequence.AppendCallback(() => { 
            goldMineCircle.gameObject.SetActive(false); 
            goldMineCircle.transform.DOScale(new Vector3(0, 0, 0), 0);
            blockController.ToggleBounceClickableBlock(false, targetBlock);
            resetDiceController.AddMoney(targetAmount);
            });
        sequence.Play();
        StartCoroutine(CloneCoin(targetBlock));
    }

    private void EffectExplosiveWarehouse(Block targetBlock)
    {

        int targetAmount = int.Parse(targetBlock.blockText.text);
        Block middleBlock = blockController.GetOneBlock(Constants.TYPE.MIDDLE_BLOCK);
        Sequence sequence = DOTween.Sequence();

        // dynamite
        sequence.Append(explosiveWarehouseEffect.dynamiteImage.transform.DOMove(new Vector2(targetBlock.transform.position.x, targetBlock.transform.position.y + 10), 0));
        sequence.AppendCallback(() => explosiveWarehouseEffect.dynamiteImage.gameObject.SetActive(true));
        sequence.AppendCallback(() => explosiveWarehouseEffect.dynamiteAnimator.SetTrigger("start"));
        sequence.AppendInterval(0.4f);
        sequence.AppendCallback(() => explosiveWarehouseEffect.dynamiteImage.gameObject.SetActive(false));
        sequence.AppendCallback(() => explosiveWarehouseEffect.dynamiteAnimator.ResetTrigger("start"));

        // smog
        sequence.Append(explosiveWarehouseEffect.smogX.transform.DOMove(new Vector2(middleBlock.transform.position.x, targetBlock.transform.position.y), 0));
        sequence.Append(explosiveWarehouseEffect.smogY.transform.DOMove(new Vector2(targetBlock.transform.position.x, middleBlock.transform.position.y), 0));
        sequence.AppendCallback(() => {
            explosiveWarehouseEffect.smogX.gameObject.SetActive(true);
            explosiveWarehouseEffect.smogY.gameObject.SetActive(true);
        });
        sequence.Append(explosiveWarehouseEffect.smogX.transform.DOScale(new Vector2(0.364f, 0.364f), 0.08f));
        sequence.Join(explosiveWarehouseEffect.smogXCanvasGroup.DOFade(0.8f, 0.08f));
        sequence.Join(explosiveWarehouseEffect.smogY.transform.DOScale(new Vector2(0.364f, 0.364f), 0.08f));
        sequence.Join(explosiveWarehouseEffect.smogYCanvasGroup.DOFade(0.8f, 0.08f));

        sequence.Append(explosiveWarehouseEffect.smogX.transform.DOScale(new Vector2(0.476f, 0.476f), 0.08f));
        sequence.Join(explosiveWarehouseEffect.smogXCanvasGroup.DOFade(0.4f, 0.08f));
        sequence.Join(explosiveWarehouseEffect.smogY.transform.DOScale(new Vector2(0.476f, 0.476f), 0.08f));
        sequence.Join(explosiveWarehouseEffect.smogYCanvasGroup.DOFade(0.4f, 0.08f));

        sequence.Append(explosiveWarehouseEffect.smogXCanvasGroup.DOFade(0f, 0.08f));
        sequence.Join(explosiveWarehouseEffect.smogYCanvasGroup.DOFade(0f, 0.08f));        
        sequence.AppendCallback(() => {
            explosiveWarehouseEffect.smogX.gameObject.SetActive(false);
            explosiveWarehouseEffect.smogY.gameObject.SetActive(false);
        });
        sequence.Play();
    }

    private IEnumerator CloneCoin(Block targetBlock)
    {
        Transform targetBlockPosition = targetBlock.transform;

        float distance = Mathf.Sqrt(Mathf.Pow(moneyText.position.x - targetBlockPosition.position.x, 2f) + Mathf.Pow(moneyText.position.y - targetBlockPosition.position.y, 2));
        float coinSize = 23;
        float coinAmount = (distance / coinSize) / 4;
        coinAmount = int.Parse(targetBlock.blockText.text);

        coins.SetActive(true);

        for (int i = 0; i < coinAmount; i++)
        {
            Sequence sequence = DOTween.Sequence();

            GameObject clonedCoin = Instantiate(coin.gameObject, targetBlockPosition.position, targetBlockPosition.rotation);
            clonedCoin.transform.SetParent(coins.transform, false);
            
                        
            sequence.Append(clonedCoin.transform.DOMove(new Vector2(targetBlockPosition.position.x, targetBlockPosition.position.y), 0));
            sequence.AppendCallback(() => { 
                clonedCoin.SetActive(true); 
                });
            sequence.Append(clonedCoin.transform.DOMoveX(moneyText.position.x, 0.4f).SetEase(Ease.Linear));
            sequence.Join(clonedCoin.transform.DOMoveY(moneyText.position.y, 0.4f).SetEase(Ease.InCubic));
            sequence.AppendCallback(() => Destroy(clonedCoin));
            sequence.Play();

            yield return new WaitForSeconds(1 / (coinAmount * 5));
        }
    }

    private void LoadItem()
    {
        if (StorageController.IsKeyExists(TYPE.GOLD_MINE))
        {
            goldMineAmount = StorageController.LoadItemAmount(TYPE.GOLD_MINE);
        }

        if (StorageController.IsKeyExists(TYPE.EXPLOSIVE_WAREHOUSE))
        {
            explosiveWarehouseAmount = StorageController.LoadItemAmount(TYPE.EXPLOSIVE_WAREHOUSE);
        }
    }

    public int GetItemAmount(string type)
    {
        switch (type)
        {
            case TYPE.GOLD_MINE: 
            {
                return goldMineAmount;
            }
            case TYPE.EXPLOSIVE_WAREHOUSE: 
            {
                return explosiveWarehouseAmount;
            }
        }

        return 0;
    }

    public void SaveItem(string type, int targetAmount)
    {
        StorageController.SaveItemAmount(type, targetAmount);
    }

    public void AddItemAmount(string type, int targetAmount)
    {
        switch (type)
        {
            case TYPE.GOLD_MINE: 
            {
                goldMineAmount += targetAmount;
                break;
            }
            case TYPE.EXPLOSIVE_WAREHOUSE: 
            {
                explosiveWarehouseAmount += targetAmount;
                break;
            }
        }
    }

    public void SubtractItemAmount(string type, int targetAmount)
    {
        switch (type)
        {
            case TYPE.GOLD_MINE: 
            {
                goldMineAmount -= targetAmount;
                break;
            }
            case TYPE.EXPLOSIVE_WAREHOUSE: 
            {
                explosiveWarehouseAmount -= targetAmount;
                break;
            }
        }
    }
}
