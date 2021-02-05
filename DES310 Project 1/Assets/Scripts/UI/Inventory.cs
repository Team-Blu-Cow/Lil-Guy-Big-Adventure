using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
        public static Inventory instance;
        private void Awake()
        {     
            if (instance != null)
            {
                Debug.LogWarning("Multiple invemtory instances");
                return;
            }
            instance = this;
        }
    #endregion

    public int invSpace = 12;
    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        if (items.Count < invSpace)
        {
            items.Add(item);
        }
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

}
