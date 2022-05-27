using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStore : MonoBehaviour
{
    public string weaponName;

    public string weaponDesc;

    public GameObject weaponPrefab;
    public Renderer weaponRenderer;

    public Weapon weapon;

    [Range(0, 1)] public int weaponSlot;

    private float currentDissolve = -1;

#if UNITY_EDITOR
    private void OnValidate()
    {
        weaponPrefab = this.gameObject;
        weaponRenderer = this.gameObject.GetComponent<Renderer>();
    }
#endif

    public void OnEnable()
    {
        currentDissolve = weaponRenderer.sharedMaterial.GetFloat("_Dissolve");
    }

    public void DissolveOut()
    {
        Debug.Log("DissolveOutCall");
        currentDissolve = (float)Math.Round(currentDissolve = Mathf.Lerp(currentDissolve, 1, 2 * Time.deltaTime), 3);
        weaponRenderer.sharedMaterial.SetFloat("_Dissolve", currentDissolve);
    }

    public void DissolveIn()
    {
        Debug.Log("DissolveInCall");
        currentDissolve = (float)Math.Round(currentDissolve = Mathf.Lerp(currentDissolve, 0, 2 * Time.deltaTime), 3);
        weaponRenderer.sharedMaterial.SetFloat("_Dissolve", currentDissolve);
    }

    public void DissolveOverride(int val)
    {
        currentDissolve = Mathf.Clamp01(val);
        weaponRenderer.sharedMaterial.SetFloat("_Dissolve", currentDissolve);
    }
}
