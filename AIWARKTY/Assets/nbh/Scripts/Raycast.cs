using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    [SerializeField] private GameObject firePos;  //총알 생성 위치
    public Transform FirePos;//총알 생성 위치
    public GameObject bulletprefab; //발사할 총알
    private LineRenderer laserLine; // 레이저 포인터
    public GameObject bullet; //발사할 총알이펙트
    public GameObject bullet2; //발사할 총알이펙트
    public float laserLength = 50.0f; // 레이저의 길이
    public LaserUpgrade bulletUpgrade;
    public GameObject LaserEF; //발사할 레이저 이펙트
    public AudioManagerKTY aM;
    public bool LaserItem = false;
    public Gauge gauge;

    void Start()
    {
        //해당 스크립트를 넣은 오브젝트의 라인 렌더러 컴포넌트를 찾아 레이저를 그려주게 한다.
        laserLine = GetComponent<LineRenderer>();
        if (laserLine == null)
        {
            laserLine = this.gameObject.AddComponent<LineRenderer>();
            laserLine.material = new Material(Shader.Find("Standard"));
            laserLine.startColor = Color.red;
            laserLine.endColor = Color.red;
            laserLine.startWidth = 0.01f;
            laserLine.endWidth = 0.01f;
        }
        laserLine.positionCount = 2;
    }
    void Update()
    {
        //trigger 누를 때
        if ((LaserItem == false && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)))
        {
            TriggerShoot();
            aM.PlaySfx(AudioManagerKTY.Sfx.Shoot);
            Debug.Log("누름");
            //Quaternion.Euler 함수는 오일러 각을 파라미터로 받아서 Quaternion을 반환하는 함수
            GameObject projectile = Instantiate(bullet2, FirePos.position, Quaternion.Euler(90f, FirePos.rotation.eulerAngles.y, FirePos.rotation.eulerAngles.z));
            Destroy(projectile, 1f);
            GameObject projectile2 = Instantiate(bullet, FirePos.position, Quaternion.Euler(90f, FirePos.rotation.eulerAngles.y, FirePos.rotation.eulerAngles.z));
            Destroy(projectile2, 1f);

        }
        else if (LaserItem == true && OVRInput.Get(OVRInput.Button.One))
        {
             //레이저 게이지가 다 찼으면 A 버튼을 눌러 레이저를 5초간 사용하게 해주는 로직
            gauge.ResetImages();
            //레이저 효과음을 재생한다.
            aM.PlaySfx(AudioManagerKTY.Sfx.Laser);
            Debug.Log("레이저");
            StartCoroutine(RunLaserForSeconds(5f));
            GameObject projectile3 = Instantiate(LaserEF, FirePos.position, FirePos.rotation);
            Destroy(projectile3, 0.1f);
        }


        // firePos의 앞에서 레이저를 그려주는 함수
        DrawLaser(firePos.transform.position, firePos.transform.position + firePos.transform.forward * laserLength);
    }

    public void TriggerShoot()
    {
        Instantiate(bulletprefab, FirePos.position, FirePos.rotation);

        //레이저를 그려주는 코드
        Ray ray = new Ray(firePos.transform.position, firePos.transform.forward);
        DrawLaser(ray.origin, ray.origin + ray.direction * 50);
    }

    public void LasorActive()
    {
        LaserItem = true;
    }
    public void Laser()
    {
        //레이캐스트를 통해 레이저를 발사 하여 레이캐스트에 Hit(닿은) Robot 태그 오브젝트는 파괴 되는 로직
        Ray ray = new Ray(firePos.transform.position, firePos.transform.forward);
        DrawLaser(ray.origin, ray.origin + ray.direction * 50);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.gameObject.tag == "Robot")
            {
                Destroy(hitInfo.transform.gameObject);
            }
        }
    }

    IEnumerator RunLaserForSeconds(float seconds)
    {
        //StartCoroutine(RunLaserForSeconds(5f)); 코루틴 시작 함수를 통해 5초간 레이저를 사용하게 해주는 로직
        //5초가 지나면 LaserItem = false;를 통해 레이저에서 기본 공격으로 바뀐다.
        StartCoroutine(CallLaserForSeconds(seconds));
        yield return new WaitForSeconds(seconds);
        LaserItem = false;
    }

    IEnumerator CallLaserForSeconds(float seconds)
    {
        //지정한 시간 만큼 Laser();가 실행되게 해주는 코드
        float endTime = Time.time + seconds;

        while (Time.time < endTime)
        {
            Laser();
            yield return null;
        }
    }

    void DrawLaser(Vector3 startPosition, Vector3 endPosition)
    {
        // 레이저를 그리는 역할을 하는 함수 startPosition,endPosition의 인자값을 받아 레이저의 시작과 끝을 설정하여 레이저를 그려준다.
        laserLine.SetPosition(0, startPosition);
        laserLine.SetPosition(1, endPosition);
    }
}
