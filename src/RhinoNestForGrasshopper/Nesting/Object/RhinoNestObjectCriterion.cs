using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Object
{
    public class RhinoNestObjectCriterion : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RhinoNestObjectCriterion class.
        /// </summary>
        public RhinoNestObjectCriterion()
            : base("RhinoNestObjectCriterion", "Nickname",
                "Description",
                "RhinoNest", "Nesting")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Resources.IconRhinoNestNesting;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{ce8585a9-3a4b-4b6a-a11b-ce97fca8e695}"); }
        }

    }
}