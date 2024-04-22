using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightDash : MonoBehaviour
{
    Transform follow;
    CharacterBase myChar;

    [SerializeField] GameObject magicHandBurst;

    public void Init(Transform follow, CharacterBase myCharacter)
    {
        this.follow = follow;
        myChar = myCharacter;
        StartCoroutine(DashTimer());
    }

    public IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(1.1f);
        Instantiate(magicHandBurst, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, follow.position, 60 * Time.deltaTime);
    }
}
