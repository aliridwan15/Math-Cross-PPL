using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DropSlotPuzzle : MonoBehaviour, IDropHandler
{
    [Tooltip("DAFTAR SEMUA baris puzzle yang memiliki slot ini")]
    public List<EquationRowPuzzle> parentRows; // Disesuaikan

    private DraggableCardPuzzle occupiedCard = null; 

    public void OnDrop(PointerEventData eventData)
    {
        if (occupiedCard == null) 
        {
            DraggableCardPuzzle droppedCard = eventData.pointerDrag.GetComponent<DraggableCardPuzzle>();

            if (droppedCard != null)
            {
                occupiedCard = droppedCard;
                droppedCard.currentSlot = this;
                droppedCard.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;

                // Beri tahu SEMUA baris yang terhubung untuk cek jawaban
                if (parentRows != null && parentRows.Count > 0)
                {
                    foreach (EquationRowPuzzle row in parentRows)
                    {
                        // Memanggil CheckAnswer() di setiap baris
                        row.CheckAnswer();
                    }
                }
            }
        }
    }

    // Mengosongkan slot
    public void ClearSlot()
    {
        occupiedCard = null;
    }

    // Mengambil nilai kartu yang ada di slot
    public int GetCardValue()
    {
        return (occupiedCard != null) ? occupiedCard.cardValue : -1;
    }

    // Mengecek apakah slot terisi
    public bool IsOccupied()
    {
        return occupiedCard != null;
    }

    // Mendapatkan kartu yang sedang menempati slot
    public DraggableCardPuzzle GetOccupiedCard()
    {
        return occupiedCard;
    }
    
    // Menetapkan kartu yang menempati slot
    public void SetOccupiedCard(DraggableCardPuzzle card)
    {
        occupiedCard = card;
    }
}
