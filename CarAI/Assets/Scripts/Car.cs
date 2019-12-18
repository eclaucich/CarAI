using UnityEngine;

public class Car : MonoBehaviour
{
    private DNA dna;
    private NeuralNetwork brain;
    private float fitness;

    [SerializeField] private float baseSpeed = 0f;
    private float speedMultiplier;
    private float acceleration;
    private float direction;

    private float timeAlived;
    private float maxLifeTime = 20f;

    private LaserController laserController;

    private MeshRenderer meshBody;
    private MeshRenderer meshWheelFR;
    private MeshRenderer meshWheelFL;
    private MeshRenderer meshWheelBR;
    private MeshRenderer meshWheelBL;

    private void Start()
    {
        timeAlived = 0f;
        laserController = GetComponent<LaserController>();
        dna = new DNA();
        brain = new NeuralNetwork();

        meshBody = transform.GetChild(0).GetComponent<MeshRenderer>();
        meshWheelFL = transform.GetChild(1).GetComponent<MeshRenderer>();
        meshWheelFR = transform.GetChild(2).GetComponent<MeshRenderer>();
        meshWheelBL = transform.GetChild(3).GetComponent<MeshRenderer>();
        meshWheelBR = transform.GetChild(4).GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        float[] inputs = laserController.GetLasersDistances();
        float[] outputs = brain.FeedForward(inputs);
        UpdateMovement(outputs);

        transform.position += transform.forward * (baseSpeed * speedMultiplier * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime);
        transform.Rotate(new Vector3(0,direction,0));

        fitness += 0.05f;
    }

    private void UpdateMovement(float[] outputs)
    {
        if(outputs[0] >= 0.5f)
        {
            acceleration = (outputs[0] * 2f) - 1f;
        }
        else
        {
            acceleration = -(outputs[0] * 2f);
        }

        if(outputs[1] >= 0.5f)
        {
            direction = (outputs[1] * 2f) - 1f;
        }
        else
        {
            direction = -(outputs[1] * 2f);
        }

        if(outputs[2] >= 0.5f)
        {
            speedMultiplier = (outputs[2] * 4f) - 1f;  //ENTRE 1 y 3
        }
        else
        {
            speedMultiplier = 1f;
        }
    }

    public void ResetCar()
    {
        gameObject.SetActive(true);
        //transform.SetPositionAndRotation(new Vector3(0f, 0.1f, -43f), Quaternion.identity);
        fitness = 0f;
        timeAlived = 0f;
    }

    public void Initialize()
    {
        dna = new DNA();
        brain = new NeuralNetwork(dna);
        fitness = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            fitness -= fitness * 0.2f;
            KillCar();
        }
        else if (other.CompareTag("FinishLine"))
        {
            fitness *= 1.2f;
            KillCar();
        }
    }

    public void KillCar()
    {
        PopulationController.instance.UpdateCarsAlive();
        fitness += 1 / Vector3.Distance(transform.position, PopulationController.instance.finishLine.transform.position);
        PopulationController.instance.UpdateTotalFitness(fitness);
        gameObject.SetActive(false);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public DNA GetDNA()
    {
        return dna;
    }

    public float GetFitness()
    {
        return fitness;
    }

    public void NewDNA(DNA newDNA)
    {
        dna = newDNA;
    }

    public void EnableMesh()
    {
      meshBody.enabled = true;
      meshWheelFR.enabled = true;
      meshWheelFL.enabled = true;
      meshWheelBR.enabled = true;
      meshWheelBL.enabled = true;
    }

    public void DisableMesh()
    {
        meshBody.enabled = false;
        meshWheelFR.enabled = false;
        meshWheelFL.enabled = false;
        meshWheelBR.enabled = false;
        meshWheelBL.enabled = false;
    }
}
