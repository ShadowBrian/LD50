using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioactiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            Game.GameManager.Player = Game.GameManager.PlayerState.Radioactive;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            Game.GameManager.Player = Game.GameManager.PlayerState.Resting;
    }
}
