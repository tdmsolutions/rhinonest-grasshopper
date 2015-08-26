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
    public class COrientation
    {
        #region fields
        private readonly ObjectOrientation _constraint;
        private readonly int _angle; // Question, is it possible to allow more than one angle?
        #endregion

        #region constructor
        /// <summary>
        /// Create a new orientation.
        /// </summary>
        /// <param name="constraint">Constraint type.</param>
        /// <param name="angle">Angle.</param>
        public COrientation(ObjectOrientation constraint, int angle)
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
        public static COrientation CreateOrientation(ObjectOrientation obj, int ob)
        {
            return new COrientation(obj, 0);
        }
        public static COrientation CreateFixedOrientation()
        {
            return new COrientation(ObjectOrientation.Fixed, 0);
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets the constraint type.
        /// </summary>
        public ObjectOrientation Constraint
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
            return Value.Constraint.ToString();
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
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle15,15); break;
                    case 30:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle30,30); break;
                    case 45:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle45,45); break;
                    case 90:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle90,90); break;
                    case 180:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Mirror,180); break;
                    case -90:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Positive90Negative90,-90); break;
                }
                return true;
            }

            if (source is string)
            {
                var text = (string)source;
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
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle15, 15); break;
                    case 30:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle30, 30); break;
                    case 45:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle45, 45); break;
                    case 90:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle90, 90); break;
                    case 180:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Mirror, 180); break;
                    case -90:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Positive90Negative90, -90); break;
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
        public override bool Write(GH_IWriter writer)
        {
            if (Value == null)
                return true;

            writer.SetInt32("Constraint", (int)Value.Constraint);
            if ((Value.Constraint != ObjectOrientation.Fixed) & (Value.Constraint != ObjectOrientation.Free))
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
        public override bool Read(GH_IReader reader)
        {
            if (!reader.ItemExists("Constraint"))
            {
                Value = null;
                return true;
            }

            var constraint = (ObjectOrientation)reader.GetInt32("Constraint");
            if (constraint == ObjectOrientation.Fixed)
                Value = COrientation.CreateFixedOrientation();
            else if (constraint == ObjectOrientation.Free)
                Value = COrientation.CreateOrientation(ObjectOrientation.Free, 0);
            else
            {
                int angle = reader.GetInt32("Angle");
                switch (angle)
                {
                    case 15:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle15, 15); break;
                    case 30:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle30, 30); break;
                    case 45:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle45, 45); break;
                    case 90:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Angle90, 90); break;
                    case 180:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Mirror, 180); break;
                    case -90:
                        Value = COrientation.CreateOrientation(ObjectOrientation.Positive90Negative90, -90); break;
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
        protected override GH_GetterResult Prompt_Plural(ref List<OrientationGoo> values)
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
        protected override ToolStripMenuItem Menu_CustomSingleValueItem()
        {
            int a=0;
            var item = new ToolStripMenuItem {Text = @"Set an Orientation"};
            foreach (var val in Enum.GetValues(typeof(ObjectOrientation)).Cast<ObjectOrientation>())
            {
                var it = Menu_AppendItem(item.DropDown,
                    EnumDescription<ObjectOrientation>.Description(val), MenuClick);
                it.Tag = val;
                a++;
                if (a == 2 | a == 8)
                    Menu_AppendSeparator(item.DropDown);
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
        private void MenuClick(object sender, EventArgs e)
        {
            var send = sender as ToolStripMenuItem;
            if (send == null)
                return;

            if (((ObjectOrientation)send.Tag == ObjectOrientation.Fixed) || ((ObjectOrientation)send.Tag == ObjectOrientation.Free))
                SetNewOrientation(COrientation.CreateOrientation((ObjectOrientation)send.Tag,1));
            else if ((ObjectOrientation)send.Tag == ObjectOrientation.Angle15)
                SetNewOrientation(COrientation.CreateOrientation((ObjectOrientation)send.Tag, 15));
            else if ((ObjectOrientation)send.Tag == ObjectOrientation.Angle30)
                SetNewOrientation(COrientation.CreateOrientation((ObjectOrientation)send.Tag, 30));
            else if ((ObjectOrientation)send.Tag == ObjectOrientation.Angle45)
                SetNewOrientation(COrientation.CreateOrientation((ObjectOrientation)send.Tag, 45));
            else if ((ObjectOrientation)send.Tag == ObjectOrientation.Angle60)
                SetNewOrientation(COrientation.CreateOrientation((ObjectOrientation)send.Tag, 60));
            else if ((ObjectOrientation)send.Tag == ObjectOrientation.Angle90)
                SetNewOrientation(COrientation.CreateOrientation((ObjectOrientation)send.Tag, 90));
            else if ((ObjectOrientation)send.Tag == ObjectOrientation.Positive90Negative90)
                SetNewOrientation(COrientation.CreateOrientation((ObjectOrientation)send.Tag, -90));
            else if ((ObjectOrientation)send.Tag == ObjectOrientation.Mirror)
                SetNewOrientation(COrientation.CreateOrientation((ObjectOrientation)send.Tag, 180));
        }
        #endregion

    }
}