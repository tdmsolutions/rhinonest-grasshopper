using System;
using System.Drawing;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting
{
    public class RhinoNestNestingParameters : GH_Component
    {
        private RhinoNestKernel.Nesting.RhinoNestNestingParameters _parameters;

        /// <summary>
        ///     Initializes a new instance of the RhinoNestNestingParameters class.
        /// </summary>
        public RhinoNestNestingParameters()
            : base("Nesting - Parameters", "Parameters",
                "Define the parameters for a nesting",
                "RhinoNest", "Nesting")
        {
            _parameters = new RhinoNestKernel.Nesting.RhinoNestNestingParameters();
        }

        /// <summary>
        ///     Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Resources.IconRhinoNestParameters;
            }
        }

        /// <summary>
        ///     Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{f479ed14-13cd-4017-943b-853a7a0acb93}"); }
        }

        /// <summary>
        ///     Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            if (_parameters == null) _parameters = new RhinoNestKernel.Nesting.RhinoNestNestingParameters();
            pManager.AddNumberParameter("Item to Item Distance", "I2I", "Item to Item Distance", GH_ParamAccess.item,
                _parameters.DistanceItemToItem);
            pManager.AddNumberParameter("Item to Sheet Distance", "I2S", "Item to Sheet Distance", GH_ParamAccess.item,
                _parameters.DistanceItemToSheet);
            pManager.AddNumberParameter("Limit of Variants", "LoV", "Limit of Variants", GH_ParamAccess.item,
                _parameters.LimitVariants);
            pManager.AddNumberParameter("Max time", "Mt", "Max time", GH_ParamAccess.item, _parameters.TimeOut);
            pManager.AddNumberParameter("Precision", "P", "Precision", GH_ParamAccess.item,
                _parameters.DistancePrecision);
            pManager.AddParameter(new GCriterion(), "Global Criterion", "GC", "Global Criterion", GH_ParamAccess.item);
        }

        /// <summary>
        ///     Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("RhinoNest Nesting Parameters", "P", "RhinoNest Nesting Parameters");
        }

        /// <summary>
        ///     This is the method that actually does the work.
        /// </summary>
        /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            double i2I = _parameters.DistanceItemToItem;
            double i2S = _parameters.DistanceItemToSheet;
            double limitOfVariants = _parameters.LimitVariants;
            double timeOut = _parameters.TimeOut;
            double precision = _parameters.DistancePrecision;
            var criterion = new GCriterionGoo();
            if (da.GetData(0, ref i2I)) _parameters.DistanceItemToItem = i2I;
            if (da.GetData(1, ref i2S)) _parameters.DistanceItemToSheet = i2S;
            if (da.GetData(2, ref limitOfVariants)) _parameters.LimitVariants = (int) limitOfVariants;
            if (da.GetData(3, ref timeOut)) _parameters.TimeOut = (int) timeOut;
            if (da.GetData(4, ref precision)) _parameters.DistancePrecision = precision;
            if (da.GetData(5, ref criterion)) _parameters.Criterion = criterion.Value.Constraint;

            da.SetData(0, _parameters);
        }
    }
}