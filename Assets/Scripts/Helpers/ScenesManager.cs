using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    public GameObject Win_Screen;
    public GameObject Loss_Screen;
    public GameObject Title_Screen;
    public GameObject LevelIndicator_Screen;
    public TextMeshProUGUI LevelText;
    private bool Title_ScreenIsActive = true;
    public Image[] LevelIndicator;
    public Color OffLevelIndicatorColor;
    public Color OnLevelIndicatorColor;
    public int[] RoomsIndex;
    public GameObject[] Prefaps;
    private GameObject ActiveRoom;
    private int PrefapIndex;
    private PlayerManager player;
    int PlayerLevel=1;
    void Awake(){
        Instance=this;
        Load(Tag.Player);
        SetActive_Loss_Screen(false);
        SetActive_Win_Screen(false);
        FillRoomsIndex();
        LoadRoom(0);
    }
    void Start()
    {
        LevelIndicator_Screen.SetActive(false);
        if (PlayerPrefs.HasKey("PlayerLevel")){

            PlayerLevel = PlayerPrefs.GetInt("PlayerLevel");
        }else{
            
            PlayerPrefs.SetInt("PlayerLevel",1);
        }
        player=FindObjectOfType<PlayerManager>();
        LevelText.text="Level "+PlayerLevel;
    }
    private void Update() {
        if(Input.GetMouseButton(0)&&Title_ScreenIsActive){
            
            Title_Screen.SetActive(false);
            Title_ScreenIsActive=false;
            LevelIndicator_Screen.SetActive(true);
            player.StartGame();
        }
    }
    public void UpdateLevelIndicator(int ReachedRoom){
        for (int i = ReachedRoom - 1; i >= 0 ; i--){
            LevelIndicator[i].color=OnLevelIndicatorColor;
        }
        for (int x = ReachedRoom; x<3;x++){
            
            LevelIndicator[x].color=OffLevelIndicatorColor;
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
        
        //UpdateLevelIndicator();
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
        UpdateLevelIndicator(0);
    }
    public void WinButton(){
        PlayerLevel++;
        PlayerPrefs.SetInt("PlayerLevel",PlayerLevel);
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
