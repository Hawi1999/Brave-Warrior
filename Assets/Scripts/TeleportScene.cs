using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Teleporttion
{
    public bool isVisible = true;
    public string ConnectScene;
    public string Name_Scene;
    public string Info_BT;
    public Vector3 PositionGo;
    public Vector3 PositionBack;
}

public class TeleportScene : MonoBehaviour
{
    private string ConnectScene;
    [SerializeField]
    GoTo EventE;

    bool inIn = false;
    // Start is called before the first frame update
    void Start()
    {
        EventE.OnGoIn += GoIn;
        EventE.OnGoOut += GoOut;
    }

    private void OnDestroy()
    {
        EventE.OnGoIn -= GoIn;
        EventE.OnGoOut -= GoOut;
    }

    private void Update()
    {
        if (inIn)
        {
            if (Control.GetKeyDown("X"))
            {
                MAPController.Instance.LoadScene(ConnectScene);
            }
        }
    }

    private void GoIn(Collider2D c)
    {
        inIn = true;
        Control.OnWaitToClick?.Invoke("X");
    }


    private void GoOut(Collider2D c)
    {
        inIn = false;
        Control.OnEndWaitToClick?.Invoke("X");
    }

    public void SetUp(Teleporttion tele)
    {
        ConnectScene = tele.ConnectScene;
        EventE.vanBan.text = tele.Name_Scene;
        gameObject.transform.position = tele.PositionGo;
    }
}
