using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour
{

    List<GameObject> _objList = new List<GameObject>();

    bool _dashFlag = false;

    int _enemyIndexCount = 0;

    [SerializeField]
    GameObject _hitParticle = null;
    [SerializeField]
    GameObject _ConnectParticle = null;
    [SerializeField]
    GameObject _lineParticle = null;

    [SerializeField]
    GameObject _fieldObj = null;

    [SerializeField]
    float _normalSpeed = 0.3f;
    [SerializeField]
    float _dashSpeed = 1.5f;

    [SerializeField]
    string _horizontalName;
    [SerializeField]
    string _verticalName;
    [SerializeField]
    string _buttonName;

    [SerializeField]
    Rigidbody _rigidbody;

    [SerializeField]
    GameObject _lineObj;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_dashFlag)
        {
            if (_objList.Count == _enemyIndexCount)
            {
                _objList.Clear();
                _enemyIndexCount = 0;
                _dashFlag = false;
            }
            else
            {
                if(_objList[_enemyIndexCount] == null)
                {
                    _dashFlag = false;
                    _enemyIndexCount = 0;
                    _objList.Clear();
                    return;
                }
                Vector3 vector = _objList[_enemyIndexCount].transform.position - transform.position;

                const int MAX_PARTICLE = 10;
                Vector3 value = vector.normalized * _dashSpeed / MAX_PARTICLE;
                for (int i = 0; i < MAX_PARTICLE; ++i)
                {
                    transform.position += value;
                    //_rigidbody.velocity += vector.normalized * _dashSpeed;
                    var obj = Instantiate(_lineParticle);
                    obj.transform.position = transform.position;
                }
            }
            Vector3 pos = transform.position;
            MoveLimit(pos);
        }
        else
        {
            float x = Input.GetAxis(_horizontalName);
            float z = Input.GetAxis(_verticalName);
            //transform.eulerAngles = Vector3.zero;

            Vector3 pos = transform.position;
            pos += new Vector3(x * _normalSpeed, 0.0f, -z * _normalSpeed);

            MoveLimit(pos);

            if (Input.GetButton(_buttonName) && _objList.Count > 0)
            {
                _dashFlag = true;
            }

            //Vector3 pos = transform.position;
            //if (Input.GetKey(KeyCode.W))
            //{
            //    pos.z += 1.0f / 5;
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    pos.z -= 1.0f / 5;
            //}

            //if (Input.GetKey(KeyCode.A))
            //{
            //    pos.x -= 1.0f / 5;
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    pos.x += 1.0f / 5;
            //}

            //transform.position = pos;

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    _flag = true;
            //}
        }
    }

    //public void OnCollisionEnter(Collision collision)
    //{
        
    //}
    void MoveLimit(Vector3 pos)
    {
        if (pos.x > _fieldObj.transform.localScale.x / 2.0f)
        {
            pos.x = _fieldObj.transform.localScale.x / 2.0f;
        }
        if (pos.x < -_fieldObj.transform.localScale.x / 2.0f)
        {
            pos.x = -_fieldObj.transform.localScale.x / 2.0f;
        }

        if (pos.z > _fieldObj.transform.localScale.z / 2.0f)
        {
            pos.z = _fieldObj.transform.localScale.z / 2.0f;
        }
        if (pos.z < -_fieldObj.transform.localScale.z / 2.0f)
        {
            pos.z = -_fieldObj.transform.localScale.z / 2.0f;
        }

        if (pos.y < _fieldObj.transform.localScale.y)
        {
            pos.y = _fieldObj.transform.localScale.y;
        }

        transform.position = pos;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            if (_dashFlag)
            {
                if(_objList.Count < _enemyIndexCount) { return; }
                if (_objList[_enemyIndexCount] == other.gameObject)
                {
                    ++_enemyIndexCount;
                    var particle = Instantiate(_hitParticle);
                    particle.transform.position = other.gameObject.transform.position;
                    Destroy(other.gameObject);
                }                
            }
            else
            {
                foreach (var obj in _objList)
                {
                    if (obj == other.gameObject)
                    {
                        return;
                    }
                }
                //var newObjList = _objList.Where(obj => obj != other.gameObject).ToArray();
                //_objList.FindAll(delegate (GameObject obj) { return obj != other.gameObject; });
                _objList.Add(other.gameObject);
                var particle = Instantiate(_ConnectParticle);
                particle.transform.position = other.gameObject.transform.position;

                if (_objList.Count >= 2)
                {
                    var lineObj = Instantiate(_lineObj);
                    var lineDraw = lineObj.GetComponent<LineDraw>();
                    lineDraw._beforeObj = _objList[_objList.Count - 2];
                    lineDraw._afterObj = _objList[_objList.Count - 1];
                }
            }
        }

    }
}
