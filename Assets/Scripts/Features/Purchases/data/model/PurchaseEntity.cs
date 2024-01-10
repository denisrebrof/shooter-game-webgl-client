using System;
using Features.Currencies.data;
using UnityEngine;
using Utils.AutoId;
using Utils.StringSelector;

namespace Features.Purchases.data.model
{
    [Serializable]
    public class PurchaseEntity
    {
        [AutoId]
        public string id;
        public string ruName;
        public string ruDescription;
        public string enName;
        public string enDescription;
        public int currencyCost = 0;
        //Attribute creates cyclic dependency
        [StringSelector(typeof(SOCurrencyRepository))] 
        public string currencyId;
        public int rewardedVideoCount = 0;
        public Sprite image;
    }
}