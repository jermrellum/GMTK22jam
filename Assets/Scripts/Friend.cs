using UnityEngine;

public class Friend : MonoBehaviour
{
    [SerializeField] private int countdown = 180;
    [SerializeField] private TMPro.TextMeshPro budTip;
    private OfficeController occ;

    private int br;
    private int bc;

    private void Start()
    {
        GameObject oc = GameObject.Find("OfficeController");
        occ = oc.GetComponent<OfficeController>();

        GenerateRandomDud();

        budTip.text = "Buddy: I put a dud " + bc + "\n" + "from left, " + br + " from top";
    }

    private void GenerateRandomDud()
    {
        int dudIdx = occ.GenerateDud();

        if (dudIdx >= 0)
        {
            bc = dudIdx % 4 + 1;
            br = dudIdx / 4 + 1;
        }
    }

    private void FixedUpdate()
    {
        if (occ.fwobCount > 0)
        {
            Destroy(this.gameObject);
        }

        if (countdown <= 0)
        {
            Destroy(this.gameObject);
        }
        countdown--;
    }
}
