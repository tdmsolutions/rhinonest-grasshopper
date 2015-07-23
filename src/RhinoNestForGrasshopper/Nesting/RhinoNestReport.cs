using System;
using System.Drawing;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting
{
    public class RhinoNestReport : GH_Component
    {
        /// <summary>
        ///     Initializes a new instance of the RhinoNestReport class.
        /// </summary>
        public RhinoNestReport()
            : base("Report", "Report",
                "Show the result of a report in a friendly way",
                "RhinoNest", "Nesting")
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
                return Resources.IconRhinoNestReport;
            }
        }

        /// <summary>
        ///     Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{4de7aef2-9b12-43d3-b2d1-792486f1f533}"); }
        }

        /// <summary>
        ///     Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        /// <summary>
        ///     Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        ///     This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }
    }
}