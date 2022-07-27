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
    [SerializeField] private GameObject friendPrefab;

    [SerializeField] private float countdownMinProp;
    [SerializeField] private float countdownMaxProp;
    [SerializeField] private int countdownThreshold;

    [SerializeField] private float mailAlerMinProp;
    [SerializeField] private float mailAlerMaxProp;
    [SerializeField] private int mailAlertThreshold;
    [SerializeField] private int repliesBeforeDud = 12;

    private GameObject screen;
    private int framesForCount = 540;

    public float popX = 9.0f;
    public float popY = 6.0f;
    public float popZ = -1.5f;
    public float popAY = 50.0f;

    public int framesWaitOnBoss = 140;
    public int fwobCount = 0;
    private bool bossAppears = false;

    private int repCount = 0;

    private int popcount = 0;
    private BulletMember bmc;

    private AudioSource alertSound;

    private void Awake()
    {
        GameObject die = GameObject.Find("Die");
        GameObject brgo = GameObject.Find("BulletRememberer");
        bmc = brgo.GetComponent<BulletMember>();
        repCount = bmc.repliesCount;

        if(!bmc.showDie)
        {
            Destroy(die);
        }
    }

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
                bool friend = false;
                GameObject toInst = popUpPrefab;
                if(repCount % repliesBeforeDud == (repliesBeforeDud - 1))
                {
                    if (!bmc.firstRound)
                    {
                        toInst = friendPrefab;
                        friend = true;
                    }
                    else
                    {
                        GenerateDud();
                    }
                    repCount = 0;
                }
                GameObject newpop = Instantiate(toInst, new Vector3(popX, popY, popZ), Quaternion.Euler(new Vector3(0.0f, popAY, 0.0f)), screen.transform);

                popcount++;

                float xMin = -2.6f;
                float xMax = 2.9f;

                float zMin = -3.3f;
                float zMax = 4.0f;

                float xRand = Random.Range(xMin, xMax);
                float zRand = Random.Range(zMin, zMax);

                newpop.transform.localPosition = new Vector3(xRand, -0.6f + popcount * 0.02f, zRand);
                newpop.transform.localEulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
                newpop.transform.localScale = new Vector3(3.0f, 1.37f, 1.37f);

                alertSound.Play();

                if (!friend)
                {
                    Popup pc = newpop.GetComponent<Popup>();
                    pc.countDown = framesForCount;
                }
                framesForCount = Random.Range((int)(framesForCount * countdownMinProp), (int)(framesForCount * countdownMaxProp));
                if (framesForCount < countdownThreshold)
                {
                    framesForCount = countdownThreshold;
                }

                cdtpMax = Random.Range((int)(cdtpMax * mailAlerMinProp), (int)(cdtpMax * mailAlerMaxProp));
                if (cdtpMax < mailAlertThreshold)
                {
                    cdtpMax = mailAlertThreshold;
                }
                countDownToPopup = cdtpMax;
            }
        }
    }

    public int GenerateDud()
    {
        int liveIdx = bmc.FindRandomLive();
        if(liveIdx >= 0)
        {
            bmc.Dudify(liveIdx);
        }
        return liveIdx;
    }

    public void IncRepCount()
    {
        repCount++;
    }

    public void TriggerFailure()
    {
        if (!bossAppears)
        {
            fwobCount = framesWaitOnBoss;
            alertSound.Play();

            bmc.repliesCount = repCount;
            GameObject bosspop = Instantiate(bossPrefab, new Vector3(popX, popY, popZ), Quaternion.Euler(new Vector3(0.0f, popAY, 0.0f)), screen.transform);

            bosspop.transform.localPosition = new Vector3(0.0f, -0.6f + popcount * 0.001f, 0.0f);
            bosspop.transform.localEulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
            bosspop.transform.localScale = new Vector3(3.0f, 1.37f, 1.37f);

            bossAppears = true;
        }
    }
}
