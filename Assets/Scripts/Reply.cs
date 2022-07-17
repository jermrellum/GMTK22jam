using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reply : MonoBehaviour
{
    private readonly CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = new(6.0f, 0.0f);
    public Texture2D cursorTexture;

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    private void OnMouseDown()
    {
        Popup pu = GetComponentInParent<Popup>();
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        pu.ReplyMessage();
    }
}
