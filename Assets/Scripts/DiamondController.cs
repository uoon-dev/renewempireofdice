using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : MonoBehaviour
{
    public static DiamondController Instance;
    private static int diamondAmount;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
            
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Initialize()
    {
        diamondAmount = StorageController.LoadDiamondAmount();
    }
        
    private void OnDestroy()
    {
        SaveDiamondAmount(diamondAmount);
    }

    private void OnApplicationQuit()
    {
        SaveDiamondAmount(diamondAmount);
    }

    private void OnApplicationPause(bool pause)
    {
        SaveDiamondAmount(diamondAmount);
    }

    public void SaveDiamondAmount(int targetAmount)
    {
        StorageController.SaveDiamondAmount(targetAmount);
    }

    public int GetDiamondAmount()
    {
        return diamondAmount;
    }

    public void AddDiamondAmount(int targetAmount)
    {
        diamondAmount += targetAmount;
    }

    public void SubtractDiamondAmount(int targetAmount)
    {
        diamondAmount -= targetAmount;
    }
}
