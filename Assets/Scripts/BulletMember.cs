using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMember : MonoBehaviour
{
    private bool[] bullets = new bool[32];
    public bool showDie = false;

    void Awake()
    {
        int bml = FindObjectsOfType<BulletMember>().Length;
        if(bml > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        ResetBullets();
    }

    public void TakeBullet(int idx)
    {
        bullets[idx] = false;
    }

    public void ResetBullets()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = true;
        }
    }

    public int BulletsLeft()
    {
        int sum = 0;
        for (int i = 0; i < bullets.Length; i++)
        {
            if(bullets[i])
            {
                sum++;
            }
        }
        return sum;
    }

    public bool DoIExist(int idx)
    {
        return bullets[idx];
    }
}
