using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickUp : MonoBehaviour
{
    public float range = 100f;
    public Camera fpsCam;

    public GameObject objectText;
    public Text pickUpObjectName;

    public GameObject pistol;


    int layerMask = (1 << 9);
    int doorMask = (1 << 10);
    int finalDoorMask = (1 << 11);
    int healthMask = (1 << 12);
    int ammoMask = (1 << 13);

    public AudioSource doorSound;
    public GameObject EnterMaze;
    public GameObject FinishMaze;

    public Gun gunScript;
    public Player playerScript;

    public AudioSource PickUpAmmo;
    public AudioSource PickUpHealth;

    // Start is called before the first frame update
    void Start()
    {
        objectText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask))
        {
            objectText.SetActive(true);
            pickUpObjectName.text = "Pistol";
            if (Input.GetKeyDown(KeyCode.E))
            {
                DisableObject disObj = hit.transform.GetComponent<DisableObject>();
                disObj.DisableThisObject();
                PickUpAmmo.Play();

                pistol.SetActive(true);
            }
        }
        else if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, doorMask))
        {
            objectText.SetActive(true);
            pickUpObjectName.text = "Open Door";
            if (Input.GetKeyDown(KeyCode.E))
            {
                doorSound.Play();
                EnterMaze.SetActive(true);
                StartCoroutine(WaitAndLoadMaze(2));
            }
        }
        else if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, finalDoorMask))
        {
            objectText.SetActive(true);
            pickUpObjectName.text = "Enter Castle";
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Enter Castle");
                doorSound.Play();
                FinishMaze.SetActive(true);
                StartCoroutine(WaitAndFinish(2));
            }
        }
        else if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ammoMask))
        {
            objectText.SetActive(true);
            pickUpObjectName.text = "Pistol Ammo";
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Pick up ammo");
                gunScript.AddAmmo();
                Destroy(hit.collider.gameObject);
                PickUpAmmo.Play();
            }
        }
        else if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, healthMask))
        {

            if (playerScript.GetCurrentHealth != 100)
            {
                objectText.SetActive(true);
                pickUpObjectName.text = "Health";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Pick up health");
                    playerScript.AddHealth();
                    Destroy(hit.collider.gameObject);
                    PickUpHealth.Play();
                }
            }
            else {
                objectText.SetActive(true);
                pickUpObjectName.text = "Your have max health";
            }
            
        }
        else
        {
            objectText.SetActive(false);
        }
    }

    IEnumerator WaitAndLoadMaze(float sec)
    {

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Maze");

    }

    IEnumerator WaitAndFinish(float sec)
    {

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
