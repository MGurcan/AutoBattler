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
		public int price;
		public int shopItemId;
        public string name;
        public string ToString => "Item Price: " + price + " Has " + name + " of them.";
    }

	public List<ShopItem> ShopItemsList;

    public GameManager gameManager;

    public Button[] buyButtons;

    public GameObject ShopPanel;
    public GameObject purchaseButtonPrefab;


    public Character PurchaseCharacter(int itemIndex)
    {
        Character character = new Character(ShopItemsList[itemIndex].shopItemId, ShopItemsList[itemIndex].name);
        gameManager.characterList.Add(character);

        gameManager.InstantiateCharacter(character);
        return character;
    }

    private void Start() {
        int len = ShopItemsList.Count;
        Transform shopItemsTransform = ShopPanel.transform.Find("ShopItems");

		for (int i = 0; i < len; i++) {
            GameObject purchaseButton = Instantiate(purchaseButtonPrefab, shopItemsTransform);
            Button button = purchaseButton.GetComponent<Button>();

            button.transform.Find("NameText").GetComponent<Text>().text = ShopItemsList[i].name;
            button.transform.Find("PriceText").GetComponent<Text>().text = ShopItemsList[i].price.ToString() + "$";


            Transform buttonGameObjectTransform = button.transform.Find("ButtonGameObject");
            GameObject buttonGameObject = Instantiate(ShopItemsList[i].prefab, buttonGameObjectTransform);
            buttonGameObject.transform.localScale = new Vector3(80f, 80f, 80f);
            buttonGameObject.layer = LayerMask.NameToLayer("UI");
            Destroy(buttonGameObject.GetComponent<DragObject>());

            int itemIndex = i;
			button.onClick.AddListener(() => PurchaseCharacter(itemIndex));
		}
    }
}
