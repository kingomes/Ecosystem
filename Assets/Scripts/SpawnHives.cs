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

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn()
    {
        foreach (TreeInstance tree in trees)
        {
            float spawnChance = Random.value;
            if (spawnChance > 0.9)
            {
                Vector3 worldPosHive = new Vector3(
                    tree.position.x * terrain.terrainData.size.x + 0.5f,
                    tree.position.y * terrain.terrainData.size.y + 1f,
                    tree.position.z * terrain.terrainData.size.z
                );

                Vector3 worldPosBee = new Vector3(
                    tree.position.x * terrain.terrainData.size.x + 2f,
                    tree.position.y * terrain.terrainData.size.y + 1f,
                    tree.position.z * terrain.terrainData.size.z
                );

                hive.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                bee.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                GameObject hiveInstance = Instantiate(hive, worldPosHive, Quaternion.Euler(-90, 0, 0));
                GameObject beeInstance = Instantiate(bee, worldPosBee, Quaternion.identity);

                hiveInstance.tag = "Hive";
                hiveInstance.AddComponent<Honey>();

                hiveInstance.AddComponent<SphereCollider>();
                collider = hiveInstance.GetComponent<SphereCollider>();
                collider.isTrigger = true;
            }
        }
    }
}
