using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenCharacterMenu : MonoBehaviour
{
    BodyConstructor bodyConstructor;

    [SerializeField] GameObject model;

    [SerializeField] GameObject mainBodyPrefab;
    [SerializeField] SkinnedMeshRenderer[] bodyParts;
    private List<SkinnedMeshRenderer> heads, body, legs, feet;
    Transform player;
    int[] currentBodyParts;

    private void OnEnable()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().transform;
        if (currentBodyParts == null)
        {
            PlayerData data = SaveSystem.LoadPlayer();
            currentBodyParts = new int[4] { 0, 0, 0, 0 };
            if (data != null && data.clothes != null)
            {
                currentBodyParts = new int[4] { data.clothes[0], data.clothes[1], data.clothes[2], data.clothes[3] };
            }
        }

        DestroyImmediate(model);
        model = Instantiate(mainBodyPrefab, player);

        player.GetComponent<RagdollOnOff>().GetNewHips();
        ArmsNewHips(player);

        player.gameObject.SetActive(false);
        Time.timeScale = 1;
        player.gameObject.SetActive(true);
        Time.timeScale = 0;

        bodyParts = new SkinnedMeshRenderer[0];
        bodyParts = player.GetComponentsInChildren<SkinnedMeshRenderer>();

        heads = new List<SkinnedMeshRenderer>();
        body = new List<SkinnedMeshRenderer>();
        legs = new List<SkinnedMeshRenderer>();
        feet = new List<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer part in bodyParts)
        {
            if (part.name.Contains("Head"))
            {
                heads.Add(part);
            }
            else if (part.name.Contains("Body"))
            {
                body.Add(part);
            }
            else if (part.name.Contains("Legs"))
            {
                legs.Add(part);
            }
            else if (part.name.Contains("Feet"))
            {
                feet.Add(part);
            }
            part.gameObject.SetActive(false);
        }

        player.GetComponentInParent<WeaponLoader>().ReAssignWeaponSlots();

        try
        {
            heads[currentBodyParts[0]].gameObject.SetActive(true);
            body[currentBodyParts[1]].gameObject.SetActive(true);
            legs[currentBodyParts[2]].gameObject.SetActive(true);
            feet[currentBodyParts[3]].gameObject.SetActive(true);
        }
        catch
        {
            heads[0].gameObject.SetActive(true);
            body[0].gameObject.SetActive(true);
            legs[0].gameObject.SetActive(true);
            feet[0].gameObject.SetActive(true);
        }

    }

    private void OnDisable()
    {
        player.gameObject.SetActive(false);
        foreach (SkinnedMeshRenderer parts in bodyParts)
        {
            if (parts.gameObject == null || parts.gameObject.activeSelf == false)
            {
                Destroy(parts.gameObject);
            }
        }
        player.gameObject.SetActive(true);
        player.GetComponentInParent<PlayerMainStateManager>().StoreCostume(currentBodyParts);
    }

    private void Update()
    {
        if (Input.GetButtonDown("L1") || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            if (current == null) return;

            Debug.Log(current.name);

            BodyLast(current.name);
        }
        else if (Input.GetButtonDown("R1") || Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            if (current == null) return;

            Debug.Log(current.name);

            BodyNext(current.name);
        }
    }

    void ArmsNewHips(Transform player)
    {
        ArmMoverIK[] arms = player.GetComponents<ArmMoverIK>();
        foreach (ArmMoverIK arm in arms)
        {
            arm.NewOrigin();
        }
    }

    private void BodyModifier(List<SkinnedMeshRenderer> part, ref int index, int valIncrease)
    {
        part[index].gameObject.SetActive(false);

        index += valIncrease;
        if (index >= part.Count)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = part.Count - 1;
        }

        part[index].gameObject.SetActive(true);
    }
    void BodyModifier(string bodyPart, int val)
    {
        bodyPart = bodyPart.ToLower();

        if (bodyPart == "head") BodyModifier(heads, ref currentBodyParts[0], val);
        else if (bodyPart == "body") BodyModifier(body, ref currentBodyParts[1], val);
        else if (bodyPart == "leg") BodyModifier(legs, ref currentBodyParts[2], val);
        else BodyModifier(feet, ref currentBodyParts[3], val);
    }

    public void BodyLast(string bodyPart)
    {
        BodyModifier(bodyPart, -1);
    }
    public void BodyNext(string bodyPart)
    {
        BodyModifier(bodyPart, +1);
    }
    public void BodyRandom(string bodyPart)
    {
        BodyModifier(bodyPart, UnityEngine.Random.Range(0, 11));
    }

}
