using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoController : MonoBehaviour
{
    private RectTransform rt;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (rt != null)
        {
            rt.offsetMax = new Vector2((int)Math.Round(PlayerController.Instance.curHP * 100.0f / PlayerController.Instance.GetMaxHP()) - 100, rt.offsetMax.y);
        }
    }
}
