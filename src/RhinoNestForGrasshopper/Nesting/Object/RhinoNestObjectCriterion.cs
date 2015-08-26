using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using RhinoNestForGrasshopper.Properties;
using RhinoNestKernel;
using RhinoNestKernel.Nesting;

namespace RhinoNestForGrasshopper.Nesting.Object
{
    /// <summary>
    /// Collates nesting orientation constraints. 
    /// This is the type one would actually use in code.
    /// </summary>
    public class Criter
    {
        #region fields
        private readonly ObjectCriterion _constraint;
        #endregion

        #region constructor

        /// <summary>
        /// Create a new orientation.
        /// </summary>
        /// <param name="constraint">RhinoNestKernel.ObjectCriterion: Constraint type.</param>
        private Criter(ObjectCriterion constraint)
        {
            _constraint = constraint;
        }

        /// <summary>
        /// Create a fixed orientation.
        /// </summary>
        /// <param name="value">RhinoNestKernel.ObjectCriterion: Constraint type.</param>
        public static Criter SetCriterion(ObjectCriterion value)
        {
            return new Criter(value);
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets the constraint type.
        /// </summary>
        public ObjectCriterion Constraint
        {
            get { return _constraint; }
        }
        
    }

    /// <summary>
    /// Goo wrapper for orientation. End-code typically doesn't concern itself with this.
    /// </summary>
    public class CriterionGoo : GH_Goo<Criter>
    {
        #region constructors
        /// <summary>
        /// Constructors Empty.
        /// </summary>
        public CriterionGoo()
            : base(Criter.SetCriterion(ObjectCriterion.MinimizeSizeByX))
        { }
        /// <summary>
        /// Constructor with Criter.
        /// </summary>
        /// <param name="value">Criter: Collates nesting orientation constraints.</param>
        public CriterionGoo(Criter value)
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
            get { return "Criterion"; }
        }

        /// <summary>
        /// Get TypeDecription.
        /// </summary>
        public override string TypeDescription
        {
            get { return "Criterion settings for nesting"; }
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
            return new CriterionGoo(Value);
        }
        #endregion

        #region conversions
        /// <summary>
        /// Create a human friendly description of this data.
        /// </summary>
        public override string ToString()
        {
            return EnumDescription<ObjectCriterion>.Description(Value.Constraint);
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
        /// Write the Criterion
        /// </summary>
        /// <param name="writer">GH_IO.Serialization.GH_IWriter: Provides acces to a subset of GH_Chunk methods used for writing archives.</param>
        /// <returns>Bool: true.</returns>
        public override bool Write(GH_IWriter writer)
        {
            if (Value == null)
                return true;

            writer.SetInt32("Criterion", (int)Value.Constraint);
            return true;
        }
        /// <summary>
        /// Read the Criterion
        /// </summary>
        /// <param name="reader">GH_IO.Serialization.GH_IReader: Provides acces to a subset of GH_Chunk methods used for reading archives.</param>
        /// <returns>Bool: true.</returns>
        public override bool Read(GH_IReader reader)
        {
            if (!reader.ItemExists("Criterion"))
            {
                Value = null;
                return true;
            }

            var constraint = (ObjectCriterion)reader.GetInt32("Criterion");
            Value = Criter.SetCriterion(constraint);

            return true;
        }
        #endregion
    }

    /// <summary>
    /// IGH_Param implementation for OrientationGoo.
    /// </summary>
    public class Criterion : GH_PersistentParam<CriterionGoo>
    {
        
        /// <summary>
        /// Constructor Empty
        /// </summary>
        public Criterion()
            : base("Criterion", "Criterion", "Criterion data for nesting", "RhinoNest", "Nesting")
        {
            SetNewCriter(Criter.SetCriterion(ObjectCriterion.GivenOrientationAsBasicOne));
        }

        /// <summary>
        /// Get off this option.
        /// </summary>
        /// <param name="values">ref CriterionGoo list: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</param>
        /// <returns>GH_GetterResult: Enumerates typica getter results</returns>
        protected override GH_GetterResult Prompt_Plural(ref List<CriterionGoo> values)
        {
            return GH_GetterResult.cancel;
        }
        /// <summary>
        /// Get off this option.
        /// </summary>
        /// <param name="value">CriterionGoo: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</param>
        /// <returns>GH_GetterResult: Enumerates typica getter results</returns>
        protected override GH_GetterResult Prompt_Singular(ref CriterionGoo value)
        {
            return GH_GetterResult.cancel;
        }


        /// <summary>
        /// This speeds up when someone calls DA.SetData(index, Orientation)
        /// </summary>
        /// <param name="data">object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <returns>CriterionGoo: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</returns>
        protected override CriterionGoo PreferredCast(object data)
        {
            var t = data as Criter;
            if (t != null)
                return new CriterionGoo(t);

            return null;
        }
        
        /// <summary>
        /// This makes it easier for Grasshopper to create new instances of the goo.
        /// </summary>
        /// <returns>CriterionGoo: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</returns>
        protected override CriterionGoo InstantiateT()
        {
            return new CriterionGoo();
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
        {
            get
            {

                return Resources.IconRhinoNestObjectCriterion;
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
            get { return new Guid("{ce8585a9-3a4b-4b6a-a11b-ce97fca8e695}"); }
        }

        #region custom menu UI
        /// <summary>
        /// Set a function for all option on ToolStripMenuItem.
        /// </summary>
        /// <returns>ToolStripMenuItem: Represent a selectable option displayed on System.Windows.Forms.MenuStrip.</returns>
        protected override ToolStripMenuItem Menu_CustomSingleValueItem()
        {
            var item = new ToolStripMenuItem {Text = @"Set an Criterion"};

            foreach (var val in Enum.GetValues(typeof(ObjectCriterion)).Cast<ObjectCriterion>())
            {
                var it = Menu_AppendItem(item.DropDown,
                    EnumDescription<ObjectCriterion>.Description(val), MenuClick);
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
        /// Select a new Criter.
        /// </summary>
        /// <param name="source">Criter: Collates nesting Criterion constraints.</param>
        private void SetNewCriter(Criter source)
        {
            try
            {
                RecordPersistentDataEvent("Set Criterion");
                PersistentData.Clear();

                if (source != null)
                    PersistentData.Append(new CriterionGoo(source), new GH_Path(0));
            }
            finally
            {
                ExpireSolution(true);
            }
        }

        /// <summary>
        /// Make a function for option 0 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void MenuClick(object sender, EventArgs e)
        {
            var send = sender as ToolStripMenuItem;
            if (send!=null)
                SetNewCriter(Criter.SetCriterion((ObjectCriterion)send.Tag));
        }

        #endregion
    }

}