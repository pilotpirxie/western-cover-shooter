using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float hp;
    [SerializeField] private int maxBullets = 5;
    [SerializeField] private int bullets = 5;
    
    [SerializeField] private bool isReloading = false; 
    [SerializeField] private GameObject player;
    [SerializeField] private float shootRadius = 100f;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform instantiateTransform;
    [SerializeField] private float shootInterval = 0.25f;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        InvokeRepeating("Shoot", 1, shootInterval + Random.Range(-0.1f, 0.25f));
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
        if (bullets > 0 && Vector3.Distance(player.transform.position, gameObject.transform.position) < shootRadius)
        {
            RaycastHit hit;

            Vector3 target = player.transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(0.25f, 1.5f), Random.Range(-0.75f, 0.75f));
            
            if (Physics.Raycast(instantiateTransform.position, (target - instantiateTransform.position), out hit, 1000f)) 
            {
                GameObject newBall = Instantiate(bulletPrefab, instantiateTransform.position, transform.rotation);
                newBall.transform.LookAt(target);
                newBall.GetComponent<Rigidbody>().velocity = (hit.point - instantiateTransform.position).normalized * 50f;
                
                audioSource.pitch = 1 + Random.Range(-0.1f, 0.1f);
                audioSource.PlayOneShot(shootClip);
            }
            
            bullets--;
        }
        
        if (bullets <= 0)
        {
            if (!isReloading)
            {
                isReloading = true;
                Invoke("FinishReloading", 3f);
                audioSource.PlayOneShot(reloadClip);
            }
        }
    }

    private void FinishReloading()
    {
        isReloading = false;
        bullets = maxBullets;
    }
}
