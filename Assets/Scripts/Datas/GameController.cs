using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[Serializable]
public class SaveScene {
    public string NameScene;
    public Vector3 Pos;

    public SaveScene(string name, Vector3 pos)
    {
        NameScene = name;
        Pos = pos;
    }
}
public class GameController : MonoBehaviour
{
    /* Thể hiện duy nhất cho GameController
     * 
     */
    public static GameController Instance { get; private set; }
    /* Trình điều khiển là duy nhất
     * Mỗi Cảnh sẽ có một Joytick riêng và qua mỗi cảnh sẽ được setJoytick vào 
     */
    public static Joystick MyJoy;
    public static MAP_GamePlay MapGamePlay;
    public BTThaoTacManHinh BTTTMH;
    public static GameObject CanvasMain;
    [SerializeField]
    private GameObject LoadingMain;
    [SerializeField]
    private Slider sli_loading;
    [SerializeField]
    private GameObject MoMan;
    public static string LastScene = "Loading";
    public static Vector3 LastPos = new Vector3(0, 0, 0);
    private void Start()
    {
        LoadingMain.SetActive(false);
        MoMan.SetActive(false);
        Application.targetFrameRate = 60;
        LoadScene("TrangTrai");
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadData();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        SceneManager.sceneLoaded += (Scene, Load) => Languages.UpdateLanguageInScene();
        Languages.LanguageCurrent = Languages.Nation.VietNam;
        Languages.LoadData();
        VFXManager.LoadData();
        ChestManager.LoadData();
        RewardManager.LoadData();
        WeaponManager.LoadData();
        DataMap.LoadData();
    }

    public static string getStringTimeBySeconds(long Giay)
    {

        string s = "";
        if (Giay < 0)
        {
            s += "-";
            Giay *= -1;
        }
        long Gio = Giay / 3600;
        if (Gio < 10)
            s += "0";
        s += Gio.ToString() + ":";
        Giay -= Gio * 3600;
        long Phut = Giay / 60;
        if (Phut < 10)
            s += "0";
        s += Phut.ToString() + ":";
        Giay -= Phut * 60;
        if (Giay < 10)
            s += "0";
        s += Giay.ToString();
        return s;
    }

    IEnumerator LoadAsynchronously(string name)
    {
        LoadingMain.SetActive(true);
        sli_loading.value = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            sli_loading.value = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
        LoadingMain.SetActive(false);
        moMan();
    }
    public static bool isLayerIn(GameObject game, LayerMask layers)
    {


        return (layers | 1 << game.layer) == layers;
    }

    public void LoadScene(string scene)
    {
        if (MAPController.Instance != null)
            LastScene = MAPController.Instance.SceneCurrent; 
        else 
            LastScene = scene;
        StartCoroutine(LoadAsynchronously(scene));
    }
    public static void ClearAllSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public void moMan()
    {
        MoMan.SetActive(true);
    }
}

