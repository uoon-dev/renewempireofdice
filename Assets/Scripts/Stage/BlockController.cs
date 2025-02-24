﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class BlockController : ControllerManager
{
    [SerializeField] GameObject block = null;
    [SerializeField] Sprite goldMineBackgroundImage;
    [SerializeField] Sprite explosiveWarehouseBackgroundImage;

    public static int boardWidth = 1;
    public static int boardHeight = 1;
    public static GameObject lastBlock = null;
    public static int boardSize = 1;
    object speicalBlocks;
    List<GameObject> blocks;
    Block[] createdBlocks;
    public Block generatedLastBlock;


    void Start()
    {
        blocks = new List<GameObject>();
        InitBlocks();
        createdBlocks = FindObjectsOfType<Block>();
        // this.gameObjezct.SetActive(false);
        // this.gameObject.SetActive(true);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("currentLevelNumber");
    }

    private void InitBlocks()
    {
        int currentLevelNumber = levelLoader.GetCurrentLevelNumber();
        List<string> blockTexts = new List<string>();
        List<string> blockTypes = new List<string>();
        SetBoardType(currentLevelNumber);
        CreateBlocks();
        AnalyticsEvent.LevelStart(currentLevelNumber);
        if (StorageController.IsBlocksSaved(currentLevelNumber) && levelLoader.GetCurrentSceneName() != Constants.SCENE_NAME.TUTORIAL)
        {
            speicalBlockController.SetSpeicialBlocks(speicalBlocks, false);
            blockTexts = StorageController.LoadBlocksText(currentLevelNumber);
            blockTypes = StorageController.LoadBlocksType(currentLevelNumber);

            for (int i = 0; i < blocks.Count; i++)
            {
                GameObject clonedBlock = blocks[i];
                Block tmpBlock = clonedBlock.GetComponent<Block>();

                string blockType = "";
                if (i < blockTypes.Count) {
                    blockType = blockTypes[i];
                }

                if (i == 0)
                {
                    tmpBlock.isClickable = true;
                }

                if (blockType != string.Empty)
                {
                    tmpBlock.SetBlockType(blockTypes[i]);
                    tmpBlock.SetTooltipInfo();
                }

                if (i < blockTexts.Count) {
                    tmpBlock.blockText.text = blockTexts[i];
                    // tmpBlock.SetBlocksValue(false);
                }
            }

            SetBlocksValue(false);
        }
        else
        {
            Debug.Log("Save");
            SetBlocksValue(true);
            foreach (GameObject clonedBlock in blocks)
            {
                Block tmpBlock = clonedBlock.GetComponent<Block>();
                // tmpBlock.SetBlocksValue();
            }
            speicalBlockController.SetSpeicialBlocks(speicalBlocks);

            foreach (GameObject clonedBlock in blocks)
            {
                Block tmpBlock = clonedBlock.GetComponent<Block>();
                blockTypes.Add(tmpBlock.blocksType);
                blockTexts.Add(tmpBlock.blockText.text);
            }

            StorageController.SaveBlocksType(currentLevelNumber, blockTypes);
            StorageController.SaveBlocksText(currentLevelNumber, blockTexts);
        }
    }

    private void CreateBlocks()
    {
        for (int i = 1; i <= boardWidth; i++)
        {
            for (int j = 1; j <= boardHeight; j++)
            {
                GameObject clonedBlock = Instantiate(block, transform.position, transform.rotation);
                clonedBlock.transform.SetParent(this.transform, false);
                clonedBlock.transform.localPosition = new Vector3(i * 46, j * 46, 1);
                // clonedBlock.isStatic = true;
                // StaticBatchingUtility.Combine(this.gameObject);

                if (i == boardHeight && j == boardWidth)
                {
                    lastBlock = clonedBlock;
                }

                blocks.Add(clonedBlock);
            }
        }

        Destroy(block);
    }

    public void SetBlocksValue(bool setNumber = true)
    {
        Block[] blocks = FindObjectsOfType<Block>();
        int randomNum = 0;

        foreach(Block block in blocks)
        {
            block.Init();
            
            randomNum = UnityEngine.Random.Range(1, block.posX + block.posY+2) * 2 + UnityEngine.Random.Range(1, 7);        

            if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.TUTORIAL)
            {
                randomNum = block.GetTutorialBlocksValue(block.posX, block.posY);
            } 
            
            if (setNumber)
            {
                block.SetBlockValue(randomNum.ToString());
            }

            if (block.posX == 1 && block.posY == 1 && levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.LEVEL)
            {
                int randomNumInDices = diceController.GetDiceNumberRandomly();
                block.SetBlockValue(randomNumInDices.ToString());
            }

            block.UpdateBlocksUI();
        }
    }    

    private void SetBoardType(int currentLevelNumber)
    {
        boardWidth = 8;
        boardHeight = 8;

        var newLandInfoController = FindObjectOfType<NewLandInfoController>();
        var guideCanvasController = FindObjectOfType<GuideCanvasController>();

        if (levelLoader.GetCurrentSceneName() == "Level")
        {
            if (currentLevelNumber == 2)
            {
                boardWidth = 4;
                boardHeight = 4;
                if (guideCanvasController != null)
                {
                    guideCanvasController.SetGuideType("clearCondition");
                    guideCanvasController.ToggleGuideCanvas(true);
                }
            }

            if (currentLevelNumber == 3)
            {
                boardWidth = 4;
                boardHeight = 4;
                speicalBlocks = new { vertical = 0, horizontal = 0, bomb = 0, mine = 3, army = 0, wizard = 0, dungeon = 0, relics = 0, lastblock = 10 };
                if (newLandInfoController != null)
                {
                    newLandInfoController.SetLandType("mine");
                    newLandInfoController.ToggleNewLandInfoCanvas(true);
                }
            }

            if (currentLevelNumber == 4)
            {
                boardWidth = 5;
                boardHeight = 5;
                speicalBlocks = new { vertical = 0, horizontal = 0, bomb = 0, mine = 4, army = 0, wizard = 0, dungeon = 0, relics = 0, lastblock = 10 };

            }

            if (currentLevelNumber == 5)
            {
                boardWidth = 5;
                boardHeight = 5;
                speicalBlocks = new { vertical = 0, horizontal = 0, bomb = 0, mine = 1, army = 0, wizard = 0, dungeon = 3, relics = 0, lastblock = 20 };
                if (newLandInfoController != null)
                {
                    newLandInfoController.SetLandType("dungeon");
                    newLandInfoController.ToggleNewLandInfoCanvas(true);
                }
            }

            if (currentLevelNumber == 6)
            {
                boardWidth = 6;
                boardHeight = 6;
                speicalBlocks = new { vertical = 0, horizontal = 0, bomb = 0, mine = 2, army = 0, wizard = 0, dungeon = 4, relics = 0, lastblock = 10 };
            }

            if (currentLevelNumber == 7)
            {
                boardWidth = 6;
                boardHeight = 6;
                speicalBlocks = new { vertical = 0, horizontal = 0, bomb = 4, mine = 1, army = 0, wizard = 0, dungeon = 1, relics = 0, lastblock = 10 };
                if (newLandInfoController != null)
                {
                    newLandInfoController.SetLandType("bomb");
                    newLandInfoController.ToggleNewLandInfoCanvas(true);
                }
            }

            if (currentLevelNumber == 8)
            {
                boardWidth = 7;
                boardHeight = 7;
                speicalBlocks = new { vertical = 0, horizontal = 3, bomb = 1, mine = 0, army = 0, wizard = 0, dungeon = 3, relics = 0, lastblock = 10 };

                if (newLandInfoController != null)
                {
                    newLandInfoController.SetLandType("horizontal");
                    newLandInfoController.ToggleNewLandInfoCanvas(true);
                }
            }

            if (currentLevelNumber == 9)
            {
                boardWidth = 7;
                boardHeight = 7;
                speicalBlocks = new { vertical = 3, horizontal = 1, bomb = 0, mine = 3, army = 0, wizard = 0, dungeon = 0, relics = 0, lastblock = 10 };
                if (newLandInfoController != null)
                {
                    newLandInfoController.SetLandType("vertical");
                    newLandInfoController.ToggleNewLandInfoCanvas(true);
                }
            }

            if (currentLevelNumber == 10)
            {
                boardWidth = 8;
                boardHeight = 8;
                speicalBlocks = new { mine = 2, dungeon = 1, vertical = 2, horizontal = 2, bomb = 0, army = 0, wizard = 0, relics = 0, lastblock = 20 };

            }

            if (currentLevelNumber == 11)
            {
                speicalBlocks = new { mine = 0, dungeon = 0, vertical = 2, horizontal = 2, bomb = 3, army = 0, wizard = 0, relics = 0, lastblock = 20 };

            }

            if (currentLevelNumber == 12)
            {
                speicalBlocks = new { mine = 3, dungeon = 0, vertical = 2, horizontal = 0, bomb = 0, army = 2, wizard = 0, relics = 0, lastblock = 20 };
                if (newLandInfoController != null)
                {
                    newLandInfoController.SetLandType("army");
                    newLandInfoController.ToggleNewLandInfoCanvas(true);
                }
            }

            if (currentLevelNumber == 13)
            {

                speicalBlocks = new { mine = 2, dungeon = 2, vertical = 0, horizontal = 2, bomb = 0, army = 2, wizard = 0, relics = 0, lastblock = 20 };
            }

            if (currentLevelNumber == 14)
            {
                speicalBlocks = new { mine = 4, dungeon = 0, vertical = 0, horizontal = 0, bomb = 1, army = 1, wizard = 2, relics = 0, lastblock = 20 };
                if (newLandInfoController != null)
                {
                    newLandInfoController.SetLandType("wizard");
                    newLandInfoController.ToggleNewLandInfoCanvas(true);
                }
            }

            if (currentLevelNumber == 15)
            {
                speicalBlocks = new { mine = 2, dungeon = 3, vertical = 0, horizontal = 1, bomb = 0, army = 1, wizard = 1, relics = 0, lastblock = 30 };
            }

            if (currentLevelNumber == 16)
            {
                speicalBlocks = new { mine = 3, dungeon = 0, vertical = 1, horizontal = 1, bomb = 1, army = 0, wizard = 0, relics = 3, lastblock = 20 };

                if (newLandInfoController != null)
                {
                    newLandInfoController.SetLandType("relics");
                    newLandInfoController.ToggleNewLandInfoCanvas(true);
                }
            }

            if (currentLevelNumber == 17)
            {
                speicalBlocks = new { mine = 0, dungeon = 3, vertical = 0, horizontal = 0, bomb = 0, army = 3, wizard = 1, relics = 2, lastblock = 20 };
            }

            if (currentLevelNumber == 18)
            {
                speicalBlocks = new { mine = 5, dungeon = 0, vertical = 0, horizontal = 0, bomb = 0, army = 0, wizard = 0, relics = 1, lastblock = 20 };

            }

            if (currentLevelNumber == 19)
            {
                speicalBlocks = new { mine = 2, dungeon = 2, vertical = 0, horizontal = 1, bomb = 2, army = 1, wizard = 0, relics = 2, lastblock = 20 };

            }

            if (currentLevelNumber == 20)
            {
                speicalBlocks = new { mine = 1, dungeon = 1, vertical = 1, horizontal = 0, bomb = 1, army = 1, wizard = 2, relics = 3, lastblock = 30 };
            }

            if (currentLevelNumber > 20)
            {
                if (currentLevelNumber % 30 == 1)
                {
                    speicalBlocks = new { mine = 2, dungeon = 2, vertical = 2, horizontal = 2, bomb = 1, army = 1, wizard = 0, relics = 0, lastblock = 20 };

                }

                if (currentLevelNumber % 30 == 2)
                {
                    speicalBlocks = new { mine = 2, dungeon = 3, vertical = 0, horizontal = 0, bomb = 2, army = 1, wizard = 0, relics = 3, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 3)
                {
                    speicalBlocks = new { mine = 3, dungeon = 2, vertical = 1, horizontal = 0, bomb = 0, army = 2, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 4)
                {
                    speicalBlocks = new { mine = 0, dungeon = 0, vertical = 0, horizontal = 0, bomb = 10, army = 10, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 5)
                {
                    speicalBlocks = new { mine = 1, dungeon = 0, vertical = 3, horizontal = 3, bomb = 2, army = 1, wizard = 0, relics = 0, lastblock = 30 };

                }

                if (currentLevelNumber % 30 == 6)
                {
                    speicalBlocks = new { mine = 5, dungeon = 1, vertical = 0, horizontal = 0, bomb = 1, army = 1, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 7)
                {
                    speicalBlocks = new { mine = 3, dungeon = 3, vertical = 1, horizontal = 0, bomb = 0, army = 1, wizard = 1, relics = 1, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 8)
                {
                    speicalBlocks = new { mine = 0, dungeon = 0, vertical = 0, horizontal = 0, bomb = 20, army = 0, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 9)
                {
                    speicalBlocks = new { mine = 4, dungeon = 3, vertical = 0, horizontal = 1, bomb = 0, army = 0, wizard = 2, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 10)
                {
                    speicalBlocks = new { mine = 2, dungeon = 4, vertical = 2, horizontal = 0, bomb = 1, army = 1, wizard = 1, relics = 1, lastblock = 30 };
                }

                if (currentLevelNumber % 30 == 11)
                {

                    speicalBlocks = new { mine = 4, dungeon = 0, vertical = 0, horizontal = 2, bomb = 0, army = 0, wizard = 2, relics = 4, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 12)
                {
                    speicalBlocks = new { mine = 0, dungeon = 1, vertical = 3, horizontal = 3, bomb = 0, army = 3, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 13)
                {
                    speicalBlocks = new { mine = 2, dungeon = 2, vertical = 3, horizontal = 0, bomb = 1, army = 0, wizard = 1, relics = 2, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 14)
                {
                    speicalBlocks = new { mine = 5, dungeon = 0, vertical = 0, horizontal = 3, bomb = 0, army = 3, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 15)
                {
                    speicalBlocks = new { mine = 2, dungeon = 2, vertical = 0, horizontal = 0, bomb = 1, army = 1, wizard = 2, relics = 2, lastblock = 60 };
                }

                if (currentLevelNumber % 30 == 16)
                {
                    speicalBlocks = new { mine = 1, dungeon = 3, vertical = 1, horizontal = 1, bomb = 1, army = 0, wizard = 0, relics = 4, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 17)
                {
                    speicalBlocks = new { mine = 3, dungeon = 0, vertical = 0, horizontal = 0, bomb = 0, army = 10, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 18)
                {
                    speicalBlocks = new { mine = 1, dungeon = 5, vertical = 1, horizontal = 1, bomb = 1, army = 2, wizard = 2, relics = 1, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 19)
                {
                    speicalBlocks = new { mine = 0, dungeon = 0, vertical = 10, horizontal = 10, bomb = 0, army = 0, wizard = 0, relics = 0, lastblock = 20 };
                }
                if (currentLevelNumber % 30 == 20)
                {
                    speicalBlocks = new { mine = 4, dungeon = 0, vertical = 4, horizontal = 4, bomb = 0, army = 0, wizard = 0, relics = 0, lastblock = 30 };
                }

                if (currentLevelNumber % 30 == 21)
                {

                    speicalBlocks = new { mine = 2, dungeon = 8, vertical = 0, horizontal = 0, bomb = 0, army = 1, wizard = 2, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 22)
                {
                    speicalBlocks = new { mine = 4, dungeon = 0, vertical = 10, horizontal = 10, bomb = 10, army = 0, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 23)
                {
                    speicalBlocks = new { mine = 3, dungeon = 2, vertical = 1, horizontal = 1, bomb = 0, army = 2, wizard = 2, relics = 2, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 24)
                {
                    speicalBlocks = new { mine = 5, dungeon = 0, vertical = 1, horizontal = 1, bomb = 2, army = 0, wizard = 2, relics = 5, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 25)
                {
                    speicalBlocks = new { mine = 3, dungeon = 3, vertical = 1, horizontal = 4, bomb = 1, army = 1, wizard = 0, relics = 0, lastblock = 30 };
                }

                if (currentLevelNumber % 30 == 26)
                {
                    speicalBlocks = new { mine = 3, dungeon = 3, vertical = 4, horizontal = 1, bomb = 1, army = 1, wizard = 2, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 27)
                {
                    speicalBlocks = new { mine = 3, dungeon = 10, vertical = 0, horizontal = 0, bomb = 0, army = 0, wizard = 4, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 28)
                {
                    speicalBlocks = new { mine = 0, dungeon = 0, vertical = 0, horizontal = 0, bomb = 0, army = 8, wizard = 0, relics = 0, lastblock = 10 };
                }

                if (currentLevelNumber % 30 == 29)
                {
                    speicalBlocks = new { mine = 5, dungeon = 0, vertical = 1, horizontal = 1, bomb = 0, army = 3, wizard = 0, relics = 0, lastblock = 20 };
                }

                if (currentLevelNumber % 30 == 0)
                {
                    speicalBlocks = new { mine = 2, dungeon = 0, vertical = 2, horizontal = 2, bomb = 2, army = 0, wizard = 4, relics = 4, lastblock = 100 };
                }
            }
        }
        else
        {
            boardWidth = 3;
            boardHeight = 3;
        }
    }

    public static int GetBoardSize()
    {
        return boardWidth;
    }

    public Block GetOneBlock(string type)
    {
        Block oneBlock = null;

        foreach (Block block in createdBlocks)
        {
            if (type == Constants.TYPE.LAST_BLOCK) 
            {
                if (block.blocksType == "마왕성")
                {
                    oneBlock = block;
                }
            }
            else if (type == Constants.TYPE.MIDDLE_BLOCK)
            {
                if (block.GetPosX() == Mathf.Round(boardWidth/2f) && block.GetPosY() == Mathf.Round(boardWidth/2f))
                {
                    oneBlock = block;
                }
            }
            else if (type == Constants.TYPE.FIRST_BLOCK)
            {
                if (block.GetPosX() == 1 && block.GetPosY() == 1)
                {
                    oneBlock = block;
                }                
            }
            else if (type == Constants.TYPE.LEFT_MIDDLE_BLOCK)
            {
                if (block.GetPosX() == 1 && block.GetPosY() == 2)
                {
                    oneBlock = block;
                }                
            }
            else if (type == Constants.TYPE.BOTTOM_MIDDLE_BLOCK)
            {
                if (block.GetPosX() == 2 && block.GetPosY() == 1)
                {
                    oneBlock = block;
                }                
            }
        }

        return oneBlock;
    }

    public void SetLastBlock(Block targetBlock)
    {
        generatedLastBlock = targetBlock;
    }

    public Block GetLastBlock()
    {
        return generatedLastBlock;
    }

    public void ToggleBounceClickableBlocks(bool isActive, string type = null)
    {
        foreach (Block block in createdBlocks)
        {
            if (block.isClickable && block.blocksType != "마왕성")
            {
                GameObject backgroundImageWrapper = block.transform.Find(Constants.GAME_OBJECT_NAME.BACKGROUND_IMAGE_WRAPPER).gameObject;
                Image bgimage = backgroundImageWrapper.GetComponent<Image>();
                backgroundImageWrapper.SetActive(isActive);
                if (type == ItemController.TYPE.GOLD_MINE)
                {
                    bgimage.GetComponent<Image>().sprite = goldMineBackgroundImage;
                }
                else if (type == ItemController.TYPE.EXPLOSIVE_WAREHOUSE)
                {
                    bgimage.GetComponent<Image>().sprite = explosiveWarehouseBackgroundImage;
                }
                block.GetComponent<Canvas>().overrideSorting = isActive;
                block.GetComponent<Canvas>().sortingOrder = isActive ? 20 : 5;
            }
        }
    }

    public void ToggleBounceClickableBlock(bool isActive, Block targetBlock)
    {
        GameObject backgroundImageWrapper = targetBlock.transform.Find(Constants.GAME_OBJECT_NAME.BACKGROUND_IMAGE_WRAPPER).gameObject;
        backgroundImageWrapper.SetActive(isActive);
        targetBlock.GetComponent<Canvas>().overrideSorting = isActive;
        targetBlock.GetComponent<Canvas>().sortingOrder = isActive ? 20 : 5;
    }

    public void HandleLastBlock(string blocksType, int resultGage)
    {
        string state = Constants.LAST_BLOCK_STATE.IS_NORMAL;

        generatedLastBlock.uiImage.backgroundImage.overrideSprite = null;
        
        if (blocksType == "마왕성")
        {
            if (resultGage == 0)
            {
                state = Constants.LAST_BLOCK_STATE.IS_DYING;
            }
            else
            {
                state = Constants.LAST_BLOCK_STATE.IS_DESTROYABLE;
            }
        }
        else
        {
            if (generatedLastBlock.isClickable)
            {
                state = Constants.LAST_BLOCK_STATE.IS_CLICKABLE;   
            }
            else
            {
                state = Constants.LAST_BLOCK_STATE.IS_NORMAL;   
            }
        }

        AnimateLastBlock(state);
    }
    public void AnimateLastBlock(string state)
    {
        generatedLastBlock.uiAnimator.backgroundImageAnimator.enabled = true;        
        generatedLastBlock.uiAnimator.backgroundImageAnimator.SetTrigger(state);
    }
}