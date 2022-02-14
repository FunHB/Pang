using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : WeaponController
{
    public override Vector3 Position { get => Player.transform.position - new Vector3(0, (Player.transform.localScale.y / 2) - ((transform.localScale.y / 2) + 0.01f), 0); }

    private float PlayerPosition { get => Player.transform.position.y - (Player.transform.localScale.y / 2); }
    private float minLength;

    public bool Returning { get; set; }

    public Chain() : base(WeaponType.Chain, "Chain", -1, -1, new Vector2(0.5f, 0.5f), 10f)
    {
        Returning = false;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Shooting)
            Change();
    }

    public override void FixedUpdate()
    {
        if (!Shooting)
            base.FixedUpdate();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.Equals(Player.gameObject) || collision.transform.position.y <= PlayerPosition) return;
        Returning = true;
        base.OnCollisionEnter2D(collision);
    }

    private void Change()
    {
        if (transform.position.y + (transform.localScale.y / 2) <= minLength)
        {
            Shooting = false;
            Returning = false;
            Start();
            return;
        }

        int modifier = Returning ? -1 : 1;

        Vector3 vector = new Vector3(0, ShootSpeed * Time.deltaTime, 0) * modifier;

        transform.position += vector;
        transform.localScale += vector * 2f;
    }

    public override void Shoot()
    {
        if (Disabled) return;
        base.Shoot();

        Shooting = true;
        minLength = PlayerPosition;
    }
}
