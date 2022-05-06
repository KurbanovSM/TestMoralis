using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoralisWeb3ApiSdk;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

#if UNITY_WEBGL
using Moralis.WebGL;
using Moralis.WebGL.Platform.Objects;
using Moralis.WebGL.Web3Api.Models;
using Cysharp.Threading.Tasks;
using Assets.MoralisWeb3ApiSdk.Example.Scripts;
using Moralis.WebGL.Platform.Queries;

#else
using Moralis.Platform.Objects;
using Moralis.Platform.Queries;
using Moralis.Web3Api.Models;
#endif
public class SaveAndDataSystem : MonoBehaviour //скрипты для работы с бэком
{
    public class PlayersData : MoralisObject
    {
        public string Contract { get; set; }
        public string UserName { get; set; }
        public int Crystals { get; set; }
        public string CharsItemsData { get; set; }
        public string HikeData { get; set; }
    }

    public async void SaveObjectToDB()  //Создаем дату первый раз
    {

        MoralisUser user = await MoralisInterface.GetUserAsync();
        string addr = user.authData["moralisEth"]["id"].ToString();
#if UNITY_WEBGL
        MoralisQuery<SaveAndDataSystem.PlayersData> q = await MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>();
        MoralisQuery<SaveAndDataSystem.PlayersData> playerDataCheck = q.WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerDataCheck.FindAsync();
#else
        MoralisQuery<SaveAndDataSystem.PlayersData> playerDataCheck = MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>(). WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerDataCheck.FindAsync();
#endif
        //  MoralisQuery<PlayersData> playerDataCheck = MoralisInterface.GetClient().Query<PlayersData>().WhereEqualTo("Contract",addr);
        //  IEnumerable<PlayersData> result = await playerDataCheck.FindAsync();
        if (result.Count() <= 0)
        {
            //MoralisUser user = await MoralisInterface.GetUserAsync();
            //string addr = user.authData["moralisEth"]["id"].ToString();

            PlayersData playerData = MoralisInterface.GetClient().Create<PlayersData>();
            playerData.ACL = new MoralisAcl(await MoralisInterface.GetUserAsync());
            playerData.Contract = addr;

            playerData.CharsItemsData = "";

            await playerData.SaveAsync();
        }
    }

    public async void RetrieveObjectFromDB() //Получаем всю дату по адресу контракта
    {
        MoralisUser user = await MoralisInterface.GetUserAsync();
        string addr = user.authData["moralisEth"]["id"].ToString();

#if UNITY_WEBGL
        MoralisQuery<SaveAndDataSystem.PlayersData> q = await MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>();
        MoralisQuery<SaveAndDataSystem.PlayersData> playerData = q.WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerData.FindAsync();
#else
        MoralisQuery<SaveAndDataSystem.PlayersData> playerData = MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>(). WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerData.FindAsync();
#endif
        //MoralisQuery<PlayersData> playerData = MoralisInterface.GetClient().Query<PlayersData>().WhereEqualTo("Contract",addr);
        //IEnumerable<PlayersData> result = await playerData.FindAsync(); 
        foreach (PlayersData c in result)
        {
            Debug.Log("123" + c.CharsItemsData); // конкретно тут получаю жсон с сейвами
        }

    }

    public async void UpdateItemsDataFromDB(string charsData) //Сохраняем новые сейвы предметов
    {
        MoralisUser user = await MoralisInterface.GetUserAsync();
        string addr = user.authData["moralisEth"]["id"].ToString();

#if UNITY_WEBGL
        MoralisQuery<SaveAndDataSystem.PlayersData> q = await MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>();
        MoralisQuery<SaveAndDataSystem.PlayersData> playerData = q.WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerData.FindAsync();
#else
        MoralisQuery<SaveAndDataSystem.PlayersData> playerData = MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>(). WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerData.FindAsync();
#endif
        //MoralisQuery<PlayersData> playerData = MoralisInterface.GetClient().Query<PlayersData>().WhereEqualTo("Contract",addr);
        //IEnumerable<PlayersData> result = await playerData.FindAsync(); 
        foreach (PlayersData c in result)
        {
            MoralisInterface.GetClient().DeleteAsync(c);//удаляю старую запись

            PlayersData newPlayerData = MoralisInterface.GetClient().Create<PlayersData>();
            newPlayerData.ACL = new MoralisAcl(await MoralisInterface.GetUserAsync());
            newPlayerData.Contract = c.Contract;
            newPlayerData.UserName = c.UserName;
            newPlayerData.Crystals = c.Crystals;
            newPlayerData.CharsItemsData = charsData; //тут сохраняется новый жсон
            newPlayerData.HikeData = c.HikeData;
            await newPlayerData.SaveAsync();
        }

    }

    public async void UpdateCrystalsFromDB(int crystalsChange) //Добовляем новые кристаллы
    {
        MoralisUser user = await MoralisInterface.GetUserAsync();
        string addr = user.authData["moralisEth"]["id"].ToString();

#if UNITY_WEBGL
        MoralisQuery<SaveAndDataSystem.PlayersData> q = await MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>();
        MoralisQuery<SaveAndDataSystem.PlayersData> playerData = q.WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerData.FindAsync();
#else
        MoralisQuery<SaveAndDataSystem.PlayersData> playerData = MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>(). WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerData.FindAsync();
#endif
        //MoralisQuery<PlayersData> playerData = MoralisInterface.GetClient().Query<PlayersData>().WhereEqualTo("Contract",addr);
        //IEnumerable<PlayersData> result = await playerData.FindAsync(); 
        foreach (PlayersData c in result)
        {
            MoralisInterface.GetClient().DeleteAsync(c);//удаляю старую запись

            PlayersData newPlayerData = MoralisInterface.GetClient().Create<PlayersData>();
#if UNITY_WEBGL
            newPlayerData.ACL = new MoralisAcl(await MoralisInterface.GetUserAsync());
#else
 newPlayerData.ACL = new MoralisAcl(MoralisInterface.GetUser());
#endif
            newPlayerData.Contract = c.Contract;
            newPlayerData.UserName = c.UserName;
            newPlayerData.Crystals = c.Crystals + crystalsChange; //тут добавляется разница в сохраненных кристаллах
            newPlayerData.CharsItemsData = c.CharsItemsData;
            newPlayerData.HikeData = c.HikeData;
            await newPlayerData.SaveAsync();
        }

    }

    public async void UpdateHikeFromDB(string hikeData) //Добовляем новую хайк дату
    {
        MoralisUser user = await MoralisInterface.GetUserAsync();
        string addr = user.authData["moralisEth"]["id"].ToString();

#if UNITY_WEBGL
        MoralisQuery<SaveAndDataSystem.PlayersData> q = await MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>();
        MoralisQuery<SaveAndDataSystem.PlayersData> playerData = q.WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerData.FindAsync();
#else
        MoralisQuery<SaveAndDataSystem.PlayersData> playerData = MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>(). WhereEqualTo("Contract", addr);
        IEnumerable<SaveAndDataSystem.PlayersData> result = await playerData.FindAsync();
#endif
        //MoralisQuery<PlayersData> playerData = MoralisInterface.GetClient().Query<PlayersData>().WhereEqualTo("Contract",addr);
        //IEnumerable<PlayersData> result = await playerData.FindAsync(); 
        foreach (PlayersData c in result)
        {
            MoralisInterface.GetClient().DeleteAsync(c);//удаляю старую запись

            PlayersData newPlayerData = MoralisInterface.GetClient().Create<PlayersData>();
#if UNITY_WEBGL
            newPlayerData.ACL = new MoralisAcl(await MoralisInterface.GetUserAsync());
#else
 newPlayerData.ACL = new MoralisAcl(MoralisInterface.GetUser());
#endif
            newPlayerData.Contract = c.Contract;
            newPlayerData.UserName = c.UserName;
            newPlayerData.Crystals = c.Crystals;
            newPlayerData.CharsItemsData = c.CharsItemsData;
            newPlayerData.HikeData = hikeData; // тут добавляем новую хайк дату
            await newPlayerData.SaveAsync();
        }
    }


    [Serializable]
    public class CharsData // структура класса хранящий шмоткИ
    {
        public List<CharInventory> charInventary { get; set; }
    }

    [Serializable]
    public class CharInventory // структура класса хранящий шмоткУ
    {
        public string charId;
        public List<string> items { get; set; }
    }

    string InventorySave() //сохраняем вещи в жсон ТУТ ДОБАВЬ НУЖНЫЕ ПЕРЕГРУЗКИ
    {
        CharInventory newChar = new CharInventory();
        newChar.charId = "1";  //ади перса по очереди
        newChar.items = new List<string>(); // список его предметов
        newChar.items.Add("Item1"); //трока для примера
        newChar.items.Add("Item2"); //трока для примера
        CharsData newChars = new CharsData();
        newChars.charInventary = new List<CharInventory>();
        newChars.charInventary.Add(newChar); //трока для примера
        newChars.charInventary.Add(newChar); //трока для примера
        var newSaves = JsonConvert.SerializeObject(newChars);

        return newSaves;
    }


    public class SettingsData : MoralisObject  //класс-схема для работы с данными настроек
    {
        public int ExchangeRates { get; set; }
        public int Id { get; set; }
        public int BonusRates { get; set; }
        public int FineRates { get; set; }
    }

    public async void RetrieveSettingsFromDB() //Получаем лобальные настройки с сервера
    {
#if UNITY_WEBGL
        MoralisQuery<SettingsData> q = await MoralisInterface.GetClient().Query<SettingsData>();
        MoralisQuery<SettingsData> playerData = q.WhereEqualTo("Id", 1);
        IEnumerable<SettingsData> result = await playerData.FindAsync();
#else
           MoralisQuery<SettingsData> playerData = MoralisInterface.GetClient().Query<SettingsData>().WhereEqualTo("Id",1);
        IEnumerable<SettingsData> result = await playerData.FindAsync(); 
#endif
        foreach (SettingsData c in result)
        {
            Debug.Log("123" + c.ExchangeRates); // получаю рейтинг обмена монеты
        }

    }

}
