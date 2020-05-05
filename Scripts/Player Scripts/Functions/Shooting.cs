using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float offset;
    public GameObject bullet;
    public Transform firePoint;
    private float timeBTWshots;
    public float startTimeBTWshots;
    // Update is called once per frame
    void Update()
    {
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

        if ( timeBTWshots <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(bullet, firePoint.position, transform.rotation);
                timeBTWshots = startTimeBTWshots;
            }
        }
        else
        {
            timeBTWshots -= Time.deltaTime;
        }
    }
}
