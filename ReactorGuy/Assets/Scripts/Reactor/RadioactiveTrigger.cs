using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadioactiveTrigger : MonoBehaviour
{
    [SerializeField] private bool isSafeZone;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(isSafeZone)
            {
                Game.GameManager.Player = Game.GameManager.PlayerState.Resting;
                Game.SoundManager.PlaySound(Game.SoundManager.Sound.RadiationRest);
            }
            else
            {
                Game.GameManager.Player = Game.GameManager.PlayerState.Radioactive;
                Game.SoundManager.PlaySound(Game.SoundManager.Sound.RadiationHit);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Game.GameManager.Player = Game.GameManager.PlayerState.Neutral;
        }
    }
}
