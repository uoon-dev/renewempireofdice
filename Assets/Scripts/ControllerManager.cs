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

    void Awake()
    {
        if (levelLoader == null) levelLoader = FindObjectOfType<LevelLoader>();
        if (diceController == null) diceController = FindObjectOfType<DiceController>();
        if (blockController == null) blockController = FindObjectOfType<BlockController>();
        if (itemController == null) itemController = FindObjectOfType<ItemController>();
        if (noDiceNoCoinController == null) noDiceNoCoinController = FindObjectOfType<NoDiceNoCoinController>();
        if (speicalBlockController == null) speicalBlockController = FindObjectOfType<SpeicalBlockController>();
    }
}
