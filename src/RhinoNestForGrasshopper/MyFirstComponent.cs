using System;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace Nesting
{
  /// <summary>
  /// Enumerates the possible orientations.
  /// </summary>
  public enum Constraint
  {
    /// <summary>
    /// No rotation allowed.
    /// </summary>
    Fixed,
    /// <summary>
    /// Any rotation allowed.
    /// </summary>
    Free,
    /// <summary>
    /// Rotation by fixed angles allowed.
    /// </summary>
    Angle
  }

  /// <summary>
  /// Collates nesting orientation constraints. 
  /// This is the type one would actually use in code.
  /// </summary>
  public class Orientation
  {
    #region fields
    private readonly Constraint _constraint;
    private readonly int _angle; // Question, is it possible to allow more than one angle?
    #endregion

    #region constructor
    /// <summary>
    /// Create a new orientation.
    /// </summary>
    /// <param name="constraint">Constraint type.</param>
    /// <param name="angle">Angle.</param>
    private Orientation(Constraint constraint, int angle)
    {
      _constraint = constraint;
      _angle = angle;
    }

    // Static creation methods.
    /// <summary>
    /// Create a fixed orientation.
    /// </summary>
    public static Orientation CreateFixedOrientation()
    {
      return new Orientation(Constraint.Fixed, 0);
    }
    /// <summary>
    /// Create a free orientation.
    /// </summary>
    public static Orientation CreateFreeOrientation()
    {
      return new Orientation(Constraint.Free, 0);
    }
    /// <summary>
    /// Create an orientation for specific angles.
    /// </summary>
    /// <param name="angle">Angle. Negative angles indicate plus-or-minus.</param>
    public static Orientation CreateAngledOrientation(int angle)
    {
      return new Orientation(Constraint.Angle, angle);
    }
    #endregion

    #region properties
    /// <summary>
    /// Gets the constraint type.
    /// </summary>
    public Constraint Constraint
    {
      get { return _constraint; }
    }
    /// <summary>
    /// Gets the constraint angle. This value only makes sense if the Constraint type is Angle.
    /// </summary>
    public int Angle
    {
      get { return Math.Abs(_angle); }
    }
    /// <summary>
    /// Gets whether the angle constraint allows for both plus and minus rotation.
    /// </summary>
    public bool IsPlusOrMinus
    {
      get { return _angle < 0; }
    }
    #endregion
  }

  /// <summary>
  /// Goo wrapper for orientation. End-code typically doesn't concern itself with this.
  /// </summary>
  public class OrientationGoo : GH_Goo<Orientation>
  {
    #region constructors
    public OrientationGoo()
      : base(Orientation.CreateFixedOrientation())
    { }
    public OrientationGoo(Orientation orientation)
      : base(orientation)
    { }
    #endregion

    #region duplication
    public override IGH_Goo Duplicate()
    {
      // since Orientation is immutable, we can just share it as often as we want.
      return new OrientationGoo(Value);
    }
    #endregion

    #region properties
    public override bool IsValid
    {
      get
      {
        if (Value == null)
          return false;

        // TODO: test the angle for validity perhaps?
        return true;
      }
    }
    public override string IsValidWhyNot
    {
      get
      {
        if (Value == null)
          return "Orientation data is null.";

        // TODO: test the angle for validity perhaps?
        return string.Empty;
      }
    }

    public override string TypeName
    {
      get { return "Orientation"; }
    }
    public override string TypeDescription
    {
      get { return "Orientation settings for nesting"; }
    }
    #endregion

    #region conversions
    /// <summary>
    /// Create a human friendly description of this data.
    /// </summary>
    public override string ToString()
    {
      if (Value == null)
        return "Null orientation";

      if (Value.Constraint == Constraint.Fixed)
        return "Fixed orientation";

      if (Value.Constraint == Constraint.Free)
        return "Free orientation";

      string prefix = string.Empty;
      if (Value.IsPlusOrMinus)
        prefix = "±";

      return string.Format("{0}{1}°", prefix, Value.Angle);
    }

    /// <summary>
    /// Handle casting to Orientation and Integer.
    /// We're not doing doubles yet.
    /// </summary>
    /// <param name="target">Target value to assign.</param>
    /// <returns>Success or failure.</returns>
    public override bool CastTo<TQ>(ref TQ target)
    {
      if (Value == null)
        return false;

      // Cast to Orientation.
      if (typeof(Orientation).IsAssignableFrom(typeof(TQ)))
      {
        target = (TQ)((object)Value);
        return true;
      }

      // Cast to integer.
      if (typeof(int).IsAssignableFrom(typeof(TQ)))
      {
        int angle = Value.Angle;
        if (Value.IsPlusOrMinus)
          angle = -angle;

        target = (TQ)((object)angle);
        return true;
      }


      return false;
    }
    /// <summary>
    /// We will support casting from Orientation, Integer and String.
    /// </summary>
    /// <param name="source">Data to cast from.</param>
    /// <returns>Success or failure.</returns>
    public override bool CastFrom(object source)
    {
      if (source == null)
        return false;

      if (source is Orientation)
      {
        Value = (Orientation)source;
        return true;
      }

      if (source is int)
      {
        Value = Orientation.CreateAngledOrientation((int)source);
        return true;
      }

      if (source is string)
      {
        string text = (string)source;
        text = text.Replace("°", string.Empty);
        text = text.Trim();

        if (text.Equals("Fixed", StringComparison.OrdinalIgnoreCase))
        {
          Value = Orientation.CreateFixedOrientation();
          return true;
        }

        if (text.Equals("Free", StringComparison.OrdinalIgnoreCase))
        {
          Value = Orientation.CreateFixedOrientation();
          return true;
        }

        bool plusOrMinus = false;
        if (text.StartsWith("-") | text.StartsWith("±"))
        {
          plusOrMinus = true;
          text = text.Replace("-", string.Empty);
          text = text.Replace("±", string.Empty);
        }

        int angle;
        if (!int.TryParse(text, out angle))
          return false;

        if (plusOrMinus)
          angle = -angle;

        Value = Orientation.CreateAngledOrientation(angle);
        return true;
      }

      return false;
    }
    /// <summary>
    /// Return the Value, it is immutable so we don't have to worry about someone changing it.
    /// </summary>
    public override object ScriptVariable()
    {
      return Value;
    }
    #endregion

    #region (de)serialization
    public override bool Write(GH_IO.Serialization.GH_IWriter writer)
    {
      if (Value == null)
        return true;

      writer.SetInt32("Constraint", (int)Value.Constraint);
      if (Value.Constraint == Constraint.Angle)
      {
        if (Value.IsPlusOrMinus)
          writer.SetInt32("Angle", -Value.Angle);
        else
          writer.SetInt32("Angle", Value.Angle);
      }

      return true;
    }
    public override bool Read(GH_IO.Serialization.GH_IReader reader)
    {
      if (!reader.ItemExists("Constraint"))
      {
        Value = null;
        return true;
      }

      // TODO: this cast may fail, I'm not checking for correctness here.
      Constraint constraint = (Constraint)reader.GetInt32("Constraint");
      if (constraint == Constraint.Fixed)
        Value = Orientation.CreateFixedOrientation();
      else if (constraint == Constraint.Free)
        Value = Orientation.CreateFreeOrientation();
      else
      {
        int angle = reader.GetInt32("Angle");
        Value = Orientation.CreateAngledOrientation(angle);
      }

      return true;
    }
    #endregion
  }

  /// <summary>
  /// IGH_Param implementation for OrientationGoo.
  /// </summary>
  public class OrientationParameter : GH_PersistentParam<OrientationGoo>
  {
    public OrientationParameter()
      : base("Orientation", "Orient", "Orientation data for nesting", "RhinoNest", "Nesting")
    {

    }

    protected override GH_GetterResult Prompt_Plural(ref System.Collections.Generic.List<OrientationGoo> values)
    {
      return GH_GetterResult.cancel;
    }
    protected override GH_GetterResult Prompt_Singular(ref OrientationGoo value)
    {
      return GH_GetterResult.cancel;
    }

    /// <summary>
    /// This speeds up when someone calls DA.SetData(index, Orientation). 
    /// It is not necessary to override, but it does make it easier for Grasshopper.
    /// </summary>
    protected override OrientationGoo PreferredCast(object data)
    {
      Orientation t = data as Orientation;
      if (t != null)
        return new OrientationGoo(t);

      return null;
    }
    /// <summary>
    /// This makes it easier for Grasshopper to create new instances of the goo.
    /// It is not necessary to override this method, but it does give a small speed boost.
    /// </summary>
    protected override OrientationGoo InstantiateT()
    {
      return new OrientationGoo();
    }

    protected override Bitmap Icon
    {
      get
      {
        Bitmap icon = new Bitmap(24, 24, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        Graphics graphics = Graphics.FromImage(icon);
        graphics.Clear(Color.Transparent);
        graphics.FillEllipse(Brushes.HotPink, new Rectangle(2, 2, 20, 20));
        return icon;
      }
    }
    public override GH_Exposure Exposure
    {
      get { return GH_Exposure.primary; }
    }
    public override Guid ComponentGuid
    {
      get { return new Guid("{9C49F12F-A57E-4FB9-847E-C1E9FEFE2FE1}"); }
    }

    #region custom menu UI
    protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomSingleValueItem()
    {
      // TODO: figure out whether one of these is already assigned as persistent data and set the check to TRUE.

      System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem();
      item.Text = @"Set an Orientation";
      Menu_AppendItem(item.DropDown, "Fixed", Menu_FixedClicked);
      Menu_AppendItem(item.DropDown, "Free", Menu_FreeClicked);
      Menu_AppendSeparator(item.DropDown);
      Menu_AppendItem(item.DropDown, "15", Menu_15Clicked);
      Menu_AppendItem(item.DropDown, "30", Menu_30Clicked);
      Menu_AppendItem(item.DropDown, "45", Menu_45Clicked);
      Menu_AppendItem(item.DropDown, "60", Menu_60Clicked);
      Menu_AppendItem(item.DropDown, "90", Menu_90Clicked);
      Menu_AppendItem(item.DropDown, "180", Menu_180Clicked);
      Menu_AppendSeparator(item.DropDown);
      Menu_AppendItem(item.DropDown, "±45", Menu_pm45Clicked);
      Menu_AppendItem(item.DropDown, "±90", Menu_pm90Clicked);

      return item;
    }
    protected override System.Windows.Forms.ToolStripMenuItem Menu_CustomMultiValueItem()
    {
      return null;
    }

    private void SetNewOrientation(Orientation orientation)
    {
      try
      {
        RecordPersistentDataEvent("Set orientation");
        PersistentData.Clear();

        if (orientation != null)
          PersistentData.Append(new OrientationGoo(orientation), new GH_Path(0));
      }
      finally
      {
        ExpireSolution(true);
      }
    }
    private void Menu_FixedClicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateFixedOrientation());
    }
    private void Menu_FreeClicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateFreeOrientation());
    }
    private void Menu_15Clicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateAngledOrientation(15));
    }
    private void Menu_30Clicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateAngledOrientation(30));
    }
    private void Menu_45Clicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateAngledOrientation(45));
    }
    private void Menu_60Clicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateAngledOrientation(60));
    }
    private void Menu_90Clicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateAngledOrientation(90));
    }
    private void Menu_180Clicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateAngledOrientation(180));
    }
    private void Menu_pm45Clicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateAngledOrientation(-45));
    }
    private void Menu_pm90Clicked(object sender, EventArgs e)
    {
      SetNewOrientation(Orientation.CreateAngledOrientation(-90));
    }
    #endregion
  }

  public class OrientationComponent : GH_Component
  {

    public OrientationComponent()
      : base("Orientation Test", "OrTest", "Test orientation classes", "Nesting", "Nesting")
    {
    }

    protected override void RegisterInputParams(GH_InputParamManager pManager)
    {
      pManager.AddParameter(new OrientationParameter(), "Orientation 1", "O1", "First orientation", GH_ParamAccess.item);
      pManager.AddParameter(new OrientationParameter(), "Orientation 2", "O2", "Second orientation", GH_ParamAccess.item);
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager)
    {
      pManager.AddBooleanParameter("Identity", "I", "Identity test for both orientations", GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      Orientation o1 = null;
      Orientation o2 = null;
      if (!DA.GetData(0, ref o1)) return;
      if (!DA.GetData(1, ref o2)) return;

      if (o1 == null || o2 == null)
      {
        DA.SetData(0, false);
        return;
      }

      if (o1.Constraint != o2.Constraint)
      {
        DA.SetData(0, false);
        return;
      }

      if (o1.IsPlusOrMinus != o2.IsPlusOrMinus)
      {
        DA.SetData(0, false);
        return;
      }

      if (o1.Angle != o2.Angle)
      {
        DA.SetData(0, false);
        return;
      }

      // The orientations are identical.
      DA.SetData(0, true);
    }

    protected override Bitmap Icon
    {
      get
      {
        Bitmap icon = new Bitmap(24, 24, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        Graphics graphics = Graphics.FromImage(icon);
        graphics.Clear(Color.Transparent);
        graphics.FillEllipse(Brushes.Maroon, new Rectangle(2, 2, 20, 20));
        return icon;
      }
    }
    public override Guid ComponentGuid
    {
      get { return new Guid("{5103C3C0-E492-4694-89DA-A41DE60EE61C}"); }
    }
  }
}