using System;
using System.Drawing;
using Grasshopper.Kernel;
using RhinoNestForGrasshopper.Properties;

namespace RhinoNestForGrasshopper
{
    public class RhinoNestForGrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "RhinoNestForGrasshopper";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Resources.IconRhinoNestNesting;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "RhinoNest for Grasshopper. Please find the source code: https://github.com/tdmsolutions";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("14fdb4d2-ead5-46c9-bb5b-294765da6ff3");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "TDM Solutions SLU";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "info@tdmsolutions.com";
            }
        }
    }
}
