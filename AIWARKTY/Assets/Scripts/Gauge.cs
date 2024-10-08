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
