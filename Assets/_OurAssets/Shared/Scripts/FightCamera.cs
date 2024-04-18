using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FightCamera : MonoBehaviour
{
    private CharacterBase P1;
    private CharacterBase P2;

    [SerializeField] private float y;
    [SerializeField] private float z;

    [SerializeField] private float zMultiplier;

    void Start()
    {
        P1 = GameObject.FindGameObjectWithTag("P1").GetComponent<CharacterBase>();
        P2 = GameObject.FindGameObjectWithTag("P2").GetComponent<CharacterBase>();
    }

    void Update()
    {
        Vector3 P1pos = P1.transform.position;
        Vector3 P2pos = P2.transform.position;
        Vector3 pos = (P1pos + P2pos) / 2;

        float dis = Vector3.Distance(P1pos, P2pos);

        pos.y = (dis <= 5f) ? (pos.y * 0.7f) + y : y ;
        pos.z = (dis * zMultiplier) + z;

        transform.position = Vector3.Lerp(transform.position, pos, 7 * Time.deltaTime);
    }
}
