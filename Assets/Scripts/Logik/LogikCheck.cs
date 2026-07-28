using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogikCheck : MonoBehaviour, IInteractable
{
    [SerializeField] Logik logik;

    private void Awake()
    {
        logik = GetComponentInParent<Logik>();
    }

    public void Interact()
    {
        logik.Round();
    }
}
