using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Rhino;
using Rhino.Geometry;
using RhinoNestForGrasshopper.Properties;
using RhinoNestKernel.Nesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RhinoNestForGrasshopper.Nesting
{
    /// <summary>
    /// Modification of component attributes for get a event when double click on it.
    /// </summary>
    public class RhinoNestComponentAttributes : GH_ComponentAttributes
    {
        private const int SPINNER_SIZE = 32;

        private const int SPINNER_THICKNESS = 5;

        //Delcaration var for the spiner
        private float _spinnerAngle;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comp"> RhinoNest: Class of the owner.</param>
        public RhinoNestComponentAttributes(RhinoNest comp)
            : base(comp)
        {
        }

        /// <summary>
        /// Set for angleProcess int
        /// </summary>
        public float AngleProcess
        {
            set { _spinnerAngle = value; }
        }

        /// <summary>
        /// Get and set for EnableSpinner
        /// </summary>
        public bool EnableSpinner { get; set; }

        /// <summary>
        ///  Event of Double Click for the left button
        /// </summary>
        /// <param name="sender"> GH_Canvas: is the control handles all mouse and paint event for a single loaded document.</param>
        /// <param name="e"> GH_CanvasMouseEvent: Class used in UI events.</param>
        /// <returns></returns>
        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    if (((RhinoNest)Owner).IsWorking == false)
                    {
                        ((RhinoNest)Owner)._mSolveme = true;
                        Owner.ExpireSolution(true);
                    }
                }
                finally
                {
                    ((RhinoNest)Owner)._mSolveme = false;
                }
            }
            return base.RespondToMouseDoubleClick(sender, e);
        }

        /// <summary>
        /// Override of the render for create the spiner over the component
        /// </summary>
        /// <param name="iCanvas"> GH_Canvas: is the control handles all mouse and paint event for a single loaded document.</param>
        /// <param name="graphics"> Graphics: Encapsulates a GDI+ drawing surface. This class cannot be inherited.</param>
        /// <param name="iChannel"> GH_CanvasChannel: Enumerates all the drawing channel that are handled inside the Grasshopper canvas.</param>
        protected override void Render(GH_Canvas iCanvas, Graphics graphics, GH_CanvasChannel iChannel)
        {
            if ((iChannel == GH_CanvasChannel.Objects))
            {
                var pointer = new PointF(Bounds.Location.X, Bounds.Location.Y);

                //render the sing of TDM Solutions
                if ((iCanvas.Viewport.IsVisible(ref pointer, 5.0f)))
                {
                    var pt = new PointF(0.5f * (Bounds.Left + Bounds.Right), Bounds.Top);
                    GH_GraphicsUtil.RenderBalloonTag(iCanvas.Graphics, "TDM solutions", pt,
                        iCanvas.Viewport.VisibleRegion);
                }

                //if it's working render the spiner
                if (((RhinoNest)Owner).IsWorking)
                {
                    var ptSpinner = new PointF(0.5f * (Bounds.Left + Bounds.Right), Bounds.Top);
                    graphics.DrawArc(new Pen(Color.SteelBlue, SPINNER_THICKNESS), ptSpinner.X - (SPINNER_SIZE / 2), ptSpinner.Y - (Bounds.Height / 2) - SPINNER_SIZE, SPINNER_SIZE, SPINNER_SIZE, 0, _spinnerAngle);
                    if (_spinnerAngle > 360) _spinnerAngle -= 360;
                    _spinnerAngle += 10.0f;
                }
            }
            base.Render(iCanvas, graphics, iChannel);
        }
    }

    /// <summary>
    /// Class of Component RhinoNest
    /// </summary>
    public class RhinoNest : GH_Component
    {
        #region definitions

        public bool _mSolveme;
        private readonly List<List<RhinoNestObject>> _buffOut = new List<List<RhinoNestObject>>();
        private readonly List<RhinoNestObject> _nonest = new List<RhinoNestObject>();
        private readonly List<Guid> _mycurves = new List<Guid>();
        private RhinoNestNesting _nesting;

        private RhinoNestKernel.Nesting.RhinoNestNestingParameters _parameters;
        private bool _setOutput;
        private RhinoNestSheet _sheets2;
        private int _tryies;
        private Int32 _buffold;
        private List<RhinoNestSheetResult> _sheetsresults;

        #endregion definitions

        #region DeclarationObjectItems

        /// <summary>
        /// Initializes a new instance of the RhinoNest class.
        /// </summary>
        public RhinoNest()
            : base("Nesting", "Nesting",
                "Main component for nesting",
                "RhinoNest", "Nesting")
        {
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{5e2de6fa-e4ff-4edd-b45f-b47962792c38}"); }
        }

        /// <summary>
        /// Het and set for IsWorking boolean
        /// </summary>
        public bool IsWorking { get; set; }

        /// <summary>
        /// Provides an Icon for the component.
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
        /// Create the Attibute for double click
        /// </summary>
        public override void CreateAttributes()
        {
            m_attributes = new RhinoNestComponentAttributes(this);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        /// <param name="pManager">GH_InputParamManager: This class is used during Components constructions to add input parameters.</param>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("RhinoNest Object", "O", "RhinoNest Object", GH_ParamAccess.list);
            pManager.AddGenericParameter("RhinoNest Sheet", "S", "RhinoNest Sheet", GH_ParamAccess.item);
            pManager.AddGenericParameter("RhinoNest Nesting Parameters", "P", "RhinoNest Nesting Parameters",
                GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        /// <param name="pManager">GH_OutputParamManager: This class is used during Components constructions to add output parameters.</param>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("RhinoNest Object Nesteds", "ON", "RhinoNest Object Nesteds",
                GH_ParamAccess.list);
            pManager.AddGenericParameter("RhinoNest Object No Nesteds", "ONN", "RhinoNest Object No Nesteds",
               GH_ParamAccess.list);
            pManager.AddGenericParameter("Report", "R", "Report", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="da">IGH_DataAccess: The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            // _setOutput put var to the exit
            if (_setOutput)
            {
                for (int a = _buffOut.Count - 1; a > 0; a--)
                {
                    if (_buffOut[a].Count == 0)
                        _buffOut.RemoveAt(a);
                }

                da.SetDataList(0, _buffOut);
                da.SetDataList(1, _nonest);
                da.SetDataList(2, _sheetsresults);

                _setOutput = false;
                IsWorking = false;
                _mSolveme = false;
            }

            //if the user doesn't make a double click force out
            if (!_mSolveme)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Double click me to start solving");
                _mSolveme = false;
                return;
            }

            //if the user does the double ckcik start getting var
            if (!IsWorking)
            {
                //if have curves delete all
                if (_mycurves.Count > 0)
                {
                    foreach (Guid t in _mycurves)
                    {
                        RhinoDoc.ActiveDoc.Objects.Delete(t, true);
                    }
                    RhinoDoc.ActiveDoc.Views.Redraw();
                }
                _sheetsresults = new List<RhinoNestSheetResult>();
                //cleaning the var
                IsWorking = true;
                var sheets = new RhinoNestSheet();
                _parameters = new RhinoNestKernel.Nesting.RhinoNestNestingParameters();
                var _object = new List<RhinoNestObject>();
                _buffold = 0;
                _tryies = 0;
                _nonest.Clear();

                for (int i = 0; i < _buffOut.Count; i++)
                    _buffOut.RemoveAt(i);

                //getting var
                if (!da.GetDataList(0, _object)) return;
                if (!da.GetData(1, ref sheets)) return;
                if (!da.GetData(2, ref _parameters)) return;

                //saving thats
                _sheets2 = new RhinoNestSheet(sheets);

                //duplicate the list
                var send = new List<RhinoNestObject>();
                _object.ForEach(item => send.Add(new RhinoNestObject(item)));

                //declaring nesting
                _nesting = new RhinoNestNesting(send, sheets, _parameters);

                //active the Flag for the event for finish and other for progress
                _nesting.OnNestingFinish += nesting_OnNestingFinish;
                _nesting.OnNestingProgressChange += Nesting_OnNestingProgressChange;

                //start nesting
                _nesting.StartNesting();
            }
        }

        /// <summary>
        /// This is the method that send again the solveInstance.
        /// </summary>
        /// <param name="doc">GH_Document: Represent a single Grasshopper document.</param>
        private void MyCallback(GH_Document doc)
        {
            ExpireSolution(false);
        }

        #endregion DeclarationObjectItems

        #region Events

        /// <summary>
        /// Event for the procces of spiner
        /// </summary>
        /// <param name="sender"> object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e"> RhinoNestEventArgs: Class used in events.</param>
        private void Nesting_OnNestingProgressChange(object sender, RhinoNestEventArgs e)
        {
            Grasshopper.Instances.ActiveCanvas.ScheduleRegen(2);
        }

        /// <summary>
        ///  Event of Nesting Finish, that make the process and paint the nested objects
        /// </summary>
        /// <param name="sender"> object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e"> RhinoNestEventArgs: Class used in events.</param>
        private void nesting_OnNestingFinish(object sender, EventArgs e)
        {
            //For the nested objects
            if (_nesting.NestingResult.NestedObjects != null)
            {
                var listguid = new List<Guid>();
                //creation and cleans for the var
                var objresult = _nesting.NestingResult.NestedObjects;
                var nestedGeometry2 = new RhinoNestObject[objresult.Count];
                var objtrans = new Transform[objresult.Count];

                //get the objects
                objresult.Keys.CopyTo(nestedGeometry2, 0);
                objresult.Values.CopyTo(objtrans, 0);

                var curves = new List<Curve>();
                //do the modify to every object (position and rotation) and add to the list of curves
                for (int i = 0; i < objresult.Count; i++)
                {
                    curves.Add(nestedGeometry2[i].ExternalCurve.DuplicateCurve());

                    foreach (var id in nestedGeometry2[i].SubObjectsIds)
                    {
                        listguid.Add(RhinoDoc.ActiveDoc.Objects.Transform(id, objtrans[i], false));
                    }

                    curves[i].Transform(objtrans[i]);

                    listguid.Add(RhinoDoc.ActiveDoc.Objects.AddCurve(curves[i]));
                }
                _mycurves.AddRange(listguid);
                //add every object to a buffer for put on the output
                _buffOut.Add(new List<RhinoNestObject>());
                for (int i = 0; i < objresult.Count; i++)
                {
                    _buffOut[_tryies].Add(new RhinoNestObject(curves[i]));
                }

                //create var for make the report
                var rnProject = new RhinoNestProject(_nesting.NestingResult, _sheets2);
                var objs = new List<Tuple<RhinoNestObject, Transform, List<Guid>>>();
                for (int i = 0; i < _nesting.NestingResult.NestedObjects.Count; i++)
                {
                    var guids = new List<Guid> { listguid[i] };
                    var tup = new Tuple<RhinoNestObject, Transform, List<Guid>>(nestedGeometry2[i], objtrans[i], guids);
                    objs.Add(tup);
                }

                _sheetsresults.Add(new RhinoNestSheetResult(rnProject, objs));

                //get the bound for print if it isn't the first sheet and print it  if is the first don't print the sheet
                var print = _sheets2.GetBounds();
                _mycurves.Add(RhinoDoc.ActiveDoc.Objects.AddPolyline(print));
                //add trie for the next time
                _tryies++;

                //redraw all
                RhinoDoc.ActiveDoc.Views.Redraw();
            }

            //if there is more object for nest
            if (_nesting.NestingResult.RemainingObjects.Any())
            {
                //get the remaining object and count how many is for nest
                var send = _nesting.NestingResult.RemainingObjects;
                Int32 count = 0;
                foreach (var item in send)
                {
                    count = count + item.Parameters.RemainingCopies;
                }

                //if have ramaining object to nest
                if (count > 0 && _buffold != count)
                {
                    _nonest.Clear();

                    _buffold = count;
                    _nonest.AddRange(send);

                    //move the sheet it's being used
                    _sheets2.NextPosition();

                    //renew the nesting
                    _nesting = new RhinoNestNesting(send, _sheets2, _parameters);

                    //delete the actual events
                    _nesting.OnNestingFinish -= nesting_OnNestingFinish;
                    _nesting.OnNestingProgressChange -= Nesting_OnNestingProgressChange;

                    //create the new events
                    _nesting.OnNestingFinish += nesting_OnNestingFinish;
                    _nesting.OnNestingProgressChange += Nesting_OnNestingProgressChange;

                    //start nasting again
                    _nesting.StartNesting();
                }
                else
                {
                    // if it's not item to nest then call to shedulesolution for the expire solution
                    _setOutput = true;
                    GH_Document doc = OnPingDocument();
                    if (doc != null)
                    {
                        doc.ScheduleSolution(1, MyCallback);
                    }
                }
            }
        }

        #endregion Events
    }
}