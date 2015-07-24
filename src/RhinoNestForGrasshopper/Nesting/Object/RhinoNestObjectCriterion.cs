using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
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
    public class Criter
    {
        #region fields
        private readonly RhinoNestKernel.ObjectCriterion _constraint;
        #endregion

        #region constructor
        /// <summary>
        /// Create a new orientation.
        /// </summary>
        /// <param name="constraint">Constraint type.</param>
        /// <param name="angle">Angle.</param>
        private Criter(RhinoNestKernel.ObjectCriterion constraint)
        {
            _constraint = constraint;
        }

        // Static creation methods.
        /// <summary>
        /// Create a fixed orientation.
        /// </summary>

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
        public CriterionGoo()
            : base(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizeSizeByX))
        { }
        public CriterionGoo(Criter value)
            : base(value)
        { }
        #endregion


        #region properties
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

        public override string TypeName
        {
            get { return "Criterion"; }
        }
        public override string TypeDescription
        {
            get { return "Criterion settings for nesting"; }
        }
        #endregion

        #region duplication
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
            else
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
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (Value == null)
                return true;

            writer.SetInt32("Criterion", (int)Value.Constraint);
            return true;
        }
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
        public string[] text =
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
        public Criterion()
            : base("Criterion", "Criterion", "Criterion data for nesting", "RhinoNest", "Nesting")
        {

        }

        protected override GH_GetterResult Prompt_Plural(ref System.Collections.Generic.List<CriterionGoo> values)
        {
            return GH_GetterResult.cancel;
        }
        protected override GH_GetterResult Prompt_Singular(ref CriterionGoo value)
        {
            return GH_GetterResult.cancel;
        }


        /// <summary>
        /// This speeds up when someone calls DA.SetData(index, Orientation). 
        /// It is not necessary to override, but it does make it easier for Grasshopper.
        /// </summary>
        protected override CriterionGoo PreferredCast(object data)
        {
            Criter t = data as Criter;
            if (t != null)
                return new CriterionGoo(t);

            return null;
        }
        /// <summary>
        /// This makes it easier for Grasshopper to create new instances of the goo.
        /// It is not necessary to override this method, but it does give a small speed boost.
        /// </summary>
        protected override CriterionGoo InstantiateT()
        {
            return new CriterionGoo();
        }

        protected override Bitmap Icon
        {
            get
            {

                return Resources.IconRhinoNestObjectCriterion;
            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{ce8585a9-3a4b-4b6a-a11b-ce97fca8e695}"); }
        }

        #region custom menu UI
        protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomSingleValueItem()
        {
            // TODO: figure out whether one of these is already assigned as persistent data and set the check to TRUE.

            System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem();
            item.Text = @"Set an Criterion";
            Menu_AppendItem(item.DropDown, text[0], Menu_Op_0);
            Menu_AppendItem(item.DropDown, text[1], Menu_Op_1);
            Menu_AppendItem(item.DropDown, text[2], Menu_Op_2);
            Menu_AppendItem(item.DropDown, text[3], Menu_Op_3);
            Menu_AppendItem(item.DropDown, text[4], Menu_Op_4);
            Menu_AppendItem(item.DropDown, text[5], Menu_Op_5);
            Menu_AppendItem(item.DropDown, text[6], Menu_Op_6);
            Menu_AppendItem(item.DropDown, text[7], Menu_Op_7);
            Menu_AppendItem(item.DropDown, text[8], Menu_Op_8);
            Menu_AppendItem(item.DropDown, text[9], Menu_Op_9);
            Menu_AppendItem(item.DropDown, text[10], Menu_Op_10);
            Menu_AppendItem(item.DropDown, text[11], Menu_Op_11);
            Menu_AppendItem(item.DropDown, text[12], Menu_Op_12);
            Menu_AppendItem(item.DropDown, text[13], Menu_Op_13);
            Menu_AppendItem(item.DropDown, text[14], Menu_Op_14);

            return item;
        }
        protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomMultiValueItem()
        {
            return null;
        }

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
        private void Menu_Op_0(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.GivenOrientationAsBasicOne));
        }
        private void Menu_Op_1(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizeSizeByX));
        }
        private void Menu_Op_2(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizeSizeByY));
        }
        private void Menu_Op_3(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizePerimeter));
        }
        private void Menu_Op_4(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.MinimizeArea));
        }
        private void Menu_Op_5(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterForPairIdenticalOfRectangularHullSearchForPair));
        }
        private void Menu_Op_6(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterWithMinimalAreaOfRectangularHullSearchForPair));
        }
        private void Menu_Op_7(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterForPairIdenticalOfRectangularHullConstructPair));
        }
        private void Menu_Op_8(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalAreaOfRectangularHullConstructPair));
        }
        private void Menu_Op_9(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalPerimeterOfRectangularHullSearchForAngle));
        }
        private void Menu_Op_10(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalAreaOfRectangularHullSearchForAngle));
        }
        private void Menu_Op_11(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.OnlySearchForAngleForLattice));
        }
        private void Menu_Op_12(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.ConstructLatticePlacement));
        }
        private void Menu_Op_13(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalPerimeterOfRectangularHullConstructPair));
        }
        private void Menu_Op_14(object sender, EventArgs e)
        {
            SetNewCriter(Criter.SetCriterion(RhinoNestKernel.ObjectCriterion.PerimeterPairWithMinimalAreaOfRectangularHullConstructPair));
        }
        #endregion
    }

}