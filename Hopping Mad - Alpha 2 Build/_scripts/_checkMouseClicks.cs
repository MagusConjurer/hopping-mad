using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _checkMouseClicks : MonoBehaviour {

    int clicks = 0;
    float clickTimer = 0f;
    float holdTime = 0f;
    float timerLimit = 0.5f;
    float holdLimit = 3f;

    bool singleClick;
    //bool doubleClick;
    bool holdClick;
    
    public bool CheckMouseClick()
    {

        if (Input.GetMouseButtonDown(0))
        {
            clickTimer += Time.fixedDeltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
			clickTimer = 0;
            return singleClick = true;
        }
        if (Input.GetMouseButtonUp(0) && clickTimer == timerLimit)
        {
            clickTimer = 0;
            return holdClick = true;
        }

        return singleClick = holdClick = false;
    }
		
    public bool GetSingle()
    {
        CheckMouseClick();
        return singleClick;
    }
    public bool GetHeld()
    {
        CheckMouseClick();
        return holdClick;
    }
	
}
