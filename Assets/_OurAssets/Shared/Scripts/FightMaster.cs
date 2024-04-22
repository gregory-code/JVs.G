using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FightMaster : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float minutes;
    private float seconds = 30;

    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] Animator winnerAnimator;

    CharacterBase p1;
    CharacterBase p2;

    [SerializeField] Image nextRoundImage;
    [SerializeField] FightCamera fightCamera;

    bool loading;

    [SerializeField] CharacterBase Grappler;
    [SerializeField] CharacterBase Knight;
    [SerializeField] Transform Spawn1;
    [SerializeField] Transform Spawn2;

    [SerializeField] GameObject[] p1Wins;
    [SerializeField] GameObject[] p2Wins;
    int p1WinCount;
    int p2WinCount;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        p1 = Instantiate(Grappler, Spawn1.transform.position, Spawn2.transform.rotation);
        p2 = Instantiate(Knight, Spawn2.transform.position, Spawn2.transform.rotation);

        fightCamera.SetUp(p1, p2);

        StartCoroutine(Timer());
    }

    private void Update()
    {
        float fill = (loading) ? 1 : 0 ;
        nextRoundImage.fillAmount = Mathf.Lerp(nextRoundImage.fillAmount, fill, 6 * Time.deltaTime);
    }

    IEnumerator Timer()
    {
        winnerText.text = "";
        winnerAnimator.SetTrigger("Hide");
        timerText.text = $"{minutes}:{seconds}";

        while (minutes > 0 || seconds > 0)
        {
            yield return new WaitForSeconds(1);
            if (seconds <= 0)
            {
                minutes--;
                seconds = 59;
            }
            else
            {
                seconds--;
            }
            timerText.text = $"{minutes}:{seconds}";

            if (seconds <= 9)
                timerText.text = $"{minutes}:0{seconds}";
        }

        p1.Tie();
        p2.Tie();

        if (p1.GetRemainingHealth() > p2.GetRemainingHealth())
        {
            Winner(true);
        }
        else
        {
            Winner(false);
        }
    }

    IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(4);
        loading = true;
        yield return new WaitForSeconds(2);
        Destroy(p1.gameObject);
        Destroy(p2.gameObject);
        Setup();
        loading = false;
    }

    public void Winner(bool p1Won)
    {
        if(p1Won)
        {
            winnerAnimator.SetTrigger("P1");
            winnerText.text = "Player 1 Wins";
            p1WinCount++;

            if(p1WinCount == 2)
            {
                p1Wins[1].SetActive(true);
                // game ends
            }

            if (p1WinCount == 1)
            {
                p1Wins[0].SetActive(true);
                StartCoroutine(StartNextRound());
                // next round
            }
        }
        else
        {
            winnerAnimator.SetTrigger("P1");
            winnerText.text = "Player 2 Wins";
            p2WinCount++;

            if (p2WinCount == 2)
            {
                p2Wins[1].SetActive(true);
                // game ends
            }

            if (p2WinCount == 1)
            {
                p2Wins[0].SetActive(true);
                StartCoroutine(StartNextRound());
                // next round
            }
        }
    }
}
