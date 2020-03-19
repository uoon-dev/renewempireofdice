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
        Debug.Log(ES3.KeyExists("diamondAmount"));
        Debug.Log(StorageController.LoadDiamondAmount() + ":StorageController.LoadDiamondAmount()");
        // if (PlayerPrefs.HasKey(Constants.SAVED_DATA.DIAMOND))
        // {
        //     Debug.Log(diamondAmount + ":diamondAmount");
        // }
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
        // PlayerPrefs.SetInt(Constants.SAVED_DATA.DIAMOND, targetAmount);
        // PlayerPrefs.Save();
        StorageController.SaveDiamondAmount(targetAmount);
        Debug.Log(StorageController.LoadDiamondAmount() + ":StorageController.LoadDiamondAmount()");
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
