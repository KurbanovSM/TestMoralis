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

public class DataBaseController : MonoBehaviour
{

    public TMP_Text TMP_Text;

    private bool isInitialized = false;

    async void Update()
    {
        if (!isInitialized && MoralisInterface.IsLoggedIn()) // получаем токены
        {
            isInitialized = true;
            SaveObjectToDB();
        }
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
        //MoralisQuery<SaveAndDataSystem.PlayersData> playerDataCheck = MoralisInterface.GetClient().Query<SaveAndDataSystem.PlayersData>().WhereEqualTo("Contract",addr);
        //IEnumerable<SaveAndDataSystem.PlayersData> result = await playerDataCheck.FindAsync();
        if (result.Count() <= 0)
        {
            SaveAndDataSystem.PlayersData playerData = MoralisInterface.GetClient().Create<SaveAndDataSystem.PlayersData>();
            playerData.ACL = new MoralisAcl(user);
            playerData.Contract = addr;
            playerData.Crystals = 0;
            await playerData.SaveAsync();

            TMP_Text.text = "Наверное все прошло удачно";

            Debug.Log("Наверное все прошло удачно");
        }
    }
}
