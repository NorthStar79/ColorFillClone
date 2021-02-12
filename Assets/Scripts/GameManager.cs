using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int CurrentLVL = 0;  //TODO Get this data from SaveFile

    public LvlGrid ActiveGrid;
    void Start()
    {
        if (instance != null) { Destroy(gameObject); return; }

        instance = this;
        DontDestroyOnLoad(this);

        ActiveGrid = FindObjectOfType<LvlGrid>();
    }

    void LoadLVL(int lvl)
    {
        StartCoroutine(LoadLevel(lvl));

    }

    public void LevelCompleted()
    {
        print(SceneManager.sceneCountInBuildSettings);
        if (CurrentLVL < SceneManager.sceneCountInBuildSettings - 1)
        {
            CurrentLVL++;
            print(CurrentLVL);
            StartCoroutine(LoadLevel(CurrentLVL));
        }
        else
        {
            Debug.Log("asdasdasd");
            StartCoroutine(LoadLevel(0));
        }
    }
    public void LevelFailed(float Delay = 0)
    {
        StartCoroutine(LoadLevel(CurrentLVL,Delay));
    }

    AsyncOperation asyncLoadLevel;

    IEnumerator LoadLevel(int lvl,float delay =0)
    {
        yield return new WaitForSeconds(delay);
        
        asyncLoadLevel = SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        ActiveGrid = FindObjectOfType<LvlGrid>();
        print("Scene Loaded");
    }
}
