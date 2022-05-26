using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyConstructor : MonoBehaviour
{
    [SerializeField] GameObject mainBodyPrefab;
    [SerializeField] SkinnedMeshRenderer[] bodyParts;
    private List<SkinnedMeshRenderer> heads, body, legs, feet;
    private int headPointer, bodyPointer, legPointer, footPointer;

    Transform player;
    private bool returnVals = false;

    // Start is called before the first frame update
    void Start()
    {        
    }

    public BodyConstructor(ref int[] bodyPointers, GameObject currentBody, GameObject newBody)
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        Destroy(currentBody);
        currentBody = Instantiate(newBody, player);

        bodyParts = new SkinnedMeshRenderer[0];
        bodyParts = player.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        heads = new List<SkinnedMeshRenderer>();
        body = new List<SkinnedMeshRenderer>();
        legs = new List<SkinnedMeshRenderer>();
        feet = new List<SkinnedMeshRenderer>();

        headPointer = bodyPointers[0];
        bodyPointer = bodyPointers[1];
        legPointer = bodyPointers[2];
        footPointer = bodyPointers[3];

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

        heads[headPointer].gameObject.SetActive(true);
        body[bodyPointer].gameObject.SetActive(true);
        legs[legPointer].gameObject.SetActive(true);
        feet[footPointer].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BodyModifier("head");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BodyModifier("body");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BodyModifier("leg");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BodyModifier("foot");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ReturnBody();
        }
    }

    private void BodyModifier(List<SkinnedMeshRenderer> part, ref int index)
    {
        part[index].gameObject.SetActive(false);

        index++;
        if (index >= part.Count)
        {
            index = 0;
        }

        part[index].gameObject.SetActive(true);
    }
    public void BodyModifier(string bodyPart)
    {
        bodyPart = bodyPart.ToLower();

        if (bodyPart == "head") BodyModifier(heads, ref headPointer);
        else if (bodyPart == "body") BodyModifier(body, ref bodyPointer);
        else if (bodyPart == "leg") BodyModifier(legs, ref legPointer);
        else BodyModifier(feet, ref footPointer);

    }

    public int[] ReturnBody()
    {
        int[] vals = new int[4] {headPointer, bodyPointer, legPointer, footPointer};

        foreach (SkinnedMeshRenderer parts in bodyParts)
        {
            if (parts.gameObject.activeSelf == false)
            {
                Destroy(parts.gameObject);
            }
        }

        return vals;
    }
}
