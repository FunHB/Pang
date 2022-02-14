using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum WeaponType
{
    Chain,
    Pistol,
    Laser,
    Granade
}

public abstract class WeaponController : MonoBehaviour
{
    public PlayerController Player { get; set; }

    private Vector2 Size { get; set; }
    public float ShootSpeed { get; set; }

    private readonly WeaponType type;
    public string WeaponName { get; }

    private int magazine;

    private Timer cooldown;
    private readonly float cooldownMaxValue;
    private float cooldownValue;

    public bool Shooting { get; set; } = false;

    public bool Disabled { get => cooldown != null; }
    public virtual Vector3 Position { get => Player.transform.position; }

    protected WeaponController(WeaponType type, string weaponName, int magazine, float cooldownMaxValue, Vector2 size, float speed)
    {
        this.type = type;
        this.WeaponName = weaponName;
        this.Size = size;
        this.ShootSpeed = speed;

        this.magazine = magazine;
        this.cooldownMaxValue = cooldownMaxValue;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        transform.localScale = Size;
        transform.position = Position;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (magazine != -1 && magazine < 1)
            Player.ChangeWeapon("Chain");

        if (cooldown != null && cooldownValue < 0)
        {
            cooldown.Dispose();
            cooldown = null;
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        BallController ball = collision.transform.GetComponent<BallController>();
        if (ball)
        {
            Player.AddPoints(ball.Points);
            ball.SplitUp();
            return;
        }

        Breakable breakable = collision.transform.GetComponent<Breakable>();
        if (breakable)
            breakable.GetDamage(1);
    }

    public virtual void FixedUpdate()
    {
        transform.position = Position;
    }

    public void SetCooldown()
    {
        if (cooldownMaxValue == -1) return;
        cooldownValue = cooldownMaxValue;
        cooldown = new Timer(_ => cooldownValue -= 0.1f, null, 100, 100);
    }

    public virtual void Shoot()
    {
        SetCooldown();
        if (magazine != -1)
            --magazine;
    }
}