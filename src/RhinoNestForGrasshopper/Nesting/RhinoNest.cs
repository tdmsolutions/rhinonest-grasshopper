using System;
using Rhino;
using System.Drawing;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;
using Rhino.Geometry;

namespace RhinoNestForGrasshopper.Nesting
{
    public class RhinoNestComponent_Attributes : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {

        public RhinoNestComponent_Attributes(RhinoNest comp)
            : base(comp)
        {
        }
        public override Grasshopper.GUI.Canvas.GH_ObjectResponse RespondToMouseDoubleClick(Grasshopper.GUI.Canvas.GH_Canvas sender, Grasshopper.GUI.GH_CanvasMouseEvent e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) 
            {
                try
                {
                    ((RhinoNest)Owner).m_solveme = true;
                    Owner.ExpireSolution(true);
                }
                finally
                {
                    ((RhinoNest)Owner).m_solveme = false;
                }
            }
        return base.RespondToMouseDoubleClick(sender, e);
        }

        protected override void Render(Grasshopper.GUI.Canvas.GH_Canvas iCanvas, Graphics graphics, Grasshopper.GUI.Canvas.GH_CanvasChannel iChannel)
        {
            if ((iChannel == Grasshopper.GUI.Canvas.GH_CanvasChannel.Objects))
            {
                PointF pointer = new PointF(Bounds.Location.X,Bounds.Location.Y);

                if((iCanvas.Viewport.IsVisible(ref pointer ,5.0f)))
                {
                    System.Drawing.PointF pt = new System.Drawing.PointF(0.5f * (Bounds.Left + Bounds.Right), Bounds.Top);
                    Grasshopper.GUI.GH_GraphicsUtil.RenderBalloonTag(iCanvas.Graphics, "TDM solutions", pt, iCanvas.Viewport.VisibleRegion);
                }
            }

            base.Render(iCanvas, graphics, iChannel);
        }
    }
    public class RhinoNest : GH_Component
    {
        private System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject>  Object;
        private System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject>  ObjectNON = new System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject>();
        private System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject> Objectresu = new System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject>();
        private RhinoNestKernel.RhinoNestSheet sheets;
        private System.Collections.Generic.List<Grasshopper.Kernel.Types.IGH_GeometricGoo> GeometricGooNotNesting = new System.Collections.Generic.List<Grasshopper.Kernel.Types.IGH_GeometricGoo>();
        private System.Collections.Generic.List<Grasshopper.Kernel.Types.IGH_GeometricGoo> GeometricGooNesting = new System.Collections.Generic.List<Grasshopper.Kernel.Types.IGH_GeometricGoo>();
        private System.Collections.Generic.List<GeometryBase> unnestedGeometry = new System.Collections.Generic.List<GeometryBase>();
        private System.Collections.Generic.List<GeometryBase> nestedGeometry = new System.Collections.Generic.List<GeometryBase>();
        private RhinoNestKernel.RhinoNestObject[] nestedGeometry2 = null;
        private System.Collections.Generic.Dictionary<RhinoNestKernel.RhinoNestObject, Rhino.Geometry.Transform> objresult;
        private Rhino.Geometry.Transform[] objtrans = null;
        private RhinoNestKernel.RhinoNestNestingParameters Parameters;
        public RhinoNestKernel.RhinoNestNesting nesting;
        public bool m_solveme;
        /// <summary>
        ///     Initializes a new instance of the RhinoNest class.
        /// </summary>
        public RhinoNest()
            : base("Nesting", "Nesting",
                "Main component for nesting",
                "RhinoNest", "Nesting")
        {
        }
        public override void CreateAttributes()
        {
            m_attributes = new RhinoNestComponent_Attributes(this);
        }
        /// <summary>
        ///     Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Resources.IconRhinoNestNesting;
            }
        }

        /// <summary>
        ///     Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{5e2de6fa-e4ff-4edd-b45f-b47962792c38}"); }
        }

        /// <summary>
        ///     Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {

            pManager.AddGenericParameter("RhinoNest Object", "O", "RhinoNest Object", GH_ParamAccess.list);
            pManager.AddGenericParameter("RhinoNest Sheet", "S", "RhinoNest Sheet", GH_ParamAccess.item);
            pManager.AddGenericParameter("RhinoNest Nesting Parameters", "P", "RhinoNest Nesting Parameters", GH_ParamAccess.item);
        }

        /// <summary>
        ///     Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

            // Nested Objects
            // Non-used Objects
            // Report
        }
        

        /// <summary>
        ///     This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!m_solveme) 
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Double click me to start solving");
                m_solveme = false;
                return ;
            }
            m_solveme = false;
            // Agafar les dades
            sheets = new RhinoNestKernel.RhinoNestSheet();
            Parameters = new RhinoNestKernel.RhinoNestNestingParameters();
            Object = new System.Collections.Generic.List<RhinoNestKernel.RhinoNestObject>();
            DA.GetDataList(0, Object);//(0, ref Object);
            DA.GetData(1, ref sheets);
            DA.GetData(2, ref Parameters);

            
            nesting = new RhinoNestKernel.RhinoNestNesting(Object,sheets,Parameters);
            nesting.Sheet.MultiSheet = true;
            /*for (int i = 0; i < Object.Count; i++)
            {
                Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(Object[i].ExternalCurve);
            }*/

            nesting.OnNestingFinish += nesting_OnNestingFinish;
            nesting.StartNesting();
          
        }

        void nesting_OnNestingFinish(object sender, EventArgs e)
        {
            if (nesting.NestingResult.NestedObjects != null)
            {
                //objresult.GetEnumerator;
                objresult = nesting.NestingResult.NestedObjects;
                Array.Resize(ref nestedGeometry2, objresult.Count);
                Array.Resize(ref objtrans, objresult.Count);
                objresult.Keys.CopyTo(nestedGeometry2, 0);
                objresult.Values.CopyTo(objtrans, 0);

                for (int i = 0; i < objresult.Count; i++)
                {
                    var curve = nestedGeometry2[i].ExternalCurve.DuplicateCurve();
                    curve.Transform(objtrans[i]);
                    Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(curve);
                    //Rhino.RhinoDoc.ActiveDoc.Objects.Delete(nestedGeometry2[i].SubObjectsIds, true);
                }
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }
            if (nesting.NestingResult.RemainingObjects != null)
            {
                ObjectNON.AddRange(nesting.NestingResult.RemainingObjects);
                for (int a = 0; a < objresult.Count; a++)
                {
                    ObjectNON.Remove(nestedGeometry2[a]);
                }
                for (int i = 0; i < ObjectNON.Count; i++)
                { 
                    //nestedGeometry2[i].SubObjectsIds
                    //unnestedGeometry.Add(ObjectNON[i].ExternalCurve);
                    Rhino.RhinoDoc.ActiveDoc.Objects.AddCurve(ObjectNON[i].ExternalCurve);
                }
                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }            
        }
    }
}