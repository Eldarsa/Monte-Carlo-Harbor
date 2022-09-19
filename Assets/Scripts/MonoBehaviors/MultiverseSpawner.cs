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
    [SerializeField] private int visibleUniverseCount = 1;

    private BlobAssetStore blob;

    private float _universeWidth;
    private float _universeLength;

    // Start is called before the first frame update
    void Start()
    {

        // Get the universe configurations
        UniverseConfig config = universePrefab.GetComponent<UniverseConfig>();
        _universeWidth = config.universeWidth;
        _universeLength = config.universeLength;

        // Fetch the necessary components from the universe prefab
        Transform universeSurface = universePrefab.transform.Find("Surface");

        // Set the universe parameters based on UniverseConfig
        float _universeXScale = _universeWidth * 0.1f;
        float _universeZScale = _universeLength * 0.1f;
        universeSurface.GetComponent<Transform>().localScale = new Vector3(_universeXScale, 1, _universeZScale);

        // Setup EntityManager and make an entity instance of our universe prefab
        blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);

        // Gives us entity from a game object
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(universePrefab, settings);

        var universeCopy = Object.Instantiate(universePrefab);
        disableRendering(universeCopy);
        var invisibleEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(universeCopy, settings);

        // We have to instantiate the entities through a manager
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;



        int counter = 0;
        for(int x = 0; x < xGridSize; x++)
        {
            for(int z = 0; z < zGridSize; z++)
            {
                Entity instance;
                if(counter < visibleUniverseCount){
                    instance = entityManager.Instantiate(entity);
                }else{
                    //instance = entityManager.Instantiate(invisibleEntity);
                    instance = entityManager.Instantiate(invisibleEntity);
                }

                //var instance = entityManager.Instantiate(entity);

                float3 position = new float3(x * (_universeWidth + spacing), 0, z * (_universeLength + spacing));
                entityManager.SetComponentData(instance, new Translation {Value = position});
                Debug.Log("Position: " + position.ToString());

                counter++;
            }
        }
    }

    private void OnDestroy() {
        blob.Dispose();    
    }


    private GameObject disableRendering(GameObject gameObject) 
    {
        Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>(true);
        foreach(Renderer r in rs){
            r.enabled = false;
        }

        return gameObject;
    }
}