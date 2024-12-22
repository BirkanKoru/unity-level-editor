using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private LevelScreen levelPrefab;
    private GameObject LevelContainer = null;

    private LevelScreen currLevelScreen = null;

    public void SetupGame(int levelIndex)
    {
        if(LevelContainer == null) CreateLevelContainer();
        else ClearLevelContainer();

        CreateLevel(levelIndex);
    }

    private void CreateLevelContainer()
    {
        LevelContainer = new GameObject();
        LevelContainer.transform.name = "LevelContainer";
        LevelContainer.transform.SetParent(this.transform);
        LevelContainer.transform.localPosition = Vector3.zero;
    }

    private void ClearLevelContainer()
    {
        if(currLevelScreen != null)
        {
            //SAFE DESTROY
            LevelScreen lastLevelScreen = currLevelScreen;
            lastLevelScreen.transform.localPosition = new Vector3(0f, 100f, 0f);
            Destroy(lastLevelScreen.gameObject, 0.1f);
        }
    }

    private void CreateLevel(int levelIndex)
    {
        if(FileManager.Instance.LoadDataFromLocal((levelIndex + 1)))
        {
            currLevelScreen = Instantiate(levelPrefab, LevelContainer.transform);
            currLevelScreen.transform.localPosition = Vector3.zero;

            currLevelScreen.SetupLevel(levelIndex);
        }
    }
}
