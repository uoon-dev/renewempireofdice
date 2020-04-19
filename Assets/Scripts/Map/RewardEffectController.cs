using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class RewardEffect {
    
    [Serializable]
    public class UICanvasGroup {
        public CanvasGroup beforeReward;
        public CanvasGroup afterReward;
    }

    [Serializable]
    public class UIAnimator {
        public Animator box;
    }

    [Serializable]
    public class UITransform {
        public Transform rewardItem;
    }

    [Serializable]
    public class UIImage {
        public Image rewardItem;
    }

    [Serializable]
    public class UIObject {
        public GameObject background;
    }

    [Serializable]
    public class UISprite {
        public Sprite heart;
        public Sprite diamond;
        public Sprite goldMine;
        public Sprite dynamite;
    }
}

public class RewardEffectController : ControllerManager
{
    public RewardEffect.UICanvasGroup uiCanvasGroup;
    public RewardEffect.UITransform uiTransform;
    public RewardEffect.UIImage uiImage;
    public RewardEffect.UISprite uiSprite;
    public RewardEffect.UIObject uiObject;
    public ProductController[] productControllers;
    public string rewardType;


    void OnEnable()
    {
        AnimateRewardEffect();
        SetRewardType();
    }

    public void OnClickGettingReward()
    {
        this.gameObject.SetActive(false);
        ResetRewardEffect();
        rewardController.SetRewardTargetTimeStamp();
    }

    public void AnimateRewardEffect()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.4f);
        sequence.Append(uiCanvasGroup.beforeReward.DOFade(0, 0));
        sequence.AppendCallback(() => uiCanvasGroup.afterReward.DOFade(1, 0.3f));
        sequence.Join(uiTransform.rewardItem.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f));
        sequence.Join(uiTransform.rewardItem.DOLocalMoveY(18.62f, 0.3f));
        sequence.Play();
    }

    public void ResetRewardEffect()
    {
        uiObject.background.SetActive(false);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(uiCanvasGroup.beforeReward.DOFade(1, 0));
        sequence.Append(uiCanvasGroup.afterReward.DOFade(0, 0f));
        sequence.Join(uiTransform.rewardItem.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0f));
        sequence.Join(uiTransform.rewardItem.DOLocalMoveY(37f, 0.3f));
        sequence.Play();
    }

    public void SetRewardSprite()
    {
        switch (ProductController.rewardType)
        {
            case Constants.REWARD_TYPE.HEART: 
            {
                uiImage.rewardItem.sprite = uiSprite.heart;
                break;
            }
            case Constants.REWARD_TYPE.DIAMOND:
            {
                uiImage.rewardItem.sprite = uiSprite.diamond;
                break;
            }
            case Constants.REWARD_TYPE.GOLD_MINE:
            {
                uiImage.rewardItem.sprite = uiSprite.goldMine;
                break;
            }
            case Constants.REWARD_TYPE.EXPLOSIVE_WAREHOUSE:
            {
                uiImage.rewardItem.sprite = uiSprite.dynamite;
                break;
            }
        }
    }

    public void SetRewardType()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue <= 0.1f) {
            productControllers[2].GetReward(1);
        }
        else if (randomValue <= 0.55) {
            productControllers[1].GetReward(1);
        }
        else {
            productControllers[0].GetReward(1);
        }

        SetRewardSprite();
    }    
}
