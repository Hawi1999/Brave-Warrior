using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Teleporttion
{
    public bool isVisible = true;
    public SceneGame ConnectScene;
    public string Name_Scene;
    public string Info_BT;
    public Vector3 PositionGo;
    public Vector3 PositionBack;
}

public class TeleportScene : MonoBehaviour
{
    private SceneGame ConnectScene;
    [SerializeField]
    GoTo EventE;
    private BTThaoTacManHinh BT_Current;
    private string TextInfo;
    // Start is called before the first frame update
    void Start()
    {
        EventE.OnGoIn += GoIn;
        EventE.OnGoOut += GoOut;
    }

    private void GoIn(Collider2D c)
    {
        if (BT_Current != null) return;
        BT_Current = Instantiate(GameController.Instance.BTTTMH, GameController.CanvasMain.transform);
        BT_Current.gameObject.SetActive(true);
        BT_Current.AddButton(0, TextInfo, () => GameController.Instance.LoadScene(ConnectScene));
    }

    private void LoadScene(SceneGame scene)
    {
        if (PlayerController.Instance.HasWeapon)
        {
            GameController.Instance.LoadScene(ConnectScene);
        } else
        {
            ThongBao.NhacNho("Bạn cần vũ khí để tiếp tục");
        }
    }

    private void GoOut(Collider2D c)
    {
        if (BT_Current == null) return;
        Destroy(BT_Current.gameObject);
    }

    public void SetUp(Teleporttion tele)
    {

        ConnectScene = tele.ConnectScene;
        TextInfo = tele.Info_BT;
        EventE.vanBan.text = tele.Name_Scene;
        gameObject.transform.position = tele.PositionGo;
    }
}
