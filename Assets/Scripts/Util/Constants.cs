using UnityEngine;
using System.Collections;
using System;

public class Constants : MonoBehaviour
{
    public static class API_ENDPOINT {
        public const string TIME_STAMP = "https://cueyedcuq4.execute-api.ap-northeast-2.amazonaws.com/default/get-time-stamp";
    }

    public static class SCENE_NAME {
        public const string MAP_SYSTEM = "Map System";
        public const string LEVEL = "Level";
        public const string LEVEL1 = "Level 1";
        public const string TUTORIAL = "Tutorial";
    }

    public static class STAGE_REWARD_TYPE {
        public const string HEART_REWARD_FULL = "heartRewardFull";
        public const string HEART_REWARD_ADD = "heartRewardADD";
    }

    public static class GAME_OBJECT_NAME {
        public const string BODY = "Body";
        public const string PANEL_SETTING = "PanelSetting";
        public const string NO_HEART_CANVAS = "No Heart Canvas";
        public const string AFTER_PURCHASE_EFFECT_CANVAS = "After Purchase Effect Canvas";
        public const string HEART_BAR_OBJECT = "Heart Bar";
        public const string HEART_IMAGE_PARENT_OBJECT = "Heart Images";
        public const string HEART_IMAGE_PARENT_OBJECT_IN_EFFECT = "Heart Images In Effect";
        public const string HEART_UPDATED_COUNT_TEXT = "Heart Updated Count Text";
        public const string HEART_TIMER_TEXT = "Heart Timer Text";
        public const string TIMER_TITLE_IN_SHOP = "Timer Title in Shop";
        public const string TITLE_IN_SHOP = "Title in Shop";
        public const string HEART_TIMER_TEXT_IN_NO_HEART_CANVAS = "Heart Timer Text in No Heart Canvas";
        public const string HEART_TIMER_TEXT_IN_SHOP = "Heart Timer Text in Shop";
        public const string HEART_RECHARGE_SPEED_UP_ITEM = "Heart Recharge Speed Up Item";
        public const string HEART_COUNT_TEXT = "Heart Count Text";
        public const string HEART_STATUS = "Heart Status";
        public const string HEART_IMAGE = "Heart Image";
        public const string HEART_CONTROLLER = "HeartController";
        public const string STAGE_TEXT = "StageText";
        public const string START_BUTTON = "Start Button";
        public const string RETRY_BUTTON = "Retry Button";
        public const string NEXT_LEVEL_BUTTON = "Next Level Button";
        public const string DICES = "Dices";
        public const string NUMBER_TEXT = "Number Text";
        public const string IMAGE = "Image";

        public const string DIAMOND_AMOUNT = "Diamond Amount";
        public const string BACKGROUND_IMAGE_WRAPPER = "Background Image Wrapper";
        public static class STAGE {
            public const string CLONED_BLOCK = "Block(Clone)";
            public const string COST_IMAGE = "Cost Image";
            public const string MONEY_TEXT = "Money Text";
            public const string MONEY_AREA = "Money Area";
            public const string COST_ICON = "Cost Icon";
            public const string COST_TEXT = "Cost Text";
            public const string ATTACK_POWER_TEXT = "Attack Power Text";
        }
        public static class SHOP {
            public const string PRICE = "Price";
            public const string TEXT = "Text";
            public const string LOADING_IMAGE = "Loading Image";
            public const string DIAMOND_IMAGE = "Diamond Image";
        }
    }

    public static class TUTORIAL {
        public static class GAME_OBJECT_NAME {
            public const string TUTORIAL_GUIDE_CANVAS = "Tutorial Guide Canvas";
            public const string INTRO_CANVAS = "Intro Canvas";
            public const string MAIN_DIALOGUE_CONTAINER = "Main Dialogue Container";
            public const string SUB_DIALOGUE_CONTAINER = "Sub Dialogue Container";
            public const string TEXT_BOX = "Text Box";
            public const string GUIDER_IMAGE = "Guider Image";
            public const string SUPER_TEXT = "Super Text";
            public const string OVAL = "Oval";
            public const string ATTACK_GAGE = "Attack Gage";
            public const string LEFT_AREA = "Left Area";
            public const string RIGHT_AREA = "Right Area";
            public const string OUTLINE = "Outline";
            public const string OUTLINE_DICE = "Outline Dice";
            public const string OUTLINE_CIRCLE = "Outline Circle";
            public const string OUTLINE_RECT = "Outline Rect";
            public const string OUTLINE_FULL_RECT = "Outline Full Rect";
            public const string BLOCKS = "Blocks";
            public const string TOAST = "Toast";
            public const string MINT_ARROW = "Mint Arrow";
            public const string INDICATE_ARROW = "Indicate Arrow";
            public const string TURN = "Turn";
            public const string DESC = "Description";
            public const string GUIDE_ITEM = "Guide Item";
            public const string BACKGROUND_IMAGE = "Background Image";
            public const string COST_IMAGE = "Cost Image";
            public const string MONEY_TEXT = "Money Text";
            public const string MONEY_AREA = "Money Area";
            public const string MONEY_ICON = "Money Icon";
            public const string COST_TEXT = "Cost Text";
            public const string ATTACK_POWER_TEXT = "Attack Power Text";
            public const string ATTACK_POWER_IMAGE = "Attack Power Image";
        }
    }

    public static class REWARD_TYPE {
        public const string HEART = "heart";
        public const string DIAMOND = "diamond";
        public const string GOLD_MINE = "goldMine";
        public const string EXPLOSIVE_WAREHOUSE = "explosiveWarehouse";
    }
    
    public static class LAST_BLOCK_STATE {
        public const string IS_NORMAL = "isNormal";
        public const string IS_CLICKABLE = "isClickable";
        public const string IS_DESTROYABLE = "isDestroyable";
        public const string IS_DYING = "isDying";
    }
    public static class TYPE {
        public const string LAST_BLOCK = "lastBlock";
        public const string FIRST_BLOCK = "firstBlock";
        public const string MIDDLE_BLOCK = "middleBlock";
        public const string LEFT_MIDDLE_BLOCK = "leftMiddleBlock";
        public const string BOTTOM_MIDDLE_BLOCK = "bottomMiddleBlock";
    }

    public static class SAVED_DATA {
        public const string DIAMOND = "diamond"; 
    }

    public const string MaldivesDice = "maldivesdice";
    public const string GoldrushDice = "goldrushdice";
    public const string SmallHeart = "smallheart";
    public const string LargeHeart = "largeheart";
    public const string DIAMOND_1 = "diamond1";
    public const string DIAMOND_2 = "diamond2";
    public const string DIAMOND_3 = "diamond3";
    public const string DIAMOND_4 = "diamond4";
    public const string DIAMOND_5 = "diamond5";
    public const string DIAMOND_6 = "diamond6";
    public const string GOLD_MINE_1 = "goldmine1";
    public const string GOLD_MINE_5 = "goldmine5";
    public const string EXPLOSIVE_WAREHOUSE_1 = "explosivewarehouse1";
    public const string EXPLOSIVE_WAREHOUSE_5 = "explosivewarehouse5";
    public const string EXPLOSIVE_WAREHOUSE = "explosivewarehouse";
    public const string HeartRechargeSpeedUp = "speedupheartrecharge1";
    public const int HEART_MAX_CHARGE_COUNT = 5;
    public const int TIMESTAMP_VALID_OFFSET_SECONDS = 20;
    // public const int HEART_CHARGE_SECONDS = 20;
    public const int HEART_CHARGE_SECONDS = 20 * 60;// 20 min
    public const int REWARD_CHARGE_SECONDS = 15 * 60;// 15 min
    public static class TEXT {
        public const string GOLD_MINE_GUIDE_TITLE = "황금 광산을 만드세요!"; 
        public const string GOLD_MINE_GUIDE_DESC = "선택한 땅을 즉시 딱뎀으로 점령하고, 그 땅의 방어력만큼 골드를 캐냅니다.";
        public const string GOLD_MINE_ITEM_TITLE = "황금 광산"; 
        public const string GOLD_MINE_ITEM_DESC = "선택한 땅(마왕성 제외)을 즉시 딱뎀으로 점령하고,\n그 땅의 방어력만큼 골드를 캐냅니다."; 
        public const string EXPLOSIVE_WAREHOUSE_GUIDE_TITLE = "화약고를 터뜨리세요!"; 
        public const string EXPLOSIVE_WAREHOUSE_GUIDE_DESC = "선택한 땅과 그 가로줄과 세로줄을 폭파시켜 즉시 딱뎀으로 점령합니다.";
        public const string EXPLOSIVE_WAREHOUSE_ITEM_TITLE = "화약고"; 
        public const string EXPLOSIVE_WAREHOUSE_ITEM_DESC = "선택한 땅과 그 가로줄과 세로줄을 폭파시켜 즉시 딱뎀으로 점령합니다.\n(마왕성 제외). 사용된 땅을 제외한 땅의 특수 효과는 얻을 수 없습니다.";
    }
}
