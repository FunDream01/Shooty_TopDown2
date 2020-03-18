using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    public GameObject Win_Screen;
    public GameObject Loss_Screen;
    public int[] RoomsIndex;
    public GameObject[] Prefaps;
    private GameObject ActiveRoom;
    private int PrefapIndex;
    private PlayerManager player;

    void Awake()
    {
        Instance=this;
        Load(Tag.Player);
        SetActive_Loss_Screen(false);
        SetActive_Win_Screen(false);
        FillRoomsIndex();
        LoadRoom(0);
    }
    void Start(){
        player=FindObjectOfType<PlayerManager>();
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
        player.RestartLevel();
        UnLoad(Tag.Player);
        Load(Tag.Player);
        LoadRoom(0);
    }
    public void WinButton(){
        Debug.Log("Win");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void FillRoomsIndex(){
        for (int i = 0; i < RoomsIndex.Length; i++){
            if(i==0){
                RoomsIndex[i] = Random.Range(0, Prefaps.Length);
            }else{
                RoomsIndex[i] =RandomRangeExcept(i);
            }
        }
    }
    int RandomRangeExcept(int index){
        int RandomNumber = Random.Range(0, Prefaps.Length);
        for (int i = index-1; i >-1; i--)
        {
            if (RandomNumber ==RoomsIndex[i]){
                return RandomRangeExcept(index); // recall function
            }
        }
        return RandomNumber;
    }
}
