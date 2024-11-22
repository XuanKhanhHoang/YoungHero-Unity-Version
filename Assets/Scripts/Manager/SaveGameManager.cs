using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SaveGameStatus { SUCCESS, FAIL, CANT_SAVE }
[Serializable]
public class GameData
{
    public Vector2 playerPos;
    public int curHP, lv, coin, curEXP;
    public List<SavedInventoryItem> inventoryItemsNameAndAmount;
    public int gameActionIndex, gameDialogIndex;
    public int sceneIndex;
    public int weaponInventoryIndex;

}
public class SavedInventoryItem
{
    public string name;
    public int amount;
    public SavedInventoryItem(string name, int amount)
    {
        this.name = name;
        this.amount = amount;
    }
}
public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static SaveGameStatus SaveGame()
    {
        if (!GameController.instance.gameStoryManager.CanSaveGame()) { return SaveGameStatus.CANT_SAVE; }
        GameData gameData = new GameData();
        gameData.inventoryItemsNameAndAmount = new List<SavedInventoryItem>();
        try
        {
            PlayerController player = PlayerController.Instance;
            foreach (var item in player.inventory)
            {
                var it = new SavedInventoryItem(item.item.name, item.amount);
                gameData.inventoryItemsNameAndAmount.Add(it);
            }
            gameData.curEXP = player.GetCurEXP();
            gameData.lv = player.level;
            gameData.coin = player.GetCoin();
            gameData.curHP = player.curHP;
            gameData.playerPos = player.transform.position;
            gameData.gameActionIndex = GameController.instance.gameStoryManager.GetCurGameActionIndex();
            gameData.gameDialogIndex = GameController.instance.gameStoryManager.GetcurGameDialogIndex();
            gameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            gameData.weaponInventoryIndex = player.weaponInventoryIndex;
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string jsonData = JsonConvert.SerializeObject(gameData, settings);
            string path = Application.persistentDataPath + "/gamedata.json";
            File.WriteAllText(path, jsonData);
            return SaveGameStatus.SUCCESS;
        }
        catch (Exception e)
        {
            Debug.LogError("Error while saving game: " + e.Message);
            return SaveGameStatus.FAIL;
        }
    }

    public bool LoadGame()
    {
        string path = Application.persistentDataPath + "/gamedata.json";
        if (File.Exists(path))
        {
            try
            {
                string jsonData = File.ReadAllText(path);
                GameData gameData = JsonConvert.DeserializeObject<GameData>(jsonData);

                //SceneManager.LoadScene(gameData.sceneIndex);


                instance.StartCoroutine(LoadSceneAndRestoreState(gameData));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading game: " + e.Message);
                return false;
            }
        }
        return false;
    }
    private IEnumerator LoadSceneAndRestoreState(GameData gameData)
    {
        int sceneIndex = gameData.sceneIndex;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        yield return null;
        GameController.instance.SetGameState(GAME_STATE.FREE_ROAM);
        GameController.instance.gameStoryManager.SetGameActionIndexAndDialogIndex(gameData.gameActionIndex, gameData.gameDialogIndex);
        PlayerController player = PlayerController.Instance;
        player.LoadPlayerATTR(gameData.playerPos, gameData.curHP, gameData.lv, gameData.curEXP, gameData.coin, gameData.inventoryItemsNameAndAmount, gameData.weaponInventoryIndex);
        MainCameraController.instance.SetPlayerTarget();
    }
}
