using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private new Renderer renderer;
    private bool unrolled;

    private void OnMouseEnter()
    {
        renderer.material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }

    private void OnMouseDown()
    {
        
    }
}
