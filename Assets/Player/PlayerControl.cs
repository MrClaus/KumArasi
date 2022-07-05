using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private int score = 0;
    private int scoreVal;
    private Animator anim;
    private Pathways pathways;
    private bool isLoose = true;
    private bool isControlAnimating;

    [Header("Player Parent Default")]
    public Animator parent;

    [Header("UI Component")]
    public UI ui;

    // Type of player animations and their execution time
    private enum TypeAnim {
        Left = 750, Right = 750, Roll = 1000, Jump = 700
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        pathways = Pathways.Instance();

        if (PlayerPrefs.HasKey("KumArasiScore"))
        {
            scoreVal = PlayerPrefs.GetInt("KumArasiScore");
        }
    }


    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (ui.isStart && isLoose)
        {
            anim.SetBool("isRunning", true);
            isLoose = false;
            pathways.Run();

            StartCoroutine(Scoring());            
        }

        if (ui.isStart && !isControlAnimating && !isLoose)
        {
            bool isSwipeRight = false, isSwipeLeft = false, isSwipeTop = false, isSwipeBottom = false;

            if (Input.touchCount > 0)
            {
                Vector2 delta = Input.GetTouch(0).deltaPosition;
                bool m_h = (Mathf.Abs(delta.x) > Mathf.Abs(delta.y));
                isSwipeRight = (m_h && delta.x > 0);
                isSwipeLeft = (m_h && delta.x < 0);
                isSwipeTop = (!m_h && delta.y > 0);
                isSwipeBottom = (!m_h && delta.y < 0);
            }

            if (h > 0 || isSwipeRight) {
                anim.SetTrigger("toRight");
                StartCoroutine(WaitAnimation(TypeAnim.Right));
                pathways.RightMoving();

            } else if (h < 0 || isSwipeLeft) { 
                anim.SetTrigger("toLeft");
                StartCoroutine(WaitAnimation(TypeAnim.Left));
                pathways.LeftMoving();

            } else if (v < 0 || isSwipeBottom) { 
                anim.SetTrigger("toRoll");
                StartCoroutine(WaitAnimation(TypeAnim.Roll));
                pathways.StopMoving(2f);

            } else if (Input.GetKey(KeyCode.Space) || isSwipeTop) {
                anim.SetTrigger("toJump");
                parent.SetTrigger("toUp");
                StartCoroutine(WaitAnimation(TypeAnim.Jump));
            }                
        }
    }


    // Checking character collisions during the game
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("barrier") && !isLoose)
        {
            isLoose = true;            
            pathways.StopMoving();
            anim.SetTrigger("toLoose");
            ui.isStart = false;
            ui.OpenReady();

            if (score > scoreVal)
            {
                ui.highScore.text = "" + score;
                PlayerPrefs.SetInt("KumArasiScore", score);
                PlayerPrefs.Save();                
            }            
        }
    }


    // Character animation process
    private IEnumerator WaitAnimation(TypeAnim type)
    {
        float time = (int)type / 1000f;
        isControlAnimating = true;
        yield return new WaitForSeconds(time);
        isControlAnimating = false;        

        if (type == TypeAnim.Roll && !isLoose)
            pathways.StartMoving(time);
    }


    // Scoring during the game
    private IEnumerator Scoring()
    {
        ui.score.text = "0";
        ui.inter.Play("Score");

        while (!isLoose)
        {
            yield return new WaitForSeconds(1f);
            if (!isLoose)
            {
                score++;
                ui.score.text = "" + score;
            }            
        }
    }
}
