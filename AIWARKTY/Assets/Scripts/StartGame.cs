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
