using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject start;
    public Animator inter;
    public Text highScore;
    public Text score;    

    [Header("Game Objects")]
    public Pathways pathways;

    [Header("Other")]
    public bool isStart;


    private void Awake()
    {
        SaveScene.isGameScene = true;

        if (SaveScene.val == "retry")
        {
            inter.Play("InterfaceEntry");
            ClickPlayAfter();
        }
        else if (SaveScene.val == "start")
        {
            inter.Play("InterfaceShow");
        }        
        if (PlayerPrefs.HasKey("KumArasiScore"))
        {
            highScore.text = "" + PlayerPrefs.GetInt("KumArasiScore");
        }
    }


    public void ClickPlay()
    {
        inter.Play("InterfaceHide");
        Invoke("ClickPlayAfter", 1.1f);
    }


    private void ClickPlayAfter()
    {
        start.SetActive(true);
        Invoke("ClickPlayEnd", 7f);
        score.text = "";
    }


    private void ClickPlayEnd()
    {
        start.SetActive(false);
        isStart = true;
    }


    public void ClickQuit()
    {
        inter.Play("Quit");
        Invoke("ClickQuitAfter", 0.5f);
    }


    private void ClickQuitAfter()
    {
        Application.Quit();
    }


    public void OpenReady()
    {
        inter.Play("ReadyShow");
    }


    private void OnApplicationQuit()
    {
        SaveScene.isGameScene = false;
    }
}
