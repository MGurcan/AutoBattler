using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	#region Singlton:Shop

	public static Shop Instance;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (gameObject);
	}

	#endregion

	[System.Serializable] public class ShopItem
	{
		public GameObject prefab;
		public int Price;
		public int shopItemId;
        public string name;
        public string ToString => "Item Price: " + Price + " Has " + name + " of them.";
    }

	public List<ShopItem> ShopItemsList;

    public GameManager gameManager;

    public Button[] buyButtons;

    public Character PurchaseCharacter(int itemIndex)
    {
        Debug.Log(itemIndex);
        Character character = new Character(ShopItemsList[itemIndex].shopItemId, ShopItemsList[itemIndex].name);
        gameManager.characterList.Add(character);

        gameManager.InstantiateCharacter(character);
        return character;
    }

    private void Start() {
        int len = buyButtons.Length;
		for (int i = 0; i < len; i++) {
            int itemIndex = i;
			buyButtons[i].onClick.AddListener(() => PurchaseCharacter(itemIndex));
		}
    }
}
