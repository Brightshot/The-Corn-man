using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    public Image transitionScreen;
    Color TransitionColor = Color.black;
    public void OpenTransition(float time)
    {
        transitionScreen.raycastTarget = true;
        LeanTween.value(0, 1, time).setOnUpdate((x) => { TransitionColor.a = x; transitionScreen.color = TransitionColor; }).setOnComplete(() => transitionScreen.raycastTarget = false).setIgnoreTimeScale(true);
    }

    public void CloseTransition(float time)
    {
        transitionScreen.raycastTarget = true;
        LeanTween.value(1, 0, time).setOnUpdate((x) => { TransitionColor.a = x; transitionScreen.color = TransitionColor; }).setOnComplete(() => transitionScreen.raycastTarget = false).setIgnoreTimeScale(true);
    }
}
