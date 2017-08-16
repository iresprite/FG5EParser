﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using FG5eParserLib.Utility;
using FG5eParserLib.View_Mo.dels;

namespace FG5eParserLib.View_Models
{
    public class MainAlternateViewModel : INotifyPropertyChanged
    {
        // Global Path variable for files
        private string PagesPath = string.Empty;
        public ObservableCollection<TabItem> TabList { get; set; }
        public RelayCommand LoadTabControl { get; set; }
        public RelayCommand LoadExistingProject { get; set; }
        private PathViewModel pathViewModel;

        /* PLAYER RELATED PAGES */
        bool Background_Flg = false;
        bool Class_Flg = false;
        bool Equipment_Flg = false;
        bool Feats_Flg = false;        
        bool PinMapping_Flg = false;
        bool Race_Flg = false;
        bool Skill_Flg = false;
        bool Spell_Flg = false;
        bool Parser_Flg = false;

        /* STORY RELATED PAGES */
        bool NPC_Flg = false;
        bool Story_Flg = false;

        // Constructor
        public MainAlternateViewModel()
        {
            // A collection of all the tabs on the screen, initialize only once!
            TabList = new ObservableCollection<TabItem>();

            LoadTabControl = new RelayCommand(populateTabControlList);
            LoadExistingProject = new RelayCommand(loadExistingProject);
            pathViewModel = new PathViewModel();
        }

        private void loadExistingProject(object obj)
        {
            // Get the new location
            FolderBrowserDialog choofdlog = new FolderBrowserDialog();

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                PagesPath = choofdlog.SelectedPath;
                pathViewModel = new PathViewModel();
                TabList.Clear();
            }

            string[] _fileNames = Directory.GetFiles(PagesPath);

            foreach (string item in _fileNames)
            {
                if (item.ToLower().Contains("background"))
                {
                    TabList.Add(new TabItem() { Content = new BackgroundViewModel() { backgroundTextPath = item }, Header = "Background" });
                    pathViewModel.BackgroundPath = item;
                }
                if (item.ToLower().Contains("class"))
                {
                    TabList.Add(new TabItem() { Content = new ClassesViewModel() { ClassesTextPath = item }, Header = "Class" });
                    pathViewModel.ClassPath = item;
                }
                if (item.ToLower().Contains("equipment"))
                {
                    TabList.Add(new TabItem() { Content = new EquipmentViewModel() { EquipmentTextPath = item }, Header = "Equipment" });
                    pathViewModel.EquipmentPath = item;
                }
                if (item.ToLower().Contains("feat"))
                {
                    TabList.Add(new TabItem() { Content = new FeatsViewModel() { FeatsTextPath = item }, Header = "Feat" });
                    pathViewModel.FeatPath = item;
                }
                if (item.ToLower().Contains("npc"))
                {
                    TabList.Add(new TabItem() { Content = new NPCViewModel() { NPCTextPath = item }, Header = "NPC" });
                    pathViewModel.NPCPath = item;
                }
                if (item.ToLower().Contains("pin"))
                {
                    TabList.Add(new TabItem() { Content = new ImagePinsViewModel() { ImagePinsTextPath = item }, Header = "Pin Mapping" });
                    pathViewModel.PinMappingPath = item;
                }
                if (item.ToLower().Contains("race"))
                {
                    TabList.Add(new TabItem() { Content = new RacesViewModel() { RacesTextPath = item }, Header = "Race" });
                    pathViewModel.RacesPath = item;
                }
                if (item.ToLower().Contains("spell"))
                {
                    TabList.Add(new TabItem() { Content = new SpellViewModel() { SpellsTextPath = item }, Header = "Spell" });
                    pathViewModel.SpellPath = item;
                }
                if (item.ToLower().Contains("story"))
                {
                    TabList.Add(new TabItem() { Content = new StoryViewModel() { storyTextPath = item }, Header = "Story" });
                    pathViewModel.StoryPath = item;
                }                
            }

            if (TabList.Count != 0)
            {
                TabList.Add(new TabItem() { Content = pathViewModel, Header = "Parser" });
            }
        }

        private void populateTabControlList(object obj)
        {
            try
            {
                // Check to see if the global path has been setup
                if (string.IsNullOrEmpty(PagesPath))
                {
                    FolderBrowserDialog choofdlog = new FolderBrowserDialog();
                    DialogResult result = choofdlog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        PagesPath = choofdlog.SelectedPath;
                        TabList.Clear();
                    }
                }

                // Single Pages
                if (!string.IsNullOrEmpty(PagesPath))
                {
                    // Check to see which controls are already present in the list
                    validateTabItemList(TabList);

                    if (obj.ToString().ToLower().Trim() == "background")
                    {
                        if (!Background_Flg)
                        {
                            File.Create(PagesPath + @"\Background.txt");
                            TabList.Add(new TabItem() { Content = new BackgroundViewModel() { backgroundTextPath = PagesPath + @"\Background.txt" }, Header = "Background" });
                            pathViewModel.BackgroundPath = PagesPath + @"\Background.txt";
                        }
                    }

                    if (obj.ToString().ToLower().Trim() == "class")
                    {
                        if (!Class_Flg)
                        {
                            File.Create(PagesPath + @"\Class.txt");
                            TabList.Add(new TabItem() { Content = new ClassesViewModel() { ClassesTextPath = PagesPath + @"\Class.txt" }, Header = "Class" });
                            pathViewModel.ClassPath = PagesPath + @"\Class.txt";
                        }
                    }

                    if (obj.ToString().ToLower().Trim() == "equipment")
                    {
                        if (!Equipment_Flg)
                        {
                            File.Create(PagesPath + @"\Equipment.txt");
                            TabList.Add(new TabItem() { Content = new EquipmentViewModel() { EquipmentTextPath = PagesPath + @"\Equipment.txt" }, Header = "Equipment" });
                            pathViewModel.EquipmentPath = PagesPath + @"\Equipment.txt";
                        }
                    }

                    if (obj.ToString().ToLower().Trim() == "feat")
                    {
                        if (!Feats_Flg)
                        {
                            File.Create(PagesPath + @"\Feat.txt");
                            TabList.Add(new TabItem() { Content = new FeatsViewModel() { FeatsTextPath = PagesPath + @"\Feat.txt" }, Header = "Feat" });
                            pathViewModel.FeatPath = PagesPath + @"\Feat.txt";
                        }
                    }

                    if (obj.ToString().ToLower().Trim() == "npc")
                    {
                        if (!NPC_Flg)
                        {
                            File.Create(PagesPath + @"\NPC.txt");
                            TabList.Add(new TabItem() { Content = new NPCViewModel() { NPCTextPath = PagesPath + @"\NPCs.txt" }, Header = "NPC" });
                            pathViewModel.NPCPath = PagesPath + @"\NPC.txt";
                        }
                    }

                    if (obj.ToString().ToLower().Trim() == "pinmapping")
                    {
                        if (!PinMapping_Flg)
                        {
                            File.Create(PagesPath + @"\ImagePin.txt");
                            TabList.Add(new TabItem() { Content = new ImagePinsViewModel() { ImagePinsTextPath = PagesPath + @"\ImagePin.txt" }, Header = "Pin Mapping" });
                            pathViewModel.PinMappingPath = PagesPath + @"\ImagePin.txt";
                        }
                    }

                    if (obj.ToString().ToLower().Trim() == "race")
                    {
                        if (!Race_Flg)
                        {
                            File.Create(PagesPath + @"\Race.txt");
                            TabList.Add(new TabItem() { Content = new RacesViewModel() { RacesTextPath = PagesPath + @"\Race.txt" }, Header = "Race" });
                            pathViewModel.RacesPath = PagesPath + @"\Race.txt";
                        }
                    }

                    //if (obj.ToString().ToLower().Trim() == "skill")
                    //{
                    //    if (!Skill_Flg)
                    //    {
                    //        File.Create(PagesPath + @"\Skill.txt");
                    //        TabList.Add(new TabItem() { Content = new SkillViewModel() { RacesTextPath = PagesPath + @"\Skill.txt" }, Header = "Skill" });
                    //    }
                    //}

                    if (obj.ToString().ToLower().Trim() == "spell")
                    {
                        if (!Spell_Flg)
                        {
                            File.Create(PagesPath + @"\Spell.txt");
                            TabList.Add(new TabItem() { Content = new SpellViewModel() { SpellsTextPath = PagesPath + @"\Spell.txt" }, Header = "Spell" });
                            pathViewModel.SpellPath = PagesPath + @"\Spell.txt";
                        }
                    }

                    if (obj.ToString().ToLower().Trim() == "story")
                    {
                        if (!Story_Flg)
                        {
                            File.Create(PagesPath + @"\Story.txt");
                            TabList.Add(new TabItem() { Content = new StoryViewModel() { storyTextPath = PagesPath + @"\Story.txt" }, Header = "Story" });
                            pathViewModel.StoryPath = PagesPath + @"\Story.txt";
                        }
                    }

                    if (obj.ToString().ToLower().Trim() == "parser")
                    {
                        if (!Parser_Flg)
                        {
                            TabList.Add(new TabItem() { Content = pathViewModel, Header = "Parser" });
                        }
                    }

                    //Presets
                    //if (obj.ToString().ToLower().Trim() == "adventuremod")
                    //{
                    //}
                    //if (obj.ToString().ToLower().Trim() == "playermod")
                    //{
                    //}
                    //if (obj.ToString().ToLower().Trim() == "splatmod")
                    //{
                    //}
                }
            }
            catch
            {
                throw;
            }
        }

        private void validateTabItemList(ObservableCollection<TabItem> _tabList)
        {
            foreach (TabItem item in _tabList)
            {                
                if (item.Header.ToString() == "Background") Background_Flg = true;
                if (item.Header.ToString() == "Class") Class_Flg = true;
                if (item.Header.ToString() == "Equipment") Equipment_Flg = true;
                if (item.Header.ToString() == "Feat") Feats_Flg = true;
                if (item.Header.ToString() == "NPC") NPC_Flg = true;
                if (item.Header.ToString() == "Pin Mapping") PinMapping_Flg = true;
                if (item.Header.ToString() == "Race") Race_Flg = true;
                if (item.Header.ToString() == "Skill") Skill_Flg = true;
                if (item.Header.ToString() == "Spell") Spell_Flg = true;

                if (item.Header.ToString() == "Parser") Parser_Flg = true;

                if (item.Header.ToString() == "Story") Story_Flg = true;
            }
        }

        #region PROPERTY CHANGES
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
