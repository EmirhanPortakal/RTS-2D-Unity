using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Transform target;

    public void SetTarget(Transform followTarget)
    {
        target = followTarget;
    }

    public void UpdateHealth(float percent)
    {
        fillImage.fillAmount = percent;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + Vector3.up * 1.5f;
            transform.rotation = Quaternion.identity; // sabit tut
        }
    }
}
