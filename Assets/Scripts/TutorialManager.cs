using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public GameObject Win_Screen;
    public GameObject Loss_Screen;
    public Image[] LevelIndicator;
    public Color LevelIndicatorColor;
    public int[] RoomsIndex;
    public GameObject[] Prefaps;
    private GameObject ActiveRoom;
    private int PrefapIndex;
    private PlayerManager player;
    void Awake(){
        Instance=this;
        Load(Tag.Player);
        SetActive_Loss_Screen(false);
        SetActive_Win_Screen(false);
        LoadRoom(0);
    }
    void Start(){
        player=FindObjectOfType<PlayerManager>();
    }
    public void UpdateLevelIndicator(int ReachedRoom){
        for (int i = ReachedRoom - 1; i >= 0 ; i--){
            LevelIndicator[i].color=LevelIndicatorColor;
        }
    }
    public void Load (string SceneName){
        if (!SceneManager.GetSceneByName(SceneName).isLoaded)
            SceneManager.LoadScene(SceneName,LoadSceneMode.Additive);
    }
    public void UnLoad (string SceneName){
        if (SceneManager.GetSceneByName(SceneName).isLoaded)
            SceneManager.UnloadSceneAsync(SceneName);
    }
    public void LoadRoom (int Index){
        if (ActiveRoom!=null){
            Destroy(ActiveRoom);
        }
        ActiveRoom = Instantiate(Prefaps[RoomsIndex[Index]],transform.position,Quaternion.identity);
    }
    public void SetActive_Loss_Screen(bool value){
        Loss_Screen.SetActive(value);
    }
    public void SetActive_Win_Screen(bool value){
        Win_Screen.SetActive(value);
    }
    public void LoseButton(){
        SetActive_Loss_Screen(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void WinButton(){
        Debug.Log("Win");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
