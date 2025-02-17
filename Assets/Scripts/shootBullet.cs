using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootBullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float timerInterval = 3f;
    float time;
    public float shootVelocity = 700f;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        while (time >= timerInterval)
        {
            fireBullet();
            time -= timerInterval;
        }
    }

    void fireBullet()
    {
        GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation); 
        newBullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, shootVelocity));
    }

}
