using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    public PlayerController playerController;
    private Land selectedLand;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectFarmLand();

        InteractLand();
    }

    public void InteractLand()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedLand.SwitchLandStatus(Land.LandState.Farm);
        }
    }

    private void DetectFarmLand()
    {
        if (selectedLand != null)
            selectedLand.SelectDirt(false);
        //Deselect last land


        RaycastHit[] raycastHits = Physics.RaycastAll(transform.position, Vector3.down, 2f);

        foreach (RaycastHit raycastHit in raycastHits)
        {
            if (raycastHit.collider.CompareTag("Dirt"))
            {
                Debug.Log("i m standing on dirt");
                Land land = raycastHit.collider.gameObject.GetComponent<Land>();
                land.SelectDirt(true);
                selectedLand = land;
            }
        }
    }
}
