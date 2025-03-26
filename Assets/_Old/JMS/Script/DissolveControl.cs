using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveControl : MonoBehaviour
{
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    [SerializeField] private Material[] materials;
    [SerializeField] private Material[] materials2;
    [SerializeField] private MeshRenderer[] meshRenderers;
    // Start is called before the first frame update
    private void Awake()
    {
            materials = meshRenderers[0].materials;
            materials2 = meshRenderers[1].materials;
    }
    // Update is called once per frame
    public void StartBuring()
    {
        StartCoroutine(DissolveCon());
        StartCoroutine(DissolveCon2());
    }

    IEnumerator DissolveCon()
    {
        if(materials.Length > 0)
        {
            float counter = 0;
            while (materials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for(int i=0; i< materials.Length; i++)
                {
                    materials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
    IEnumerator DissolveCon2()
    {
        if (materials2.Length > 0)
        {
            float counter = 0;
            while (materials2[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < materials2.Length; i++)
                {
                    materials2[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
        gameObject.SetActive(false);
    }
}
