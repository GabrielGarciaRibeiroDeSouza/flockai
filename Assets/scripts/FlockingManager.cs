using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    // variavel publica para receber o prefab
    public GameObject fishPrefab;

    //quantidade de peixes para instanciar
    public int numFish = 20;

    //array de objetos
    public GameObject[] allFish;

    //distancia maxima entre os objetos
    public Vector3 swinLimits = new Vector3 (5,5,5);

    //cria um vetor para 
    public Vector3 goalPos;

    //cria uma "seção" "configuração de cardume" 
    [Header("Configuração do cardume")]

    //cria uma barra com limite de 5 para modificar a velocidade minima
    [Range(0f, 5.0f)]
    //armazena a velocidade minima
    public float minSpeed;

    //cria uma barra com limite de 5 para modificar a velocidade maxima
    [Range(0f, 5.0f)]
    //armazena a velocidade maxima
    public float maxSpeed;

    //cria uma barra com limite de 10 para modificar a distância entre os peixes
    [Range(1f, 10f)]
    //armazena a distância 
    public float neighbourDistance;

    //cria uma barra com limite de 5 para modificar a velocidade de rotação
    [Range(1f,5f)]
    public float rotationSpeed;

    void Start()
    {
        //define o tamanho  do array com a variavel "numFish"
        allFish = new GameObject[numFish];

        //'for' para instanciar os peixes
        for (int i = 0; i < allFish.Length; i++)
        {
            //armazena uma posição para instanciar nela, com a uma distancia variada
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            //instancia o prefab do peixe na posição armazenada em "pos"
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            //pega o componente "Flock" do peixe
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        //pega a posição atual do gameobject que estiver portando esse script
        goalPos = this.transform.position;
    }

    
    void Update()
    {
        //atualiza a posição do vetor
        goalPos = this.transform.position;
        if (Random.Range(0, 100) < 10)
            //atualiza o vetor com os limite de posição
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                            Random.Range(-swinLimits.y, swinLimits.y),
                                                            Random.Range(-swinLimits.z, swinLimits.z));
    }
}

