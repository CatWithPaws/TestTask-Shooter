using System.Collections;
using UnityEngine;
using UnityEditor;
public class Gun : MonoBehaviour
{

    [SerializeField] private int MaxBullets;
    public int currStoredBullets { get; private set; }
    [SerializeField] private int MagazineSize;
    public int currBullets { get; private set; } = 0;
    public int ReloadTime;
    public int ShootDelay;
    public void ConsumeAmmo(int value)
    {
            currBullets -= value;
    }

    private void Start()
    {
        SetMaxBullets();
    }
    public void Reload()
    {

        if (currStoredBullets - (MagazineSize - currBullets) >= 0)
        {
            currStoredBullets -= MagazineSize - currBullets;
            currBullets = MagazineSize;
        }
    }
    public void SetMaxBullets()
    {
        currStoredBullets = MaxBullets;
        currBullets = MagazineSize;
    }
    public int GetMagazineSize()
    {
        return MagazineSize;
    }
}

