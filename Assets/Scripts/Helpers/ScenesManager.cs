using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    public GameObject Win_Screen;
    public GameObject Loss_Screen;
    void Awake()
    {
        Instance=this;
        Load("Player");
        Load("Room 1");
        SetActive_Loss_Screen(false);
        SetActive_Win_Screen(false);
    }
    public void Load (string SceneName){
        if (!SceneManager.GetSceneByName(SceneName).isLoaded)
            SceneManager.LoadScene(SceneName,LoadSceneMode.Additive);
    }
    public void UnLoad (string SceneName){
        if (SceneManager.GetSceneByName(SceneName).isLoaded)
            SceneManager.UnloadSceneAsync(SceneName);
    }
    public void SetActive_Loss_Screen(bool value){
        Loss_Screen.SetActive(value);
    }
    public void SetActive_Win_Screen(bool value){
        Win_Screen.SetActive(value);
    }
    public void LoseButton(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void WinButton(){
        Debug.Log("Win");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
