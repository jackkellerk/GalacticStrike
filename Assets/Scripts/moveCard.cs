using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCard : MonoBehaviour
{
    public bool placeInDeck = false;
    Transform card = null;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 100.0f))
            {
                if(hit.transform.tag == "card")
                {
                    card = hit.transform;
                }
            }
        }
        else if(card != null && card.localScale.x < 6)
        {
            card.localScale += new Vector3(0.4f, 0.4f, 0.4f);
        }
        else if(Input.GetMouseButton(0) && card != null)
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
            newPos.y = 30;
            card.position = newPos;
        }
        else if(card != null)
        {
            if(placeInDeck)
            {
                // TODO: Place the card in the deck (spawn it there)

                if (card.name.Contains("Common"))
                {
                    this.GetComponent<readDeckCollection>().pickedUpCard(card.name.Substring(0, card.name.Length - 14), "Common");
                }
                else if (card.name.Contains("Ultra Rare"))
                {
                    this.GetComponent<readDeckCollection>().pickedUpCard(card.name.Substring(0, card.name.Length - 18), "Ultra");
                }
                else if (card.name.Contains("Secret Rare"))
                {
                    this.GetComponent<readDeckCollection>().pickedUpCard(card.name.Substring(0, card.name.Length - 19), "Secret");
                }
            }
            else
            {
                if(card.name.Contains("Common"))
                {
                    this.GetComponent<readDeckCollection>().respawnCard(card.name.Substring(0, card.name.Length - 14), "Common");
                }
                else if(card.name.Contains("Ultra Rare"))
                {
                    this.GetComponent<readDeckCollection>().respawnCard(card.name.Substring(0, card.name.Length - 18), "Ultra");
                }
                else if(card.name.Contains("Secret Rare"))
                {
                    this.GetComponent<readDeckCollection>().respawnCard(card.name.Substring(0, card.name.Length - 19), "Secret");
                }
            }

            Destroy(card.gameObject);
            card = null;
        }
    }

    /// <summary>
    /// These functions are here because of the invisible button in the Canvas GameObject. When the cursor hovers over it,
    /// enterPlaceInDeck() is called via the Event Trigger Component. When the cursor is not hovering over it, leavePlaceInDeck()
    /// is called. The idea is that if you release the card while placeInDeck is true, then it adds the card to the deck otherwise
    /// it returns it to the deck collection book.
    /// </summary>
    public void enterPlaceInDeck()
    {
        placeInDeck = true;
    }

    public void leavePlaceInDeck()
    {
        placeInDeck = false;
    }
}
