using UnityEngine;
using System.Collections;

public class Move_Camera : MonoBehaviour {

	// ***Script attaché à la caméra souhaité.***
	public float ScrSpeed=10f, ZoneMorte=0.95f, ZoomIn=10f, ZoomOut=100f;
	void Update ()
	{
		/*Zoom In/Out
       * Ceci n'est pas un "vrai" zoom, c'est uniquement une modification de la hauteur de la caméra.
       * ZoomIn et ZoomOut servent à définir les bornes du "zoom".
      */
		
		float Molette;
		Vector2 MPos=Vector3.zero;
		Vector3 ScrMove=transform.position;
		
		
		Molette = Input.GetAxis("Mouse ScrollWheel")*20;
		
		if(ScrMove.y-Molette>=ZoomIn && ScrMove.y-Molette<=ZoomOut)
		{
			ScrMove.y -= Molette;
		}
		if(ScrMove.y<ZoomIn) {ScrMove.y = ZoomIn;}
		if(ScrMove.y>ZoomOut) {ScrMove.y = ZoomOut;}
		
		/*  Déplacement Caméra
       * Récupère la position de la sourie dans l'écran et déplace la caméra sur l'axe.
       * Vous pouvez régler la largeur de la zone provoquant les déplacement en modifiant
       * la variable ZoneMorte.
       * Vous pouvez modifier la vitesse de translation en modifiant la valeur de ScrSpeed.
      */
		
		MPos.Set(Input.mousePosition.x/Screen.width,Input.mousePosition.y/Screen.height);
		
		
		if(MPos.x < 1-ZoneMorte)
		{
			ScrMove.x-=Time.deltaTime*ScrSpeed*ScrMove.y/10;
		}
		else if(MPos.x > ZoneMorte)
		{
			ScrMove.x +=Time.deltaTime*ScrSpeed*ScrMove.y/10;
		}
		
		
		if(MPos.y < 1-ZoneMorte)
		{
			ScrMove.z-=Time.deltaTime*ScrSpeed*ScrMove.y/10;
		}
		else if(MPos.y > ZoneMorte)
		{
			ScrMove.z +=Time.deltaTime*ScrSpeed*ScrMove.y/10;
		}
		
		transform.position =ScrMove;
	}
}
