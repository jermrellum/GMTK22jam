using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    private Renderer rend;
    private bool rolled = false;
    private bool landed = false;
    private int diceSoundCountDelay = 20;
    private int diceSoundCount = -1;
    private bool diceSoundPlayed = false;
    private Rigidbody rb;

    [SerializeField] private int framesToWaitAfterRoll = 0;
    private int waitDelay;
    [SerializeField] private float yThrowMin = 3.5f;
    [SerializeField] private float yThrowMax = 6.5f;
    [SerializeField] private float zThrowMin = 1.5f;
    [SerializeField] private float zThrowMax = 2.5f;
    [SerializeField] private float dieSpawnY = 0.0f;
    [SerializeField] private float dieSpawnZ = -10.0f;

    [SerializeField] private GameObject boxFab;

    public Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new Vector2(6.0f, 0.0f);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;
    }

    private void OnMouseEnter()
    {
        if (!rolled)
        {
            rend.material.color = Color.yellow;
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    private void OnMouseExit()
    {
        rend.material.color = Color.red;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    private void OnMouseDown()
    {
        if (!rolled)
        {
            RollDie();
        }
    }

    private void RollDie()
    {
        float rotX = Random.Range(0, 360);
        float rotY = Random.Range(0, 360);
        float rotZ = Random.Range(0, 360);

        transform.position = new Vector3(0.0f, dieSpawnY, dieSpawnZ);
        transform.rotation = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));

        float xThrow = Random.Range(-0.2f, 0.2f);
        float yThrow = Random.Range(yThrowMin, yThrowMax);

        float yThrowDiff = yThrowMax - yThrowMin;
        float yThrowPercent = (yThrow - yThrowMin) / yThrowDiff;
        float zThrowDiff = zThrowMax - zThrowMin;
        float zThrow = zThrowMin + zThrowDiff * (1 - yThrowPercent);

        rb.velocity = new Vector3(xThrow, yThrow, zThrow);

        float avX = Random.Range(1, 500);
        float avY = Random.Range(1, 500);
        float avZ = Random.Range(1, 500);

        rb.AddTorque(avX, avY, avZ);

        rolled = true;
        diceSoundCount = diceSoundCountDelay;
        
    }

    private int GetLandedOn(int dx, int dz)
    {
        switch (dx)
        {
            case 180: return 5;
            case 270: return 4;
            case 90: return 3;
            case 0: 
                switch(dz)
                {
                    case 0: return 2;
                    case 90: return 6;
                    case 270: return 1;
                    case 180: return 5;

                    default: return 0;
                }
            default: return 0;
        }
    }

    private void FixedUpdate()
    {
        /*
         *  x  0,  0,    0,  0
            y  n,  n,    n,  n
            z 90, -90, 180,  0

            n  1,  6,    5,  2
         * 
         * */

        if(diceSoundCount > 0)
        {
            diceSoundCount--;
        }

        if (!diceSoundPlayed && diceSoundCount == 0 && transform.position.y < 0.8f)
        {
            diceSoundPlayed = true;
            GameObject dss = GameObject.Find("diceSoundSource");
            AudioSource diceSound = dss.GetComponent<AudioSource>();
            diceSound.Play();
        }

        if (rolled && rb.velocity.y == 0 && rb.velocity.z == 0)
        {
            if(!landed)
            {
                landed = true;
                waitDelay = framesToWaitAfterRoll;
            }

            if (waitDelay == 0)
            {
                int dx = Mathf.RoundToInt(transform.rotation.eulerAngles.x);
                int dz = Mathf.RoundToInt(transform.rotation.eulerAngles.z);

                int rollValue = GetLandedOn(dx, dz);

                GameObject ur = GameObject.Find("You rolled");

                Text urt = ur.GetComponent<Text>();
                urt.text = "Take " + rollValue + " bullets";

                GenerateBoxAndGun(rollValue);
            }
            else
            {
                waitDelay--;
            }
        }
    }
    private void GenerateBoxAndGun(int bullets)
    {
        GameObject box = Instantiate(boxFab, new Vector3(0.17f, 0.5f, -9.1f), Quaternion.identity);
        BulletBox bb = box.GetComponent<BulletBox>();
        bb.bulletCount = bullets;
        bb.initBulletCount = bullets;
        Destroy(this.gameObject);
    }
}
