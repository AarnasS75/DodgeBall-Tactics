using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 10f;

    private Vector3 targetPosition;
    private GridPosition currentGridPosition;
    
    float stoppingDistance = 0.1f;

    private void Awake()
    {
        targetPosition = transform.position;
    }
    private void Start()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        this.currentGridPosition = gridPosition;
        LevelGrid.Instance.SetUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            animator.SetBool("Walking", true);
            Vector3 moveDiection = (targetPosition - transform.position).normalized;
            transform.position += moveDiection * Time.deltaTime * moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, moveDiection, Time.deltaTime * rotateSpeed);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if(newGridPosition != currentGridPosition)
        {
            // Unit changed grid position
            LevelGrid.Instance.UnitMovedGridPosition(this, currentGridPosition, newGridPosition);
            currentGridPosition = newGridPosition;
        }
    }

    public  void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
