using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponController
{
    private Rigidbody2D rb;

    public Pistol() : base(WeaponType.Pistol, "Pistol", 15, 0.01f, new Vector2(0.2f, 0.2f), 30f)
    {

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();  
    }

    public override void FixedUpdate()
    {
        if (!Shooting)
            base.FixedUpdate();
        else
            Change();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player && collision.gameObject.Equals(Player.gameObject)) return;

        Shooting = false;
        Destroy(gameObject);

        base.OnCollisionEnter2D(collision);
    }

    private void Change()
    {
        rb.velocity = Vector2.up * ShootSpeed;
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(gameObject, transform.position, Quaternion.identity);
        bullet.GetComponent<Pistol>().Player = Player;

        return bullet;
    }

    public override void Shoot()
    {
        if (Disabled) return;
        base.Shoot();

        GameObject bullet = CreateBullet();
        bullet.GetComponent<Pistol>().Shooting = true;
    }
}
