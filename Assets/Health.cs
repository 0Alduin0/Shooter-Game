using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;
    public bool isLocalPlayer;

    [Header("UI")]
    public TextMeshProUGUI healthText;


  

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;

        healthText.text = health.ToString();

        if (health <= 0)
        {
            if (isLocalPlayer)
            {
                RoomManager.instance.SpawnPlayer();
                RoomManager.instance.deaths++;
                RoomManager.instance.SetHashes();
            }


            Destroy(gameObject);
        }
    }


}
