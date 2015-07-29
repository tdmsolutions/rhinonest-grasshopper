using System;
using Grasshopper.Kernel;
using System.Drawing;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting
{
    /// <summary>
    /// Collates nesting orientation constraints. 
    /// This is the type one would actually use in code.
    /// </summary>
    public class GCriter
    {
        #region fields
        private readonly RhinoNestKernel.GlobalNestingCriterion _constraint;
        #endregion

        #region constructor

        /// <summary>
        /// Create a new orientation.
        /// </summary>
        /// <param name="constraint">Constraint type.</param>
        public GCriter(RhinoNestKernel.GlobalNestingCriterion constraint)
        {
            _constraint = constraint;
        }

        
        /// <summary>
        /// Set a Global criterion
        /// </summary>
        /// <param name="value">GlobalNestingCriterion: Set a Global criterion option.</param>
        /// <returns>GCriter: Collates global criterion constraints.</returns>
        public static GCriter SetCriterion(RhinoNestKernel.GlobalNestingCriterion value)
        {
            return new GCriter(value);
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets the globalcriterion type.
        /// </summary>
        public RhinoNestKernel.GlobalNestingCriterion Constraint
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
            : base(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.LowerLeftPoint))
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

                // TODO: test the angle for validity perhaps?
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

                // TODO: test the angle for validity perhaps?
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
            if (Value == null)
                return "Null Global Criterion";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.CenterMinAdditionX2Y2)
                return "Center Min Addition X2 Y2";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.CenterMinAdditionXy)
                return "Center Min Addition Xy";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.CenterMinMaxXy)
                return "Center Min Max Xy";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.CenterMinMultiplicationXy)
                return "Center Min Multiplication Xy";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.LowerLeftPoint)
                return "Lower Left Point";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.LowerRightPoint)
                return "Lower Right Point";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.MaxFreeSpace)
                return "Max Free Space";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.MinLostRegions)
                return "Min Lost Regions";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.MinPerimeterOfNested)
                return "Min Perimeter Of Nested";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.MinSpacerOfNested)
                return "Min Spacer Of Nested";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.MinX)
                return "Min X";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.Random)
                return "Random";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.TryEvery)
                return "Try Every";

            if (Value.Constraint == RhinoNestKernel.GlobalNestingCriterion.TryEveryCenter)
                return "Try Every Center";

            return "Error";
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
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
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
        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (!reader.ItemExists("GCriterion"))
            {
                Value = null;
                return true;
            }

            // TODO: this cast may fail, I'm not checking for correctness here.
            var constraint = (RhinoNestKernel.GlobalNestingCriterion)reader.GetInt32("GCriterion");
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
        public string[] Text =
        {
            "Min X",
            "MinPerimeterOfNested",
            "Min Space Of Nested",
            "Max Space Free",
            "Min Lost Region",
            "Lower Left Point",
            "Lower Right Point",
            "Random",
            "Center Min X+Y",
            "Center Min X*Y",
            "Center Min (2X*2Y)",
            "Center MinMax XY",
            "Try Every",
            "try Every Center"
        };
        /// <summary>
        /// Constructor Empty
        /// </summary>
        public GCriterion()
            : base("Global Criterion", "Global Criterion", "Global Criterion data for nesting", "RhinoNest", "Nesting")
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.MinX));
        }
        /// <summary>
        /// Get off this option.
        /// </summary>
        /// <param name="values">ref CriterionGoo list: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</param>
        /// <returns>GH_GetterResult: Enumerates typica getter results</returns>
        protected override GH_GetterResult Prompt_Plural(ref System.Collections.Generic.List<GCriterionGoo> values)
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
        protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomSingleValueItem()
        {
            // TODO: figure out whether one of these is already assigned as persistent data and set the check to TRUE.

            var item = new System.Windows.Forms.ToolStripMenuItem {Text = @"Set an Criterion"};
            Menu_AppendItem(item.DropDown, Text[0], Menu_GOp_0);
            Menu_AppendItem(item.DropDown, Text[1], Menu_GOp_1);
            Menu_AppendItem(item.DropDown, Text[2], Menu_GOp_2);
            Menu_AppendItem(item.DropDown, Text[3], Menu_GOp_3);
            Menu_AppendItem(item.DropDown, Text[4], Menu_GOp_4);
            Menu_AppendItem(item.DropDown, Text[5], Menu_GOp_5);
            Menu_AppendItem(item.DropDown, Text[6], Menu_GOp_6);
            Menu_AppendItem(item.DropDown, Text[7], Menu_GOp_7);
            Menu_AppendItem(item.DropDown, Text[8], Menu_GOp_8);
            Menu_AppendItem(item.DropDown, Text[9], Menu_GOp_9);
            Menu_AppendItem(item.DropDown, Text[10], Menu_GOp_10);
            Menu_AppendItem(item.DropDown, Text[11], Menu_GOp_11);
            Menu_AppendItem(item.DropDown, Text[12], Menu_GOp_12);
            Menu_AppendItem(item.DropDown, Text[13], Menu_GOp_13);

            return item;
        }
        /// <summary>
        /// Make imposible do a multislection
        /// </summary>
        /// <returns>return null</returns>
        protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomMultiValueItem()
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
        /// Make a function for option 0 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>    
        private void Menu_GOp_0(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.MinX));
        }
        /// <summary>
        /// Make a function for option 1 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_1(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.MinPerimeterOfNested));
        }
        /// <summary>
        /// Make a function for option 2 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_2(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.MinSpacerOfNested));
        }
        /// <summary>
        /// Make a function for option 3 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_3(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.MaxFreeSpace));
        }
        /// <summary>
        /// Make a function for option 4 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_4(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.MinLostRegions));
        }
        /// <summary>
        /// Make a function for option 5 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_5(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.LowerLeftPoint));
        }
        /// <summary>
        /// Make a function for option 6 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_6(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.LowerRightPoint));
        }
        /// <summary>
        /// Make a function for option 7 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_7(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.Random));
        }
        /// <summary>
        /// Make a function for option 8 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_8(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.CenterMinAdditionXy));
        }
        /// <summary>
        /// Make a function for option 9 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_9(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.CenterMinMultiplicationXy));
        }
        /// <summary>
        /// Make a function for option 10 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_10(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.CenterMinAdditionX2Y2));
        }
        /// <summary>
        /// Make a function for option 11 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_11(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.CenterMinMaxXy));
        }
        /// <summary>
        /// Make a function for option 12 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_12(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.TryEvery));
        }
        /// <summary>
        /// Make a function for option 13 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_GOp_13(object sender, EventArgs e)
        {
            SetNewGCriter(GCriter.SetCriterion(RhinoNestKernel.GlobalNestingCriterion.TryEveryCenter));
        }
        #endregion
    }
}