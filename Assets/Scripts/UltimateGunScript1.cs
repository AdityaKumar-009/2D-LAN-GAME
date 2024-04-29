using UnityEngine;
using TMPro;
using System.Diagnostics;
using System;

public class UltimateGunScript1 : MonoBehaviour
{

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;
    public GameObject BulletSpawnPoint;
    Animator myAnimaitor;


    //    public TextMeshProUGUI BulletInfo;
    public GameObject Bullet;
    static Audio audio;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        myAnimaitor = GetComponent<Animator>();
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }
    private void Update()
    {
        MyInput();
//        BulletInfo.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetMouseButton(0);
        else shooting = Input.GetMouseButtonDown(0);

        if (bulletsLeft == 0 && !reloading)
        {
            Reload();
            audio.playreload();
        }

            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        Vector3 spready = BulletSpawnPoint.transform.rotation.eulerAngles;
        float z = UnityEngine.Random.Range(-spread, spread);
        spready = new Vector3 (spready.x, spready.y, spready.z+z);

        Instantiate(Bullet, BulletSpawnPoint.transform.position, Quaternion.Euler(spready));
        bulletsLeft--;
        bulletsShot--;


        myAnimaitor.SetTrigger("shoot");
        

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
        UnityEngine.Debug.Log("oy");
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}