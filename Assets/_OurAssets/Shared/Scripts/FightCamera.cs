using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCamera : MonoBehaviour
{
    private CharacterBase P1;
    private CharacterBase P2;

    [SerializeField] private float y;
    [SerializeField] private float z;

    private Transform winnerPos;
    bool p1Wins;

    [SerializeField] private float zMultiplier;

    private bool gameOver;

    public void SetUp(CharacterBase p1, CharacterBase p2)
    {
        P1 = p1;
        P2 = p2;
    }

    void Update()
    {
        if (P1 == null || P2 == null)
            return;

        float speed = 7;

        Vector3 P1pos = P1.transform.position;
        Vector3 P2pos = P2.transform.position;
        Vector3 pos = (P1pos + P2pos) / 2;

        float dis = Vector3.Distance(P1pos, P2pos);

        pos.y = (dis <= 5f) ? (pos.y * 0.7f) + y : y ;
        pos.z = (dis * zMultiplier) + z;

        if (shaking && gameOver == false)
        {
            Vector3 shakeAmount = new Vector3(Random.value, Random.value, Random.value) * shakeMangintude * (Random.value > 0.5f ? 1 : -1);
            transform.localPosition += shakeAmount;
        }
        else
        {
            if (gameOver)
            {
                speed = 3f;
                GameObject playerToLookAt = (p1Wins) ? P1.gameObject : P2.gameObject ;
                transform.LookAt(playerToLookAt.transform);
                pos = winnerPos.position;
            }

            transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
        }
    }

    public void GameEnd(bool p1)
    {
        if(p1)
        {
            p1Wins = true;
            gameOver = true;
            winnerPos = P1.winPos;
        }
        else
        {
            p1Wins = false;
            gameOver = true;
            winnerPos = P2.winPos;
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
