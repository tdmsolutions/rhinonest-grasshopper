using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;
using RhinoNestKernel.Nesting;

namespace RhinoNestForGrasshopper.Nesting
{
    public class RhinoNestReport : GH_Component
    {
        private readonly List<RhinoNestSheetResult> _sheetsresults = new List<RhinoNestSheetResult>();
        private List<string> _reportList = new List<string>(); 
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
            pManager.AddGenericParameter("Report", "R", "Report", GH_ParamAccess.list);
        }

        /// <summary>
        ///     Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Report text", "RT", "Report text", GH_ParamAccess.list);
        }

        /// <summary>
        ///     This is the method that actually does the work.
        /// </summary>
        /// <param name="da">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            da.GetDataList(0, _sheetsresults);
            _reportList=new List<string>();
            int t = 1;
            foreach (var res in _sheetsresults)
            {
                var aux = new List<string>
                {
                    "Sheet : " + t + "\n",
                    "Panel Size : " + res.SheetInfo.PanelSize.X + "x" + res.SheetInfo.PanelSize.Y + "\n",
                    "PanelArea : " + res.SheetInfo.PanelArea + "\n",
                    "Objects Num : " + res.SheetInfo.CountObjects + "\n",
                    "Objects Area : " + res.SheetInfo.TotalAreaObjects + "\n",
                    "Obtimization : " + res.SheetInfo.Optimitzation + "\n",
                    "\n"
                };
                _reportList.AddRange(aux);
                t++;
            }
            
           da.SetDataList(0, _reportList);
        }
    }
}