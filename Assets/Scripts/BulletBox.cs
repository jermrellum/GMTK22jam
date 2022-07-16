using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBox : MonoBehaviour
{

    public Texture2D cursorTexture;
    private AudioSource loadSound;
    private AudioSource clickSound;
    private AudioSource fireSound;
    public GameObject gunFab;
    private GameObject gg;
    private readonly CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new(6.0f, 0.0f);
    private Text urt;

    private int bulletDelay = -1;
    private bool fireTime;
    [SerializeField] private int framesWaitAfterBox = 45;
    [SerializeField] private int framesWaitTweenShots = 20;
    private int shotWaitCount = 0;
    bool[] gunClip = new bool[6];
    private int clipPosition = 0;

    public int bulletCount = 1;
    public int initBulletCount = 1;

    private Renderer rend;
    private void Start()
    {
        gg = Instantiate(gunFab, new Vector3(-0.41f, 0.65f, -8.57f), Quaternion.identity);
        rend = GetComponentInChildren<Renderer>();
        GameObject ur = GameObject.Find("You rolled");
        urt = ur.GetComponent<Text>();
        GameObject lss = GameObject.Find("loadSoundSource");
        loadSound = lss.GetComponent<AudioSource>();
        GameObject css = GameObject.Find("clickSoundSource");
        clickSound = css.GetComponent<AudioSource>();
        GameObject fss = GameObject.Find("fireSoundSource");
        fireSound = fss.GetComponent<AudioSource>();

        for (int i=0; i<gunClip.Length; i++)
        {
            gunClip[i] = false;
        }
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
    }

    private void FixedUpdate()
    {
        if(bulletDelay > 0)
        {
            bulletDelay--;
        }
        else if(!fireTime && bulletDelay == 0)
        {
            urt.text = "Left-click to fire";
            gg.transform.rotation = Quaternion.Euler(new Vector3(-15.66f, -202.9f, -2.4f));
            gg.transform.position = new Vector3(0.06f, 0.75f, -10.17f);

            fireTime = true;

            clipPosition = Random.Range(0, 6);
        }

        if(fireTime)
        {
            if(shotWaitCount > 0)
            {
                shotWaitCount--;
            }
        }
    }

    private void OnMouseEnter()
    {
        if(bulletCount > 0)
        { 
            rend.material.color = Color.yellow;
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    private void OnMouseExit()
    {
        rend.material.color = Color.white;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    private void OnMouseDown()
    {
        if (!fireTime && bulletCount > 0)
        {
            bulletCount--;
            loadSound.Play();
            gunClip[clipPosition] = true;
            clipPosition++;

            if (bulletCount == 0)
            {
                rend.material.color = Color.white;
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                bulletDelay = framesWaitAfterBox;
            }
            
            urt.text = "Take " + bulletCount + " bullets";
        }
    }

    private void GunFire()
    {
        int ayn = Random.Range(1, 6);

        if(gunClip[clipPosition])
        {
            fireSound.Play();
            urt.text = "You died";
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
