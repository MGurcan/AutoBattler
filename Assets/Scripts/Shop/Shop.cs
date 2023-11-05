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
    public List<ShopItem> CurrentShopItems;

    public GameManager gameManager;

    public Button[] buyButtons;

    public GameObject ShopPanel;
    public GameObject purchaseButtonPrefab;

    public Material hologramMaterial;


    public Character PurchaseCharacter(int itemIndex)
    {
        Character character = new Character(CurrentShopItems[itemIndex].shopItemId, CurrentShopItems[itemIndex].name);
        gameManager.characterList.Add(character);

        gameManager.InstantiateCharacter(character);

        Transform shopItemsTransform = ShopPanel.transform.Find("ShopItems");
        shopItemsTransform.GetChild(itemIndex).gameObject.SetActive(false);

        gameManager.LevelUpCharacter(character);
        return character;
    }

    private void Start() {
        CurrentShopItems = PrepareRandomShopItems(5);
        StartCoroutine(InstantiateShopItemsToPanel());
    }

    private IEnumerator InstantiateShopItemsToPanel(){
        int len = CurrentShopItems.Count;
        Transform shopItemsTransform = ShopPanel.transform.Find("ShopItems");

		for (int i = 0; i < len; i++) {
            yield return new WaitForSeconds(0.2f);

            GameObject purchaseButton = Instantiate(purchaseButtonPrefab, shopItemsTransform);
            Button button = purchaseButton.GetComponent<Button>();

            button.transform.Find("NameText").GetComponent<Text>().text = CurrentShopItems[i].name;
            button.transform.Find("PriceText").GetComponent<Text>().text = CurrentShopItems[i].price.ToString() + "$";


            Transform buttonGameObjectTransform = button.transform.Find("ButtonGameObject");
            GameObject buttonGameObject = Instantiate(CurrentShopItems[i].prefab, buttonGameObjectTransform);
            buttonGameObject.transform.localScale = new Vector3(80f, 80f, 80f);
            buttonGameObject.layer = LayerMask.NameToLayer("UI");
            Destroy(buttonGameObject.GetComponent<DragObject>());

            buttonGameObject.AddComponent<RotateAroundItself>();
            buttonGameObject.GetComponent<Renderer>().material = hologramMaterial;
            buttonGameObject.AddComponent<GlitchControl>();
            int itemIndex = i;
			button.onClick.AddListener(() => PurchaseCharacter(itemIndex));
		}
    }

    private void ClearShopItemsList(){
        Transform shopItemsTransform = ShopPanel.transform.Find("ShopItems");
        if(shopItemsTransform != null)
        {
            foreach (Transform child in shopItemsTransform){
                Destroy(child.gameObject);
            }
        }

    }

    private bool isShopOpen = false;

    public void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        ShopPanel.SetActive(isShopOpen);
    }

    private List<ShopItem> PrepareRandomShopItems(int count)
    {
        List<ShopItem> randomItems = new List<ShopItem>();

        for(int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, ShopItemsList.Count);
            randomItems.Add(ShopItemsList[randomIndex]);
        }
        return randomItems;
    }

    public void RefreshShopList()
    {
        ClearShopItemsList();
        CurrentShopItems = PrepareRandomShopItems(5);
        StartCoroutine(InstantiateShopItemsToPanel());
    }
}
