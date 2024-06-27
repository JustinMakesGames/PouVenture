using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPouDialog : MonoBehaviour
{
    public DialogManager pouDialog;
    private bool hasTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            FindObjectOfType<PlayerOverworld>().enabled = false;
            GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioRef>().ambient.Stop();
            GetComponent<AudioSource>().Play();
            pouDialog.StartDialog();
        }
    }
}
