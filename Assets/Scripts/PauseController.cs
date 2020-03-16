using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject PauseCanvas = null;
    LevelLoader levelLoader;
    [SerializeField]
    GameObject stageText;

    void Start()
    {
        Initialize();
        HideScreen();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>(); 

        stageText.GetComponent<Text>().text = $"Stage {levelLoader.GetCurrentLevelNumber()}";
    }

    public void ShowScreen()
    {
        var blocks = FindObjectsOfType<Block>();
        foreach (var block in blocks) {
            block.HideTooltip();
        }
        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.TUTORIAL)
        {
            PauseCanvas.GetComponent<Canvas>().sortingOrder = 103;
        }
        PauseCanvas.SetActive(true);
    }

    public void HideScreen()
    {
        PauseCanvas.SetActive(false);
    }
}
