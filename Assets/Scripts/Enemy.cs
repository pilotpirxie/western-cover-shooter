using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float hp;
    [SerializeField] private int bullets = 5;
    
    [SerializeField] private bool isReloading = false; 
    [SerializeField] private GameObject player;
    [SerializeField] private float shootRadius = 100f;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform instantiateTransform;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        InvokeRepeating("Shoot", 1f, 1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            hp--;

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < shootRadius)
        {
            gameObject.transform.LookAt(player.transform);
        }
    }

    private void Shoot()
    {
        if (bullets > 0)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(gameObject.transform.position, (player.transform.position - gameObject.transform.position), out hit, 1000f)) 
            {
                GameObject newBall = Instantiate(bulletPrefab, instantiateTransform.position, transform.rotation);
                newBall.GetComponent<Rigidbody>().velocity = (hit.point - instantiateTransform.position).normalized * 25f;
            }
            
            bullets--;
        }
        else
        {
            if (!isReloading)
            {
                isReloading = true;
                Invoke("FinishReloading", 2f);
            }
        }
    }

    private void FinishReloading()
    {
        isReloading = false;
        bullets = 5;
    }
}
