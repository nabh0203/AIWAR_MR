using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text countdownText;
    private int countdownValue = 3;
    public RobotSpawner rS;
    public int life = 5;
    public static GameManager instance;
    public Image[] lifeImages; // 생명력을 표시하는 UI 이미지 배열
    public GameObject gameOverUI;
    public float timeLeft = 120f;
    public Text timerText; // 타이머 텍스트
    public GameObject gameClearUI; //클리어 ui

    /*
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }*/
    void Start()
    {
        Debug.Log("남은 라이프 횟수 :" + life);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (countdownValue > 0)
        {
            countdownText.text = countdownValue.ToString();
            yield return new WaitForSeconds(1f);
            countdownValue--;
        }

        countdownText.text = "Start!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        StartCoroutine("Timer"); 
        StartCoroutine(rS.SpawnRobot());
    }


    // 생명력이 감소하는 함수
    public void DecreaseLife()
    {

        life--;
        // 생명력에 해당하는 UI 이미지를 비활성화
        if (life >= 0 && life < lifeImages.Length)
        {
            lifeImages[life].gameObject.SetActive(false);
        }
        CheckLife();

        Debug.Log("남은 라이프 횟수:" + life);
    }

    // 생명력을 확인하고 게임 오버 처리를 하는 함수
    private void CheckLife()
    {
        if (life == 0)
        {
            GameOver();
        }
    }

    IEnumerator Timer() // 코루틴 정의
    {
        while (true) // 무한 반복
        {
            yield return new WaitForSeconds(1); // 1초마다
            timeLeft--; // 시간 감소
        }
    }

    // 게임오버 함수
    private void GameOver()
    {
        Debug.Log("Game Over");

        // 모든 코루틴 멈추기
        StopAllCoroutines();

        // RobotSpawner 코루틴 멈추기
        if (rS != null)
        {
            rS.StopAllCoroutines();
        }

        // 게임오버 ui
         gameOverUI.SetActive(true);
         

        // 게임 정지
        /*Time.timeScale = 0f; */
    }

    void GameClear()
    {
        Time.timeScale = 0;
        // 게임 클리어 ui 
        gameClearUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "Time Left: " + timeLeft; // 타이머 텍스트 갱신

        if (timeLeft <= 0) // 시간이 0 이하면 타이머를 중지
        {
            StopCoroutine("Timer");
            GameClear();
        }
    }

}
