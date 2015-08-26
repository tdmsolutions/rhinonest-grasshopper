using System;
using System.Drawing;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Sheet
{
    public class RhinoNestSheet : GH_Component
    {
        /// <summary>
        ///     Initializes a new instance of the RhinoNestSheet class.
        /// </summary>
        public RhinoNestSheet()
            : base("Sheet", "Sheet",
                "Define a sheet by width and height",
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
                return Resources.IconSheetBySize;
            }
        }

        /// <summary>
        ///     Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{9731acf0-9af5-49ed-a4d3-055583fa60af}"); }
        }

        /// <summary>
        ///     Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Witdh", "W", "Witdh", GH_ParamAccess.item,100);
            pManager.AddNumberParameter("Height", "H", "Height", GH_ParamAccess.item,100);
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
            double w = 0;
            double h = 0;
            const bool _ms = true;

            if (!da.GetData(0, ref h)) return;
            if ((!da.GetData(1, ref w))) return;
            //if ((!DA.GetData(2, ref MS))) return;

            if (h <= 0) return;
            if (w <= 0) return;

            var sheet = new RhinoNestKernel.Nesting.RhinoNestSheet(w, h) { MultiSheet = _ms };
            da.SetData(0, sheet);
        }
    }
}