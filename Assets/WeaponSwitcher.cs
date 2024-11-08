using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public PhotonView playerSetupView;

    private int selectedWeapon = 0;

    public Animation _animation;
    public AnimationClip draw;

    private void Start()
    {
        SelectWeapon();
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon += 1;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon -= 1;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
    void SelectWeapon()
    {
        playerSetupView.RPC("setTPWeapon", RpcTarget.All, selectedWeapon);

        if (selectedWeapon >= transform.childCount)
        {
            selectedWeapon = transform.childCount - 1;
        }

        _animation.Stop();
        _animation.Play(draw.name);


        int i = 0;
        foreach (Transform _weapon in transform)
        {
            if (i == selectedWeapon)
            {
                _weapon.gameObject.SetActive(true);
            }
            else
            {
                _weapon.gameObject.SetActive(false);
            }
            i++;


        }
    }


}
