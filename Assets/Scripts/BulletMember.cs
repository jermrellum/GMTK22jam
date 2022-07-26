using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMember : MonoBehaviour
{
    private int[] bullets = new int[32];
    public bool showDie = false;
    public int repliesCount;
    public bool firstRound = true;

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
        bullets[idx] = 0;
    }

    public void Dudify(int idx)
    {
        bullets[idx] = 2;
    }

    public void ResetBullets()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = 1;
        }
        repliesCount = 0;
    }

    public int BulletsLeft()
    {
        int sum = 0;
        for (int i = 0; i < bullets.Length; i++)
        {
            if(bullets[i] > 0)
            {
                sum++;
            }
        }
        return sum;
    }

    public bool DoIExist(int idx)
    {
        return (bullets[idx] > 0);
    }

    public int GetDudStatus(int idx)
    {
        return bullets[idx];
    }

    public int FindRandomLive()
    {
        int[] bclone = new int[32];
        int bcidx = 0;

        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i] == 1)
            {
                bclone[bcidx] = i;
                bcidx++;
            }
        }

        if (bcidx > 0)
        {
            int ayn = Random.Range(0, bcidx);
            return bclone[ayn];
        }
        else
        {
            return -1;
        }
    }
}
