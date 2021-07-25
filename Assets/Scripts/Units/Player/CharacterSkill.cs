using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    private PlayerInput playerInput = null;

    [SerializeField]
    private float canUsePortalRange = 1f;

    [SerializeField]
    private Transform portalSpawnPosition = null;
    private Transform characterPosition = null;

    [SerializeField]
    private PortalManager portalManager = null;
    [SerializeField]
    private GameObject[] portals = new GameObject[2];
    private bool canSpawnPortal0 = true;
    private bool canSpawnPortal1 = true;
    private bool canUsePortal = true;
    private Portal portal0;
    private Portal portal1;


    void Start()
    {
        portalManager = FindObjectOfType<PortalManager>();

        playerInput = GetComponent<PlayerInput>();
        characterPosition = playerInput.transform;

        portal0 = portals[0].GetComponent<Portal>();
        portal1 = portals[1].GetComponent<Portal>();
    }

    void Update()
    {
        if (playerInput.spawnPortal0 && canSpawnPortal0)
        {
            canSpawnPortal0 = false;
            SpawnPortal0();
            Invoke("CanSpawnPortal0Set", 1f);
        }
        if (playerInput.spawnPortal1 && canSpawnPortal1)
        {
            canSpawnPortal1 = false;
            SpawnPortal1();
            Invoke("CanSpawnPortal1Set", 1f);
        }

        if (playerInput.usePortal && canUsePortal)
        {
            canUsePortal = false;
            UsePortal();
            Invoke("CanUsePortalSet", 1f);
        }
    }
    private void CanSpawnPortal0Set()
    {
        canSpawnPortal0 = true;
    }
    private void CanSpawnPortal1Set()
    {
        canSpawnPortal1 = true;
    }
    private void CanUsePortalSet()
    {
        canUsePortal = true;
    }
    public void UsePortal()
    {
        if (portalManager.portalNum == 2)
        {
            float distance0 = Vector2.Distance(characterPosition.position, portal0.transform.position);
            float distance1 = Vector2.Distance(characterPosition.position, portal1.transform.position);

            if (distance0 <= canUsePortalRange || distance1 <= canUsePortalRange)
            {
                if (distance0 <= distance1)
                {
                    transform.position = portal1.transform.position;
                }
                else
                {
                    transform.position = portal0.transform.position;
                }
            }
        }
    }
    public void SpawnPortal0()
    {
        if (!portalManager.portal0Spawned)
        {
            portalManager.portal0Spawned = true;

            portals[0].SetActive(true);
            portals[0].transform.position = portalSpawnPosition.position;

            portalManager.portalNumPlus();
        }
        else
        {
            portals[0].transform.position = portalSpawnPosition.position;
        }

    }
    public void SpawnPortal1()
    {
        if (!portalManager.portal1Spawned)
        {
            portalManager.portal1Spawned = true;

            portals[1].SetActive(true);
            portals[1].transform.position = portalSpawnPosition.position;

            portal0.otherPortalPosition = portal1.transform;
            portal1.otherPortalPosition = portal0.transform;

            portalManager.portalNumPlus();
        }
        else
        {
            portals[1].transform.position = portalSpawnPosition.position;
        }
    }
}
