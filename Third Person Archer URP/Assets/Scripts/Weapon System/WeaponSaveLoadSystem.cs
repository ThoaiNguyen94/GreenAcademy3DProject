using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[RequireComponent(typeof(WeaponSystem))]
[RequireComponent(typeof(GetWeaponName))]
public class WeaponSaveLoadSystem : MonoBehaviour
{
    private WeaponSystem weaponSystem;
    private GetWeaponName getWeaponName;
    List<IDictionary> weapons = new List<IDictionary>();

    // Start is called before the first frame update
    void Start()
    {
        weaponSystem = GetComponent<WeaponSystem>();
        getWeaponName = GetComponent<GetWeaponName>();
        ReadData();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReadData()
    {
        using(StreamReader reader = new StreamReader("Assets/Data/WeaponData.json"))
        {
            string weapon = reader.ReadToEnd();
            weapons = JsonConvert.DeserializeObject<List<IDictionary>>(weapon);
        }
    }

    private void LoadData()
    {
        foreach (var weapon in weapons)
        {
            if ((string)weapon["weaponName"] == getWeaponName.WeaponName())
            {
                weaponSystem.weaponType = weapon["weaponType"].ToString();
                weaponSystem.weaponName = weapon["weaponName"].ToString();
                weaponSystem.weaponDamage = int.Parse(weapon["weaponDamage"].ToString());
                weaponSystem.weaponRange = float.Parse(weapon["weaponRange"].ToString());
                weaponSystem.weaponFireRate = float.Parse(weapon["weaponFireRate"].ToString());
            }
        }
    }
}
