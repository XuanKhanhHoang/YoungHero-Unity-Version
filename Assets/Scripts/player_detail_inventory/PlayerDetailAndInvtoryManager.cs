using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_d_invt_State_Manager : MonoBehaviour
{
    public static Player_d_invt_State_Manager Instance { get; private set; }

    [SerializeField] private PlayerDetailController playerDetailController;
    [SerializeField] private InventoryController inventoryController;
    public void Awake()
    {
        Instance = this;
    }

    public void HandleUpdate()
    {

        playerDetailController.HandleUpdate();
        inventoryController.HandleUpdate();
    }
}
