using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MultiverseSpawner : MonoBehaviour
{
    [SerializeField] private GameObject universePrefab;
    [SerializeField] private int xGridSize;
    [SerializeField] private int zGridSize;
    [SerializeField] private int spacing;

    private BlobAssetStore blob;

    private float _universeWidth;
    private float _universeLength;

        /*
            float widthToScale = universeWidth / 0.1f;
        float lengthToScale = universeLength / 0.1f;

        Debug.Log("Burde skje noe her");

        Vector3 surfaceScale = new Vector3(widthToScale, 1, lengthToScale);
        transform.Find("Surface").transform.localScale = surfaceScale;     
        */

    // Start is called before the first frame update
    void Start()
    {
        // Setup EntityManager and make an entity instance of our universe prefab
        blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(universePrefab, settings);   // Gives us entity from a game object
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Transform universeSurface = universePrefab.transform.Find("Surface");
        _universeWidth = universeSurface.GetComponent<Transform>().localScale.x / 0.1f;
        _universeLength = universeSurface.GetComponent<Transform>().localScale.z / 0.1f;

        Debug.Log("Universe width: " + _universeWidth);

        for(int x = 0; x < xGridSize; x++)
        {
            for(int z = 0; z < zGridSize; z++)
            {
                var instance = entityManager.Instantiate(entity);

                float3 position = new float3(x * (_universeWidth + spacing), 0, z * (_universeLength + spacing));
                entityManager.SetComponentData(instance, new Translation {Value = position});
                Debug.Log("Position: " + position.ToString());
            }
        }

        entityManager.Instantiate(entity);
    }

    private void OnDestroy() {
        blob.Dispose();    
    }
}
