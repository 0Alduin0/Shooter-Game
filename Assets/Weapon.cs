using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 5;
    public float fireRate = 10;
    public Camera Camera;

    [Header("VFX")]
    public GameObject hitVFX;

    private float nextFire;

    [Header("Ammo")]
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]
    public Animation Animation;
    public AnimationClip reload;

    [Header("Recoil Settings")]
    //[Range(0f, 1f)]
    //public float recoilPercent = 0.3f;

    [Range(0f, 2f)]
    public float recoverPercent = 0.7f;

    [Space]
    public float recoilUp = 0.025f;
    public float recoilBack = 0.025f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;

    private bool recoiling;
    private bool recovering;

    private void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }
    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0) && nextFire <= 0 && ammo > 0 && Animation.isPlaying == false)
        {
            nextFire = 1 / fireRate;

            ammo--;

            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;

            Fire();
        }
        if ((Input.GetKeyDown(KeyCode.R) && mag > 0) || (mag > 0 && ammo == 0))
        {
            Reload();
        }

        if (recoiling)
        {
            Recoil();
        }
        if (recovering)
        {
            Recover();
        }
    }


    void Reload()
    {
        Animation.Play(reload.name);

        if (mag > 0)
        {
            mag--;
            ammo = magAmmo;
        }
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }


    void Fire()
    {
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);

        RaycastHit hit;



        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);

            if (hit.transform.gameObject.GetComponent<Health>())
            {
                PhotonNetwork.LocalPlayer.AddScore(damage);

                if (damage >= hit.transform.gameObject.GetComponent<Health>().health)
                {
                    RoomManager.instance.kills++;
                    RoomManager.instance.SetHashes();

                    PhotonNetwork.LocalPlayer.AddScore(100);
                }

                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }


    }
    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }
    void Recover()
    {
        Vector3 finalPosition = originalPosition;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
}
