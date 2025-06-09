using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Объект Particle System, который будет оставлять след от пуль
    [SerializeField] protected GameObject particle;
    //Камера(понадобится для определения центра экрана)
    [SerializeField] protected GameObject cam;
    //Тип стрельбы
    protected bool auto = false;
    //Время задержки между выстрелами и таймер, который считает время
    protected float cooldown = 1;
    protected float timer = 0;
    //Сколько патронов в обойме
    protected int ammoCurrent;
    //Сколько патронов помещается в обойму
    protected int ammoMax;
    //Сколько патронов в запасе
    protected int ammoBackPack;
    //Переменная для отображения текста на UI
    [SerializeField] TMP_Text ammoText;
    [SerializeField] AudioSource shoot;
    [SerializeField] AudioClip bulletSound, noBulletSound, reload;


    //При старте приравниваем таймер к задержке между выстрелами
    //Так не будет задержки перед первым выстрелом
    private void Start()
    {
        timer = cooldown;
    }
    private void Update()
    {
        //Запускаем таймер
        timer += Time.deltaTime;
        //Если удерживаем клавишу мыши, то вызываем метод Shoot
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        AmmoTextUpdate();
        if (Input.GetKeyDown(KeyCode.R))
        {
            
            if (ammoCurrent != ammoMax && ammoBackPack != 0)
            {
                shoot.PlayOneShot(reload);
                Invoke("Reload", 1);
            }
        }
    }
    //способность оружия стрелять. 
    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0) || auto)
        {
            if (ammoCurrent>0)
            {
                OnShoot();
                timer = 0;
                ammoCurrent = ammoCurrent-1;
                shoot.PlayOneShot(bulletSound);
                shoot.pitch = Random.Range(1f, 1.5f);
            }
            else
            {
                shoot.PlayOneShot(noBulletSound);
            }
        }
    }
    protected virtual void OnShoot()
    {

    }
    private void AmmoTextUpdate()
    {
        ammoText.text = ammoCurrent + " / " + ammoBackPack;
    }
    private void Reload()
    {
        int ammoNeed = ammoMax - ammoCurrent;
        //если кол-во патронов в запасе больше или равно кол-ву, которое нам нужно добавить то,
        if (ammoBackPack >= ammoNeed)
        {
            //из кол-ва патронов в запасе вычитаем кол-во, которое добавляем в обойму
            ammoBackPack -= ammoNeed;
            //в обойму добавляем нужное количество патронов
            ammoCurrent += ammoNeed;
        }
        //иначе(если в запасе меньше патронов, чем нам нужно)
        else
        {
            //добавляем в обойму столько патронов, сколько осталось в запасе
            ammoCurrent += ammoBackPack;
            //обнуляем кол-во патронов в запасе
            ammoBackPack = 0;
        }
    }
}
