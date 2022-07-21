using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new(6.0f, 0.0f);
    public Texture2D cursorTexture;
    private BulletBox bb;
    [SerializeField] private GameObject bulletInRevolver;
    [SerializeField] private int bulletIdx;

    private BulletMember bmc;

    private void Awake()
    {
        GameObject brgo = GameObject.Find("BulletRememberer");
        bmc = brgo.GetComponent<BulletMember>();

        if(!bmc.DoIExist(bulletIdx))
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        bb = GetComponentInParent<BulletBox>();
    }

    private void OnMouseEnter()
    {
        if (bb.bulletCount > 0)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    private void OnMouseDown()
    {
        if (!bb.fireTime && bb.bulletCount > 0 && bb.ctfCount == 0)
        {
            bb.bulletCount--;
            bb.loadSound.Play();
            bb.gunClip[bb.clipPosition] = true;
            bb.clipPosition++;

            if (bb.bulletCount == 0)
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                bb.bulletDelay = bb.framesWaitAfterBox;
            }

            bb.MakeBulletText();

            GameObject rvr = GameObject.Find("Revolver");

            Instantiate(bulletInRevolver, new Vector3(-0.4137f, 0.776136f, -9.235f), Quaternion.identity, rvr.transform);

            bb.ctfCount = bb.cylinderTurnFrames;

            bmc.TakeBullet(bulletIdx);

            Destroy(this.gameObject);
        }
    }
}
