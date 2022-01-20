using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchArea : MonoBehaviour
{
    [Header("Power bar settings")]
    [SerializeField] private GameObject m_powerGauge;
    [SerializeField] private Image m_fillMask;
    [Range(0, 1)]
    [SerializeField] private float m_fillAmount = 0f;

    [Header("Mouse magnitude")]
    [SerializeField] private float m_maxMagnitude = 2f;
    [Min(0.1f)]
    [SerializeField] private float m_minMagnitude = 0.15f;

    private bool m_isTouching = false;
    private Vector2 m_direction;

    public static Action<Vector2, float> onButtonRelease;

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.transform.CompareTag("Player") && !m_isTouching)
            {
                StartCoroutine(PowerGaugeCR(hit.transform));
            }
        }
        else if (Input.GetMouseButtonUp(0) && m_isTouching)
            m_isTouching = false;
    }

    private IEnumerator PowerGaugeCR(Transform _objTf)
    {
        m_isTouching = true;
        m_powerGauge.SetActive(true);
        m_powerGauge.transform.position = _objTf.position;

        while (m_isTouching)
        {
            RotatePowerGauge(_objTf);
            ChargePowerGauge(_objTf);
            yield return null;
        }

        if (m_fillAmount > 0)
            onButtonRelease?.Invoke(m_direction, m_fillAmount);

        HidePowerGauge();
        yield break;
    }

    private void RotatePowerGauge (Transform _objTf)
    {
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(m_powerGauge.transform.position);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        m_powerGauge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void ChargePowerGauge (Transform _objTf)
    {
        m_direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_objTf.position;
        m_fillAmount = (m_direction.magnitude - m_minMagnitude) / m_maxMagnitude;
        m_fillAmount = Mathf.Clamp01(m_fillAmount);
        m_fillMask.fillAmount = m_fillAmount;
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void HidePowerGauge()
    {
        m_powerGauge.SetActive(false);
    }
}
