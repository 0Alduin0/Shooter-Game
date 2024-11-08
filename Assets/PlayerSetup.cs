using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;
    public GameObject Camera;

    public string nickname;

    public TextMeshPro nicknameText;

    public Transform TPWeaponHolder;


    public void IsLocalPlayer()
    {
        TPWeaponHolder.gameObject.SetActive(false);

        movement.enabled = true;
        Camera.SetActive(true);
    }

    [PunRPC]
    public void setTPWeapon(int _weaponIndex)
    {
        foreach (Transform _weapon in TPWeaponHolder)
        {
            _weapon.gameObject.SetActive(false);
        }
        TPWeaponHolder.GetChild(_weaponIndex).gameObject.SetActive(true);
    }



    [PunRPC]
    public void SetNickname(string _name)
    {
        nickname = _name;
        nicknameText.text = nickname;
    }



}
