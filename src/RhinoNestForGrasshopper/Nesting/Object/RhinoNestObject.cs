using System;
using System.Drawing;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Object
{
    public class RhinoNestObject : GH_Component
    {
        private System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject> _parameters2 = new System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject>();
        private System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject> _parameters = new System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject>();
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
            pManager.AddCurveParameter("Curve", "G", "Closed Curves", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Copies", "C", "Copies", GH_ParamAccess.item,1);
            pManager.AddIntegerParameter("Priority", "P", "Priority", GH_ParamAccess.item,1);
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
            pManager.Register_GenericParam("RhinoNest Object ", "O", "RhinoNest Object");
        }

        /// <summary>
        ///     This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            System.Collections.Generic.List<Rhino.Geometry.Curve> Object=new System.Collections.Generic.List<Rhino.Geometry.Curve>(100);
            
            OrientationGoo orientation= new OrientationGoo();
            CriterionGoo criterion = new CriterionGoo();
            Int32 copies = 0;
            Int32 priority = 0;
            
            _parameters.Clear();
            if (DA.GetDataList(0, Object)) _parameters2 = new  System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject>();
            if (DA.GetData(1, ref copies)) {}
            if (DA.GetData(2, ref priority)){}
            if (DA.GetData(3, ref orientation)) {}
            if (DA.GetData(4, ref criterion)) { }

            RhinoNestKernel.RhinoNestObject obj;
            //_parameters2.Capacity=Object.Count;
            for (int i=0; i<Object.Count;i++)
            {
                obj=new RhinoNestKernel.RhinoNestObject(Object[i]); 
                obj.Parameters.Copies = copies;
                obj.Parameters.Priority = priority;
                obj.Parameters.Orientation = orientation.Value.Constraint;
                obj.Parameters.Criterion = criterion.Value.Constraint;
                 _parameters2.Add(obj);
               
            }


            _parameters.AddRange(_parameters2);
            DA.SetDataList(0, _parameters);
            //RhinoNestKernel.RhinoNestObject obj = new RhinoNestKernel.RhinoNestObject();
            //TODO: Implementar
        }
    }
}