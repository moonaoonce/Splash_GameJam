using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rb;

    [Header("Force to Apply")]
    [SerializeField] private float m_maxForce = 5f;

    private void Awake ()
    {
        m_rb = GetComponent<Rigidbody2D>();
        TouchArea.onButtonRelease += ApplyInitialForceAtPosition;
    }

    private void OnDestroy ()
    {
        TouchArea.onButtonRelease -= ApplyInitialForceAtPosition;
    }

    private void ApplyInitialForceAtPosition (Vector2 _forceDirection, float _forceDelta)
    {
        Debug.Log("ForceDelta: " + _forceDelta);
        Debug.Log("Both: " + (m_maxForce * _forceDelta));
        m_rb.AddForce(-_forceDirection * (m_maxForce * _forceDelta), ForceMode2D.Impulse);
    }
}
