using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] private int damage;
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
        transform.parent = null;
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
        if (collision.gameObject.CompareTag("HitBox"))
        {
            collision.gameObject.GetComponentInParent<Unit>().Damage(damage);
            CalculateBounceOff(collision.transform);

            destroy = true;
        }
    }
    void CalculateBounceOff(Transform collisionTransform)
    {
        
        Vector3 bounceOffset = Vector3.zero;
        int randomDir = Random.Range(0, 3);

        if ((rb.velocity).normalized == Vector3.forward)
        {
            switch (randomDir)
            {
                case 0:
                    bounceOffset = new Vector3(2, 0, -2);
                    break;
                case 1:
                    bounceOffset = new Vector3(-2, 0, -2);
                    break;
                case 2:
                    bounceOffset = Vector3.back * 2;
                    break;
            }
        }
        else if ((rb.velocity).normalized == Vector3.back)
        {
            switch (randomDir)
            {
                case 0:
                    bounceOffset = new Vector3(2, 0, 2);
                    break;
                case 1:
                    bounceOffset = new Vector3(-2, 0, 2);
                    break;
                case 2:
                    bounceOffset = Vector3.forward * 2;
                    break;
            }
        }

        rb.velocity = Vector3.zero;
        Vector3 newDirection = (collisionTransform.position - (collisionTransform.position + bounceOffset)).normalized;

        rb.AddForce(newDirection * throwForce, ForceMode.Impulse);
    }
}
