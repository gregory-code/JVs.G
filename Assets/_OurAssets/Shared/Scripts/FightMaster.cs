using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FightMaster : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float minutes;
    private float seconds = 30;

    CharacterBase p1;
    CharacterBase p2;

    private void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("P1").GetComponent<CharacterBase>();
        p2 = GameObject.FindGameObjectWithTag("P2").GetComponent<CharacterBase>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        timerText.text = $"{minutes}:{seconds}";

        while(minutes > 0 || seconds > 0)
        {
            yield return new WaitForSeconds(1);
            if(seconds <= 0)
            {
                minutes--;
                seconds = 59;
            }
            else
            {
                seconds--;
            }
            timerText.text = $"{minutes}:{seconds}";

            if(seconds <= 9)
                timerText.text = $"{minutes}:0{seconds}";
        }

        p1.Tie();
        p2.Tie();

        if(p1.GetRemainingHealth() > p2.GetRemainingHealth())
        {
            Debug.Log("P1 wins tie");
        }
        else
        {
            Debug.Log("P2 wins tie");
        }
    }
}
