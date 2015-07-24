using System;
using System.Drawing;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Object
{
    public class RhinoNestObject : GH_Component
    {
        /// <summary>
        ///     Initializes a new instance of the RhinoNestObjectParameters class.
        /// </summary>
        public RhinoNestObject()
            : base("Object", "Object",
                "Defines the object and paramaters",
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
                return Resources.IconRhinoNestObject;
            }
        }

        /// <summary>
        ///     Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{a178c667-726e-4b8d-a68a-11c0e7b55517}"); }
        }

        /// <summary>
        ///     Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Closed Curves", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Copies", "C", "Copies", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Priority", "P", "Priority", GH_ParamAccess.item);
            pManager.AddParameter(new Orientation(),"Orientation", "O", "Orientation", GH_ParamAccess.item);
                // Ha de ser un ObjectOrientation 
            pManager.AddParameter(new Criterion(),"Criterion", "Cr", "Criterion", GH_ParamAccess.item);
                // Ha de ser un ObjectCriterion 
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
            //TODO: Implementar
        }
    }
}