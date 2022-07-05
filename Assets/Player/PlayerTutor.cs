using System.Collections;
using UnityEngine;

public class PlayerTutor : MonoBehaviour
{
    private Animator anim;
    private bool isControlAnimating, isDone;
    private int currentTutor = 0;

    [Header("Game Objects")]
    public Animator parent;
    public Animator popup;
    public GameObject[] tutorArrow;
    public GameObject[] tutorInfo;

    // Type of player animations and their execution time
    private enum TypeAnim
    {
        Left = 750, Right = 749, Roll = 1000, Jump = 700
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning", true);
    }


    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (!isControlAnimating && !isDone)
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

            if (h > 0 || isSwipeRight)
            {
                anim.SetTrigger("toRight");
                StartCoroutine(WaitAnimation(TypeAnim.Right));
            }
            else if (h < 0 || isSwipeLeft)
            {
                anim.SetTrigger("toLeft");
                StartCoroutine(WaitAnimation(TypeAnim.Left));
            }
            else if (v < 0 || isSwipeBottom)
            {
                anim.SetTrigger("toRoll");
                StartCoroutine(WaitAnimation(TypeAnim.Roll));
            }
            else if (Input.GetKey(KeyCode.Space) || isSwipeTop)
            {
                anim.SetTrigger("toJump");
                parent.SetTrigger("toUp");
                StartCoroutine(WaitAnimation(TypeAnim.Jump));
            }
        }
    }


    private IEnumerator WaitAnimation(TypeAnim type)
    {
        float time = (int)type / 1000f;
        isControlAnimating = true;        
        yield return new WaitForSeconds(time);
        CheckTutor(type);
        isControlAnimating = false;
    }


    private void CheckTutor(TypeAnim type)
    {
        if ((type == TypeAnim.Right && tutorArrow[currentTutor].name == "arrow_right") ||
            (type == TypeAnim.Left && tutorArrow[currentTutor].name == "arrow_left") ||
            (type == TypeAnim.Jump && tutorArrow[currentTutor].name == "arrow_up") ||
            (type == TypeAnim.Roll && tutorArrow[currentTutor].name == "arrow_down"))
        {            
            tutorArrow[currentTutor].SetActive(false);
            if (currentTutor < 3)
                tutorInfo[currentTutor].SetActive(false);
            currentTutor++;
        }
        if (currentTutor > 3)
        {
            isDone = true;
            popup.Play("TutorPopupShow");
        }
        else
        {
            tutorArrow[currentTutor].SetActive(true);
            tutorInfo[currentTutor].SetActive(true);
        }
    }
}
