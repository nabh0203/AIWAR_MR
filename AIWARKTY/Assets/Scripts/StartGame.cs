//~Generic : List�� ���� ���׸� �÷��� Ŭ������ ����ϱ� ����
using System.Collections.Generic;
using UnityEngine;
//OculusSampleFramework : OVRGrabbable Ŭ���� ���� ����ϱ� ����
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
