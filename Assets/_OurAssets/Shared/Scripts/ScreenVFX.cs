using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenVFX : MonoBehaviour
{
    [SerializeField] float timeScale = 0.02f;
    [SerializeField] float duration = 0.02f;

    bool frozen;

    public void StartFreezeFrame()
    {
        if (!frozen)
        {
            StartCoroutine(FreezeFrameCoroutine());
        }
    }

    IEnumerator FreezeFrameCoroutine()
    {
        frozen = true;
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
        frozen = false;
    }
}
