using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUI : MonoBehaviour
{
    [SerializeField] HealthDamage boosHealth;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(boosHealth.healthPoint.Value / 1000f, 1f, 1f);
    }
}
