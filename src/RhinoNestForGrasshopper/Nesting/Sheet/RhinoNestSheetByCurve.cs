using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Sheet
{
    public class RhinoNestSheetByCurve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RhinoNestSheetByCurve class.
        /// </summary>
        public RhinoNestSheetByCurve()
            : base("RhinoNestSheetByCurve", "Sheet",
                "Description",
                "RhinoNest", "Nesting")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Curvvv", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Sheet", "S", "Sheet Object");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) return;

            if (curve == null) return;

            var boundingBox = curve.GetBoundingBox(true);

            var w =   Math.Abs(boundingBox.Max.X-boundingBox.Min.X);
            var h = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y);
           
            if (h <= 0) return;
            if (w <= 0) return;

            var sheet = new RhinoNestKernel.RhinoNestSheet(w, h);
            DA.SetData(0, sheet);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.IconSheetByCurve;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{7a188019-d001-4602-a733-7e8c9c4aae79}"); }
        }
    }
}