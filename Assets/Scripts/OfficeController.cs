using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfficeController : MonoBehaviour
{
    private int countDownToPopup = 180;
    private int cdtpMax = 180;

    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private GameObject bossPrefab;
    private GameObject screen;
    private int framesForCount = 540;

    public float popX = 9.0f;
    public float popY = 6.0f;
    public float popZ = -1.5f;
    public float popAY = 50.0f;

    public int framesWaitOnBoss = 140;
    public int fwobCount = 0;
    private bool bossAppears = false;

    private int popcount = 0;

    private AudioSource alertSound;


    private void Start()
    {
        screen = GameObject.Find("Screen");

        GameObject asnd = GameObject.Find("alertSound");
        alertSound = asnd.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (fwobCount == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (fwobCount > 0)
        {
            fwobCount--;
        }

        if (!bossAppears)
        {
            if (countDownToPopup > 0)
            {
                countDownToPopup--;
            }
            else if (countDownToPopup == 0)
            {
                GameObject newpop = Instantiate(popUpPrefab, new Vector3(popX, popY, popZ), Quaternion.Euler(new Vector3(0.0f, popAY, 0.0f)), screen.transform);

                popcount++;

                float xMin = -3.0f;
                float xMax = 3.0f;

                float zMin = -3.3f;
                float zMax = 4.0f;

                float xRand = Random.Range(xMin, xMax);
                float zRand = Random.Range(zMin, zMax);

                newpop.transform.localPosition = new Vector3(xRand, -0.6f + popcount * 0.001f, zRand);
                newpop.transform.localEulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
                newpop.transform.localScale = new Vector3(3.0f, 1.37f, 1.37f);

                alertSound.Play();

                Popup pc = newpop.GetComponent<Popup>();

                pc.countDown = framesForCount;
                framesForCount = framesForCount * 11 / 13;
                if (framesForCount < 30)
                {
                    framesForCount = 30;
                }

                cdtpMax = Random.Range(cdtpMax * 11 / 16, cdtpMax * 15 / 16);
                if (cdtpMax < 6)
                {
                    cdtpMax = 6;
                }
                countDownToPopup = cdtpMax;
            }
        }
    }

    public void TriggerFailure()
    {
        Debug.Log("gerf");
        if (!bossAppears)
        {
            fwobCount = framesWaitOnBoss;
            alertSound.Play();

            GameObject bosspop = Instantiate(bossPrefab, new Vector3(popX, popY, popZ), Quaternion.Euler(new Vector3(0.0f, popAY, 0.0f)), screen.transform);

            bosspop.transform.localPosition = new Vector3(0.0f, -0.6f + popcount * 0.001f, 0.0f);
            bosspop.transform.localEulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
            bosspop.transform.localScale = new Vector3(3.0f, 1.37f, 1.37f);

            bossAppears = true;
        }
    }
}
