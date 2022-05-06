using Lowscope.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Lowscope.Saving.Examples
{

    public class ExampleEventlistening : MonoBehaviour
    {
        [SerializeField] private Button buttonSyncSave;
        [SerializeField] private Button buttonSyncLoad;
        [SerializeField] private Button buttonWriteToDisk;
        [SerializeField] private Button buttonLoadFromDisk;

        [SerializeField] private Text eventText;

        private List<string> events = new List<string>(10);

        private void Awake()
        {
            SaveMaster.OnSyncSaveBegin += (slot) => { events.Insert(0, string.Format("Sync saving slot: {0}", slot)); UpdateEventList(); };
            SaveMaster.OnSyncSaveDone += (slot) => { events.Insert(0, string.Format("Sync saving slot (done): {0}", slot)); UpdateEventList(); };

            SaveMaster.OnSyncLoadBegin += (slot) => { events.Insert(0, string.Format("Sync loading slot: {0}", slot)); UpdateEventList(); };
            SaveMaster.OnSyncLoadDone += (slot) => { events.Insert(0, string.Format("Sync loading slot (done): {0}", slot)); UpdateEventList(); };

            SaveMaster.OnLoadingFromDiskBegin += (slot) => { events.Insert(0, string.Format("Loading from disk for slot: {0}", slot)); UpdateEventList(); };
            SaveMaster.OnLoadingFromDiskDone += (slot) => { events.Insert(0, string.Format("Loading from disk for slot: {0} (done)", slot)); UpdateEventList(); };

            SaveMaster.OnWritingToDiskBegin += (slot) => { events.Insert(0, string.Format("Writing to disk for slot: {0}", slot)); UpdateEventList(); };
            SaveMaster.OnWritingToDiskDone += (slot) => { events.Insert(0, string.Format("Writing to disk for slot (done): {0}", slot)); UpdateEventList(); };

            SaveMaster.OnSlotChangeBegin += (newslot, oldslot) => { events.Insert(0, string.Format("Changing slot from {0} to {1}", oldslot, newslot)); UpdateEventList(); };
            SaveMaster.OnSlotChangeDone += (newslot, oldslot) => { events.Insert(0, string.Format("Changing slot from {0} to {1} (done)", oldslot, newslot)); UpdateEventList(); };

            buttonSyncSave.onClick.AddListener(OnSyncSaveButton);
            buttonSyncLoad.onClick.AddListener(OnSyncLoadButton);
            buttonWriteToDisk.onClick.AddListener(OnWriteToDiskButton);
            buttonLoadFromDisk.onClick.AddListener(OnLoadFromDiskButton);
        }

        private void OnLoadFromDiskButton()
        {
            SaveMaster.SetSlot(0, false);

            events.Clear();
        }

        private void OnWriteToDiskButton()
        {
            SaveMaster.WriteActiveSaveToDisk(false);

            events.Clear();
        }

        private void OnSyncLoadButton()
        {
            SaveMaster.SyncLoad();

            events.Clear();
        }

        private void OnSyncSaveButton()
        {
            SaveMaster.SyncSave();

            events.Clear();
        }

        private void UpdateEventList()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = events.Count - 1; i >= 0; i--)
            {
                sb.AppendLine(events[i]);
            }

            eventText.text = sb.ToString();
        }
    }
}