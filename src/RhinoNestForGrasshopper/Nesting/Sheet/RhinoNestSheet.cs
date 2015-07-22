﻿using System;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper.Nesting.Sheet
{
    public class RhinoNestSheet : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RhinoNestSheet class.
        /// </summary>
        public RhinoNestSheet()
            : base("RhinoNestSheet", "Sheet",
                "Define a sheet by width and height",
                "RhinoNest", "Nesting")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Witdh", "W", "Witdh", GH_ParamAccess.item);
            pManager.AddNumberParameter("Height", "H", "Height", GH_ParamAccess.item);
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
          
            double w = 0;
            double h = 0;
            if (!DA.GetData(0, ref h)) return;
            if ((!DA.GetData(1, ref w))) return;
            
            if (h <= 0) return;
            if (w <= 0) return;

           var  sheet = new RhinoNestKernel.RhinoNestSheet(w,h);
           DA.SetData(0, sheet);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Resources.IconSheetBySize;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{9731acf0-9af5-49ed-a4d3-055583fa60af}"); }
        }
    }
}