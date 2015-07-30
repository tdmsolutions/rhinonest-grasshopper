using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;
using Rhino.Geometry;



namespace RhinoNestForGrasshopper.Nesting
{
    class RhinoNestClass:Grasshopper.Kernel.IGH_Param
    {
        public Rhino.Geometry.GeometryBase Geometry{ get; set; }
        public int Copies {get;set;}
        public int Rotation { get; set; }
        public int Priority { get; set; }
        public GH_ParamAccess Access
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public void AddSource(IGH_Param source, int index)
        {
            throw new NotImplementedException();
        }

        public void AddSource(IGH_Param source)
        {
            throw new NotImplementedException();
        }

        public bool AddVolatileData(Grasshopper.Kernel.Data.GH_Path path, int index, object data)
        {
            throw new NotImplementedException();
        }

        public bool AddVolatileDataList(Grasshopper.Kernel.Data.GH_Path path, System.Collections.IEnumerable list)
        {
            throw new NotImplementedException();
        }

        public bool AddVolatileDataTree(Grasshopper.Kernel.Data.IGH_Structure tree)
        {
            throw new NotImplementedException();
        }

        public void ClearProxySources()
        {
            throw new NotImplementedException();
        }

        public void CreateProxySources()
        {
            throw new NotImplementedException();
        }

        public GH_DataMapping DataMapping
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public GH_ParamData DataType
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasProxySources
        {
            get { throw new NotImplementedException(); }
        }

        public GH_ParamKind Kind
        {
            get { throw new NotImplementedException(); }
        }

        public bool Optional
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int ProxySourceCount
        {
            get { throw new NotImplementedException(); }
        }

        public IList<IGH_Param> Recipients
        {
            get { throw new NotImplementedException(); }
        }

        public bool RelinkProxySources(GH_Document document)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllSources()
        {
            throw new NotImplementedException();
        }

        public void RemoveEffects()
        {
            throw new NotImplementedException();
        }

        public void RemoveSource(Guid source_id)
        {
            throw new NotImplementedException();
        }

        public void RemoveSource(IGH_Param source)
        {
            throw new NotImplementedException();
        }

        public void ReplaceSource(Guid old_source_id, IGH_Param new_source)
        {
            throw new NotImplementedException();
        }

        public void ReplaceSource(IGH_Param old_source, IGH_Param new_source)
        {
            throw new NotImplementedException();
        }

        public bool Reverse
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Simplify
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int SourceCount
        {
            get { throw new NotImplementedException(); }
        }

        public IList<IGH_Param> Sources
        {
            get { throw new NotImplementedException(); }
        }

        public GH_StateTagList StateTags
        {
            get { throw new NotImplementedException(); }
        }

        public Type Type
        {
            get { throw new NotImplementedException(); }
        }

        public string TypeName
        {
            get { throw new NotImplementedException(); }
        }

        public Grasshopper.Kernel.Data.IGH_Structure VolatileData
        {
            get { throw new NotImplementedException(); }
        }

        public int VolatileDataCount
        {
            get { throw new NotImplementedException(); }
        }

        public GH_ParamWireDisplay WireDisplay
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void AddRuntimeMessage(GH_RuntimeMessageLevel Type, string Message)
        {
            throw new NotImplementedException();
        }

        public void ClearData()
        {
            throw new NotImplementedException();
        }

        public void ClearRuntimeMessages()
        {
            throw new NotImplementedException();
        }

        public void CollectData()
        {
            throw new NotImplementedException();
        }

        public void ComputeData()
        {
            throw new NotImplementedException();
        }

        public bool DependsOn(IGH_ActiveObject PotentialSource)
        {
            throw new NotImplementedException();
        }

        public bool IsDataProvider
        {
            get { throw new NotImplementedException(); }
        }

        public bool Locked
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool MutableNickName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public GH_SolutionPhase Phase
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public TimeSpan ProcessorTime
        {
            get { throw new NotImplementedException(); }
        }

        public void RegisterRemoteIDs(GH_GuidTable id_list)
        {
            throw new NotImplementedException();
        }

        public GH_RuntimeMessageLevel RuntimeMessageLevel
        {
            get { throw new NotImplementedException(); }
        }

        public IList<string> RuntimeMessages(GH_RuntimeMessageLevel level)
        {
            throw new NotImplementedException();
        }

        public bool SDKCompliancy(int exeVersion, int exeServiceRelease)
        {
            throw new NotImplementedException();
        }

        public void AddedToDocument(GH_Document document)
        {
            throw new NotImplementedException();
        }

        public bool AppendMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            throw new NotImplementedException();
        }

        public IGH_Attributes Attributes
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event IGH_DocumentObject.AttributesChangedEventHandler AttributesChanged;

        public Guid ComponentGuid
        {
            get { throw new NotImplementedException(); }
        }

        public void CreateAttributes()
        {
            throw new NotImplementedException();
        }

        public event IGH_DocumentObject.DisplayExpiredEventHandler DisplayExpired;

        public void DocumentContextChanged(GH_Document document, GH_DocumentContext context)
        {
            throw new NotImplementedException();
        }

        public void ExpirePreview(bool redraw)
        {
            throw new NotImplementedException();
        }

        public void ExpireSolution(bool recompute)
        {
            throw new NotImplementedException();
        }

        public GH_Exposure Exposure
        {
            get { throw new NotImplementedException(); }
        }

        public GH_IconDisplayMode IconDisplayMode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public System.Drawing.Bitmap Icon_24x24
        {
            get { throw new NotImplementedException(); }
        }

        public System.Drawing.Bitmap Icon_24x24_Locked
        {
            get { throw new NotImplementedException(); }
        }

        public void IsolateObject()
        {
            throw new NotImplementedException();
        }

        public void MovedBetweenDocuments(GH_Document oldDocument, GH_Document newDocument)
        {
            throw new NotImplementedException();
        }

        public event IGH_DocumentObject.ObjectChangedEventHandler ObjectChanged;

        public bool Obsolete
        {
            get { throw new NotImplementedException(); }
        }

        public void OnAttributesChanged()
        {
            throw new NotImplementedException();
        }

        public void OnDisplayExpired(bool redraw)
        {
            throw new NotImplementedException();
        }

        public void OnObjectChanged(GH_ObjectChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnObjectChanged(string customEvent, object tag)
        {
            throw new NotImplementedException();
        }

        public void OnObjectChanged(GH_ObjectEventType eventType, object tag)
        {
            throw new NotImplementedException();
        }

        public void OnObjectChanged(string customEvent)
        {
            throw new NotImplementedException();
        }

        public void OnObjectChanged(GH_ObjectEventType eventType)
        {
            throw new NotImplementedException();
        }

        public GH_Document OnPingDocument()
        {
            throw new NotImplementedException();
        }

        public void OnPreviewExpired(bool redraw)
        {
            throw new NotImplementedException();
        }

        public void OnSolutionExpired(bool recompute)
        {
            throw new NotImplementedException();
        }

        public event IGH_DocumentObject.PingDocumentEventHandler PingDocument;

        public event IGH_DocumentObject.PreviewExpiredEventHandler PreviewExpired;

        public void RecordUndoEvent(Grasshopper.Kernel.Undo.GH_UndoRecord record)
        {
            throw new NotImplementedException();
        }

        public Guid RecordUndoEvent(string name, Grasshopper.Kernel.Undo.IGH_UndoAction action)
        {
            throw new NotImplementedException();
        }

        public Guid RecordUndoEvent(string name)
        {
            throw new NotImplementedException();
        }

        public void RemovedFromDocument(GH_Document document)
        {
            throw new NotImplementedException();
        }

        public event IGH_DocumentObject.SolutionExpiredEventHandler SolutionExpired;

        public void TriggerAutoSave(GH_AutoSaveTrigger trigger, Guid id)
        {
            throw new NotImplementedException();
        }

        public void TriggerAutoSave(Guid id)
        {
            throw new NotImplementedException();
        }

        public void TriggerAutoSave(GH_AutoSaveTrigger trigger)
        {
            throw new NotImplementedException();
        }

        public void TriggerAutoSave()
        {
            throw new NotImplementedException();
        }

        public string Category
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool HasCategory
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasSubCategory
        {
            get { throw new NotImplementedException(); }
        }

        public string InstanceDescription
        {
            get { throw new NotImplementedException(); }
        }

        public Guid InstanceGuid
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Keywords
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void NewInstanceGuid(Guid UUID)
        {
            throw new NotImplementedException();
        }

        public void NewInstanceGuid()
        {
            throw new NotImplementedException();
        }

        public string NickName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SubCategory
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            throw new NotImplementedException();
        }

        public bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
