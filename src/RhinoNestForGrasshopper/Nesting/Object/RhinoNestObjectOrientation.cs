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
    public class COrientation
    {
        #region fields
        private readonly RhinoNestKernel.ObjectOrientation _constraint;
        private readonly int _angle; // Question, is it possible to allow more than one angle?
        #endregion

        #region constructor
        /// <summary>
        /// Create a new orientation.
        /// </summary>
        /// <param name="constraint">Constraint type.</param>
        /// <param name="angle">Angle.</param>
        public COrientation(RhinoNestKernel.ObjectOrientation constraint, int angle)
        {
            _constraint = constraint;
            _angle = angle;
        }
        public COrientation()
        {
            CreateFixedOrientation();
        }

        // Static creation methods.
        /// <summary>
        /// Create a fixed orientation.
        /// </summary>
        public static COrientation CreateFixedOrientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Fixed, 0);
        }
        /// <summary>
        /// Create a free orientation.
        /// </summary>
        public static COrientation CreateFreeOrientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Free, 0);
        }
        /// <summary>
        /// Create a 15 degree Orientation.
        /// </summary>
        /// <returns></returns>
        public static COrientation Create15Orientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Angle15, 15);
        }
        /// <summary>
        /// Create a 30 degree Orientation.
        /// </summary>
        /// <returns></returns>
        public static COrientation Create30Orientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Angle30, 30);
        }
        /// <summary>
        /// Create a 45 degree Orientation.
        /// </summary>
        /// <returns></returns>
        public static COrientation Create45Orientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Angle45, 45);
        }
        /// <summary>
        /// Create a 60 degree Orientation.
        /// </summary>
        /// <returns></returns>
        public static COrientation Create60Orientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Angle60, 60);
        }
        /// <summary>
        /// Create a 90 degree Orientation.
        /// </summary>
        /// <returns></returns>
        public static COrientation Create90Orientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Angle90, 90);
        }
        /// <summary>
        /// Create a -90 degree Orientation.
        /// </summary>
        /// <returns></returns>
        public static COrientation Createn90Orientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Positive90Negative90, -90);
        }
        /// <summary>
        /// Create a -45 degree Orientation.
        /// </summary>
        /// <returns></returns>
        public static COrientation Createn45Orientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Mirror, -45);
        }
        /// <summary>
        /// Create a 180 degree Orientation.
        /// </summary>
        /// <returns></returns>
        public static COrientation Create180Orientation()
        {
            return new COrientation(RhinoNestKernel.ObjectOrientation.Positive90Negative90, 180);
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets the constraint type.
        /// </summary>
        public RhinoNestKernel.ObjectOrientation Constraint
        {
            get { return _constraint; }
        }
        /// <summary>
        /// Gets the constraint angle. This value only makes sense if the Constraint type is Angle.
        /// </summary>
        public int Angle
        {
            get { return Math.Abs(_angle); }
        }
        /// <summary>
        /// Gets whether the angle constraint allows for both plus and minus rotation.
        /// </summary>
        public bool IsPlusOrMinus
        {
            get { return _angle < 0; }
        }
        #endregion
    }

    /// <summary>
    /// Goo wrapper for orientation. End-code typically doesn't concern itself with this.
    /// </summary>
    public class OrientationGoo : GH_Goo<COrientation>
    {
        #region constructors
        /// <summary>
        /// Constructor Empti default is Fixed.
        /// </summary>
        public OrientationGoo()
            : base(COrientation.CreateFixedOrientation())
        { }
        /// <summary>
        /// Constructor with Orientation.
        /// </summary>
        /// <param name="orientation">COrientation: Collatesnesting orientation constraints</param>
        public OrientationGoo(COrientation orientation)
            : base(orientation)
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
            get { return "Orientation"; }
        }
        /// <summary>
        /// Get TypeDecription.
        /// </summary>
        public override string TypeDescription
        {
            get { return "Orientation settings for nesting"; }
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
            return new OrientationGoo(Value);
        }
        #endregion

        #region conversions
        /// <summary>
        /// Create a human friendly description of this data.
        /// </summary>
        public override string ToString()
        {
            if (Value == null)
                return "Null orientation";

            if (Value.Constraint == RhinoNestKernel.ObjectOrientation.Fixed)
                return "Fixed orientation";

            if (Value.Constraint == RhinoNestKernel.ObjectOrientation.Free)
                return "Free orientation";

            string prefix = string.Empty;
            if (Value.IsPlusOrMinus)
                prefix = "±";

            return string.Format("{0}{1}°", prefix, Value.Angle);
        }

        /// <summary>
        /// Handle casting to Orientation and Integer.
        /// We're not doing doubles yet.
        /// </summary>
        /// <param name="target">Target value to assign.</param>
        /// <returns>Success or failure.</returns>
        public override bool CastTo<TQ>(ref TQ target)
        {
            if (Value == null)
                return false;

            // Cast to Orientation.
            if (typeof(COrientation).IsAssignableFrom(typeof(TQ)))
            {
                target = (TQ)((object)Value);
                return true;
            }

            // Cast to integer.
            if (typeof(int).IsAssignableFrom(typeof(TQ)))
            {
                int angle = Value.Angle;
                if (Value.IsPlusOrMinus)
                    angle = -angle;

                target = (TQ)((object)angle);
                return true;
            }


            return false;
        }
        /// <summary>
        /// We will support casting from Orientation, Integer and String.
        /// </summary>
        /// <param name="source">Data to cast from.</param>
        /// <returns>Success or failure.</returns>
        public override bool CastFrom(object source)
        {
            if (source == null)
                return false;

            if (source is COrientation)
            {
                Value = (COrientation)source;
                return true;
            }

            if (source is int)
            {
                switch ((int)source)
                {
                    case 15:
                        Value = COrientation.Create15Orientation(); break;
                    case 30:
                        Value = COrientation.Create30Orientation(); break;
                    case 45:
                        Value = COrientation.Create45Orientation(); break;
                    case 90:
                        Value = COrientation.Create90Orientation(); break;
                    case 180:
                        Value = COrientation.Create180Orientation(); break;
                    case -90:
                        Value = COrientation.Createn90Orientation(); break;
                    case -45:
                        Value = COrientation.Createn45Orientation(); break;
                }
                return true;
            }

            if (source is string)
            {
                string text = (string)source;
                text = text.Replace("°", string.Empty);
                text = text.Trim();

                if (text.Equals("Fixed", StringComparison.OrdinalIgnoreCase))
                {
                    Value = COrientation.CreateFixedOrientation();
                    return true;
                }

                if (text.Equals("Free", StringComparison.OrdinalIgnoreCase))
                {
                    Value = COrientation.CreateFixedOrientation();
                    return true;
                }

                bool plusOrMinus = false;
                if (text.StartsWith("-") | text.StartsWith("±"))
                {
                    plusOrMinus = true;
                    text = text.Replace("-", string.Empty);
                    text = text.Replace("±", string.Empty);
                }

                int angle;
                if (!int.TryParse(text, out angle))
                    return false;

                if (plusOrMinus)
                    angle = -angle;

                switch (angle)
                {
                    case 15:
                        Value = COrientation.Create15Orientation(); break;
                    case 30:
                        Value = COrientation.Create30Orientation(); break;
                    case 45:
                        Value = COrientation.Create45Orientation(); break;
                    case 90:
                        Value = COrientation.Create90Orientation(); break;
                    case 180:
                        Value = COrientation.Create180Orientation(); break;
                    case -90:
                        Value = COrientation.Createn90Orientation(); break;
                    case -45:
                        Value = COrientation.Createn45Orientation(); break;
                }
                return true;
            }

            return false;
        }
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
        /// Write the Orientation
        /// </summary>
        /// <param name="writer">GH_IO.Serialization.GH_IWriter: Provides acces to a subset of GH_Chunk methods used for writing archives.</param>
        /// <returns> Bool: true.</returns>
        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            if (Value == null)
                return true;

            writer.SetInt32("Constraint", (int)Value.Constraint);
            if ((Value.Constraint != RhinoNestKernel.ObjectOrientation.Fixed) & (Value.Constraint != RhinoNestKernel.ObjectOrientation.Free))
            {
                if (Value.IsPlusOrMinus)
                    writer.SetInt32("Angle", -Value.Angle);
                else
                    writer.SetInt32("Angle", Value.Angle);
            }

            return true;
        }
        /// <summary>
        /// Read the Orientation
        /// </summary>
        /// <param name="reader">GH_IO.Serialization.GH_IReader: Provides acces to a subset of GH_Chunk methods used for reading archives.</param>
        /// <returns>Bool: true.</returns>
        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            if (!reader.ItemExists("Constraint"))
            {
                Value = null;
                return true;
            }

            // TODO: this cast may fail, I'm not checking for correctness here.
            RhinoNestKernel.ObjectOrientation constraint = (RhinoNestKernel.ObjectOrientation)reader.GetInt32("Constraint");
            if (constraint == RhinoNestKernel.ObjectOrientation.Fixed)
                Value = COrientation.CreateFixedOrientation();
            else if (constraint == RhinoNestKernel.ObjectOrientation.Free)
                Value = COrientation.CreateFreeOrientation();
            else
            {
                int angle = reader.GetInt32("Angle");
                switch (angle)
                {
                    case 15:
                        Value = COrientation.Create15Orientation(); break;
                    case 30:
                        Value = COrientation.Create30Orientation(); break;
                    case 45:
                        Value = COrientation.Create45Orientation(); break;
                    case 90:
                        Value = COrientation.Create90Orientation(); break;
                    case 180:
                        Value = COrientation.Create180Orientation(); break;
                    case -90:
                        Value = COrientation.Createn90Orientation(); break;
                    case -45:
                        Value = COrientation.Createn45Orientation(); break;
                }
            }

            return true;
        }
        #endregion
    }

    /// <summary>
    /// IGH_Param implementation for OrientationGoo.
    /// </summary>
    public class Orientation : GH_PersistentParam<OrientationGoo>
    {
        /// <summary>
        /// Constructor Empty
        /// </summary>
        public Orientation()
            : base("Orientation", "Orient", "Orientation data for nesting", "RhinoNest", "Nesting")
        {
            SetNewOrientation(COrientation.CreateFixedOrientation());
        }
        /// <summary>
        /// Get off this option.
        /// </summary>
        /// <param name="values">ref CriterionGoo list: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</param>
        /// <returns>GH_GetterResult: Enumerates typica getter results</returns>
        protected override GH_GetterResult Prompt_Plural(ref System.Collections.Generic.List<OrientationGoo> values)
        {
            return GH_GetterResult.cancel;
        }
        /// <summary>
        /// Get off this option.
        /// </summary>
        /// <param name="value">CriterionGoo: Goo wrapper for orientation. End-Code typically doesn't concern itself with this.</param>
        /// <returns>GH_GetterResult: Enumerates typica getter results</returns>
        protected override GH_GetterResult Prompt_Singular(ref OrientationGoo value)
        {
            return GH_GetterResult.cancel;
        }


        /// <summary>
        /// This speeds up when someone calls DA.SetData(index, Orientation). 
        /// It is not necessary to override, but it does make it easier for Grasshopper.
        /// </summary>
        protected override OrientationGoo PreferredCast(object data)
        {
            var t = data as COrientation;
            if (t != null)
                return new OrientationGoo(t);

            return null;
        }
        /// <summary>
        /// This makes it easier for Grasshopper to create new instances of the goo.
        /// It is not necessary to override this method, but it does give a small speed boost.
        /// </summary>
        protected override OrientationGoo InstantiateT()
        {
            return new OrientationGoo();
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
        {
            get
            {

                return Resources.IconRhinoNestObjectOrientation;
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
            get { return new Guid("{5eb61977-8779-498c-b67d-caddbb2f7fc7}"); }
        }

        #region custom menu UI
        /// <summary>
        /// Set a function for all option on ToolStripMenuItem.
        /// </summary>
        /// <returns>ToolStripMenuItem: Represent a selectable option displayed on System.Windows.Forms.MenuStrip.</returns>
        protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomSingleValueItem()
        {
            // TODO: figure out whether one of these is already assigned as persistent data and set the check to TRUE.

            var item = new System.Windows.Forms.ToolStripMenuItem {Text = @"Set an Orientation"};
            Menu_AppendItem(item.DropDown, "Fixed", Menu_FixedClicked);
            Menu_AppendItem(item.DropDown, "Free", Menu_FreeClicked);
            Menu_AppendSeparator(item.DropDown);
            Menu_AppendItem(item.DropDown, "15", Menu_15Clicked);
            Menu_AppendItem(item.DropDown, "30", Menu_30Clicked);
            Menu_AppendItem(item.DropDown, "45", Menu_45Clicked);
            Menu_AppendItem(item.DropDown, "60", Menu_60Clicked);
            Menu_AppendItem(item.DropDown, "90", Menu_90Clicked);
            Menu_AppendItem(item.DropDown, "180", Menu_180Clicked);
            Menu_AppendSeparator(item.DropDown);
            Menu_AppendItem(item.DropDown, "±45", Menu_pm45Clicked);
            Menu_AppendItem(item.DropDown, "±90", Menu_pm90Clicked);

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
        /// Select a new Orientation.
        /// </summary>
        /// <param name="orientation">COrientation: Collates nesting orientation constraints.</param>
        public void SetNewOrientation(COrientation orientation)
        {
            try
            {
                RecordPersistentDataEvent("Set orientation");
                PersistentData.Clear();

                if (orientation != null)
                    PersistentData.Append(new OrientationGoo(orientation), new GH_Path(0));
            }
            finally
            {
                ExpireSolution(true);
            }
        }
        /// <summary>
        /// Make a function for Fixed option from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_FixedClicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.CreateFixedOrientation());
        }
        /// <summary>
        /// Make a function for free option from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_FreeClicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.CreateFreeOrientation());
        }
        /// <summary>
        /// Make a function for option 15 degree from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_15Clicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.Create15Orientation());
        }
        /// <summary>
        /// Make a function for option 30 degree from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_30Clicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.Create30Orientation());
        }
        /// <summary>
        /// Make a function for option 45 degree from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_45Clicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.Create45Orientation());
        }
        /// <summary>
        /// Make a function for option 60 degree from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_60Clicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.Create60Orientation());
        }
        /// <summary>
        /// Make a function for option 90 degree from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_90Clicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.Create90Orientation());
        }
        /// <summary>
        /// Make a function for option 180 degree from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_180Clicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.Create180Orientation());
        }
        /// <summary>
        /// Make a function for option -45degree from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_pm45Clicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.Createn45Orientation());
        }
        /// <summary>
        /// Make a function for option -90degree from ToolStripMenu
        /// </summary>
        /// <param name="sender">>object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e">RhinoNestEventArgs: Class used in events.</param>
        private void Menu_pm90Clicked(object sender, EventArgs e)
        {
            SetNewOrientation(COrientation.Createn90Orientation());
        }
        #endregion

    }
}