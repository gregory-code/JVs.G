using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    CharacterBase otherguy;
    Transform follow;
    Vector3 playerFoward;
    bool facingRight;
    bool charging;
    int bonusDamage;

    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject explosion;
    
    public void Init(Transform transform, Vector3 foward, bool facingRight, CharacterBase otherguy, int bonusDamage)
    {
        follow = transform;
        playerFoward = foward;
        this.otherguy = otherguy;
        this.facingRight = facingRight;
        this.bonusDamage = bonusDamage;
        charging = true;
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(0.75f);
        charging = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        if (facingRight)
            rb.AddForce(playerFoward * 20, ForceMode.VelocityChange);

        if (facingRight == false)
            rb.AddForce(playerFoward * -20, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CharacterBase>() == otherguy)
        {
            GameObject.Find("fireballHit").GetComponent<AudioSource>().Play();
            otherguy.ApplyDamage(6 + bonusDamage);
            otherguy.ApplyHitStun(false);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (charging)
        {
            transform.position = Vector3.Lerp(transform.position, follow.position, 60 * Time.deltaTime);
        }

    }
}
