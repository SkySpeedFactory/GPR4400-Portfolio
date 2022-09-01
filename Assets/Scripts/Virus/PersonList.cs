using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonList : MonoBehaviour
{
    [SerializeField] int xRange;
    [SerializeField] int zRange;
    [SerializeField] int spawnAmount;

    public UnityEngine.GameObject personPrefab;

    private List<UnityEngine.GameObject> personList = new List<UnityEngine.GameObject>();//change to private

    private static PersonList _instance;
    public static PersonList Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        CreatePersons(spawnAmount);
    }
    
    public void CreatePersons(int persons)
    {        
        for (int i = 0; i < persons; i++)
        {
            int randomx = Random.Range(-xRange, xRange);
            int randomz = Random.Range(-zRange, zRange);
            UnityEngine.GameObject person = Instantiate(personPrefab, new Vector3(randomx, 0, randomz), Quaternion.identity);
            person.transform.SetParent(this.transform);
            personList.Add(person);
        }
    }
    
    public void RemoveFromUnitList(UnityEngine.GameObject person)
    {
        personList.Remove(person);
    }
    public List<UnityEngine.GameObject> GetListOfPersons()
    {
        return personList;
    }
}
