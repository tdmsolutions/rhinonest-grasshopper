using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Object
{
    /// <summary>
    /// Class to make a class of curves with priority, Copies,Orientation and criterion.
    /// </summary>
    public class RhinoNestObject : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RhinoNestObjectParameters class.
        /// </summary>
        public RhinoNestObject()
            : base("Object", "Object",
                "Defines the object and paramaters",
                "RhinoNest", "Nesting")
        {
            
        }

        /// <summary>
        /// Provides an Icon for the component.
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
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{a178c667-726e-4b8d-a68a-11c0e7b55517}"); }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        /// <param name="pManager">GH_InputParamManager: This class is used during Components constructions to add input parameters.</param>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "G", "Closed Curves", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Copies", "C", "Copies", GH_ParamAccess.item,1);
            pManager.AddIntegerParameter("Priority", "P", "Priority", GH_ParamAccess.item,1);
            pManager.AddParameter(new Orientation(),"Orientation", "O", "Orientation", GH_ParamAccess.item);
                // Ha de ser un ObjectOrientation 
            pManager.AddParameter(new Criterion(),"Criterion", "Cr", "Criterion", GH_ParamAccess.item);
                // Ha de ser un ObjectCriterion 
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        /// <param name="pManager">GH_OutputParamManager: This class is used during Components constructions to add Output parameters.</param>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("RhinoNest Object ", "O", "RhinoNest Object");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="da">IGH_DataAccess: The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            var parameters = new List<RhinoNestKernel.Nesting.RhinoNestObject>();
        
            //Declaration.
            var Object=new List<Curve>();
            var orientation= new OrientationGoo();
            var criterion = new CriterionGoo();
            var copies = 0;
            var priority = 0;
            
            //clear value and get the objects.
            if (!da.GetDataList(0, Object)) return;
            da.GetData(1, ref copies);
            da.GetData(2, ref priority);
            da.GetData(3, ref orientation);
            da.GetData(4, ref criterion);

            //asignament of all objects on list.
            foreach (Curve t in Object)
            {
                var obj = new RhinoNestKernel.Nesting.RhinoNestObject(t)
                {
                    Parameters =
                    {
                        Copies = copies,
                        Priority = priority,
                        Orientation = orientation.Value.Constraint,
                        Criterion = criterion.Value.Constraint
                    }
                };
                parameters.Add(obj);
            }

            //Put the list to Output.
            da.SetDataList(0, parameters);
        }
    }
}