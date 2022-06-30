using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SourceType {
    Food,
    Water
}

public enum AnimalType {
    Penguin,
    Cat,
    Dog,
    Chicken,
    Lion
}

/// <summary>
/// Nutrition value is the value which the entity gains that eats this current entity
/// Nutrition needed is the total value needed to fullfill entities hunger
/// </summary>
/// <remarks></remarks>
[System.Serializable]
public struct NutritionObject
{
    public float NutritionValue;
    public float NutritionNeeded;
}

/// <summary>
/// Stucture that contains a list of predators and preys.
/// Where predators are the entities hunting on this entity and
/// where the preys are the entities being hunted on by this entity.
/// </summary>
/// <remarks></remarks>
[System.Serializable]
public struct HunterStats
{
    public List<AnimalType> Predators;
    public List<AnimalType> Prey;
}


/// <summary>
/// This class contains all data and calculations 
/// needed to secure good transitions between states.
/// It has vital information about the entity.
/// </summary>
/// <remarks></remarks>
public class EntityData : MonoBehaviour
{
    [Header("Animal type used in searching")]
    [SerializeField] private AnimalType _animalType;
    public AnimalType TypeAnimal
    {
        get { return _animalType; }
    }
    [Header("Animal statistics which can be hunted and which will hunt")]
    [SerializeField] private HunterStats _stats;
    public HunterStats Stats
    {
        get { return _stats; }
    }
    [Header("Thirst threshold in hours")]
    [SerializeField] private int _thirstThreshold;
    public int ThirstThreshold
    {
        get { return _thirstThreshold; }
    }
    [Header("Hunger threshold in hours")]
    [SerializeField] private int _hungerThreshold;
    public int HungerThreshold
    {
        get { return _hungerThreshold; }
    }

    private GameObject _currentPredator;
    public GameObject CurrentPredator
    {
        get { return _currentPredator; }
        set { _currentPredator = value; }
    }

    private GameObject _currentPrey;
    public GameObject CurrentPrey
    {
        get { return _currentPrey; }
        set { _currentPrey = value; }
    }

    private float _thirstMeter;
    public float ThirstMeter
    {
        get { return _thirstMeter; }
    }

    private float _hungerMeter;
    public float HungerMeter
    {
        get { return _hungerMeter; }
    }
    private List<Vector3> _waterSources = new List<Vector3>();
    public List<Vector3> WaterSources
    {
        get { return _waterSources; }
    }

    private List<Vector3> _foodSources = new List<Vector3>();
    public List<Vector3> FoodSources
    {
        get { return _foodSources; }
    }
    [SerializeField] private NutritionObject _nutrition;
    public NutritionObject Nutrition
    {
        get { return _nutrition; }
    }

    private DayTime _dayTime;
    private WorldGrid _grid;

    private void Start()
    {
        _thirstMeter = _thirstThreshold;
        _hungerMeter = _hungerThreshold;
        _dayTime = FindObjectOfType<DayTime>();
        _grid = FindObjectOfType<WorldGrid>();
        StartCoroutine(CalculateMeters());
    }

    private void Update()
    {
        Death();
    }


    public void AddSource(SourceType sourceType, Vector3 pos)
    {
        switch (sourceType)
        {
            case SourceType.Food:
                _foodSources.Add(pos);
                break;
            case SourceType.Water:
                _waterSources.Add(pos);
                break;
        }
    }

    /// <summary>
    /// Determines the closest position of a certain source.
    /// </summary>
    /// <param name="sourceType">A choice between food and water</param>
    /// <param name="currentPos">Current position</param>
    /// <returns>A vector 3 where the position is the closest possible source</returns>
    /// <remarks></remarks>
    public Vector3 GetClosestSource(SourceType sourceType, Vector3 currentPos)
    {
        List<Vector3> sources = new List<Vector3>();
        switch (sourceType)
        {
            case SourceType.Food:
                sources = _foodSources;
                break;
            case SourceType.Water:
                sources = _waterSources;
                break;
        }

        float minDist = float.MaxValue;
        Vector3 closest = Vector3.zero;

        foreach (Vector3 v in sources)
        {
            float dist = Vector3.Distance(currentPos, v);
            if (dist < minDist)
            {
                minDist = dist;

                if (sourceType == SourceType.Water)
                {
                    closest = _grid.GetFirstWalkableNeighbour(_grid.GetNeighboursFromNode(_grid.GetNodeFromPosition(v))).Position;
                }
                else
                {
                    closest = v;
                }


            }
        }
        return closest;
    }

    /// <summary>
    /// This function runs every second so that the meters will be decreased every second
    /// </summary>
    /// <returns>A wait for seconds function</returns>
    /// <remarks></remarks>
    private IEnumerator CalculateMeters()
    {
        while (true)
        {
            _thirstMeter -= (_thirstThreshold / (60f * _thirstThreshold));

            _hungerMeter -= (_hungerThreshold / (60f * _hungerThreshold));
            yield return new WaitForSeconds(1f);
        }
    }

    public bool HasThirst()
    {
        // Debug.Log(_thirstMeter);
        if (_thirstMeter <= 0)
        {
            return true;
        }
        return false;
    }

    public void ResetThirst()
    {
        _thirstMeter = _thirstThreshold;

    }

    /// <summary>
    /// Calculates the percentage of how much nutrition will be added,
    /// and adds this to the hunger meter
    /// </summary>
    /// <param name="obj">The entity which is being consumed</param>
    /// <remarks></remarks>
    public void AddNutrtion(GameObject obj)
    {
        NutritionObject nutrition = obj.GetComponent<EntityData>().Nutrition;
        float nutritionPercentage = (100f / _nutrition.NutritionNeeded) * nutrition.NutritionValue;
        _hungerMeter += (_hungerThreshold / 100f) * nutritionPercentage;
    }

    public bool HasHunger()
    {
        if (_hungerMeter <= 0)
        {
            return true;
        }
        return false;
    }

    public bool IsPredator(GameObject obj)
    {
        EntityData data = obj.GetComponent<EntityData>();
        if (_stats.Predators.Contains(data.TypeAnimal))
        {
            return true;
        }
        return false;
    }

    public bool IsPrey(GameObject obj)
    {
        EntityData data = obj.GetComponent<EntityData>();
        if (_stats.Prey.Contains(data.TypeAnimal))
        {
            return true;
        }

        return false;
    }

    public void RemoveSource(SourceType sourceType, Vector3 position)
    {
        switch (sourceType)
        {
            case SourceType.Food:
                _foodSources.RemoveAt(_foodSources.IndexOf(position));
                break;
        }
    }

    public bool CanConsume(GameObject obj)
    {
        NutritionObject nutrition = obj.GetComponent<EntityData>().Nutrition;
        float hungerPercentage = (100f / _hungerThreshold) * _hungerMeter;
        float nutritionPercentage = (100f / _nutrition.NutritionNeeded) * nutrition.NutritionValue;
        if (100f - hungerPercentage > nutritionPercentage)
        {
            return true;
        }

        return false;
    }

    private void Death()
    {
        if (_thirstMeter < -_thirstThreshold * 2 || _hungerMeter < -_hungerThreshold * 5)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Entity")
        {
            Physics.IgnoreCollision(other.collider, transform.GetComponent<Collider>());
        }
    }
}
