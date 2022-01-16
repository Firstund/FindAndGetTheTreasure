using UnityEngine;
using Cinemachine;

public class BoundingShape2DCollider : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.cinemachineVirtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = GetComponent<CompositeCollider2D>();
    }
}
