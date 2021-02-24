using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeUI : MonoBehaviour
{
    public BattleManager battleManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;
        if (battleManager.BattleState == BattleState.IN_BATTLE)
        {
			if (battleManager.getBattleQueue() != null)
	        {
	            while (count < 5)
	            {
	                Queue <GameObject> battleQueue = new Queue<GameObject>(battleManager.getBattleQueue());
	
	                for (int i = 0; i < battleQueue.Count - 1; i++)
	                {
	                    battleQueue.Enqueue(battleQueue.Dequeue());
	                }
	
	                while (battleQueue.Count != 0 && count < 5)
	                {
	                    Image initImage = transform.GetChild(count).GetChild(0).GetComponent<Image>();
	                    GameObject combatant = battleQueue.Dequeue();
                        if (combatant != null)
                        {
                            initImage.sprite = combatant.GetComponent<SpriteRenderer>().sprite;
                            initImage.SetNativeSize();
                            count++;
                        }
	                }
	
	            }
	        }
		}

    }
}
