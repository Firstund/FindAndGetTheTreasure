using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject[] despawnIts = new GameObject[0];
    [SerializeField]
    private GameObject[] spawnIts = new GameObject[0];

    public void OnClick()
    {
        spawnIts.ForEach(x => { if (x != null) { x.SetActive(true); } });
        despawnIts.ForEach(x => { if (x != null) { x.SetActive(false); } });
    }
}
