using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Sheet
{
    public class RhinoNestSheetByCurve : GH_Component
    {
        /// <summary>
        ///     Initializes a new instance of the RhinoNestSheetByCurve class.
        /// </summary>
        public RhinoNestSheetByCurve()
            : base("Sheet by Curve", "Sheet",
                "Define a sheet by a curve (Bounding box)",
                "RhinoNest", "Nesting")
        {
        }

        /// <summary>
        ///     Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
        {
            get { return Resources.IconSheetByCurve; }
        }

        /// <summary>
        ///     Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{7a188019-d001-4602-a733-7e8c9c4aae79}"); }
        }

        /// <summary>
        ///     Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Curvvv", GH_ParamAccess.item);
            //pManager.AddBooleanParameter("Multi Sheet", "MS", "Multi Sheet", GH_ParamAccess.item, true);
        }

        /// <summary>
        ///     Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Sheet", "S", "Sheet Object");
        }

        /// <summary>
        ///     This is the method that actually does the work.
        /// </summary>
        /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            Curve curve = null;
            const bool ms = true;
            if (!da.GetData(0, ref curve)) return;
            //if (!(DA.GetData(1, ref MS))) return;

            if (curve == null) return;

            BoundingBox boundingBox = curve.GetBoundingBox(true);

            double w = Math.Abs(boundingBox.Max.X - boundingBox.Min.X);
            double h = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y);

            if (h <= 0) return;
            if (w <= 0) return;

            var sheet = new RhinoNestKernel.RhinoNestSheet(w, h) {MultiSheet = ms};
            da.SetData(0, sheet);
        }
    }
}