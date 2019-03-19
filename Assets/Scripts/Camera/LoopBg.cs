using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBg : MonoBehaviour
{
    public Transform bg1;
    public Transform bg2;
    public Transform cam;
    public bool isActive = true;

    private float bgDim;

    private void Start()
    {
        bgDim = bg2.position.x - bg1.position.x;
    }

    void Update()
    {
        if (isActive)
        {
            if (bg1.position.x < cam.position.x - bgDim)
                bg1.localPosition = new Vector2(bg1.localPosition.x + bgDim * 2, bg1.localPosition.y);
            else if (bg2.position.x < cam.position.x - bgDim)
                bg2.localPosition = new Vector2(bg2.localPosition.x + bgDim * 2, bg2.localPosition.y);
        }
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }
}
