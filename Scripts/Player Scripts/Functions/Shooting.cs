using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Shooting : MonoBehaviour
{
    public float offset;
    public GameObject bullet;
    public Transform firePoint;
    private float timeBTWshots;
    public float startTimeBTWshots;
    public int ammo;
    private int currAmmo;
    public TextMeshProUGUI tmp;
    // Update is called once per frame
    void Start()
    {
        currAmmo = ammo;
        tmp = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        tmp.text = "Ammo: " + currAmmo + "/" + ammo;
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (transform.localScale.x == 1)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);
        }
        else if (transform.localScale.x == -1)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -rotateZ + offset);
        }
        if (Input.GetKey(KeyCode.R))
        {
            currAmmo = ammo;
        }
        if ( timeBTWshots <= 0)
        {
            if (currAmmo > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(bullet, firePoint.position, transform.rotation);
                    timeBTWshots = startTimeBTWshots;
                    --currAmmo;
                }
                if (Input.GetKey(KeyCode.R))
                {
                    currAmmo = ammo;
                }

            } 
            else
            {
                reload();
            }
        }
        else
        {
            timeBTWshots -= Time.deltaTime;
        }
    }
    void reload()
    {
        if (Input.GetKey(KeyCode.R))
        {
            currAmmo = ammo;
        }
        else if (!Input.GetKey(KeyCode.R))
        {
            print("RELOAD!");
        }
    }
}
