using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLoader : MonoBehaviour
{
    [SerializeField] Transform weaponSlotL;
    [SerializeField] Transform weaponSlotR;

    [SerializeField] WeaponStore[] currentWeapons;
    int index;
    GameObject[] activeWeapons;

    void Start()
    {
        activeWeapons = new GameObject[currentWeapons.Length];

        ReAssignWeaponSlots();
        //ReLoadCurrentWeapon(currentWeapons);
    }

    public void ReAssignWeaponSlots()
    {
        weaponSlotL = null;
        weaponSlotR = null;

        GameObject[] slots = GameObject.FindGameObjectsWithTag("DoNotBlend");
        foreach (GameObject slot in slots)
        {
            if (slot.transform.IsChildOf(transform))
            {
                if (weaponSlotL == null) weaponSlotL = slot.transform;
                else weaponSlotR = slot.transform;
            }

            if (weaponSlotR != null && weaponSlotL != null)
            {
                break;
            }
        }

        ReLoadCurrentWeapon(currentWeapons);
    }

    public void ReLoadCurrentWeapon(WeaponStore[] weapon)
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            Transform weaponSlot;
            if (currentWeapons[i].weaponSlot == 0) weaponSlot = weaponSlotL;
            else weaponSlot = weaponSlotR;
            activeWeapons[i] = Instantiate(currentWeapons[i].weaponPrefab, weaponSlot);
            activeWeapons[i].SetActive(false);
        }
        SwapWeapons(index);
    }

    public void SwapWeapons(int val)
    {
        Debug.Log(activeWeapons[index].name);

        activeWeapons[index].gameObject.SetActive(false);
        index = val;
        activeWeapons[index].gameObject.SetActive(true);

        Transform weaponSlot;
        if (currentWeapons[index].weaponSlot == 0) weaponSlot = weaponSlotL;
        else weaponSlot = weaponSlotR;

        weaponSlot.localPosition = currentWeapons[index].weaponPrefab.transform.position;
        weaponSlot.localRotation = currentWeapons[index].weaponPrefab.transform.rotation;
        activeWeapons[index].transform.localPosition = Vector3.zero;
        activeWeapons[index].transform.localRotation = Quaternion.identity;
    }

    public void FadeInWeapon() => currentWeapons[index].DissolveIn();
    public void FadeOutWeapon() => currentWeapons[index].DissolveOut();
}
