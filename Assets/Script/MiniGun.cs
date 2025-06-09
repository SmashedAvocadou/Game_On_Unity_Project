using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Random;

public class MiniGun : Weapon
{
    void Start()
    {
        cooldown = 12f;
        auto = true;
        ammoBackPack = 100;
        ammoCurrent = 100;
        ammoMax = 200;
    }
    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 drift = new Vector3(UnityEngine.Random.Range(-15, 15), UnityEngine.Random.Range(-15, 15), UnityEngine.Random.Range(-15, 15));
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameBullet = Instantiate(particle, hit.point, hit.transform.rotation);
            if (hit.collider.CompareTag("enemy"))
            {
                //Число 10 можешь поменять на своё. Это урон, который наносит одна пуля
                hit.collider.gameObject.GetComponent<Enemy>().ChangeHealth(5);
            }
            Destroy(gameBullet, 1);
        }
    }

    public enum Enemies
    {
        None,
        Turret,
        Zombie,
        Wizzard
    }
    public Enemies enemies;

}
