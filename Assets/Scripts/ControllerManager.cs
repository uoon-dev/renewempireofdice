using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Serializable]
// public class ControllerManagement {

//     [Serializable]
//     public class Stage {
//         public DiceController diceController;
//         public BlockController blockController;
//         public NoDiceNoCoinController noDiceNoCoinController;
//     }

//     public class Product {
//         public ItemController itemController;
//     }

//     [Serializable]
//     public class Util {
//         public LevelLoader levelLoader;
//     }
// }


public class ControllerManager : MonoBehaviour
{
    // public StageController StageController;
    // public UtilController UtilController;
    // [SerializeField] public NoDiceNoCoinController noDiceNoCoinController;
    public static LevelLoader levelLoader;
    public static DiceController diceController;
    public static BlockController blockController;
    public static ItemController itemController;
    public static NoDiceNoCoinController noDiceNoCoinController;
    public static SpeicalBlockController speicalBlockController;
    public static StatisticsController statisticsController;
    public static ResetDiceController resetDiceController;
    public static UIController uiController;
    public static CameraShaker[] cameraShakersForPlate;
    public static HeartShopController heartShopController;
    public static ItemShopController itemShopController;
    public static DiamondShopController diamondShopController;
    public static StartController startController;
    public static NewHeartController newHeartController;
    public static DiamondController diamondController;
    public static IAPManager iAPManager;
    public static RewardController rewardController;

    void Awake()
    {
        if (levelLoader == null) levelLoader = FindObjectOfType<LevelLoader>();
        if (diceController == null) diceController = FindObjectOfType<DiceController>();
        if (blockController == null) blockController = FindObjectOfType<BlockController>();
        if (itemController == null) itemController = FindObjectOfType<ItemController>();
        if (noDiceNoCoinController == null) noDiceNoCoinController = FindObjectOfType<NoDiceNoCoinController>();
        if (speicalBlockController == null) speicalBlockController = FindObjectOfType<SpeicalBlockController>();
        if (statisticsController == null) statisticsController = FindObjectOfType<StatisticsController>();
        if (resetDiceController == null) resetDiceController = FindObjectOfType<ResetDiceController>();
        if (uiController == null) uiController = FindObjectOfType<UIController>();
        if (heartShopController == null) heartShopController = FindObjectOfType<HeartShopController>();
        if (itemShopController == null) itemShopController = FindObjectOfType<ItemShopController>();
        if (diamondShopController == null) diamondShopController = FindObjectOfType<DiamondShopController>();
        if (startController == null) startController = FindObjectOfType<StartController>();
        if (newHeartController == null) newHeartController = FindObjectOfType<NewHeartController>();
        if (diamondController == null) diamondController = FindObjectOfType<DiamondController>();
        if (iAPManager == null) iAPManager = FindObjectOfType<IAPManager>();
        if (rewardController == null) rewardController = FindObjectOfType<RewardController>();
        if (this.name != Constants.GAME_OBJECT_NAME.STAGE.CLONED_BLOCK) cameraShakersForPlate = FindObjectsOfType<CameraShaker>();

        _initialize();
    }

    protected virtual void _initialize(){}

    void Update()
    {
        _update();   
    }

    protected virtual void _update(){}
}
