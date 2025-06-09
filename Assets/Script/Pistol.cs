using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
        auto = false;
        ammoMax = 10;
        ammoCurrent = 10;
        ammoBackPack = 30;
    }

    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameBullet = Instantiate(particle, hit.point, hit.transform.rotation);
            if (hit.collider.CompareTag("enemy"))
            {
                //Число 10 можешь поменять на своё. Это урон, который наносит одна пуля
                hit.collider.gameObject.GetComponent<Enemy>().ChangeHealth(20);
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
