using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public int magazineCapacity = 5;
    public int pocketAmmo = 0;
    public float range = 100f;

    public ParticleSystem muzzleFlash;
    public Camera fpsCam;

    public GameObject impactShotEffect;
    public GameObject impactBloodEffect;
    public GameObject ammoUI;

    public Text currentMagazineText;
    public Text pocketAmmoText;

    int layerMask = (1 << 8);

    private int currentAmmoInMagazine;

    public AudioSource[] gunSounds;

    private float waitToFire = 0;

    private void Start()
    {
        currentAmmoInMagazine = 10;

        currentMagazineText.text = currentAmmoInMagazine.ToString();
        pocketAmmoText.text = pocketAmmo.ToString();

        ammoUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentAmmoInMagazine == 0 && pocketAmmo > 0) {
            Reload();
        }

        if (Input.GetButtonDown("Fire1")) {
            if (currentAmmoInMagazine > 0)
            {
                if (waitToFire <= 0) {
                    Shoot();
                    waitToFire = 0.85f;
                }
                
            }
            else {
                gunSounds[2].Play();
            }
        }

        waitToFire -= Time.deltaTime * 2;

        if (currentAmmoInMagazine < magazineCapacity && pocketAmmo > 0) {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

    }

    void Shoot() {
        GetComponent<Animator>().SetTrigger("Fire");

        currentAmmoInMagazine -= 1;
        currentMagazineText.text = currentAmmoInMagazine.ToString();

        muzzleFlash.Play();
        gunSounds[0].Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask)) {

            TakeDamage takeDamage = hit.transform.GetComponent<TakeDamage>();

            if (takeDamage != null) {
                Debug.Log("We Hit Enemy!");
            }

            if (hit.transform.tag != null)
            {
                if (hit.transform.tag == "ZombieHead")
                {
                    takeDamage.CallReduceDamage(50.0f, hit.transform.tag);
                    Debug.Log("Headshot!");
                    
                    GameObject impactGO = Instantiate(impactBloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGO, 0.5f);
                }
                else if (hit.transform.tag == "ZombieBody")
                {
                    takeDamage.CallReduceDamage(10.0f, hit.transform.tag);
                    Debug.Log("Body Shot!");
                    
                    GameObject impactGO = Instantiate(impactBloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGO, 0.5f);
                }
                else if (hit.transform.tag == "Ground") {
                    GameObject impactGO = Instantiate(impactShotEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGO, 15f);
                }
            }

        }

    }

    void Reload() {
        Debug.Log("Reloading");
        GetComponent<Animator>().SetTrigger("Reload");
        gunSounds[1].Play();
        int ammoToInsert = magazineCapacity - currentAmmoInMagazine;

        waitToFire = 4;

        if (ammoToInsert > pocketAmmo) {
            currentAmmoInMagazine += pocketAmmo;
            pocketAmmo = 0;
        }
        else
        {
            currentAmmoInMagazine += ammoToInsert;
            pocketAmmo -= ammoToInsert;
        }

        currentMagazineText.text = currentAmmoInMagazine.ToString();
        pocketAmmoText.text = pocketAmmo.ToString();
        
    }

    public void AddAmmo() {
        pocketAmmo += 20;
        pocketAmmoText.text = pocketAmmo.ToString();
    }
}

