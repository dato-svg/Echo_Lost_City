using UnityEngine;

public class LastOne : MonoBehaviour
{
    public CheckThis chesThis;

    public void Start()
    {
        chesThis = new CheckThis();
        chesThis.Start();
        Debug.Log(chesThis.items);
    }


    public void CheckCunnentObject()
    {

    }
}
