using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;
using System;
using System.Collections.Generic;

namespace RhinoNestForGrasshopper.Nesting.Object
{
    public class RhinboNestObjectGetter : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RhinboNestObjectGetter class.
        /// </summary>
        public RhinboNestObjectGetter()
          : base("Smart Object", "so",
              "Multiple objects getter",
              "RhinoNest", "Nesting")
        {
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{e03f789c-db86-4325-8adc-e9e97f90c01e}"); }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Resources.IconRhinoNestObject;
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ids", "ids", "input objects ids", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Copies", "C", "Copies", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Priority", "P", "Priority", GH_ParamAccess.item, 1);
            pManager.AddParameter(new Orientation(), "Orientation", "O", "Orientation", GH_ParamAccess.item);
            // Ha de ser un ObjectOrientation
            pManager.AddParameter(new Criterion(), "Criterion", "Cr", "Criterion", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("RhinoNest Object ", "O", "RhinoNest Object");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            var parameters = new List<RhinoNestKernel.Nesting.RhinoNestObject>();

            //Declaration.
            var Object = new List<Guid>();
            var orientation = new OrientationGoo();
            var criterion = new CriterionGoo();
            var copies = 0;
            var priority = 0;
            //clear value and get the objects.
            if (!da.GetDataList(0, Object)) return;
            da.GetData(1, ref copies);
            da.GetData(2, ref priority);
            da.GetData(3, ref orientation);
            da.GetData(4, ref criterion);

            var organizer = new RhinoNestKernel.Curves.OrganizeObjects(Object);
            var res = organizer.Sort();

            foreach (var rhinoNestObject in organizer.Result)
            {
                rhinoNestObject.Parameters.Priority = priority;
                rhinoNestObject.Parameters.Copies = copies;
                rhinoNestObject.Parameters.Criterion = criterion.Value.Constraint;
                rhinoNestObject.Parameters.Orientation = orientation.Value.Constraint;
                parameters.Add(rhinoNestObject);
            }
            //Put the list to Output.
            da.SetDataList(0, parameters);
        }
    }
}