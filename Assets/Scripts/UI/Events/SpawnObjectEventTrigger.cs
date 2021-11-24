using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectEventTrigger : MonoBehaviour, IEventTrigger
{
    private GameManager gameManager = null;
    [SerializeField]
    private GameObject spawnObject = null;
    
    [SerializeField]
    private Vector2 spawnPosition = Vector2.zero;
    private void Awake() 
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void DoEvent()
    {

    }
}
