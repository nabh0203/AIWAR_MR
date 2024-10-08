using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    [SerializeField] private GameObject firePos;  //�Ѿ� ���� ��ġ
    public Transform FirePos;//�Ѿ� ���� ��ġ
    public GameObject bulletprefab; //�߻��� �Ѿ�
    private LineRenderer laserLine; // ������ ������
    public GameObject bullet; //�߻��� �Ѿ�����Ʈ
    public GameObject bullet2; //�߻��� �Ѿ�����Ʈ
    public float laserLength = 50.0f; // �������� ����
    public LaserUpgrade bulletUpgrade;
    public GameObject LaserEF; //�߻��� ������ ����Ʈ
    public AudioManagerKTY aM;
    public bool LaserItem = false;
    public Gauge gauge;

    void Start()
    {
        //�ش� ��ũ��Ʈ�� ���� ������Ʈ�� ���� ������ ������Ʈ�� ã�� �������� �׷��ְ� �Ѵ�.
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
        //trigger ���� ��
        if ((LaserItem == false && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)))
        {
            TriggerShoot();
            aM.PlaySfx(AudioManagerKTY.Sfx.Shoot);
            Debug.Log("����");
            //Quaternion.Euler �Լ��� ���Ϸ� ���� �Ķ���ͷ� �޾Ƽ� Quaternion�� ��ȯ�ϴ� �Լ�
            GameObject projectile = Instantiate(bullet2, FirePos.position, Quaternion.Euler(90f, FirePos.rotation.eulerAngles.y, FirePos.rotation.eulerAngles.z));
            Destroy(projectile, 1f);
            GameObject projectile2 = Instantiate(bullet, FirePos.position, Quaternion.Euler(90f, FirePos.rotation.eulerAngles.y, FirePos.rotation.eulerAngles.z));
            Destroy(projectile2, 1f);

        }
        else if (LaserItem == true && OVRInput.Get(OVRInput.Button.One))
        {
             //������ �������� �� á���� A ��ư�� ���� �������� 5�ʰ� ����ϰ� ���ִ� ����
            gauge.ResetImages();
            //������ ȿ������ ����Ѵ�.
            aM.PlaySfx(AudioManagerKTY.Sfx.Laser);
            Debug.Log("������");
            StartCoroutine(RunLaserForSeconds(5f));
            GameObject projectile3 = Instantiate(LaserEF, FirePos.position, FirePos.rotation);
            Destroy(projectile3, 0.1f);
        }


        // firePos�� �տ��� �������� �׷��ִ� �Լ�
        DrawLaser(firePos.transform.position, firePos.transform.position + firePos.transform.forward * laserLength);
    }

    public void TriggerShoot()
    {
        Instantiate(bulletprefab, FirePos.position, FirePos.rotation);

        //�������� �׷��ִ� �ڵ�
        Ray ray = new Ray(firePos.transform.position, firePos.transform.forward);
        DrawLaser(ray.origin, ray.origin + ray.direction * 50);
    }

    public void LasorActive()
    {
        LaserItem = true;
    }
    public void Laser()
    {
        //����ĳ��Ʈ�� ���� �������� �߻� �Ͽ� ����ĳ��Ʈ�� Hit(����) Robot �±� ������Ʈ�� �ı� �Ǵ� ����
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
        //StartCoroutine(RunLaserForSeconds(5f)); �ڷ�ƾ ���� �Լ��� ���� 5�ʰ� �������� ����ϰ� ���ִ� ����
        //5�ʰ� ������ LaserItem = false;�� ���� ���������� �⺻ �������� �ٲ��.
        StartCoroutine(CallLaserForSeconds(seconds));
        yield return new WaitForSeconds(seconds);
        LaserItem = false;
    }

    IEnumerator CallLaserForSeconds(float seconds)
    {
        //������ �ð� ��ŭ Laser();�� ����ǰ� ���ִ� �ڵ�
        float endTime = Time.time + seconds;

        while (Time.time < endTime)
        {
            Laser();
            yield return null;
        }
    }

    void DrawLaser(Vector3 startPosition, Vector3 endPosition)
    {
        // �������� �׸��� ������ �ϴ� �Լ� startPosition,endPosition�� ���ڰ��� �޾� �������� ���۰� ���� �����Ͽ� �������� �׷��ش�.
        laserLine.SetPosition(0, startPosition);
        laserLine.SetPosition(1, endPosition);
    }
}
