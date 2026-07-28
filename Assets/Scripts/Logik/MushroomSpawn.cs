using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpawn : MonoBehaviour
{
    [SerializeField] GameObject _correctPlaceAndColorMushroom;
    [SerializeField] GameObject _correctColorMushroom;
    public GameObject CurrentMushroom;
    public void Spawn(MushroomState state)
    {
        Destroy(CurrentMushroom);

        switch (state)
        {
            case MushroomState.PlaceAndColor:
                CurrentMushroom = Instantiate(_correctPlaceAndColorMushroom, transform);
                break;
            case MushroomState.Color:
                CurrentMushroom = Instantiate(_correctColorMushroom, transform);
                break;
            default:
                Destroy(CurrentMushroom);
                break;

        }
    }
}