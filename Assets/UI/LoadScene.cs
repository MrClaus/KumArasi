using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    void Start()
    {
        SaveScene.isGameScene = false;
        Invoke(((SaveScene.val == "tutor") ? "Tutor" : "Goto"), 1.5f);        
    }

    void Goto()
    {
        SceneManager.LoadScene("GameScene");
    }

    void Tutor()
    {
        SceneManager.LoadScene("TutorScene");
    }
}