using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
   private void  OnTriggerEnter(Collider other) 
   {
    if(other.CompareTag(Consts.WheatTypes.Gold_Wheat))
    {
       other.gameObject?.GetComponent<GoldWheatCollectible>().Collect();

    }

    if(other.CompareTag(Consts.WheatTypes.Holy_Wheat))
    {
       other.gameObject?.GetComponent<HolyWheatCollectible>().Collect();
        
    }

   if(other.CompareTag(Consts.WheatTypes.Rotten_Wheat))
    {
        other.gameObject?.GetComponent<RottenWheatCollectible>().Collect();
        
    }

   }
}
