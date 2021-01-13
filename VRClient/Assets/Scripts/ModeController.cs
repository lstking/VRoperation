using UnityEngine;
using System.Collections;

public enum Mode
{
    Manual,
    Auto
}

public class ModeController : MonoBehaviour
{
    public GameObject manualObj;
    public GameObject autoObj;

    public HighlighterController highlightCtr;

    public float flashTime = 2;

    private ViveHand touchHand;

    private Mode curMode = Mode.Manual;

    private void Start()
    {
        touchHand = GetComponent<ViveHand>();
    }

    private void Update()
    {
        var device = SteamVR_Controller.Input((int)touchHand.trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if(curMode == Mode.Manual)
            {
                curMode = Mode.Auto;
                manualObj.SetActive(false);
                autoObj.SetActive(true);

                StartCoroutine(Flash(flashTime));
            }
            else
            {
                curMode = Mode.Manual;
                manualObj.SetActive(true);
                autoObj.SetActive(false);
            }
        }
    }

    public IEnumerator Flash(float _time)
    {
        /*
        float _unit = _time / flashTime;
        for (int t = 0; t < flashTime; ++t)
        {
            if (t % 2 == 0)
            {
                highlightCtr.h.On(Color.blue);
                highlightCtr.h.FlashingOn();
            }
            else
            {
                highlightCtr.h.Off();
            }
            yield return new WaitForSeconds(_unit);
        }
        */

        highlightCtr.h.FlashingOn();
        yield return new WaitForSeconds(_time);
        highlightCtr.h.FlashingOff();
    }

}
