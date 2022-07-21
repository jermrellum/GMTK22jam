using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BulletBox : MonoBehaviour
{
    [HideInInspector] public AudioSource loadSound;
    private AudioSource clickSound;
    private AudioSource fireSound;
    public GameObject gunFab;
    private GameObject gg;
    [HideInInspector] public Text urt;
    [HideInInspector] public Text urt2;

    [HideInInspector] public int bulletDelay = -1;
    [HideInInspector] public bool fireTime;
    public int framesWaitAfterBox = 45;
    [SerializeField] private int framesWaitTweenShots = 45;
    private int shotWaitCount = 0;
    [HideInInspector] public bool[] gunClip = new bool[6];
    [HideInInspector] public int clipPosition = 0;
    public int cylinderTurnFrames = 15;
    [HideInInspector] public int ctfCount = 0;

    private GameObject rvr;

    public int bulletCount = 1;
    public int initBulletCount = 1;

    private Renderer rend;
    private void Start()
    {
        GameObject brgo = GameObject.Find("BulletRememberer");
        BulletMember bmc = brgo.GetComponent<BulletMember>();
        int bLeft = bmc.BulletsLeft();

        if(bLeft < bulletCount)
        {
            bulletCount = bLeft;
        }

        gg = Instantiate(gunFab, new Vector3(-0.41f, 0.65f, -8.57f), Quaternion.identity);
        rend = GetComponentInChildren<Renderer>();

        GameObject ur = GameObject.Find("You rolled");
        urt = ur.GetComponent<Text>();

        GameObject gp = GameObject.Find("Get promoted");
        urt2 = gp.GetComponent<Text>();

        GameObject lss = GameObject.Find("loadSoundSource");
        loadSound = lss.GetComponent<AudioSource>();
        GameObject css = GameObject.Find("clickSoundSource");
        clickSound = css.GetComponent<AudioSource>();
        GameObject fss = GameObject.Find("fireSoundSource");
        fireSound = fss.GetComponent<AudioSource>();

        rvr = GameObject.Find("Revolver"); 

        for (int i=0; i<gunClip.Length; i++)
        {
            gunClip[i] = false;
        }
        MakeBulletText();

        if(bulletCount == 0)
        {
            urt.text = "";
            urt2.text = "We're out of bullets. Have a promotion.";
            bmc.showDie = true;
            bmc.ResetBullets();
            fireTime = true;
            shotWaitCount = 150;
        }
    }

    public void MakeBulletText()
    {
        string theS = "s";

        if (bulletCount == 1)
        {
            theS = "";
        }

        urt.text = "Take " + bulletCount + " bullet" + theS;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (fireTime)
            {
                if (shotWaitCount == 0)
                {
                    GunFire();
                }
            }
        }

        if(shotWaitCount == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    private void FixedUpdate()
    {
        if (ctfCount > 0)
        {
            ctfCount--;

            float rotateAmt = 60.0f / cylinderTurnFrames;

            rvr.transform.rotation = Quaternion.Euler(new Vector3(rvr.transform.rotation.eulerAngles.x, rvr.transform.rotation.eulerAngles.y, rvr.transform.rotation.eulerAngles.z + rotateAmt));
        }
        if (bulletDelay > 0)
        {
            bulletDelay--;
        }
        else if (!fireTime && bulletDelay == 0)
        {
            urt.text = "Left-click to fire";

            rvr.transform.localPosition = new Vector3(-0.0659f, 0.108f, -0.65f);
            gg.transform.rotation = Quaternion.Euler(new Vector3(-15.66f, -202.9f, -2.4f));
            gg.transform.position = new Vector3(0.06f, 0.75f, -10.17f);

            fireTime = true;

            clipPosition = Random.Range(0, 6);
        }

        if (fireTime)
        {
            if (shotWaitCount > 0)
            {
                shotWaitCount--;
            }
        }
    }

    private void GunFire()
    {
        if(gunClip[clipPosition])
        {
            fireSound.Play();
            urt.text = "";
            shotWaitCount = -1;
            GameObject cam = GameObject.Find("Main Camera");
            cam.transform.position = new Vector3(0.0f, 0.0f, 9.0f);
        }
        else
        {
            clipPosition = (clipPosition + 1) % 6;
            clickSound.Play();
            shotWaitCount = framesWaitTweenShots;
        }
    }
}
