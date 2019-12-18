using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationController : MonoBehaviour
{
    List<Car> cars;
    Car bestCar;

    private int carsAlive;
    private float totalPopulationFitness;

    private int generationCount;
    [SerializeField] private Text generationCountText = null;

    [SerializeField] private int population = 0;
    [SerializeField] private GameObject carPrefab = null;
    [SerializeField] public GameObject finishLine = null;

    [SerializeField] private Transform populationTransform = null;

    public static PopulationController instance = null;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(instance);

        generationCount = 1;
        generationCountText.text = "Generation: " + generationCount.ToString();

        FirstGeneration();
    }

    private void Update()
    {
        if(carsAlive <= 0)
        {
            NewGeneration();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var car in cars)
            {
                if (car.gameObject.activeSelf)
                {
                    car.KillCar();
                }
            }
            NewGeneration();
        }
    }

    private void FirstGeneration()
    {
        cars = new List<Car>();

        for (int i = 0; i < population; i++)
        {
            Car newCar = Instantiate(carPrefab, populationTransform).GetComponent<Car>();
            newCar.Initialize();
            cars.Add(newCar);
        }

        carsAlive = population;
        totalPopulationFitness = 0f;
        generationCount++;

        bestCar = cars[0];
    }

    private void NewGeneration()
    {
        List<Car> carsSaved = new List<Car>(cars);

        bestCar = GetBestCar();

        for (int i = 0; i < population; i++)
        {
            Car parentOne = PickOne(carsSaved);
            Car parentTwo = PickOne(carsSaved);

            DNA newDNA = parentOne.GetDNA().CrossOver(parentTwo.GetDNA());
            DNA mutatedDNA = newDNA.Mutate();
            cars[i].Delete();
            GameObject newCar = Instantiate(carPrefab, populationTransform);
            cars[i] = newCar.GetComponent<Car>();
            cars[i].Initialize();
            cars[i].NewDNA(mutatedDNA);
        }

        carsSaved.Clear();

        totalPopulationFitness = 0f;
        carsAlive = population;
        generationCount++;
        generationCountText.text = "Generation: " + generationCount.ToString();
    }

    private Car GetBestCar()
    {
        int maxI = 0;
        float maxFitness = cars[maxI].GetFitness();
        for (int i = 1; i < cars.Count; i++)
        {
            if (cars[i].GetFitness() > maxFitness)
            {
                maxFitness = cars[i].GetFitness();
                maxI = i;
            }
        }

        return cars[maxI];
    }

    private Car PickOne(List<Car> carsSaved)
    {
        int index = 0;
        float r = Random.Range(0f, 1f);
        int beSafe = 0;
        while (r > 0 || beSafe > 100000)
        {
            r -= carsSaved[index].GetFitness() / totalPopulationFitness;
            index++; if (index > population - 1) index = population - 1;
            beSafe++;
        }
        index--; if (index < 0) index = 0;

        return carsSaved[index];
    }

    public void UpdateTotalFitness(float fitness)
    {
        totalPopulationFitness += fitness;
    }

    public void UpdateCarsAlive()
    {
        carsAlive--;
    }

    public Car GetNextFollowTarget()
    {
        foreach (var car in cars)
        {
            if (car.gameObject.activeSelf) return car;
        }

        return null;
    }
}
