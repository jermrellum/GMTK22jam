using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private Renderer rend;
    private bool unrolled;
    private Rigidbody rb;

    [SerializeField] private float yThrowMin = 3.5f;
    [SerializeField] private float yThrowMax = 6.5f;
    [SerializeField] private float zThrowMin = 1.5f;
    [SerializeField] private float zThrowMax = 2.5f;
    [SerializeField] private float dieSpawnY = 0.0f;
    [SerializeField] private float dieSpawnZ = -10.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;
    }

    private void OnMouseEnter()
    {
        rend.material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        rend.material.color = Color.red;
    }

    private void OnMouseDown()
    {
        RollDie();
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
    }
}
