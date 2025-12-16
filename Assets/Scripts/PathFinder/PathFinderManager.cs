using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderManager : MonoBehaviour
{
    [Header("Configurações do Grafo")]
    public Transform nodesParent; // Objeto pai com todos os nodes
    public Color lineColor = Color.cyan;
    public float lineWidth = 2f;

    private List<Node> nodes = new List<Node>();


    [Header("Debug")]
    [SerializeField] private bool _debug = false;
    [SerializeField] private Node Origin;
    [SerializeField] private Node Destiny;
    [SerializeField] private List<Node> _currentPath = new List<Node>();
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _animationSpeed = 0.5f;

    void Start()
    {
        // Coleta todos os nós filhos
        nodes.Clear();
        if (nodesParent == null) nodesParent = transform;

        foreach (Transform child in nodesParent)
        {
            Node node = child.GetComponent<Node>();
            if (node != null)
                nodes.Add(node);
        }
    }

    // ============================================================
    // MÉTODO PARA ENCONTRAR O MENOR CAMINHO ENTRE DOIS NÓS
    // ============================================================
    public List<Node> GetPath(Node origem, Node destino)
    {
        if (origem == null || destino == null)
            return new List<Node>();

        // Dicionário para rastrear de onde viemos
        Dictionary<Node, Node> veioDe = new Dictionary<Node, Node>();
        Queue<Node> fila = new Queue<Node>();
        HashSet<Node> visitados = new HashSet<Node>();

        fila.Enqueue(origem);
        visitados.Add(origem);

        bool encontrou = false;

        while (fila.Count > 0)
        {
            Node atual = fila.Dequeue();

            if (atual == destino)
            {
                encontrou = true;
                break;
            }

            foreach (var viz in atual.Neighbors)
            {
                if (viz == null || visitados.Contains(viz))
                    continue;

                visitados.Add(viz);
                veioDe[viz] = atual;
                fila.Enqueue(viz);
            }
        }

        // Se não achou caminho, retorna vazio
        if (!encontrou)
            return new List<Node>();

        // Reconstrói o caminho do destino até a origem
        List<Node> caminho = new List<Node>();
        Node n = destino;
        while (n != origem)
        {
            caminho.Insert(0, n);
            n = veioDe[n];
        }
        caminho.Insert(0, origem);

        return caminho;
    }



    // ============================================================
    // DESENHAR NO MODO EDITOR (Gizmos)
    // ============================================================
    private void OnDrawGizmos()
    {
        if (nodesParent == null || !_debug) return;
        Gizmos.color = lineColor;

        foreach (Transform child in nodesParent)
        {
            Node node = child.GetComponent<Node>();
            if (node == null) continue;

            foreach (var viz in node.Neighbors)
            {
                if (viz != null)
                    Gizmos.DrawLine(node.transform.position, viz.transform.position);
            }
        }


        Gizmos.color = Color.red;
        for (int i = 1; i <_currentPath.Count; i++)
        {
            Gizmos.DrawLine(_currentPath[i - 1].transform.position, _currentPath[i].transform.position);
        }
    }
}
