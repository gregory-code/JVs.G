using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCamera : MonoBehaviour
{
    private CharacterBase P1;
    private CharacterBase P2;

    [SerializeField] private float y;
    [SerializeField] private float z;

    [SerializeField] private float zMultiplier;

    public void SetUp(CharacterBase p1, CharacterBase p2)
    {
        P1 = p1;
        P2 = p2;
    }

    void Update()
    {
        if (P1 == null || P2 == null)
            return;

        Vector3 P1pos = P1.transform.position;
        Vector3 P2pos = P2.transform.position;
        Vector3 pos = (P1pos + P2pos) / 2;

        float dis = Vector3.Distance(P1pos, P2pos);

        pos.y = (dis <= 5f) ? (pos.y * 0.7f) + y : y ;
        pos.z = (dis * zMultiplier) + z;

        if (shaking)
        {
            Vector3 shakeAmount = new Vector3(Random.value, Random.value, Random.value) * shakeMangintude * (Random.value > 0.5f ? 1 : -1);
            transform.localPosition += shakeAmount;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, pos, 7 * Time.deltaTime);
        }
    }

    [SerializeField] float shakeDuration = 0.2f;
    [SerializeField] float shakeMangintude = 0.1f;

    bool shaking;

    public void StartShake()
    {
        if (!shaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    IEnumerator ShakeCoroutine()
    {
        shaking = true;
        yield return new WaitForSeconds(shakeDuration);
        shaking = false;
    }
}
