using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    Transform follow;
    CharacterBase myChar;

    public void Init(Transform follow, CharacterBase myCharacter)
    {
        this.follow = follow;
        myChar = myCharacter;
        StartCoroutine(RageTimer());
    }

    public IEnumerator RageTimer()
    {
        yield return new WaitForSeconds(8);
        myChar.RageOver();
        Destroy(this.gameObject);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, follow.position, 40 * Time.deltaTime);
    }
}
