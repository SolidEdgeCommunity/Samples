using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SolidEdge.OpenSave
{
    public class OpenSaveSettings : MarshalByRefObject
    {
        private ApplicationSettings _applicationSettings = new ApplicationSettings();
        private AssemblySettings _assemblySettings = new AssemblySettings();
        private DraftSettings _draftSettings = new DraftSettings();
        private PartSettings _partSettings = new PartSettings();
        private SheetMetalSettings _sheetMetalSettings = new SheetMetalSettings();
        private WeldmentSettings _weldmentSettings = new WeldmentSettings();

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ApplicationSettings Application
        {
            get { return _applicationSettings; }
            set { _applicationSettings = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AssemblySettings Assembly
        {
            get { return _assemblySettings; }
            set { _assemblySettings = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DraftSettings Draft
        {
            get { return _draftSettings; }
            set { _draftSettings = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PartSettings Part
        {
            get { return _partSettings; }
            set { _partSettings = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SheetMetalSettings SheetMetal
        {
            get { return _sheetMetalSettings; }
            set { _sheetMetalSettings = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public WeldmentSettings Weldment
        {
            get { return _weldmentSettings; }
            set { _weldmentSettings = value; }
        }
    }

    public class ApplicationSettings : MarshalByRefObject
    {
        private bool _disableAddins;
        private bool _visible = true;

        public bool DisableAddins
        {
            get { return _disableAddins; }
            set { _disableAddins = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public override string ToString()
        {
            return "Application Settings";
        }
    }

    public class AssemblySettings : MarshalByRefObject
    {
        public override string ToString()
        {
            return "Assembly Settings";
        }
    }

    public class DraftSettings : MarshalByRefObject
    {
        private bool _updateDrawingViews;

        public bool UpdateDrawingViews
        {
            get { return _updateDrawingViews; }
            set { _updateDrawingViews = value; }
        }

        public override string ToString()
        {
            return "Draft Settings";
        }
    }

    public class PartSettings : MarshalByRefObject
    {
        public override string ToString()
        {
            return "Part Settings";
        }
    }

    public class SheetMetalSettings : MarshalByRefObject
    {
        public override string ToString()
        {
            return "SheetMetal Settings";
        }
    }

    public class WeldmentSettings : MarshalByRefObject
    {
        public override string ToString()
        {
            return "Weldment Settings";
        }
    }
}
