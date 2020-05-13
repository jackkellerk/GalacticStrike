using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readDeckCollection : MonoBehaviour
{
    // In Start(), read a text file from the server that contains this. Using this as a testing example rn. Format is self-explanatory.
    string deckCollection = "3/Ghost Tiger/Common;1/Ghost Tiger/Ultra;1/Ghost Tiger/Secret;1/Starburst/Common;1/Starburst/Ultra;1/Starburst/Secret;1/Sleeping Beast/Common;1/Sleeping Beast/Ultra;1/Sleeping Beast/Secret;3/Mind Upload/Secret";
    string cardsInDeck = ""; // TODO

    // These are the prefabs for the cards. If adding a card, make a new variable here copying the format.
    // Then assign this variable in the Awake() function below.
    public static GameObject starburstCommon;
    public static GameObject starburstUltra;
    public static GameObject starburstSecret;
    public static GameObject ghostTigerCommon;
    public static GameObject ghostTigerUltra;
    public static GameObject ghostTigerSecret;
    public static GameObject sleepingBeastCommon;
    public static GameObject sleepingBeastUltra;
    public static GameObject sleepingBeastSecret;
    public static GameObject mindUploadCommon;
    public static GameObject mindUploadUltra;
    public static GameObject mindUploadSecret;

    void Awake()
    {
        // If you're making a new card, add a line here to the name of the prefab in the 'Assets/Resources/Prefab' directory.
        // Then in the determineCard() function, create a new if block.
        starburstCommon = Resources.Load("Prefabs/Starburst Common") as GameObject;
        ghostTigerCommon = Resources.Load("Prefabs/Ghost Tiger Common") as GameObject;
        sleepingBeastCommon = Resources.Load("Prefabs/Sleeping Beast Common") as GameObject;
        starburstUltra = Resources.Load("Prefabs/Starburst Ultra Rare") as GameObject;
        ghostTigerUltra = Resources.Load("Prefabs/Ghost Tiger Ultra Rare") as GameObject;
        sleepingBeastUltra = Resources.Load("Prefabs/Sleeping Beast Ultra Rare") as GameObject;
        starburstSecret = Resources.Load("Prefabs/Starburst Secret Rare") as GameObject;
        ghostTigerSecret = Resources.Load("Prefabs/Ghost Tiger Secret Rare") as GameObject;
        sleepingBeastSecret = Resources.Load("Prefabs/Sleeping Beast Secret Rare") as GameObject;
        // mindUploadCommon = TODO;
        // mindUploadUltra = TODO;
        mindUploadSecret = Resources.Load("Prefabs/Mind Upload Secret Rare") as GameObject; // TODO
    }

    // If you're making a new card, copy one of the if statements below, and customize it. That's it, you're done!
    static GameObject determineCard(Card card)
    {
        if(card.name == "Starburst")
        {
            if(card.rarity == "Common")
            {
                return starburstCommon;
            }
            else if(card.rarity == "Ultra")
            {
                return starburstUltra;
            }
            else if(card.rarity == "Secret")
            {
                return starburstSecret;
            }
        }
        else if (card.name == "Ghost Tiger")
        {
            if (card.rarity == "Common")
            {
                return ghostTigerCommon;
            }
            else if (card.rarity == "Ultra")
            {
                return ghostTigerUltra;
            }
            else if (card.rarity == "Secret")
            {
                return ghostTigerSecret;
            }
        }
        else if (card.name  == "Sleeping Beast")
        {
            if (card.rarity == "Common")
            {
                return sleepingBeastCommon;
            }
            else if (card.rarity == "Ultra")
            {
                return sleepingBeastUltra;
            }
            else if (card.rarity == "Secret")
            {
                return sleepingBeastSecret;
            }
        }
        else if (card.name == "Mind Upload")
        {
            if (card.rarity == "Common")
            {
                return mindUploadCommon;
            }
            else if (card.rarity == "Ultra")
            {
                return mindUploadUltra;
            }
            else if (card.rarity == "Secret")
            {
                return mindUploadSecret;
            }
        }

        // Could not find the card!
        return null;
    }

    /// <summary>
    /// Below this is the infrastructure for the game. You don't need to edit it if you're adding a new card.
    /// </summary>

    class Card
    {
        public string name = null;
        public string rarity = null;
        public int amount = 0;

        public Card(string amount, string name, string rarity)
        {
            this.name = name;
            this.rarity = rarity;
            this.amount = int.Parse(amount) > 3 ? 3 : int.Parse(amount);
        }
    }

    class LocationAndCard
    {
        public Card card = null;
        public Vector3 spawnLocation = new Vector3(0, 0, 0);
        public int amount = 0;

        public LocationAndCard(Card card, Vector3 spawnLocation)
        {
            this.card = card;
            this.spawnLocation = spawnLocation;
        }

        public void spawnCard(int amount)
        {
            if(amount <= 0)
            {
                return;
            }
            else if (amount > 1)
            {
                GameObject prefab = determineCard(card);
                for(int i = 0; i < amount; i++)
                {
                    Vector3 newLoc = spawnLocation + new Vector3(-1 * this.amount, -0.1f * this.amount, 1 * this.amount);
                    Instantiate(prefab, newLoc, Quaternion.Euler(new Vector3(-90, 180, 0)));
                    this.amount += 1;
                }
            }
            else
            {
                GameObject prefab = determineCard(card);
                Vector3 newLoc = spawnLocation + new Vector3(1 * this.amount, -0.1f * this.amount, 1 * this.amount);
                Instantiate(prefab, newLoc, Quaternion.Euler(new Vector3(-90, 180, 0)));
                this.amount += amount;
            }
        }
    }

    List<Card> listOfCards = new List<Card>();
    List<LocationAndCard> listOfLc = new List<LocationAndCard>();

    void Start()
    {
        // TODO: make a call to the server

        string[] stringListOfCards = deckCollection.Split(';');
        foreach(string eachCard in stringListOfCards)
        {
            string[] info = eachCard.Split('/');
            Card card = new Card(info[0], info[1], info[2]);
            listOfCards.Add(card);
        }

        for(int i = 0; i < 9; i++)
        {
            Card card = listOfCards[i];
            Vector3 spawnLocation = new Vector3(-50.90f + (23.5f * (i % 3)), 11.2f, 30f - (30 * (i / 3)));
            LocationAndCard lc = new LocationAndCard(card, spawnLocation);
            lc.spawnCard(card.amount);
            listOfLc.Add(lc);
        }
    }

    int page = 1;
    bool firstPage = true;

    public void calculateNextPage()
    {
        if(listOfCards.Count > (page * 9))
        {
            GameObject[] cards = GameObject.FindGameObjectsWithTag("card");
            foreach(GameObject c in cards)
            {
                Destroy(c);
            }

            if(listOfLc.Count < (page * 9) + 9)
            {
                for (int i = 0; i < 9; i++)
                {
                    if(listOfLc.Count > (page * 9) + i)
                    {
                        LocationAndCard lc = listOfLc[(page * 9) + i];
                        int amount = lc.amount;
                        lc.amount = 0;
                        lc.spawnCard(amount);
                    }
                    else if (listOfCards.Count > ((page * 9) + i))
                    {
                        Card card = listOfCards[(page * 9) + i];
                        Vector3 spawnLocation = new Vector3(-50.90f + (23.5f * (i % 3)), 11.2f, 30f - (30 * (i / 3)));
                        LocationAndCard lc = new LocationAndCard(card, spawnLocation);
                        lc.spawnCard(card.amount);
                        listOfLc.Add(lc);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    LocationAndCard lc = listOfLc[(page * 9) + i];
                    int amount = lc.amount;
                    lc.amount = 0;
                    lc.spawnCard(amount);
                }
            }

            firstPage = false;
            page++;
        }
    }

    public void calculatePreviousPage()
    {
        if((listOfLc.Count - ((page - 1) * 9) > 0) && (listOfLc.Count - ((page - 1) * 9) <= 9) && !firstPage)
        {
            GameObject[] cards = GameObject.FindGameObjectsWithTag("card");
            foreach (GameObject c in cards)
            {
                Destroy(c);
            }

            for(int i = 0; i < 9; i++)
            {
                LocationAndCard lc = listOfLc[listOfLc.Count - ((page - 1) * 9) - 1 + i];
                int amount = lc.amount;
                lc.amount = 0;
                lc.spawnCard(amount);
            }

            page--;
        }
    }

    public void pickedUpCard(string cardName, string rarity)
    {
        foreach (LocationAndCard lc in listOfLc)
        {
            if (lc.card.name == cardName && lc.card.rarity == rarity)
            {
                lc.amount--;
                Debug.Log(lc.amount);
                break;
            }
        }
    }

    public void respawnCard(string cardName, string rarity)
    {
        foreach(LocationAndCard lc in listOfLc)
        {
            if(lc.card.name == cardName && lc.card.rarity == rarity)
            {
                int amount = lc.amount;
                lc.amount = 0;
                lc.spawnCard(1);
                lc.amount = amount;
                break;
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown("right"))
        {
            calculateNextPage();
        }
        else if(Input.GetKeyDown("left"))
        {
            calculatePreviousPage();
        }
    }
}
