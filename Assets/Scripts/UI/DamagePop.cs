using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform camera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        transform.localPosition = new Vector3(0f, 1f, 0f);
        Vector3 direction = camera.position - transform.position;
        direction.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }
}
