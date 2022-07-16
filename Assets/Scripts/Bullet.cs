using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new(6.0f, 0.0f);
    public Texture2D cursorTexture;
    private BulletBox bb;

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
        if (!bb.fireTime && bb.bulletCount > 0)
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

            string theS = "s";

            if (bb.bulletCount == 1)
            {
                theS = "";
            }

            bb.urt.text = "Take " + bb.bulletCount + " bullet" + theS;

            Destroy(this.gameObject);
        }
    }
}
