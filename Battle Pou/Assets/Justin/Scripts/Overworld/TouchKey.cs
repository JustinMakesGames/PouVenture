using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchKey : MonoBehaviour
{
    public float rotateSpeed;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") & !FindObjectOfType<InvincibleFrames>().isInvincible)
        {
            print("Touched key");
            GetComponent<AudioSource>().Play();
            FindObjectOfType<HasKey>().hasKey = true;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 1f);
        }
    }
}
