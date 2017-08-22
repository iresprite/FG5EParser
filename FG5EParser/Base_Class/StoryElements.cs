﻿using FG5EParser.Utilities;
using FG5eParserModels.DM_Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FG5EParser.Base_Class
{
    public class StoryElements
    {
        public int StoryIndex { get; set; } // Keep track of what needs to be linked where later
        public string StoryHeader { get; set; } // Specify what main tab all entries should fall under
        public string StoryTitle { get; set; }
        public string StoryDescription { get; set; } // Needs formatting options
        public string isLocked { get { return "1"; } set { isLocked = value; } }

        public StoryElements bindValues(List<string> _Basic, string Header, string _moduleName)
        {
            StoryElements _storyElement = new StoryElements();
            StringBuilder xml = new StringBuilder();
            StringBuilder _description = new StringBuilder();
            XMLFormatting _xmlFormatting = new XMLFormatting();

            // Variable that will be used in order to process fields that are not mandatory
            string line = _Basic.First();

            while (line != "Its done!")
            {
                if (line.Contains("#@;"))
                {
                    line = shiftUp(_Basic);
                }

                if (line.Contains("##;"))
                {
                    // Adding the page Name
                    _storyElement.StoryTitle = line.Replace("##;", "").Trim();
                    line = shiftUp(_Basic);
                }

                if (line != "Its done!")
                {
                    _description.Append(_xmlFormatting.returnFormattedString(line, _moduleName));
                    line = shiftUp(_Basic);
                }

                // Add in the description details
                _storyElement.StoryDescription = _description.ToString();
            }

            if (!string.IsNullOrEmpty(Header))
            {
                _storyElement.StoryHeader = Header;
            }

            return _storyElement;
        }

        // Makes reading the list variable consistant
        private string shiftUp(List<string> _Basic)
        {
            _Basic.RemoveAt(0);
            if (_Basic.Count != 0)
            {
                return _Basic.First();
            }
            else
            {
                _Basic.Add("Its done!");
                return _Basic.First();
            }
        }
    }

    class Items
    {
        #region PROPERTIES
        // Mandatory
        public string Name { get; set; }
        public string isLocked { get { return "1"; } set { isLocked = value; } }
        public string Type { get; set; }
        public string Subtype { get; set; }
        public string Cost { get; set; }
        public string Weight { get; set; }

        // Armor Related
        public string AC { get; set; }
        public string DexBonus { get; set; }
        public string StrRequired { get; set; }
        public string StealthDisadvantage { get; set; }

        // Weapon Related
        public string Damage { get; set; }
        public string Properties { get; set; }

        // Mounts and Locomotives
        public string Speed { get; set; }
        public string CarryingCapacity { get; set; }

        // Magic Item Related
        public string ACBonus { get; set; }
        public string isIdentified { get; set; }
        public string Rarity { get; set; }
        public string UnidentifiedBaseType { get; set; }
        public string UnidentifiedDescription { get; set; }

        // Misc
        public string Description { get; set; } // Needs formatting options
        public int ItemIndex { get; set; } // Keep track of what needs to be linked where later
        private List<Subitems> _itemList = new List<Subitems>();
        public List<Subitems> Subitems { get { return _itemList; } set { _itemList = value; } }
        public string isTemplate { get { return "0"; } set { isTemplate = value; } }

        #endregion

        public List<Items> bindValues(List<string> _Basic, string itemHeader, string _moduleName)
        {
            Items _item = new Items();
            List<Items> _itemList = new List<Items>();

            StringBuilder xml = new StringBuilder();
            XMLFormatting _xmlFormatting = new XMLFormatting();

            List<string> tableFields = new List<string>();

            // Variable that will be used in order to process fields that are not mandatory
            string line = _Basic.First();

            while (line != "Its done!" && !line.Contains("##;"))
            {
                // Clear heading line
                if (line.Contains("#@;"))
                {
                    line = shiftUp(_Basic);
                }

                // Obtain the possible fields
                if (line.Contains("#th;"))
                {
                    tableFields = line.Split(';').ToList();
                    tableFields.RemoveAt(0);
                    line = shiftUp(_Basic);
                }

                string subTypeName = string.Empty;

                // Obtain the subtype
                if (line.Contains("#st;"))
                {
                    subTypeName = line.Split(';')[1].Trim();
                    line = shiftUp(_Basic);
                }

                while (line != "Its done!" && !line.Contains("#st;") && !line.Contains("##;"))
                {
                    // Break up the line
                    List<string> _itemDetails = line.Split(';').ToList();

                    for (int i = 0; i < tableFields.Count; i++)
                    {
                        if (tableFields[i].Trim() == "Item" || tableFields[i].Trim() == "Armor" || tableFields[i].Trim() == "Name")
                        {
                            _item.Name = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Cost")
                        {
                            _item.Cost = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Speed")
                        {
                            _item.Speed = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Carrying Capacity")
                        {
                            _item.CarryingCapacity = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Damage")
                        {
                            _item.Damage = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Weight")
                        {
                            _item.Weight = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Properties")
                        {
                            _item.Properties = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Armor Class (AC)")
                        {
                            // Check for AC value
                            if (_itemDetails[i].Contains("+"))
                            {
                                _item.AC = _itemDetails[i].Split('+')[0].Trim();

                                if (_itemDetails[i].Split('+')[1].Trim() == "Dex modifier")
                                {
                                    _item.DexBonus = "Yes";
                                }
                                else if (_itemDetails[i].Split('+')[1].Trim() == "Dex modifier (max 2)")
                                {
                                    _item.DexBonus = "Yes (max 2)";
                                }
                                else if (_itemDetails[i].Split('+')[1].Trim() == "Dex modifier (max 3)")
                                {
                                    _item.DexBonus = "Yes (max 3)";
                                }
                                else
                                    _item.DexBonus = "-";
                            }
                            else // For shields
                            {
                                _item.AC = _itemDetails[i].Trim();
                                _item.DexBonus = "-";
                            }
                        }
                        if (tableFields[i].Trim() == "Strength")
                        {
                            _item.StrRequired = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Stealth")
                        {
                            _item.StealthDisadvantage = _itemDetails[i].Trim();
                        }
                    }

                    // Assign constants
                    _item.Type = itemHeader;
                    _item.Subtype = subTypeName;

                    // Add the item to the list
                    _itemList.Add(_item);
                    _item = new Items();

                    // Shift to next item
                    line = shiftUp(_Basic);
                }
            }

            return _itemList;
        }

        public List<Items> bindMagicalValues(List<string> _Basic, string itemHeader, string _moduleName)
        {
            Items _item = new Items();
            List<Items> _itemList = new List<Items>();

            StringBuilder xml = new StringBuilder();
            XMLFormatting _xmlFormatting = new XMLFormatting();

            List<string> tableFields = new List<string>();

            // Variable that will be used in order to process fields that are not mandatory
            string line = _Basic.First();

            while (line != "Its done!" && !line.Contains("##;"))
            {
                // Clear heading line
                if (line.Contains("#@;"))
                {
                    line = shiftUp(_Basic);
                }

                // Obtain the possible fields
                if (line.Contains("#th;"))
                {
                    tableFields = line.Split(';').ToList();
                    tableFields.RemoveAt(0);
                    line = shiftUp(_Basic);
                }

                string type = string.Empty;

                // Obtain the subtype
                if (line.Contains("#st;"))
                {
                    type = line.Split(';')[1].Trim();
                    line = shiftUp(_Basic);
                }

                while (line != "Its done!" && !line.Contains("#st;") && !line.Contains("##;"))
                {
                    // Break up the line
                    List<string> _itemDetails = line.Split(';').ToList();

                    for (int i = 0; i < tableFields.Count; i++)
                    {
                        if (tableFields[i].Trim() == "Item" && i < _itemDetails.Count)
                        {
                            _item.Name = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Rarity" && i < _itemDetails.Count)
                        {
                            _item.Rarity = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Cost" && i < _itemDetails.Count)
                        {
                            _item.Cost = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Weight" && i < _itemDetails.Count)
                        {
                            _item.Weight = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "AC" && i < _itemDetails.Count)
                        {
                            _item.AC = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "AC Bonus" && i < _itemDetails.Count)
                        {
                            _item.ACBonus = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Dex Bonus" && i < _itemDetails.Count)
                        {
                            _item.DexBonus = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Stealth" && i < _itemDetails.Count)
                        {
                            _item.StealthDisadvantage = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Strength" && i < _itemDetails.Count)
                        {
                            _item.StrRequired = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Subtype" && i < _itemDetails.Count)
                        {
                            _item.Subtype = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Damage" && i < _itemDetails.Count)
                        {
                            _item.Damage = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Properties" && i < _itemDetails.Count)
                        {
                            _item.Properties = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Identified" && i < _itemDetails.Count)
                        {
                            if (_itemDetails[i].ToLower().Trim() == "no")
                            {
                                _item.UnidentifiedBaseType = "0";
                            }
                            else
                                _item.UnidentifiedBaseType = "1";
                        }
                        if (tableFields[i].Trim() == "Unidentified Type" && i < _itemDetails.Count)
                        {
                            _item.UnidentifiedBaseType = _itemDetails[i].Trim();
                        }
                        if (tableFields[i].Trim() == "Unidentified Description" && i < _itemDetails.Count)
                        {
                            _item.UnidentifiedDescription = _itemDetails[i].Trim();
                        }
                    }

                    // Assign constants
                    _item.Type = type;
                    //_item.Subtype = subTypeName;

                    // Add the item to the list
                    _itemList.Add(_item);
                    _item = new Items();

                    // Shift to next item
                    line = shiftUp(_Basic);
                }
            }

            return _itemList;
        }

        // Makes reading the list variable consistant
        private string shiftUp(List<string> _Basic)
        {
            _Basic.RemoveAt(0);
            if (_Basic.Count != 0)
            {
                return _Basic.First();
            }
            else
            {
                _Basic.Add("Its done!");
                return _Basic.First();
            }
        }
    }

    class Subitems
    {
        public string ItemName { get; set; }
        public string Count { get; set; }
    }

    class Parcles
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public string isLocked { get { return "1"; } set { isLocked = value; } }

        private List<Coins> _coins = new List<Coins>();
        public List<Coins> coinsList { get { return _coins; } set { _coins = value; } }
        private List<ItemList> _itemList = new List<ItemList>();
        public List<ItemList> itemList { get { return _itemList; } set { _itemList = value; } }

        public List<Parcles> bindValues(List<string> _Basic, string ParcleHeader, string _moduleName)
        {
            Parcles _parcle = new Parcles();
            List<Parcles> _parcleList = new List<Parcles>();
            // Init the lists required
            List<Coins> _listCoins = new List<Coins>();
            List<ItemList> _listItems = new List<ItemList>();

            StringBuilder xml = new StringBuilder();
            XMLFormatting _xmlFormatting = new XMLFormatting();

            // Variable that will be used in order to process fields that are not mandatory
            string line = _Basic.First();

            while (line != "Its done!")
            {
                // Clear heading line
                if (line.Contains("#@;"))
                {
                    line = shiftUp(_Basic);
                }

                // Get the Parcle Name
                if (line.Contains("##;"))
                {
                    _parcle = new Parcles();
                    _parcle.Name = line.Replace("##;", "");
                    line = shiftUp(_Basic);
                    _listCoins = new List<Coins>();
                    _listItems = new List<ItemList>();
                }

                while (line != "Its done!" && !line.Contains("##;"))
                {
                    // Get the items in this parcle
                    while (line != "Its done!" && !line.Contains("##;"))
                    {
                        // Check to see if coins or items
                        if (line.Contains("coin"))
                        {
                            //coin;100;PP 
                            Coins _coins = new Coins();
                            _coins.Name = line.Split(';')[2].Trim();
                            _coins.Amount = line.Split(';')[1].Trim();

                            // Add to the list of coins
                            _listCoins.Add(_coins);
                            line = shiftUp(_Basic);
                        }
                        else if (line.Contains("item"))
                        {
                            //item; 1; Cloak of Invisibility
                            ItemList _itemList = new ItemList();
                            _itemList.Name = line.Split(';')[2].Trim();
                            _itemList.Count = line.Split(';')[1].Trim();

                            // Add to the list of items
                            _listItems.Add(_itemList);
                            line = shiftUp(_Basic);
                        }

                        // Add the sublists to the parent object
                        _parcle.coinsList = _listCoins;
                        _parcle.itemList = _listItems;
                    }                   
                }
                // Add the category
                _parcle.Category = ParcleHeader;
                // Add Parcle to main list 
                _parcleList.Add(_parcle);
            }

            return _parcleList;
        }

        // Makes reading the list variable consistant
        private string shiftUp(List<string> _Basic)
        {
            _Basic.RemoveAt(0);
            if (_Basic.Count != 0)
            {
                return _Basic.First();
            }
            else
            {
                _Basic.Add("Its done!");
                return _Basic.First();
            }
        }
    }

    class Coins
    {
        public string Name { get; set; }
        public string Amount { get; set; }
    }

    class ItemList
    {
        public string Name { get; set; }
        public string Count { get; set; }
    }

    class Encounters
    {
        public List<Encounter> bindValuesNew(List<string> _Basic)
        {
            List<Encounter> _encounterList = new List<Encounter>();
            string currentCategory = string.Empty;

            Encounter _encounter = new Encounter();
            for (int i = 0; i < _Basic.Count; i++)
            {
                // Category
                if (_Basic[i].Contains("#@;"))
                {
                    currentCategory = _Basic[i].Replace("#@;","").Trim();
                }
                if (_Basic[i].Contains("##;"))
                {
                    if (!string.IsNullOrEmpty(_encounter._Name))
                    {
                        _encounter._Category = currentCategory;

                        _encounterList.Add(_encounter);
                        _encounter = new Encounter();
                    }
                    _encounter._Name = _Basic[i].Replace("##;", "").Trim();
                }
                if (_Basic[i].Contains("CR") && _Basic[i].Contains("XP"))
                {
                    _encounter._CR = Convert.ToInt32(_Basic[i].Split(' ')[1]);
                    _encounter._XP = Convert.ToInt32(_Basic[i].Split(' ')[3]);
                }
                if (!_Basic[i].Contains("#@;") && !_Basic[i].Contains("##;") && (!_Basic[i].Contains("CR") && !_Basic[i].Contains("XP")))
                {
                    _encounter._NpcList.Add(new NPCList() {
                        _Count = Convert.ToInt32(_Basic[i].Split(';')[0]),
                        _Name = _Basic[i].Split(';')[1],
                        _UniqueName = _Basic[i].Split(';')[2]
                        // Map coordinates would come here but they dont for now :P
                    });
                }
            }

            // Catch anything left over
            if (!string.IsNullOrEmpty(_encounter._Name))
            {
                _encounter._Category = currentCategory;

                _encounterList.Add(_encounter);
                _encounter = new Encounter();
            }

            return _encounterList;
        }
    }
}
