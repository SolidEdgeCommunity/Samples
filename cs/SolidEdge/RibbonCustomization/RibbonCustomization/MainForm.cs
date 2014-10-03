using SolidEdgeCommunity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RibbonCustomization
{
    public partial class MainForm : Form
    {
        private string _themeName = "Community";
        //private string _tabName = "Community";
        private string _tabName = "Home";
        private string _groupName = "Group 1";
        private string _macro = "Notepad.exe";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OleMessageFilter.Register();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OleMessageFilter.Unregister();
        }

        private void buttonCreateTheme_Click(object sender, EventArgs e)
        {
            var application = SolidEdgeUtils.Connect(false);
            var customization = application.Customization;
            var ribbonBarThemes = customization.RibbonBarThemes;
            SolidEdgeFramework.RibbonBarTheme ribbonBarTheme = null;

            // Look for our custom theme.
            foreach (SolidEdgeFramework.RibbonBarTheme theme in ribbonBarThemes)
            {
                if (theme.Name.Equals(_themeName, StringComparison.Ordinal))
                {
                    ribbonBarTheme = theme;
                }
            }

            customization.BeginCustomization();

            // If our theme is not found, create it.
            if (ribbonBarTheme == null)
            {
                ribbonBarTheme = ribbonBarThemes.Create(null);
                ribbonBarTheme.Name = _themeName;
            }

            var ribbonBars = ribbonBarTheme.RibbonBars;
            foreach (SolidEdgeFramework.RibbonBar ribbonBar in ribbonBars)
            {
                // For this demo, only change the ribbon for the active environment.
                if (ribbonBar.Environment.Equals(application.ActiveEnvironment))
                {
                    var ribbonBarTabs = ribbonBar.RibbonBarTabs;

                    // Some environments likely dont' have RibbonBarTabs by default! i.e. Application environment.
                    if (ribbonBarTabs != null)
                    {
                        SolidEdgeFramework.RibbonBarTab ribbonBarTab = null;
                        SolidEdgeFramework.RibbonBarGroup ribbonBarGroup = null;
                        SolidEdgeFramework.RibbonBarControl ribbonBarControl = null;

                        // Check to see if the tab exists.
                        foreach (SolidEdgeFramework.RibbonBarTab tab in ribbonBarTabs)
                        {
                            if (tab.Name.Equals(_tabName, StringComparison.Ordinal))
                            {
                                ribbonBarTab = tab;
                            }
                        }

                        // Create the tab if it does not already exist.
                        if (ribbonBarTab == null)
                        {
                            var tabIndex = ribbonBarTabs.Count; // Insert at the end.
                            ribbonBarTab = ribbonBarTabs.Insert(_tabName, tabIndex, SolidEdgeFramework.RibbonBarInsertMode.seRibbonBarInsertCreate);
                            ribbonBarTab.Visible = true;
                        }

                        var ribbonBarGroups = ribbonBarTab.RibbonBarGroups;

                        // Check to see if the group exists.
                        foreach (SolidEdgeFramework.RibbonBarGroup group in ribbonBarGroups)
                        {
                            if (group.Name.Equals(_groupName, StringComparison.Ordinal))
                            {
                                ribbonBarGroup = group;
                            }
                        }

                        // Create the group if it does not already exist.
                        if (ribbonBarGroup == null)
                        {
                            var groupIndex = ribbonBarGroups.Count; // Insert at the end.
                            ribbonBarGroup = ribbonBarGroups.Insert(_groupName, groupIndex, SolidEdgeFramework.RibbonBarInsertMode.seRibbonBarInsertCreate);
                            ribbonBarGroup.Visible = true;
                        }

                        var ribbonBarControls = ribbonBarGroup.RibbonBarControls;

                        // Check to see if the control exists.
                        foreach (SolidEdgeFramework.RibbonBarControl control in ribbonBarControls)
                        {
                            if (control.Name.Equals(_macro, StringComparison.Ordinal))
                            {
                                ribbonBarControl = control;
                            }
                        }

                        // Create the control if it does not already exist.
                        if (ribbonBarControl == null)
                        {
                            object[] itemArray = { _macro };
                            ribbonBarControl = ribbonBarControls.Insert(itemArray, null, SolidEdgeFramework.RibbonBarInsertMode.seRibbonBarInsertCreateButton);
                            ribbonBarControl.Visible = true;
                        }

                        break;
                    }
                }
            }

            ribbonBarThemes.ActivateTheme(_themeName);
            ribbonBarThemes.Commit();
            customization.EndCustomization();
        }

        private void buttonDeleteTheme_Click(object sender, EventArgs e)
        {
            var application = SolidEdgeUtils.Connect(false);
            var customization = application.Customization;
            var ribbonBarThemes = customization.RibbonBarThemes;
            SolidEdgeFramework.RibbonBarTheme ribbonBarTheme = null;

            // Look for our custom theme.
            foreach (SolidEdgeFramework.RibbonBarTheme theme in ribbonBarThemes)
            {
                if (theme.Name.Equals(_themeName, StringComparison.Ordinal))
                {
                    ribbonBarTheme = theme;
                }
            }

            // If found, delete it.
            if (ribbonBarTheme != null)
            {
                customization.BeginCustomization();
                ribbonBarThemes.Remove(ribbonBarTheme);
                ribbonBarThemes.Commit();
                customization.EndCustomization();
            }
        }
    }
}
