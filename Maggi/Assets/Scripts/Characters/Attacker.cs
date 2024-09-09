using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private GameObject _attackCollider;

    /* Execute in Animation Clip */

    public void EnableWeapon()
    {
        _attackCollider.SetActive(true);
    }

    public void DisableWeapon()
    {
        _attackCollider?.SetActive(false);
    }
}
