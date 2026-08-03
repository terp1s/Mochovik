using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButtonResize : MonoBehaviour
{
    public TMP_Text text;
    public RectTransform rtText;
    public float paddingX = 40f;
    public float paddingY = 20f;
    public float maxX = 500;

    private void Awake()
    {
        //text = GetComponentInChildren<TextMeshProUGUI>();
        //rtText = GetComponentInChildren<RectTransform>();
    }
    public void UpdateSize()
    {
        
        Vector2 size = text.GetPreferredValues();

        rtText.sizeDelta = size;

        RectTransform rt = GetComponent<RectTransform>();

        rt.sizeDelta = new Vector2(
            size.x + paddingX,
            size.y + paddingY
        );
    }
}

