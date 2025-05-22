using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string fileName;

    public static SaveManager instance;
    private GameData gameData;
    private FileDataHandler dataHandler;
    private List<ISaveManager> saveManagers;


    [ContextMenu("Delet save file")]
    public void DeletSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("No save data found!");
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);    
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None).OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSaveData()
    {
        if(dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }

}
