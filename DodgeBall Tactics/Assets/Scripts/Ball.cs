using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] int damage = 5;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float destroyTime = 1f;

    private bool destroy = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Vector3 throwDirection = GetComponentInParent<ThrowAction>().GetThrowDirection();
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }
    private void Update()
    {
        if (destroy)
        {
            destroyTime -= Time.deltaTime;
            if(destroyTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<Unit>().Damage(damage);
            CalculateBounceOff(collision);
            
            destroy = true;
        }
    }
    void CalculateBounceOff(Collision collision)
    {
        rb.velocity = Vector3.zero;
        Vector3 bounceOffset = Vector3.zero;

        int randomDir = Random.Range(0, 2);
        switch (randomDir)
        {
            case 0:
                bounceOffset = new Vector3(2, collision.transform.position.y, 2);
                break;
            case 1:
                bounceOffset = new Vector3(-2, collision.transform.position.y, 2);
                break;
        }
        

        Vector3 newDirection = (collision.transform.position - (collision.transform.position + bounceOffset)).normalized;

        rb.AddForce(newDirection * throwForce, ForceMode.Impulse);
    }
}
