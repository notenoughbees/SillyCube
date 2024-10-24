using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SillyCube : MonoBehaviour
{
    /// <summary>
    /// Rules of Silly Cube:
    /// - Don't create a crash or hang the program.
    /// Your contactOtherCube() must be unique.
    /// </summary>
    #region("Framework")
    [SerializeField] string initials = "SC";

    //do not modify
    const float maxDist = 5;
    const float speed = 5;
    const float interval = 5;
    Quaternion rotationTarget;
    bool colliderReady;

    // Start is called before the first frame update
    void Start()
    {
        startCube();
        name = "Cube_" + initials;
        //Setup collision
        colliderReady = false;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (!rb) { rb = gameObject.AddComponent<Rigidbody>(); }
        if (rb) { rb.isKinematic = true; }
        Collider col = GetComponent<Collider>();
        if (col) { col.isTrigger = true; }
        StartCoroutine(changeRotation());
    }

    // Update is called once per frame
    void Update()
    {
        moveAround();
        updateCube();
        Debug.DrawLine(transform.position, Vector3.up, Color.black);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
    }

    // Move the cube by randomly rotating it, then moving directly forward at set speed.
    void moveAround()
    {
        //Time.deltatime is the time between Update frames.
        // It changes the motion to be more consistent across different speed CPUs
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    //Detects when one cube touches another. Different from OnCollisionEnter. This detects intersections only, not collision.
    private void OnTriggerEnter(Collider other)
    {
        if (colliderReady)
        {
            //Debug.Log(string.Concat(name," hit ", other.gameObject.name));  // !!!very resource-intensive when lots of cubes!!!
            contactOtherCube(other);
        }
    }

    //IEnumerators are coroutines that run outside of Update(). You can control your own timing and events in them.
    IEnumerator changeRotation()
    {
        yield return new WaitForSeconds(1);
        //dirty way to block collisons until the object has a chance to move away, otherwise potential change for recursive collision.
        colliderReady = true;
        // while(true) means this loop will run for as long as the object exists.
        while (true)
        {
            //detects if cube goes beyond 10units form the centre. Automatically creates rotation looking at centre.
            if (Vector3.Distance(transform.position, Vector3.zero) > maxDist)
            {
                rotationTarget = Quaternion.LookRotation(Vector3.zero - transform.position, Vector3.up);
            }
            else
            {
                rotationTarget = Random.rotation;
            }

            //Every while() loop should include a "yield null" or other yield type, otherwise Unity may crash if the logic never resolves.
            yield return new WaitForSeconds(Random.Range(0, interval));
        }
    }
    #endregion

    /// <summary>
    /// Edit these methods.
    /// </summary>
    #region("Editable Methods")
    void startCube()
    {
        //Sample Action
        GetComponent<Renderer>().material.color = Color.black;

        //Color purple = new Vector4(127, 63, 255, 0);
        //GetComponent<Renderer>().material.color = purple;
    }
    void updateCube()
    {
        //Sample Action
        Renderer r = GetComponent<Renderer>();
        //r.material.color = Vector4.MoveTowards(r.material.color, Color.black, Time.deltaTime);
        r.transform.localScale = Vector4.MoveTowards(r.transform.localScale, transform.localScale, Time.deltaTime); // move back to default scale
    }

    void contactOtherCube(Collider other)
    {
        //Sample Action
        //GetComponent<Renderer>().material.color = Color.red;

        //transform.localScale = transform.localScale + new Vector3(1.1f, 1f, 0.9f);

        Renderer r = GetComponent<Renderer>();
        GetComponent<Renderer>().material.color = changeColour(r);
        //transform.position = other.transform.position;

        transform.localScale = Vector4.MoveTowards(r.transform.localScale, transform.localScale + new Vector3(1, 1, 1), Time.deltaTime); // get 2x bigger on collision

    }
    #endregion



    Color changeColour(Renderer r) {
        Color currColor = r.material.color;
        int hueToChange = Random.Range(1, 4);
        Color newColor;
        switch (hueToChange) {
            case 1:
                newColor = new Vector4(Random.Range(0f, 1f), currColor.g, currColor.b, currColor.a);
                break;
            case 2:
                newColor = new Vector4(currColor.r, Random.Range(0f, 1f), currColor.b, currColor.a);
                break;
            case 3:
                newColor = new Vector4(currColor.r, currColor.g, Random.Range(0f, 1f), currColor.a);
                break;
            case 4:
                newColor = new Vector4(currColor.r, currColor.g, currColor.b, Random.Range(0f, 1f));
                break;
            default:
                newColor = currColor;
                break;
        }
        return newColor;
    }
}
