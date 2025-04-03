using UnityEngine;

public class SpawnPollen : MonoBehaviour
{
    private Terrain terrain;
    private TreeInstance[] trees;
    [SerializeField] private GameObject pollen;
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
            if (tree.prototypeIndex == 1) // it is a flower
            {
                float spawnChance = Random.value;
                if (spawnChance > 0.9)
                {
                    Vector3 worldPosPollen = new Vector3(
                        tree.position.x * terrain.terrainData.size.x + 0.5f,
                        tree.position.y * terrain.terrainData.size.y + 1f,
                        tree.position.z * terrain.terrainData.size.z
                    );

                    pollen.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                    GameObject pollenInstance = Instantiate(pollen, worldPosPollen, Quaternion.identity);

                    pollenInstance.tag = "Pollen";
                    pollenInstance.AddComponent<Pollen>();

                    pollenInstance.AddComponent<SphereCollider>();
                    collider = pollenInstance.GetComponent<SphereCollider>();
                    collider.isTrigger = true;
                }
            }
        }
    }
}
