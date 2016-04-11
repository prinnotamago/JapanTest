using UnityEngine;
using System.Collections;

public class EnemyCreate : MonoBehaviour {

    [SerializeField]
    GameObject _fieldObj = null;

    [SerializeField]
    GameObject _enemyObj = null;

    float _createTime = 0.0f;

    [SerializeField]
    int MAX_NUM = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        _createTime++;
        if((int)_createTime % 10 == 0)
        {
            if(GetComponentsInChildren<Enemy>().Length < MAX_NUM)
            {
                Create();
            }
        }
    }

    void Create()
    {
        GameObject enemy = Instantiate(_enemyObj);
        enemy.transform.parent = transform;
        Vector3 scale = _fieldObj.transform.localScale / 2;
        Vector3 pos = new Vector3(Random.Range(-scale.x, scale.x), 1.0f, Random.Range(-scale.z, scale.z));
        enemy.transform.position = pos;
    }
}
