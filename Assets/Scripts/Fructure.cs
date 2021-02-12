using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fructure : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeOut()
    {
        Rigidbody[] transforms = transform.GetComponentsInChildren<Rigidbody>();

        foreach (var item in transforms)
        {
            item.AddExplosionForce(.25f+(Random.Range(.125f,.25f)),transform.position,1,.125f+Random.Range(.025f,.1f),ForceMode.Impulse);
        }
        yield return new WaitForSeconds(.75f);
        while (transforms[0].transform.localScale.x >.1f)
        {
            foreach (var item in transforms)
            {
                item.transform.localScale-=Vector3.one*.02f;
            }
            yield return new WaitForSeconds(.01f);
        }

        Destroy(gameObject);
    }
}
