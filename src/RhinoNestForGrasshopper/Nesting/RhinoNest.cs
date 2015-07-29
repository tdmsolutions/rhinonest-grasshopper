using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Rhino;
using Rhino.Geometry;
using RhinoNestForGrasshopper.Properties;
using RhinoNestKernel;

namespace RhinoNestForGrasshopper.Nesting
{
    /// <summary>
    /// Modification of component attributes for get a event when double click on it.
    /// </summary>
    public class RhinoNestComponentAttributes : GH_ComponentAttributes
    {
        //Delcaration var for the spiner
        private float _spinnerAngle;
        private const int SpinnerSize = 32;
        private const int SpinnerThickness = 5;
        private bool _enableSpinner;

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
        public bool EnableSpinner
        {
            get { return _enableSpinner; }
            set { _enableSpinner = value; }
        }

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
                        ((RhinoNest) Owner).MSolveme = true;
                        Owner.ExpireSolution(true);
                    }
                }
                finally
                {
                    ((RhinoNest) Owner).MSolveme = false;
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
                    var pt = new PointF(0.5f*(Bounds.Left + Bounds.Right), Bounds.Top);
                    GH_GraphicsUtil.RenderBalloonTag(iCanvas.Graphics, "TDM solutions", pt,
                        iCanvas.Viewport.VisibleRegion);
                }

                //if it's working render the spiner 
                if (((RhinoNest) Owner).IsWorking)
                {
                    var ptSpinner = new PointF(0.5f * (Bounds.Left + Bounds.Right), Bounds.Top);
                    graphics.DrawArc(new Pen(Color.SteelBlue, SpinnerThickness), ptSpinner.X - (SpinnerSize / 2), ptSpinner.Y - (Bounds.Height / 2) - SpinnerSize, SpinnerSize, SpinnerSize, 0, _spinnerAngle);
                    _spinnerAngle += 10.0f;
                    if (_spinnerAngle > 360) _spinnerAngle -= 360;
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
        //declaration of var
        private readonly List<List<RhinoNestObject>> _buffOut = new List<List<RhinoNestObject>>();
        public RhinoNestNesting Nesting;
        private List<RhinoNestObject> _object;
        private RhinoNestKernel.RhinoNestNestingParameters _parameters;
        private List<RhinoNestObject> _send;
        public bool SetOutput = false;
        public bool MSolveme;
        private Curve[] _modifi;
        private RhinoNestObject[] _nestedGeometry2;
        private Dictionary<RhinoNestObject, Transform> _objresult;
        private Transform[] _objtrans;
        private RhinoNestSheet _sheets;
        private RhinoNestSheet _sheets2;
        private int _tryies;
        private bool _isWorking;
        private readonly List<Guid> _mycurves = new List<Guid>();

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
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{5e2de6fa-e4ff-4edd-b45f-b47962792c38}"); }
        }

        /// <summary>
        /// Het and set for IsWorking boolean
        /// </summary>
        public bool IsWorking
        {
            get { return _isWorking; }
            set { _isWorking = value; }
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
            pManager.AddGenericParameter("RhinoNest Object Nesteds", "O", "RhinoNest Object Nesteds",
                GH_ParamAccess.list);
            // Non-used Objects
            pManager.AddTextParameter("Report", "R", "Report", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that send again the solveInstance.
        /// </summary>
        /// <param name="doc">GH_Document: Represent a single Grasshopper document.</param>
        private void MyCallback(GH_Document doc)
        {
            ExpireSolution(false);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="da">IGH_DataAccess: The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            // _setOutput put var to the exit
            if (SetOutput)
            {
                da.SetDataList(0, _buffOut);
                da.SetData(1, "RhinoNest was Successfully");
                SetOutput = false;
                IsWorking = false;
                MSolveme = false;
            }

            //if the user doesn't make a double click force out
            if (!MSolveme)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Double click me to start solving");
                MSolveme = false;
                return;
            }

            //if the user does the double ckcik start getting var
            if (!IsWorking)
            {
                //if have curves delete all
                if (_mycurves.Count>0)
                {
                    foreach (Guid t in _mycurves)
                    {
                        RhinoDoc.ActiveDoc.Objects.Delete(t,true);
                    }
                    RhinoDoc.ActiveDoc.Views.Redraw();
                }
                //cleaning the var
                IsWorking = true;
                _sheets = new RhinoNestSheet();
                _parameters = new RhinoNestKernel.RhinoNestNestingParameters();
                _object = new List<RhinoNestObject>();

                //getting var
                if (!da.GetDataList(0, _object)) return;
                if (!da.GetData(1, ref _sheets)) return;
                if (!da.GetData(2, ref _parameters)) return;
                //saving thats
                _sheets2 = new RhinoNestSheet(_sheets);
                _send = new List<RhinoNestObject>(_object);
                //declaring nesting
                Nesting = new RhinoNestNesting(_send, _sheets, _parameters);
                //active the Flag for the event for finish and other for progress 
                Nesting.OnNestingFinish += nesting_OnNestingFinish;
                Nesting.OnNestingProgressChange += Nesting_OnNestingProgressChange;
                //start nesting
                Nesting.StartNesting();
            }
        }

        /// <summary>
        /// Event for the procces of spiner
        /// </summary>
        /// <param name="sender"> object: Support all classes in the .NET Framework class hierachy and provides low-level service to derived classes.</param>
        /// <param name="e"> RhinoNestEventArgs: Class used in events.</param>
        void Nesting_OnNestingProgressChange(object sender, RhinoNestEventArgs e)
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
            //For the nestet objects
            if (Nesting.NestingResult.NestedObjects != null)
            {
                //creation and cleans for the var
                _objresult = Nesting.NestingResult.NestedObjects;
                _nestedGeometry2 = null;
                _objtrans = null;
                _modifi = null;
                //resize for the array
                Array.Resize(ref _nestedGeometry2, _objresult.Count);
                Array.Resize(ref _objtrans, _objresult.Count);
                Array.Resize(ref _modifi, _objresult.Count);
                //get the objects
                _objresult.Keys.CopyTo(_nestedGeometry2, 0);
                _objresult.Values.CopyTo(_objtrans, 0);

                //do the modify to every object (position and rotation) and add to the list of curves
                for (int i = 0; i < _objresult.Count; i++)
                {
                    _modifi[i] = _nestedGeometry2[i].ExternalCurve.DuplicateCurve();
                    _modifi[i].Transform(_objtrans[i]);
                    _mycurves.Add(RhinoDoc.ActiveDoc.Objects.AddCurve(_modifi[i]));
                }
                //add every objject to a buffer for put ir on the output
                _buffOut.Add( new List<RhinoNestObject>());
                for (int i = 0; i < _objresult.Count; i++)
                {
                    _buffOut[_tryies].Add(new RhinoNestObject(_modifi[i]));
                }
                //redraw all
                RhinoDoc.ActiveDoc.Views.Redraw();
            }
            //if any object can't be nested
            if (Nesting.NestingResult.RemainingObjects.Any())
            {
                //first remove from the list the nested objects
                for (int a = 0; _nestedGeometry2.Length > a; a++)
                {
                    for (int i = 0; i < _send.Count; i++)
                    {
                        if (_send[i].Parameters.Id == _nestedGeometry2[a].Parameters.Id)
                        {
                            _send[i].Parameters.Copies = _send[i].Parameters.Copies - 1;
                            if (_send[i].Parameters.Copies <= 0)
                            {
                                _send.RemoveAt(i);
                            }
                            a++;
                            i = -1;
                            if (a >= _nestedGeometry2.Length)
                            {
                                i = _send.Count;
                            }
                        }
                    }
                }
                //if it can not nest all object then use again the nest with a new sheet at right of the last
                if (_send.Count > 0)
                {
                    _tryies++;
                    //move the sheet it's being used
                    _sheets2.Move(_sheets2.LenX(), 0);
                    //create a var for print the new sheet
                    var print = new Polyline(4)
                    {
                        new Point3d((_sheets2.LenX())*_tryies, 0, 0),
                        new Point3d((_sheets2.LenX())*_tryies + (_sheets2.LenX()), 0, 0),
                        new Point3d((_sheets2.LenX())*_tryies + (_sheets2.LenX()), _sheets2.LenY(), 0),
                        new Point3d((_sheets2.LenX())*_tryies, _sheets2.LenY(), 0)
                    };
                    //we add 1 try for the next nesting
                    
                    //print the object and add to the list of curves 
                    _mycurves.Add(RhinoDoc.ActiveDoc.Objects.AddPolyline(print));
                    RhinoDoc.ActiveDoc.Views.Redraw();
                    //send the new nest and wait the event of finish and progress for spiner
                    Nesting = new RhinoNestNesting(_send, _sheets2, _parameters);
                    Nesting.OnNestingFinish += nesting_OnNestingFinish;
                    Nesting.OnNestingProgressChange += Nesting_OnNestingProgressChange;
                    Nesting.StartNesting();
                }
                else
                {
                   // if it's not item to nest then call to shedulesolution for the expire solution
                    SetOutput = true;
                    GH_Document doc = OnPingDocument();
                    if (doc != null)
                    {
                        doc.ScheduleSolution(1, MyCallback);
                    }
                }
            }
        }
    }
}