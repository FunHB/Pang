using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType
{
    HpUp
}

public class ArtefactController : MonoBehaviour
{
    [SerializeField]
    private ModifierType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            MakeAction(player);
            Destroy(gameObject);
        }
    }

    private async void MakeAction(PlayerController player)
    {
        if (type == ModifierType.HpUp)
            await player.Damage(-100);
    }
}
