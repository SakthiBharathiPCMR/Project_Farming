using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    // Start is called before the first frame update

    public enum LandState
    {
        Dirt,
        Farm,
        Watered,
    }

    public Material dirtMat, farmMat, wateredMat;

    public LandState landState;

    private new Renderer renderer;

    public Transform selectTransform;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        SwitchLandStatus(LandState.Dirt);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchLandStatus(LandState switchLandState)
    {
        landState = switchLandState;
        Material materialToSwitch = dirtMat;

        switch (switchLandState)
        {
            case LandState.Dirt:
                {
                    materialToSwitch = dirtMat;
                }
                break;
            case LandState.Farm:
                {
                    materialToSwitch = farmMat;

                }
                break;
            case LandState.Watered:
                {
                    materialToSwitch = wateredMat;

                }
                break;
            default:
                {
                    Debug.Log("Default farm state");
                }
                break;

        }

        renderer.material = materialToSwitch;
    }


    public void SelectDirt(bool isActive)
    {
        selectTransform.gameObject.SetActive(isActive);
    }

}
