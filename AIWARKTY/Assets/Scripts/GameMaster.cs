using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public Text countdownText;
    private int countdownValue = 3;
    public RobotSpawner rS;
    public int life = 5;
    public Image[] lifeImages; // ������� ǥ���ϴ� UI �̹��� �迭
    public GameObject gameOverUI;
    public float timeLeft = 120f;
    public Text timerText; // Ÿ�̸� �ؽ�Ʈ
    public GameObject gameClearUI; //Ŭ���� ui
    public AudioManagerKTY aM;

    void Start()
    {
        Debug.Log("���� ������ Ƚ�� :" + life);
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
        aM.PlayBgm(true);

        StartCoroutine("Timer"); 
        StartCoroutine(rS.SpawnRobot());
    }


    // ������� �����ϴ� �Լ�
    public void DecreaseLife()
    {
        aM.PlaySfx(AudioManagerKTY.Sfx.BarrierDamage);
        life--;
        // ����¿� �ش��ϴ� UI �̹����� ��Ȱ��ȭ
        if (life >= 0 && life < lifeImages.Length)
        {
            lifeImages[life].gameObject.SetActive(false);
        }
        CheckLife();

        Debug.Log("���� ������ Ƚ��:" + life);
    }

    // ������� Ȯ���ϰ� ���� ���� ó���� �ϴ� �Լ�
    private void CheckLife()
    {
        if (life == 0)
        {
            //
            aM.PlaySfx(AudioManagerKTY.Sfx.BarrierBroke);
            StartCoroutine("Delay");
            GameOver();
        }
    }
    IEnumerator Delay()
    {
        //2�� �� ���ӿ���
        yield return new WaitForSeconds(2);
    }
    IEnumerator Timer() // �ڷ�ƾ ����
    {
        while (true) // ���� �ݺ�
        {
            yield return new WaitForSeconds(1); // 1�ʸ���
            timeLeft--; // �ð� ����
        }
    }

    // ���ӿ��� �Լ�
    private void GameOver()
    {
        Debug.Log("Game Over");

        // ��� �ڷ�ƾ ���߱�
        StopAllCoroutines();

        // RobotSpawner �ڷ�ƾ ���߱�
        if (rS != null)
        {
            rS.StopAllCoroutines();
        }

        // ���ӿ��� ui
        gameOverUI.SetActive(true);


        //���� ���� ����
        aM.PlaySfx(AudioManagerKTY.Sfx.GameOver);

        // ���� ����
        /*Time.timeScale = 0f; */
    }

    void GameClear()
    {
        Time.timeScale = 0;
        // ���� Ŭ���� ui 
        gameClearUI.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "Time Left: " + timeLeft; // Ÿ�̸� �ؽ�Ʈ ����

        if (timeLeft <= 0) // �ð��� 0 ���ϸ� Ÿ�̸Ӹ� ����
        {
            StopCoroutine("Timer");
            GameClear();
            aM.PlaySfx(AudioManagerKTY.Sfx.GameClear);
            aM.PlayBgm(false);
        }
    }

}
