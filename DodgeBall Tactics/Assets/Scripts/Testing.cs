using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Transform enemy, player;

    public Vector3 bouncePos;
    public float angle;

    private void Update()
    {
        Vector3 initialDir = player.position - enemy.position;
        Vector3 reflectDir = enemy.position - (enemy.position + bouncePos);

        initialDir.Normalize();
        reflectDir.Normalize();
        print("Initial direction: " + initialDir);
        print("Reflect direction: " + reflectDir);
        print("Reflect direction full vector: " + enemy.position + reflectDir * 4);
        Debug.DrawLine(enemy.position, enemy.position + reflectDir * 4, Color.green, 1f);
       
        angle = Vector3.Angle(initialDir, reflectDir);
    }
}
