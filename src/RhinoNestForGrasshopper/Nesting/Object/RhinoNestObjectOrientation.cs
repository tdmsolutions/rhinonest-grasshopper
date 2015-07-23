using System;
using System.Drawing;
using System.Windows.Forms;
using GH_IO.Serialization;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Types;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Object
{
    public class RhinoNestObjectOrientationComponent : GH_Param<GH_Integer>
    {
        public int Value = 0;

        public string[] OrientationText =
        {
            "Fixed",
            "180º",
            "±90º",
            "90º",
            "60º",
            "45º",
            "30º",
            "15º",
            "Free"
        };

        /// <summary>
        ///     Initializes a new instance of the RhinoNestObjectOrientation class.
        /// </summary>
        public RhinoNestObjectOrientationComponent()
            : base("Object - Orientation", "RhinoNest - Object Orientation",
                "Define the rotation freedom",
                "RhinoNest", "Nesting", GH_ParamAccess.item)
        {
        }


        /// <summary>
        ///     Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Resources.IconRhinoNestObjectOrientation;
            }
        }

        /// <summary>
        ///     Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{feb55074-0af0-4a5f-8b7b-cf5a3a3ffb97}"); }
        }

        public override void CreateAttributes()
        {
            m_attributes = new RhinoNestObjectOrientationComponentAttributes(this);
        }

        protected override void CollectVolatileData_Custom()
        {
            m_data.Clear();
            m_data.Append(new GH_Integer(Value));
        }

        public void ChangeValue(Int32 shift)
        {
            RecordUndoEvent("RhinoNest - Object Orientation");
            Value += shift;
            if ((Value < 0))
                Value = OrientationText.GetUpperBound(0);
            if ((Value > OrientationText.GetUpperBound(0)))
                Value = 0;
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            for (var i = 0; i <= OrientationText.Length - 1; i++)
            {
                 menu.Items.Add(OrientationText[i], null, OnClick);
            }
        }

        private void OnClick(object sender, EventArgs eventArgs)
        {
            Value = (Int32) ((ToolStripMenuItem) sender).Tag;
            ExpireSolution(true);
        }

        public override bool Read(GH_IReader reader)
        {
            Value = 0;
            reader.TryGetInt32("rhinonest_freedom", ref Value);
            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("rhinonest_freedom", Value);
            return base.Write(writer);
        }
    }

    public class RhinoNestObjectOrientationComponentAttributes : GH_FloatingParamAttributes
    {
        public RhinoNestObjectOrientationComponentAttributes(RhinoNestObjectOrientationComponent param)
            : base(param)
        {
        }

        public override bool HasOutputGrip
        {
            get { return true; }
        }

        public override bool HasInputGrip
        {
            get { return false; }
        }

        private RectangleF RecNext
        {
            get
            {
                RectangleF rec = RecPrev;
                rec.X -= 18;
                return rec;
            }
        }

        private RectangleF RecPrev
        {
            get
            {
                RectangleF rec = Bounds;
                rec.Inflate(-2, -2);
                rec.Width = 16;
                rec.X = Bounds.Right - 18;
                return rec;
            }
        }

        protected override void Layout()
        {
            Bounds = new RectangleF(Pivot, new Size(100, 20));
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if ((e.Button == MouseButtons.Left))
            {
                if ((RecNext.Contains(e.CanvasLocation)))
                {
                    ((RhinoNestObjectOrientationComponent) Owner).ChangeValue(1);
                    Owner.ExpireSolution(true);
                    return GH_ObjectResponse.Handled;
                }

                if ((RecPrev.Contains(e.CanvasLocation)))
                {
                    ((RhinoNestObjectOrientationComponent) Owner).ChangeValue(-1);
                    Owner.ExpireSolution(true);
                    return GH_ObjectResponse.Handled;
                }
            }

            return base.RespondToMouseDown(sender, e);
        }


        protected override void Render(GH_Canvas iCanvas, Graphics graphics, GH_CanvasChannel iChannel)
        {
            if ((iChannel == GH_CanvasChannel.Objects))
            {
                GH_Capsule capsule = GH_Capsule.CreateCapsule(Bounds, GH_Palette.Normal);
                capsule.AddOutputGrip(OutputGrip);
                capsule.Render(graphics, Selected, Owner.Locked, true);
                capsule.Dispose();
                // Comprovar
                var rhinoNestComponent = (RhinoNestObjectOrientationComponent) Owner;
                var message = rhinoNestComponent.OrientationText[rhinoNestComponent.Value];

  
                var rec = Bounds;
                rec.Inflate(-3, 0);
                var format = new StringFormat
                {
                    Trimming = StringTrimming.EllipsisCharacter,
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near
                };

                graphics.DrawString(message, GH_FontServer.Standard, Brushes.Black, rec, format);
                graphics.DrawImage(Resources.IconButtonDown, RecNext);
                graphics.DrawImage(Resources.IconButtonUp, RecPrev);

                format.Dispose();
            }
        }
    }
}