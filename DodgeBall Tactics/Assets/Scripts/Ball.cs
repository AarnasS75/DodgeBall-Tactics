using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float destroyTime = 1f;

    Rigidbody rb;

    bool destroy = false;

    Vector3 throwDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        throwDirection = GetComponentInParent<ThrowAction>().GetThrowDirection();
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            print("Hit!");
            destroy = true;
        }
    }
}
