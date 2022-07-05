using UnityEngine;

public class Path : MonoBehaviour
{
    private Pathways pathways;

    // Start is called before the first frame update
    private void Start()
    {
        pathways = Pathways.Instance();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.back * pathways.Speed() * Time.deltaTime);
        transform.position = new Vector3(pathways.HorizontalPosition(), 0, transform.position.z);

        if (transform.position.z < -20)
            Destroy(gameObject);        
    }

    private void OnDestroy()
    {
        if (SaveScene.isGameScene)
            pathways.CreatePath(false);
    }
}
