// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cleverous.VaultSystem
{
    public class VaultItemNotFoundException : Exception { }
     
    public static class Vault
    {
        public static string VaultDatabasePath;
        public static string VaultDatabaseName;
        public static string VaultItemPath;

        /// <summary>
        /// All of the references to project assets. Not recommended to access directly. Use the Vault class methods or create your own extension methods, such as non-linear search types.
        /// </summary>
        public static Database Data;
        public static bool IsReady;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitData()
        {
            const string dbPath = "Assets/Cleverous/Vault/Resources/";
            const string dStoragePath = "Assets/Cleverous/Vault/Storage/";
            const string dbName = "Database";

            VaultDatabasePath = dbPath;
            VaultDatabaseName = dbName;
            VaultItemPath = dStoragePath;

            if (Data == null) Data = Resources.Load(VaultDatabaseName) as Database; // simply not loaded
            if (Data != null && Data.Items == null) Data.Items = new List<DataEntity>(); // db was empty
            IsReady = Data != null;
            // if (!IsReady) Debug.LogError("Could not load Database! Check the file and folder. <color=yellow>NOTE:</color> This always happens on the first Editor load (Null/Empty DB)");
        }

        /// <summary>
        /// Directly access <see cref="Database.Items"/> at an index. This is the most efficient way to access data.
        /// </summary>
        /// <param name="index">The item ID (index)</param>
        /// <returns>A reference to the <see cref="DataEntity"/>.</returns>
        public static DataEntity Get(int index)
        {
            return index < 0 
                ? null 
                : Data.Items[index];
        }
        /// <summary>
        /// Linear Search <see cref="Database.Items"/> for an item with a specific <see cref="DataEntity.Title"/> and return the item found.
        /// </summary>
        /// <param name="itemTitle">The item Title</param>
        /// <returns>A reference to the <see cref="DataEntity"/> found with matching <see cref="DataEntity.Title"/>.</returns>
        public static DataEntity Get(string itemTitle)
        {
            for (int i = 0; i < Data.Items.Count; i++)
            {
                if (Data.Items[i].Title == itemTitle) return Data.Items[i];
            }

            throw new VaultItemNotFoundException();
        }

        /// <summary>
        /// Linear Search <see cref="Database.Items"/> for an item with a specific <see cref="DataEntity.Title"/> and return that index.
        /// </summary>
        /// <param name="entityTitle">The item Title</param>
        /// <returns>The found item's index value in the <see cref="Vault"/>.</returns>
        public static int GetIndex(string entityTitle)
        {
            for (int i = 0; i < Data.Items.Count; i++)
            {
                if (Data.Items[i].Title == entityTitle) return i;
            }

            throw new VaultItemNotFoundException();
        }
        /// <summary>
        /// Linear Search <see cref="Database.Items"/> for a matching <see cref="DataEntity"/> and return it's index in the database.
        /// </summary>
        /// <param name="originalVaultEntity">The <see cref="DataEntity"/> who's index you want. You must provide the original entity in the DB, not an instance.</param>
        /// <returns>The found item's index value in the <see cref="Vault"/>. Returns -1 on failure.</returns>
        public static int GetIndex(DataEntity originalVaultEntity)
        {
            for (int i = 0; i < Data.Items.Count; i++)
            {
                if (Data.Items[i] == originalVaultEntity)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get all of the items with the given type in the database.
        /// </summary>
        /// <typeparam name="T">The type of Items you want.</typeparam>
        /// <returns>All of the items that are of the given type.</returns>
        public static List<T> GetAll<T>() where T : DataEntity
        {
            List<T> results = new List<T>();
            for (int i = 0; i < Data.Items.Count; i++)
            {
                if (Data.Items[i].GetType() == typeof(T)) results.Add((T)Data.Items[i]);
            }

            return results;
        }
        /// <summary>
        /// Get the indexes of all items in the database which are of the given type.
        /// </summary>
        /// <typeparam name="T">The type you want to find.</typeparam>
        /// <returns>The indexes of every item in the database of type T.</returns>
        public static List<int> GetAllIndexes<T>() where T : DataEntity
        {
            List<int> results = new List<int>();
            for (int i = 0; i < Data.Items.Count; i++)
            {
                if (Data.Items[i].GetType() == typeof(T)) results.Add(i);
            }

            return results;
        }
    }
}