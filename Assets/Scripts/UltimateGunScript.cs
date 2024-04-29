using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class UltimateGunScript : NetworkBehaviour
{
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public ulong bullet_id = 0;
    bool shooting, readyToShoot, reloading, isFireBtnPressed = false;
    public GameObject BulletSpawnPoint;
    Animator myAnimaitor;
    private List<GameObject> spawnedFireBullets = new List<GameObject>();

    // public TextMeshProUGUI BulletInfo;
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
        if (!IsOwner) return;
        MyInput();
        // ShootingByButton();
    }

    public void ToggleFirePress(bool firePressed)
    {
        isFireBtnPressed = firePressed;
    }

    public void ShootingByButton()
    {

        if (bulletsLeft == 0 && !reloading)
        {
            audio.playreload();
            Reload();
        }

        if (isFireBtnPressed && readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }

    }

    // Network object can only be spawn from server side
    // so that's why need a Server Remote Procedure Call
    [Rpc(SendTo.Server)]
    public void ShootRpc()
    {
        Vector3 spready = BulletSpawnPoint.transform.rotation.eulerAngles;
        float z = Random.Range(-spread, spread);
        spready = new Vector3(spready.x, spready.y, spready.z + z);

        GameObject go = Instantiate(Bullet, BulletSpawnPoint.transform.position, Quaternion.Euler(spready));
        go.GetComponent<NetworkObject>().Spawn(); // Spawning in network, which will reflect across all devices

    }

    public void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Space);
        else shooting = Input.GetKeyDown(KeyCode.Space);

        if (bulletsLeft == 0 && !reloading)
        {
            audio.playreload();
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    public void Shoot()
    {
        readyToShoot = false;
        /*ulong id = GetComponent<Player1>().playerID;
        Debug.Log("ID in UltimateGunScript class: " + id);*/
        
        ShootRpc();

        bulletsLeft--;
        bulletsShot--;

        // myAnimaitor.SetTrigger("shoot"); !!! NOT WORKING SINCE THIS ANIMATOR DOESN'T CONTAIN SPRITE SO DISAPPEARING PROBLEM OCCURS
        
        Invoke(nameof(ResetShot), timeBetweenShooting);

        /*if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke(nameof(ShootRpc), timeBetweenShots);*/
        Debug.Log("Bullet Shotted...");
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