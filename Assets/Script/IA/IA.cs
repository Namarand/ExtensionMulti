using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IA : MonoBehaviour
{   
	public float angle = 75f; // Angle d'ouverture, degré
	public float distance = 10f;
	public int precision = 20; // Nombre de rayons lancé dans l'angle ci dessus
	public Material material; // Mat appliqué au mesh de vue
	public bool debug = false; // Dessine les rayons dans la scene view
	public float freq = 0.05F; // Fréquence de calcul. > 0.05F ça devient moche
	public LayerMask mask; // Layers qui vont bloquer la vue
	
	Vector3[] directions; // va contenir les rayons, déterminés par precision, distance et angle
	Mesh sightMesh; // Le mesh dont les vertex seront modifiés selons les obstacles
	Transform m_Transform;
	
	int nbPoints;
	int nbTriangle;
	int nbFace;
	int nbIndice;
	int row;
	Vector3[] points;
	int[] indices;

	public float speed = 0.1f;
	public List<Vector3> destination = new List<Vector3>(){new Vector3(3,0.73f,10)};
	int i = 0;
	GameObject target = null;
	bool getTarget = false;
	NavMeshAgent agent;
	
	// Use this for initialization
	void Start ()
	{
		destination.Add (this.transform.position);
		agent = this.gameObject.GetComponent<NavMeshAgent> ();
		// Initialisation du cone
		GameObject sightObject = new GameObject( "ConeSight" );
		sightMesh = new Mesh();
		(sightObject.AddComponent( typeof( MeshFilter )) as MeshFilter).mesh = sightMesh;
		(sightObject.AddComponent( typeof( MeshRenderer )) as MeshRenderer).material = material;
		m_Transform = transform; // histoire de limiter les getcomponent dans la boucle
		
		// Préparation des rayons
		precision = precision > 1 ? precision : 2;
		directions = new Vector3[precision];
		float angle_start = -angle*0.5F;
		float angle_step = angle / (precision-1);
		for( int i = 0; i < precision; i++ )
		{
			Matrix4x4 mat = Matrix4x4.TRS( Vector3.zero, Quaternion.Euler(0,angle_start + i*angle_step,0), Vector3.one );
			directions[i] = mat * Vector3.forward;
		}
		
		// préparations des outils de manipulation du mesh
		nbPoints =  precision*2;
		nbTriangle = nbPoints - 2;
		nbFace = nbTriangle / 2;
		nbIndice =  nbTriangle * 3;
		row = nbFace+1;
		
		points = new Vector3[nbPoints];
		indices = new int[ nbIndice ];
		
		for( int i = 0; i < nbFace; i++ )
		{
			indices[i*6+0] = i+0;
			indices[i*6+1] = i+1;
			indices[i*6+2] = i+row;
			indices[i*6+3] = i+1;
			indices[i*6+4] = i+row+1;
			indices[i*6+5] = i+row;
		}
		
		sightMesh.vertices = new Vector3[nbPoints];
		sightMesh.uv = new Vector2[nbPoints];
		sightMesh.triangles = indices;      
		
		StartCoroutine( "Scan" );
	}
	
	// Appelle la modification du mesh tous les freq secondes
	IEnumerator Scan()
	{
		while (true) {
			UpdateSightMesh ();
			yield return new WaitForSeconds (freq);
		}

	}

	void Update(){
		this.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (speed, speed, speed);
		if (getTarget) {
			agent.SetDestination(target.transform.position);
		} else {
			if (Vector3.Distance(this.gameObject.transform.position, destination[i]) < 0.5f){
				i = (i+1)% destination.Count;
			}else{
				agent.SetDestination(destination[i]);
			}
		}
	}

	// Fonction qui modifie le mesh
	private void UpdateSightMesh()
	{    
		GameObject bestTarget = null;
		float bestRange = 0;
		// Lance les rayons pour placer les vertices le plus loin possible
		for (int i = 0; i < precision; i++) {
			Vector3 dir = m_Transform.TransformDirection (directions [i]); // repere objet
			RaycastHit hit;
			float dist = distance;
			if (Physics.Raycast (m_Transform.position, dir, out hit, distance, mask)) { // Si on touche, on rétrécit le rayon
				dist = hit.distance;
				if (hit.transform.CompareTag ("Runner")){
					if (hit.distance > bestRange)
						bestTarget = hit.transform.gameObject;
					hit.transform.gameObject.GetComponent<SeePlayer>().isSee = true;
				}	
			}
			if (debug)
				Debug.DrawRay (m_Transform.position, dir * dist);
		
			// Positionnement du vertex
			points [i] = m_Transform.position + dir * dist;
			points [i + precision] = m_Transform.position;

			if (bestTarget != null) {
				getTarget = true;
				target = bestTarget;
			} else {
				getTarget = false;
				target = null;
			}
		}
		// On réaffecte les vertices
		sightMesh.vertices = points;      
		sightMesh.RecalculateNormals (); // normales doivent être calculé pour tout changement
		// du tableau vertices, même si on travaille sur un plan
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Runner")){
			Destroy (other.gameObject);
		}
	}
	
}

