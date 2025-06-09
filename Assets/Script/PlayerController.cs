using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Image pistolUI, rifleUI, miniGunUI, cusror;
    [SerializeField] float movementSpeed = 5f;
    float currentSpeed;
    Rigidbody rb;
    Vector3 direction;
    [SerializeField] float shiftSpeed = 10f;
    [SerializeField] float jumpForce = 7f;
    bool isGrounded = true;
    [SerializeField] Animator anim;
    float stamina = 5f;
    [SerializeField] GameObject pistol, rifle, miniGun;
    bool isPistol, isRifle, isMiniGun;
    private int health;
    private Weapons weapons;
    [SerializeField] AudioSource characterSounds;
    [SerializeField] AudioClip jump;

    public enum Weapons
    {
        None,
        Pistol,
        Rifle,
        MiniGun
    }

    public PlayerController(Weapons weapons)
    {
        this.weapons = weapons;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        direction = transform.TransformDirection(direction);
        if (direction.x != 0 || direction.z != 0)
        {
            anim.SetBool("Run", true);
            if(!characterSounds.isPlaying && isGrounded)
            {
                characterSounds.Play();
            }
        }
        if (direction.x == 0 && direction.z == 0)
        {
            anim.SetBool("Run", false);
            characterSounds.Stop();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            characterSounds.Stop();
            AudioSource.PlayClipAtPoint(jump, transform.position);
            anim.SetBool("Jump", true);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0)
            {
                stamina -= Time.deltaTime;
                currentSpeed = shiftSpeed;
            }
            else
            {
                currentSpeed = movementSpeed;
            }
        }
        if (stamina > 5f)
        {
            stamina = 5f;
        }
        else if (stamina < 0)
        {
            stamina = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && isPistol)
        {
            ChooseWeapon(Weapons.Pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isRifle)
        {
            ChooseWeapon(Weapons.Rifle);
        }
        //Здесь допиши логику для минигана и для отсутствия оружия
        if (Input.GetKeyDown(KeyCode.Alpha3) && isMiniGun)
        {
            ChooseWeapon(Weapons.MiniGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) )
        {
            ChooseWeapon(Weapons.None);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("Jump", false);
    }
    public void ChooseWeapon(Weapons weapons)
    {
        anim.SetBool("Pistol", weapons == Weapons.Pistol);
        anim.SetBool("Assault", weapons == Weapons.Rifle);
        anim.SetBool("MiniGun", weapons == Weapons.MiniGun);
        anim.SetBool("NoWeapon", weapons == Weapons.None);
        pistol.SetActive(weapons == Weapons.Pistol);
        rifle.SetActive(weapons == Weapons.Rifle);
        miniGun.SetActive(weapons == Weapons.MiniGun);
        if (weapons != Weapons.None)
        {
            cusror.enabled = true;
        }
        else
        {
            cusror.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "pistol":
                if (!isPistol)
                {
                    isPistol = true;
                    pistolUI.color = Color.white;
                    ChooseWeapon(Weapons.Pistol);
                }
                break;
            case "rifle":
                if (!isRifle)
                {
                    isRifle = true;
                    rifleUI.color = Color.white;
                    ChooseWeapon(Weapons.Rifle);
                }
                break;
            case "minigun":
                if (!isMiniGun)
                {
                    isMiniGun = true;
                    miniGunUI.color = Color.white;
                    ChooseWeapon(Weapons.MiniGun);
                }
                break;
            default:
                break;
        }
        Destroy(other.gameObject);
    }
    public void ChangeHealth(int count)
    {
        health -= count;
        if (health <= 0)
        {
            anim.SetBool("Die", true);
            ChooseWeapon(Weapons.None);
            this.enabled = false;
        }
    }

}
