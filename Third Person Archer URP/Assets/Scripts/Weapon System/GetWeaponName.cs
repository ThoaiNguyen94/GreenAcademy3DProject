using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeaponName : MonoBehaviour
{
    public string weaponName;

    // Start is called before the first frame update
    void Start()
    {
        WeaponName();
    }

    public string WeaponName()
    {
        GameObject weaponObject = GameObject.FindGameObjectWithTag("Weapon");
        if (weaponObject != null)
        {
            weaponName = weaponObject.name;
        }
        else
        {
            Debug.Log("Can't find the weapon name");
        }
        return weaponName;
    }
}
