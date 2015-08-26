using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using System.Drawing;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using RhinoNestForGrasshopper.Properties;
using RhinoNestKernel;
using RhinoNestKernel.Nesting;

namespace RhinoNestForGrasshopper.Nesting
{
    /// <summary>
    /// Collates nesting orientation constraints. 
    /// This is the type one would actually use in code.
    /// </summary>
    public class GCriter
    {
        #region fields
        private readonly GlobalNestingCriterion _constraint;
        #endregion

        #region constructor

        /// <summary>
        /// Create a new orientation.
        /// </summary>
        /// <param name="constraint">Constraint type.</param>
        public GCriter(GlobalNestingCriterion constraint)
        {
            _constraint = constraint;
        }

        
        /// <summary>
        /// Set a Global criterion
        /// </summary>
        /// <param name="value">GlobalNestingCriterion: Set a Global criterion option.</param>
        /// <returns>GCriter: Collates global criterion constraints.</returns>
        public static GCriter SetCriterion(GlobalNestingCriterion value)
        {
            return new GCriter(value);
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets the globalcriterion type.
        /// </summary>
        public GlobalNestingCriterion Constraint
        {
            get { return _constraint; }
        }
        
    }

    /// <summary>
    /// Goo wrapper for orientation. End-code typically doesn't concern itself with this.
    /// </summary>
    public class GCriterionGoo : GH_Goo<GCriter>
    {
        #region constructors
        public GCriterionGoo()
            : base(GCriter.SetCriterion(GlobalNestingCriterion.LowerLeftPoint))
        { }
        public GCriterionGoo(GCriter value)
            : base(value)
        { }
        #endregion


        #region properties
        /// <summary>
        /// if the value is Valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (Value == null)
                    return false;

                return true;
            }
        }
        /// <summary>
        /// Test te angle for Validity
        /// </summary>
        public override string IsValidWhyNot
        {
            get
            {
                if (Value == null)
                    return "Orientation data is null.";

                return string.Empty;
            }
        }
        /// <summary>
        /// Get the TypeName.
        /// </summary>
        public override string TypeName
        {
            get { return "Global Criterion"; }
        }
        /// <summary>
        /// Get TypeDecription.
        /// </summary>
        public override string TypeDescription
        {
            get { return "Global Criterion settings for nesting"; }
        }
        #endregion

        #region duplication
        /// <summary>
        /// Diplicate the value.
        /// </summary>
        /// <returns>IGH_Goo: Base interface for all data inside Grasshopper.</returns>
        public override IGH_Goo Duplicate()
        {
            // since Orientation is immutable, we can just share it as often as we want.
            return new GCriterionGoo(Value);
        }
        #endregion

        #region conversions
        /// <summary>
        /// Create a human friendly description of this data.
        /// </summary>
        public override string ToString()
        {
            return EnumDescription<GlobalNestingCriterion>.Description(Value.Constraint);
        }
        #endregion

     
        /// <summary>
        /// Return the Value, it is immutable so we don't have to worry about someone changing it.
        /// </summary>
        public override object ScriptVariable()
        {
            return Value;
        }
        #endregion

        #region (de)serialization
        /// <summary>
        /// Write the Global Criterion
        /// </summary>
        /// <param name="writer">GH_IO.Serialization.GH_IWriter: Provides acces to a subset of GH_Chunk methods used for writing archives.</param>
        /// <returns>Bool: true.</returns>
        public override bool Write(GH_IWriter writer)
        {
            if (Value == null)
                return true;

            writer.SetInt32("GCriterion", (int)Value.Constraint);
            return true;
        }
        /// <summary>
        /// Read the global Criterion
        /// </summary>
        /// <param name="reader">GH_IO.Serialization.GH_IReader: Provides acces to a subset of GH_Chunk methods used for reading archives.</param>
        /// <returns>Bool: true.</returns>
        public override bool Read(GH_IReader reader)
        {
            if (!reader.ItemExists("GCriterion"))
            {
                Value = null;
                return true;
            }

            var constraint = (GlobalNestingCriterion)reader.GetInt32("GCriterion");
            Value = GCriter.SetCriterion(constraint);

            return true;
        }
        #endregion
    }

    /// <summary>
    /// IGH_Param implementation for OrientationGoo.
    /// </summary>
    public class GCriterion : GH_PersistentParam<GCriterionGoo>
    {
        /// <summary>
        /// Constructor Empty
        /// </summary>
        public GCriterion()
            : base("Global Criterion", "Global Criterion", "Global Criterion data for nesting", "RhinoNest", "Nesting")
        {
            SetNewGCriter(GCriter.SetCriterion(GlobalNestingCriterion.MinX));
        }
        /// <summary>
        /// Get off this option.
        /// </summary>
        /// <param name="values">ref CriterionGoo list: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</param>
        /// <returns>GH_GetterResult: Enumerates typica getter results</returns>
        protected override GH_GetterResult Prompt_Plural(ref List<GCriterionGoo> values)
        {
            return GH_GetterResult.cancel;
        }
        /// <summary>
        /// Get off this option.
        /// </summary>
        /// <param name="value">ref CriterionGoo list: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</param>
        /// <returns>GH_GetterResult: Enumerates typica getter results</returns>
        protected override GH_GetterResult Prompt_Singular(ref GCriterionGoo value)
        {
            return GH_GetterResult.cancel;
        }


        /// <summary>
        /// This speeds up when someone calls DA.SetData(index, Orientation). 
        /// It is not necessary to override, but it does make it easier for Grasshopper.
        /// </summary>
        protected override GCriterionGoo PreferredCast(object data)
        {
            var t = data as GCriter;
            if (t != null)
                return new GCriterionGoo(t);

            return null;
        }
        /// <summary>
        /// This makes it easier for Grasshopper to create new instances of the goo.
        /// It is not necessary to override this method, but it does give a small speed boost.
        /// </summary>
        protected override GCriterionGoo InstantiateT()
        {
            return new GCriterionGoo();
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
        {
            get
            {
                return Resources.IconRhinoNestNestingGlobalCriterion;
            }
        }
        /// <summary>
        /// Get for Exposure
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{13169c9a-a200-488e-ac59-3a2667cecea7}"); }
        }

        #region custom menu UI
        /// <summary>
        /// Set a function for all option on ToolStripMenuItem.
        /// </summary>
        /// <returns>ToolStripMenuItem: Represent a selectable option displayed on System.Windows.Forms.MenuStrip.</returns>
        protected override ToolStripMenuItem Menu_CustomSingleValueItem()
        {
            var item = new ToolStripMenuItem {Text = @"Set an Criterion"};

            foreach (var val in Enum.GetValues(typeof(GlobalNestingCriterion)).Cast<GlobalNestingCriterion>())
            {
                var it = Menu_AppendItem(item.DropDown,
                    EnumDescription<GlobalNestingCriterion>.Description(val), MenuClick);
                it.Tag = val;
            }

            return item;
        }
        /// <summary>
        /// Make imposible do a multislection
        /// </summary>
        /// <returns>return null</returns>
        protected override ToolStripMenuItem Menu_CustomMultiValueItem()
        {
            return null;
        }
        /// <summary>
        /// Select a new Globalcriterion.
        /// </summary>
        /// <param name="source">GCriter: Collates nesting global criterion constraints.</param>
        private void SetNewGCriter(GCriter source)
        {
            try
            {
                RecordPersistentDataEvent("Set Criterion");
                PersistentData.Clear();

                if (source != null)
                    PersistentData.Append(new GCriterionGoo(source), new GH_Path(0));
            }
            finally
            {
                ExpireSolution(true);
            }
        }
        /// <summary>
        /// Make a function for all option from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>  
        private void MenuClick(object sender, EventArgs e)
        {
            var send = sender as ToolStripMenuItem;
            if (send != null) SetNewGCriter(GCriter.SetCriterion((GlobalNestingCriterion)send.Tag));
        }
        #endregion
    }
}