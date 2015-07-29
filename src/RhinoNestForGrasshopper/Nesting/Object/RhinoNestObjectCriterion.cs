using System;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Object
{
    /// <summary>
    /// Collates nesting orientation constraints. 
    /// This is the type one would actually use in code.
    /// </summary>
    public class Criter
    {
        #region fields
        private readonly RhinoNestKernel.ObjectCriterion _constraint;
        #endregion

        #region constructor

        /// <summary>
        /// Create a new orientation.
        /// </summary>
        /// <param name="constraint">RhinoNestKernel.ObjectCriterion: Constraint type.</param>
        private Criter(RhinoNestKernel.ObjectCriterion constraint)
        {
            _constraint = constraint;
        }

        /// <summary>
        /// Create a fixed orientation.
        /// </summary>
        /// <param name="value">RhinoNestKernel.ObjectCriterion: Constraint type.</param>
        public static Criter SetCriterion(RhinoNestKernel.ObjectCriterion value)
        {
            return new Criter(value);
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets the constraint type.
        /// </summary>
        public RhinoNestKernel.ObjectCriterion Constraint
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
            : base(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizeSizeByX))
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
            if (Value == null)
                return "Null Criterion";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.ConstructLatticePlacement)
                return "Construct Lattice Placement";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.GivenOrientationAsBasicOne)
                return "Given Orientation As Basic One";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.MinimizeArea)
                return "Minimize Area";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.MinimizePerimeter)
                return "Minimize Perimeter";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.MinimizeSizeByX)
                return "Minimize Size By X";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.MinimizeSizeByY)
                return "Minimize Size By Y";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.OnlySearchForAngleForLattice)
                return "Only Search For Angle For Lattice";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.PerimeterForPairIdenticalOfRectangularHullConstructPair)
                return "Perimeter For Pair Identical Of Rectangular Hull Construct Pair";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.PerimeterForPairIdenticalOfRectangularHullSearchForPair)
                return "Perimeter For Pair Identical Of Rectangular Hull Search For Pair";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalAreaOfRectangularHullConstructPair)
                return "Perimeter Pair With Minimal Area Of Rectangular Hull Construct Pair";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalAreaOfRectangularHullSearchForAngle)
                return "Perimeter Pair With Minimal AreaOf Rectangular Hull Search For Angle";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalPerimeterOfRectangularHullConstructPair)
                return "Perimeter Pair With Minimal Perimeter Of Rectangular Hull ConstructPair";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalPerimeterOfRectangularHullSearchForAngle)
                return "Perimeter Pair With Minimal Perimeter Of Rectangular Hull Search For Angle";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.PerimeterWithMinimalAreaOfRectangularHullConstructPair)
                return "Perimeter With Minimal Area Of Rectangular Hull Construct Pair";

            if (Value.Constraint == RhinoNestKernel.ObjectCriterion.PerimeterWithMinimalAreaOfRectangularHullSearchForPair)
                return "Perimeter With Minimal Area Of Rectangular Hull Search For Pair";

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
        /// Write the Criterion
        /// </summary>
        /// <param name="writer">GH_IO.Serialization.GH_IWriter: Provides acces to a subset of GH_Chunk methods used for writing archives.</param>
        /// <returns>Bool: true.</returns>
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
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
        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (!reader.ItemExists("Criterion"))
            {
                Value = null;
                return true;
            }

            // TODO: this cast may fail, I'm not checking for correctness here.
            RhinoNestKernel.ObjectCriterion constraint = (RhinoNestKernel.ObjectCriterion)reader.GetInt32("Criterion");
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
        public string[] Text =
        {
            "Given Orientation As Basic One",
            "Minimize Size By X",
            "Minimize Size By Y",
            "Minimize Perimeter",
            "Minimize Area",
            "Perimeter For Pair Identical Of Rectangular    Hull - Search For Pair",
            "Perimeter With Minimal Area Of Rectangular Hull - Search For Pair",
            "Perimeter For Pair Identical Of Rectangula     Hull - Construct Pair",
            "Perimeter With Minimal Area Of Rectangula   Hull - Construct Pair",
            "Perimeter Pair With Minimal Perimeter Of Rectangular Hull - Shearch for Angle",
            "Perimeter Pair With Minimal Area Of Rectangular Hull - Shearch for Angle",
            "Only Search For Angle For Lattice",
            "Construct Lattice Placement",
            "Perimeter Pair With Minimal Perimeter Of Rectangular Hull - Construct Pair",
            "Perimeter Pair With Minimal Area Of Rectangular Hull - Construct Pair"
        };
        /// <summary>
        /// Constructor Empty
        /// </summary>
        public Criterion()
            : base("Criterion", "Criterion", "Criterion data for nesting", "RhinoNest", "Nesting")
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.GivenOrientationAsBasicOne));
        }

        /// <summary>
        /// Get off this option.
        /// </summary>
        /// <param name="values">ref CriterionGoo list: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</param>
        /// <returns>GH_GetterResult: Enumerates typica getter results</returns>
        protected override GH_GetterResult Prompt_Plural(ref System.Collections.Generic.List<CriterionGoo> values)
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
        protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomSingleValueItem()
        {
            // TODO: figure out whether one of these is already assigned as persistent data and set the check to TRUE.

            var item = new System.Windows.Forms.ToolStripMenuItem {Text = @"Set an Criterion"};
            Menu_AppendItem(item.DropDown, Text[0], Menu_Op_0);
            Menu_AppendItem(item.DropDown, Text[1], Menu_Op_1);
            Menu_AppendItem(item.DropDown, Text[2], Menu_Op_2);
            Menu_AppendItem(item.DropDown, Text[3], Menu_Op_3);
            Menu_AppendItem(item.DropDown, Text[4], Menu_Op_4);
            Menu_AppendItem(item.DropDown, Text[5], Menu_Op_5);
            Menu_AppendItem(item.DropDown, Text[6], Menu_Op_6);
            Menu_AppendItem(item.DropDown, Text[7], Menu_Op_7);
            Menu_AppendItem(item.DropDown, Text[8], Menu_Op_8);
            Menu_AppendItem(item.DropDown, Text[9], Menu_Op_9);
            Menu_AppendItem(item.DropDown, Text[10], Menu_Op_10);
            Menu_AppendItem(item.DropDown, Text[11], Menu_Op_11);
            Menu_AppendItem(item.DropDown, Text[12], Menu_Op_12);
            Menu_AppendItem(item.DropDown, Text[13], Menu_Op_13);
            Menu_AppendItem(item.DropDown, Text[14], Menu_Op_14);

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
        private void Menu_Op_0(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.GivenOrientationAsBasicOne));
        }
        /// <summary>
        /// Make a function for option 1 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_1(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizeSizeByX));
        }
        /// <summary>
        /// Make a function for option 2 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_2(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizeSizeByY));
        }
        /// <summary>
        /// Make a function for option 3 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_3(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizePerimeter));
        }
        /// <summary>
        /// Make a function for option 4 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_4(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizeArea));
        }
        /// <summary>
        /// Make a function for option 5 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_5(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterForPairIdenticalOfRectangularHullSearchForPair));
        }
        /// <summary>
        /// Make a function for option 6 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_6(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterWithMinimalAreaOfRectangularHullSearchForPair));
        }
        /// <summary>
        /// Make a function for option 7 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_7(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterForPairIdenticalOfRectangularHullConstructPair));
        }
        /// <summary>
        /// Make a function for option 8 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_8(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalAreaOfRectangularHullConstructPair));
        }
        /// <summary>
        /// Make a function for option 9 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_9(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalPerimeterOfRectangularHullSearchForAngle));
        }
        /// <summary>
        /// Make a function for option 10 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_10(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalAreaOfRectangularHullSearchForAngle));
        }
        /// <summary>
        /// Make a function for option 11 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_11(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.OnlySearchForAngleForLattice));
        }
        /// <summary>
        /// Make a function for option 12 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_12(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.ConstructLatticePlacement));
        }
        /// <summary>
        /// Make a function for option 13 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_13(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalPerimeterOfRectangularHullConstructPair));
        }
        /// <summary>
        /// Make a function for option 14 from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_Op_14(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalAreaOfRectangularHullConstructPair));
        }
        #endregion
    }

}