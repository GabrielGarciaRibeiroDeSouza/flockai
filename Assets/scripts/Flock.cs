using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //variavel publica para receber o gameobject que est� com o script "FlockingManager"
    public FlockingManager myManeger;

    //armazena a velocidade do peixe
    float speed;

    void Start()
    {
        //pega os par�metros de velocidade do script "FlockinManager" 
        speed = Random.Range(myManeger.minSpeed, myManeger.maxSpeed);  
    }

    
    void Update()
    {
        //aplica as regras do flocking
        ApplyRules();

        //faz o gameobject se mover no eixo Z
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    void ApplyRules()
    {
        //cria um array de objetos
        GameObject[] gos;
        //adiciona todos os objetos do array "allFish"
        gos = myManeger.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        //pega o array de objetos com os peixes
        foreach (GameObject go in gos)
        {
            //verifica se � um "gameobjetc"
            if (go != this.gameObject)
            {
                //armazena a distancia entre o objeto "go" e a posi��o desse objeto
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);

                //verifica se a distancia de "nDistance" � menor ou igual a distancia entre os peixes
                if (nDistance <= myManeger.neighbourDistance)
                {
                    //adiciona a posi��o do peixe atual como o centro
                    vcentre += go.transform.position;
                    //adiciona mais um no tamanho do grupo
                    groupSize++;

                    //se a distancia � menor que 1
                    if (nDistance < 1f)
                    {
                        //armazena a distance entre esse objeto e o objeto "go"
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    //cria um novo grupo
                    Flock anotherFlock = go.transform.GetComponent<Flock>();
                    //limita a velocidade do grupo
                    if (gSpeed < 40)
                    {
                        //adiciona velocidade no grupo
                        gSpeed = gSpeed + anotherFlock.speed;
                    }
                }
            }
        }
        //se o tamnaho do grupo for maior que 0
        if (groupSize > 0)
        {
            //define a posi��o do centro do grupo
            vcentre = vcentre / groupSize;
            //define a velocidade do grupo
            speed = gSpeed / groupSize;
            //define a dire��o
            Vector3 direction = (vcentre + vavoid) - transform.position;

            //se tiver uma dire��o
            if (direction != Vector3.zero)
            {
                //faz o grupo rotacionar na dire��o do grupo(direction)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManeger.rotationSpeed * Time.deltaTime);
            }
        }
        
    }
}
