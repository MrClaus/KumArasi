using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Button : MonoBehaviour
{
    public UI ui;

    // Click Tutorial in Menu
    public void ClickTutor()
    {
        SaveScene.val = "tutor";
        SceneManager.LoadScene("LoadScene");
    }

    // Click Play in Menu
    public void ClickPlay()
    {
        ui.ClickPlay();
    }

    // Click Retry in Menu-Ready
    public void ClickRetry()
    {
        SaveScene.val = "retry";
        SceneManager.LoadScene("LoadScene");
    }

    // Click Quit in Menu
    public void ClickQuit()
    {
        ui.ClickQuit();
    }

    // Click Back in Menu-Ready and Menu-Done
    public void ClickBack()
    {
        SaveScene.val = "start";
        SceneManager.LoadScene("LoadScene");
    }
}
