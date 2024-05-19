using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingController : MonoBehaviour
{
    public MeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinnedMaterials;
    // Start is called before the first frame update
    void Start()
    {
        if(skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;
    }

    // Update is called once per frame
    void Update()
    {
        //Canviar al morir i no al clicar espai
        if(Input.GetKeyDown (KeyCode.Space))
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo ()
    {
        //Animació de mort aqui
        //alive = false;

        //if(animator != null)
            //animator.SetTrigger("Dead");

        if (VFXGraph != null)
        {
            VFXGraph.Play();
        }
        if(skinnedMaterials.Length > 0)
        {
            float counter = 0;
            while(skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for(int i=0; i<skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
