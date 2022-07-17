using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [HideInInspector] public int countDown;
    [SerializeField] private TextMesh countText;
    private OfficeController occ;

    private void Start()
    {
        GameObject oc = GameObject.Find("OfficeController");
        occ = oc.GetComponent<OfficeController>();
    }

    public void ReplyMessage()
    {
        GameObject ssnd = GameObject.Find("sendSound");
        AudioSource sendSnd = ssnd.GetComponent<AudioSource>();

        sendSnd.Play();

        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        if(occ.fwobCount > 0)
        {
            Destroy(this.gameObject);
        }

        if(countDown > 0)
        {
            countDown--;
            int sec = countDown / 60;
            countText.text = "" + sec;
        }
        else if(countDown == 0)
        {
            occ.TriggerFailure();
        }
    }
}
