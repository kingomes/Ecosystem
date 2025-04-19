using UnityEngine;

public class SpawnHives : MonoBehaviour
{
    private Terrain terrain;
    private TreeInstance[] trees;
    [SerializeField] private GameObject hive;
    [SerializeField] private GameObject bee;
    [SerializeField] private Collider collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terrain = this.GetComponent<Terrain>();
        trees = terrain.terrainData.treeInstances;
        Spawn();
    }

    public void Spawn()
    {
        foreach (TreeInstance tree in trees)
        {
            if (tree.prototypeIndex == 0) // it is a tree
            {
                float spawnChance = Random.value;
                if (spawnChance > 0.9)
                {
                    Vector3 worldPosHive = new Vector3(
                        tree.position.x * terrain.terrainData.size.x + 0.5f,
                        tree.position.y * terrain.terrainData.size.y + 1f,
                        tree.position.z * terrain.terrainData.size.z
                    );

                    hive.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    bee.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                    GameObject hiveInstance = Instantiate(hive, worldPosHive, Quaternion.Euler(-90, 0, 0));

                    float numBeesPerTree = Random.Range(1, 3);
                    for (int i = 0; i < numBeesPerTree; i++)
                    {
                        float xFromTree = Random.Range(-5, 5);
                        Vector3 worldPosBee = new Vector3(
                            tree.position.x * terrain.terrainData.size.x + xFromTree,
                            tree.position.y * terrain.terrainData.size.y + 1f,
                            tree.position.z * terrain.terrainData.size.z
                        );

                        GameObject beeInstance = Instantiate(bee, worldPosBee, Quaternion.identity);
                        beeInstance.AddComponent<Swarm>();
                        beeInstance.AddComponent<Bee>();
                        beeInstance.AddComponent<CollectPollen>();
                        beeInstance.AddComponent<CapsuleCollider>();
                        CapsuleCollider triggerCollider = beeInstance.GetComponent<CapsuleCollider>();
                        triggerCollider.isTrigger = true;
                    }

                    hiveInstance.tag = "Hive";
                    hiveInstance.AddComponent<Hive>();

                    hiveInstance.AddComponent<SphereCollider>();
                    collider = hiveInstance.GetComponent<SphereCollider>();
                    collider.isTrigger = true;
                }
            }
        }
    }
}
