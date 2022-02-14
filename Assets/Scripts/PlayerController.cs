using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string playerName;

    [SerializeField]
    private WeaponType weaponType;

    private WeaponController weapon;

    [SerializeField]
    [Range(0f, 10f)]
    private float moveSpeed = 5f;

    [SerializeField]
    private float maxHp = 100;

    private float hp;

    [SerializeField]
    [Range(0f, 5f)]
    private float immuneTime = 1f;

    public int TotalPoints { get; set; } = 0;
    public int Points { get; set; } = 0;

    private Rigidbody2D rb;
    private BoxCollider2D bc;

    private float horizontalMove = 0f;
    private float vertivalMove = 0f;

    private bool immune = false;
    private bool climbing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        CreateWeapon(weaponType.ToString());

        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        Climb();

        SetImmune();

        if (!climbing && Input.GetKeyDown(KeyCode.Space))
            weapon.Shoot();

        if (hp <= 0)
            Die();

        if (hp > maxHp)
            hp = maxHp;

        Debug.Log(hp);
    }

    private void FixedUpdate()
    {
        /*Move(horizontalMove, vertivalMove);*/

        /*if (vertivalMove.Equals(0))
        {
            MoveHorizontally();
            return;
        }

        MoveVertically();*/

        MoveHorizontally();
        if (climbing)
            MoveVertically();
    }

    private void MoveHorizontally()
    {
        if (climbing)
        {
            transform.position += new Vector3(horizontalMove, 0) * moveSpeed * Time.deltaTime;
            return;
        }

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position - new Vector3((transform.localScale.x / 2) + 0.1f, 0), Vector3.down);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3((transform.localScale.x / 2) + 0.1f, 0), Vector3.down);

        if (((hit1.distance - (transform.localScale.y / 2)) <= 0.1f || (hit2.distance - (transform.localScale.y / 2)) <= 0.1f) && horizontalMove != 0)
            rb.velocity = new Vector2(horizontalMove, 0) * moveSpeed;
    }

    private void MoveVertically()
    {
        transform.position += new Vector3(0, vertivalMove) * moveSpeed * Time.deltaTime;
    }

    /*private void Move(float horizontalInput, float verticalInput)
    {
        if (horizontalInput.Equals(0) && verticalInput.Equals(0)) return;
        rb.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;
    }*/

    private void Climb()
    {
        RaycastHit2D[] hitInformation = Physics2D.RaycastAll(transform.position - new Vector3(0, transform.localScale.y / 2, 0), Vector3.back);

        if (hitInformation.Where(hit => hit.transform.gameObject.layer.Equals(10)).ToArray().Length > 0 && hitInformation.Where(hit => hit.transform.gameObject.layer.Equals(7)).ToArray().Length < 1)
        {
            climbing = true;
            vertivalMove = Input.GetAxisRaw("Vertical");
            rb.bodyType = RigidbodyType2D.Static;
            return;
        }
        climbing = false;
        vertivalMove = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Die()
    {
        Destroy(weapon.gameObject);
        Destroy(gameObject);
    }

    private void SetImmune()
    {
        int playerLayer = 6;
        int weaponLayer = 9;
        int immunePlayerLayer = 11;

        if (immune && gameObject.layer.Equals(playerLayer))
        {
            weapon.gameObject.layer = immunePlayerLayer;
            gameObject.layer = immunePlayerLayer;
        }
        else if (gameObject.layer.Equals(immunePlayerLayer))
        {
            gameObject.layer = playerLayer;
            weapon.gameObject.layer = weaponLayer;
        }
    }

    private GameObject CreateWeapon(string weaponName)
    {
        GameObject weaponObject = Instantiate(Resources.Load("Prefabs/" + weaponName) as GameObject, transform.position, Quaternion.identity, transform.parent);
        weapon = weaponObject.AddComponent(Type.GetType(weaponName)) as WeaponController;
        weapon.Player = this;
        return weaponObject;
    }

    public void ChangeWeapon(string weaponName)
    {
        Debug.Log("Weapon Changed");
        Destroy(weapon.gameObject);
        CreateWeapon(weaponName);
    }

    public void Finish()
    {
        TotalPoints += Points;
        Points = 0;
    }

    public async Task Damage(float damage)
    {
        if (immune) return;
        hp -= damage;

        await Task.Run(async () =>
        {
            immune = true;
            await Task.Delay((int)(immuneTime * 1000));
            immune = false;
        });
    }

    public void AddPoints(int newPoints) => Points += newPoints;
}
