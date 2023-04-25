using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShot : MonoBehaviour, IProjectile
{

    [SerializeField] private GameObject explosionEffect;
    
    [SerializeField]
    private GameObject audioSource;
    private float damage = 5f;
    public float moveSpeed = 1.5f;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Node node;
    public void Seek(Transform target, float damage)
    {
        // Move mine:
        targetPosition = new Vector3(target.position.x, target.position.y + 0.17f, target.position.z);
        node = target.GetComponent<Node>();
        StartMoving();
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            node.hasMine = false;
            var effect = Instantiate(explosionEffect, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), explosionEffect.transform.rotation);
            Instantiate(audioSource, gameObject.transform.position, Quaternion.identity);
            Destroy(effect, 2f);
            Destroy(gameObject);
        }
    }
    
    private void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}
