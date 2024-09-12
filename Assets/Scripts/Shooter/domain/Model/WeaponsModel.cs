using System;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Shooter.domain.Model
{
    [Serializable]
    public struct PlayerWeaponState
    {
        public long id;
        public string name;
        public string nameLocalizationKey;
        public int level;
        public WeaponSettings settings;
    }

    [Serializable]
    public struct WeaponSettings
    {
        public int rpm;
        public int damage;
        public int rounds;
        public long cost;

        public bool Equals(WeaponSettings other)
        {
            return rpm == other.rpm && damage == other.damage && rounds == other.rounds && cost == other.cost;
        }

        public override bool Equals(object obj)
        {
            return obj is WeaponSettings other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(rpm, damage, rounds, cost);
        }

        public static bool operator ==(WeaponSettings left, WeaponSettings right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WeaponSettings left, WeaponSettings right)
        {
            return !left.Equals(right);
        }
    }

    public struct WeaponFullData
    {
        public WeaponState Weapon;
        public bool IsLocked;
        public bool IsPurchasable;
        public bool IsUpgradable;
        public bool IsPrimary;
        public bool IsSecondary;
    }

    [Serializable]
    public struct WeaponsData
    {
        public WeaponState[] weapons;
        public long primaryId;
        public long secondaryId;
    }

    [Serializable]
    public struct WeaponState
    {
        public WeaponInfo info;
        public int currentLevel;

        public bool Purchased => currentLevel > 0;
        public bool Upgradable => Purchased && info.settingsLevels.Length > currentLevel;

        public WeaponSettings CurrentSettings =>
            !Purchased
                ? info.settingsLevels[0]
                : info.settingsLevels[currentLevel - 1];

        public long Cost =>
            Purchased
                ? Upgradable ? info.settingsLevels[currentLevel].cost : 0
                : PurchaseCost;

        private long PurchaseCost => info.settingsLevels[0].cost;

        public bool Equals(WeaponState other)
        {
            return info.Equals(other.info) && currentLevel == other.currentLevel;
        }

        public override bool Equals(object obj)
        {
            return obj is WeaponState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(info, currentLevel);
        }

        public static bool operator ==(WeaponState left, WeaponState right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WeaponState left, WeaponState right)
        {
            return !left.Equals(right);
        }
    }

    [Serializable]
    public struct WeaponInfo
    {
        public long id;
        public string name;
        public string nameLocalizationKey;
        public int availableFromLevel;
        public bool automatic;
        public bool premium;
        public WeaponSettings[] settingsLevels;

        public bool Equals(WeaponInfo other) =>
            id == other.id && 
            name == other.name && 
            nameLocalizationKey == other.nameLocalizationKey &&
            availableFromLevel == other.availableFromLevel &&
            automatic == other.automatic &&
            premium == other.premium &&
            Equals(settingsLevels, other.settingsLevels);

        public override bool Equals(object obj) => obj is WeaponInfo other && Equals(other);

        public override int GetHashCode()
        {
            return HashCode.Combine(id, name, nameLocalizationKey, availableFromLevel, automatic, settingsLevels);
        }

        public static bool operator ==(WeaponInfo left, WeaponInfo right) => left.Equals(right);

        public static bool operator !=(WeaponInfo left, WeaponInfo right) => !left.Equals(right);
    }
}