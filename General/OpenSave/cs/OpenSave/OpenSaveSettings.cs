using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SolidEdge.OpenSave
{
    [Serializable]
    public class OpenSaveSettings
    {
        private ApplicationSettings _applicationSettings = new ApplicationSettings();
        private AssemblySettings _assemblySettings = new AssemblySettings();
        private DraftSettings _draftSettings = new DraftSettings();
        private PartSettings _partSettings = new PartSettings();
        private SheetMetalSettings _sheetMetalSettings = new SheetMetalSettings();
        private WeldmentSettings _weldmentSettings = new WeldmentSettings();

        public OpenSaveSettings()
        {
            Application = new ApplicationSettings();
            Assembly = new AssemblySettings();
            Draft = new DraftSettings();
            Part = new PartSettings();
            SheetMetal = new SheetMetalSettings();
            Weldment = new WeldmentSettings();
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ApplicationSettings Application { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AssemblySettings Assembly { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DraftSettings Draft { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PartSettings Part { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SheetMetalSettings SheetMetal { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public WeldmentSettings Weldment { get; set; }
    }

    [Serializable]
    public class ApplicationSettings
    {
        public ApplicationSettings()
        {
            Visible = true;
        }

        public bool DisableAddins { get; set; }
        public bool DisplayAlerts { get; set; }
        public bool Visible { get; set; }

        public override string ToString()
        {
            return "Application Settings";
        }
    }

    [Serializable]
    public class AssemblySettings
    {
        public override string ToString()
        {
            return "Assembly Settings";
        }
    }

    [Serializable]
    public class DraftSettings
    {
        public bool UpdateDrawingViews { get; set; }

        public override string ToString()
        {
            return "Draft Settings";
        }
    }

    [Serializable]
    public class PartSettings
    {
        public override string ToString()
        {
            return "Part Settings";
        }
    }

    [Serializable]
    public class SheetMetalSettings
    {
        public override string ToString()
        {
            return "SheetMetal Settings";
        }
    }

    [Serializable]
    public class WeldmentSettings
    {
        public override string ToString()
        {
            return "Weldment Settings";
        }
    }
}
