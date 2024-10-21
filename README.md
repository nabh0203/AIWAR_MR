AIWAR_MR
=============
![스크린샷(450)](https://github.com/user-attachments/assets/7c67aabb-80ce-4dbe-8e4f-0569f8838ba3)

**사용 기술**

- Unity
- C#
- XR Interaction ToolKit
- VR

## “Value”를발견하다.
![Frame 215](https://github.com/user-attachments/assets/2eab1593-d429-4174-8c4a-f37a2002c7a1)

> **VR 과 내가 가진 “ 가치(Value) ”  를 발견 해준 콘텐츠**
>
> 계원예술대학교 졸업 막바지에 진행하였던 **“AIWar”** 라는 프로젝트 입니다.
>
> **VR** 의 기능중 **Pass-Through** 기능과 **MR** 을 사용하여 제작한 **VR 콘텐츠** 입니다.
>
> 해당 콘텐츠는 **VR** 이 가진 ***“가치”*** 와 제가 가지고 있는 능력의 ***“가치”*** 를
>
> 발견하여 앞으로의 진로에 대한 자신감을 심어준 콘텐츠 입니다.
---

## VR의 숨겨진 기능

> 해당 콘텐츠는 **MR** 기술을 사용해서 제작한 콘텐츠 입니다.
>
> **MR** 이란 기능은 아직까지 개발중인 기능이며 현실과 가상이 혼합된
>
> 혼합 현실이라고도 불립니다.
>
> 이러한 기능은 **Pass-Through** 기능을 통해 제작하고 사용할 수 있습니다.
>
> 해당 프로젝트를 통해 사용자에게 콘텐츠를 체험시켜 줌으로써 가상과 현실을 아우르는 
> 
> **VR** 의 가치를 선사하고 제가 정한 진로의 가치를 확실시 할수 있었습니다.
> 
 

---

## **개발 과정**

### 1. GameMaster

> 게임의 전체적인 로직을 담당하는 스크립트입니다.
>
> 오디오,UI,Life 시스템,스폰 로직을

### Raycast

```csharp
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

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **Raycast 스크립트 제작중 중점 사항**

> **“학도”** 프로젝트의 **Raycast** 스크립트를 수정하여 제작한 스크립트입니다.
해당 스크립트는 **사격**과 일정 게이지가 차면 **특수 무기**로 바뀌는 로직을 구성한
스크립트입니다.
> 
> 
> 
> 해당 스크립트 변수부터 살펴 보겠습니다.
> 
> ```csharp
> [SerializeField] private GameObject firePos;  //총알 생성 위치
> public Transform FirePos;//총알 생성 위치
> public GameObject bulletprefab; //발사할 총알
> private LineRenderer laserLine; // 레이저 포인터
> public GameObject bullet; //발사할 총알이펙트
> public GameObject bullet2; //발사할 총알이펙트
> public float laserLength = 50.0f; // 레이저의 길이
> public LaserUpgrade bulletUpgrade;
> public GameObject LaserEF; //발사할 레이저 이펙트
> public AudioManagerKTY aM;
> public bool LaserItem = false;
> public Gauge gauge;
> ```
> 
> **firePos** 변수
> 
> 총알이 생성되는 위치를 지정하기 위한 변수입니다.
> 
> ---
> 
> **FirePos** 변수
> 
> 총알이 발사되는 이펙트의 생성 위치를 지정하기 위한 변수입니다.
> 
> ---
> 
> **bulletprefab** 변수
> 
> 생성되는 총알 모델링을 지정해주는 변수입니다.
> 
> ---
> 
> **laserLine** 변수입니다.
> 
> 해당 변수는 가상의 선이 제대로 적용되고 있는지 파악하기 위하여 **Unity** 에
> 존재하는 **LineRenderer** 속성을 사용하여 레이저 포인터를 표현하기 위해
> 설정하였습니다.
> 
> ---
> 
> 다음은 **bullet** , **bullet2** 변수입니다.
> 
> 사격시 보이는 **Particle** 오브젝트를 설정해주는 변수입니다.
> 
> ---
> 
> **laserLength** 변수
> 
> **laserLine** 변수와 연결되는 변수이며 그려지는 레이저의 길이를 설정하는 
> 변수입니다.
> 
> ---
> 
> **bulletUpgrade** 변수
> 
> 스크립트 **LaserUpgrade**를 참조하는 변수입니다.
> 해당 변수를 통해 특수 무기로 교체되는 로직을 구현할수 있습니다.
> 
> ---
> 
> **LaserEF** 변수
> 
> 특수무기의 이펙트를 지정하는 변수입니다.
> 
> ---
> 
> **aM** 변수
> 
> 해당 변수는 모든 오디오를 관리해주는 스크립트인 **AudioManagerKTY** 를 참조
> 해주는 변수입니다.
> 
> ---
> 
> **LaserItem** 변수
> 
> 특수무기로 변환하게 해주는 변수입니다.
> 
> ---
> 
> **gauge** 변수
> 
> **Gauge** 스크립트를 참조해주는 변수입니다.
> 
> ---
> 
> **Start()** 함수
> 
> ```csharp
>  void Start()
>     {
>         //해당 스크립트를 넣은 오브젝트의 라인 렌더러 컴포넌트를 찾아 레이저를 그려주게 한다.
>         laserLine = GetComponent<LineRenderer>();
>         if (laserLine == null)
>         {
>             laserLine = this.gameObject.AddComponent<LineRenderer>();
>             laserLine.material = new Material(Shader.Find("Standard"));
>             laserLine.startColor = Color.red;
>             laserLine.endColor = Color.red;
>             laserLine.startWidth = 0.01f;
>             laserLine.endWidth = 0.01f;
>         }
>         laserLine.positionCount = 2;
>     }
> ```
> 
> 실행 시 **LineRenderer** 를 컴포넌트 하여 **Unity** 내에서 레이저를 그려줍니다.
> 해당 코드에대한 자세한 설명은 [**“학도”**](https://www.notion.so/19ec12036a454b848fbeb60431ce1801?pvs=21) **Script** 부분에 **Raycast** 부분을 참고해주세요
> 
> ---
> 
> **Update()** 함수
> 
> ```csharp
> void Update()
>     {
>         //trigger 누를 때
>         if ((LaserItem == false && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)))
>         {
>             TriggerShoot();
>             aM.PlaySfx(AudioManagerKTY.Sfx.Shoot);
>             Debug.Log("누름");
>             //Quaternion.Euler 함수는 오일러 각을 파라미터로 받아서 Quaternion을 반환하는 함수
>             GameObject projectile = Instantiate(bullet2, FirePos.position, Quaternion.Euler(90f, FirePos.rotation.eulerAngles.y, FirePos.rotation.eulerAngles.z));
>             Destroy(projectile, 1f);
>             GameObject projectile2 = Instantiate(bullet, FirePos.position, Quaternion.Euler(90f, FirePos.rotation.eulerAngles.y, FirePos.rotation.eulerAngles.z));
>             Destroy(projectile2, 1f);
> 
>         }
>         else if (LaserItem == true && OVRInput.Get(OVRInput.Button.One))
>         {
>              //레이저 게이지가 다 찼으면 A 버튼을 눌러 레이저를 5초간 사용하게 해주는 로직
>             gauge.ResetImages();
>             //레이저 효과음을 재생한다.
>             aM.PlaySfx(AudioManagerKTY.Sfx.Laser);
>             Debug.Log("레이저");
>             StartCoroutine(RunLaserForSeconds(5f));
>             GameObject projectile3 = Instantiate(LaserEF, FirePos.position, FirePos.rotation);
>             Destroy(projectile3, 0.1f);
>         }
> 
>         // firePos의 앞에서 레이저를 그려주는 함수
>         DrawLaser(firePos.transform.position, firePos.transform.position + firePos.transform.forward * laserLength);
>     }
> ```
> 
> 매 프레임마다 실행되는 **Update()** 함수입니다.
> 
> 로직을 설명드리겠습니다.
> 
> > **1.**  **If 문**의 조건이 모두 부합하다면 **If 문**안에 명령들을 실행합니다.
> **(**변수 **LaserItem** 의 값이 **거짓**이고 ****오른쪽 컨트롤러의 트리거 버튼을 눌렀을때 **)**
> > 
> > 
> > **2.**  **TriggerShoot()** 함수를 실행하고 **Shoot** 오디오를 재생합니다.
> > 
> > **3.**  **"누름"** 을 디버깅하여 출력합니다.
> > 
> > **4.**  총알 이펙트 2가지를 변수 **FirePos** 위치에 생성한뒤 **1초** 뒤에 파괴합니다.
> > 
> > **5.  else** **If 문**을 사용하여 **LaserItem** 의 값이 참이고 오른쪽 컨트롤러의 A 버튼
> >     을 누른다면 명령들을 실행합니다.
> > 
> > **6.  Gauge** 스크립트의 **ResetImages()** 함수를 실행합니다.
> > 
> > **7.**  **Laser** 오디오를 재생합니다.
> > 
> > **8.**  **"Laser"** 를 디버깅하여 출력한 뒤 **RunLaserForSeconds()** 코루틴을 5초간
> >     사용할수 있게 합니다.
> > 
> > **9.**  변수 **LaserEF** 에 설정한 **Particle** 을  **FirePos** 위치에 생성한뒤 **1초** 뒤에 
> >     파괴합니다.
> > 
> > **10.** 변수 **firePos** 위치에 레이저를 그려줍니다.
> > 
> 
> ---
> 
> **TriggerShoot()** 함수 , **LasorActive()** 함수 ,  **Laser()** 함수 입니다.
> 
> ```csharp
> public void public void TriggerShoot()
>     {
>         Instantiate(bulletprefab, FirePos.position, FirePos.rotation);
> 
>         //레이저를 그려주는 코드
>         Ray ray = new Ray(firePos.transform.position, firePos.transform.forward);
>         DrawLaser(ray.origin, ray.origin + ray.direction * 50);
>     }
> 
>     public void LasorActive()
>     {
>         LaserItem = true;
>     }
>     public void Laser()
>     {
>         //레이캐스트를 통해 레이저를 발사 하여 레이캐스트에 Hit(닿은) Robot 태그 오브젝트는 파괴 되는 로직
>         Ray ray = new Ray(firePos.transform.position, firePos.transform.forward);
>         DrawLaser(ray.origin, ray.origin + ray.direction * 50);
>         RaycastHit hitInfo;
> 
>         if (Physics.Raycast(ray, out hitInfo))
>         {
>             if (hitInfo.transform.gameObject.tag == "Robot")
>             {
>                 Destroy(hitInfo.transform.gameObject);
>             }
>         }
>     }
> ```
> 
> 해당 함수들은 총알을 발사하는 기능과 특수무기로 교체되는 로직을 제작한 
> 함수입니다.
> 
> 먼저 **TriggerShoot()** 함수 로직입니다.
> 
> > **1.**  변수 **bulletprefab** 를 **FirePos** 위치에 복제하여 생성합니다.
> > 
> > 
> > **2.**  변수 **ray** 를 생성하여 레이저와 레이캐스트를 생성합니다.
> > 
> 
> **LasorActive()** 함수는 **특수무기로 교체**하기 위한 조건이 해당되면 실행되는 
> 함수입니다.
> 
> **Laser()** 함수는 특수무기로 교체되어 사용자가 발사하는 레이저를 구현한 
> 함수입니다.
> 
> 로직을 설명드리겠습니다.
> 
> > **1.**  앞서 생성한 레이캐스트를 재 생성합니다.
> > 
> > 
> > **2.**  레이캐스트에 닿은 **"Robot"** 태그가 붙은 오브젝트를 파괴합니다.
> > 
> 
> ---
> 
> **RunLaserForSeconds() 코루틴** 함수,**CallLaserForSeconds() 코루틴** 함수입니다.
> 
> ```csharp
>  IEnumerator RunLaserForSeconds(float seconds)
>     {
>         //StartCoroutine(RunLaserForSeconds(5f)); 코루틴 시작 함수를 통해 5초간 레이저를 사용하게 해주는 로직
>         //5초가 지나면 LaserItem = false;를 통해 레이저에서 기본 공격으로 바뀐다.
>         StartCoroutine(CallLaserForSeconds(seconds));
>         yield return new WaitForSeconds(seconds);
>         LaserItem = false;
>     }
> 
>     IEnumerator CallLaserForSeconds(float seconds)
>     {
>         //지정한 시간 만큼 Laser();가 실행되게 해주는 코드
>         float endTime = Time.time + seconds;
> 
>         while (Time.time < endTime)
>         {
>             Laser();
>             yield return null;
>         }
>     }
> ```
> 
> 해당 **코루틴 함수**들은 지정한 시간동안 레이저를 사용하게해주는 **코루틴 함수**입니다.
> 
> 먼저 **RunLaserForSeconds() 코루틴** 함수 로직입니다.
> 
> > **1.**  **코루틴**이 실행되면 **CallLaserForSeconds() 코루틴** 을 매개변수 값 시간만큼 
>     실행시킵니다.
> > 
> > 
> > **2.**  **WaitForSeconds** 의 매개변수 값 시간 뒤에 다시 돌아옵니다.
> > 
> > **3.**  변수 **LaserItem** 의 값을 **거짓**으로 바꿉니다
> > 
> 
> **CallLaserForSeconds() 코루틴** 함수 로직입니다.
> 
> > **1.**  **코루틴**이 실행되면 **endTime** 만큼만 실행되게 하는 변수를 생성합니다.
> > 
> > 
> > **2.**  **While** 문 조건인 변수 **Time** 이  **endTime** 보다 작다면 함수 **Laser()** 를 실행
> >     합니다.
> > 
> > **3.**  그 다음 지정한 시간이 끝난다면 반복문을 빠져나옵니다.
> > 
> 
> ---
> 
> **DrawLaser()**함수
> 
> ```csharp
> void DrawLaser(Vector3 startPosition, Vector3 endPosition)
>     {
>         // 레이저를 그리는 역할을 하는 함수 startPosition,endPosition의 인자값을 받아 레이저의 시작과 끝을 설정하여 레이저를 그려준다.
>         laserLine.SetPosition(0, startPosition);
>         laserLine.SetPosition(1, endPosition);
>     }
> ```
> 
> 해당 함수는 **레이저**를 그려주는 함수입니다.
> **startPosition** 에서부터 **endPosition** 까지의 두께가 얇은 레이저를 그려줍니다.
> 
</aside>

### StartGame

```csharp
//~Generic : List와 같은 제네릭 컬렉션 클래스를 사용하기 위함
using System.Collections.Generic;
using UnityEngine;
//OculusSampleFramework : OVRGrabbable 클래스 등을 사용하기 위함
using OculusSampleFramework;

public class StartGame : OVRGrabbable
{
    public List<GameObject> objectsToActivate;
    public GameObject Logo;
    private OVRGrabber hand;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        this.hand = hand;

        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        Invoke("ReleaseAndDeactivate", 0.1f);
    }

    void ReleaseAndDeactivate()
    {
        hand.ForceRelease(this);
        gameObject.SetActive(false);
        Logo.SetActive(false);
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **StartGame 스크립트 제작중 중점 사항**

> 사용자는 눈 앞에 있는 총 오브젝트를 잡아야 게임이 실행되는 로직을 구현한
스크립트 입니다.
> 
> 
> 먼저 **NameSpace** 부분 부터 살펴보겠습니다.
> 
> ```csharp
> //~Generic : List와 같은 제네릭 컬렉션 클래스를 사용하기 위함
> using System.Collections.Generic;
> using UnityEngine;
> //OculusSampleFramework : OVRGrabbable 클래스 등을 사용하기 위함
> using OculusSampleFramework;
> ```
> 
> 먼저 **~Generic** 입니다. 
> 
> **~Generic** 는 **List** 와 같은 제너릭 컬렉션 클래스를 사용할수 있게 해줍니다.
> 기본적으로 스크립트 생성시 작성되어 있습니다.
> 
> ---
> 
> **OculusSampleFramework**
> 
> **Oculus SDK** 중 **OVRGrabbable** 클래스 등 **Oculus** 관련 기능을 사용하기 위해 
> 작성하였습니다.
> 
> ---
> 
> 해당 스크립트 변수부터 살펴 보겠습니다.
> 
> ```csharp
> public List<GameObject> objectsToActivate;
> public GameObject Logo;
> private OVRGrabber hand;
> ```
> 
> **objectsToActivate** 변수
> 
> **활성화** 될 오브젝트를 지정하는 변수입니다.
> 
> ---
> 
> **Logo** 변수
> 
> 로고를 지정하는 변수입니다.
> 
> ---
> 
> **hand** 변수
> 
> **OVRGrabber** 스크립트를 참조하는 변수이며 잡는 동작을 수행한 손의 정보를 
> 저장하는 변수입니다.
> 
> ---
> 
> **GrabBegin()** 함수
> 
> ```csharp
>  public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
>     {
>         base.GrabBegin(hand, grabPoint);
>         this.hand = hand;
> 
>         foreach (GameObject obj in objectsToActivate)
>         {
>             obj.SetActive(true);
>         }
> 
>         Invoke("ReleaseAndDeactivate", 0.1f);
>     }
> ```
> 
> 해당 함수는 사용자가 지정한 오브젝트를 잡았을시 **objectsToActivate** 변수의
> 오브젝트들을 **활성화** 시키기 위한 함수입니다.
> 
> 로직을 설명 드리겠습니다.
> 
> > **1.  OVRGrabbable** 스크립트를 참조하여 **오버라이드**합니다.
> > 
> > 
> > **2.**  **GrabBegin 메서드**를 호출해 기본 동작을 수행합니다.
> > 
> > **3.  foreach** 문을 사용하여 **objectsToActivate** 변수에서 지정한 오브젝트들을
> >     활성화 합니다.
> > 
> > 4.  **ReleaseAndDeactivate()** 함수를 0.1 초 뒤에 실행 시킵니다.
> > 
> 
> ---
> 
> **ReleaseAndDeactivate()** 함수
> 
> ```csharp
> void ReleaseAndDeactivate()
>     {
>         hand.ForceRelease(this);
>         gameObject.SetActive(false);
>         Logo.SetActive(false);
>     }
> ```
> 
> 해당 스크립트는 손에 붙어 있는 오브젝트를 놓게 한뒤 변수 **Logo** 와 함께
> **비활성화** 합니다.
> 
> 로직을 설명드리겠습니다.
> 
> > **1.**  **ForceRelease** 함수를 사용하여 사용자가 현재 잡고있는 오브젝트를 강제로
>     놓게 합니다.
> > 
> > 
> > **2.**  해당 스크립트가 컴포넌트 되어있는 오브젝트를 **비활성화** 합니다.
> > 
> > **3.**  변수 **Logo** 로 지정한 오브젝트를 **비활성화** 합니다.
> > 
> 
> ---
> 
</aside>

### RobotSpawner

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    public GameObject RobotPrefab;
    public List<Transform> SpawnPoints;
    public float minSpawnDelay = 3f;
    public float maxSpawnDelay = 7f;   
    public RobotController rC;

    public IEnumerator SpawnRobot()
    {

        while (true)
        {
            int randomIndex = Random.Range(0, SpawnPoints.Count);
            Transform spawnPoint = SpawnPoints[randomIndex];
            GameObject Robot = Instantiate(RobotPrefab, spawnPoint.position, spawnPoint.rotation);

            //로봇과 로봇 컨트롤러에게 게이지와 게임마스터 스크립트를 변수를 통해 참조해준다.
            rC = Robot.GetComponent<RobotController>();
            rC.gm = FindObjectOfType<GameMaster>();
            rC.gauge = FindObjectOfType<Gauge>();
            rC.aM = FindObjectOfType<AudioManagerKTY>();

            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **RobotSpawner 스크립트 제작중 중점 사항**

> Robot 즉 적군을 생성해내는 스크립트입니다.
> 
> 
> 
> 해당 스크립트 변수부터 살펴 보겠습니다.
> 
> ```csharp
> public GameObject RobotPrefab;
> public List<Transform> SpawnPoints;
> public float minSpawnDelay = 3f;
> public float maxSpawnDelay = 7f;   
> public RobotController rC;
> ```
> 
> **RobotPrefab** 변수
> 
> 생성되는 로봇의 모델링을 지정하는 변수입니다.
> 
> ---
> 
> **SpawnPoints** 변수
> 
> 로봇이 생성되는 위치를 지정하는 변수이며 **List<>** 함수를 사용하여 더 많은 생성
> 위치를 지정할수 있습니다.
> 
> ---
> 
> **minSpawnDelay**,**maxSpawnDelay** 변수
> 
> 로봇이 재 생성되기 까지 걸리는 시간의 최솟값과 최대값의 변수입니다.
> 
> ---
> 
> **rC** 변수
> 
> **RobotController** 스크립트를 참조하여 생성되는 로봇에 관여해주는 변수입니다. 
> 
> ---
> 
> **SpawnRobot()** **코루틴** 함수
> 
> ```csharp
>  public IEnumerator SpawnRobot()
>     {
> 
>         while (true)
>         {
>             int randomIndex = Random.Range(0, SpawnPoints.Count);
>             Transform spawnPoint = SpawnPoints[randomIndex];
>             GameObject Robot = Instantiate(RobotPrefab, spawnPoint.position, spawnPoint.rotation);
> 
>             //로봇과 로봇 컨트롤러에게 게이지와 게임마스터 스크립트를 변수를 통해 참조해준다.
>             rC = Robot.GetComponent<RobotController>();
>             rC.gm = FindObjectOfType<GameMaster>();
>             rC.gauge = FindObjectOfType<Gauge>();
>             rC.aM = FindObjectOfType<AudioManagerKTY>();
> 
>             float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
>             yield return new WaitForSeconds(spawnDelay);
>         }
>     }
> ```
> 
> **코루틴** 함수를 사용하여 반복적으로 **로봇을 생성하는 명령**을 가진 함수입니다.
> 
> 로직을 살펴보겠습니다.
> 
> > **1.**  **While 문**을 사용하여 매개변수의 값이 **참**일때 반복실행합니다.
> > 
> > 
> > **2.**  **randomIndex** 변수를 사용하여 랜덤으로 **SpawnPoints** 중 하나의 위치에서
> >     로봇을 생성합니다.
> > 
> > **3.**  생성되는 로봇은 변수 **rC**인 **RobotController** 스크립트를 참조합니다.
> > 
> > **4.  rC.gm** , **rC.gauge** , **rC.aM**  총 3가지의 스크립트를 참조하여 로봇의 
> >     상호작용을 가능하게 해줍니다.
> > 
> > **5.  spawnDelay** 변수를 설정하여 **minSpawnDelay**, **maxSpawnDelay** 값의    
> >     사이값 중 랜덤값을 지정합니다.
> > 
> > **6.  WaitForSeconds** 을 사용하여 다음 로봇이 생성되기까지의 대기시간을
> >     **spawnDelay** 변수값 으로 지정하여 대기시킵니다.
> > 
> 
> ---
> 
</aside>

### Bullet

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 2000.0f; // 여기에서 원하는 스피드 값을 지정할 수 있습니다.
    public bool Laser;
    public LaserUpgrade LaserItem;
    public GameObject Hit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Robot") || other.CompareTag("Laser"))
        {
            Destroy(gameObject);
            GameObject projectile = Instantiate(Hit, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(projectile, 1f);
        }
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **Bullet 스크립트 제작중 중점 사항**

> 사격시 발사되는 총알 스크립트 입니다.
> 
> 
> 
> 해당 스크립트 변수부터 살펴 보겠습니다.
> 
> ```csharp
>   private Rigidbody rb;
>   public float speed = 2000.0f; // 여기에서 원하는 스피드 값을 지정할 수 있습니다.
>   public bool Laser;
>   public LaserUpgrade LaserItem;
>   public GameObject Hit;
> ```
> 
> **rb** 변수
> 
> **Rigidbody** 속성을 참조하는 변수입니다.
> 
> ---
> 
> **speed** 변수
> 
> 총알의 속도를 지정하는 변수입니다.
> 
> ---
> 
> **Laser** 변수
> 
> 특수무기에서 기본무기로 전환될떄 사용되는 **Bool** 형 변수입니다.
> 
> ---
> 
> **LaserItem** 변수
> 
> **LaserUpgrade** 스크립트를 참조하는 변수입니다.
> 
> ---
> 
> **Hit** 변수
> 
> 총알이 **Robot** 오브젝트에 충돌하였을때 생기는 **Particle** 을 지정한 변수입니다. 
> 
> ---
> 
> **Start()** 함수
> 
> ```csharp
>  void Start()
>     {
>         rb = GetComponent<Rigidbody>();
>         rb.AddRelativeForce(Vector3.forward * speed);
>     }
> ```
> 
> 실행 시 해당 스크립트가 컴포넌트 되어있는 오브젝트에게 **Rigidbody** 속성과
> **Rigidbody** 속성의 **AddRelativeForce** 을 부여하는 함수입니다.
> 
> ---
> 
> **OnTriggerEnter()** 함수
> 
> ```csharp
> private void OnTriggerEnter(Collider other)
>     {
>         if(other.CompareTag("Robot") || other.CompareTag("Laser"))
>         {
>             Destroy(gameObject);
>             GameObject projectile = Instantiate(Hit, gameObject.transform.position, gameObject.transform.rotation);
>             Destroy(projectile, 1f);
>         }
>     }
> ```
> 
> 총알이 오브젝트와 **Trigger** 시 실행되는 함수입니다.
> 
> 로직을 간략하게 설명 드리겠습니다.
> 
> > **1.**  **If 문**을 사용하여 **“Robot”**,**”Laser”** 태그가 붙은 오브젝트와 **Trigger** 시 
>     명령문을 실행합니다.
> > 
> > 
> > **2.**  **Trigger** 된 오브젝트를 파괴합니다.
> > 
> > **3.**  **projectile** 변수를 지정하여 **Trigger** 된 오브젝트 위치에서 변수 **Hit** 을 
> >     생성합니다.
> > 
> > **4.  projectile** 변수를 1초 뒤에 파괴합니다.
> > 
> 
> ---
> 
</aside>

### LaserUpgrade

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUpgrade : MonoBehaviour
{
    public bool LaserItem = false;
    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("레이저 업그레이드");
            Destroy(gameObject);
            LaserItem = true;
        }
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **LaserUpgrade 스크립트 제작중 중점 사항**

> 특수무기로 교체하게 해주는 스크립트 입니다.
> 
> 
> 
> 해당 스크립트 변수부터 살펴 보겠습니다.
> 
> ```csharp
> public bool LaserItem = false;
> ```
> 
> **LaserItem** 변수
> 
> 특수무기로 교체하기 위한 변수이며 실행시 값이 거짓으로 지정하였습니다.
> 
> ---
> 
> **OnTriggerEnter()** 함수
> 
> ```csharp
> private void OnTriggerEnter(Collider other)
>     {
>         if (other.CompareTag("Bullet"))
>         {
>             Debug.Log("레이저 업그레이드");
>             Destroy(gameObject);
>             LaserItem = true;
>         }
>     }
> ```
> 
> 오브젝트가 **Trigger** 시 실행되는 함수입니다.
> 
> 로직을 간략하게 설명 드리겠습니다.
> 
> > **1.**  **If 문**을 사용하여 **”Bullet”** 태그가 붙은 오브젝트와 **Trigger** 시 명령문을 
>     실행합니다.
> > 
> > 
> > **2.**  **"레이저 업그레이드"** 디버깅하여 출력합니다.
> > 
> > **3.**  스크립트가 부착된 오브젝트를 파괴합니다.
> > 
> > **4.  LaserItem** 변수의 값을 참으로 변환합니다.
> > 
> 
> ---
> 
</aside>

### Gauge

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    private int lasor = 0;
    public Image[] gImages; 
    public GameObject pressAUI;
    public Raycast raycast;
    public AudioManagerKTY aM;

    private void Start()
    {
        
        Debug.Log("Lasor: " + lasor + ", gImages.Length: " + gImages.Length);
    }
    public void GaugeFill()
    {
        Debug.Log("Before GaugeFill - Lasor: " + lasor);
        if (lasor < 10 && lasor < gImages.Length)
        {
            gImages[lasor].gameObject.SetActive(true);
            lasor++;
        }
        Debug.Log("After GaugeFill - Lasor: " + lasor);
        CheckGauge();
    }

    void CheckGauge()
    {
        if (lasor == 10)
        {
            //게이지 참 사운드재생
            aM.PlaySfx(AudioManagerKTY.Sfx.Gauge);
            pressAUI.SetActive(true);
            raycast.LasorActive();
        }

    }

    public void ResetImages()
    {
        for (int i = 0; i < gImages.Length; i++)
        {
            gImages[i].gameObject.SetActive(false);
        }
        lasor = 0;
        pressAUI.SetActive(false);
    }

}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **Gauge 스크립트 제작중 중점 사항**

> 앞서 설명드린 **LaserUpgrade** 가 부착된 오브젝트를 파괴시 게이지가 차게되고
게이지가 다 차게되면 특수무기로 교체할수 있게 해주는 스크립트입니다.
> 
> 
> 
> 변수를 보시겠습니다.
> 
> ```csharp
> private int lasor = 0;
> public Image[] gImages; 
> public GameObject pressAUI;
> public Raycast raycast;
> public AudioManagerKTY aM;
> ```
> 
> **lasor** 변수
> 
> 게이지의 갯수를 지정한 변수이며 초기값은 0 입니다.
> 
> ---
> 
> **gImages** 변수
> 
> 사용자에게 게이지가 차고있는걸 시각적으로 나타내기 위한 **Image** 배열 변수입니다.
> 
> ---
> 
> **pressAUI** 변수
> 
> 사용자에게 **A** 버튼을 눌러 특수무기를 사용하라는 **UI**를 지정하기 위한 변수입니다.
> 
> ---
> 
> **raycast** 변수
> 
> **Raycast** 스크립트를 참조하는 변수입니다.
> 
> ---
> 
> **aM** 변수
> 
> **AudioManagerKTY** 스크립트를 참조하는 변수입니다.
> 
> ---
> 
> **Start()** 함수
> 
> ```csharp
> private void Start()
>     {
>         Debug.Log("Lasor: " + lasor + ", gImages.Length: " + gImages.Length);
>     }
> ```
> 
> 실행 시 **Lasor** 의 갯수 **gImages** 의 길이를 디버깅 하는 함수입니다.
> 
> ---
> 
> **GaugeFill()** 함수
> 
> ```csharp
> public void GaugeFill()
>     {
>         Debug.Log("Before GaugeFill - Lasor: " + lasor);
>         if (lasor < 10 && lasor < gImages.Length)
>         {
>             gImages[lasor].gameObject.SetActive(true);
>             lasor++;
>         }
>         Debug.Log("After GaugeFill - Lasor: " + lasor);
>         CheckGauge();
>     }
> ```
> 
> **LaserUpgrade** 를 부착된 오브젝트가 파괴될시 게이지가 올라가는것을 보여주기
> 위한 함수입니다.
> 
> 로직을 간략하게 설명 드리겠습니다.
> 
> > **1.**  함수 실행시 디버깅하여 **"Before GaugeFill - Lasor: " + lasor** 를 
>     출력합니다.
> > 
> > 
> > **2.**  **If 문**을 사용하여 변수 **lasor** 의 값이 **10**보다 작고 **gImages** 길이 보다 
> >     작으면 명령문을 실행합니다.
> > 
> > **3.**  **gImages** 변수의 이미지 중 하나를 **활성화** 하고 변수 **lasor** 값을 **+1** 합니다.
> > 
> > **4.  "After GaugeFill - Lasor: " + lasor** 디버깅 합니다.
> > 
> > **5.**  **CheckGauge()** 를 실행합니다.
> > 
> 
> ---
> 
</aside>

### Barrier

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameMaster GM;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Robot"))
        {
            Destroy(other.gameObject);
            GM.DecreaseLife();
        }
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **Barrier 스크립트 제작중 중점 사항**

> 사용자의 라이프 대신 **Barrier** 라는 오브젝트를 설정하여 **Barrier** 가 깨진다면
**GameMaster** 스크립트의 **DecreaseLife()** 함수를 실행하는 스크립트입니다.
> 
> 
> 
> 변수
> 
> ```csharp
> public GameMaster GM;
> ```
> 
> **GM** 변수
> 
> **GameMaster** 스크립트를 참조할수 있게 해주는 변수입니다.
> 
> ---
> 
> **OnTriggerEnter()** 함수
> 
> ```csharp
> private void OnTriggerEnter(Collider other)
>     {
>         if (other.gameObject.CompareTag("Robot"))
>         {
>             Destroy(other.gameObject);
>             GM.DecreaseLife();
>         }
>     }
> ```
> 
> 해당 스크립트가 **Trigger** 시에 실행되는 함수입니다.
> 
> 로직을 간략하게 설명 드리겠습니다.
> 
> > **1.**  **If 문**을 사용하여 **Robot** 태그가 붙은 오브젝트와 **Trigger** 된다면 명령문을
>     실행합니다.
> > 
> > 
> > **2.**  해당 스크립트와 **Trigger** 된 오브젝트를 파괴합니다.
> > 
> > **3.**  변수 **GM** 을 통해 **GameMaster** 스크립트의 **DecreaseLife()** 함수를 
> >     실행합니다.
> > 
> 
> ---
> 
</aside>

### Reset

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("다시시작");
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("진짜다시시작");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        
    }
}

```

<aside>
<img src="/icons/snippet_gray.svg" alt="/icons/snippet_gray.svg" width="40px" /> **Reset 스크립트 제작중 중점 사항**

> 해당 스크립트가 컴포넌트된 오브젝트가 **Trigger** 시에 **Scene**을 다시 불러와 
다시시작하게 해주는 스크립트 입니다. 바로 로직을 설명드리겠습니다.
> 
> 
> 
> **OnTriggerEnter()** 함수
> 
> ```csharp
>  private void OnTriggerEnter(Collider other)
>     {
>         Debug.Log("다시시작");
>         if (other.CompareTag("Bullet"))
>         {
>             Debug.Log("진짜다시시작");
>             Scene currentScene = SceneManager.GetActiveScene();
>             SceneManager.LoadScene(currentScene.name);
>         }
>         
>     }
> ```
> 
> > **1.**  **"다시시작"** 을 디버깅하여 출력합니다.
> > 
> > 
> > **2.**  **If 문**을 사용하여 **Bullet** 태그가 붙은 오브젝트와 **Trigger** 된다면 명령문을
> >     실행합니다.
> > 
> > **3.  "진짜다시시작"** 을 디버깅하여 출력합니다.
> > 
> > **4.**  **Scene** 속성의 변수 **currentScene** 를 지정합니다.
> > 
> > **5.  GetActiveScene()** 함수를 실행하여 현재 **Scene** 의 정보를 저장합니다.
> > 
> > **6.**  **LoadScene()** 함수를 실행하여 현재 **Scene**을 불러와 다시시작합니다.
> > 
> 
> ---
> 
</aside>
