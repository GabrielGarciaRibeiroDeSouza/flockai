using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //variavel publica para receber o gameobject que está com o script "FlockingManager"
    public FlockingManager myManager;

    //armazena a velocidade do peixe
    float speed;

    //variavel para controla se pode virar
    bool turning = false;

    void Start()
    {
        //pega os parâmetros de velocidade do script "FlockinManager" 
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);  
    }

    
    void Update()
    {
        {
            //cria um bond (no pillar)
            Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

            //cria um raycast
            RaycastHit hit = new RaycastHit();

            //cria a direção com base na posição atual menos a posição do "myManager"
            Vector3 direction = myManager.transform.position - transform.position;

            
            if (!b.Contains(transform.position))
            {
                //o peixe precisa virar
                turning = true;

                //atualiza a direção
                direction = myManager.transform.position - transform.position;
            }
            //se o raycast colidir em alguma coisa
            else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
            {
                //o peixe pode virar
                turning = true;
                //atualiza a direção com o vetor reflect, que faz o pexei mudar de direção para não colidir o pilar
                direction = Vector3.Reflect(this.transform.forward, hit.normal);
            }
            else
                //o peixe não precisa virar
                turning = false;

            //se o peixe precisa virar
            if (turning)
            {
                //faz o peixe virar suavemente para a direção atual
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);

            }
            //se o peixe não precisa virar
            else
            {
               
                if (Random.Range(0, 100) < 10)
                    //define a velocidade
                    speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);

                if (Random.Range(0, 100) < 20)
                    //chama a função
                    ApplyRules();
            }
            //faz o peixe se mover para frente
            transform.Translate(0, 0, Time.deltaTime * speed);
        }
    }
    //função aplica a regra do flocking
    void ApplyRules()
    {
        {
            //cria um array de objetos
            GameObject[] gos;

            //adiciona todos os objetos do array "allFish"
            gos = myManager.allFish;

            Vector3 vcentre = Vector3.zero;
            Vector3 vavoid = Vector3.zero;
            float gSpeed = 0.01f;
            float nDistance;
            int groupSize = 0;

            //pega o array de objetos com os peixes
            foreach (GameObject go in gos)
            {
                //verifica se é um "gameobject"
                if (go != this.gameObject)
                {
                    //armazena a distancia entre o objeto "go" e a posição desse objeto
                    nDistance = Vector3.Distance(go.transform.position, this.transform.position);

                    //verifica se a distancia de "nDistance" é menor ou igual a distancia entre os peixes
                    if (nDistance <= myManager.neighbourDistance)
                    {
                        //adiciona a posição do peixe atual como o centro
                        vcentre += go.transform.position;

                        //adiciona mais um no tamanho do grupo
                        groupSize++;

                        //se a distancia é menor que 1
                        if (nDistance < 1.0f)
                        {
                            //faz o peixe virar da direção armazena em "vavoid"
                            vavoid = vavoid + (this.transform.position - go.transform.position);
                        }
                        //cria um novo grupo
                        Flock anotherFlock = go.GetComponent<Flock>();
                        //velocidade do grupo
                        gSpeed = gSpeed + anotherFlock.speed;
                    }
                }
            }
            //se o tamanho do grupo for maior que 0
            if (groupSize > 0)
            {
                //define a posição do centro do cardume
                vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);

                //define a velocidade do grupo
                speed = gSpeed / groupSize;

                //define a direção
                Vector3 direction = (vcentre + vavoid) - transform.position;

                //se tiver uma direção
                if (direction != Vector3.zero)
                    //faz o grupo rotacionar na direção do grupo(direction)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
