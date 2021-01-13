using UnityEngine;
using System.Collections;

public class TestHighLight : MonoBehaviour
{
    private HighlighterController hlc;

    void Start ()
    {
        hlc = GetComponent<HighlighterController>();
        //hlc.h.On(Color.red);
    }
	
	// Update is called once per frame
	void Update ()
    {
        hlc.h.On(Color.red);

    }
}
