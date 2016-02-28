using UnityEngine;
using System.Collections;

public class PlayerCollisionsController : PlayerBase
{
    #region Events
    public delegate void SheepDestroyAction();
    public static event SheepDestroyAction OnSheepDestroyAction;
    #endregion

    public GameObject explosionPrefab;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Sheep"))
        {
            if (this.PlayerController != null && this.PlayerController.isDashing)
            {
                GameObject.Instantiate(explosionPrefab, collision.transform.position, Quaternion.Euler(new Vector3(280, 0, 0)));

                if (OnSheepDestroyAction != null)
                    OnSheepDestroyAction();

                Destroy(collision.gameObject);
            }
        }
    }
}
